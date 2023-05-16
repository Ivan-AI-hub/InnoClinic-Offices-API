using OfficesAPI.Domain.Exceptions;

namespace OfficesAPI.Application.Exceptions
{
    public class BlobNameIsNotValidException : BadRequestException
    {
        public BlobNameIsNotValidException(string name)
            : base($"File with the {name} name already exist in database")
        {
        }
    }
}
