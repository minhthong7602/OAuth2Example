using IdentityModel.Client;

namespace API.Handlers
{
    public class ProtectedApiBearerTokenMiddleWare : IMiddleware
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProtectedApiBearerTokenMiddleWare> _logger;

        public ProtectedApiBearerTokenMiddleWare(ILogger<ProtectedApiBearerTokenMiddleWare> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            if(context.Request.Path.StartsWithSegments("/api"))
            {
              await SetAccessToken(context);
            }
            await next(context);
        }

        private async Task SetAccessToken(HttpContext context)
        {
            // request the access token
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5018/connect/token",
                ClientId = context.Request.Headers["ClientId"],
                ClientSecret = context.Request.Headers["ClientSecret"]
            });

            if (tokenResponse.IsError)
            {
                _logger.Log(LogLevel.Error, $"ErrorType: {tokenResponse.ErrorType} Error: {tokenResponse.Error}");
                throw new HttpRequestException("Something went wrong while requesting Token to the AuthServer.");
            }
            context.Request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
        }
    }
}
