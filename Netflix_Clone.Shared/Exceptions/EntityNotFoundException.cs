namespace Netflix_Clone.Domain.Exceptions
{
    public class EntityNotFoundException(string Message) : Exception(Message)
    {
    }
}
