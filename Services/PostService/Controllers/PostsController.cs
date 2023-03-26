using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Clients;
using PostService.Data.Repository;
using PostService.Dtos;
using PostService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using AutoMapper;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using PostService.Extensions;
using TalkingBirdContracts;

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
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;

        public PostsController(IPostRepository postRepository, ILikeRepository likeRepository,
            IdentityServiceClient identityServiceClient, IPublishEndpoint publishEndpoint, IMapper mapper,
            IMemoryCache memoryCache, IDistributedCache cache)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _identityServiceClient = identityServiceClient;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _cache = cache;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Get()
        {          
            try
            {
                var userId = await ReadUserIdFromService();

                string recordKey = "PostAll_" + $"{userId}_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
                IEnumerable<PostDto> postDtos;
                IEnumerable<Post> postsFromCache = await _cache.GetRecordAsync<IEnumerable<Post>>(recordKey);

                if (postsFromCache == null)
                {
                    var posts = await _postRepository.Get(orderBy: q => q.OrderByDescending(p => p.CreatedAt),
                    includeProperties: "User");

                    await _cache.SetRecordAsync<IEnumerable<Post>>(recordKey, posts);

                    postDtos = posts.Select(x =>
                    {
                        var dto = _mapper.Map<PostDto>(x);
                        var haveLike = ((_likeRepository.Get(filter: x => x.UserId == userId && x.PostId == dto.PostId)).Result).Count() > 0;
                        dto.UserLiked = haveLike;
                        return dto;
                    });

                }
                else
                {
                    postDtos = postsFromCache.Select(x =>
                    {
                        var dto = _mapper.Map<PostDto>(x);
                        var haveLike = ((_likeRepository.Get(filter: x => x.UserId == userId && x.PostId == dto.PostId)).Result).Count() > 0;
                        dto.UserLiked = haveLike;
                        return dto;
                    });
                }

                return Ok(postDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }         
        }

        [HttpGet("self")]
        public async Task<ActionResult> GetPostSelf()
        {
            try
            {
                var userId = await ReadUserIdFromService();

                string recordKey = "PostSelf_" + $"{userId}_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
                IEnumerable<Post> posts;
                IEnumerable<Post> postsFromMemoryCache = _memoryCache.Get<IEnumerable<Post>>(recordKey);

                if(postsFromMemoryCache == null)
                {
                    var postsFromDb = await _postRepository.Get(orderBy: q => q.OrderByDescending(p => p.CreatedAt),
                    includeProperties: "User", filter: x => x.UserId == userId);
                    _memoryCache.Set(recordKey, postsFromDb, TimeSpan.FromSeconds(2));

                    posts = postsFromDb;
                }
                else
                {
                    posts = postsFromMemoryCache;
                }

                var postDtos = posts.Select(x =>
                {
                    var dto = _mapper.Map<PostDto>(x);
                    var haveLike = ((_likeRepository.Get(filter: x => x.UserId == userId && x.PostId == dto.PostId)).Result).Count() > 0;
                    dto.UserLiked = haveLike;
                    return dto;
                });

                return Ok(postDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("filtered")]
        public async Task<ActionResult> GetPostsWithFilter(string filterName)
        {
            try
            {
                var userId = await ReadUserIdFromService();                

                var posts = await _postRepository.Get(orderBy: q => q.OrderByDescending(p => p.CreatedAt),
                    includeProperties: "User");
                var postDtos = posts.Select(x =>
                {
                    var dto = _mapper.Map<PostDto>(x);
                    var haveLike = ((_likeRepository.Get(filter: x => x.UserId == userId && x.PostId == dto.PostId)).Result).Count() > 0;
                    dto.UserLiked = haveLike;
                    return dto;
                });

                if(filterName == "Liked")
                {
                    postDtos = postDtos.Where(x => x.UserLiked == true);
                }

                return Ok(postDtos);

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
                    UserId = userId,
                    LikeCount = newPost.LikeCount,
                };
                await _publishEndpoint.Publish(postContract);

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

            return Ok(newLike);
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

            return Ok(targetLike);
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
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var userFromService = await _identityServiceClient.GetUserSelf(accessToken);
                return userFromService.UserId;
            }
            catch (Exception)
            {
                return 0;
            }          
        }



    }
}
