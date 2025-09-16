using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace Bootsik.TestTask.Logic.Browser;

public class BrowserProvider : IBrowserProvider
{
    private readonly ILogger<BrowserProvider> _logger;

    public BrowserProvider(ILogger<BrowserProvider> logger)
    {
        _logger = logger;
    }
    
    public async Task EnsureBrowserDownloadedAsync()
    {
        var fetcher = new BrowserFetcher();
        if (fetcher.GetInstalledBrowsers().Any())
        {
            return;
        }
        
        _logger.LogInformation("Downloading browser...");
        
        await fetcher.DownloadAsync();
        
        _logger.LogInformation("Browser downloaded successfully");
    }

    public async Task<IBrowser> LaunchBrowserAsync()
    {
        await EnsureBrowserDownloadedAsync();
        return await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args =
            [
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage", 
                "--disable-extensions",
                "--disable-gpu",
                "--disable-software-rasterizer"
            ]
        });

    }
}