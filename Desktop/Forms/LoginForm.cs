using FrutNatura.Desktop.Api;
using FrutNatura.Desktop.Models;
using FrutNatura.Desktop.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace FrutNatura.Desktop.Forms
{
    public sealed class LoginForm : System.Windows.Forms.Form
    {
        private readonly ApiClient _api;
        private readonly TextBox txtEmail = new();
        private readonly TextBox txtSenha = new();
        private readonly Button btnLogin = new();
        private readonly Label lblErro = new();

        private void InitializeComponent()
        {

        }

        public LoginForm(ApiClient api)
        {
            _api = api;

            Text = "Login - FrutNatura";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(360, 220);

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 2,
                RowCount = 4
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 3; i++) grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(grid);

            grid.Controls.Add(new Label { Text = "E-mail:", TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill }, 0, 0);
            txtEmail.Dock = DockStyle.Fill; txtEmail.PlaceholderText = "usuario@dominio.com";
            grid.Controls.Add(txtEmail, 1, 0);

            grid.Controls.Add(new Label { Text = "Senha:", TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill }, 0, 1);
            txtSenha.Dock = DockStyle.Fill; txtSenha.UseSystemPasswordChar = true;
            grid.Controls.Add(txtSenha, 1, 1);

            btnLogin.Text = "Entrar"; btnLogin.Dock = DockStyle.Fill; btnLogin.Height = 34;
            grid.Controls.Add(btnLogin, 1, 2);

            lblErro.ForeColor = Color.Maroon; lblErro.Visible = false; lblErro.Dock = DockStyle.Fill;
            grid.SetColumnSpan(lblErro, 2);
            grid.Controls.Add(lblErro, 0, 3);

            btnLogin.Click += async (_, __) =>
            {
                try
                {
                    btnLogin.Enabled = false; lblErro.Visible = false;

                    var resp = await _api.LoginAsync(new LoginRequest
                    {
                        Email = txtEmail.Text.Trim(),
                        HashPassword = txtSenha.Text
                    });

                    if (resp is null || string.IsNullOrWhiteSpace(resp.AccessToken) || resp.Success == false)
                    {
                        lblErro.Text = resp?.Error ?? "Credenciais inválidas.";
                        lblErro.Visible = true;
                        return;
                    }

                    SessionManager.UserName = resp.Name;
                    SessionManager.UserRole = resp.Role;

                    DialogResult = DialogResult.OK;

                    
                    Close();
                }
                catch (Exception ex)
                {
                    lblErro.Text = ex.Message;
                    lblErro.Visible = true;
                }
                finally
                {
                    btnLogin.Enabled = true;
                }
            };

            txtSenha.KeyDown += async (_, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnLogin.PerformClick(); }
            };
        }
    }
}
