using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netflix_Clone.Domain.Options;
using System.Text;

namespace Netflix_Clone.API.Extensions
{
    public static class JwtAuthenticationConfiguration
    {
        public static void ConfigureJwtBearerAuthentication(this IServiceCollection services)
        {
            //will throw an exception if the options service does not registered
            var jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Value.Key)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Value.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Value.Audience
                    };
                });
        }
    }
}
