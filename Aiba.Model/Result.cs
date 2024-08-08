namespace Aiba.Model
{
    public class Result
    {
        private Result(string message, bool isSuccess = true)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public string Message { get; }

        public bool IsSuccess { get; private set; }

        public static Result Success()
        {
            return new Result(string.Empty);
        }

        public static Result Failure(string message)
        {
            return new Result(message, false);
        }
    }

    public class Result<T>
    {
        private Result(T? value, string message, bool isSuccess = true)
        {
            Value = value;
            Message = message;
            IsSuccess = isSuccess;
        }

        public T? Value { get; }

        public bool IsSuccess { get; private set; }
        public string Message { get; }

        public static Result<T?> Success(T? value)
        {
            return new Result<T?>(value, string.Empty);
        }

        public static Result<T?> Failure(string message)
        {
            return new Result<T?>(default, message, false);
        }
    }
}