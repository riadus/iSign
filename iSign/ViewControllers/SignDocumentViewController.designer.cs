// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iSign
{
    [Register ("SignDocumentViewController")]
    partial class SignDocumentViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        iSign.TouchableScrollView ContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EditBtn { get; set; }

        [Action ("EditBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void EditBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (EditBtn != null) {
                EditBtn.Dispose ();
                EditBtn = null;
            }
        }
    }
}