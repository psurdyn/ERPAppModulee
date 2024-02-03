namespace ERPAppModuleCommon.Result;

public class Result
{
    private Result(bool isSuccess, ExceptionResult? exceptionResult)
    {
        IsSuccess = isSuccess;
        ExceptionResult = exceptionResult;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ExceptionResult? ExceptionResult { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(Exception exception, int statusCode) =>
        new(false, new ExceptionResult(exception, statusCode));
}

public class Result<T> where T : class
{
    private Result(bool isSuccess, ExceptionResult? exceptionResult, T? response)
    {
        IsSuccess = isSuccess;
        ExceptionResult = exceptionResult;

        if (isSuccess)
        {
            Response = response;
        }
    }
    
    public T? Response { get; init; }
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    public ExceptionResult ExceptionResult { get; }
    
    public static Result<T> Success(T response) => new(true, null, response);
    public static Result<T> Failure(Exception exception, int statusCode) => new(false, new ExceptionResult(exception, statusCode), null);
    public static Result<T> Failure(ExceptionResult exceptionResult) => new(false, exceptionResult, null);
}