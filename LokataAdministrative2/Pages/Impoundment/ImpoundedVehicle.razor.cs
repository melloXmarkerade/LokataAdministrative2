using LokataAdministrative2.Models;
using LokataAdministrative2.Services;

namespace LokataAdministrative2.Pages.Impoundment
{
    public partial class ImpoundedVehicle
    {
        List<VehicleDto> vehicleList = new();
        private VehicleDto Vehicle { get; set; } = new();
        private bool VehiclePopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var token    = await tokenProvider.GetTokenAsync();
            vehicleList  = await vehicleImpoundedClient.GetAllRequest(token);
        }

        private void ViewVehicle(VehicleDto vehicle)
        {
            Vehicle      = vehicle;
            VehiclePopup = true;
        }

        private void VehicleCloseDialog() => VehiclePopup = false;
    }
}
