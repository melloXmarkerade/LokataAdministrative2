using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models;

namespace LokataAdministrative2.Pages.Login
{
    public partial class Signup
    {
        AdminSignup admin = new();

        private async Task AdminSignup()
        {
            admin.DateCreated = DateTime.Now.ToShortDateString();

            var result = await adminAuthClient.SignupPostRequest(admin);

            if(result is "Username existed.")
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