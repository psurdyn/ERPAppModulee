namespace ERPAppModuleCommon.Result;

public class ExceptionResult
{
    public ExceptionResult(Exception exception, int statusCode)
    {
        Exception = exception;
        StatusCode = statusCode;
    }

    public Exception Exception { get; set; }
    public int StatusCode { get; set; }
}