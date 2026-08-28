using System;
using System.Windows.Forms;
using FrutNatura.Desktop.Api;
using FrutNatura.Desktop.Models;

namespace FrutNatura.Desktop.Forms
{
    public class FormBase : System.Windows.Forms.Form
    {
        protected ApiClient ApiClient = new ApiClient();

        // Método genérico para carregar dados
        public virtual async void CarregarDados(Guid id)
        {
            try
            {
                var chamado = await ApiClient.GetChamadoAsync(id);
                PreencherFormulario(chamado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }

        // Método genérico para salvar dados
        public virtual async void SalvarDados()
        {
            try
            {
                var chamado = new ChamadoDto
                {
                    Titulo = "Novo Título", // Aqui você pega os dados do formulário
                    Descricao = "Descrição do chamado", // Preencher os campos
                    // Definir outros campos
                };

                await ApiClient.SaveChamadoAsync(chamado);
                MessageBox.Show("Chamado salvo com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar dados: {ex.Message}");
            }
        }

        // Método para preencher o formulário com os dados do chamado
        protected virtual void PreencherFormulario(ChamadoDto chamado)
        {
            // Exemplo de preenchimento dos campos com os dados
            this.Text = chamado.Titulo;
        }
    }
}
