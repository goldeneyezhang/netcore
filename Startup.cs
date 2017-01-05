using System.IO;  
using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Hosting;  
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
namespace ConsoleApplication  
{
    public class Startup
    {
          private static Dictionary<string,string> _cities=new Dictionary<string,string>
    {
        ["010"]="北京",
        ["028"]="成都",
        ["0512"]="苏州"
    };
        public void ConfigureServices(IServiceCollection services)
        {
                services.AddDirectoryBrowser();
        }

        public void Configure(IApplicationBuilder app)
        {
             string contentRoot = Directory.GetCurrentDirectory();
                IFileProvider fileProvider = new PhysicalFileProvider(Path.Combine(contentRoot, "doc"));
            app.UseFileServer(enableDirectoryBrowsing: true).
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
            //2017/1/5 userouter
            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("weather/{city}/{days}", WeatherForecast);
            app.UseRouter(routeBuilder.Build());
        }        
        public static async Task WeatherForecast(HttpContext context)
        {
            string city=(string)context.GetRouteData().Values["city"];
            city=_cities[city];
            int days=int.Parse(context.GetRouteData().Values["days"].ToString());
            WeatherReport report=new WeatherReport(city,days);
            context.Response.ContentType="text/html";
            await context.Response.WriteAsync("<html><head><title>Weather</title> <meta charste='gb2312'></head></body>");
            await context.Response.WriteAsync("<h3>{city}</h3>");
            foreach(var it in report.WeatherInfos)
            {
                await context.Response.WriteAsync($"{it.Key.ToString("yyyy-MM-dd")}:");
                 await context.Response.WriteAsync($"{it.Value.Condition}({it.Value.LowTemperature}℃ ~ {it.Value.HighTemperature}℃)<br/><br/>");
            }
            await context.Response.WriteAsync("</body><html>");
        }
      
    }
}
