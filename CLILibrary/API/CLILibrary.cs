using CLILibrary.Exceptions;
using Microsoft.VisualBasic;

#nullable enable
///<summary>
///A library for Command Line Interface, used for parsing command line arguments of a command and manipulating with them.
///<summary>
namespace CLILibrary {
    ///<summary>
    ///An enumeration signaling the type of <see cref="Argument.Value"/>. <see cref="ArgumentType.Bool"/> -> bool, <see cref="ArgumentType.String"/> -> string,
    ///<see cref="ArgumentType.Int"/> -> int and <see cref="ArgumentType.Object"/> -> boxed object.
    ///<summary>
    public enum ArgumentType {
        String,
        Int,
        Bool,
        Object
    }

    ///<summary>
    ///A base class characterizing an element on the Command Line after the <see cref="Command.Name"/> specification.
    ///<summary>
    public abstract class Argument {
        /// <value>A property storing the value of the argument.</value>
        public string? Value { get; set; }

        /// <value>Upper boundary for the Value of the argument.</value>
        public virtual int? MaxVal { get; set; }

        /// <value>Lower boundary for the Value of the argument.</value>
        public virtual int? MinVal { get; set; }

        public abstract void CheckIfCorrectlyConstructed();
    }

    ///<summary>
    ///A class inheriting from the base class <see cref="Argument"/> and only removing the abstract modifier from it.
    ///Characterizes an Argument for the <see cref="Command"/> with no name, only a value.
    ///<summary>
    public sealed class PlainArgument : Argument {
        public override void CheckIfCorrectlyConstructed() {
            if (int.TryParse(Value, out var val)) {
                if (MaxVal < val || MinVal > val) {
                    throw new ValueOutOfBoundsException($"{Value} is out of bounds.");
                }
            }
            else {
                if (Value?.Length > MaxVal || Value?.Length < MinVal) {
                    throw new ValueOutOfBoundsException($"{Value} is out of bounds.");
                }
            }


        }
    }
    

    ///<summary>
    ///<remarks>A class inheriting from the base class <see cref="Argument"/> and  extending it as a "storage" for one <see cref="Command"/> option.</remarks>
    ///<summary>
    public sealed class Option : Argument {
        private string? _longName;
        private string? _shortName;
        private int _argumentCount;

        public Option() {
            Values = new List<object>();
            RequiredOptions = new List<Option>();
            Synonyms = new List<string>();
            IncompatibleOptions = new List<Option>();
            CheckIfCorrectlyConstructed();
        }

        /// <value>The long name of the Option, if set.</value>
        public string? LongName {
            get => _longName;
            set {
                if (value is null || value.Length < 2 || int.TryParse(value[0].ToString(), out var res) ||
                    "*-/.\n\t\r\0 ".Contains(value[0]) || value.Contains("\n") || value.Contains("\t") || value.Contains("\r") || value.Contains(" ") || Synonyms.Contains(value)) {
                    throw new OptionException("Invalid long name.");
                }
                _longName = value;
            }

        }

        /// <value>The short name of the Option, if set.</value>
        public string? ShortName {
            get => _shortName;
            set {
                if (value is null || value.Length != 1 || int.TryParse(value, out var results) ||
                    "*-/.\n\t\r\0 ".Contains(value) || Synonyms.Contains(value)) {
                    throw new OptionException("Invalid short name.");
                }

                _shortName = value;
            }
        }

        public List<object> Values { get; set; }

        /// <value>Description of the usage for the Option.</value>
        public string? Description { get; set; }

        /// <value>List of all recognized Names for the Option.</value>
        public List<string> Synonyms { get; set; }

        /// <value>The name of the type in which the Option operates.</value>
        public ArgumentType Type { get; set; }

        /// <value>Number of possible arguments in the Option.</value>
        public int ArgumentCount
        {
            get => _argumentCount;
            set {
                if (value < 0)
                {
                    throw new OptionException("Argument count cannot be negative.");
                }
                _argumentCount = value;} 
        }

        /// <value>A bool property indicating whether the Option allows for any parameters after it.</value>
        public bool AcceptsParams { get; set; }
        /// <value>A list of options that are not compatible with each other.</value>
        public List<Option> IncompatibleOptions { get; set; }

        /// <value>A list of options that are mandatory to be included together.</value>
        public List<Option> RequiredOptions { get; set; }
        /// <value>Upper boundary for the Values of the arguments.</value>
        public override int? MaxVal { get; set; }

