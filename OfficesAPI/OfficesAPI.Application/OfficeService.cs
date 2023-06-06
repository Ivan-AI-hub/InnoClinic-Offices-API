﻿using AutoMapper;
using FluentValidation;
using OfficesAPI.Application.Abstraction;
using OfficesAPI.Application.Abstraction.Models;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Exceptions;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application
{
    public class OfficeService : IOfficeService
    {
        private readonly IMapper _mapper;
        private readonly IOfficeRepository _officeRepository;
        private readonly IValidator<CreateOfficeModel> _createOfficeValidator;
        private readonly IValidator<UpdateOfficeModel> _updateOfficeValidator;

        public OfficeService(IMapper mapper, IOfficeRepository officeRepository,
            IValidator<CreateOfficeModel> createOfficeValidator, IValidator<UpdateOfficeModel> updateOfficeValidator)
        {
            _mapper = mapper;
            _officeRepository = officeRepository;
            _createOfficeValidator = createOfficeValidator;
            _updateOfficeValidator = updateOfficeValidator;
        }

        public async Task<OfficeDTO> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await ValidateModel(model, _createOfficeValidator, cancellationToken);
            await ValidateOfficeNumber(model.City, model.OfficeNumber, default, cancellationToken);

            var office = _mapper.Map<Office>(model);
            await _officeRepository.CreateAsync(office);

            return _mapper.Map<OfficeDTO>(office);
        }

        public async Task UpdateAsync(Guid id, UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await ValidateModel(model, _updateOfficeValidator, cancellationToken);
            await ValidateOfficeNumber(model.City, model.OfficeNumber, id, cancellationToken);


            var office = _mapper.Map<Office>(model);
            office.Id = id;
            await _officeRepository.UpdateAsync(id, office);
        }

        public async Task UpdateStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
            var office = await _officeRepository.GetItemAsync(id, cancellationToken);

            if (office == null)
            {
                throw new OfficeNotFoundException(id);
            }

            if (office.Status == newStatus)
            {
                throw new OfficeHaveThatStatusException(newStatus);
            }

            office.Status = newStatus;
            await _officeRepository.UpdateAsync(id, office);
        }

        public IEnumerable<OfficeDTO> GetOffices(int pageNumber, int pageSize)
        {
            var offices = _officeRepository.GetItems();
            offices = offices.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }

        public async Task<OfficeDTO> GetOfficeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var office = await _officeRepository.GetItemAsync(id, cancellationToken);
            if (office == null)
            {
                throw new OfficeNotFoundException(id);
            }

            return _mapper.Map<OfficeDTO>(office);
        }

        private async Task ValidateOfficeNumber(string city, int officeNumber, Guid excludeId = default, CancellationToken cancellationToken = default)
        {
            var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == city &&
                                                                                      x.OfficeNumber == officeNumber &&
                                                                                      x.Id != excludeId,
                                                                                      cancellationToken);

            if (isOfficeNumberInvalid)
            {
                throw new CityAlreadyHaveOfficeWithThatNumberException(city, officeNumber);
            }
        }

        private async Task ValidateModel<Tmodel>(Tmodel model, IValidator<Tmodel> validator, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(model, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
