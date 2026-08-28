using System;
using System.Windows.Forms;
using FrutNatura.Form;
using FrutNatura.Desktop.Forms.Login;
using FrutNatura.Desktop.Forms.Chamados;
using FrutNatura.Desktop.Models;

namespace FrutNatura.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var api = new ApiClient { BaseUrl = "https://localhost:7094" };

            using var login = new LoginForm(api);
            if (login.ShowDialog() == DialogResult.OK)
            {
                var token = api.Token!;
                var userIdStr = Jwt.GetClaim(token, "sub") ?? Jwt.GetClaim(token, "userId");
                var email = Jwt.GetClaim(token, "email") ?? login.LoggedUserEmail;
                var role = Jwt.GetClaim(token, "role") ?? Jwt.GetClaim(token, "roles");

                Guid.TryParse(userIdStr, out var userId);
                Application.Run(new MainForm(api, email, userId));
            }
        }
    }
}
