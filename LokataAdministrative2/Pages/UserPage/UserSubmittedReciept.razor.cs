using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class UserSubmittedReciept
    {
        private List<UserReceipt> Receipts { get; set; } = new();
        private UserReceipt Receipt { get; set; } = new();
        private bool RecPopup { get; set; } = false;

        HashSet<Requirement> approvedRequirements = new HashSet<Requirement>();
        HashSet<Requirement> declinedRequirements = new HashSet<Requirement>();

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            Receipts  = await userReceipt.GetAllRequest(token);
        }

        private void ViewRequirement(UserReceipt receipt)
        {
            Receipt = receipt;
            RecPopup = true;
        }

        void ApproveRequirement(Requirement receipt)
        {
            approvedRequirements.Add(receipt);
            receipt.IsApproved = true;
        }

        void DeclineRequirement(Requirement receipt)
        {
            declinedRequirements.Add(receipt);
            receipt.IsDeclined = true;
        }

        bool IsApproved(Requirement receipt)
        {
            return approvedRequirements.Contains(receipt);
        }

        bool IsDeclined(Requirement receipt)
        {
            return declinedRequirements.Contains(receipt);
        }

        private string GetStatus(Requirement req)
        {
            if (req.IsApproved)
                return "Approved";
            else if (req.IsDeclined)
                return "Declined";
            else
                return "Pending";
        }

        private void ReqCloseDialog() => RecPopup = false;
    }
}
