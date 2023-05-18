namespace OfficesAPI.Domain.Exceptions
{
    public class CityAlreadyHaveOfficeWithThatNumberException : BadRequestException
    {
        public CityAlreadyHaveOfficeWithThatNumberException(string city, int officeNumber)
            : base($"There is already an office at {officeNumber} in {city}")
        {
        }
    }
}
