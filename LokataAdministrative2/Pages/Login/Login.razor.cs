using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.Login
{
    public partial class Login
    {
        AdminLogin login = new();

        private async Task SubmitCredentials()
        {
            var result = await adminAuthClient.LoginPostRequest(login);

            if (result.ToString() is "Admin Not Found" or "Wrong password.")
            {
                var errorNotif = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = result,
                    Icon = SweetAlertIcon.Error,
                    ShowCloseButton = true
                });

                return;
            }

            if (result.ToString() is "Pending Approval.")
            {
                var errorNotif = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = result,
                    Icon = SweetAlertIcon.Info,
                    ShowCloseButton = true
                });

                return;
            }

            await tokenProvider.SetTokenAsync(result);
            navigation.NavigateTo("/index");
        }

        private void NavigateToSignUp() => navigation.NavigateTo("/signup");
    }
}