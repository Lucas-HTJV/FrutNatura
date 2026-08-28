using System;
using System.Drawing;
using System.Windows.Forms;

namespace FrutNatura.Desktop.Forms
{
    public partial class ConversasForm : System.Windows.Forms.Form
    {
        public ConversasForm()
        {
           
            CriarLayoutConversas();
        }

        private void CriarLayoutConversas()
        {
            Color VerdePrincipal = Color.FromArgb(47, 143, 63);
            Color FundoClaro = Color.FromArgb(247, 255, 247);
            Color TextoEscuro = Color.FromArgb(35, 53, 35);

            this.Text = "Conversas - FrutNatura";
            this.BackColor = FundoClaro;
            this.Size = new Size(900, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Label lblTitulo = new Label
            {
                Text = "Conversas com Clientes",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = TextoEscuro,
                AutoSize = true,
                Location = new Point(30, 30)
            };
            this.Controls.Add(lblTitulo);

            TextBox txtChat = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(820, 350),
                Location = new Point(30, 80),
                BackColor = Color.White
            };
            this.Controls.Add(txtChat);

            TextBox txtMensagem = new TextBox
            {
                Size = new Size(650, 30),
                Location = new Point(30, 450)
            };
            this.Controls.Add(txtMensagem);

            Button btnEnviar = new Button
            {
                Text = "Enviar",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = VerdePrincipal,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 30),
                Location = new Point(700, 450)
            };
            btnEnviar.FlatAppearance.BorderSize = 0;
            btnEnviar.Click += (s, e) => MessageBox.Show("Mensagem enviada!");
            this.Controls.Add(btnEnviar);
        }
    }
}
