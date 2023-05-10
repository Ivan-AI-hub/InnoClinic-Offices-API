using FluentValidation;
using OfficesAPI.Application.Commands.Offices.UpdateStatus;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Validators
{
    public class UpdateOfficeStatusValidator : AbstractValidator<UpdateOfficeStatus>
    {
        public UpdateOfficeStatusValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
