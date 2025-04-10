using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.CreateCashRegisterDetail;

class CreateCashRegisterDetailCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<CreateCashRegisterDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateCashRegisterDetailCommand request, CancellationToken cancellationToken)
    {
        CashRegister cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.CashRegisterId, cancellationToken);

        cashRegister.DepositAmount += (request.Type == 0 ? request.Amount : 0);
        cashRegister.WithdrawalAmount += (request.Type == 1 ? request.Amount : 0);

        CashRegisterDetail cashRegisterDetail = new()
        {
            Date = request.Date,
            DepositAmount = request.Type == 0 ? request.Amount : 0,
            WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
            Description = request.Description,
            CashRegisterId = request.CashRegisterId,
        };

        await cashRegisterDetailRepository.AddAsync(cashRegisterDetail, cancellationToken);

        if (request.OppositeCashRegisterId is not null)
        {
            CashRegister oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeCashRegisterId, cancellationToken);

            oppositeCashRegister.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeCashRegister.WithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            CashRegisterDetail oppositeCashRegisterDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                CashRegisterDetailId = cashRegisterDetail.Id,
                Description = request.Description,
                CashRegisterId = (Guid)request.OppositeCashRegisterId,
            };

            cashRegisterDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

            await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);
        }

        if (request.OppositeBankId is not null)
        {
            Bank oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeBankId, cancellationToken);

            oppositeBank.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeBank.WithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            BankDetail oppositeBankDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                CashRegisterDetailId = cashRegisterDetail.Id,
                Description = request.Description,
                BankId = (Guid)request.OppositeBankId,
            };

            cashRegisterDetail.BankDetailId = oppositeBankDetail.Id;

            await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
        }

        if (request.OppositeCustomerId is not null)
        {
            Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeCustomerId, cancellationToken);
            if (customer is null) return DomainResult.Failed<string>("Cari bulunamadı");

            customer.DepositAmount += (request.Type == 1 ? request.Amount : 0);
            customer.WithdrawalAmount += (request.Type == 0 ? request.Amount : 0);

            CustomerDetail customerDetail = new()
            {
                CustomerId = customer.Id,
                CashRegisterDetailId = cashRegisterDetail.Id,
                Date = request.Date,
                Description = request.Description,
                DepositAmount = request.Type == 1 ? request.Amount : 0,
                WithdrawalAmount = request.Type == 0 ? request.Amount : 0,
                Type = Domain.Enums.CustomerDetailTypeEnum.CashRegister
            };

            cashRegisterDetail.CustomerDetailId = customerDetail.Id;

            await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
        }

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveRangeCompanyFromContext(new[] { "banks", "cashRegisters" });

        return DomainResult.Success("Cash register detail created successfully.");
    }
}