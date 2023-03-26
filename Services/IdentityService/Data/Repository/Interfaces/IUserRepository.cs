using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityService.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<IEnumerable<User>> Get(
            Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            string includeProperties = "");
        public Task<User> GetById(object id);
        public Task Insert(User user);
        public Task Update(User entityToUpdate);
        public Task Save();

    }
}
