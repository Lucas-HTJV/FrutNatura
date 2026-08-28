using FrutNatura.Desktop.Api;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FrutNatura.Desktop.Forms
{
    public class AtribuirResponsavelForm : Form, IDisposable
    {
        public Guid ResponsavelId { get; private set; }

        private ComboBox cmbResponsaveis;
        private Button btnAtribuir;
        private readonly ApiClient _apiClient;

        public AtribuirResponsavelForm(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            CarregarResponsaveis();
        }

       

        // Método para inicializar os componentes do formulário
        private void InitializeComponent()
        {
            
            cmbResponsaveis = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200, 
                Left = 20,
                Top = 20
            };

           
            btnAtribuir = new Button()
            {
                Text = "Atribuir",
                Width = 100,  
                Left = 20,
                Top = 60
            };

            btnAtribuir.Click += BtnAtribuir_Click;

            
            Controls.Add(cmbResponsaveis);
            Controls.Add(btnAtribuir);

            this.Text = "Atribuir Responsável";
            this.Size = new System.Drawing.Size(300, 150); 
        }


        private void CarregarResponsaveis()
        {
            // Limpar os itens anteriores
            cmbResponsaveis.Items.Clear();

            // Aqui, você deve carregar os responsáveis do backend ou de uma lista interna
            cmbResponsaveis.Items.Add("Atendente 1");
            cmbResponsaveis.Items.Add("Atendente 2");

            if (cmbResponsaveis.Items.Count > 0)
                cmbResponsaveis.SelectedIndex = 0;  // Seleciona o primeiro item
        }


        // Evento de clique no botão "Atribuir"
        // Arquivo: Desktop/Forms/AtribuirResponsavelForm.cs
        private async void BtnAtribuir_Click(object sender, EventArgs e)
        {
            if (cmbResponsaveis.SelectedItem != null)
            {
                var responsavelNome = cmbResponsaveis.SelectedItem.ToString();

                // ✅ Correto:
                var responsavelId = await ObterResponsavelId(responsavelNome);

                ResponsavelId = responsavelId;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Selecione um responsável para atribuir o chamado.");
            }
        }


        private async Task<Guid> ObterResponsavelId(string nomeResponsavel)
        {
            var responsavelId = await _apiClient.ObterIdResponsavelPorNome(nomeResponsavel);
            return responsavelId;
        }


    }
}
