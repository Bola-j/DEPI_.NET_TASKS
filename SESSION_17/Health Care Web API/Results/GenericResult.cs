using Microsoft.Identity.Client;

namespace Health_Care_Web_API.Results
{
    public class GenericResult<T> : Result
    {
        public T Value { get; }

        protected GenericResult(bool isSuccess, T value, string error) : base(isSuccess, error)
            => Value = value;

        public static GenericResult<T> Success(T value)
            => new GenericResult<T>(true, value, string.Empty);

        public static GenericResult<T> Failure(string error)
            => new GenericResult<T>(false, default, error);
    }
}
