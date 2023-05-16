using CLILibrary;
using CLILibrary.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace APITests1
{
	[TestClass]
	public class BasicParsedValueTests
	{
		Command cmd;
		[TestInitialize()]
		public void Setup()
		{
			cmd = new Command("testcommand", "usage", "v1.0.0");
		}


		[DataRow("shortString", "Short string")]
		[DataRow("multi words string", "Multiple words string")]
		[DataRow("multi\nline\nstring", "Multiple lines string")]
		[DataRow("   ", "Only whitespaces string")]
		[DataRow("", "Empty string")]
		[TestMethod]
		public void StringOptionHasCorrectParsedValue(string valueToTest, string? testNote)
		{
			var stringOption = new Option
			{
				ShortName = "s",
				LongName = "stringoption",
				Description = "string option",
				Type = ArgumentType.String,
				AcceptsParams = true,
				ArgumentCount = 1,
			};
			cmd.AddOption(stringOption, false);

			cmd.Parse(new string[2] { "-s", valueToTest});

			Assert.AreEqual(valueToTest, cmd.GetOptionValues("stringoption"));
		}

		[DataRow("42", "42", "Basic int number")]
		[DataRow("0042", "42", "Number begins with zeros")] //should be parsed to normal integer - honestly should be possible to store as int in my opinion
        [TestMethod]
		public void IntOptionHasCorrectParsedValue(string valueToTest, string expectedStoredValue, string? testNote)
		{
			var intOption = new Option
			{
				ShortName = "i",
				LongName = "intoption",
				Description = "string option",
				Type = ArgumentType.Int,
				AcceptsParams = true,
				ArgumentCount = 1,
			};
			cmd.AddOption(intOption, false);
            cmd.Parse(new string[2] { "-i", valueToTest });

			Assert.AreEqual(expectedStoredValue, cmd.GetOptionValues("intoption"));
		}
	}

	[TestClass]
	public class FlagOptionPresenceTests
	{
		Command cmd;
		[TestInitialize()]
		public void Setup()
		{
			cmd = new Command("testcommand", "usage", "v1.0.0");
		}

		[TestMethod]
		public void FlagOptionPresentAfterParsing()
		{
			var flagOption = new Option
			{
				ShortName = "f",
				LongName = "flag",
				Description = "flag option",
				Type = ArgumentType.Bool
			};
			cmd.AddOption(flagOption, false);

			cmd.Parse(new string[] { "-f"});
			//HAS TO BE IMPLEMENTED
			Assert.IsTrue(cmd.HasOption("flag")); 
		}


		[TestMethod]
		public void FlagOptionNotPresentWhileOtherOptionsAre()
		{
			var flagOption = new Option
			{
				ShortName = "f",
				LongName = "flag",
				Description = "flag option",
				Type = ArgumentType.Bool
			};
			var dummyOption = new Option
			{
				ShortName = "d",
				LongName = "dummy",
				Description = "dummy option",
				Type = ArgumentType.Bool
			};
			cmd.AddOption(flagOption, false);
			cmd.AddOption(dummyOption, false);
				
			cmd.Parse(new string[] { "-d" });

			//HAS TO BE IMPLEMENTED
			Assert.IsTrue(cmd.HasOption("flag"));
		}
	}

	[TestClass]
	public class SynonymsTests
	{
		Command cmd;
		[TestInitialize()]
		public void Setup()
		{
			cmd = new Command("testcommand", "usage", "v1.0.0");
		}

		[TestMethod]
		public void SameParsedValueWithLongAndShortName()
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = ArgumentType.String,
				ArgumentCount = 1,
				AcceptsParams = true,
			};
			cmd.AddOption(option, false);

			cmd.Parse(new string[] { "-o", "parameter" });

			Assert.AreEqual("parameter", cmd.GetOptionValues("option"));
			Assert.AreEqual("parameter", cmd.GetOptionValues("o"));
		}

		[DataRow(new string[] { "synonym1" })]
		[DataRow(new string[] { "synonym1", "synonym2" })]
		[TestMethod]
		public void SameParsedValueWithSynonyms(string[] synonyms)
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = ArgumentType.String,
				Synonyms = synonyms.ToList(),
				ArgumentCount = 1,
				AcceptsParams = true,
			};
			cmd.AddOption(option, false);

			cmd.Parse(new string[] { "-o", "parameter" });

			foreach(var synonym in synonyms)
			{
				Assert.AreEqual("parameter", cmd.GetOptionValues(synonym));
			}
		}
	}

    [TestClass]
	public class ArgumentCountTests
	{
		Command cmd;
		[TestInitialize()]
		public void Setup()
		{
			cmd = new Command("testcommand", "usage", "v1.0.0");            
		}

		[TestMethod]
		public void CorrectlyPersesOneParameter()
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = ArgumentType.String,
				ArgumentCount = 1,
				AcceptsParams = true
			};
			cmd.AddOption(option, false);

			cmd.Parse(new string[] { "-o", "value" });

			Assert.AreEqual("value", cmd.GetOptionValues("o"));
		}

		[TestMethod]
		[DataRow(new string[] { "-o" }, 1, "ArgumentCount=1, 0 arguments provided")]
		[DataRow(new string[] { "-o" }, 3, "ArgumentCount=3, 0 arguments provided")]
		[DataRow(new string[] { "-o", "arg1", "arg2" }, 3, "ArgumentCount=3, 2 arguments provided")]
		public void NonZeroArgumentCountFailsOnLessParametersProvided(string[] args, int argCount, string message)
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = ArgumentType.String,
				ArgumentCount = argCount,
				AcceptsParams = true
			};
			cmd.AddOption(option, false);

			Assert.ThrowsException<OptionParseException>(() => cmd.Parse(args));
		}
	}

	[TestClass]
	public class ArgumentTypeTests
	{
		Command cmd;
		[TestInitialize()]
		public void Setup()
		{
			cmd = new Command("testcommand", "usage", "v1.0.0");
		}

        [TestMethod]
        [DataRow(ArgumentType.Int, new string[] {"-o", "randomWord"}, "Type: int, value: string")]
		[DataRow(ArgumentType.Int, new string[] { "-o", "42xyz42" }, "Type: int, value: number with text")]
        public void ParsingFailsOnIncorrectTypeProvidedOneParameter(ArgumentType type, string[] args, string message)
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = type,
				ArgumentCount = 1,
				AcceptsParams = true
			};
			cmd.AddOption(option, false);

			Assert.ThrowsException<UnsupportedValueTypeException>(() => cmd.Parse(args));
		}

        [TestMethod]
        [DataRow(ArgumentType.Int, new string[] { "-o", "	" }, "Type: int, value: blank spaces string")]
        [DataRow(ArgumentType.Int, new string[] { "-o", "\n" }, "Type: bool, value: newline string")]		
		public void ParsingFailsOnBlankStringParameter(ArgumentType type, string[] args, string message)
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = type,
				ArgumentCount = 1,
				AcceptsParams = true
			};
			cmd.AddOption(option, false);

			Assert.ThrowsException<UnsupportedValueTypeException>(() => cmd.Parse(args));
		}

		[TestMethod]
		[DataRow("o")]
        [DataRow("option")]
		[DataRow("synonym")]
        public void GetOptionReturnsSameOption(string getName)
		{
			var option = new Option
			{
				ShortName = "o",
				LongName = "option",
				Description = "option",
				Type = ArgumentType.Int,
				Synonyms = new List<string> { "synonym" }
			};
			cmd = new Command();

			cmd.AddOption(option, false);

			Assert.AreSame(option, cmd.GetOption(getName));
		}
	}
}