using MediatR;
using Netflix_Clone.Domain.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Queries
{
    public class GetMoviesByQuery : IRequest<ApiResponseDto>
    {
        public readonly int? genreId;
        public readonly int? releaseYear;
        public readonly int? minimumAgeToWatch;
        public readonly int? languageId;
        public readonly int? directorId;

        public GetMoviesByQuery(
            int? GenreId = default,
            int? ReleaseYear = default,
            int? MinimumAgeToWatch = default,
            int? LanguageId = default,
            int? DirectorId = default)
        {
            genreId = GenreId;
            releaseYear = ReleaseYear;
            minimumAgeToWatch = MinimumAgeToWatch;
            languageId = LanguageId;
            directorId = DirectorId;
        }
    }
}
