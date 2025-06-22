using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OfiPecas
{
    public static class PdfService
    {
        // Construtor estático para configurar o FontResolver uma única vez.
        // Isto garante que a biblioteca encontra as fontes do Windows.
        static PdfService()
        {
            GlobalFontSettings.FontResolver = new FontResolver();
        }

        public static void GerarFaturaPdf(EncomendaInfo encomenda, List<ItemEncomendaInfo> itens)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    FileName = $"Fatura_OfiPecas_{encomenda.Id}.pdf",
                    Title = "Guardar Fatura PDF"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string caminho = saveFileDialog.FileName;

                    PdfDocument document = new PdfDocument();
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // --- CORREÇÃO: Usando XFontStyleEx para a versão 6.1.0 ---
                    XFont fontTitulo = new XFont("Arial", 20, XFontStyleEx.Bold);
                    XFont fontNormal = new XFont("Arial", 10, XFontStyleEx.Regular);
                    XFont fontTabelaHeader = new XFont("Arial", 10, XFontStyleEx.Bold);

                    int y = 40; // Posição vertical

                    // Cabeçalho
                    gfx.DrawString("Fatura OfiPeças", fontTitulo, XBrushes.Black, new XRect(0, y, page.Width, 0), XStringFormats.TopCenter);
                    y += 40;
                    gfx.DrawString($"Encomenda Nº: {encomenda.Id}", fontNormal, XBrushes.Black, 40, y);
                    y += 15;
                    gfx.DrawString($"Data: {encomenda.Data:dd/MM/yyyy HH:mm}", fontNormal, XBrushes.Black, 40, y);
                    y += 15;
                    gfx.DrawString($"Estado: {encomenda.Estado}", fontNormal, XBrushes.Black, 40, y);
                    y += 40;

                    // Tabela
                    gfx.DrawString("Produto", fontTabelaHeader, XBrushes.Black, 40, y);
                    gfx.DrawString("Qtd.", fontTabelaHeader, XBrushes.Black, 350, y);
                    gfx.DrawString("Preço Unit.", fontTabelaHeader, XBrushes.Black, 400, y);
                    gfx.DrawString("Subtotal", fontTabelaHeader, XBrushes.Black, 500, y);
                    y += 5;
                    gfx.DrawLine(XPens.Black, 40, y, page.Width - 40, y);
                    y += 20;

                    foreach (var item in itens)
                    {
                        gfx.DrawString(item.NomePeca, fontNormal, XBrushes.Black, 40, y);
                        gfx.DrawString(item.Quantidade.ToString(), fontNormal, XBrushes.Black, 350, y);
                        gfx.DrawString($"{item.PrecoUnitario:C}", fontNormal, XBrushes.Black, 400, y);
                        gfx.DrawString($"{item.Subtotal:C}", fontNormal, XBrushes.Black, 500, y);
                        y += 20;
                    }

                    // Total
                    gfx.DrawLine(XPens.Black, 40, y, page.Width - 40, y);
                    y += 20;
                    gfx.DrawString($"Valor Total: {encomenda.ValorTotal:C}", fontTabelaHeader, XBrushes.Black, 450, y);

                    document.Save(caminho);
                    MessageBox.Show($"Fatura guardada com sucesso em:\n{caminho}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}