namespace ERPAppModuleCommon.Exceptions;

public class InvalidTruckIdException : Exception
{
    public InvalidTruckIdException(int id) : base(message: $"An entity with id: {id} cannot be found")
    {
    }
}