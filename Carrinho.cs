using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfiPecas
{
    public partial class Carrinho : Form
    {
        private readonly int _userId;

        // O construtor agora precisa de receber o ID do utilizador
        public Carrinho(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // Define o auto-scroll no painel de produtos
            flowPanel_Produtos.AutoScroll = true;

            // Associa o evento Load do formulário ao nosso método
            this.Load += Carrinho_Load;
        }

        private void Carrinho_Load(object sender, EventArgs e)
        {
            CarregarItensDoCarrinho();
        }

        private void CarregarItensDoCarrinho()
        {
            // Limpa o painel antes de adicionar os itens
            flowPanel_Produtos.Controls.Clear();

            // Vai buscar os itens do carrinho à base de dados
            var itens = StoreService.GetItensDoCarrinho(_userId);

            if (itens.Count == 0)
            {
                Label lblVazio = new Label
                {
                    Text = "O seu carrinho está vazio.",
                    AutoSize = true,
                    Font = new System.Drawing.Font("Segoe UI", 12F),
                    ForeColor = System.Drawing.Color.White,
                    Margin = new Padding(10)
                };
                flowPanel_Produtos.Controls.Add(lblVazio);
                Button_Comprar.Enabled = false; // Desativa o botão de compra se não houver itens
            }
            else
            {
                foreach (var item in itens)
                {
                    // Cria um novo UserControl para cada item
                    var itemControl = new ItemCarrinho(item);

                    // Subscreve aos eventos do UserControl
                    itemControl.RemoverClicked += ItemControl_RemoverClicked;
                    itemControl.QuantidadeChanged += ItemControl_QuantidadeChanged;

                    flowPanel_Produtos.Controls.Add(itemControl);
                }
                Button_Comprar.Enabled = true;
            }

            // Atualiza os totais
            AtualizarTotais();
        }

        private void AtualizarTotais()
        {
            int totalProdutos = 0;
            decimal valorTotal = 0;

            // Itera por todos os controlos de item no painel
            foreach (ItemCarrinho itemControl in flowPanel_Produtos.Controls.OfType<ItemCarrinho>())
            {
                // Acede aos controlos internos para obter a informação
                int quantidade = (int)itemControl.Controls.Find("NumUpDwn_Quantidade", true).FirstOrDefault().GetType().GetProperty("Value").GetValue(itemControl.Controls.Find("NumUpDwn_Quantidade", true).FirstOrDefault());

                // Extrai o preço do texto da label (ex: "12,00 €")
                string precoTexto = itemControl.Controls.Find("lblPreco", true).FirstOrDefault().Text;
                decimal preco = decimal.Parse(precoTexto.Replace("€", "").Trim());

                totalProdutos += quantidade;
                valorTotal += preco * quantidade;
            }

            lbl_Total_Produtos.Text = $"Total de Produtos: {totalProdutos}";
            lbl_Total.Text = $"Total: {valorTotal:C}";
        }

        // --- Eventos dos Controlos Dinâmicos ---

        private void ItemControl_QuantidadeChanged(object sender, EventArgs e)
        {
            if (sender is ItemCarrinho itemControl)
            {
                // Acede ao NumericUpDown para obter a nova quantidade
                var numericUpDown = itemControl.Controls.Find("NumUpDwn_Quantidade", true).FirstOrDefault() as Guna.UI2.WinForms.Guna2NumericUpDown;
                if (numericUpDown != null)
                {
                    StoreService.AtualizarQuantidadeItem(itemControl.ItemId, (int)numericUpDown.Value);
                    AtualizarTotais(); // Apenas atualiza os totais, não recarrega tudo
                }
            }
        }

        private void ItemControl_RemoverClicked(object sender, EventArgs e)
        {
            if (sender is ItemCarrinho itemControl)
            {
                var confirmResult = MessageBox.Show("Tem a certeza que quer remover este item do carrinho?", "Confirmar Remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    StoreService.RemoverItemDoCarrinho(itemControl.ItemId);
                    CarregarItensDoCarrinho(); // Recarrega o carrinho para remover o controlo
                }
            }
        }

        // --- Evento do Botão de Compra ---

        private void Button_Comprar_Click(object sender, EventArgs e)
        {
            var (success, message) = StoreService.FinalizarEncomenda(_userId);

            MessageBox.Show(message, success ? "Sucesso" : "Erro", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                // Fecha o formulário do carrinho após a encomenda ser bem-sucedida
                this.Close();
            }
        }
    }
}