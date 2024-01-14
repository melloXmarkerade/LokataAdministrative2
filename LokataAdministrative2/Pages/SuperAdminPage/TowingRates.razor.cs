using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using System.Xml.Linq;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class TowingRates
    {

        bool IsSaveRate = false;
        bool ViewRatePopup = false;
        string FeeToString = "";
        public List<TowingRateDto> towingRates { get; set; } = new();
        public TowingRateDto? TowingRate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            towingRates = await towingClient.GetAllRequest(await tokenProvider.GetTokenAsync());
        }

        private void AddTowingRate()
        {
            TowingRate = new();
            IsSaveRate = true;
            ViewRatePopup = true;
        }

        private async Task SaveTowingRate()
        {
            if (!double.TryParse(FeeToString, out double result))
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Invalid Fee",
                    Icon = SweetAlertIcon.Info
                });
                return;
            }

            TowingRate!.Fee = result;
            await towingClient.PostRequest(TowingRate!, await tokenProvider.GetTokenAsync());
            Thread.Sleep(1000);
            towingRates = await towingClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Save Success",
                Icon = SweetAlertIcon.Success
            });

            IsSaveRate = false;
            ViewRatePopup = false;
        }

        private async Task UpdateTowingRate()
        {
            TowingRate!.Fee = Double.Parse(FeeToString);
            await towingClient.PutRequest(TowingRate!, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });

            ViewRatePopup = false;
        }

        private async Task DeleteTowingRate()
        {
            var delete = await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Warning,
                Title = "Do you want to delete towing rate?",
                ShowDenyButton = true,
                ConfirmButtonText = "Delete",
                DenyButtonText = "Cancel"
            });

            if (delete.IsConfirmed)
            {
                await towingClient.DeleteRequest(TowingRate!.Id!, await tokenProvider.GetTokenAsync());
                towingRates.Remove(TowingRate!);
                ViewRatePopup = false;
            }
        }

        private void ViewTowingRate(TowingRateDto storage)
        {
            TowingRate = storage;
            FeeToString = storage.Fee.ToString();
            ViewRatePopup = true;
        }

        private void CloseViewTowingRate()
        {
            FeeToString = string.Empty;
            IsSaveRate = false;
            ViewRatePopup = false;
        }
    }
}