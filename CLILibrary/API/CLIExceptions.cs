namespace CLILibrary.Exceptions {
    /// <summary>
    /// General Command Line Exception. Superclass for all the exceptions in CLI Parser.
    /// </summary>
    [Serializable]
    public class CLIException : Exception {
        public CLIException(string message) : base(message) { }
    }
    /// <summary>
    /// Thrown when adding an option with existing name, adding duplicate synonym or any other duplicate name within the defined collections.
    /// </summary>
    [Serializable]
    public class DuplicateException : CLIException {
        public DuplicateException(string message) : base(message) { }
    }
    /// <summary>
    /// Thrown when the option is not found within the supported list.
    /// </summary>
    [Serializable]
    public class MissingOptionException : CLIException {
        public MissingOptionException(string message) : base(message) { }
    }
    /// <summary>
    /// Thrown when an unexpected error occurs when defining/running option.
    /// </summary>
    [Serializable]
    public class OptionException : CLIException {
        public OptionException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when value provided to the option/command is not supported by the option or command
    /// </summary>
    [Serializable]
    public class UnsupportedValueTypeException : CLIException {
        public UnsupportedValueTypeException(string message) : base(message) { }
    }
    /// <summary>
    /// Thrown when the value provided is not within the predefined range of value of the option.
    /// </summary>
    [Serializable]
    public class ValueOutOfBoundsException : CLIException {
        public ValueOutOfBoundsException(string message) : base(message) { }
    }
    /// <summary>
    /// Thrown when the name does not correspond to any option
    /// </summary>
    [Serializable]
    public class OptionNameException : CLIException {
        public OptionNameException(string message) : base(message) { }
    }
    /// <summary>
    /// Thrown when an error occurs during parsing
    /// </summary>
    [Serializable]
    public class OptionParseException : CLIException {
        public OptionParseException(string message) : base(message) { }
    }

}
