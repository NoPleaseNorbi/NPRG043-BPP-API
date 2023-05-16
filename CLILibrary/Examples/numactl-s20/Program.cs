
using CLILibrary;
using System;
using System.Text;


class Program
{
    public static void Main(string[] args)
    {
        var Program = new Program();

        Program.run(args);
    }

    public void run(string[] args)
    {

        var usageString =
        @"numactl [--interleave= | -i <nodes>] [--preferred= | -p <node>]
        [--physcpubind= | -C <cpus>] [--membind= | -m <nodes>]
        command args ...
        numactl [--show | -s]
        numactl [--hardware | -H]

<nodes> is a comma delimited list of node numbers or A-B ranges or all.
<cpus> is a comma delimited list of cpu numbers or A-B ranges or all.";

        var versionString = "";

        var parser = new Command("numactl", usageString, versionString);

        var interleaveOption = new Option
        {
            ShortName = "i",
            LongName = "interleave",
            Description = "Interleave memory allocation on the specified nodes.",
            Type = ArgumentType.String,
        };

        var preferredOption = new Option
        {
            ShortName = "p",
            LongName = "preferred",
            Description = "Prefer memory allocations from the specified node.",
            Type = ArgumentType.Int,
            MinVal = 0,
            MaxVal = 3
        };

        var membindOption = new Option
        {
            ShortName = "m",
            LongName = "membind",
            Description = "Allocate memory from the specified nodes only.",
            Type = ArgumentType.String,
        };

        var physcpubindOption = new Option
        {
            ShortName = "C",
            LongName = "physcpubind",
            Description = "Run on the specified CPUs only.",
            Type = ArgumentType.String,
        };

        var showOption = new Option
        {
            ShortName = "S",
            LongName = "show",
            Description = "Show current NUMA policy.",
            Type = ArgumentType.Bool,
        };

        var hardwareOption = new Option
        {
            ShortName = "H",
            LongName = "hardware",
            Description = "Print hardware configuration.",
            Type = ArgumentType.Bool,
        };

        parser.AddOption(interleaveOption, false);
        parser.AddOption(preferredOption, false);
        parser.AddOption(membindOption, false);
        parser.AddOption(physcpubindOption, false);
        parser.AddOption(showOption, false);
        parser.AddOption(hardwareOption, false);

        var commandArgument = new PlainArgument();

        // Cannot use PlainArgument
        // parser.AddOption(commandArgument);


        // The parser accepts string, not string[].
        parser.Parse(args);

        if(parser.Options.Count() > 0)
        {
            // print manually created help
            //Console.WriteLine(parser.Description);
            foreach(var option in parser.Options!)
            { 
                Console.WriteLine(String.Format("{0,-27}{}", $"--{option.LongName}, -{option.ShortName}", option.Description));
            }
            return;
        }

        if (parser.HasOption("hardware"))
        {
            PrintHardwareInfo();
            return;
        }
        else if (parser.HasOption("info"))
        {
            PrintShowInfo();
            return;
        }


        var configuration = new ProgramConfig();


        if (parser.HasOption("physcpubind"))
        {
            configuration.Physcpubind = parseIntList(physcpubindOption.Value!, 0, 31) ;
        }
        if (parser.HasOption("Membind"))
        {
            configuration.Membind = parseIntList(membindOption.Value!, 0, 3);
        }
        if (parser.HasOption("Interleave"))
        {
            configuration.Interleave = parseIntList(interleaveOption.Value!, 0, 3);
        }
        if (parser.HasOption("preferred"))
        {
            var val = int.Parse(preferredOption.Value!);
            if (val < 0 || val > 3)
                throw new ArgumentOutOfRangeException();

            configuration.Preferred = val;
        }


        // validate command is not empty ??

        Console.WriteLine(configuration);
    }

    private void PrintHardwareInfo()
    {

        Console.WriteLine(@"available: 2 nodes (0-1)
node 0 cpus: 0 2 4 6 8 10 12 14 16 18 20 22
node 0 size: 24189 MB
node 0 free: 18796 MB
node 1 cpus: 1 3 5 7 9 11 13 15 17 19 21 23
node 1 size: 24088 MB
node 1 free: 16810 MB
node distances:
node   0   1
  0:  10  20
  1:  20  10");

    }

    private void PrintShowInfo()
    {
        Console.WriteLine(@"policy: default
preferred node: current
physcpubind: 0 1 2 3 4 5 6 7 8
cpubind: 0 1
nodebind: 0 1
membind: 0 1");
    }

    private List<int> parseIntList(string from, int min, int max)
    {
        if (from == "all")
            return Enumerable.Range(min, max - min).ToList();

        List<int> list = new();
        foreach(var s in from.Split(","))
        {
            var i = int.Parse(s);
            if (i < min || i > max)
                throw new ArgumentOutOfRangeException();
            list.Add(i);
        }
        return list;
    }

}

class ProgramConfig
{
    public List<int>? Physcpubind { get; set; }
    public List<int>? Membind { get; set; }
    public List<int>? Interleave { get; set; }
    public int? Preferred { get; set; }

    public override string ToString()
    {
        // I am not sure which value belongs where. Should not matter
        StringBuilder sb = new();
        sb.AppendLine($"policy: default");
        sb.AppendLine(String.Format("preferred node: {}", Preferred is null ? "current" : Preferred));
        if (this.Physcpubind is not null) sb.AppendLine($"physcpubind: {String.Join(' ', this.Physcpubind)}");
        sb.AppendLine($"cpubind: 0 1");
        if (this.Interleave is not null) sb.AppendLine($"nodebind: {String.Join(' ', this.Membind)}");
        if (this.Membind is not null) sb.AppendLine($"membind: {String.Join(' ', this.Membind)}");

        return sb.ToString();
    }
}
