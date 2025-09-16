using iText.Html2pdf;
using Microsoft.Extensions.Logging;

namespace Bootsik.TestTask.Logic.PdfConverter;

public class PdfConverter : IPdfConverter
{
    private readonly ILogger<PdfConverter> _logger;

    public PdfConverter(ILogger<PdfConverter> logger)
    {
        _logger = logger;
    }

    public byte[] GeneratePdf(string htmlContent)
    {
        using var pdfStream = new MemoryStream();

        HtmlConverter.ConvertToPdf(htmlContent, pdfStream);

        return pdfStream.ToArray();
    }
}