using API.IOptionsModels;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class TokenService: ITokenService
    {
        private readonly DiscoveryDocumentResponse _discDocument;
        private readonly IOptions<Discovery> _discConfig;
        public TokenService(IOptions<Discovery> discConfig)
        {
            _discConfig = discConfig;
            using (var client = new HttpClient())
            {
                _discDocument = client.GetDiscoveryDocumentAsync(_discConfig.Value.DiscoveryDocumentUrl).Result;
            }
        }
        public async Task<TokenResponse> GetToken(string scope, string email, string password)
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    UserName = email,
                    Password = password,
                    Scope = scope,
                    ClientId = "swagger_client",
                    ClientSecret = "secret",
                    Address = _discDocument.TokenEndpoint
                });
                if (tokenResponse.IsError)
                {
                    throw new Exception("Token Error");
                }
                return tokenResponse;
            }
        }
    }
}
