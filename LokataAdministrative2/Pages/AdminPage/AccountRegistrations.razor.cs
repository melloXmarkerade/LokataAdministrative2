using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.AdminPage
{
    public partial class AccountRegistrations
    {
        private List<AdminSignup> Accounts { get; set; } = new();
        private AdminSignup? Account { get; set; } = new();
        private bool AccountPopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            Accounts = await adminClient.GetAllRequest(await tokenProvider.GetTokenAsync());
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
