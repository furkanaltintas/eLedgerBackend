﻿using Application.Features.Reports.Queries;
using Application.Features.Reports.Responses;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reports.Handlers;

internal sealed class ProductProfitabilityReportsQueryHandler(IProductRepository productRepository) : IRequestHandler<ProductProfitabilityReportsQuery, IDomainResult<List<ProductProfitabilityReportsQueryResponse>>>
{
    public async Task<IDomainResult<List<ProductProfitabilityReportsQueryResponse>>> Handle(ProductProfitabilityReportsQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await productRepository
            .Where(p => p.Withdrawal > 0)
            .Include(p => p.Details)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        List<ProductProfitabilityReportsQueryResponse> response = new();

        foreach (var product in products)
        {
            Decimal depositPriceSum = product.Details!.Where(p => p.Deposit > 0).Sum(p => p.Price);
            Int64 depositCount = product.Details!.Where(p => p.Deposit > 0).Count();

            Decimal withdrawalPriceSum = product.Details!.Where(p => p.Withdrawal > 0).Sum(p => p.Price);
            Int64 withdrawalCount = product.Details!.Where(p => p.Withdrawal > 0).Count();

            Decimal depositPrice = depositPriceSum > 0 || depositCount > 0 ? depositPriceSum / depositCount : 0;
            Decimal withdrawalPrice = withdrawalPriceSum > 0 || withdrawalCount > 0 ? withdrawalPriceSum / withdrawalCount : 0;

            ProductProfitabilityReportsQueryResponse data = new()
            {
                Id = product.Id,
                Name = product.Name,
                DepositPrice = depositPrice,
                WithdrawalPrice = withdrawalPrice,
                Profit = withdrawalPrice - depositPrice,
                ProfitPercent = depositPrice > 0 ? (withdrawalPrice - depositPrice) / depositPrice * 100 : 0 
                // Kar yüzdesini hesaplıyoruz. (Kar / Alış fiyatı) * 100
            };

            response.Add(data);
        }

        return DomainResult.Success(response);
    }
}