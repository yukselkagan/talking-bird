using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Clients;
using TalkingBird.Contracts;
using PostService.Data.Repository;
using PostService.Dtos;
using PostService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IdentityServiceClient _identityServiceClient;
        private readonly IPublishEndpoint _publishEndpoint;
        
        public PostsController(IPostRepository postRepository, ILikeRepository likeRepository,
            IdentityServiceClient identityServiceClient, IPublishEndpoint publishEndpoint)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _identityServiceClient = identityServiceClient;
            _publishEndpoint = publishEndpoint;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get()
        {          
            try
            {
                var posts = await _postRepository.Get(orderBy: q => q.OrderByDescending(p => p.CreatedAt));
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }         
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var post = await _postRepository.GetById(id);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> Create(PostCreateDto postCreateDto)
        {
            try
            {
                var userId = await ReadUserIdFromService();

                var newPost = new Post()
                {
                    Content = postCreateDto.Content,
                    UserId = userId
                };

                await _postRepository.Insert(newPost);
                await _postRepository.Save();

                var postContract = new PostCreated()
                {
                    PostId = newPost.PostId,
                    Content = newPost.Content,
                    UserId = 7,
                    LikeCount = newPost.LikeCount,
                };
                _publishEndpoint.Publish(postContract);

                return Ok(newPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }       
        }

        
        [HttpPost("repost/{targetPostId}")]
        public async Task<ActionResult> Repost(int targetPostId, PostCreateDto postCreateDto)
        {
            var targetPost = await _postRepository.GetById(targetPostId);
            if (targetPost == null)
            {
                return NotFound("Can't find the post");
            }

            Post newPost = new Post()
            {
                Content = postCreateDto.Content,
                HaveRepost = true,
                RepostId = targetPostId
            };

            await _postRepository.Insert(newPost);
            await _postRepository.Save();

            return CreatedAtAction(nameof(GetById), new { id = newPost.PostId }, newPost);
        }


        [HttpPost("like/{postId}")]
        public async Task<ActionResult> LikePost(int postId)
        {
            var userId = await ReadUserIdFromService();

            var targetPost = await _postRepository.GetById(postId);
            if (targetPost == null)
            {
                return NotFound("Can't found the post");
            }

            var existingLike = (await _likeRepository.Get(x => x.PostId == postId && x.UserId == userId)).FirstOrDefault();
            if (existingLike != null)
            {
                return BadRequest("Already liked");
            }

            var newLike = new Like()
            {
                PostId = postId,
                UserId = userId,
            };

            await _likeRepository.Insert(newLike);
            await _likeRepository.Save();

            targetPost.LikeCount += 1;
            await _postRepository.Update(targetPost);
            await _postRepository.Save();

            return Ok("Liked to post");
        }

        [HttpDelete("like/{postId}")]
        public async Task<ActionResult> RevokeLike(int postId)
        {
            var userId = await ReadUserIdFromService();

            var targetLike = (await _likeRepository.Get(x => x.PostId == postId && x.UserId == userId)).FirstOrDefault();
            if (targetLike == null)
            {
                return NotFound("Can't find the like");
            }
            var targetPost = await _postRepository.GetById(postId);
            if (targetPost == null)
            {
                return NotFound("Can't find the post");
            }

            await _likeRepository.Delete(targetLike);
            await _likeRepository.Save();

            targetPost.LikeCount -= 1;
            await _postRepository.Update(targetPost);
            await _postRepository.Save();

            return Ok("Like revoked");
        }


        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            var postContract = new PostCreated()
            {
                PostId = 2,
                Content = "Formessagebus"
            };
            await _publishEndpoint.Publish(postContract);

            return Ok("Tested");
        }

        private async Task<int> ReadUserIdFromService()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var userFromService = await _identityServiceClient.GetUserSelf(accessToken);
            return userFromService.UserId;
        }



    }
}
