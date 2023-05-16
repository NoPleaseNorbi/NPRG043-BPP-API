using CLILibrary;
using CLILibrary.Exceptions;

namespace APITests2 {
    public class PlainArgumentTests {
        [Test]
        public void TestPlainArgumentConstructor() {
            PlainArgument argument = new PlainArgument();
            Assert.IsNotNull(argument);
        }
    }

    public class OptionTests {
        [Test]
        public void TestOptionConstructor() {
            Option option = new Option();
            Assert.IsNotNull(option);
        }
    }

    public class CommandTests {
        [Test]
        public void TestCommandConstructor() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            Assert.IsNotNull(command);
        }

        [Test]
        public void TestAddOption() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            Option option = new Option { LongName = "option", ShortName = "o" };
            command.AddOption(option, false);
            Assert.IsTrue(command.HasOption("option"));
            Assert.IsTrue(command.HasOption("o"));
        }

        [Test]
        public void TestAddOptions() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            Option option1 = new Option { LongName = "option1", ShortName = "o" };
            Option option2 = new Option { LongName = "option2", ShortName = "p" };
            command.AddOptions((option1, false), (option2, true));
            Assert.IsTrue(command.HasOption("option1"));
            Assert.IsTrue(command.HasOption("o"));
            Assert.IsTrue(command.HasOption("option2"));
            Assert.IsTrue(command.HasOption("p"));
        }

        [Test]
        public void TestGetOption() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            Option option = new Option { LongName = "option", ShortName = "o" };
            command.AddOption(option, false);
            Option retrievedOption = command.GetOption("option");
            Assert.AreEqual(option, retrievedOption);
        }

        [Test]
        public void TestGetOptionValue() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            Option option = new Option { LongName = "option", ShortName = "o" };
            command.AddOption(option, false);
            option.Values.Add("option");
            string optValue = command.GetOptionValues("option");
            Assert.AreEqual("option", optValue);
        }

        [Test]
        public void TestHasOption() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            Option option = new Option { LongName = "option", ShortName = "o" };
            command.AddOption(option, false);
            Assert.IsTrue(command.HasOption("option"));
            Assert.IsTrue(command.HasOption("o"));
        }

        [Test]
        public void TestParse() {
            Command command = new Command("my-command", "my-command usage", "1.0.0");
            string[] strings = { "-a" };
            Assert.Throws<MissingOptionException>(() => command.Parse(strings));
        }
    }
}
