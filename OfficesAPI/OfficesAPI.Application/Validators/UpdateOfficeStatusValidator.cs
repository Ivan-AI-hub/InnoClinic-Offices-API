using FluentValidation;
using OfficesAPI.Application.Commands.Offices.UpdateStatus;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Validators
{
    public class UpdateOfficeStatusValidator : AbstractValidator<UpdateOfficeStatus>
    {
        public UpdateOfficeStatusValidator(IOfficeRepository officeRepository)
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();

            RuleFor(m => new { m.Id, m.NewStatus })
                .Must(x => IsStatusValidAsync(officeRepository, x.Id, x.NewStatus).Result)
                .WithMessage(x => $"Office already have status {x.NewStatus}");
        }

        private async Task<bool> IsStatusValidAsync(IOfficeRepository officeRepository, Guid id, bool status)
        {
            var office = await officeRepository.GetItemAsync(id);
            return office.Status != status;
        }
    }
}
