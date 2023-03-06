using CommentService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CommentService.Data.Repository
{
    public interface ICommentRepository
    {
        public Task<IEnumerable<Comment>> GetAll();
        public Task<IEnumerable<Comment>> Get(
            Expression<Func<Comment, bool>> filter = null,
            Func<IQueryable<Comment>, IOrderedQueryable<Comment>> orderBy = null,
            string includeProperties = "");

        public Task<Comment> GetById(object id);
        public Task Insert(Comment comment);
        public Task Delete(object id);
        public Task Delete(Comment entityToDelete);
        public Task Update(Comment entityToUpdate);
        public Task Save();
    }
}
