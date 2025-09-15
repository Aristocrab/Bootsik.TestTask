using Bootsik.TestTask.WebApi.Dtos;
using Bootsik.TestTask.WebApi.Entities;
using Bootsik.TestTask.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bootsik.TestTask.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ITemplatesService _templatesService;

    public TemplatesController(ITemplatesService templatesService)
    {
        _templatesService = templatesService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HtmlTemplate>>> GetAll()
    {
        var templates = await _templatesService.GetAllAsync();
        return Ok(templates);
    }

    [HttpPost]
    public async Task<ActionResult<HtmlTemplate>> Create([FromBody] HtmlTemplateDto template)
    {
        var createdTemplate = await _templatesService.CreateAsync(template);
        return CreatedAtAction(nameof(GetAll), new { id = createdTemplate.Id }, createdTemplate);
    }

    [HttpPut("{templateId:guid}")]
    public async Task<ActionResult<HtmlTemplate>> Update(Guid templateId, [FromBody] HtmlTemplateDto template)
    {
        var updatedTemplate = await _templatesService.UpdateAsync(templateId, template);
        return Ok(updatedTemplate);
    }

    [HttpDelete("{templateId:guid}")]
    public async Task<ActionResult> Delete(Guid templateId)
    {
        await _templatesService.DeleteAsync(templateId);
        return NoContent();
    }

    [HttpPost("{templateId:guid}/generate-pdf")]
    public async Task<FileContentResult> GeneratePdf(Guid templateId, [FromBody] PdfRequest jsonData)
    {
        var pdfBytes = await _templatesService.GeneratePdfAsync(templateId, jsonData);
        return File(pdfBytes, "application/pdf", $"generated-{DateTime.Now:dd.MM.yyyy-HH:mm:ss}.pdf");
    }
}