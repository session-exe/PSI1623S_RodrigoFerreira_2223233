using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace OfiPecas
{
    // A classe agora chama-se Loja, como corrigiste.
    public partial class Loja : Form
    {
        // Campos para guardar a informação do utilizador que fez login
        private readonly int _userId;
        private readonly bool _isAdmin;

        public Loja(int userId, bool isAdmin)
        {
            InitializeComponent();
            _userId = userId;
            _isAdmin = isAdmin;

            // Esconde o botão de admin se o utilizador não for administrador (usa o ID correto do botão)
            // ImageButton_Admin.Visible = _isAdmin;

            // Associar o evento Load do formulário a um método nosso
            this.Load += Loja_Load;
        }

        /// <summary>
        /// Este método é chamado assim que o formulário é carregado.
        /// </summary>
        private void Loja_Load(object sender, EventArgs e)
        {
            // Carrega todos os produtos por defeito quando a loja abre
            CarregarPecas();
        }

        /// <summary>
        /// Método central para carregar e exibir as peças no FlowLayoutPanel.
        /// </summary>
        private void CarregarPecas(List<Peca> pecas = null)
        {
            // Limpa os controlos antigos antes de adicionar novos (usa o ID correto do painel)
            flowLayoutPanel_Produtos.Controls.Clear();

            // Se não for passada uma lista de peças, vai buscar todas à base de dados
            if (pecas == null)
            {
                pecas = StoreService.GetPecas();
            }

            if (pecas.Count == 0)
            {
                // Opcional: Mostrar uma mensagem se nenhuma peça for encontrada
                Label lblEmpty = new Label();
                lblEmpty.Text = "Nenhuma peça encontrada.";
                lblEmpty.AutoSize = true;
                lblEmpty.Font = new System.Drawing.Font("Segoe UI", 12F);
                lblEmpty.ForeColor = System.Drawing.Color.White;
                flowLayoutPanel_Produtos.Controls.Add(lblEmpty);
                return;
            }

            // Itera por cada peça na lista
            foreach (var peca in pecas)
            {
                // Cria um novo ProdutoCard com os dados da peça
                var card = new ProdutoCard(
                    peca.Id,
                    peca.Nome,
                    peca.Preco,
                    peca.Estoque,
                    peca.GetImagem(), // Converte os bytes da imagem para um objeto Image
                    _userId
                );

                // Adiciona o card ao painel (usa o ID correto do painel)
                flowLayoutPanel_Produtos.Controls.Add(card);
            }
        }

        /// <summary>
        /// Evento de clique do botão de pesquisa.
        /// </summary>
        private void ImageButton_Pesquisa_Click(object sender, EventArgs e)
        {
            // Usa o ID correto da caixa de texto
            string termoPesquisa = TextBox_Pesquisa.Text.Trim();

            // Chama o método de pesquisa do nosso serviço
            var pecasEncontradas = StoreService.PesquisarPecas(termoPesquisa);

            // Recarrega o painel de produtos com os resultados da pesquisa
            CarregarPecas(pecasEncontradas);
        }

        // --- Funções dos outros botões (a implementar) ---

        private void ImageButton_Admin_Click(object sender, EventArgs e)
        {
            // TODO: Abrir o painel de administração
        }

        private void ImageButton_SidePanel_Click(object sender, EventArgs e)
        {
            // Lógica para mostrar/esconder a barra de categorias (usa o ID correto do painel)
            flowLayoutPanel_Sidebar.Visible = !flowLayoutPanel_Sidebar.Visible;
        }

        private void ImageButton_Cart_Click(object sender, EventArgs e)
        {
            // TODO: Abrir o formulário/painel do carrinho
        }

        private void ImageButton_Settings_Click(object sender, EventArgs e)
        {
            // TODO: Abrir as definições do utilizador
        }
    }
}
