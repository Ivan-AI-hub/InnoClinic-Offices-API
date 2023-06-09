﻿using FluentValidation;
using OfficesAPI.Application.Abstraction.Models;

namespace OfficesAPI.Application.Validators
{
    public class UpdateOfficeValidator : AbstractValidator<UpdateOfficeModel>
    {
        private const string _phoneRegex = "^(\\+)?((\\d{2,3}) ?\\d|\\d)(([ -]?\\d)|( ?(\\d{2,3}) ?)){5,12}\\d$";
        public UpdateOfficeValidator()
        {
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.HouseNumber).GreaterThan(0);
            RuleFor(x => x.OfficeNumber).GreaterThan(0);

            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().Matches(_phoneRegex);
        }
    }
}
