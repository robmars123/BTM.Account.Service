using BTM.Account.Domain.Abstractions;

namespace BTM.Account.Infrastructure.Repositories.Base
{
    internal class Repository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _dbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
