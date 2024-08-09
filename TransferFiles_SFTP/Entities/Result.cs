namespace TransferFiles_SFTP.Entities
{
    public class Result<T>
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        private Result(T value, bool isSuccess, string message)
        {
            Value = value;
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, "Operation successfully completed.");
        public static Result<T> Failure(string error) => new Result<T>(default, false, error);
    }
}
