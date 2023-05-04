using FluentValidation;
using OfficesAPI.Application.Commands.Offices.Create;

namespace OfficesAPI.Application.Validators
{
    public class CreateOfficeValidator : AbstractValidator<CreateOffice>
    {
        private const string _phoneRegex = "/^(\\+\\d{1,3}[- ]?)?\\d{10}$/";
        public CreateOfficeValidator()
        {
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.HouseNumber).GreaterThan(0);
            RuleFor(x => x.OfficeNumber).GreaterThan(0);

            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().Matches(_phoneRegex);
        }
    }
}
