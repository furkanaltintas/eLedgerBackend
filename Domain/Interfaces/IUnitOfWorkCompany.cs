namespace Domain.Interfaces;

public interface IUnitOfWorkCompany
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}