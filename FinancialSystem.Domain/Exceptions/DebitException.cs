using System;

namespace FinancialSystem.Domain.Exceptions
{
    public class DebitException : Exception
    {
        public DebitException() : base() { }

        public DebitException(string message) : base(message) { }

        public DebitException(string message, Exception innerException) : base(message, innerException) { }
    }
}
