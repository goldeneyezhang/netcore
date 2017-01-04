using System.IO;  
using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Hosting;  
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
namespace ConsoleApplication  
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
                services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app)
        {
             string contentRoot = Directory.GetCurrentDirectory();
                IFileProvider fileProvider = new PhysicalFileProvider(Path.Combine(contentRoot, "doc"));
            app.UseFileServer(enableDirectoryBrowsing: true) .
            UseDefaultFiles().
              UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider=fileProvider,
                RequestPath="/documents"
            }).
            UseStaticFiles().
            UseStaticFiles(new StaticFileOptions
            {
                FileProvider=fileProvider,
                RequestPath="/documents"
            }).
            UseDirectoryBrowser(new DirectoryBrowserOptions { FileProvider = fileProvider, RequestPath = "/documents"});
        }        

      
    }
}
