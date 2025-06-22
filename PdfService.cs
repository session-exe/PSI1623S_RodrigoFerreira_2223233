using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OfiPecas
{
    public static class PdfService
    {
        public static void GerarFaturaPdf(EncomendaInfo encomenda, List<ItemEncomendaInfo> itens)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    FileName = $"Fatura_OfiPecas_{encomenda.Id}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string caminho = saveFileDialog.FileName;
                    using (var writer = new PdfWriter(caminho))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            // Cabeçalho
                            document.Add(new Paragraph("Fatura OfiPecas")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(20)
                                .SimulateBold()
                            ); 

                            document.Add(new Paragraph($"Encomenda Nº: {encomenda.Id}"));
                            document.Add(new Paragraph($"Data: {encomenda.Data:dd/MM/yyyy HH:mm}"));
                            document.Add(new Paragraph($"Estado: {encomenda.Estado}"));
                            document.Add(new Paragraph("\n"));

                            // Tabela com os itens
                            Table tabela = new Table(UnitValue.CreatePercentArray(new float[] { 4, 1, 2, 2 })).UseAllAvailableWidth();
                            tabela.AddHeaderCell("Produto");
                            tabela.AddHeaderCell("Qtd.");
                            tabela.AddHeaderCell("Preço Unit.");
                            tabela.AddHeaderCell("Subtotal");

                            foreach (var item in itens)
                            {
                                tabela.AddCell(item.NomePeca);
                                tabela.AddCell(item.Quantidade.ToString());
                                tabela.AddCell($"{item.PrecoUnitario:C}");
                                tabela.AddCell($"{item.Subtotal:C}");
                            }
                            document.Add(tabela);

                            // Rodapé com o total
                            document.Add(new Paragraph($"\nValor Total: {encomenda.ValorTotal:C}")
                                .SetTextAlignment(TextAlignment.RIGHT)
                                .SetFontSize(14)
                                .SimulateBold()
                            ); 

                            document.Close();
                        }
                    }
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
