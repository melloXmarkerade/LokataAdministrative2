using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class UserSubmittedReciept
    {
        private List<UserReceipt> Receipts { get; set; } = new();
        private UserRequirement UserReq { get; set; } = new();
        private UserReceipt Receipt { get; set; } = new();
        private string InputText { get; set; } = string.Empty;
        private bool RecPopup { get; set; } = false;
        bool subjectForImpound = false;

        List<FileRequirement> approvedReceiptReqs = new();
        List<FileRequirement> declinedReceiptReqs = new();

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            Receipts = await userReceipt.GetAllRequest(token);
        }

        private async Task ViewReceipt(UserReceipt receipt)
        {
            UserReq = await userRequirement.GetByTctNo(receipt.TctNo, await tokenProvider.GetTokenAsync());
            Receipt = receipt;
            subjectForImpound = await vehicleImpoundedClient.CheckTctNoHasImpoundVehicle(Receipt.TctNo);
            RecPopup = true;
        }

        private async Task ApproveReceipt(FileRequirement receipt)
        {
            //if(subjectForImpound)
            //{

            //}

            if (UserReq.Id is null && subjectForImpound == true)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Receipt Info",
                    Text = "Owner didn't submitted a requirements.",
                    Icon = SweetAlertIcon.Info
                });
                return;
            }

            if (!subjectForImpound)
            {
                approvedReceiptReqs.Add(receipt);
                receipt.IsApproved = true;
                return;
            }

            await CheckApproveRequirements(receipt, approvedReceiptReqs);
        }            

        async void DeclineReceipt(FileRequirement receipt)
        {
            await CheckApproveRequirements(receipt, declinedReceiptReqs);
        }

        public async Task SendNotification()
        {
            if (approvedReceiptReqs.Count == 0)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Required to Approve Receipt",
                    Text = "Need to approve the receipt before sending a notifications.",
                    Icon = SweetAlertIcon.Info
                });
                return;
            }

            var notif = new NotificationDto
            {
                Email = Receipt.Email!,
                Message = InputText
            };

            var userReq = new UserReceipt
            {
                Id = Receipt.Id,
                Email = Receipt?.Email!,
                LicenseNo = Receipt!.LicenseNo,
                Receipt = approvedReceiptReqs.First(),
                DateSubmitted = Receipt.DateSubmitted,
                TctNo = Receipt.TctNo
            };

            //declinedReceiptReqs.ForEach(e => userReq.Receipt.Add(e));
            await userReceipt.PutRequest(userReq, await tokenProvider.GetTokenAsync());
            await notificationClient.PostRequest(notif, null!);

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Send Success",
                Icon = SweetAlertIcon.Success
            });


            RecPopup = false;
        }

        static bool IsApproved(FileRequirement receipt)
        {
            if (receipt.IsApproved)
                return true;
            else
                return false;
        }

        static bool IsDeclined(FileRequirement receipt)
        {
            if (receipt.IsDeclined)
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

        private void RecCloseDialog()
        {
            UserReq = null!;
            Receipt = null!;
            RecPopup = false;
        }

        private async Task CheckApproveRequirements(FileRequirement receipt, List<FileRequirement> req)
        {
            var isApproved = UserReq!.Requirements!.All(r => r.IsApproved == true);

            if (isApproved)
            {
                req.Add(receipt);
                receipt.IsApproved = true;
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Requirements Pending",
                    Text = "Submitted requirements should be approve.",
                    Icon = SweetAlertIcon.Info
                });
            }
        }
    }
}