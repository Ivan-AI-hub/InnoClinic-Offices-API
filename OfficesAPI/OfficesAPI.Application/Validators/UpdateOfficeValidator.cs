using FluentValidation;
using OfficesAPI.Application.Commands.Offices.Update;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Validators
{
    public class UpdateOfficeValidator : AbstractValidator<UpdateOffice>
    {
        private const string _phoneRegex = "^(\\+)?((\\d{2,3}) ?\\d|\\d)(([ -]?\\d)|( ?(\\d{2,3}) ?)){5,12}\\d$";
        public UpdateOfficeValidator(IOfficeRepository officeRepository)
        {
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.HouseNumber).GreaterThan(0);
            RuleFor(x => x.OfficeNumber).GreaterThan(0);

            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().Matches(_phoneRegex);

            RuleFor(x => x.Id).NotEmpty().NotNull()
                .Must(id => officeRepository.GetItemAsync(id).Result != null)
                .WithMessage(x => $"Office with ID = {x.Id} does not exist")
                .DependentRules(() =>
                {
                    RuleFor(m => new { m.Id, m.City, m.OfficeNumber })
                        .Must(m => IsOfficeNumberAndCityValidAsync(officeRepository, m.Id, m.City, m.OfficeNumber).Result)
                        .WithMessage(x => $"There is already an office at {x.OfficeNumber} in {x.City}");
                });

        }

        private async Task<bool> IsOfficeNumberAndCityValidAsync(IOfficeRepository officeRepository, Guid id, string city, int officeNumber)
        {
            var oldOffice = await officeRepository.GetItemAsync(id);
            if (oldOffice == null)
                return false;
            if (oldOffice.Address.City != city || oldOffice.OfficeNumber != officeNumber)
            {
                var isOfficeNumberInvalid = await officeRepository.IsItemExistAsync(x => x.Address.City == city && x.OfficeNumber == officeNumber);

                return !isOfficeNumberInvalid;
            }
            return true;
        }
    }
}
