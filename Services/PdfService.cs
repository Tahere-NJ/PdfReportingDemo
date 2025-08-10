using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PdfReportingDemo.Services
{
    public class PdfService
    {
        public void GeneratePdf(string html, string outputPath)
        {
            GeneratePdfAsync(html, outputPath).Wait();
        }

        private async Task GeneratePdfAsync(string html, string outputPath)
        {
            try
            {
                // Find Chromium executable path
                string chromiumPath = GetChromiumPath();

                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = chromiumPath
                });

                var page = await browser.NewPageAsync();
                await page.SetContentAsync(html);

                await page.PdfAsync(outputPath, new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true,
                    MarginOptions = new MarginOptions
                    {
                        Top = "1cm",
                        Right = "1cm",
                        Bottom = "1cm",
                        Left = "1cm"
                    }
                });

                await browser.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate PDF: {ex.Message}", ex);
            }
        }

        private string GetChromiumPath()
        {
            // Common Chromium installation paths
            string[] possiblePaths = {
                // Google Chrome paths
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                
                // Chromium paths
                @"C:\Users\TiChan\AppData\Local\Chromium\Application\chrome.exe",
                //@"C:\Program Files (x86)\Chromium\Application\chrome.exe",
                
                // Microsoft Edge (Chromium-based)
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
                
                // Local user installations
                Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\Google\Chrome\Application\chrome.exe"),
                Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\Chromium\Application\chrome.exe")
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    Console.WriteLine($"Found Chromium at: {path}");
                    return path;
                }
            }

            // If no standard path found, try to find via registry or environment
            string pathFromRegistry = GetChromiumPathFromRegistry();
            if (!string.IsNullOrEmpty(pathFromRegistry) && File.Exists(pathFromRegistry))
            {
                Console.WriteLine($"Found Chromium via registry: {pathFromRegistry}");
                return pathFromRegistry;
            }

            throw new Exception("Could not find Chromium installation. Please install Google Chrome, Chromium, or Microsoft Edge, or specify the path manually.");
        }

        private string GetChromiumPathFromRegistry()
        {
            try
            {
                // Try to find Chrome in registry
                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
                {
                    if (key != null)
                    {
                        var path = key.GetValue("")?.ToString();
                        if (!string.IsNullOrEmpty(path))
                            return path;
                    }
                }

                // Try current user registry
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
                {
                    if (key != null)
                    {
                        var path = key.GetValue("")?.ToString();
                        if (!string.IsNullOrEmpty(path))
                            return path;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registry lookup failed: {ex.Message}");
            }

            return null;
        }
    }
}