namespace ERPAppModuleCommon.Exceptions;

public class StatusNotFoundException : Exception
{
    public StatusNotFoundException(string statusId) : base(message: $"Status with id: {statusId} has not been found!")
    {
    }
}