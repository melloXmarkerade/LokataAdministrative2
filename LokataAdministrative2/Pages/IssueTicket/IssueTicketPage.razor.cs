using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LokataAdministrative2.Pages.IssueTicket
{
    public partial class IssueTicketPage
    {
        [Parameter]
        public string issuedTicketId { get; set; } = string.Empty;

        string btnText = string.Empty;
        bool popup     = false;

        CitationDto citation       = new();
        OfficerDto officer         = new();
        VehicleDto vehicle         = new();
        PlaceDto placeApprehended  = new();
        AddressDto address         = new();
        UserViolationDto violation = new();
        TrackingDto? vehicleTrack;

        List<ProvinceDto> provinces           = new();
        List<CityDto> cities                  = new();
        List<BarangayDto> barangays           = new();
        List<UserViolationDto> userViolations = new();
        List<ViolationCategoryDto> categories = new();
        List<ViolationDto> violations         = new();
        List<ViolationFeeDto> violationFees   = new();
        List<string> itemConfiscated          = new();

        private void ShowPopup() => popup = true;

        private void CloseDialog() => popup = false;

        private void SaveChangesAsync()
        {
            userViolations.Add(violation);
            popup = false;
            violation = new();
        }

        private async Task OnValidSubmit()
        {
            vehicle.Status    = "Unsettled";
            vehicle.TctNo     = citation.TctNo;
            vehicle.LicenseNo = citation.LicenseNo;
            vehicle.DateImpounded = placeApprehended.Date;

            citation.Address             = address;
            citation.VehicleDescription  = vehicle;
            citation.PlaceApprehended    = placeApprehended;
            citation.Violations          = userViolations;
            citation.ItemConfiscated     = itemConfiscated;
            citation.ApprehendingOfficer = officer;

            await citationClient.PostRequest(citation, await tokenProvider.GetTokenAsync());
            await js.InvokeVoidAsync("alert", "Record Success");
            navigation.NavigateTo("/issuedticket");
        }

        private void Cancel() => navigation.NavigateTo("/issuedticket");

        private void SelectedLicenseType(ChangeEventArgs e)
        {
            if (e.Value!.ToString()! != null)
                citation.LicenseType = e.Value!.ToString()!;
        }

        private void SelectedVehicle(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
            {
                vehicleTrack = new TrackingDto
                {
                    Latitude = "10.323297",
                    Longitude = "123.941392"
                };

                itemConfiscated.Add("Motor Vehicle");
                vehicle.IsImpounded = true;
            }
            else
            {
                vehicleTrack = null;
                itemConfiscated.Remove("Motor Vehicle");
                vehicle.IsImpounded = false;
            }
        }

        private void SelectedLicense(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
                itemConfiscated.Add("Driver's License");
            else
                itemConfiscated.Remove("Driver's License");
        }

        protected async override Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            if (issuedTicketId is not null)
            {
                citation = await citationClient.GetRequestById(issuedTicketId, token);
                vehicle = citation!.VehicleDescription!;
                placeApprehended = citation!.PlaceApprehended!;
                officer = citation!.ApprehendingOfficer!;
                address = citation!.Address!;
                userViolations = citation!.Violations!;

                btnText = "Update Citation";
            }
            else
            {
                btnText = "Add Citation";
            }

            provinces = await provinceClient.GetAllRequest(token);
            categories = await violationCatClient.GetAllRequest(token);
        }

        private async void ProvinceClicked(ChangeEventArgs provinceEvent)
        {
            cities.Clear();

            if (provinceEvent.Value!.ToString()! == "0")
            {
                barangays.Clear();
                return;
            }

            address.Province = provinceEvent.Value!.ToString()!;
            cities = await cityClient.GetRequestByProvinceId(provinceEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
            this.StateHasChanged();
        }

        private async void CityClicked(ChangeEventArgs cityEvent)
        {
            barangays.Clear();

            if (cityEvent.Value!.ToString()! == "0")
                return;

            address.City = cityEvent.Value!.ToString()!;
            barangays = await barangayClient.GetRequestByCityId(cityEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
            this.StateHasChanged();
        }

        private void BarangayClicked(ChangeEventArgs barangayClient)
        {
            address.Barangay = barangayClient.Value!.ToString()!;
            this.StateHasChanged();
        }

        private async void ViolationCategoryClicked(ChangeEventArgs violationEvent)
        {
            violations.Clear();

            if (violationEvent.Value!.ToString()! == "0")
            {
                violationFees.Clear();
                return;
            }

            violations = await violationClient.GetRequestByCategoryId(violationEvent.Value!.ToString()!,
                            await tokenProvider.GetTokenAsync());
            violation.Category = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private async void ViolationClicked(ChangeEventArgs violationEvent)
        {
            violationFees.Clear();
            if (violationEvent.Value!.ToString()! == "0") return;

            violationFees = await violationFeeClient.GetRequestByViolationId(violationEvent.Value!.ToString()!,
                                await tokenProvider.GetTokenAsync());
            violation.Name = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private void ViolationFeeClicked(ChangeEventArgs violationFeeEvent)
        {
            violation.Offense = violationFeeEvent.Value!.ToString()!.Substring(0, 1);
            violation.Fine = Double.Parse(violationFeeEvent.Value!.ToString()!.Substring(3));
            this.StateHasChanged();
        }
    }
}
