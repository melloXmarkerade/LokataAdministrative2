using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using Microsoft.AspNetCore.Components;

namespace LokataAdministrative2.Pages.AdminPage
{
    public partial class IssueTicketPage
    {
        [Parameter]
        public string IssuedTicketId { get; set; } = string.Empty;
        public bool PaymentSummaryPopup { get; set; } = false;
        bool popup = false;
        bool isCheckedVehicle = false;

        readonly CitationDto citation = new();
        readonly OfficerDto officer = new();
        readonly VehicleDto vehicle = new();
        readonly PlaceDto placeApprehended = new();
        readonly AddressDto address = new();
        UserViolationDto userViolation = new();
        StorageRateDto? storageRate;
        TowingRateDto? towingRate;
        ImpoundedAreaDto? impoundingArea;
        PaymentSummaryDto? paymentSummary;
        ViolationDto? violationTemp = new();

        List<ProvinceDto> provinces = new();
        List<CityDto> cities = new();
        List<BarangayDto> barangays = new();
        List<UserViolationDto> userViolations = new();
        List<ViolationCategoryDto> categories = new();
        List<ViolationDto> violations = new();
        //List<ViolationFeeDto> violationFees = new();
        List<StorageRateDto> storages = new();
        List<TowingRateDto> towings = new();
        List<ImpoundedAreaDto> impoundingAreas = new();
        readonly List<string> itemConfiscated = new();

        private void ShowPopup()
        {
            popup = true;
        }

        private void CloseDialog()
        {
            popup = false;
        }

        private void ClosePaymentSummary()
        {
            isCheckedVehicle = false;
            itemConfiscated.Remove("Motor Vehicle");
            PaymentSummaryPopup = false;
        }

        private void SaveViolations()
        {
            userViolations.Add(userViolation);
            popup = false;
            userViolation = new();
        }

        private void SavePaymentsummary()
        {
            paymentSummary = new()
            {
                StorageRate = storageRate,
                TowingRate = towingRate,
                TotalStorageFee = storageRate!.Fee,
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
            if (await CheckEmptyViolations()) return;
            if (await CheckExistingTctNo()) return;
            
            CheckPaymentSummary();
            MapCitationData();

            await citationClient.PostRequest(citation, await tokenProvider.GetTokenAsync());
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Record Success",
                Icon = SweetAlertIcon.Success
            });
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
            isCheckedVehicle = (bool)e.Value!;
            if (isCheckedVehicle)
            {
                PaymentSummaryPopup = true;
                itemConfiscated.Add("Motor Vehicle");
                vehicle.IsImpounded = isCheckedVehicle;
            }
            else
            {
                paymentSummary = null;
                itemConfiscated.Remove("Motor Vehicle");
                vehicle.IsImpounded = isCheckedVehicle;
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
                //violationFees.Clear();
                return;
            }

            violations = await violationClient.GetRequestByCategoryId(violationEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
            userViolation.Category = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private void ViolationClicked(ChangeEventArgs violationEvent)
        {
            //violationFees.Clear();
            if (violationEvent.Value!.ToString()! == "0") return;

            violationTemp = violations.First(i => i.Name == violationEvent.Value!.ToString()!);
            userViolation.Name = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private void ViolationFeeClicked(ChangeEventArgs violationFeeEvent)
        {
            userViolation.Offense = violationFeeEvent.Value!.ToString()![..1];
            userViolation.Fine = Double.Parse(violationFeeEvent.Value!.ToString()![3..]);
            this.StateHasChanged();
        }

        private async Task<bool> CheckEmptyViolations()
        {
            if (userViolations.Count == 0)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Add Violation",
                    Icon = SweetAlertIcon.Info
                });
                return true;
            }
            return false;
        }

        private void CheckPaymentSummary()
        {
            if (paymentSummary is null)
            {
                vehicle.Status = string.Empty;
                paymentSummary = new()
                {
                    TotalViolationFees = TotalViolationFees(),
                    Date = DateTime.Now.ToUniversalTime(),
                };
            }
        }

        private void RemoveViolation(UserViolationDto violation)
        {
            userViolations.Remove(violation);
        }

        private void MapCitationData()
        {
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
        }

        private async Task<bool> CheckExistingTctNo()
        {
            var isExisted = await citationClient.CheckExistedTctNo(citation.TctNo!, await tokenProvider.GetTokenAsync());

            if(isExisted)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Tct No. is existed",
                    Icon = SweetAlertIcon.Info
                });
                return true;
            }
            return false;

        }
    }
}