        /// <value>Lower boundary for the Values of the arguments.</value>
        public override int? MinVal { get; set; }
        /// <summary>
        /// Checks if the option class was constructed correctly
        /// </summary>
        /// <exception cref="OptionException"></exception>
        public override void CheckIfCorrectlyConstructed() {

            if (IncompatibleOptions == null && RequiredOptions == null) return;
            foreach (var requiredOption in IncompatibleOptions.SelectMany(incompatibleOption =>
                         RequiredOptions.Where(
                             requiredOption => requiredOption.ShortName == incompatibleOption.ShortName ||
                                               requiredOption.LongName == incompatibleOption.LongName || requiredOption == incompatibleOption))) {
                throw new OptionException($"Required option {requiredOption} also in Incompatible options.");
            }

            if (MinVal > MaxVal)
            {
                throw new OptionException($"Invalid bound were set for the option.");
            }

            if (AcceptsParams && ArgumentCount == 0)
            {
                throw new OptionException("Incompatible settings.");
            }
        }
    }

    ///<summary>
    ///A class representing a Command and it's settings. Stores all of the defined Options for it and is responsible for parsing and processing the command line input.
    ///<summary>
    public sealed class Command {
        /// <value>Name of the command.</value>
        private string? Name { get; }

        /// <value>General usage description and manual for the given command and its options.</value>
        private string? Description { get; }

        /// <value>List of all recognized Names for the command.</value>
        public List<string> Synonyms { get; set; }

        /// <value>List of all recognized Options for the command.</value>
        public List<Option> Options { get; set; }

        /// <value>List of plain arguments for the command, if any</value>
        public List<PlainArgument> PlainArguments { get; set; }

        /// <value>List of all Options that are mandatory for the command.</value>
        public List<Option> RequiredOptions { get; set; }

        /// <value>A property containing the version of the command.</value>
        private string? Version { get; }

        /// <value>The delimiter set for distinguishing plain arguments.</value>
        public char Delimiter { get; set; }

        /// <summary>
        /// Basic constructor for the class <see cref="Command"/>
        /// </summary>
        public Command() {
            Options = new List<Option>();
            RequiredOptions = new List<Option>();
            PlainArguments = new List<PlainArgument>();
            Synonyms = new List<string>();
        }

        /// <summary>
        /// A three parameter constructor initializing properties <see cref="Name"/>, <see cref="Description"/> and <see cref="Version"/>.
        /// </summary>
        /// <param name="name">Initializes <see cref="Name"/></param>
        /// <param name="usage">Initializes <see cref="Description"/></param>
        /// <param name="version">Initializes <see cref="Version"/></param>
        public Command(string name, string usage, string version) {
            Name = name;
            Description = usage;
            Version = version;
            Options = new List<Option>();
            RequiredOptions = new List<Option>();
            PlainArguments = new List<PlainArgument>();
            Synonyms = new List<string>();
            Delimiter = ' ';
        }
        /// <summary>
        /// Parses an array of strings into arguments for options.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="OptionParseException"></exception>
        /// <exception cref="ValueOutOfBoundsException"></exception>
        /// <exception cref="UnsupportedValueTypeException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OptionNameException"></exception>
        public void Parse(string[] args)
        {
            var argCount = 0;
            var plain = false;
            var index = 0;
            var foundOptionsList = new List<Option>();
            Option currentOption = null;
            if (args.Length == 0) return;
            foreach (var arg in args) {
                if (!plain) {
                    if (arg.StartsWith("-")) {
                        if (currentOption is not null) {
                            if (argCount < currentOption.ArgumentCount)
                            {
                                throw new OptionParseException(
                                    $"{currentOption} expected {currentOption.ArgumentCount} arguments, got {argCount}.");
                            }
                            Options[index] = currentOption;
                        }

                        currentOption = GetOption(arg[1..]);
                        foundOptionsList.Add(currentOption);
                        index = Options.IndexOf(currentOption);
                        if (!currentOption.AcceptsParams) {
                            currentOption.Values.Add(true);
                        }
                        argCount = 0;
                        continue;
                    }

                    if (arg.StartsWith("--") && arg.Length > 2) {
                        if (currentOption is not null) {
                            if (argCount < currentOption.ArgumentCount)
                            {
                                throw new OptionParseException(
                                    $"{currentOption} expected {currentOption.ArgumentCount} arguments, got {argCount}.");
                            }
                            Options[index] = currentOption;
                        }
                        currentOption = GetOption(arg[2..]);
                        foundOptionsList.Add(currentOption);
                        index = Options.IndexOf(currentOption);
                        if (!currentOption.AcceptsParams) {
                            currentOption.Values.Add(true);
                        }
                        argCount = 0;
                        continue;
                    }

                    if (arg == "--") { plain = true; continue; }

                    argCount++;
                    switch (currentOption.AcceptsParams) {
                        case false:
                            throw new OptionParseException($"The option {currentOption} does not accept arguments.");
                        case true when argCount <= currentOption.ArgumentCount:
                            switch (currentOption.Type) {
                                case ArgumentType.String:
                                    if (arg.Length < currentOption.MinVal && arg.Length > currentOption.MaxVal) {
                                        throw new ValueOutOfBoundsException(
                                            $"{arg} has to be of length from {currentOption.MinVal} to {currentOption.MaxVal}.");
                                    }
                                    currentOption.Values.Add(arg);
                                    break;
                                case ArgumentType.Int:
                                    if (!int.TryParse(arg, out var result))
                                        throw new UnsupportedValueTypeException($"{arg} has to be of type Int32.");
                                    if (result < currentOption.MinVal || result > currentOption.MaxVal) {
                                        throw new ValueOutOfBoundsException(
                                            $"{arg} has to be from {currentOption.MinVal} to {currentOption.MaxVal}.");
                                    }
                                    currentOption.Values.Add(result);
                                    break;
                                case ArgumentType.Bool:
                                    currentOption.Values.Add(true);
                                    break;
                                case ArgumentType.Object:
                                    currentOption.Values.Add(arg);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            throw new OptionParseException(
                                $"{currentOption} expected {currentOption.ArgumentCount} arguments, got {currentOption.ArgumentCount + 1}.");
                    }

                }
                else {
                    PlainArguments.Add(new PlainArgument { Value = arg });
                }
            }
            if (currentOption is not null)
            {
                if (argCount < currentOption.ArgumentCount) {
                    throw new OptionParseException(
                        $"{currentOption} expected {currentOption.ArgumentCount} arguments, got {argCount}.");
                }
                Options[index] = currentOption;
            }
            foreach (var option in RequiredOptions.Where(option => !foundOptionsList.Contains(option)))
            {
                throw new OptionNameException($"{option}, which was required was not found.");
            }
        }

        /// <summary>
        /// Method used for parsing of the command line arguments and saving their values in the Options list
        /// </summary>
        /// <param name="line">User input after command name (e.g. Options, Arguments, Plain Arguments).</param>
        public void StringParse(string line)
        {
            var parsedLine = line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);
            Parse(parsedLine);
        }

