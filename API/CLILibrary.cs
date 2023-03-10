using System;

namespace CLILibrary
{
    enum Types
    {
        BOOL, STRING, INT, LONG, FLOAT, DOUBLE
    }
    class Tester 
    {
        public void testerfunc() 
        {
            Console.WriteLine("test working");
        }
    }
    class Parser
    {
        string delimiter;
    }
    class CommandLine
    {
        readonly List<Command> commands;

    }
    abstract class Command
    {
        readonly List<Parameter> options;
        string Name { get; set; }
        string Description { get; set; }
        public abstract void Run();

    }
    class Parameter
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public List<Types> supported = new();
        public bool IsOptional { get; set; }
        public Parameter(string shortname, string longname)
        {
            ShortName = shortname;
            LongName = longname;
        }
        public void Run<T>(T value)
        {

        }


    }
}