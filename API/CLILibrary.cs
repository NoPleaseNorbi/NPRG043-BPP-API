using System;

namespace CLILibrary
{
    public enum OptionType
    {
        None,
        String,
        Integer,
        Boolean,
        Enumeration
    }

    public class Option
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public OptionType Type { get; set; }
        public object DefaultValue { get; set; }
        public object Value { get; set; }
        public List<string> Synonyms { get; set; }
        public bool IsRequired { get; set; }
        public bool IsFlag { get; set; }
        public bool IsSet { get; set; }
        public List<string> AllowedValues { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class CommandLineParser
    {
        public List<string> PlainArguments { get; set; }
        public List<Option> Options { get; set; }
        public List<Option> RequiredOptions { get; set; }
        public string Usage { get; set; }
        public string Version { get; set; }
        

        public void AddOption(Option option) 
        {
            Console.WriteLine("Not Implemented");
        }
        public void Parse(string[] args) 
        {
            Console.WriteLine("Not Implemented");
        }
        public string GetOptionValue(string optionName) 
        {
            return "Not Implemented";
        }
        public bool HasOption(string optionName) 
        {
            return true;
        }
    }
}