using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OfiPecas
{
    // Formulário principal da loja, que exibe os produtos.
    public partial class Loja : Form
    {
        // Armazena os dados do utilizador logado.
        private readonly int _userId;
        private readonly bool _isAdmin;

        // Construtor que recebe os dados do utilizador após o login.
        public Loja(int userId, bool isAdmin)
        {
            InitializeComponent();
            _userId = userId;
            _isAdmin = isAdmin;

            // Esconde o botão de administração se o utilizador não for admin.
            ImageButton_Admin.Visible = _isAdmin;

            // Associa o evento de carregamento do formulário.
            this.Load += Loja_Load;
        }

        // Método executado quando o formulário é carregado.
        private void Loja_Load(object sender, EventArgs e)
        {
            CarregarPecas();
            CarregarCategorias();
        }

        // Carrega as peças no painel principal.
        // Opcionalmente, pode receber uma lista de peças para exibir (para filtros).
        private void CarregarPecas(List<Peca> pecas = null)
        {
            flowLayoutPanel_Produtos.Controls.Clear();
            if (pecas == null)
            {
                pecas = StoreService.GetPecas();
            }
            if (pecas.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Nenhuma peça encontrada.", AutoSize = true, ForeColor = System.Drawing.Color.Gray };
                flowLayoutPanel_Produtos.Controls.Add(lblEmpty);
                return;
            }
            // Cria um UserControl ProdutoCard para cada peça.
            foreach (var peca in pecas)
            {
                var card = new ProdutoCard(peca.Id, peca.Nome, peca.Preco, peca.Estoque, peca.GetImagem(), _userId);
                // Subscreve ao evento do botão "Adicionar".
                card.AdicionarAoCarrinhoClicked += Card_AdicionarAoCarrinhoClicked;
                flowLayoutPanel_Produtos.Controls.Add(card);
            }
        }

        // Carrega as categorias na barra lateral.
        private void CarregarCategorias()
        {
            flowLayoutPanel_Sidebar.Controls.Clear();
            // Adiciona um item "Todas as Peças" para limpar o filtro.
            var todas = new Categoria(0, "Todas as Peças");
            todas.CategoriaClicked += Categoria_Clicked;
            flowLayoutPanel_Sidebar.Controls.Add(todas);

            var categorias = StoreService.GetCategorias();
            foreach (var cat in categorias)
            {
                var categoriaControl = new Categoria(cat.Id, cat.Nome);
                categoriaControl.CategoriaClicked += Categoria_Clicked;
                flowLayoutPanel_Sidebar.Controls.Add(categoriaControl);
            }
        }

        // Handler para o clique numa categoria. Filtra as peças exibidas.
        private void Categoria_Clicked(object sender, EventArgs e)
        {
            if (sender is Categoria categoriaControl)
            {
                if (categoriaControl.CategoriaId == 0)
                {
                    CarregarPecas(); // Mostra todas as peças
                }
                else
                {
                    // Mostra apenas as peças da categoria selecionada.
                    var pecasFiltradas = StoreService.GetPecasPorCategoria(categoriaControl.CategoriaId);
                    CarregarPecas(pecasFiltradas);
                }
            }
        }

        // Handler para o clique no botão "Adicionar" de um ProdutoCard.
        private void Card_AdicionarAoCarrinhoClicked(object sender, EventArgs e)
        {
            if (sender is ProdutoCard card)
            {
                // Chama o serviço para adicionar o item ao carrinho.
                var (success, message) = CartService.AdicionarAoCarrinho(_userId, card.PecaId);
                MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
        }

        // Handler para o botão de pesquisa.
        private void ImageButton_Pesquisa_Click(object sender, EventArgs e)
        {
            string termoPesquisa = TextBox_Pesquisa.Text.Trim();
            var pecasEncontradas = StoreService.PesquisarPecas(termoPesquisa);
            CarregarPecas(pecasEncontradas);
        }

        // Handler para o botão do carrinho de compras.
        private void ImageButton_Cart_Click(object sender, EventArgs e)
        {
            var carrinho = new Carrinho(_userId);
            carrinho.ShowDialog();
            // Recarrega a loja quando o carrinho é fechado para atualizar o stock.
            Loja_Load(this, EventArgs.Empty);
        }

        // Handler para o botão que mostra/esconde a barra lateral.
        private void ImageButton_SidePanel_Click(object sender, EventArgs e)
        {
            flowLayoutPanel_Sidebar.Visible = !flowLayoutPanel_Sidebar.Visible;
        }

        // Handler para o clique no logo, que termina a sessão.
        private void PictureBox_Logo_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        // Handler para o botão de histórico de encomendas.
        private void ImageButton_HistoricoEcomendas_Click(object sender, EventArgs e)
        {
            var formHistorico = new HistoricoEncomendas(_userId);
            formHistorico.ShowDialog();
        }

        // Handler para o botão de definições do utilizador.
        private void ImageButton_Settings_Click(object sender, EventArgs e)
        {
            var formDefinicoes = new DefinicoesUser(_userId);
            formDefinicoes.ShowDialog();
        }

        // Handler para o botão do painel de administração.
        private void ImageButton_Admin_Click(object sender, EventArgs e)
        {
            // Passa a permissão de admin para o formulário.
            var formAdmin = new PainelAdmin(_isAdmin);
            formAdmin.ShowDialog();

            // Recarrega a loja quando o painel de admin é fechado.
            Loja_Load(this, EventArgs.Empty);
        }
    }
}
