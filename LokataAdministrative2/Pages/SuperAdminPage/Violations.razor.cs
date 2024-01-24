using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;
using LokataAdministrative2.Models.Citation;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class Violations
    {
        bool IsSaveViolation = false;
        bool ViewViolationPopup = false;

        private string FirstOffense
        {
            get => Violation!.ViolationFees!.FirstOffense.ToString() ?? "";
            set
            {
                if (double.TryParse(value, out var parsedValue))
                {
                    Violation!.ViolationFees!.FirstOffense = parsedValue;
                }
            }
        }
        private string SecondOffense
        {
            get => Violation!.ViolationFees!.SecondOffense.ToString();
            set
            {
                if (double.TryParse(value, out var parsedValue))
                {
                    Violation!.ViolationFees!.SecondOffense = parsedValue;
                }
            }
        }
        private string ThirdOffense
        {
            get => Violation!.ViolationFees.ThirdOffense.ToString();
            set
            {
                if (double.TryParse(value, out var parsedValue))
                {
                    Violation!.ViolationFees.ThirdOffense = parsedValue;
                }
            }
        }

        public ViolationCategoryDto? ViolationCategory { get; set; }
        public List<ViolationDto> ViolationList { get; set; } = new();
        public ViolationDto? Violation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViolationList = await violationClient.GetAllRequest(await tokenProvider.GetTokenAsync());
        }

        private void AddViolation()
        {
            Violation = new();
            IsSaveViolation = true;
            ViewViolationPopup = true;
        }

        private async Task SaveViolation()
        {
            //validate the violation Category if existed not add to db, if not add to category

            await violationClient.PostRequest(Violation!, await tokenProvider.GetTokenAsync());
            Thread.Sleep(1000);
            ViolationList = await violationClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Save Success",
                Icon = SweetAlertIcon.Success
            });

            IsSaveViolation = false;
            ViewViolationPopup = false;
        }

        private async Task UpdateViolation()
        {
            await violationClient.PutRequest(Violation!, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });
            ViewViolationPopup = false;
        }

        private async Task DeleteViolation()
        {
            var delete = await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Warning,
                Title = "Do you want to delete area?",
                ShowDenyButton = true,
                ConfirmButtonText = "Delete",
                DenyButtonText = "Cancel"
            });

            if (delete.IsConfirmed)
            {
                await violationClient.DeleteRequest(Violation!.Id!, await tokenProvider.GetTokenAsync());
                ViolationList.Remove(Violation!);
                ViewViolationPopup = false;
            }
        }

        private void ViewViolation(ViolationDto req)
        {
            Violation = req;
            FirstOffense = req.ViolationFees!.FirstOffense.ToString();
            SecondOffense = req.ViolationFees!.SecondOffense.ToString();
            ThirdOffense = req.ViolationFees!.ThirdOffense.ToString();
            ViewViolationPopup = true;
        }

        private void CloseViewViolation()
        {
            IsSaveViolation = false;
            ViewViolationPopup = false;
        }
    }
}