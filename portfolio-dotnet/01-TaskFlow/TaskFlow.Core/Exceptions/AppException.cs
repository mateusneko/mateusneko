namespace TaskFlow.Core.Exceptions;
public abstract class AppException(string message) : Exception(message)
{
    public abstract int HttpStatus { get; }
}
public sealed class NotFoundException(string message) : AppException(message)
{
    public override int HttpStatus => 404;
}
public sealed class ConflictException(string message) : AppException(message)
{
    public override int HttpStatus => 409;
}
public sealed class BusinessRuleException(string message) : AppException(message)
{
    public override int HttpStatus => 422;
}
