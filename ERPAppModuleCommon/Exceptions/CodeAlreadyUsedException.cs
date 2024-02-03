namespace ERPAppModuleCommon.Exceptions;

public class CodeAlreadyUsedException : Exception
{
    public CodeAlreadyUsedException(string code) : base(message: $"Provided code: {code} is already used!")
    {
    }
}