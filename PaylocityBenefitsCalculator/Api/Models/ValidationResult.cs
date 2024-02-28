namespace Api.Models;

public sealed class ValidationResult<T>
{
    public T? Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; } = string.Empty;

    public static ValidationResult<T> GetSuccess(T data)
    {
        return new ValidationResult<T>
        {
            Data = data,
            Error = string.Empty,
            IsSuccess = true
        };
    }

    public static ValidationResult<T> GetFailure(string error)
    {
        return new ValidationResult<T>
        {
            Data = default,
            Error = error,
            IsSuccess = false
        };
    }
}

