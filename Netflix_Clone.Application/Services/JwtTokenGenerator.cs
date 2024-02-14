using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Netflix_Clone.Application.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IOptions<JwtOptions> options;

        public JwtTokenGenerator(IOptions<JwtOptions> options)
        {
            this.options = options;
        }

        public string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> UserRoles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(options.Value.Key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email!),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName!)
            };

            claims.AddRange(UserRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = options.Value.Audience,
                Issuer = options.Value.Issuer,
                Expires = DateTime.UtcNow.AddDays(options.Value.ExpiresAfterDays),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
