using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.DependencyInjection;
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
            logger.LogWarning(eventId,"并发量接近上线({maximum})",200);
            logger.LogError(eventId,"数据库连接失败(数据库:{Database},用户名:{User})","TestDb","sa");

            Console.WriteLine("Hello World!");
        }
    }
}
