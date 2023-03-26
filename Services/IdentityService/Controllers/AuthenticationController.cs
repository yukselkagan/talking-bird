using AutoMapper;
using IdentityService.Data.Repository.Interfaces;
using IdentityService.Dtos;
using IdentityService.Extensions;
using IdentityService.Models;
using IdentityService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkingBirdContracts;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPublishEndpoint _publishEndpoint;
        public AuthenticationController(IMapper mapper, IUserRepository userRepository, ITokenService tokenService, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("users")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userRepository.Get();
            return Ok(users);
        }

        [HttpGet("user")]
        [AllowAnonymous]
        public async Task<ActionResult> Get()
        {
            var aragorn = (await _userRepository.Get(x => x.Email == "aragorn")).FirstOrDefault();
            var mapped = _mapper.Map<UserDto>(aragorn);

            return Ok(mapped);
        }

        [HttpGet("user/self")]
        public async Task<ActionResult<UserDto>> GetUserSelf()
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    throw new Exception("Can not find user");
                }
                var userDto = _mapper.Map<UserDto>(user);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }


        

        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserRegisterDto userCreateDto)
        {
            try
            {
                var sameEmailUser = (await _userRepository.Get(x => x.Email == userCreateDto.Email)).FirstOrDefault();
                var sameUserNameUser = (await _userRepository.Get(x => x.UserName == userCreateDto.UserName)).FirstOrDefault();
                if (sameEmailUser != null) { return BadRequest("Email already taken"); };
                if (sameUserNameUser != null) { return BadRequest("User name already taken"); };

                var mappedUser = _mapper.Map<User>(userCreateDto);

                byte[] salt;
                var passwordHash = PasswordService.CreatePasswordHash(userCreateDto.Password, out salt);

                mappedUser.PasswordHash = passwordHash;
                mappedUser.PasswordSalt = salt;

                await _userRepository.Insert(mappedUser);
                await _userRepository.Save();

                var tokenString = _tokenService.CreateAccessToken(mappedUser);
                var userToken = new UserToken()
                {
                    User = mappedUser,
                    AccessToken = tokenString
                };

                var userCreatedContract = new UserCreated()
                {
                    UserId = mappedUser.UserId,
                    Email = mappedUser.Email,
                    UserName = mappedUser.UserName,
                    DisplayName = mappedUser.DisplayName,
                    ProfileImage = mappedUser.ProfileImage
                };
                await _publishEndpoint.Publish(userCreatedContract);

                return Ok(userToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }         
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = (await _userRepository.Get(x => x.Email == userLoginDto.Email)).FirstOrDefault();
            if(user == null)
            {
                return Unauthorized("Email or password is wrong");
            }

            var passwordVerified = PasswordService.VerifyPassword(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (passwordVerified == false)
            {
                return Unauthorized("Email or password is wrong");
            }

            string tokenString = _tokenService.CreateAccessToken(user);
            var userToken = new UserToken()
            {
                User = user,
                AccessToken = tokenString
            };

            return Ok(userToken);
        }

        [HttpPatch("user/display-name")]
        public async Task<ActionResult> ChangeDisplayName(UserChangeNameDto userChangeNameDto)
        {
            try
            {
                int userId = HttpContext.User.ReadUserId();
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    throw new Exception("Can not find user");
                }

                user.DisplayName = userChangeNameDto.DisplayName;
                await _userRepository.Update(user);
                await _userRepository.Save();

                var userUpdatedContract = new UserUpdated()
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    ProfileImage = user.ProfileImage
                };
                await _publishEndpoint.Publish(userUpdatedContract);

                return Ok("Display name changed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<ActionResult> TestUsers()
        {
            var userCreated = new UserCreated()
            {
                UserId = 5,
                Email = "testmail",
                UserName = "testname",
                DisplayName = "test display",
                ProfileImage = "tes image"
            };
            await _publishEndpoint.Publish(userCreated);

            return Ok("test is okay");
        }



    }
}
