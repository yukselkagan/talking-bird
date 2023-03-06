using PostService.Model;
using System.Linq.Expressions;

namespace PostService.Data.Repository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> Get(
            Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            string includeProperties = "");

        public Task<User> GetById(object id);
        public Task Insert(User user);
        public Task Delete(object id);
        public Task Delete(User entityToDelete);
        public Task Update(User entityToUpdate);
        public Task Save();

    }
}
