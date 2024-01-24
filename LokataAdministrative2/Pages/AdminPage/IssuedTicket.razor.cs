using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using LokataAdministrative2.Pages.SuperAdminPage;
using Microsoft.AspNetCore.Components;

namespace LokataAdministrative2.Pages.AdminPage
{
    public partial class IssuedTicket
    {
        [Parameter]
        public bool SelectProfessional { get; set; } = false;

        [Parameter]
        public bool SelectNonProfessional { get; set; } = false;

        [Parameter]
        public bool SelectVehicle { get; set; } = false;

        [Parameter]
        public bool SelectLicense { get; set; } = false;

        public string ProvinceSelectedOption { get; set; } = string.Empty;
        public string CitySelectedOption { get; set; } = string.Empty;
        public string BarangaySelectedOption { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new();
        private List<ProvinceDto> Provinces { get; set; } = new();
        private List<CityDto> Cities { get; set; } = new();
        private List<BarangayDto> Barangays { get; set; } = new();
        private List<ViolationCategoryDto> Categories { get; set; } = new();
        private List<ViolationDto> Violations { get; set; } = new();
        //private List<ViolationFeeDto> ViolationFees { get; set; } = new();
        private UserViolationDto Violation { get; set; } = new(); 
        private bool Popup { get; set; } = false;
        public bool PaymentSummaryPopup { get; set; } = false;
        private bool ViewCitationPopup { get; set; } = false;
        private bool ViolationPopup { get; set; } = false;
        private CitationDto? Citation { get; set; }

        List<StorageRateDto> storages = new();
        List<TowingRateDto> towings = new();
        List<ImpoundedAreaDto> impoundingAreas = new();
        List<CitationDto> filteredTickets = new();
        List<CitationDto>? citationList;
        StorageRateDto? storageRate;
        TowingRateDto? towingRate;
        ImpoundedAreaDto? impoundingArea;
        PaymentSummaryDto? paymentSummary;
        ViolationDto? violationTemp;

        private void ShowViolationPopup() => ViolationPopup = true;

        private void ClosePaymentSummary() => PaymentSummaryPopup = false;

        private void ClosePopup()
        {
            BarangaySelectedOption = Citation!.Address!.Barangay!;
            Popup = false;
        }

        private void CloseViewCitationPopup() => ViewCitationPopup = false;

        private void ViolationCloseDialog() => ViolationPopup = false;

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            citationList = await citationClient.GetAllRequest(token);
            filteredTickets = citationList;
            Provinces = await provinceClient.GetAllRequest(token);
            Cities = await cityClient.GetAllRequest(token   );
            Categories = await violationCatClient.GetAllRequest(token);
            storages = await storageClient.GetAllRequest(token);
            towings = await towingClient.GetAllRequest(token);
            impoundingAreas = await impoundingClient.GetAllRequest(token);
        }

