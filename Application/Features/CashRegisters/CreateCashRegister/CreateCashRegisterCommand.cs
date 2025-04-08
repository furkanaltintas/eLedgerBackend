﻿using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.CreateCashRegister;

public record CreateCashRegisterCommand(
    string Name,
    int TypeValue) : IRequest<IDomainResult<string>>;