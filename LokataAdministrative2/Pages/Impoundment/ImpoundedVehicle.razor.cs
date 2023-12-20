using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;

namespace LokataAdministrative2.Pages.Impoundment
{
    public partial class ImpoundedVehicle
    {
        string token = "";
        UserDto? user;
        CitationDto? citation;
        List<VehicleDto> vehicleList = new();
        List<VehicleDto> filteredVehicles = new();
        List<TrackingDto> impoundingAreas = new();
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

        private async void UpdateImpoundedArea()
        {
            user = await userClient.GetRequestById(Vehicle.LicenseNo!, null);
            var notif = new NotificationDto
            {
                Email = user!.Email!,
                Date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                Message = $"Your vehicle has been moved to {ImpoundingAreaSelected}"
            };

            var selectedArea = impoundingAreas.FirstOrDefault(e => e.ImpoundingArea == ImpoundingAreaSelected);
            Vehicle.Location = selectedArea;

            await notificationClient.PostRequest(notif, null);
            await vehicleImpoundedClient.PutRequest(Vehicle, await tokenProvider.GetTokenAsync());

            var success = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Updated Area Success.",
                Icon = SweetAlertIcon.Success
            });

            VehiclePopup = false;
        }

        private async void ClaimedVehicle()
        {
            var token = await tokenProvider.GetTokenAsync();
            user = await userClient.GetRequestById(Vehicle.LicenseNo!, null);
            citation = await citationClient.GetByTctNo(Vehicle.TctNo!, token);
            Vehicle.IsImpounded = false;
            citation.IsSettled = true;
            Vehicle.Status = "Settled";

            var notif = new NotificationDto
            {
                Email = user!.Email!,
                Date = DateTime.UtcNow,
                Message = $"Your vehicle with a plate no. {Vehicle.PlateNo} has been claimed."
            };

            //await notificationClient.PostRequest(notif, null);
            //await citationClient.PutRequest(citation, token);
            //await vehicleImpoundedClient.PutRequest(Vehicle, token);

            var postNotificationTask = notificationClient.PostRequest(notif, null);
            var putCitationTask = citationClient.PutRequest(citation, token);
            var putVehicleTask = vehicleImpoundedClient.PutRequest(Vehicle, token);
            await Task.WhenAll(postNotificationTask, putCitationTask, putVehicleTask);

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
