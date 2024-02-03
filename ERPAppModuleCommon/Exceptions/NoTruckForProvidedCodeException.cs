namespace ERPAppModuleCommon.Exceptions;

public class NoTruckForProvidedCodeException : Exception
{
    public NoTruckForProvidedCodeException(string code) : base(message: $"No truck has been found for code : {code}")
    {
    }
}