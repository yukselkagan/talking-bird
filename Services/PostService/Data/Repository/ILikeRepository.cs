using PostService.Model;
using System.Linq.Expressions;

namespace PostService.Data.Repository
{
    public interface ILikeRepository
    {
        public Task<IEnumerable<Like>> GetAll();
        public Task<IEnumerable<Like>> Get(
            Expression<Func<Like, bool>> filter = null,
            Func<IQueryable<Like>, IOrderedQueryable<Like>> orderBy = null,
            string includeProperties = "");

        public Task<Like> GetById(object id);
        public Task Insert(Like like);
        public Task Delete(object id);
        public Task Delete(Like entityToDelete);
        public Task Update(Like entityToUpdate);
        public Task Save();



    }
}
