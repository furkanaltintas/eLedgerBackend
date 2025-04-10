﻿using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.GetAllCashRegisters;

public record GetAllCashRegistersQuery() : IRequest<IDomainResult<List<CashRegister>>>;