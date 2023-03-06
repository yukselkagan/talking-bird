using CommentService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CommentService.Data.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationContext _context;
        private DbSet<Post> _dbSet;
        public PostRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Post>();
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Post>> Get(
            Expression<Func<Post, bool>> filter = null,
            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null,
            string includeProperties = ""
            )
        {
            IQueryable<Post> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);          
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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

        public async Task<Post> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Insert(Post post)
        {
            await _dbSet.AddAsync(post);
        }

        public async Task Delete(object id)
        {
            Post entityToDelete = await _dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }

        public async Task Delete(Post entityToDelete)
        {
            if(_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public async Task Update(Post entityToUpdate)
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
