using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using LokataAdministrative2.Services;

namespace LokataAdministrative2.Pages.Impoundment
{
    public partial class ImpoundedVehicle
    {
        List<VehicleDto> vehicleList = new();
        List<TrackingDto> impoundingAreas = new();
        public string ImpoundingAreaSelected { get; set; } = string.Empty;
        private VehicleDto Vehicle { get; set; } = new();
        private bool VehiclePopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            vehicleList = await vehicleImpoundedClient.GetAllRequest(token);
            impoundingAreas = await impoundingClient.GetAllRequest(token);
        }

        private async void ClaimedVehicle()
        {
            Vehicle.IsImpounded = false;
            Vehicle.Status = "Settled";
            await vehicleImpoundedClient.PutRequest(Vehicle, await tokenProvider.GetTokenAsync());

            var success = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Vehicle has claimed.",
                Icon = SweetAlertIcon.Success
            });

            if (success.IsConfirmed)
            {
                VehiclePopup = false;
            }
        }

        private void ViewVehicle(VehicleDto vehicle)
        {
            ImpoundingAreaSelected = vehicle.Location!.ImpoundingArea;
            Vehicle = vehicle;
            VehiclePopup = true;
        }

        private void VehicleCloseDialog() => VehiclePopup = false;

        private void ImpoundingAreaOnValueChanged(string value)
        {
            if (value == "0")
                return;

            ImpoundingAreaSelected = value;
        }
    }
}
