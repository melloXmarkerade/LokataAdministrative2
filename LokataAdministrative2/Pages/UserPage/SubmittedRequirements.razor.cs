using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class SubmittedRequirements
    {
        private List<UserRequirement> Requirements { get; set; } = new();
        private UserRequirement Requirement { get; set; } = new();
        private bool ReqPopup { get; set; } = false;

        HashSet<Requirement> approvedRequirements = new HashSet<Requirement>();
        HashSet<Requirement> declinedRequirements = new HashSet<Requirement>();

        protected override async Task OnInitializedAsync()
        {
            var token    = await tokenProvider.GetTokenAsync();
            Requirements = await userRequirement.GetAllRequest(token);
        }

        private void ViewRequirement(UserRequirement req)
        {
            Requirement = req;
            ReqPopup    = true;
        }

        void ApproveRequirement(Requirement req)
        {
            approvedRequirements.Add(req);
            req.IsApproved = true;
        }

        void DeclineRequirement(Requirement req)
        {
            declinedRequirements.Add(req);
            req.IsDeclined = true;
        }

        bool IsApproved(Requirement req)
        {
            return approvedRequirements.Contains(req);
        }

        bool IsDeclined(Requirement req)
        {
            return declinedRequirements.Contains(req);
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

        private void ReqCloseDialog() => ReqPopup = false;
    }
}
