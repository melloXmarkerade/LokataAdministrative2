using CurrieTechnologies.Razor.SweetAlert2;
using LokataAdministrative2.Models.Users;

namespace LokataAdministrative2.Pages.Login
{
    public partial class Login
    {
        readonly AdminLogin login = new();

        private async Task SubmitCredentials()
        {
            var result = await adminAuthClient.LoginPostRequest(login);

            if (result.ToString() is "Admin Not Found" or "Wrong password.")
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = result as string,
                    Icon = SweetAlertIcon.Error,
                    ShowCloseButton = true
                });

                return;
            }

            if (result.ToString() is "Pending Approval.")
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = result as string,
                    Icon = SweetAlertIcon.Info,
                    ShowCloseButton = true
                });

                return;
            }

            AdminLoginResponseDto? adminResponse = result as AdminLoginResponseDto;
            adminStateService.AdminResponse = adminResponse;

            await tokenProvider.SetTokenAsync(adminResponse!.Token);
            navigation.NavigateTo("/index");
        }

        private void NavigateToSignUp() => navigation.NavigateTo("/signup");
    }
}