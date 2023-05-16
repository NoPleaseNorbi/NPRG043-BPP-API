using CLILibrary;
using CLILibrary.Exceptions;

namespace APITests1
{
	[TestClass]
	public class OptionDefinitionTests
	{
		[TestMethod]
		public void DefinitionFailsOnMoreThanOneCharacterShortName()
		{
            Option option;

            //TODO: In the CLILIbrary add checking 
            Assert.ThrowsException<OptionException>(() =>
                option = new Option
                {
                    ShortName = "ooo",
                    LongName = "option",
                    Description = "option",
                    Type = ArgumentType.String
                }
            ); ;
		}

        [TestMethod]
        [DataRow("1option", "longName starts with a number")]
        [DataRow("-option", "longName starts with -")]
        [DataRow("*option", "longName starts with *")]
        [DataRow(" option", "longName starts with a space")]
        [DataRow("\noption", "longName starts with a newline")]
        [DataRow("\toption", "longName starts with a tab")]
        [DataRow("option\n", "longName contains a newline")]
        [DataRow("option\noption\noption", "longName contains multiple newlines")]
        [DataRow("1234", "longName is a number")]
        [DataRow("option option", "longName contains a space")]
        [DataRow("  ", "longName is only spaces")]
        [DataRow("\t\t", "longName is only tabs")]
        [DataRow("o", "longName is short (1 character)")]
        [DataRow("", "longName is empty string")]
        public void DefinitionFailsOnIncorrectLongName(string longName, string message)
        {
            Option option;

            //TODO: Insert Exception type, that ISN'T CURRENTLY SPECIFIED IN THE API
            Assert.ThrowsException<OptionException>(() =>
                option = new Option
                {
                    ShortName = "o",
                    LongName = longName,
                    Description = "option",
                    Type = ArgumentType.String
                }
            ); ;
        }

        [TestMethod]
        [DataRow("1", "shortName is a number")]
        [DataRow("-", "shortName is a -")]
        [DataRow("*", "shortName is a *")]
        [DataRow(" ", "shortName is a space")]
        [DataRow("\n", "shortName is a newline")]
        [DataRow("\t", "shortName is a tab")]
        [DataRow("a\n", "shortName contains a newline")]
        [DataRow("\na", "shortName starts with a newline")]
        [DataRow("abcd", "shortName is too long (more than 1 character)")]
        [DataRow("", "shortName is empty string")]

        public void DefinitionFailsOnIncorrectShortName(string shortName, string message)
        {
            Option option;

            //TODO: Insert Exception type, that ISN'T CURRENTLY SPECIFIED IN THE API
            Assert.ThrowsException<OptionException>(() =>
                option = new Option
                {
                    ShortName = shortName,
                    LongName = "option",
                    Description = "option",
                    Type = ArgumentType.String
                }
            );
        }


        

        [TestMethod]
        public void DefinitionFailsOnNegativeArgumentCount()
        {
            Option option;

            //TODO: Insert Exception type, that ISN'T CURRENTLY SPECIFIED IN THE API
            Assert.ThrowsException<OptionException>(() =>
            option = new Option
            {
                ShortName = "o",
                LongName = "option",
                Description = "option",
                Type = ArgumentType.Int,
                ArgumentCount = -1
            }
            );
        }

    }

    [TestClass]
    public class CommandDefinitionTests
    {
        [TestMethod]
        public void DefinitionFailsOnNewOptionWithAlreadyExistingShortName()
        {
            Option option1 = new Option
            {
                ShortName = "o",
                LongName = "option1",
                Description = "option",
                Type = ArgumentType.Int
            };
            Option option2 = new Option
            {
                ShortName = "o",
                LongName = "option2",
                Description = "option",
                Type = ArgumentType.Int
            };

            var cmd = new Command();
            cmd.AddOption(option1, false);

            //TODO: Insert Exception type, that ISN'T CURRENTLY SPECIFIED IN THE API
            Assert.ThrowsException<DuplicateException>(() =>
                cmd.AddOption(option2, false)
            ) ;
        }

        [TestMethod]
        public void DefinitionFailsOnNewOptionWithAlreadyExistingLongName()
        {
            Option option1 = new Option
            {
                ShortName = "o",
                LongName = "option",
                Description = "option",
                Type = ArgumentType.Int
            };
            Option option2 = new Option
            {
                ShortName = "o",
                LongName = "option",
                Description = "option",
                Type = ArgumentType.Int
            };

            var cmd = new Command();
            cmd.AddOption(option1, false);

            Assert.ThrowsException<DuplicateException>(() =>
                cmd.AddOption(option2, false)
            );
        }

        [TestMethod]
        [DataRow(new string[] {}, new string[] { "o1" }, "synonym of 2nd is short of 1st")]
        [DataRow(new string[] {}, new string[] { "option1" }, "synonym of 2nd is long of 1st")]
        [DataRow(new string[] { "o2" }, new string[] {}, "synonym of 1st is short of 2nd")]
        [DataRow(new string[] { "option2" }, new string[] {}, "synonym of 1st is long of 2nd")]
        [DataRow(new string[] { "option3" }, new string[] { "option3" }, "synonyms of both contain same synonym")]
        public void DefinitionFailsOnNewOptionWithAlreadyExistingSynonym(string[] synonyms1, string[] synonyms2, string message)
        {
            Option option1 = new Option
            {
                ShortName = "o",
                LongName = "option1",
                Description = "option",
                Type = ArgumentType.Int,
                Synonyms = synonyms1.ToList()
            };
            Option option2 = new Option
            {
                ShortName = "o",
                LongName = "option2",
                Description = "option",
                Type = ArgumentType.Int,
                Synonyms = synonyms2.ToList()
            };

            var cmd = new Command();
            cmd.AddOption(option1, false);

            Assert.ThrowsException<DuplicateException>(() =>
                cmd.AddOption(option2, false)
            );
        }

    }
}
