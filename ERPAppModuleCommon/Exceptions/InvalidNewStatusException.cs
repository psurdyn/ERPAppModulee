namespace ERPAppModuleCommon.Exceptions;

public class InvalidNewStatusException : Exception
{
    public InvalidNewStatusException(string newStatus) : base(message: $"A status: {newStatus} can'T be assigned to a truck, because it violates the phases order!")
    {
        
    }
}