using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class Requirements
    {
        bool IsSaveRequirement = false;
        bool ViewRequirementPopup = false;
        public List<Requirement> RequirementList { get; set; } = new();
        public Requirement? Requirement { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RequirementList = await requirementClient.GetAllRequest(null!);
        }

        private void AddRequirement()
        {
            Requirement = new();
            IsSaveRequirement = true;
            ViewRequirementPopup = true;

        }

        private async Task SaveRequirement()
        {
            await requirementClient.PostRequest(Requirement!, await tokenProvider.GetTokenAsync());
            Thread.Sleep(1000);
            RequirementList = await requirementClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Save Success",
                Icon = SweetAlertIcon.Success
            });

            IsSaveRequirement = false;
            ViewRequirementPopup = false;
        }

        private async Task UpdateRequirement()
        {
            await requirementClient.PutRequest(Requirement!, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });

            ViewRequirementPopup = false;
        }

        private async Task DeleteRequirement()
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
                await requirementClient.DeleteRequest(Requirement!.Id!, await tokenProvider.GetTokenAsync());
                RequirementList.Remove(Requirement!);
                ViewRequirementPopup = false;
            }
        }

        private void ViewRequirement(Requirement req)
        {
            Requirement = req;
            ViewRequirementPopup = true;
        }

        private void CloseViewRequirement()
        {
            IsSaveRequirement = false;
            ViewRequirementPopup = false;
        }
    }
}