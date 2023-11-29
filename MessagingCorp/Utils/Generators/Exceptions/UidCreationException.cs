namespace MessagingCorp.Common.Generators.Exceptions
{
    public class UidCreationException : Exception
    {
        public UidCreationException() { }
        public UidCreationException(string message) : base(message) { }
        public UidCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
