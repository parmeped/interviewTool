using InterviewTool.src;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace InterviewTool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Build configuration
                var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                        .AddJsonFile("appsettings.json", false)
                        .Build();
                var runner = new Runner(configuration);
                runner.Run();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Helpers.PrintWarningMsg(ex.Message);
                return;
            }
        }
    }
}
