using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, IEnumerable<MovieDto>>
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<GetAllMoviesHandler> logger;

        public GetAllMoviesHandler(
            ApplicationDbContext applicationDbContext,
            ILogger<GetAllMoviesHandler> logger
            )
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }


        public async Task<IEnumerable<MovieDto>> Handle(GetAllMoviesQuery request,
            CancellationToken cancellationToken)
        {
            var movies = await applicationDbContext
                .Movies
                .AsNoTracking()
                .ToListAsync();

            logger.LogTrace($"The movies are retrieved from the database");

            var result = Enumerable.Empty<MovieDto>();
            try
            {
                logger.LogTrace("try to map the database result movie list of dto`s");

                result = movies.Adapt<List<MovieDto>>();

                logger.LogTrace("the movies list mapped successfully");

                result = result.ToList();
            }
            catch(Exception ex)
            {
                logger.LogError($"the movies the comes from the database failed to map because : {ex.Message}");
            }
            return result;
        }
    }
}
