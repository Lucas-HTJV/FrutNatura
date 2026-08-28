using FrutNatura.Desktop.Api;
using FrutNatura.Desktop.Models;
using FrutNatura.Desktop.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrutNatura.Desktop.Forms
{
    public partial class Form1 : Form
    {
        // =========================================================
        // SEÇÃO: CAMPOS / DEPENDÊNCIAS
        // =========================================================

        private readonly ApiClient _apiClient;
        private readonly IAService _iaService;

        // UI principal
        private Panel? panelMenu;
        private Panel? panelHeader;
        private Panel? panelConteudo;

        private Label? lblUser;
        private Label? lblRole;
        private Label? lblTitulo;

        // Lista de chamados (lado direito)
        private FlowLayoutPanel? pnlChamados;

        // Chat
        private FlowLayoutPanel pnlMensagens;
        private TextBox? txtMensagem;
        private Button? btnEnviar;

        // Atribuição de responsável (não usada diretamente na tela principal,
        // mas deixei para possíveis telas futuras)
        private ComboBox? cmbResponsavel;
        private Button? btnAtribuir;

        // Estado atual
        private ChamadoDto? _chamadoAtual;

        // Cores do tema
        private readonly Color textoEscuro = Color.FromArgb(33, 37, 41);
        private readonly Color verdePrincipal = Color.FromArgb(47, 143, 63);
        private readonly Color fundoClaro = Color.FromArgb(247, 250, 247);

        //Atualização programada
        private System.Windows.Forms.Timer? _timerMensagens;
        private bool _atualizandoMensagens = false;
        private List<Guid> _idsAnteriores = new();

        // =========================================================
        // SEÇÃO: CONSTRUTOR
        // =========================================================

        public Form1(ApiClient apiClient, IAService iaService)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _iaService = iaService;

            InitializeCustomComponents();
            CriarLayoutFrutNatura();   

            // Painel onde serão exibidas as mensagens do chat
            pnlMensagens = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(5),
                Margin = new Padding(0),
                BackColor = Color.White
            };
            _timerMensagens = new System.Windows.Forms.Timer();
            _timerMensagens.Interval = 5000; 
            _timerMensagens.Tick += async (s, e) => await TimerMensagens_Tick();

            
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // =========================================================
        // SEÇÃO: INICIALIZAÇÃO DE CONTROLES (NÃO VISUAL)
        // =========================================================

        private void InitializeCustomComponents()
        {
            // Lista de chamados (será colocada dentro do panelConteudo em MostrarChamados)
            pnlChamados = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Dock = DockStyle.Fill
            };

            // Caixa de texto utilizada para o chat
            txtMensagem = new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle
            };

            // Botão enviar utilizado no chat
            btnEnviar = new Button
            {
                Text = "Enviar",
                Width = 90,
                Height = 30
            };
            btnEnviar.Click += BtnEnviar_Click;

            // IMPORTANTE: aqui NÃO adicionamos nada no Form ainda.
            // Quem decide onde cada controle entra é o layout (CriarLayoutFrutNatura / MostrarTelaChatDoChamado / MostrarChamados).
        }

        // =========================================================
        // SEÇÃO: EVENTOS DO FORM
        // =========================================================

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Quando o form carrega, mostramos a tela de chamados
            await CarregarChamados();
        }

        // =========================================================
        // SEÇÃO: OPERAÇÕES DE CHAMADOS (LISTAGEM, CARREGAR, ATRIBUIR)
        // =========================================================

        private async Task CarregarChamados()
        {
            try
            {
                if (pnlChamados == null)
                    return;

                var chamados = await _apiClient.ListarChamadosAsync();

                pnlChamados.Controls.Clear();

                foreach (var chamado in chamados)
                    AdicionarChamadoNaInterface(chamado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar chamados: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AdicionarChamadoNaInterface(ChamadoDto chamado)
        {
            if (pnlChamados == null)
                return;

            var painelChamado = new Panel
            {
                Size = new Size(550, 80),
                Padding = new Padding(5),
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var lblTituloChamado = new Label
            {
                Text = chamado.Titulo,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(5, 5),
                AutoSize = true
            };

            var lblDescricao = new Label
            {
                Text = chamado.Descricao,
                Font = new Font("Segoe UI", 9),
                Location = new Point(5, 25),
                AutoSize = true,
                MaximumSize = new Size(500, 0)
            };

            var corStatus =
                chamado.Status == "Pendente" ? Color.Orange :
                chamado.Status == "Realizado" ? Color.Green :
                Color.Red;

            var lblStatus = new Label
            {
                Text = "Status: " + chamado.Status,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = corStatus,
                Location = new Point(5, 45),
                AutoSize = true
            };

            var btnAtribuir = new Button
            {
                Text = "Atribuir a mim",
                Location = new Point(300, 5),
                Size = new Size(120, 30),
                BackColor = verdePrincipal,
                ForeColor = Color.White
            };

            btnAtribuir.Tag = chamado;
            btnAtribuir.Click += async (sender, e) => await AtribuirEabrirChatAsync(chamado);

            painelChamado.Controls.Add(lblTituloChamado);
            painelChamado.Controls.Add(lblDescricao);
            painelChamado.Controls.Add(lblStatus);
            painelChamado.Controls.Add(btnAtribuir);

            pnlChamados.Controls.Add(painelChamado);
        }

        private async Task AtribuirResponsavel(Guid chamadoId, Guid responsavelId)
        {
            try
            {
                await _apiClient.AtribuirResponsavelAsync(chamadoId, responsavelId);
                MessageBox.Show("Responsável atribuído com sucesso!");
                await CarregarChamados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atribuir responsável: {ex.Message}");
            }
        }

        private async Task AtribuirEabrirChatAsync(ChamadoDto chamado)
        {
            try
            {
                // 1) Atribui o chamado ao usuário logado
                if (chamado.ResponsavelId != SessionManager.UsuarioId)
                {
                    await AtribuirResponsavel(chamado.Id, SessionManager.UsuarioId);
                   
                }

                // 2) Guarda o chamado atual
                _chamadoAtual = chamado;

                // 3) Monta a tela de chat
                MostrarTelaChatDoChamado(chamado);

                // 4) Carrega as mensagens existentes
                await AtualizarMensagens(chamado.Id);
                _timerMensagens?.Start();


                // 5) Pegamos a última mensagem do cliente
                var mensagens = await _apiClient.ObterMensagensAsync(chamado.Id);

                string mensagemAtual = chamado.Descricao; // fallback (1ª mensagem)

                if (mensagens != null && mensagens.Count > 0)
                {
                    // pega a última mensagem que NÃO é do atendente logado
                    var ultimaMensagemCliente = mensagens
                        .FindLast(m => m.AutorId != SessionManager.UsuarioId);

                    if (ultimaMensagemCliente != null &&
                        !string.IsNullOrWhiteSpace(ultimaMensagemCliente.Texto))
                    {
                        mensagemAtual = ultimaMensagemCliente.Texto;
                    }
                }

                // 6) Agora sim a IA recebe o ID + a ÚLTIMA mensagem correta
                var sugestao = GerarSugestaoIaParaChamado(chamado);

              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atribuir e abrir chat: {ex.Message}");
            }
        }

        // =========================================================
        // SEÇÃO: TELA DE CHAT
        // =========================================================

        private void MostrarTelaChatDoChamado(ChamadoDto chamado)
        {
            if (panelConteudo == null || txtMensagem == null || btnEnviar == null)
                return;

            panelConteudo.Controls.Clear();

            // Cabeçalho (título + status)
            var lblCabecalho = new Label
            {
                Text = $"{chamado.Titulo}  -  Status: {chamado.Status}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = textoEscuro,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            panelConteudo.Controls.Add(lblCabecalho);

            // Caixa do chat (onde ficam as mensagens)
            var panelChatBox = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(10, lblCabecalho.Bottom + 10),
                Size = new Size(panelConteudo.Width - 50, panelConteudo.Height - 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            panelConteudo.Controls.Add(panelChatBox);

            // Painel de mensagens dentro da caixa
            pnlMensagens.Location = new Point(10, 10);
            pnlMensagens.Size = new Size(panelChatBox.Width - 20, panelChatBox.Height - 20);
            pnlMensagens.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMensagens.AutoScroll = true;

            if (pnlMensagens.Parent != panelChatBox)
                panelChatBox.Controls.Add(pnlMensagens);

            // Barra inferior (mensagem + botão Enviar)
            var panelBarraInferior = new Panel
            {
                Height = 40,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(235, 235, 235)
            };
            panelConteudo.Controls.Add(panelBarraInferior);
            panelBarraInferior.BringToFront();

            // TextBox de mensagem
            txtMensagem.BorderStyle = BorderStyle.None;
            txtMensagem.Location = new Point(10, 10);
            txtMensagem.Size = new Size(panelBarraInferior.Width - 120, 20);
            txtMensagem.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            if (txtMensagem.Parent != panelBarraInferior)
                panelBarraInferior.Controls.Add(txtMensagem);

            // Botão Enviar
            btnEnviar.Text = "Enviar";
            btnEnviar.Size = new Size(80, 24);
            btnEnviar.Location = new Point(panelBarraInferior.Width - btnEnviar.Width - 10, 8);
            btnEnviar.Anchor = AnchorStyles.Right | AnchorStyles.Top;

            if (btnEnviar.Parent != panelBarraInferior)
                panelBarraInferior.Controls.Add(btnEnviar);
        }

        private async void BtnEnviar_Click(object? sender, EventArgs e)
        {
            if (txtMensagem == null || _chamadoAtual == null)
            {
                MessageBox.Show("Nenhum chamado selecionado para enviar mensagem.");
                return;
            }

            var mensagem = txtMensagem.Text.Trim();
            if (string.IsNullOrWhiteSpace(mensagem))
                return;

            try
            {
                var request = new NovaMensagemRequest
                {
                    // envia o autorId (usuário logado)
                    AutorId = SessionManager.UsuarioId,
                    Conteudo = mensagem,
                    VisivelParaCliente = true
                };

                await _apiClient.EnviarMensagemAsync(_chamadoAtual.Id, request);
                await AtualizarMensagens(_chamadoAtual.Id); 
                var respostaIa = await _iaService.ObterRespostaIAAsync(_chamadoAtual.Id, mensagem);

                txtMensagem.Text = respostaIa;


                txtMensagem.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar mensagem: {ex.Message}");
            }
        }



        // ARQUIVO: FrutNatura.Desktop/Forms/Form1.cs
        private async Task AtualizarMensagens(Guid chamadoId)
        {
            var mensagens = await _apiClient.ObterMensagensAsync(chamadoId)
                           ?? new List<MensagemDto>();

            pnlMensagens.Controls.Clear();

            foreach (var msg in mensagens)
            {
                // Descobre se foi o atendente (usuário logado) ou o cliente
                bool ehAtendente = msg.AutorId == SessionManager.UsuarioId;
                string autor = ehAtendente ? "Atendente" : "Cliente";

                var painelMensagem = new Panel
                {
                    Width = pnlMensagens.ClientSize.Width - 25,
                    AutoSize = true,
                    Padding = new Padding(8),
                    Margin = new Padding(0, 0, 0, 8),
                    BackColor = ehAtendente
                        ? Color.FromArgb(230, 255, 230) // verdinho para atendente
                        : Color.White                    // branco para cliente
                };

                var lblAutor = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = textoEscuro,
                    Text = autor,
                    Location = new Point(0, 0)
                };


                
                var texto = string.IsNullOrWhiteSpace(msg.Texto)
                    ? "(sem conteúdo)"
                    : msg.Texto;



                var lblMensagem = new Label
                {
                    AutoSize = true,
                    MaximumSize = new Size(painelMensagem.Width - 10, 0),
                    Text = texto,
                    Location = new Point(0, lblAutor.Bottom + 2)
                };

                painelMensagem.Controls.Add(lblAutor);
                painelMensagem.Controls.Add(lblMensagem);

                pnlMensagens.Controls.Add(painelMensagem);
            }

            pnlMensagens.Refresh();
            if (pnlMensagens.Controls.Count > 0)
            {
                var ultimo = pnlMensagens.Controls[pnlMensagens.Controls.Count - 1];
                pnlMensagens.ScrollControlIntoView(ultimo);
            }
        }

        private async Task GerarSugestaoIaParaChamado(ChamadoDto chamado)
        {
            try
            {
                // Busca as mensagens atuais do chamado
                var mensagens = await _apiClient.ObterMensagensAsync(chamado.Id);

                // Fallback: se não tiver mensagens, usa a descrição inicial
                string textoParaIa = chamado.Descricao;

                if (mensagens != null && mensagens.Count > 0)
                {
                    // Última mensagem que NÃO é do atendente (ou seja, do cliente)
                    var ultimaDoCliente = mensagens.FindLast(m => m.AutorId != SessionManager.UsuarioId);

                    if (ultimaDoCliente != null && !string.IsNullOrWhiteSpace(ultimaDoCliente.Texto))
                    {
                        textoParaIa = ultimaDoCliente.Texto;
                    }
                }

                // Chama a IA passando: id do chamado + última mensagem do cliente
                var sugestao = await _iaService.ObterRespostaIAAsync(chamado.Id, textoParaIa);

                if (txtMensagem != null && !string.IsNullOrWhiteSpace(sugestao))
                {
                    txtMensagem.Text = sugestao;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar sugestão da IA: {ex.Message}");
                // não precisa mostrar MessageBox aqui pra não incomodar o atendente
            }
        }




        // =========================================================
        // SEÇÃO: CHAMADA DE IA (USADA PELO SERVIÇO UTILITÁRIO)
        // =========================================================

        public async Task<string> ObterRespostaIAAsync(Guid chamadoId, string texto)
        {
            try
            {
                var resposta = await _apiClient.ChamarIAAsync(chamadoId, texto);
                return resposta ?? "Desculpe, não conseguimos obter a resposta da IA.";
            }
            catch (Exception ex)
            {
                return $"Erro ao obter resposta da IA: {ex.Message}";
            }
        }

        // =========================================================
        // SEÇÃO: ATUALIZAÇÃO 
        // =========================================================


       

        private async Task TimerMensagens_Tick()
        {
            if (_atualizandoMensagens || _chamadoAtual == null)
                return;

            try
            {
                _atualizandoMensagens = true;

                // Obtenha as mensagens atuais
                var mensagens = await _apiClient.ObterMensagensAsync(_chamadoAtual.Id);

                // Detectar chegada de nova mensagem (diferente do atendente)
                var novas = mensagens.FindAll(m =>
                    !_idsAnteriores.Contains(m.Id) &&
                    m.AutorId != SessionManager.UsuarioId
                );

                // Atualizar tela
                await AtualizarMensagens(_chamadoAtual.Id);

                // Atualiza cache
                _idsAnteriores = mensagens.ConvertAll(m => m.Id);

                // Se chegou nova mensagem do cliente → chamar IA
                if (novas.Count > 0)
                {
                    var ultimaMensagem = novas[^1].Texto;
                    var respostaIa = await _iaService.ObterRespostaIAAsync(_chamadoAtual.Id, ultimaMensagem);

                    if (txtMensagem != null)
                        txtMensagem.Text = respostaIa;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Timer IA: {ex.Message}");
            }
            finally
            {
                _atualizandoMensagens = false;
            }
        }



        // =========================================================
        // SEÇÃO: LAYOUT GERAL (MENU, HEADER, ÁREA DE CONTEÚDO)
        // =========================================================

        private void CriarLayoutFrutNatura()
        {
            // Configurações gerais da janela
            this.Text = "FrutNatura - Central de Chamados";
            this.BackColor = fundoClaro;
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            // Menu lateral
            panelMenu = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Left,
                Width = 220
            };
            this.Controls.Add(panelMenu);

            lblUser = new Label
            {
                Text = "👤 User123",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(25, 25)
            };
            lblRole = new Label
            {
                Text = "Administrador",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Location = new Point(25, 45)
            };
            panelMenu.Controls.Add(lblUser);
            panelMenu.Controls.Add(lblRole);

            // Botões do menu
            CriarBotaoMenu("📋 Chamados", 100, verdePrincipal, MostrarChamados);
            CriarBotaoMenu("💬 Conversas", 160, verdePrincipal, MostrarConversas);
            CriarBotaoMenu("📢 Avisos", 220, verdePrincipal, MostrarAvisos);
            CriarBotaoMenu("📊 Relatórios", 280, verdePrincipal, MostrarRelatorios);
            CriarBotaoMenu("⚙️ Configurações", 340, verdePrincipal, MostrarConfig);
            CriarBotaoMenu("🚪 Sair", 420, Color.FromArgb(220, 53, 69), () => this.Close());

            // Cabeçalho superior
            panelHeader = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60
            };
            this.Controls.Add(panelHeader);

            lblTitulo = new Label
            {
                Text = "Central de Chamados",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = textoEscuro,
                AutoSize = true,
                Location = new Point(250, 18)
            };
            panelHeader.Controls.Add(lblTitulo);

            // Área de conteúdo (onde a lista de chamados / chat é desenhada)
            panelConteudo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = fundoClaro,
                Padding = new Padding(25)
            };
            this.Controls.Add(panelConteudo);
            panelConteudo.BringToFront();

            // Tela inicial: chamados
            MostrarChamados();
        }

        // =========================================================
        // SEÇÃO: LAYOUT - BOTÕES DO MENU E TELAS BÁSICAS
        // =========================================================

        private void CriarBotaoMenu(string texto, int top, Color cor, Action onClick)
        {
            var btnMenu = new Button
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = cor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(25, top),
                Size = new Size(170, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.MouseEnter += (s, e) => btnMenu.BackColor = Color.FromArgb(240, 255, 240);
            btnMenu.MouseLeave += (s, e) => btnMenu.BackColor = Color.Transparent;
            btnMenu.Click += (s, e) => onClick?.Invoke();

            panelMenu?.Controls.Add(btnMenu);
        }

        private async void MostrarChamados()
        {
            _timerMensagens?.Stop();
            _chamadoAtual = null;

            if (panelConteudo == null || pnlChamados == null)
                return;

            panelConteudo.Controls.Clear();
            pnlChamados.Dock = DockStyle.Fill;
            panelConteudo.Controls.Add(pnlChamados);

            await CarregarChamados();
        }

        private void MostrarAvisos()
        {
            panelConteudo?.Controls.Clear();
            var lblAvisos = new Label
            {
                Text = "Nenhum aviso disponível no momento.",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = verdePrincipal,
                AutoSize = true,
                Location = new Point(30, 30)
            };
            panelConteudo?.Controls.Add(lblAvisos);
        }

        private void MostrarConversas()
        {
            panelConteudo?.Controls.Clear();
            var lblConversas = new Label
            {
                Text = "Nenhuma conversa disponível no momento.",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = verdePrincipal,
                AutoSize = true,
                Location = new Point(30, 30)
            };
            panelConteudo?.Controls.Add(lblConversas);
        }

        private void MostrarRelatorios()
        {
            panelConteudo?.Controls.Clear();
            var lblRelatorios = new Label
            {
                Text = "Nenhum relatório disponível no momento.",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = verdePrincipal,
                AutoSize = true,
                Location = new Point(30, 30)
            };
            panelConteudo?.Controls.Add(lblRelatorios);
        }

        private void MostrarConfig()
        {
            panelConteudo?.Controls.Clear();
            var lblConfig = new Label
            {
                Text = "Nenhuma configuração disponível no momento.",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = verdePrincipal,
                AutoSize = true,
                Location = new Point(30, 30)
            };
            panelConteudo?.Controls.Add(lblConfig);
        }
    }
}
