using System.Linq.Expressions;
using TrendService.Models;

namespace TrendService.Data.Repository
{
    public interface ITrendRepository
    {
        public Task<IEnumerable<Trend>> Get(
            Expression<Func<Trend, bool>> filter = null,
            Func<IQueryable<Trend>, IOrderedQueryable<Trend>> orderBy = null,
            string includeProperties = ""
            );
        public Task Insert(Trend trend);
        public Task Update(Trend entityToUpdate);
        public Task Save();
    }
}
