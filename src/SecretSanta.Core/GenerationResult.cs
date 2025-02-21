namespace SecretSanta;

public class GenerationResult<T> where T : notnull
{
    public bool IsSuccess { get; }

    public T Value { get; }

    public string Error { get; }

    private GenerationResult(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static GenerationResult<T> Success(T value) => new(true, value, string.Empty);

    public static GenerationResult<T> Failure(string error) => new(false, default!, error);

    public static implicit operator GenerationResult<T>(T value) => GenerationResult<T>.Success(value);
}