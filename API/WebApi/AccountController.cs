using API.Security.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace API.WebApi
{
    [Route("api/internal/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginParameter model)
        {
            if (!model.Username.Equals("MinhThong"))
            {
                throw new Exception("Not Found");
            }

            if (!model.Password.Equals("123456"))
            {
                throw new Exception("Not Found");
            }
            else
            {
                var token = new JwtTokenBuilder()
                            .AddSecurityKey(JwtSecurityKey.Create("B8akSP.Security.Bearer.QAYOR838GS"))
                            .AddIssuer("B8akSP.Secret.QAYOR838GS")
                            .AddAudience("B8akSP.Security.Bearer.QAYOR838GS")
                            .AddSubject(model.Username)
                            .AddClaim("userId", "122")
                            .AddClaim("roleId", "Admin")
                            .AddClaim("userName", model.Username)
                            .AddClaim("role", "Admin")
                            .AddExpiryInMinutes(1440)
                            .Build();
                return Ok(token);
            }
        }
    }

    public class LoginParameter
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
