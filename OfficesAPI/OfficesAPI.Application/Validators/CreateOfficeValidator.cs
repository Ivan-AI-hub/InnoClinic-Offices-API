using FluentValidation;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Validators
{
    public class CreateOfficeValidator : AbstractValidator<CreateOffice>
    {
        private const string _phoneRegex = "^(\\+)?((\\d{2,3}) ?\\d|\\d)(([ -]?\\d)|( ?(\\d{2,3}) ?)){5,12}\\d$";
        public CreateOfficeValidator(IOfficeRepository officeRepository)
        {
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.HouseNumber).GreaterThan(0);
            RuleFor(x => x.OfficeNumber).GreaterThan(0);

            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().Matches(_phoneRegex);

            RuleFor(m => new { m.City, m.OfficeNumber })
                .Must(m => IsOfficeNumberAndCityValidAsync(officeRepository, m.City, m.OfficeNumber).Result)
                .WithMessage(x => $"There is already an office at {x.OfficeNumber} in {x.City}");
        }

        private async Task<bool> IsOfficeNumberAndCityValidAsync(IOfficeRepository officeRepository, string city, int officeNumber)
        {
            var isOfficeNumberInvalid = await officeRepository.IsItemExistAsync(x => x.Address.City == city && x.OfficeNumber == officeNumber);

            return !isOfficeNumberInvalid;
        }
    }
}
