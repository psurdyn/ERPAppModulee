namespace ERPAppModuleCommon.Exceptions;

public class InvalidCodeFormatException : Exception
{
    public InvalidCodeFormatException(string code) : base(message: $"Provided code: {code} has invalid format!")
    {
        
    }
}