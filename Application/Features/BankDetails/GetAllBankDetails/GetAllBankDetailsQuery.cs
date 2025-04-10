﻿using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.GetAllBankDetails;

public record GetAllBankDetailsQuery(
    Guid BankId,
    DateOnly StartDate,
    DateOnly EndDate) : IRequest<IDomainResult<Bank>>;