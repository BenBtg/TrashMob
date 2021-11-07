﻿namespace TrashMobMobile.ViewModels
{
    using System;
    using TrashMobMobile.Services;
    using Xamarin.Forms;

    public class TermsAndConditionsViewModel : BaseViewModel
    {
        private bool isTermsOfUseAgreed;
        private bool isPrivacyPolicyAgreed;
        private readonly IUserManager userManager;

        public TermsAndConditionsViewModel(IUserManager userManager)
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            this.userManager = userManager;
        }

        private bool ValidateSave()
        {
            return isTermsOfUseAgreed && isPrivacyPolicyAgreed;
        }

        public bool IsTermsOfUseAgreed
        {
            get => isTermsOfUseAgreed;
            set => SetProperty(ref isTermsOfUseAgreed, value);
        }

        public bool IsPrivacyPolicyAgreed
        {
            get => isPrivacyPolicyAgreed;
            set => SetProperty(ref isPrivacyPolicyAgreed, value);
        }

        public Command SaveCommand { get; }

        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var user = App.CurrentUser;

            user.TermsOfServiceVersion = App.CurrentTermsOfServiceVersion;
            user.PrivacyPolicyVersion = App.CurrentPrivacyPolicyVersion;
            user.DateAgreedToPrivacyPolicy = DateTimeOffset.UtcNow;
            user.DateAgreedToTermsOfService = DateTimeOffset.UtcNow;

            await userManager.UpdateUserAsync(user);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
