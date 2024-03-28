using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models.Users;
using LokataAdministrative2.Services.AdminClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LokataAdministrative2.Pages.Login
{
    public partial class Signup
    {
        AdminDto admin = new();

        private async Task AdminSignup()
        {
            admin.DateCreated = DateTime.Now.ToShortDateString();

            var result = await adminAuthClient.SignupPostRequest(admin);

            if (result is not "Signup Success")
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = result,
                    Icon = SweetAlertIcon.Error
                });

                return;
            }

            var successNotif = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = result,
                Icon = SweetAlertIcon.Success
            });

            if (successNotif.IsConfirmed)
                admin = new();
        }

        private void NavigateToSignIn() => navigation.NavigateTo("/");
    }
}
