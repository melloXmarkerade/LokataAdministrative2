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
        private bool ReceiptPopup { get; set; } = false;

        bool subjectForImpound = false;
        string token = string.Empty;
        CitationDto? citation;
        FileRequirement approvedReceipt = new();
        FileRequirement declinedReceipt = new();

        protected override async Task OnInitializedAsync()
        {
            token = await tokenProvider.GetTokenAsync();
            Receipts = await userReceipt.GetAllRequest(token);
        }

        private async Task ViewReceipt(UserReceipt receipt)
        {
            UserReq = await userRequirement.GetByTctNo(receipt.TctNo, token);
            Receipt = receipt;
            subjectForImpound = await vehicleImpoundedClient.CheckTctNoHasImpoundVehicle(Receipt.TctNo);
            ReceiptPopup = true;
        }

        private async Task ApproveReceipt(FileRequirement receipt)
        {
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
                approvedReceipt = receipt;
                receipt.IsApproved = true;
                return;
            }

            await CheckApproveRequirements(receipt);
        }            

        async void DeclineReceipt(FileRequirement receipt)
        {
            if(UserReq.Id is null)
            {
                declinedReceipt = receipt;
                receipt.IsDeclined = true;
            } 
            else
                await CheckApproveRequirements(receipt);
        }

        public async Task SendNotification()
        {
            citation = await citationClient.GetByTctNo(UserReq.TctNo!, token);

            if (Receipt.Receipt!.IsApproved == false && Receipt.Receipt.IsDeclined == false)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Required to approve or decline receipt",
                    Icon = SweetAlertIcon.Info
                });

                return;
            }

            var notif = new NotificationDto
            {
                Email = Receipt.Email!,
                Message = InputText
            };

            var userRec = new UserReceipt
            {
                Id = Receipt.Id,
                Email = Receipt?.Email!,
                LicenseNo = Receipt!.LicenseNo,
                DateSubmitted = Receipt.DateSubmitted,
                TctNo = Receipt.TctNo
            };

            CheckFileUrl(userRec);
            await CheckCitationReceiptIsImpounded();
            await Task.WhenAll(userReceipt.PutRequest(userRec, token), notificationClient.PostRequest(notif, null!));

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Send Success",
                Icon = SweetAlertIcon.Success
            });

            ReceiptPopup = false;
        }

        private async Task CheckCitationReceiptIsImpounded()
        {
            if (!citation!.VehicleDescription!.IsImpounded)
                await citationClient.PutRequest(citation, token);
        }

        private void CheckFileUrl(UserReceipt userRec)
        {
            if (approvedReceipt.FileUrl == string.Empty)
                userRec.Receipt = declinedReceipt;
            else
                userRec.Receipt = approvedReceipt;
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

        private void ReceiptCloseDialog()
        {
            UserReq = null!;
            Receipt = null!;
            ReceiptPopup = false;
        }

        private async Task CheckApproveRequirements(FileRequirement receipt)
        {
            var isApproved = UserReq!.Requirements!.All(r => r.IsApproved == true);

            if (isApproved)
            {
                approvedReceipt = receipt;
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