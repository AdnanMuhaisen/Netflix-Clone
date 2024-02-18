using Netflix_Clone.API.Extensions.Domain;
using Netflix_Clone.Application.Services.FileOperations;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Application.Services;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork;

namespace Netflix_Clone.API.Extensions.Application
{
    public static class ApplicationServices
    {
        public static void RegisterApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IFileCompressor, FileCompressor>();
            builder.Services.AddScoped<IFileManager, FileManager>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        }
    }
}
