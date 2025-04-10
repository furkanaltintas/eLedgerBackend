﻿using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Invoices.GetAllInvoices;

public sealed record GetAllInvoicesQuery(int Type) : IRequest<IDomainResult<List<Invoice>>>;