        /// <summary>
        /// Method adding an <see cref="Option"/> object to <see cref="Options"/> or <see cref="RequiredOptions"/>
        /// depending on the value of the <paramref name="required"/> value.
        /// </summary>
        /// <param name="option">The Option to be added.</param>
        /// <param name="required">A bool value indicating if the Option being added is mandatory.</param>
        public void AddOption(Option option, bool required) {
            if (option is { ShortName: { }, LongName: { } } &&
                (HasOption(option.LongName) || HasOption(option.ShortName))) {
                throw new DuplicateException(
                    $"An option with the name {option.ShortName} or {option.LongName} already exists.");
            }
            if (required) RequiredOptions.Add(option);
            Options.Add(option);
        }

        /// <summary>
        /// Method adding one or more <see cref="Option"/> objects to <see cref="Options"/> or <see cref="RequiredOptions"/>
        /// depending on the value of the second item in the <paramref name="options"/> tuple.
        /// </summary>
        /// <param name="options">Any number of 2 element tuples with types Option, bool that indicate in order: The Option to be added and a bool value indicating if it is mandatory.</param>
        public void AddOptions(params (Option, bool)[] options) {
            foreach (var option in options) {
                AddOption(option.Item1, option.Item2);
            }
        }

        /// <summary>
        /// Method used for extracting <see cref="Argument.Value"/> from a string containing the Option's name.
        /// </summary>
        /// <param name="optionName">The Name (identifier) of an option.</param>
        /// <returns>Returns the Value of an Option identified by its Name.</returns>
        public string? GetOptionValues(string optionName)
        {
            var result = "";
            foreach (var option in Options.Where(option =>
                         optionName == option.ShortName || optionName == option.LongName || option.Synonyms.Contains(optionName)))
            {
                return option.Values.Count == 0 ? "" : option.Values.Aggregate("", (current, value) => current + value);
            }
            throw new MissingOptionException(
                $"There is no existing option of the name {optionName} in the command {Name}");
        }


        /// <summary>
        /// Method used for extracting <see cref="Option"/> from a string containing the Option's name.
        /// </summary>
        /// <param name="optionName">The Name (identifier) of an option.</param>
        /// <returns>Returns an <see cref="Option"/> from its Name (identifier) or <see cref="null"/> if there is no such option.</returns>
        public Option? GetOption(string optionName) {
            foreach (var option in Options.Where(option =>
                         optionName == option.ShortName || optionName == option.LongName || option.Synonyms.Contains(optionName))) {
                return option;
            }

            throw new MissingOptionException(
                $"There is no existing option of the name {optionName} in the command {Name}");
        }


        /// <summary>
        /// Method indicating the presence of an <see cref="Option"/> in <see cref="Options"/> list with 
        /// <see cref="Option.LongName"/> or <see cref="Option.ShortName"/> being equal to the string <paramref name="optionName"/>.
        /// </summary>
        /// <param name="optionName">The Name (identifier) of an option.</param>
        /// <returns>Returns false if there is no option with this name, true if it is one of the recognized options.</returns>
        public bool HasOption(string optionName) {
            return Options.Any(option => optionName == option.ShortName || optionName == option.LongName);
        }
    }
}