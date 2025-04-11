using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class InvoiceDetailRepository : Repository<InvoiceDetail, CompanyDbContext>, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(CompanyDbContext context) : base(context) { }
}