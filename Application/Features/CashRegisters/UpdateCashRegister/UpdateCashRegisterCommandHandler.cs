﻿using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.CashRegisters.UpdateCashRegister;

class UpdateCashRegisterCommandHandler(
    ICashRegisterRepository cashRegisterRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<UpdateCashRegisterCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (cashRegister is null) return DomainResult.Failed<string>("Cash register not found.");

        if (cashRegister.Name != request.Name)
        {
            Boolean isNameExists = await cashRegisterRepository.AnyAsync(c => c.Name == request.Name, cancellationToken);
            if (isNameExists) return DomainResult.Failed<string>("Bu isimde bir kasa zaten mevcut.");
        }

        mapper.Map(request, cashRegister);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("cashRegisters");

        return DomainResult.Success("Cash register updated successfully.");
    }
}