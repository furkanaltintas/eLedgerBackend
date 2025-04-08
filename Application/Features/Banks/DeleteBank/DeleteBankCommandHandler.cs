using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Banks.DeleteBank;

class DeleteBankCommandHandler(
        IBankRepository bankRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        ICompanyContextHelper companyContextHelper) : IRequestHandler<DeleteBankCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
    {
        Bank bank = await bankRepository.GetByExpressionWithTrackingAsync(b => b.Id == request.Id, cancellationToken);
        if (bank is null) return DomainResult.NotFound<string>($"Bank not found.");

        bank.IsDeleted = true;
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        companyContextHelper.RemoveCompanyFromContext("banks");

        return DomainResult.Success("Bank deleted successfully");
    }
}