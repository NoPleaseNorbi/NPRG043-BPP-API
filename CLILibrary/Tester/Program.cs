using System;
using CLILibrary;

namespace Tester;
class Program
{
    static void Main(string[] args)
    {
        var parser = new Command("time", "Usage info", "Help info");

        var formatOption = new Option
        {
            ShortName = "f",
            LongName = "format",
            Description = "Specify output format, possibly overriding the format specified in the environment variable TIME.",
            Type = ArgumentType.String,
        };
        parser.AddOption(formatOption, false);

        var portabilityOption = new Option
        {
            ShortName = "p",
            LongName = "portability",
            Description = "Use the portable output format.",
            Type = ArgumentType.Bool,
        };
        parser.AddOption(portabilityOption, false);

        var outputOption = new Option
        {
            ShortName = "o",
            LongName = "output",
            Description = "Do not send the results to stderr, but overwrite the specified file.",
            Type = ArgumentType.String,
            Synonyms = new List<string> { "out" }
        };
        parser.AddOption(outputOption, false);

        var appendOption = new Option
        {
            ShortName = "a",
            LongName = "append",
            Description = "(Used together with -o.) Do not overwrite but append.",
            Type = ArgumentType.Bool,
        };
        parser.AddOption(appendOption, false);

        var verboseOption = new Option
        {
            ShortName = "v",
            LongName = "verbose",
            Description = "Give very verbose output about all the program knows about.",
            Type = ArgumentType.Bool,
        };
        parser.AddOption(verboseOption, false);


    }
}