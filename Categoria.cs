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
    // Este é um UserControl reutilizável que representa um botão de categoria.
    public partial class Categoria : UserControl
    {
        // Propriedades públicas para guardar os dados da categoria.
        // O formulário principal pode aceder a estas propriedades para saber qual categoria foi clicada.
        public int CategoriaId { get; private set; }
        public string NomeCategoria { get; private set; }

        // Um evento personalizado. Quando este controlo é clicado, ele "avisa"
        // o formulário principal (a Loja) para que ele possa filtrar os produtos.
        public event EventHandler CategoriaClicked;

        // O construtor é o método chamado quando criamos um novo 'Categoria'.
        // Ele recebe o ID e o nome para configurar o controlo.
        public Categoria(int id, string nome)
        {
            InitializeComponent();

            // Atribui os valores recebidos às propriedades.
            this.CategoriaId = id;
            this.NomeCategoria = nome;
            // Define o texto da label visível para o utilizador.
            this.lblCategoria.Text = nome;

            // Liga o evento de clique do próprio UserControl e da sua label
            // a um único método, para garantir que toda a área é clicável.
            this.Click += OnControlClick;
            this.lblCategoria.Click += OnControlClick;
        }

        // Este método é executado sempre que o controlo é clicado.
        private void OnControlClick(object sender, EventArgs e)
        {
            // Dispara o evento 'CategoriaClicked' para notificar o formulário da Loja.
            // O '?' verifica se alguém está a "ouvir" o evento antes de o disparar, para evitar erros.
            CategoriaClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
