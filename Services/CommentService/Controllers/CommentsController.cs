using CommentService.Data.Repository;
using CommentService.Dtos;
using CommentService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public CommentsController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            Console.WriteLine("test from controller");

            return Ok("tested");
        }


        [HttpPost]
        public async Task<ActionResult> CreateComment(CommentCreateDto commentCreateDto)
        {
            var targetPost = (await _postRepository.Get(x => x.ExternalPostId == commentCreateDto.PostId)).FirstOrDefault();
            if(targetPost == null)
            {
                return NotFound("Can't find target post");
            }

            var newComment = new Comment()
            {
                Content = commentCreateDto.Content,
                PostId = commentCreateDto.PostId,
                UserId = 9
            };

            await _commentRepository.Insert(newComment);
            await _commentRepository.Save();

            return Ok("commented");
        }


    }
}
