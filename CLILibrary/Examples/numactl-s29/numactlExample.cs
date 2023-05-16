using CLILibrary;


namespace numactlExample;
class Program {
    static void Main(string[] args) {
        var parserShowAndHardware = new Command("numactl", "Control NUMA policy for processes or shared memory ", "a1.0.0");
        AddSwitchesToParser(parserShowAndHardware);

        var parserConfigurationAndCommand = new Command("numactl", "Control NUMA policy for processes or shared memory ", "a1.0.0");
        AddPolicyConfigToParser(parserConfigurationAndCommand);
        
        if (args.Length == 0) {
            System.Console.WriteLine("Printing fancy help text ... ");
            return;
        }

        try { 
            parserShowAndHardware.Parse(args);
            if (parserShowAndHardware.HasOption("hardware") && !parserShowAndHardware.HasOption("show")){
                System.Console.WriteLine("Printing inventory of available nodes on the system ... ");
                return;
            }
            if (!parserShowAndHardware.HasOption("hardware") && parserShowAndHardware.HasOption("show")){
                System.Console.WriteLine("Printing NUMA policy settings of the current process ... ");
                return;
            }
        }
        catch {}

        try {
            parserConfigurationAndCommand.Parse(args);
            if (parserConfigurationAndCommand.HasOption("membind") && HasCorrectNodeFormat(parserConfigurationAndCommand.GetOptionValues("membind")!) &&
                !parserConfigurationAndCommand.HasOption("preferred") &&
                !parserConfigurationAndCommand.HasOption("interleave")) {
                    System.Console.WriteLine($"physcpubind: {parserConfigurationAndCommand.GetOptionValues("physcpubind")}, membind: {parserConfigurationAndCommand.GetOptionValues("physcpubind")}" +
                        $"command and arguments {parserConfigurationAndCommand.GetOptionValues("command")}");
            }
            if (!parserConfigurationAndCommand.HasOption("membind") && 
                !parserConfigurationAndCommand.HasOption("preferred") && HasCorrectNodeFormat(parserConfigurationAndCommand.GetOptionValues("preferred")!) &&
                !parserConfigurationAndCommand.HasOption("interleave")) {
                    System.Console.WriteLine($"physcpubind: {parserConfigurationAndCommand.GetOptionValues("physcpubind")}, preferred: {parserConfigurationAndCommand.GetOptionValues("preferred")}" +
                        $"command and arguments {parserConfigurationAndCommand.GetOptionValues("command")}");
            }
            if (!parserConfigurationAndCommand.HasOption("membind") && 
                !parserConfigurationAndCommand.HasOption("preferred") &&
                !parserConfigurationAndCommand.HasOption("interleave") && HasCorrectNodeFormat(parserConfigurationAndCommand.GetOptionValues("interleave")!)) {
                    System.Console.WriteLine($"physcpubind: {parserConfigurationAndCommand.GetOptionValues("physcpubind")}, interleave: {parserConfigurationAndCommand.GetOptionValues("interleave")}" +
                        $"command and arguments {parserConfigurationAndCommand.GetOptionValues("command")}");
            }
        }
        catch(Exception e) {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void AddSwitchesToParser(Command parser) {
        var hardwareSwitch = new Option
        {
            ShortName = "H",
            LongName = "hparserConfigurationAndCommandardware",
            Description = "Show inventory of available nodes on the system.",
            Type = ArgumentType.Bool,
        };
        parser.AddOption(hardwareSwitch, false);

        var showSwitch = new Option
        {
            ShortName = "s",
            LongName = "show",
            Description = "Show NUMA policy settings of the current process.",
            Type = ArgumentType.Bool,
        };
        parser.AddOption(showSwitch, false);
    }

    public static void AddPolicyConfigToParser(Command parser) {
        var physCPUBindOption = new Option
        {
            ShortName = "C",
            LongName = "physcpubind",
            Description = "Only execute process on cpus. This accepts cpu numbers as shown in the processor fields of /proc/cpuinfo,"
                        + "or relative cpus as in relative to the current cpuset.",
            Type = ArgumentType.String, //in fact I want it to take not just spring, but string "all", list of ints and list of ints with intervals.
            AcceptsParams = true,
            ArgumentCount = 1,
        };
        parser.AddOption(physCPUBindOption, false);

        var memBindOption = new Option
        {
            ShortName = "m",
            LongName = "membind",
            Description = "Only allocate memory from nodes. Allocation will fail when there is not enough memory available on these nodes. nodes may be specified as noted above.",
            Type = ArgumentType.String, //in fact I want it to take not just spring, but string "all", list of ints and list of ints with intervals.
            AcceptsParams = true,
            ArgumentCount = 1,
        };
        parser.AddOption(memBindOption, false);

        var preferredOption = new Option
        {
            ShortName = "p",
            LongName = "preferred",
            Description = "Only allocate memory from nodes. Allocation will fail when there is not enough memory available on these nodes. nodes may be specified as noted above.",
            Type = ArgumentType.Int, 
            AcceptsParams = true,
            ArgumentCount = 1,
            MinVal = 0,
            MaxVal = 3,
        };
        parser.AddOption(preferredOption, false);

        var interleaveOption = new Option
        {
            ShortName = "i",
            LongName = "interleave",
            Description = "Only allocate memory from nodes. Allocation will fail when there is not enough memory available on these nodes. nodes may be specified as noted above.",
            Type = ArgumentType.String, //in fact I want it to take not just spring, but string "all", list of ints and list of ints with intervals.
            AcceptsParams = true,
            ArgumentCount = 1,
        };
        parser.AddOption(interleaveOption, false);

        // !!!
        // !!! NO WAY TO ADD PLAIN TEXT ARGUMENTS RN SO IM GONNA ADD ANOTHER OPTION AND PRETEND LIKE IT IS A PLAIN TEXT ARGUMENT :)
        // !!! tbh even if it could be added to parser, there is also no way how to retrive the content anyways
        // !!!

        var commandPlainTextArgument = new Option
        {
            LongName = "command",
            Description = "Plain text argument pretending to be option.",
            Type = ArgumentType.String, 
            AcceptsParams = true,
        };
        parser.AddOption(commandPlainTextArgument, true);
    }

    public static bool HasCorrectCPUFormat(string arguments) {
        //correct format check needed
        return true;
    }

    public static bool HasCorrectNodeFormat(string arguments) {
        //correct format check needed
        return true;
    }
}