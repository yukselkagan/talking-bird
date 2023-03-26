using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using TrendService.Models;

namespace TrendService.Data.Repository
{
    public class TrendRepository : ITrendRepository
    {
        private readonly ApplicationContext _context;
        private DbSet<Trend> _dbSet;
        public TrendRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<Trend>();
        }

        public async Task<IEnumerable<Trend>> Get(
            Expression<Func<Trend, bool>> filter = null,
            Func<IQueryable<Trend>, IOrderedQueryable<Trend>> orderBy = null,
            string includeProperties = ""           
            )
        {
            IQueryable<Trend> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach(var includeProperty in includeProperties.Split(new char[] {','},  StringSplitOptions.RemoveEmptyEntries))
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


        public async Task Insert(Trend trend)
        {
            await _dbSet.AddAsync(trend);
        }

        public async Task Update(Trend entityToUpdate)
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
