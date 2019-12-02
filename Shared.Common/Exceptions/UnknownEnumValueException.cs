using System;

namespace Shared.Common.Exceptions
{
    /// <summary>
    /// Exception that is invoked if enum was updated to contain additional value, but code where enum was used was not updated accordingly.
    /// </summary>
    public sealed class UnknownEnumValueException : Exception
    {
        public UnknownEnumValueException() { }

        public UnknownEnumValueException(string message) : base(message) { }

        public UnknownEnumValueException(string message, Exception inner) : base(message, inner) { }
    }
}