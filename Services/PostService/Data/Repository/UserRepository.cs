using Microsoft.EntityFrameworkCore;
using PostService.Model;
using System.Linq.Expressions;

namespace PostService.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private DbSet<User> _dbSet;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }


        public async Task<IEnumerable<User>> Get(
            Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null,
            string includeProperties = "" )
        {
            IQueryable<User> query = _dbSet;
            
            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries) )
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<User> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Insert(User user)
        {
            await _dbSet.AddAsync(user);
        }

        public async Task Delete(object id)
        {
            var entityToDelete = _dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }

        public async Task Delete(User entityToDelete)
        {
            if(_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public async Task Update(User entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


    }
}
