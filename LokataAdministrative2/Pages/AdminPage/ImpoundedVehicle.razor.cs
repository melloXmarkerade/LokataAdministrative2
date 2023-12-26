using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;

namespace LokataAdministrative2.Pages.AdminPage
{
    public partial class ImpoundedVehicle
    {
        string token = "";
        UserDto? user;
        CitationDto? citation;
        List<VehicleDto> vehicleList = new();
        List<VehicleDto> filteredVehicles = new();
        List<ImpoundedArea> impoundingAreas = new();
        public string ImpoundingAreaSelected { get; set; } = string.Empty;
        private VehicleDto Vehicle { get; set; } = new();
        private bool VehiclePopup { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            token = await tokenProvider.GetTokenAsync();
            vehicleList = await vehicleImpoundedClient.GetAllRequest(token);
            filteredVehicles = vehicleList;
            impoundingAreas = await impoundingClient.GetAllRequest(token);
        }

        void UpdateFilteredVehicle(string searchItem)
        {
            if (string.IsNullOrEmpty(searchItem))
                filteredVehicles = vehicleList;
            else
            {
                filteredVehicles = vehicleList!.Where(vehicle => vehicle.PlateNo!.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                                                vehicle.TctNo!.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                                                vehicle.LicenseNo!.Contains(searchItem, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private async Task UpdateImpoundedArea()
        {
            user = await userClient.GetRequestById(Vehicle.LicenseNo!, null);
            var notif = new NotificationDto
            {
                Email = user!.Email!,
                Date = DateTime.Now.ToString("MMMM dd, yyyy"),
                Message = $"Your vehicle has been moved to {ImpoundingAreaSelected}"
            };

            var selectedArea = impoundingAreas.FirstOrDefault(e => e.ImpoundingArea == ImpoundingAreaSelected);
            Vehicle.Location = selectedArea;

            await notificationClient.PostRequest(notif, null);
            await vehicleImpoundedClient.PutRequest(Vehicle, token);

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Updated Area Success.",
                Icon = SweetAlertIcon.Success
            });

            VehiclePopup = false;
        }

        private async Task ClaimedVehicle()
        {
            user = await userClient.GetRequestById(Vehicle.LicenseNo!, null);
            citation = await citationClient.GetByTctNo(Vehicle.TctNo!, token);
            Vehicle.IsImpounded = false;
            citation.IsSettled = true;
            Vehicle.Status = "Settled";

            var notif = new NotificationDto
            {
                Email = user!.Email!,
                Date = DateTime.Now.ToString("MMMM dd, yyyy"),
                Message = $"Your vehicle with a plate no. {Vehicle.PlateNo} has been claimed."
            };

            await notificationClient.PostRequest(notif, null);
            await citationClient.PutRequest(citation, token);
            Thread.Sleep(2000);
            await vehicleImpoundedClient.PutRequest(Vehicle, token);

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Vehicle has claimed.",
                Icon = SweetAlertIcon.Success
            });

            VehiclePopup = false;
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
