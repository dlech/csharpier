using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpier
{
    class Program
    {
        // TODO 1 Configuration.cs from data.entities
        // TODO 1 CurrencyDto.cs from data.entities

        // TODO https://github.com/dotnet/command-line-api/blob/main/docs/Your-first-app-with-System-CommandLine-DragonFruit.md
        // can be used to simplify this, but it didn't appear to work with descriptions of the parameters
        static async Task<int> Main(string[] args)
        {
            var rootCommand = CommandLineOptions.Create();

            rootCommand.Handler = CommandHandler.Create(new CommandLineOptions.Handler(Run));

            return await rootCommand.InvokeAsync(args);
        }

        public static async Task<int> Run(
            string directoryOrFile,
            bool check,
            bool fast,
            bool skipWrite,
            CancellationToken cancellationToken
        ) {
            if (string.IsNullOrEmpty(directoryOrFile))
            {
                directoryOrFile = Directory.GetCurrentDirectory();
            }
            else
            {
                directoryOrFile = Path.Combine(Directory.GetCurrentDirectory(), directoryOrFile);
            }

            var commandLineOptions = new CommandLineOptions
            {
                DirectoryOrFile = directoryOrFile,
                Check = check,
                Fast = fast,
                SkipWrite = skipWrite
            };

            return await CommandLineFormatter.Format(
                commandLineOptions,
                new FileSystem(),
                new SystemConsole(),
                cancellationToken
            );
        }
    }
}
