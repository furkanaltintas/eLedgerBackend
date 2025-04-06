using Domain.Entities;
using Domain.Interfaces;
using GenericRepository;
using Persistence.Context;

namespace Persistence.Repositories;

class UserRepository : Repository<AppUser, AppDbContext>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
}