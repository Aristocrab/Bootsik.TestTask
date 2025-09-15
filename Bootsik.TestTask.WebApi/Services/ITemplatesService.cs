using Bootsik.TestTask.WebApi.Dtos;
using Bootsik.TestTask.WebApi.Entities;

namespace Bootsik.TestTask.WebApi.Services;

public interface ITemplatesService
{
    Task<IReadOnlyList<HtmlTemplate>> GetAllAsync();
    Task<HtmlTemplate> CreateAsync(HtmlTemplateDto template);
    Task<HtmlTemplate> UpdateAsync(Guid templateId, HtmlTemplateDto template);
    Task DeleteAsync(Guid templateId);
    Task<byte[]> GeneratePdfAsync(Guid templateId, PdfRequest jsonData);
}