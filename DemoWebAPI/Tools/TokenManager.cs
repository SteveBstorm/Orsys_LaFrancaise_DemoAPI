using DemoWebAPI.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoWebAPI.Tools
{
    public class TokenManager
    {
        private string _key;

        public TokenManager(IConfiguration config)
        {
            _key = config.GetSection("TokenInfo").GetSection("SecretKey").Value;
        }


        public string GenerateToken(User user)
        {
            //Generate signin key
            SymmetricSecurityKey securityKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            SigningCredentials credentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            //Payload 
            Claim[] myClaims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user")
            };

            //Construction du token
            JwtSecurityToken securityToken = new JwtSecurityToken(
                claims: myClaims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1),
                audience: "https://monAppclient.com",
                issuer: "https://monserver.com"
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(securityToken);
        }
    }
}
