namespace OfficesAPI.Domain.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string? message) : base(message)
        {
        }
    }
}
