using System;
#nullable enable
///<summary>
///A library for Command Line Interface, used for parsing command line arguments of a command and manipulating with them.
///<summary>
namespace CLILibrary
{
    ///<summary>
    ///An enumeration signaling the type of <see cref="Argument.Value"/>. <see cref="ArgumentType.Bool"/> -> bool, <see cref="ArgumentType.String"/> -> string,
    ///<see cref="ArgumentType.Int"/> -> int, <see cref="ArgumentType.Null"/> -> null and <see cref="ArgumentType.Object"/> -> boxed object.
    ///<summary>
    public enum ArgumentType {
        Null,
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
        public int? MaxVal { get; set; }
        /// <value>Lower boundary for the Value of the argument.</value>
        public int? MinVal { get; set; }
    }
    ///<summary>
    ///A class inheriting from the base class <see cref="Argument"/> and only removing the abstract modifier from it.
    ///Characterizes an Argument for the <see cref="Command"/> with no name, only a value.
    ///<summary>
    public sealed class PlainArgument : Argument {

    }
    ///<summary>
    ///<remarks>A class inheriting from the base class <see cref="Argument"/> and  extending it as a "storage" for one <see cref="Command"/> option.</remarks>
    ///<summary>
    public sealed class Option : Argument
    {
        /// <value>The long name of the Option, if set.</value>
        public string? LongName { get; set; }
        /// <value>The short name of the Option, if set.</value>
        public string? ShortName { get; set; }
        /// <value>Description of the usage for the Option.</value>
        public string? Description { get; set; }
        /// <value>List of all recognized Names for the Option.</value>
        public List<string>? Synonyms { get; set; }
        /// <value>The name of the type in which the Option operates.</value>
        public ArgumentType Type { get; set; }
        /// <value>Default value of the Option.</value>
        public string? DefaultValue { get; set; }
        /// <value>Number of possible arguments in the Option.</value>
        public int? ArgumentCount { get; set; }
        /// <value>A bool property indicating if the value of the Option has been set.</value>
        public bool? IsSet { get; set; }
        /// <value>A bool property indicating whether the Option allows for any parameters after it.</value>
        public bool? AcceptsParams { get; set; }
    }
    ///<summary>
    ///A class representing a Command and it's settings. Stores all of the defined Options for it and is responsible for parsing and processing the command line input.
    ///<summary>
    public sealed class Command
    {
        /// <value>Name of the command.</value>
        public string? Name { get; set; }
        /// <value>General usage description and manual for the given command and its options.</value>
        public string? Description { get; set; }
        /// <value>List of all recognized Names for the command.</value>
        public List<string>? Synonyms { get; set; }
        /// <value>List of all recognized Options for the command.</value>
        public List<Option>? Options { get; set; }
        /// <value>List of all Options that are mandatory for the command.</value>
        public List<Option>? RequiredOptions { get; set; }
        /// <value>A property containing the version of the command.</value>
        public string? Version { get; set; }
        /// <value>A property indicating if the delimiter set for distinguishing plain arguments has been found during parsing.</value>
        public bool FoundDelimiter { get; set; } = false;
        /// <value>The delimiter set for distinguishing plain arguments.</value>
        public string? Delimiter { get; set; }
        /// <summary>
        /// Basic constructor for the class <see cref="Command"/>
        /// </summary>
        public Command() { }
        /// <summary>
        /// A three parameter constructor initializing properties <see cref="Name"/>, <see cref="Description"/> and <see cref="Version"/>.
        /// </summary>
        /// <param name="name">Initializes <see cref="Name"/></param>
        /// <param name="usage">Initializes <see cref="Description"/></param>
        /// <param name="version">Initializes <see cref="Version"/></param>
        public Command(string name, string usage, string version)
        {
            Name = name;
            Description = usage;
            Version = version;
        }
        /// <summary>
        /// Method responsible for the core functionality of the command, serves as a connector among all necessary function to parse,
        /// check and process options and finally, run the command.
        /// </summary>
        /// <param name="args">User input after command name (e.g. Options, Arguments, Plain Arguments).</param>
        public void Run(string args) 
        {
            return;
        }
        /// <summary>
        /// Method used for parsing of the command line arguments and allowing for other methods to process them as type <see cref="Argument"/>.
        /// <remarks><see cref="Argument"/> type can be both <see cref="PlainArgument"/> or an <see cref="Option"/>.</remarks>
        /// </summary>
        /// <param name="args">User input after command name (e.g. Options, Arguments, Plain Arguments).</param>
        /// <returns>Returns null if there were no arguments, otherwise returns an array of user-selected Arguments and sets their values(if possible).</returns>
        public Argument[]? Parse(string args) {
            return null;
        }
        /// <summary>
        /// Method adding an <see cref="Option"/> object to <see cref="Options"/> or <see cref="RequiredOptions"/>
        /// depending on the value of the <paramref name="Required"/> value.
        /// </summary>
        /// <param name="option">The Option to be added.</param>
        /// <param name="Required">A bool value indicating if the Option being added is mandatory.</param>
        public void AddOption(Option option, bool Required) 
        {
            return;
        }
        /// <summary>
        /// Method adding one or more <see cref="Option"/> objects to <see cref="Options"/> or <see cref="RequiredOptions"/>
        /// depending on the value of the second item in the <paramref name="option"/> tuple.
        /// </summary>
        /// <param name="option">Any number of 2 element tuples with types Option, bool that indicate in order: The Option to be added and a bool value indicating if it is mandatory.</param>
        public void AddOptions(params (Option,bool)[] option)
        {
            return;
        }
        /// <summary>
        /// Method used for extracting <see cref="Argument.Value"/> from a string containing the Option's name.
        /// </summary>
        /// <param name="optionName">The Name (identifier) of an option.</param>
        /// <returns>Returns the Value of an Option identified by its Name.</returns>
        public string? GetOptionValue(string optionName) 
        {
            return null;
        }
        /// <summary>
        /// Method used for extracting <see cref="Option"/> from a string containing the Option's name.
        /// </summary>
        /// <param name="optionName">The Name (identifier) of an option.</param>
        /// <returns>Returns an <see cref="Option"/> from its Name (identifier) or <see cref="null"/> if there is no such option.</returns>
        public Option? GetOption(string optionName) {
            return null;
        }
        /// <summary>
        /// Method indicating the presence of an <see cref="Option"/> in <see cref="Options"/> list with 
        /// <see cref="Option.LongName"/> or <see cref="Option.ShortName"/> being equal to the string <paramref name="optionName"/>.
        /// </summary>
        /// <param name="optionName">The Name (identifier) of an option.</param>
        /// <returns>Returns false if there is no option with this name, true if it is one of the recognized options.</returns>
        public bool HasOption(string optionName) 
        {
            return false;
        }
    }
}