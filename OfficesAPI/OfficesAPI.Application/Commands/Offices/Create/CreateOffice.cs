﻿using MediatR;
using OfficesAPI.Application.Results;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Commands.Offices.Create
{
    public record CreateOffice(string? PhotoFileName,
                              string City,
                              string Street,
                              int HouseNumber,
                              int OfficeNumber,
                              string PhoneNumber,
                              bool Status) : IRequest<ApplicationValueResult<Office>>;
}
