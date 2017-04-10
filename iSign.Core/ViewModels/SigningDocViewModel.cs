﻿using System;
using iSign.Core.Services;
using MvvmCross.Core.ViewModels;

namespace iSign.Core.ViewModels
{
    public class SigningDocViewModel : BaseViewModel
    {
        private IDialogService DialogService { get; }
        public SigningViewModel SigningViewModel { get; }
        public string Filename { get; set;}

        public SigningDocViewModel (IViewModelServices viewModelServices, IDialogService dialogService, SigningViewModel signingViewModel) : base(viewModelServices)
        {
            AddLabelCommand = new MvxCommand (AddLabel);
            DialogService = dialogService;
            SigningViewModel = signingViewModel;
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
