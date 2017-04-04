﻿using System;
using System.Linq;
using CoreGraphics;
using iSign.Core;
using iSign.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace iSign
{
    public class SigningView : MvxView
    {
        private CanvasView CanvasView { get; }
        private UIButton OkButton { get; }
        private UIButton CancelButton { get; }
        private UILabel HelpMessage { get; }
        private PaletteView PaletteView { get; }
        private MvxSubscriptionToken Token { get; set; }
        private IMvxMessenger Messenger { get; }
        public SigningView (CGRect bounds) : base (bounds)
        {
            Frame = bounds;
            CanvasView = new CanvasView (new CGRect(0, 0, 500, 500));
            CanvasView.OnLineAdded += CanvasView_OnLineAdded;
            BackgroundColor = UIColor.FromRGB (0, 153, 255).ColorWithAlpha(0.3f);
            OkButton = new UIButton ();
            OkButton.TouchUpInside += OkButton_TouchUpInside;

            CancelButton = new UIButton ();
            CancelButton.TouchUpInside += CancelButton_TouchUpInside;

            PaletteView = new PaletteView ();

            this.DelayBind (() => {
                var set = this.CreateBindingSet<SigningView, SigningViewModel> ();
                set.Bind (OkButton)
                   .For ("Title")
                   .To (vm => vm.AddSignatureTxt);

                set.Bind (CancelButton)
                   .For ("Title")
                   .To (vm => vm.CancelTxt);

                set.Bind (PaletteView)
                   .For (v => v.DataContext)
                   .To (vm => vm.PaletteContext);

                set.Bind (CanvasView)
                   .For (v => v.DrawColor)
                   .To (vm => vm.DrawingColor)
                   .WithConversion (new UIColorConverter ());

                set.Bind (CanvasView)
                   .For (v => v.HandLineWidth)
                   .To (vm => vm.Thickness);

                set.Bind (CanvasView)
                   .For (v => v.ForceSensitivity)
                   .To (vm => vm.Thickness);

                set.Apply ();
            });

            Messenger = Mvx.Resolve<IMvxMessenger> ();
        }

        private void Undo ()
        {
            CanvasView.Undo ();
            PaletteView.UpdateUndo (CanvasView.CanUndo);
        }

        public override void MovedToSuperview ()
        {
            base.MovedToSuperview ();
            if (Superview == null) return;
            Token = Messenger.Subscribe<UndoMessage> (m => Undo ());
            ShowViewWithAnimation ();
            ShowPalette ();
        }

        void ShowViewWithAnimation ()
        {
            var topLeft = this.GetCenter (CanvasView.Frame);
            var topRight = new CGPoint (topLeft.X + CanvasView.Frame.Width, topLeft.Y);
            CanvasView.Frame = new CGRect (topLeft, CanvasView.Frame.Size);
            CanvasView.Layer.BorderWidth = 5;
            CanvasView.Layer.CornerRadius = 10;
            CanvasView.Transform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity (), 0.001f, 0.001f);
            Add (CanvasView);

            AnimateAsync (0.2, () => {
                CanvasView.Transform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity (), 1, 1);
            });

            CancelButton.SizeToFit ();
            CancelButton.Frame = new CGRect (new CGPoint (topLeft.X + 10, topLeft.Y - 40), CancelButton.Frame.Size);
            Add (CancelButton);
            Add (OkButton);
            OkButton.SizeToFit ();
            OkButton.Frame = new CGRect (new CGPoint (topRight.X - OkButton.Frame.Width - 10, topRight.Y - 40), OkButton.Frame.Size);
        }

        private SigningViewModel Context => DataContext as SigningViewModel;

        private void ShowPalette ()
        {
            PaletteView.Frame = new CGRect (Frame.X, Frame.Height, Frame.Width, 50);
            PaletteView.Layout ();
            Animate (0.5, 0.2, UIViewAnimationOptions.CurveLinear, () =>
                     Superview.Add (PaletteView), null);
        }

        public UIImage GetSignature ()
        {
            return CanvasView?.DrawingImage;
        }

        public void StartWith (UIImage image)
        {
            CanvasView?.StartWith (image);
        }

        public Action OkAction { get; set;}
        public Action CancelAction { get; set;}
        private void CloseView ()
        {
            CanvasView.OnLineAdded -= CanvasView_OnLineAdded;
            Messenger.Unsubscribe<UndoMessage> (Token);
            PaletteView.RemoveFromSuperview ();
            RemoveFromSuperview ();
        }

        void OkButton_TouchUpInside (object sender, EventArgs e)
        {
            if (OkAction != null) {
                OkAction ();
            }
            CloseView ();
        }

        void CancelButton_TouchUpInside (object sender, EventArgs e)
        {
            if (CancelAction != null) {
                CancelAction ();
            }
            CloseView ();
        }

        void CanvasView_OnLineAdded (object sender, EventArgs e)
        {
            PaletteView.UpdateUndo (CanvasView.CanUndo);
        }
    }
}