using Mapster;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Authentication.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Movies.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShowEpisodes.Queries;
using Netflix_Clone.Infrastructure.DataAccess.TVShows.Commands;
using Netflix_Clone.Infrastructure.DataAccess.TVShowsSeasons.Commands;
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

            TypeAdapterConfig<RegistrationRequestDto, RegisterUserCommand>
                .NewConfig()
                .ConstructUsing(src => new RegisterUserCommand(src))
                .Map(dest => dest.registrationRequestDto.FirstName, src => src.FirstName)
                .Map(dest => dest.registrationRequestDto.LastName, src => src.LastName)
                .Map(dest => dest.registrationRequestDto.Email, src => src.Email)
                .Map(dest => dest.registrationRequestDto.Password, src => src.Password)
                .Map(dest => dest.registrationRequestDto.PhoneNumber, src => src.PhoneNumber);


            TypeAdapterConfig<AssignUserToRoleRequestDto, AssignUserToRoleCommand>
                .NewConfig()
                .ConstructUsing(src => new AssignUserToRoleCommand(src));

            TypeAdapterConfig<MovieToInsertDto, AddNewMovieCommand>
                .NewConfig()
                .ConstructUsing(src => new AddNewMovieCommand(src));

            TypeAdapterConfig<TVShowSeasonEpisodesRequestDto, GetTVShowSeasonEpisodesQuery>
                .NewConfig()
                .ConstructUsing(src => new GetTVShowSeasonEpisodesQuery(src));

            TypeAdapterConfig<TVShowEpisodeToInsertDto, AddNewTVShowEpisodeCommand>
                .NewConfig()
                .ConstructUsing(src => new AddNewTVShowEpisodeCommand(src));

            TypeAdapterConfig<TVShowSeasonEpisodeToDeleteDto, DeleteSeasonEpisodeCommand>
                .NewConfig()
                .ConstructUsing(src => new DeleteSeasonEpisodeCommand(src));

            TypeAdapterConfig<TVShowEpisodeRequestDto, GetTVShowEpisodeQuery>
                .NewConfig()
                .ConstructUsing(src => new GetTVShowEpisodeQuery(src));

            TypeAdapterConfig<TVShowToInsertDto, AddNewTVShowCommand>
                .NewConfig()
                .ConstructUsing(src => new AddNewTVShowCommand(src));

            TypeAdapterConfig<TVShowSeasonToInsertDto, AddNewTVShowSeasonCommand>
                .NewConfig()
                .ConstructUsing(src => new AddNewTVShowSeasonCommand(src));

            TypeAdapterConfig<DeleteTVShowSeasonRequestDto, DeleteTVShowSeasonCommand>
                .NewConfig()
                .ConstructUsing(src => new DeleteTVShowSeasonCommand(src));



            TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
