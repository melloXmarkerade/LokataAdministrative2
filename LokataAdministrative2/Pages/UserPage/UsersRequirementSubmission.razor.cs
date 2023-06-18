using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class UsersRequirementSubmission
    {
        private List<UserRequirement> Requirements { get; set; } = new();
        private UserRequirement Requirement { get; set; } = new();
        private bool ReqPopup { get; set; } = false;

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

        private void ApproveRequirement(Requirement req)
        {

        }

        private void DeclineRequirement(Requirement req)
        {

        }

        private void ReqCloseDialog() => ReqPopup = false;
    }
}
