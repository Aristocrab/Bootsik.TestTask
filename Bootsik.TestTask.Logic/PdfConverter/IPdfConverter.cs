namespace Bootsik.TestTask.Logic.PdfConverter;

public interface IPdfConverter
{
    byte[] GeneratePdf(string htmlContent);
}