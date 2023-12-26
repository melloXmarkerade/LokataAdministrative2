using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class PendingAccounts
    {
        private List<AdminDto> Accounts { get; set; } = new();
        private List<AdminDto> FilteredAccounts { get; set; } = new();
        private AdminDto Account { get; set; } = new();
        private bool AccountPopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            Accounts = await adminClient.GetAllRequest(await tokenProvider.GetTokenAsync());
            FilteredAccounts = Accounts;
        }

        private void ViewAccount(AdminDto account)
        {
            Account = account;
            AccountPopup = true;
        }

        private void AccountCloseDialog()
        {
            Account = null;
            AccountPopup = false;
        }

        private async Task AprroveAccount()
        {
            Account.IsApproved = true;
            await adminClient.PutRequest(Account, await tokenProvider.GetTokenAsync());

            var success = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Approved Success",
                Icon = SweetAlertIcon.Success
            });

            if(success.IsConfirmed)
            {
                await OnInitializedAsync();
                AccountCloseDialog();
            }
        }

        void UpdateFilteredPending(string searchItem)
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
    }
}
