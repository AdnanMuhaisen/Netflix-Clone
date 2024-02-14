using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Queries;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class GetTVShowSeasonsQueryHandler : IRequestHandler<GetTVShowSeasonsQuery, IEnumerable<TVShowSeasonDto>>
    {
        private readonly ILogger<GetTVShowSeasonsQueryHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;

        public GetTVShowSeasonsQueryHandler(ILogger<GetTVShowSeasonsQueryHandler> logger,
            ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<TVShowSeasonDto>> Handle(GetTVShowSeasonsQuery request, CancellationToken cancellationToken)
        {
            var tvShowSeasons = await applicationDbContext
                .TVShowsSeasons
                .Where(x => x.TVShowId == request.tVShowContentId)
                .ToListAsync();
                
            if(tvShowSeasons is null)
            {
                return Enumerable.Empty<TVShowSeasonDto>();
            }

            var result = tvShowSeasons.Adapt<List<TVShowSeasonDto>>();

            foreach (var season in result)
            {
                season.DirectoryName = Encoding.UTF8.GetString(Convert.FromBase64String(season.DirectoryName));
                foreach (var episode in season.Episodes)
                {
                    episode.FileName = Encoding.UTF8.GetString(Convert.FromBase64String(episode.FileName));
                }
            }

            return result;
        }
    }
}
