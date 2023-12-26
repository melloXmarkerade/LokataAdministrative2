using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class PaymentChannel
    {

        bool IsSaveChannel = false;
        bool ViewChannelPopup = false;
        public List<PaymentChannelDto> PaymentChannels { get; set; } = new();
        public PaymentChannelDto? Channel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PaymentChannels = await requirementClient.GetAllRequest(null!);
        }

        private void AddPaymentChannel()
        {
            Channel = new();
            IsSaveChannel = true;
            ViewChannelPopup = true;

        }

        private async Task SaveChannel()
        {
            await requirementClient.PostRequest(Channel!, await tokenProvider.GetTokenAsync());
            Thread.Sleep(1000);
            PaymentChannels = await requirementClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Save Success",
                Icon = SweetAlertIcon.Success
            });

            IsSaveChannel = false;
            ViewChannelPopup = false;
        }

        private async Task UpdateChannel()
        {
            await requirementClient.PutRequest(Channel!, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });

            ViewChannelPopup = false;
        }

        private async Task DeleteChannel()
        {
            var delete = await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Warning,
                Title = "Do you want to delete area?",
                ShowDenyButton = true,
                ConfirmButtonText = "Delete",
                DenyButtonText = "Cancel"
            });

            if (delete.IsConfirmed)
            {
                await requirementClient.DeleteRequest(Channel!.Id!, await tokenProvider.GetTokenAsync());
                PaymentChannels.Remove(Channel!);
                ViewChannelPopup = false;
            }
        }

        private void ViewChannel(PaymentChannelDto req)
        {
            Channel = req;
            ViewChannelPopup = true;
        }

        private void CloseViewChannel()
        {
            ViewChannelPopup = false;
        }
    }
}