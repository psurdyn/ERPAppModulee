namespace ERPAppModuleCommon.Exceptions;

public class StatusNotFoundException : Exception
{
    public StatusNotFoundException(int statusId) : base(message: $"Status with id: {statusId} has not been found!")
    {
    }
}