        void UpdateFilteredTicket(string searchItem)
        {
            if (string.IsNullOrEmpty(searchItem))
                filteredTickets = citationList!;
            else
            {
                filteredTickets = citationList!.Where(ticket => ticket.TctNo!.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                                                ticket.LicenseNo!.Contains(searchItem, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private void LicenseTypeChecked()
        {
            if (Citation!.LicenseType == "Professional")
                SelectProfessional = true;
            else
                SelectNonProfessional = true;
        }

        private void SelectedLicenseType(ChangeEventArgs e)
        {
            if (e.Value!.ToString()! != null)
                Citation!.LicenseType = e.Value!.ToString()!;
        }

        private async Task DeleteCitation(string id)
        {
            var delete = await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Warning,
                Title = "Do you want to delete citation",
                ShowDenyButton = true,
                ConfirmButtonText = "Delete",
                DenyButtonText = "Cancel"
            });

            if(delete.IsConfirmed)
            {
                var citation = filteredTickets.Where(e => e.Id == id).FirstOrDefault();
                await citationClient.DeleteRequest(id, await tokenProvider.GetTokenAsync());
                filteredTickets.Remove(citation!);
            }
        }

        private void ViewTicket(CitationDto dto)
        {
            if(dto is not null)
            {
                Citation = dto;
                SelectVehicle = Citation!.ItemConfiscated!.Contains("Motor Vehicle");
                SelectLicense = Citation!.ItemConfiscated!.Contains("Driver's License");
                LicenseTypeChecked();
                ViewCitationPopup = true;
            }
        }

        private async Task EditTicket(CitationDto citation)
        {
            if (citation is not null)
            {
                Citation = citation;
                SelectVehicle = citation!.ItemConfiscated!.Contains("Motor Vehicle");
                SelectLicense = citation!.ItemConfiscated!.Contains("Driver's License");
                LicenseTypeChecked();

                ProvinceSelectedOption = citation.Address!.Province!;
                CitySelectedOption     = citation.Address!.City!;
                BarangaySelectedOption = citation.Address!.Barangay!;

                Barangays = await barangayClient.GetRequestByCityId(CitySelectedOption, await tokenProvider.GetTokenAsync());
                Popup     = true;                
            }
        }

        private void SelectedVehicle(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
            {
                PaymentSummaryPopup = true;
                Citation!.ItemConfiscated!.Add("Motor Vehicle");
                Citation!.VehicleDescription!.IsImpounded = true;
            }
            else
            {
                Citation!.ItemConfiscated!.Remove("Motor Vehicle");
                Citation!.VehicleDescription!.IsImpounded = false;
            }
        }

        private void SelectedLicense(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
                Citation!.ItemConfiscated!.Add("Driver's License");
            else
                Citation!.ItemConfiscated!.Remove("Driver's License");
        }

        private void TicketIssuePage() => navigation.NavigateTo("/issuedTicketPage");

        private async Task ProvinceOnValueChanged(string value)
        {
            Cities.Clear();

            if (value == "0")
            {
                Barangays.Clear();
                return;
            }

            ProvinceSelectedOption = value;
            Cities = await cityClient.GetRequestByProvinceId(value, await tokenProvider.GetTokenAsync());
        }

        private async Task CityOnValueChanged(string value)
        {
            Barangays.Clear();

            if (value == "0")
                return;

            CitySelectedOption = value;
            Barangays = await barangayClient.GetRequestByCityId(value, await tokenProvider.GetTokenAsync());
        }

        private Task BarangayOnValueChanged(string value)
        {
            BarangaySelectedOption = value;
            return Task.CompletedTask;
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
            Violations.Clear();

            if (violationEvent.Value!.ToString()! == "0")
            {
                //ViolationFees.Clear();
                return;
            }

            Violations = await violationClient.GetRequestByCategoryId(violationEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
            Violation.Category = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private void ViolationClicked(ChangeEventArgs violationEvent)
        {
            //ViolationFees.Clear();
            if (violationEvent.Value!.ToString()! == "0") return;

            violationTemp = Violations.First(i => i.Name == violationEvent.Value!.ToString()!);
            Violation.Name = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private void ViolationFeeClicked(ChangeEventArgs violationFeeEvent)
        {
            Violation.Offense = violationFeeEvent.Value!.ToString()!.Substring(0, 1);
            Violation.Fine = Double.Parse(violationFeeEvent.Value!.ToString()!.Substring(3));
            this.StateHasChanged();
        }

        private void SaveViolations()
        {
            Citation!.Violations!.Add(Violation);
            ViolationPopup = false;
            Violation = new();
        }

        private void UpdatePaymentSummary()
        {
            //double totalViolations = 0;
            //Citation!.Violations!.ForEach(v =>
            //{
            //    totalViolations += v.Fine;
            //});

            //paymentSummary = new()
            //{
            //    StorageRate = storageRate,
            //    TowingRate = towingRate,
            //    TotalViolationFees = totalViolations,
            //    Date = DateTime.Now.ToShortDateString()
            //};

            //PaymentSummaryPopup = false;
            //paymentSummary = new();
        }

        private async Task UpdateChanges()
        {
            double totalViolations = 0;
            Citation!.Violations!.ForEach(v =>
            {
                totalViolations += v.Fine;
            });

            Citation!.PaymentSummary!.TotalViolationFees = totalViolations;
            Citation!.Address!.Province = ProvinceSelectedOption;
            Citation.Address.City       = CitySelectedOption;
            Citation.Address.Barangay   = BarangaySelectedOption;

            await citationClient.PutRequest(Citation, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });

            Popup = false;
        }
    }
}
