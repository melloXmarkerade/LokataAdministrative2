using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Users;
using LokataAdministrative2.Services;

namespace LokataAdministrative2.Pages.UserPage
{
    public partial class UserSubmittedReciept
    {
        private List<UserReceipt> Receipts { get; set; } = new();
        private UserReceipt Receipt { get; set; } = new();
        private string InputText { get; set; } = string.Empty;
        private bool RecPopup { get; set; } = false;

        List<Requirement> approvedReceiptReqs = new();
        List<Requirement> declinedReceiptReqs = new();

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            Receipts = await userReceipt.GetAllRequest(token);
        }

        private void ViewRequirement(UserReceipt receipt)
        {
            Receipt = receipt;
            RecPopup = true;
        }

        void ApproveRequirement(Requirement receipt)
        {
            approvedReceiptReqs.Add(receipt);
            receipt.IsApproved = true;
        }

        void DeclineRequirement(Requirement receipt)
        {
            declinedReceiptReqs.Add(receipt);
            receipt.IsDeclined = true;
        }

        public async Task SendNotification()
        {
            var notif = new NotificationDto
            {
                Email = Receipt.Email!,
                Date = DateTime.Now.ToString("MMMM dd, yyyy"),
                Message = InputText
            };

            var userReq = new UserReceipt
            {
                Id = Receipt.Id,
                Email = Receipt?.Email!,
                LicenseNo = Receipt!.LicenseNo,
                Receipt = approvedReceiptReqs.First(),
                DateSubmitted = Receipt.DateSubmitted,
                PlateNo = Receipt.PlateNo
            };

            //declinedReceiptReqs.ForEach(e => userReq.Receipt.Add(e));
            await userReceipt.PutRequest(userReq, await tokenProvider.GetTokenAsync());
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

        static bool IsApproved(Requirement receipt)
        {
            if (receipt.IsApproved)
                return true;
            else
                return false;
        }

        static bool IsDeclined(Requirement receipt)
        {
            if (receipt.IsDeclined)
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

        private void ReqCloseDialog() => RecPopup = false;
    }
}
