using PuppeteerSharp;

namespace Bootsik.TestTask.Logic.Browser;

public interface IBrowserProvider
{
    Task EnsureBrowserDownloadedAsync();
    Task<IBrowser> LaunchBrowserAsync();
}