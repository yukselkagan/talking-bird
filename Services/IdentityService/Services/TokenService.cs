using IdentityService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string CreateAccessToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("id", user.UserId.ToString()),
                new Claim("email", user.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TALKINGBIRDSECRETTOKEN123456789"));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (

                    claims: claimsIdentity.Claims,
                    signingCredentials: signingCredentials,

                    expires: DateTime.Now.AddMinutes(500),
                    notBefore: DateTime.Now

                );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string tokenString = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return tokenString;
        }

    }
}
