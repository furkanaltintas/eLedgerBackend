using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class InvoiceRepository : Repository<Invoice, AppDbContext>, IInvoiceRepository
{
    public InvoiceRepository(AppDbContext context) : base(context) { }
}