using Microsoft.EntityFrameworkCore;
using PostService.Model;
using System.Linq.Expressions;

namespace PostService.Data.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationContext _context;
        private DbSet<Like> _dbSet;
        public LikeRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Like>();
        }

        
        public async Task<IEnumerable<Like>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Like>> Get(
            Expression<Func<Like, bool>> filter = null,
            Func<IQueryable<Like>, IOrderedQueryable<Like>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Like> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach(var includeProperty in includeProperties.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries ))
            {
                query = query.Include(includeProperty);
            }

            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<Like> GetById(object id)
        {
            var like = await _dbSet.FindAsync(id);
            return like;
        }

        public async Task Insert(Like like)
        {
            await _dbSet.AddAsync(like);
        }

        public async Task Delete(object id)
        {
            Like entityToDelete = await _dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }

        public async Task Delete(Like entityToDelete)
        {
            if(_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public async Task Update(Like entityToUpdate)
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
