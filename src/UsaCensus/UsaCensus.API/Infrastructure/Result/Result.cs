namespace UsaCensus.API.Infrastructure.Result;

public class Result<T>
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public string ErrorMessage { get; } = string.Empty;

    private Result(bool isSuccess, T? value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, string.Empty);

    public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
}
