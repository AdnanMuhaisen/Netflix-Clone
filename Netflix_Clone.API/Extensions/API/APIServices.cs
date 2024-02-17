using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netflix_Clone.Domain.Options;
using Serilog;
using System.Text;

namespace Netflix_Clone.API.Extensions.API
{
    public static class APIServices
    {
        public static void RegisterAPIServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            //Swagger
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter The Bearer Authorization string as follows : ` Bearer Generated - JWT - Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },new string[] {}
                        }
                    });
            });

            //JWT Authentication

            //will throw an exception if the options service does not registered
            var jwtOptions = builder.Services.BuildServiceProvider()
                .GetRequiredService<IOptions<JwtOptions>>();

            builder.Services.AddAuthentication(options =>
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
                        ValidAudience = jwtOptions.Value.Audience,
                    };
                });
            
            builder.Services.AddAuthorization();

            //configure the logger
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });




        }
    }
}
