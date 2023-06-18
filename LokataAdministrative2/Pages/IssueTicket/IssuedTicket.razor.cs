using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.PdfViewer;

namespace LokataAdministrative2.Pages.IssueTicket
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
        private List<ViolationFeeDto> ViolationFees { get; set; } = new();
        private UserViolationDto Violation { get; set; } = new(); 
        private bool Popup { get; set; } = false;
        private bool ViolationPopup { get; set; } = false;
        private CitationDto? Citation { get; set; }

        List<CitationDto>? citationList;

        private void ShowPopup() => ViolationPopup = true;

        private void ClosePopup()
        {
            BarangaySelectedOption = Citation!.Address!.Barangay!;
            Popup = false;
        }

        private void ViolationCloseDialog() => ViolationPopup = false;

        protected override async Task OnInitializedAsync()
        {
            citationList = await citationClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            Provinces = await provinceClient.GetAllRequest(await tokenProvider.GetTokenAsync());
            Cities    = await cityClient.GetAllRequest(await tokenProvider.GetTokenAsync());
            

            Categories = await violationCatClient.GetAllRequest(await tokenProvider.GetTokenAsync());
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

        private async Task DeleteAddress(string id)
        {
            await citationClient.DeleteRequest(id, await tokenProvider.GetTokenAsync());
            await Task.Delay(3000);
            await OnInitializedAsync();
        }

        private void ViewTicket(CitationDto dto)
        {

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
                Popup = true;                
            }

            //navigation.NavigateTo($"/issuedTicketPage/{id}");
        }

        private void SelectedVehicle(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
            {
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

        private async void ViolationCategoryClicked(ChangeEventArgs violationEvent)
        {
            Violations.Clear();

            if (violationEvent.Value!.ToString()! == "0")
            {
                ViolationFees.Clear();
                return;
            }

            Violations = await violationClient.GetRequestByCategoryId(violationEvent.Value!.ToString()!,
                            await tokenProvider.GetTokenAsync());
            Violation.Category = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private async void ViolationClicked(ChangeEventArgs violationEvent)
        {
            ViolationFees.Clear();
            if (violationEvent.Value!.ToString()! == "0") return;

            ViolationFees = await violationFeeClient.GetRequestByViolationId(violationEvent.Value!.ToString()!,
                                await tokenProvider.GetTokenAsync());
            Violation.Name = violationEvent.Value!.ToString()!;
            this.StateHasChanged();
        }

        private void ViolationFeeClicked(ChangeEventArgs violationFeeEvent)
        {
            Violation.Offense = violationFeeEvent.Value!.ToString()!.Substring(0, 1);
            Violation.Fine = Double.Parse(violationFeeEvent.Value!.ToString()!.Substring(3));
            this.StateHasChanged();
        }

        private void SaveChangesAsync()
        {
            Citation!.Violations!.Add(Violation);
            ViolationPopup = false;
            Violation = new();
        }

        private async Task UpdateChanges()
        {
            Citation!.Address!.Province = ProvinceSelectedOption;
            Citation.Address.City       = CitySelectedOption;
            Citation.Address.Barangay   = BarangaySelectedOption;

            await citationClient.PutRequest(Citation, await tokenProvider.GetTokenAsync());
            await js.InvokeVoidAsync("alert", "Update Success");
        }
    }
}
