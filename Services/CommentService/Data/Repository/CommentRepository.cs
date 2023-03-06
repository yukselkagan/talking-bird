using CommentService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CommentService.Data.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _context;
        private DbSet<Comment> _dbSet;
        public CommentRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Comment>();
        }


        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Comment>> Get(
            Expression<Func<Comment, bool>> filter = null,
            Func<IQueryable<Comment>, IOrderedQueryable<Comment>> orderBy = null,
            string includeProperties = "" )
        {
            IQueryable<Comment> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries ))
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

        public async Task<Comment> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task Insert(Comment comment)
        {
            await _dbSet.AddAsync(comment);           
        }

        public async Task Delete(object id)
        {
            Comment entityToDelete = await _dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }

        public async Task Delete(Comment entityToDelete)
        {
            if(_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public async Task Update(Comment entityToUpdate)
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
