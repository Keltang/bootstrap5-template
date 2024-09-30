namespace BlackBox.Domain.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public string ParamName { get; }

        public BusinessRuleException() : base()
        {
            ParamName = "Unknown";
        }

        public BusinessRuleException(string paramName, string message) : base(message)
        {
            ParamName = paramName;
        }
    }
}
