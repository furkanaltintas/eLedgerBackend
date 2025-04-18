using Domain.Entities.Companies;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class InvoiceRepository : Repository<Invoice, CompanyDbContext>, IInvoiceRepository
{
    public InvoiceRepository(CompanyDbContext context) : base(context) { }
}