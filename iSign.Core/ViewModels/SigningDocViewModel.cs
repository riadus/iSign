﻿using System;
using MvvmCross.Core.ViewModels;

namespace iSign.Core
{
    public class SigningDocViewModel : BaseViewModel
    {
        private IDialogService DialogService { get; }
        private IFileStorageChooser FileStorageChooser { get; }
        public string Filename { get; set;}
        public PaletteViewModel PaletteContext { get; }
        public SigningDocViewModel (INavigationService navigationService, IDialogService dialogService, PaletteViewModel paletteContext) : base(navigationService)
        {
            PaletteContext = paletteContext;
            AddLabelCommand = new MvxCommand (AddLabel);
            DialogService = dialogService;
        }

        public IMvxCommand AddLabelCommand { get; }
        private void AddLabel ()
        {
            DialogService.ShowDialog (OnInputSet);
        }

        public event EventHandler<string> InputSet;
        private void OnInputSet (string input)
        {
            InputSet?.Invoke (this, input);
        }

        public void LoadFile (IFileStorage tool)
        {
            tool.DownloadFile (OnFileDownloaded);
        }

        public event EventHandler<string> FileDownloaded;
        private void OnFileDownloaded (string filename)
        {
            Filename = filename;
            FileDownloaded?.Invoke (this, filename);
        }
    }
}
