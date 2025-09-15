using Bootsik.TestTask.Logic.Dtos;
using Bootsik.TestTask.Logic.Entities;

namespace Bootsik.TestTask.Logic.Services;

public interface ITemplatesService
{
    Task<IReadOnlyList<HtmlTemplate>> GetAllAsync();
    Task<HtmlTemplate> CreateAsync(HtmlTemplateDto template);
    Task<HtmlTemplate> UpdateAsync(Guid templateId, HtmlTemplateDto template);
    Task DeleteAsync(Guid templateId);
    Task<byte[]> GeneratePdfAsync(Guid templateId, PdfRequest jsonData);
}