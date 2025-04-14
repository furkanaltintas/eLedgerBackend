using Application.Common.Interfaces;
using Application.Features.BankDetails.Commands;
using Application.Features.BankDetails.Constants;
using Application.Features.Banks.Constants;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.Handlers;

class CreateBankDetailCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<CreateBankDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
    {
        Bank bank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.BankId, cancellationToken);
        bank.DepositAmount += request.Type == 0 ? request.Amount : 0;
        bank.WithdrawalAmount += request.Type == 1 ? request.Amount : 0;

        BankDetail bankDetail = new()
        {
            Date = request.Date,
            DepositAmount = request.Type == 0 ? request.Amount : 0,
            WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
            Description = request.Description,
            BankId = request.BankId
        };

        await bankDetailRepository.AddAsync(bankDetail, cancellationToken);

        if (request.OppositeBankId is not null)
        {
            Bank oppositeCashRegister = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeBankId, cancellationToken);

            oppositeCashRegister.DepositAmount += request.Type == 1 ? request.OppositeAmount : 0;
            oppositeCashRegister.WithdrawalAmount += request.Type == 0 ? request.OppositeAmount : 0;

            BankDetail oppositeBankDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                BankDetailId = bankDetail.Id,
                Description = request.Description,
                BankId = (Guid)request.OppositeBankId,
            };

            bankDetail.BankDetailId = oppositeBankDetail.Id;

            await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
        }

        if (request.OppositeCashRegisterId is not null)
        {
            CashRegister oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeCashRegisterId, cancellationToken);

            oppositeCashRegister.DepositAmount += request.Type == 1 ? request.OppositeAmount : 0;
            oppositeCashRegister.WithdrawalAmount += request.Type == 0 ? request.OppositeAmount : 0;

            CashRegisterDetail oppositeCashRegisterDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                BankDetailId = bankDetail.Id,
                Description = request.Description,
                CashRegisterId = (Guid)request.OppositeCashRegisterId,
            };

            bankDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

            await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);
        }

        if (request.OppositeCustomerId is not null)
        {
            Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeCustomerId, cancellationToken);
            if (customer is null) return DomainResult.Failed<string>("Cari bulunamadı");

            customer.DepositAmount += request.Type == 1 ? request.Amount : 0;
            customer.WithdrawalAmount += request.Type == 0 ? request.Amount : 0;

            CustomerDetail customerDetail = new()
            {
                CustomerId = customer.Id,
                BankDetailId = bankDetail.Id,
                Date = request.Date,
                Description = request.Description,
                DepositAmount = request.Type == 1 ? request.Amount : 0,
                WithdrawalAmount = request.Type == 0 ? request.Amount : 0,
                Type = Domain.Enums.CustomerDetailTypeEnum.Bank
            };

            bankDetail.CustomerDetailId = customerDetail.Id;

            await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
        }

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveRangeCompanyFromContext(new[] { BanksMessages.Cache, CashRegisters.Cache, CustomersMessages.Cache });

        return DomainResult.Success(BankDetailsMessages.Created);
    }
}
