using System.Text.RegularExpressions;
using Bootsik.TestTask.Logic.Browser;
using Bootsik.TestTask.Logic.Database;
using Bootsik.TestTask.Logic.Dtos;
using Bootsik.TestTask.Logic.Entities;
using Bootsik.TestTask.Logic.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace Bootsik.TestTask.Logic.Services;

public class TemplatesService : ITemplatesService
{
    private readonly AppDbContext _dbContext;
    private readonly IBrowserProvider _browserProvider;
    private readonly ILogger<TemplatesService> _logger;
    private readonly IValidator<HtmlTemplateDto> _templateDtoValidator;

    public TemplatesService(AppDbContext dbContext, 
        IBrowserProvider browserProvider,
        ILogger<TemplatesService> logger, 
        IValidator<HtmlTemplateDto> templateDtoValidator)
    {
        _dbContext = dbContext;
        _browserProvider = browserProvider;
        _logger = logger;
        _templateDtoValidator = templateDtoValidator;
    }
    
    public async Task<IReadOnlyList<HtmlTemplate>> GetAllAsync()
    {
        return await _dbContext.HtmlTemplates.ToListAsync();
    }

    public async Task<HtmlTemplate> CreateAsync(HtmlTemplateDto template)
    {
        await _templateDtoValidator.ValidateAndThrowAsync(template);

        var newTemplate = new HtmlTemplate
        {
            Name = template.Name,
            Content = template.Content
        };

        await _dbContext.HtmlTemplates.AddAsync(newTemplate);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("New template {TemplateName} has been created", newTemplate.Name);

        return newTemplate;
    }

    public async Task<HtmlTemplate> UpdateAsync(Guid templateId, HtmlTemplateDto template)
    {
        await _templateDtoValidator.ValidateAndThrowAsync(template);

        var existingTemplate = await GetTemplateOrThrowAsync(templateId);

        existingTemplate.Name = template.Name;
        existingTemplate.Content = template.Content;
        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Template {TemplateName} has been updated", template.Name);

        return existingTemplate;
    }

    public async Task DeleteAsync(Guid templateId)
    {
        var existingTemplate = await GetTemplateOrThrowAsync(templateId);

        _dbContext.HtmlTemplates.Remove(existingTemplate);
        
        _logger.LogInformation("Template {TemplateName} has been deleted", existingTemplate.Name);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<byte[]> GeneratePdfAsync(Guid templateId, PdfRequest jsonData)
    {
        var template = await GetTemplateOrThrowAsync(templateId);

        var htmlContent = ReplacePlaceholders(template.Content, jsonData.Data);
        
        // if some placeholders are not replaced write warning log
        var unmatchedPlaceholders = Regex.Matches(htmlContent, @"\{\{\s*(\w+)\s*\}\}")
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();
        if (unmatchedPlaceholders.Count != 0)
        {
            _logger.LogWarning("Some placeholders in template {TemplateName} were not filled: {@Placeholders}", 
                template.Name, unmatchedPlaceholders);
        }

        await _browserProvider.EnsureBrowserDownloadedAsync();
        
        // generate pdf
        _logger.LogInformation("Generating PDF for template {TemplateName}", template.Name);
        
        var browser = await _browserProvider.LaunchBrowserAsync();
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(htmlContent);
        var bytes = await page.PdfDataAsync();
        
        await browser.CloseAsync();
        
        _logger.LogInformation("PDF for template {TemplateName} has been generated", template.Name);
        
        return bytes;
    }
    
    private async Task<HtmlTemplate> GetTemplateOrThrowAsync(Guid templateId)
    {
        var template = await _dbContext.HtmlTemplates.FirstOrDefaultAsync(x => x.Id == templateId);
        if (template is null)
            throw new NotFoundException("Template not found");
        return template;
    }
    
    private static string ReplacePlaceholders(string content, IDictionary<string, object> data)
    {
        // Replaces placeholders in the format {{key}} with values from the dictionary
        foreach (var parameter in data)
        {
            var pattern = @"\{\{\s*" + Regex.Escape(parameter.Key) + @"\s*\}\}";
            content = Regex.Replace(content, pattern, parameter.Value.ToString() ?? "");
        }
        
        return content;
    }
}