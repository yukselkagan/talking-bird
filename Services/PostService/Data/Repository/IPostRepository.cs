using PostService.Model;
using System.Linq.Expressions;

namespace PostService.Data.Repository
{
    public interface IPostRepository
    {
        public Task<IEnumerable<Post>> GetAll();
        public Task<IEnumerable<Post>> Get(
            Expression<Func<Post, bool>> filter = null,
            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null,
            string includeProperties = "");
        public Task<Post> GetById(object id);
        public Task Insert(Post post);
        public Task Delete(object id);
        public Task Delete(Post entityToDelete);
        public Task Update(Post entityToUpdate);
        public Task Save();
    }
}
