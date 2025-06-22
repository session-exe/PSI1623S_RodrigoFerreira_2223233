using System.Drawing;
using System.IO;


namespace OfiPecas
{
    // Representa os dados de um utilizador para o formulário de definições
    public class UserInfo
    {
        public int Id { get; set; } // <-- ADICIONADO
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } // <-- ADICIONADO
        public string RecoveryKey { get; set; }
        public string NomeEmpresa { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
    }

    // Representa uma Peça do catálogo
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public int CategoriaId { get; set; }
        public string NomeCategoria { get; set; } // <-- PROPRIEDADE ADICIONADA
        public byte[] ImagemBytes { get; set; }

        public Image GetImagem()
        {
            if (ImagemBytes == null || ImagemBytes.Length == 0) return null;
            using (var ms = new MemoryStream(ImagemBytes)) { return Image.FromStream(ms); }
        }
    }

    // Representa uma Categoria
    public class CategoriaInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    // Representa um Item dentro do Carrinho
    public class ItemCarrinhoInfo
    {
        public int ItemId { get; set; }
        public int PecaId { get; set; }
        public string Nome { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Estoque { get; set; }
        public int Quantidade { get; set; }
        public byte[] ImagemBytes { get; set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;

        public Image GetImagem()
        {
            if (ImagemBytes == null || ImagemBytes.Length == 0) return null;
            using (var ms = new MemoryStream(ImagemBytes)) { return Image.FromStream(ms); }
        }
    }

    // Representa o resumo de uma encomenda para a lista do histórico
    public class EncomendaInfo
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public string Estado { get; set; }
    }

    // Representa um item detalhado dentro de uma encomenda (para o PDF)
    public class ItemEncomendaInfo
    {
        public string NomePeca { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }

}
