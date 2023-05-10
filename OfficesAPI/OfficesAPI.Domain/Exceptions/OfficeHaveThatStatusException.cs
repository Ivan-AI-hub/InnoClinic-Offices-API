namespace OfficesAPI.Domain.Exceptions
{
    public class OfficeHaveThatStatusException : BadRequestException
    {
        public OfficeHaveThatStatusException(bool status)
            : base($"Office already have status {status}")
        {
        }
    }
}
