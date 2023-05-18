namespace OfficesAPI.Domain.Exceptions
{
    public class OfficeNotFoundException : NotFoundException
    {
        public OfficeNotFoundException(Guid id)
            : base($"Office with ID = {id} does not exist")
        {
        }
    }
}
