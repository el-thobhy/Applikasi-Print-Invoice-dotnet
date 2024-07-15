using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PrintHtmlToPDF.Services
{
    public class PrintInvoice
    {
        private readonly IBrowser _browser;
        private readonly PdfOptions _options;

        public PrintInvoice()
        {
            var browserFetcher = new BrowserFetcher();
            browserFetcher.DownloadAsync().GetAwaiter().GetResult();
            _browser = Puppeteer.LaunchAsync(new LaunchOptions 
            { 
                Headless = false,
            }).GetAwaiter().GetResult();
            _options = new PdfOptions
            {
                Format = PaperFormat.A4,
                MarginOptions = new MarginOptions
                {
                    Top = "10mm",
                    Right = "10mm",
                    Left = "10mm",
                    Bottom = "10mm",
                }
            };
        }

        public async Task ToFile(string htmlContent, string outputFilePath)
        {
            using var page = await _browser.NewPageAsync();
            await page.SetContentAsync(htmlContent);
            await page.PdfAsync(outputFilePath, _options);
            await page.CloseAsync();
        }

        public async Task<byte[]> ToByteArray(string htmlContent)
        {
            using var page = await _browser.NewPageAsync();
            await page.SetContentAsync(htmlContent);
            var data = await page.PdfDataAsync(_options);
            await page.CloseAsync();
            return data;
        }
    }
}
