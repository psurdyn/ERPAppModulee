namespace ERPAppModuleCommon.Exceptions;

public class InvalidStatusException : Exception
{
    public InvalidStatusException(string statusName) : base(message: $"Status: {statusName} is invalid!")
    {
    }
}