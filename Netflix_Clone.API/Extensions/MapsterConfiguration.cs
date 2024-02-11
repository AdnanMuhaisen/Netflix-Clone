using Mapster;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.API.Extensions
{
    public static class MapsterConfiguration
    {
        public static void RegisterMapsterConfigurations(this IServiceCollection services)
        {
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

            TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
