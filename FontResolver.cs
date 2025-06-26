using PdfSharp.Fonts;
using System.IO;

namespace OfiPecas
{
    // Implementação personalizada de IFontResolver para ajudar a biblioteca PDFsharp
    // a localizar ficheiros de fonte no sistema operativo.
    public class FontResolver : IFontResolver
    {
        // Devolve informação sobre o tipo de letra.
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Retorna o nome da família da fonte para a biblioteca processar.
            return new FontResolverInfo(familyName);
        }

        // Devolve os bytes do ficheiro de fonte correspondente.
        public byte[] GetFont(string faceName)
        {
            // Constrói o caminho para o ficheiro .ttf na pasta de fontes do Windows.
            var fontPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), faceName + ".ttf");

            // Se o ficheiro existir, lê e retorna o seu conteúdo em bytes.
            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }

            // Fallback para uma fonte comum caso a primeira não seja encontrada.
            fontPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), "verdana.ttf");
            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }

            // Retorna nulo se nenhuma fonte for encontrada.
            return null;
        }
    }
}
