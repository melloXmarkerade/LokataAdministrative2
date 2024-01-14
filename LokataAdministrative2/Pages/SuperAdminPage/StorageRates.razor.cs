using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using System.Threading.Channels;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class StorageRates
    {
        bool IsSaveRate = false;
        bool ViewRatePopup = false;
        string FeeToString = "";
        public List<StorageRateDto> storageRates { get; set; } = new();
        public StorageRateDto? StorageRate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            storageRates = await storageClient.GetAllRequest(await tokenProvider.GetTokenAsync());
        }

        private void AddStorageRate()
        {
            StorageRate = new();
            IsSaveRate = true;
            ViewRatePopup = true;
        }

        private async Task SaveStorageRate()
        {
            if(!double.TryParse(FeeToString, out double result))
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Invalid Fee",
                    Icon = SweetAlertIcon.Info
                });
                return;
            }

            StorageRate!.Fee = result;
            await storageClient.PostRequest(StorageRate!, await tokenProvider.GetTokenAsync());
            Thread.Sleep(1000);
            storageRates = await storageClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Save Success",
                Icon = SweetAlertIcon.Success
            });

            FeeToString = string.Empty;
            IsSaveRate = false;
            ViewRatePopup = false;
        }

        private async Task UpdateStorageRate()
        {
            StorageRate!.Fee = Double.Parse(FeeToString);
            await storageClient.PutRequest(StorageRate!, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });

            ViewRatePopup = false;
        }

        private async Task DeleteStorageRate()
        {
            var delete = await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Warning,
                Title = "Do you want to delete storage rate?",
                ShowDenyButton = true,
                ConfirmButtonText = "Delete",
                DenyButtonText = "Cancel"
            });

            if (delete.IsConfirmed)
            {
                await storageClient.DeleteRequest(StorageRate!.Id!, await tokenProvider.GetTokenAsync());
                storageRates.Remove(StorageRate!);
                ViewRatePopup = false;
            }
        }

        private void ViewStorageRate(StorageRateDto storage)
        {
            StorageRate = storage;
            FeeToString = storage.Fee.ToString();
            ViewRatePopup = true;
        }

        private void CloseViewStorageRate()
        {
            FeeToString = string.Empty;
            IsSaveRate = false;
            ViewRatePopup = false;
        }
    }
}