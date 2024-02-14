using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, MovieDto>
    {
        private readonly ILogger<GetMovieQuery> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentMovieOptions> options;

        public GetMovieQueryHandler(ILogger<GetMovieQuery> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentMovieOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<MovieDto> Handle(GetMovieQuery request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The get movie query handler is start to execute");

            var targetMovieToRetrieve = await applicationDbContext
                .Movies
                .FindAsync(request.contentId);

            ArgumentNullException.ThrowIfNull(targetMovieToRetrieve);

            logger.LogTrace("The movie is retrieved successfully");

            //decode the movie location
            targetMovieToRetrieve.Location = Path.Combine(options.Value.TargetDirectoryToSaveTo, targetMovieToRetrieve.Location);
            //targetMovieToRetrieve.Location = Path.Combine(options.Value.TargetDirectoryToSaveTo,
            //    Encoding.UTF8.GetString(Convert.FromBase64String(targetMovieToRetrieve.Location)));

            return targetMovieToRetrieve.Adapt<MovieDto>(); ;
        }
    }
}
