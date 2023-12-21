using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class SubmittedRequirements
    {
        private List<UserRequirement> Requirements { get; set; } = new();
        private UserRequirement Requirement { get; set; } = new();
        private string InputText { get; set; } = string.Empty;
        private bool ReqPopup { get; set; } = false;

        List<Requirement> approvedRequirements = new();
        List<Requirement> declinedRequirements = new();

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            Requirements = await userRequirement.GetAllRequest(token);
        }

        private void ViewRequirement(UserRequirement req)
        {
            Requirement = req;
            ReqPopup = true;
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

        public async Task SendNotification()
        {
            var notif = new NotificationDto
            {
                Email = Requirement.Email!,
                Date = DateTime.Now.ToString("MMMM dd, yyyy"),
                Message = InputText
            };

            var userReq = new UserRequirement
            {
                Id = Requirement.Id,
                Email = Requirement.Email,
                LicenseNo = Requirement!.LicenseNo,
                Requirements = approvedRequirements,
                DateSubmitted = Requirement.DateSubmitted,
                PlateNo = Requirement.PlateNo
            };

            declinedRequirements.ForEach(e => userReq.Requirements.Add(e));
            await userRequirement.PutRequest(userReq, await tokenProvider.GetTokenAsync());
            await notificationClient.PostRequest(notif, null);

            var success = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Send Success",
                Icon = SweetAlertIcon.Success
            });

            if (success.IsConfirmed)
            {
                await OnInitializedAsync();
                ReqCloseDialog();
            }

        }

        static bool IsApproved(Requirement req) 
        {
            if (req.IsApproved)
                return true;
            else
                return false;
        }

        static bool IsDeclined(Requirement req)
        {
            if (req.IsDeclined)
                return true;
            else
                return false;
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

        private void ReqCloseDialog()
        {
            ReqPopup = false;
        }
    }
}
