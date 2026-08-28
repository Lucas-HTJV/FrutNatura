using System;
using System.Windows.Forms;
using FrutNatura.Desktop.Api;
using FrutNatura.Desktop.Forms;

namespace FrutNatura.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var api = new ApiClient("http://localhost:5000");

            // 2) Cria o serviço de IA usando esse ApiClient
            var iaService = new IAService(api);

            // ✅ você pode definir a URL manualmente aqui:
           

            using (var lf = new LoginForm(api))
            {
                if (lf.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1(api, iaService));
                }
            }
        }
    }
}
