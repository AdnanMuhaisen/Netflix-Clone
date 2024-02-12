using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
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

        public GetMovieQueryHandler(ILogger<GetMovieQuery> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<MovieDto> Handle(GetMovieQuery request, CancellationToken cancellationToken)
        {
            logger.LogTrace("The get movie query handler is start to execute");

            var targetMovieToRetrieve = await applicationDbContext
                .Movies
                .FindAsync(request.contentId);

            ArgumentNullException.ThrowIfNull(targetMovieToRetrieve);

            logger.LogTrace("The movie is retrieved successfully");

            targetMovieToRetrieve.Location = Encoding.UTF8.GetString(Convert.FromBase64String(targetMovieToRetrieve.Location));
           
            return targetMovieToRetrieve.Adapt<MovieDto>(); ;
        }
    }
}
