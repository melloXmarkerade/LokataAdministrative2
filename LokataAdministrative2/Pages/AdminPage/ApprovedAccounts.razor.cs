﻿using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.AdminPage
{
    public partial class ApprovedAccounts
    {
        private List<AdminSignup> Accounts { get; set; } = new();
        private List<AdminSignup> FilteredAccounts { get; set; } = new();
        private AdminSignup Account { get; set; } = new();
        private bool AccountPopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            Accounts = await adminClient.GetRegisteredAccounts(await tokenProvider.GetTokenAsync());
            FilteredAccounts = Accounts!;
        }

        void UpdateFilteredApproved(string searchItem)
        {
            if (string.IsNullOrEmpty(searchItem))
                FilteredAccounts = Accounts!;
            else
            {
                FilteredAccounts = Accounts!.Where(account => account.FirstName!.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                                              account.LastName!.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                                              account.GovernmentId!.Contains(searchItem, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private void ViewAccount(AdminSignup account)
        {
            Account = account;
            AccountPopup = true;
        }

        private void AccountCloseDialog()
        {
            Account = null;
            AccountPopup = false;
        }
    }
}
