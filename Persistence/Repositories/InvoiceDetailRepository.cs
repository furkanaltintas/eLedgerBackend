using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class InvoiceDetailRepository : Repository<InvoiceDetail, AppDbContext>, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(AppDbContext context) : base(context) { }
}