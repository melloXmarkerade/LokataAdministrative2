using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class UserSubmittedReciept
    {
        private List<UserReceipt> Receipts { get; set; } = new();
        private UserReceipt Receipt { get; set; } = new();
        private bool RecPopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            Receipts  = await userReceipt.GetAllRequest(token);
        }

        private void ViewRequirement(UserReceipt req)
        {
            Receipt  = req;
            RecPopup = true;
        }

        private void ApproveRequirement(UserReceipt req)
        {

        }

        private void DeclineRequirement(UserReceipt req)
        {

        }

        private void ReqCloseDialog() => RecPopup = false;
    }
}
