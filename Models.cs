using System.Drawing;
using System.IO;


namespace OfiPecas
{
    // Representa uma Peça do catálogo
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
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
}
