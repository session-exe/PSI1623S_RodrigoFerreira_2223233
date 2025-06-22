using PdfSharp.Fonts;
using System.IO;

namespace OfiPecas
{
    // Esta classe ensina a biblioteca PDFsharp a encontrar as fontes do Windows.
    public class FontResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // O estilo é determinado pela biblioteca com base nos dados da fonte.
            // Apenas precisamos de devolver o nome da família.
            return new FontResolverInfo(familyName);
        }

        // Devolve os bytes do ficheiro da fonte.
        public byte[] GetFont(string faceName)
        {
            // Tenta carregar a fonte a partir da pasta de fontes do Windows.
            // O faceName será algo como "Arial".
            var fontPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), faceName + ".ttf");

            // Se o ficheiro da fonte existir, lê e devolve os seus bytes.
            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }

            // Se "Arial.ttf" não for encontrado (raro), tenta com "Verdana.ttf" como fallback.
            fontPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), "verdana.ttf");
            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }

            // Se nenhuma fonte for encontrada, devolve nulo (a biblioteca tratará do erro).
            return null;
        }
    }
}
