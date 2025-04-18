using Application.Common.Interfaces;
using DomainResults.Common;
using GenericRepository;
using MapsterMapper;

namespace Application.Common.Handlers.Partners;

public abstract class ApplicationCommandHandlerBase
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ICacheService CacheService;
    protected readonly IMapper Mapper;

    protected ApplicationCommandHandlerBase(
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        CacheService = cacheService;
        Mapper = mapper;
    }

    protected async Task<IDomainResult<T>> SuccessAsync<T>(string cacheKey, T result, CancellationToken cancellationToken = default)
    {
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        CacheService.Remove(cacheKey);
        return DomainResult.Success(result);
    }
}