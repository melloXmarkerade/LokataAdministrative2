using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using Microsoft.AspNetCore.Components;

namespace LokataAdministrative2.Pages.AdminPage
{
    public partial class IssueTicketPage
    {
        [Parameter]
        public string issuedTicketId { get; set; } = string.Empty;
        public bool PaymentSummaryPopup { get; set; } = false;
        bool popup     = false;

        CitationDto citation = new();
        OfficerDto officer = new();
        VehicleDto vehicle = new();
        PlaceDto placeApprehended = new();
        AddressDto address = new();
        UserViolationDto violation = new();
        StorageRateDto? storageRate;
        TowingRateDto? towingRate;
        ImpoundedArea? impoundingArea;
        PaymentSummaryDto? paymentSummary;

        List<ProvinceDto> provinces = new();
        List<CityDto> cities = new();
        List<BarangayDto> barangays = new();
        List<UserViolationDto> userViolations = new();
        List<ViolationCategoryDto> categories = new();
        List<ViolationDto> violations = new();
        List<ViolationFeeDto> violationFees = new();
        List<StorageRateDto> storages = new();
        List<TowingRateDto> towings = new();
        List<ImpoundedArea> impoundingAreas = new();
        List<string> itemConfiscated = new();

        private void ShowPopup() => popup = true;
        private void CloseDialog() => popup = false;
        private void ClosePaymentSummary() => PaymentSummaryPopup = false;

        private void SaveViolations()
        {
            userViolations.Add(violation);
            popup = false;
            violation = new();
        }

        private void SavePaymentsummary()
        {
            paymentSummary = new()
            {
                StorageRate = storageRate,
                TowingRate = towingRate,
                TotalViolationFees = TotalViolationFees(),
                Date = DateTime.Now.ToUniversalTime(),
            };

            PaymentSummaryPopup = false;
        }

        private double TotalViolationFees()
        {
            double totalViolations = 0;
            userViolations.ForEach(v =>
            {
                totalViolations += v.Fine;
            });
            return totalViolations;
        }

        private async Task OnValidSubmit()
        {
            if(paymentSummary is null)
            {
                vehicle.Status = string.Empty;
                paymentSummary = new()
                {
                    TotalViolationFees = TotalViolationFees(),
                    Date = DateTime.Now.ToUniversalTime(),
                };
            }

            vehicle.Status = "Unsettled";
            vehicle.TctNo = citation.TctNo;
            vehicle.LicenseNo = citation.LicenseNo;
            vehicle.DateImpounded = placeApprehended.Date;

            citation.Address = address;
            citation.VehicleDescription = vehicle;
            citation.VehicleDescription.Location = impoundingArea;
            citation.PlaceApprehended = placeApprehended;
            citation.Violations = userViolations;
            citation.ItemConfiscated = itemConfiscated;
            citation.ApprehendingOfficer = officer;
            citation.PaymentSummary = paymentSummary;

            await citationClient.PostRequest(citation, await tokenProvider.GetTokenAsync());

            var success = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Record Success",
                Icon = SweetAlertIcon.Success
            });
            navigation.NavigateTo("/issuedticket");

            //if (success.IsConfirmed)
            //{
            //    await OnInitializedAsync();
            //    navigation.NavigateTo("/issuedticket");
            //}
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
                PaymentSummaryPopup = true;
                itemConfiscated.Add("Motor Vehicle");
                vehicle.IsImpounded = true;
            }
            else
            {
                paymentSummary = new();
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
            provinces = await provinceClient.GetAllRequest(token);
            categories = await violationCatClient.GetAllRequest(token);
            storages = await storageClient.GetAllRequest(token);
            towings = await towingClient.GetAllRequest(token);
            impoundingAreas = await impoundingClient.GetAllRequest(token);
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

        private void StorageRateClicked(ChangeEventArgs storageEvent)
        {
            if (storageEvent.Value!.ToString() == "0") return;

            storageRate = storages.FirstOrDefault(s => s.Id == storageEvent.Value.ToString());
            this.StateHasChanged();
        }

        private void TowingRateClicked(ChangeEventArgs towingEvent)
        {
            if (towingEvent.Value!.ToString() == "0") return;

            towingRate = towings.FirstOrDefault(t => t.Id == towingEvent.Value.ToString());
            this.StateHasChanged();
        }

        private void ImpoundingAreaClicked(ChangeEventArgs impoundingEvent)
        {
            if (impoundingEvent.Value!.ToString() == "0") return;

            impoundingArea = impoundingAreas.FirstOrDefault(t => t.Id == impoundingEvent.Value.ToString());
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

            violations = await violationClient.GetRequestByCategoryId(violationEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
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