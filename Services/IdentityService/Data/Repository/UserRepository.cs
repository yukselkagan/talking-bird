using IdentityService.Data.Repository.Interfaces;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace IdentityService.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public DbSet<User> dbSet;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
            dbSet = _context.Set<User>();

        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IEnumerable<User>> Get(
            Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            string includeProperties = "")
        {

            IQueryable<User> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach(var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries ) )
            {
                query = query.Include(includeProperty);
            }

            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public async Task<User> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Insert(User user)
        {
            dbSet.Add(user);
        }


        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }



    }
}
