using Microsoft.AspNetCore.Http;
using PostService.Dtos;

namespace PostService.Clients
{
    public class IdentityServiceClient
    {
        private readonly HttpClient _httpClient;
        public IdentityServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<UserDto> GetUser()
        {
            var user = await _httpClient.GetFromJsonAsync<UserDto>("api/Authentication/user");
            return user;
        }

        public async Task<UserDto> GetUserSelf(string tokenString)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

            var user = await _httpClient.GetFromJsonAsync<UserDto>("api/authentication/user/self");
            return user;
        }


    }
}
