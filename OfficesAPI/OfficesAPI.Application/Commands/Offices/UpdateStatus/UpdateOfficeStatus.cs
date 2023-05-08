using MediatR;
using OfficesAPI.Application.Results;

namespace OfficesAPI.Application.Commands.Offices.UpdateStatus
{
    public record UpdateOfficeStatus(Guid Id, bool NewStatus) : IRequest<ApplicationVoidResult>;
}
