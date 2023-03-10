using System;
using CLILibrary;

namespace Tester;
class Program
{
    static void Main(string[] args)
    {
        var parser = new CommandLineParser();

        var formatOption = new Option
        {
            ShortName = "f",
            LongName = "format",
            Description = "Specify output format, possibly overriding the format specified in the environment variable TIME.",
            Type = OptionType.String,
            DefaultValue = "",
            Synonyms = new List<string> { "fmt" }
        };
        parser.AddOption(formatOption);

        var portabilityOption = new Option
        {
            ShortName = "p",
            LongName = "portability",
            Description = "Use the portable output format.",
            Type = OptionType.Boolean,
            DefaultValue = false
        };
        parser.AddOption(portabilityOption);

        var outputOption = new Option
        {
            ShortName = "o",
            LongName = "output",
            Description = "Do not send the results to stderr, but overwrite the specified file.",
            Type = OptionType.String,
            DefaultValue = "",
            Synonyms = new List<string> { "out" }
        };
        parser.AddOption(outputOption);

        var appendOption = new Option
        {
            ShortName = "a",
            LongName = "append",
            Description = "(Used together with -o.) Do not overwrite but append.",
            Type = OptionType.Boolean,
            DefaultValue = false
        };
        parser.AddOption(appendOption);

        var verboseOption = new Option
        {
            ShortName = "v",
            LongName = "verbose",
            Description = "Give very verbose output about all the program knows about.",
            Type = OptionType.Boolean,
            DefaultValue = false
        };
        parser.AddOption(verboseOption);

        var helpOption = new Option
        {
            LongName = "help",
            Description = "Print a usage message on standard output and exit successfully.",
            Type = OptionType.None,
            DefaultValue = false
        };
        parser.AddOption(helpOption);

        var versionOption = new Option
        {
            ShortName = "V",
            LongName = "version",
            Description = "Print version information on standard output, then exit successfully.",
            Type = OptionType.None,
            DefaultValue = false
        };
        parser.AddOption(versionOption);

        parser.Usage = "Usage: time [options] command [arguments...]";
        parser.Version = "2023-69-69";

        parser.Parse(args);

        if (parser.HasOption("help"))
        {
            Console.WriteLine(parser.Usage);
        }

        if (parser.HasOption("version"))
        {
            Console.WriteLine(parser.Version);
        }
    }
}