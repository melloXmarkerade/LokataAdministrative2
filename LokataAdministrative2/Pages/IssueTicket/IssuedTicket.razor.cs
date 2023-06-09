using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;
using Microsoft.AspNetCore.Components;

namespace LokataAdministrative2.Pages.IssueTicket
{
    public partial class IssuedTicket
    {
        [Parameter]
        public bool SelectLicenseType { get; set; } = false;

        [Parameter]
        public bool SelectVehicle { get; set; } = false;

        [Parameter]
        public bool SelectLicense { get; set; } = false;

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

        private void ShowPopup() => Popup = true;

        private void ClosePopup() => Popup = false;

        private void ViolationCloseDialog() => ViolationPopup = false;

        protected override async Task OnInitializedAsync()
        {
            citationList = await citationClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            Provinces = await provinceClient.GetAllRequest(await tokenProvider.GetTokenAsync());
            Cities = await cityClient.GetAllRequest(await tokenProvider.GetTokenAsync());
            
            Categories = await violationCatClient.GetAllRequest(await tokenProvider.GetTokenAsync());
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

        private async void EditTicket(CitationDto citation)
        {
            if (citation is not null)
            {
                Citation = citation;

                SelectVehicle = citation!.ItemConfiscated!.Contains("Motor Vehicle");
                SelectLicense = citation!.ItemConfiscated!.Contains("Driver's License");
                Popup = true;

                Barangays = await barangayClient.GetRequestByCityId(Citation!.Address!.City!, await tokenProvider.GetTokenAsync());
            }

            //navigation.NavigateTo($"/issuedTicketPage/{id}");
        }

        private void SelectedVehicle(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
                Citation!.ItemConfiscated!.Add("Motor Vehicle");
            else
                Citation!.ItemConfiscated!.Remove("Motor Vehicle");
        }

        private void SelectedLicense(ChangeEventArgs e)
        {
            if ((bool)e.Value!)
                Citation!.ItemConfiscated!.Add("Driver's License");
            else
                Citation!.ItemConfiscated!.Remove("Driver's License");
        }

        private void TicketIssuePage() => navigation.NavigateTo("/issuedTicketPage");

        private async void ProvinceClicked(ChangeEventArgs provinceEvent)
        {
            Cities.Clear();

            if (provinceEvent.Value!.ToString()! == "0")
            {
                Barangays.Clear();
                return;
            }

            Citation!.Address!.Province = provinceEvent.Value!.ToString()!;
            Cities = await cityClient.GetRequestByProvinceId(provinceEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
            this.StateHasChanged();
        }

        private async void CityClicked(ChangeEventArgs cityEvent)
        {
            Barangays.Clear();

            if (cityEvent.Value!.ToString()! == "0")
                return;

            Citation!.Address!.City = cityEvent.Value!.ToString()!;
            Barangays = await barangayClient.GetRequestByCityId(cityEvent.Value!.ToString()!, await tokenProvider.GetTokenAsync());
            this.StateHasChanged();
        }

        private void BarangayClicked(ChangeEventArgs barangayClient)
        {
            Citation!.Address!.Barangay = barangayClient.Value!.ToString()!;
            this.StateHasChanged();
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
    }
}
