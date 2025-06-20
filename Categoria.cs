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
    public partial class Categoria : UserControl
    {
        public int CategoriaId { get; private set; }
        public string NomeCategoria { get; private set; }

        // Evento que será disparado quando o controlo for clicado
        public event EventHandler CategoriaClicked;

        // O construtor agora aceita os dados da categoria
        public Categoria(int id, string nome)
        {
            InitializeComponent();

            this.CategoriaId = id;
            this.NomeCategoria = nome;
            this.lblCategoria.Text = nome; // Define o texto da label

            // Adiciona manipuladores de clique para tornar toda a área clicável
            this.Click += OnControlClick;
            this.lblCategoria.Click += OnControlClick;
        }

        private void OnControlClick(object sender, EventArgs e)
        {
            // Dispara o evento para notificar a Loja de que esta categoria foi clicada
            CategoriaClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}