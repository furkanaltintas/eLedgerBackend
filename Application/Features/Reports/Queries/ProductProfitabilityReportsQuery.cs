using Application.Features.Reports.Responses;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Reports.Queries;

public sealed record ProductProfitabilityReportsQuery() : IRequest<IDomainResult<List<ProductProfitabilityReportsQueryResponse>>>;