namespace BlackBox.Domain.Common
{
    public class Result<T>
    {
        public bool Succeeded { get; private set; }

        public string[] Errors { get; private set; }
        public T Data { get; private set; }

        private Result(bool succeeded, IEnumerable<string> errors, T data)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Data = data;
        }
        public static Result<T> Failure(IEnumerable<string> errors, T details)
        {
            return new Result<T>(false, errors, details);
        }

        public static Result<T> Failure(string error, T details)
        {
            return new Result<T>(false, new[] { error }, details);
        }

        public static Result<T> Success(T details)
        {
            return new Result<T>(true, Array.Empty<string>(), details);
        }
    }
}
