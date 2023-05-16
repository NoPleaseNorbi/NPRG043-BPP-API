# CLILibrary

## Project Overview

CLILibrary is a command line parsing library targeting .NET Framework 6.0 or greater, that provides command line parsing in an easy and understandable way. 

## Dependencies and Requirements

- .NET Core 6.0 or later

## Building the Project

- Clone the project repository to your local machine using the following command:
`git clone https://gitlab.mff.cuni.cz/teaching/nprg043/2023-summer/task-1/t14-api-design.git`
- Open a terminal window and navigate to the `CLILibrary` directory.
- Run the command `dotnet build` to build the project.

## Documentation:

- Ensure that you have installed Doxygen on your system using the `doxygen --version` command.
- Run the command `doxygen doxygen.config` in the `CLILibrary` folder to generate the documentation.
- The generated documentation will be located in the `html` directory. You can view the documentation by opening the `index.html` file in the `html` directory in a web browser, or using the following command: `./html/index.html` assuming you are in the CLILibrary folder.

## Examples

- Here are the examples of using the `CLILibrary` for command line parsing:
  
### Simple example

In the following example is shown how can the user define a string option.

- In the contructor of the `Command` class the user can define the name of the command, the usage and the help information.
- The user can add options to the command using the `Option` class, the user defines the following attributes in the following example: `ShortName`, `LongName`, `Description`, `Type`. 
```csharp
var parser = new Command("time", "Usage info", "1.0.0");

var formatOption = new Option
{
    ShortName = "f",
    LongName = "format",
    Description = "Specify output format, possibly overriding the format specified in the environment variable TIME.",
    Type = ArgumentType.String,
};

parser.AddOption(formatOption, false);
```

### Example using all of the attributes
- The following example demonstrates, how the user is defining a string option using all the available option parameters.

```csharp
var parser = new Command("time", "Usage info", "1.0.0");

var ExampleOption = new Option
{
    ShortName = "e",
    LongName = "example",
    Description = "This is an example option",
    Synonyms = new List<string> { "example2", "example3" },
    Type = ArgumentType.Int,
    DefaultValue = "ExampleDefault",
    ArgumentCount = 56,
    AcceptsParams = true,
    IncompatibleOptions = new List<Option> { new Option {ShortName = "ea" } },
    RequiredOptions = new List<Option> { },
    MaxVal = 1000,
    MinVal = 500,
};

parser.AddOption(ExampleOption, false);
```

### Example of defining options and parsing
- In this example we see three options added to our command called `testcommand`. After the three options are added, we parse our array of options given by the user.
```csharp
cmd = new Command("testcommand", "usage", "v1.0.0");
option1 = new Option
{
    ShortName = "o1",
    LongName = "option1",
    Description = "option1",
    Type = ArgumentType.String,
    ArgumentCount = 1,
    AcceptsParams = true
};
option2 = new Option
{
    ShortName = "o2",
    LongName = "option2",
    Description = "option2",
    Type = ArgumentType.String,
    ArgumentCount = 1,
    AcceptsParams = true,
    IncompatibleOptions = new List<Option> { option1 }
};
option3 = new Option
{
    ShortName = "o3",
    LongName = "option3",
    Description = "option3",
    Type = ArgumentType.String,
    ArgumentCount = 1,
    AcceptsParams = true,
    IncompatibleOptions = new List<Option> { option2 }
};

cmd.AddOption(option1, false);
cmd.AddOption(option2, false);
cmd.AddOption(option3, false);
cmd.Parse(new string[] { "-o1", "str1", "-o3", "str3"});
```

## Running the Project

- Open a terminal window and navigate to the project directory.
- Locate the Tester folder using the `cd Tester` command (Or you custom made project).
- Run the command `dotnet run` to start the application.

## Running Tests

To run tests for the project, follow these steps:

- Ensure that you have installed the .NET Core SDK and the NUnit testing framework.
- Open a terminal window and navigate to the `CLILibrary` directory.
- Run the command `dotnet test` to run the unit tests using the MSUnit framework.
