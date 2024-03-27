using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class SubmittedRequirements
    {
        private List<UserRequirement> Requirements { get; set; } = new();
        private UserRequirement? Requirement { get; set; } = new();
        private string InputText { get; set; } = string.Empty;
        private bool ReqPopup { get; set; } = false;

        string token = string.Empty;
        readonly List<FileRequirement> approvedRequirements = new();
        readonly List<FileRequirement> declinedRequirements = new();

        protected override async Task OnInitializedAsync()
        {
            token = await tokenProvider.GetTokenAsync();
            Requirements = await userRequirement.GetAllRequest(token);
        }

        private void ViewRequirement(UserRequirement req)
        {
            Requirement = req;
            ReqPopup = true;
        }

        void ApproveRequirement(FileRequirement req)
        {
            approvedRequirements.Add(req);
            req.IsApproved = true;
        }

        void DeclineRequirement(FileRequirement req)
        {
            declinedRequirements.Add(req);
            req.IsDeclined = true;
        }

        public async Task SendNotification()
        {
            if (approvedRequirements.Count == 0)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Required to Approve or Decline Requirements",
                    Text = "Need to approve the requirements before sending a notifications.",
                    Icon = SweetAlertIcon.Info
                });

                return;
            }

            var notif = new NotificationDto
            {
                Email = Requirement!.Email!,
                Message = InputText
            };

            var userReq = new UserRequirement
            {
                Id = Requirement.Id,
                Email = Requirement.Email,
                LicenseNo = Requirement!.LicenseNo,
                Requirements = approvedRequirements,
                DateSubmitted = Requirement.DateSubmitted,
                TctNo = Requirement.TctNo
            };

            declinedRequirements.ForEach(e => userReq.Requirements.Add(e));
            await userRequirement.PutRequest(userReq, token);
            await notificationClient.PostRequest(notif, null!);

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Send Success",
                Icon = SweetAlertIcon.Success
            });

            ReqPopup = false;
        }

        static bool IsApproved(FileRequirement req) 
        {
            if (req.IsApproved)
                return true;
            else
                return false;
        }

        static bool IsDeclined(FileRequirement req)
        {
            if (req.IsDeclined)
                return true;
            else
                return false;
        }

        private static string GetStatus(FileRequirement req)
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
            Requirement = null;
            ReqPopup = false;
        }
    }
}
