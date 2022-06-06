using CommandLine.Text;
using CommandLine;

using System.Reflection;
using System.Diagnostics;

namespace KestrelMonitor;

internal class Program
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "The file to run.")]
        public string FileName { get; set; }

        [Option('c', "customArgs", Required = false, HelpText = "The custom arguments for the executable. ex: ' -e Development'. Please start with space else all arguements and include single quotes are not getting supplied")]
        public string CustomArgs { get; set; }
    }

    static void Main(string[] args)
    {

        Process process = null;
        Console.CancelKeyPress += delegate
        {
            if (process != null)
            {
                process.Kill();
            }
        };

        new Parser((settings) =>
        {
            settings.HelpWriter = Console.Error;
            settings.IgnoreUnknownArguments = true;
        })
             .ParseArguments<Options>(args)
            .WithParsedAsync<Options>(async o =>
            {
                if (File.Exists(o.FileName))
                {
                    int counter = 0;
                    while (counter < 3)
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = o.FileName,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true,
                            WorkingDirectory = Path.GetDirectoryName(o.FileName)
                        };

                        if (!string.IsNullOrEmpty(o.CustomArgs.TrimStart()))
                        {
                            startInfo.Arguments = o.CustomArgs.TrimStart();
                        }
                        process = Process.Start(startInfo);


                        process.OutputDataReceived += (sender, line) =>
                        {
                            if (line.Data != null)
                                Console.WriteLine(line.Data);
                        };

                        process.BeginOutputReadLine();
                        await process.WaitForExitAsync();
                        Console.WriteLine("Process exitred. Launching again in 1 second.");
                        Thread.Sleep(1000);
                        counter++;
                    }

                    Console.WriteLine($"Maximum retries(3) to launch this app are exceeded.");
                }
                else
                {
                    Console.WriteLine("File not found. Please check the path.");
                    Console.WriteLine(o.FileName);
                }
            }).Wait();
    }
}