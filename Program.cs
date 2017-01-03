using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //注册EncodingProvider实现对中文编码的支持
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Func<string,LogLevel,bool>filter=(category,level)=>true;
            //ILoggerFactory loggerFactory=new LoggerFactory();
            //loggerFactory.AddProvider(new ConsoleLoggerProvider(filter,false));
            //loggerFactory.AddProvider(new DebugLoggerProvider(filter));
            //ILogger logger=loggerFactory.CreateLogger(nameof(Program));
            ILogger logger=new ServiceCollection().AddLogging().BuildServiceProvider().GetService<ILoggerFactory>().
            AddConsole().AddDebug().CreateLogger(nameof(Program));
            int eventId=3721;

            logger.LogInformation(eventId,"升级到最新.Net Core版本({version})","1.0.0");
            logger.LogWarning(eventId,"并发量接近上限({maximum})",200);
            logger.LogError(eventId,"数据库连接失败(数据库:{Database},用户名:{User})","TestDb","sa");

            Console.WriteLine("----------2016年12月26日-----------------------");
            ILogger logger2=new ServiceCollection().AddLogging()
            .BuildServiceProvider().GetService<ILoggerFactory>()
            .AddConsole(true)
            .CreateLogger("Ordering");
            //上下文关联输出日志
            using(logger2.BeginScope("订单:{ID}","20160520001"))
            {
                logger2.LogWarning("商品库存不足(商品ID:{0},当前库存:{1},订购数量:{2})","9787121237812",20,50);
                logger2.LogError("商品ID录入错误(商品ID:{0})","9787121235368");
            }
            //AddConsole的扩展方法
            Console.WriteLine("根据定义在logging.json文件中的日志配置，只有等级不低于Warning的日志才会真正被输出到控制台上");
            IConfiguration settings=new ConfigurationBuilder().AddJsonFile("logging.json").Build();
            ILogger logger3=new ServiceCollection().AddLogging().BuildServiceProvider().GetService<ILoggerFactory>().AddConsole(settings).CreateLogger("App");
             logger3.LogInformation(eventId,"升级到最新.Net Core版本({version})","1.0.0");
            logger3.LogWarning(eventId,"并发量接近上限({maximum})",200);
            logger3.LogError(eventId,"数据库连接失败(数据库:{Database},用户名:{User})","TestDb","sa");
             Console.WriteLine("Trace Source");
             TraceSource traceSource =new TraceSource(nameof(Program),SourceLevels.Warning);
             traceSource.Listeners.Add(new ConsoleTraceListener());
              traceSource.TraceEvent(TraceEventType.Information,eventId,"升级到最新.Net Core版本({0})","1.0.0");
            traceSource.TraceEvent(TraceEventType.Warning,eventId,"并发量接近上限({0})",200);
            traceSource.TraceEvent(TraceEventType.Error,eventId,"数据库连接失败(数据库:{0},用户名:{0})","TestDb","sa");
            Console.WriteLine("Trace Source Logger Provider");
            ILogger logger4=new ServiceCollection().AddLogging().BuildServiceProvider().GetService<ILoggerFactory>()
            .AddTraceSource(new SourceSwitch(nameof(Program),"Warning"),new ConsoleTraceListener())
            .CreateLogger<Program>();

              logger4.LogInformation(eventId,"升级到最新.Net Core版本({version})","1.0.0");
            logger4.LogWarning(eventId,"并发量接近上限({maximum})",200);
            logger4.LogError(eventId,"数据库连接失败(数据库:{Database},用户名:{User})","TestDb","sa");
            //以Web的形式读取文件
            new WebHostBuilder(). UseContentRoot(Directory.GetCurrentDirectory())
             .UseKestrel() .Configure(app => app.UseStaticFiles()) .Build() .Run();
        }
    }
}
