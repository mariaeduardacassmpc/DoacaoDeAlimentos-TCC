using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;
using BackDoacaoDeAlimentos.Interfaces.Repositorios;
using BackDoacaoDeAlimentos.Interfaces.Servicos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TCCDoacaoDeAlimentos.Shared.Models;
using TCCDoacaoDeAlimentos.Shared.Dto;

public class 
    : IRelatorioService
{
    private readonly IRelatorioRepositorio _relatorioRepositorio;

    public RelatorioService(IRelatorioRepositorio relatorioRepositorio)
    {
        _relatorioRepositorio = relatorioRepositorio;
    }

    public void GerarRelatorio(string caminhoArquivo, string nomeUsuario, List<DoacaoComDetalhes> doacoes, List<DoacaoComDetalhes> alimentoNome)
    {
        var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate());

        using (var stream = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            // Fonte padrão
            var fonteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            var fonteSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var fonteNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            // 1. Cabeçalho
            string titulo = "Relatório de Doações Recebidas";
            Paragraph pTitulo = new Paragraph(titulo, fonteTitulo);
            pTitulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(pTitulo);
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph($"Data de geração: {DateTime.Now:dd/MM/yyyy HH:mm}", fonteNormal));

            // 2. Texto descritivo
            string textoIntro = "Este relatório apresenta todas as doações recebidas por sua instituição, com informações sobre os doadores, datas, status, entre outros.";
            doc.Add(new Paragraph(textoIntro, fonteNormal));
            doc.Add(new Paragraph("\n"));

            // 3. Tabela principal
            PdfPTable tabela = new PdfPTable(7);
            tabela.WidthPercentage = 100;
            tabela.SetWidths(new float[] { 15, 35, 15, 25, 10, 10, 20 }); // proporcional

            // Cabeçalho da tabela
            tabela.AddCell(new PdfPCell(new Phrase("Data doação", fonteSubtitulo)));
            tabela.AddCell(new PdfPCell(new Phrase("Nome doador", fonteSubtitulo)));
            tabela.AddCell(new PdfPCell(new Phrase("Status", fonteSubtitulo)));
            tabela.AddCell(new PdfPCell(new Phrase("Alimentos Doado(s)", fonteSubtitulo)));
            tabela.AddCell(new PdfPCell(new Phrase("Quantidade", fonteSubtitulo)));
            tabela.AddCell(new PdfPCell(new Phrase("Unidade", fonteSubtitulo)));
            tabela.AddCell(new PdfPCell(new Phrase("Validade", fonteSubtitulo)));

            // Dados
            var fontePequena = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            foreach (var d in doacoes)
            {
                tabela.AddCell(new PdfPCell(new Phrase(d.DataDoacao.ToString("dd/MM/yyyy"), fontePequena)));
                tabela.AddCell(new PdfPCell(new Phrase(d.DoadorNome.ToString(), fontePequena)));
                tabela.AddCell(new PdfPCell(new Phrase(d.Status.ToString(), fontePequena)));

                var alimentos = string.Join(", ", d.AlimentoNome);
                tabela.AddCell(new PdfPCell(new Phrase(alimentos, fontePequena)));

                var quantidadeTotal = d.Quantidade.ToString("0.##");
                tabela.AddCell(new PdfPCell(new Phrase(quantidadeTotal, fontePequena)));

                tabela.AddCell(new PdfPCell(new Phrase(d.UnidadeMedida != null ? d.UnidadeMedida.ToString() : "N/A", fontePequena)));
                tabela.AddCell(new PdfPCell(new Phrase(d.Validade.ToString(), fontePequena)));
            }

            doc.Add(tabela);
            doc.Add(new Paragraph("\n"));

            // 4. Totais e resumo
            int totalDoacoes = doacoes.Count;
            string statusMaisComum = GetStatusMaisComum(doacoes);
            doc.Add(new Paragraph("\n"));

            // 5. Rodapé
            var rodape = new Paragraph("AlimentAção", FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8));
            rodape.Alignment = Element.ALIGN_CENTER;
            doc.Add(rodape);

            doc.Close();
        }
    }


    public async Task<int> ObterTotalDoacoesMes(int id)
    {
        try
        {
            return await _relatorioRepositorio.ObterTotalDoacoesMes(id);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao obter total de doações do mês.", ex);
        }
    }

    public async Task<int> ObterTotalUsuarios()
    {
        try
        {
            return await _relatorioRepositorio.ObterTotalUsuarios();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao obter total de usuários.", ex);
        }
    }

    public async Task<IEnumerable<AlimentoPorCategoriaDto>> ObterAlimentosPorCategoria(int id)
    {
        try
        {
            return await _relatorioRepositorio.ObterAlimentosPorCategoria(id);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao obter alimentos por categoria.", ex);
        }
    }

    public async Task<double> ObterTotalKgAlimentos(int id)
    {
        try
        {
            return await _relatorioRepositorio.ObterTotalKgAlimentos(id);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Erro ao obter total em kg dos alimentos.", ex);
        }
    }


private string GetStatusMaisComum(List<DoacaoComDetalhes> doacoes)
    {
        var counts = new Dictionary<string, int>();
        foreach (var d in doacoes)
        {
            string statusAsString = d.Status.ToString(); 
            if (!counts.ContainsKey(statusAsString))
                counts[statusAsString] = 0;
            counts[statusAsString]++;
        }
        int maxCount = 0;
        string status = "";
        foreach (var kvp in counts)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                status = kvp.Key;
            }
        }
        return status;
    }

    string IRelatorioService.GetStatusMaisComum(List<Doacao> doacoes)
    {
        throw new NotImplementedException();
    }
}
