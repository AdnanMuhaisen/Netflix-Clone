using Mapster;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.API.Extensions.Domain
{
    public static class DomainServices
    {
        public static void RegisterDomainServices(this WebApplicationBuilder builder)
        {
            //configure the options:
            builder.Services.Configure<ContentMovieOptions>(builder.Configuration.GetSection("Content:Movies"));
            builder.Services.Configure<ContentTVShowOptions>(builder.Configuration.GetSection("Content:TVShows"));
            builder.Services.Configure<UserRolesOptions>(builder.Configuration.GetSection("UserRoles"));
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

            TypeAdapterConfig<Movie, MovieDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.ReleaseYear, src => src.ReleaseYear)
                .Map(dest => dest.MinimumAgeToWatch, src => src.MinimumAgeToWatch)
                .Map(dest => dest.Synopsis, src => src.Synopsis)
                .Map(dest => dest.LengthInMinutes, src => src.LengthInMinutes)
                .Map(dest => dest.DirectorId, src => src.DirectorId)
                .Map(dest => dest.LanguageId, src => src.LanguageId)
                .Map(dest => dest.ContentGenreId, src => src.ContentGenreId)
                .IgnoreAttribute(new Type[]
                {
                    typeof(ContentLanguage),
                    typeof(ContentAward),
                    typeof(ContentGenre),
                    typeof(Director),
                    typeof(Actor),
                    typeof(ContentActor),
                    typeof(ApplicationUser),
                    typeof(UserWatchHistory),
                    typeof(Tag),
                    typeof(ContentTag)
                })
                .TwoWays();

            TypeAdapterConfig<Movie, MovieToInsertDto>
                    .NewConfig()
                    .Map(dest => dest.Title, src => src.Title)
                    .Map(dest => dest.Location, src => src.Location)
                    .Map(dest => dest.ReleaseYear, src => src.ReleaseYear)
                    .Map(dest => dest.MinimumAgeToWatch, src => src.MinimumAgeToWatch)
                    .Map(dest => dest.Synopsis, src => src.Synopsis)
                    .Map(dest => dest.LengthInMinutes, src => src.LengthInMinutes)
                    .Map(dest => dest.DirectorId, src => src.DirectorId)
                    .Map(dest => dest.LanguageId, src => src.LanguageId)
                    .Map(dest => dest.ContentGenreId, src => src.ContentGenreId)
                    .IgnoreAttribute(new Type[]
                    {
                    typeof(ContentLanguage),
                    typeof(ContentAward),
                    typeof(ContentGenre),
                    typeof(Director),
                    typeof(Actor),
                    typeof(ContentActor),
                    typeof(ApplicationUser),
                    typeof(UserWatchHistory),
                    typeof(Tag),
                    typeof(ContentTag)
                    })
                    .TwoWays();

            TypeAdapterConfig<TVShowEpisode, TVShowEpisodeToInsertDto>
                .NewConfig()
                .Map(dest => dest.Location, src => src.FileName)
                .TwoWays();

            TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
