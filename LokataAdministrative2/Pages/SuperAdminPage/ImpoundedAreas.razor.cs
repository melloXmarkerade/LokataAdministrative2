using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models.Citation;

namespace LokataAdministrative2.Pages.SuperAdminPage
{
    public partial class ImpoundedAreas
    {
        bool ViewAreaPopup = false;
        bool SaveImpoundArea = false;
        public List<ImpoundedAreaDto> ImpoundAreas { get; set; } = new();
        public ImpoundedAreaDto? Area { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var token = await tokenProvider.GetTokenAsync();
            ImpoundAreas = await impoundingClient.GetAllRequest(token);
        }

        private void AddImpoundedArea()
        {
            Area = new();
            SaveImpoundArea = true;
            ViewAreaPopup = true;
        }

        private async Task SaveArea()
        {
            await impoundingClient.PostRequest(Area!, await tokenProvider.GetTokenAsync());
            Thread.Sleep(1000);
            ImpoundAreas = await impoundingClient.GetAllRequest(await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Save Success",
                Icon = SweetAlertIcon.Success
            });

            SaveImpoundArea = false;
            ViewAreaPopup = false;
        }

        private async Task UpdateArea()
        {
            await impoundingClient.PutRequest(Area!, await tokenProvider.GetTokenAsync());

            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Update Success",
                Icon = SweetAlertIcon.Success
            });

            ViewAreaPopup = false;
        }

        private async Task DeleteArea()
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
                await impoundingClient.DeleteRequest(Area!.Id!, await tokenProvider.GetTokenAsync());
                ImpoundAreas.Remove(Area!);
                ViewAreaPopup = false;
            }
        }

        private void ViewArea(ImpoundedAreaDto area)
        {
            Area = area;
            ViewAreaPopup = true;
        }

        private void CloseViewArea()
        {
            SaveImpoundArea = false;
            ViewAreaPopup = false;
        }
    }
}
