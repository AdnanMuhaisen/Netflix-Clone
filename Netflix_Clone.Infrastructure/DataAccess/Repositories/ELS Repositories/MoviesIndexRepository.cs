using Microsoft.Extensions.Options;
using Nest;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories
{
    public class MoviesIndexRepository(IElasticClient elasticClient, IOptions<ELSIndicesOptions> options)
        : ELSIndexRepository<MovieDocument>(elasticClient), IMoviesIndexRepository
    {
        private readonly IOptions<ELSIndicesOptions> options = options;

        public async Task<ELSUpdateDocumentResponse> UpdateDocumentAsync(MovieDocument document)
        {
            var updateResponse = await elasticClient.UpdateAsync<MovieDocument>(document.Id, u => u
            .Index(options.Value.MoviesIndexName)
            .Doc(document));

            //log the debug information in the update response 

            return new ELSUpdateDocumentResponse
            {
                IsUpdated = updateResponse.IsValid,
                ErrorList = updateResponse.IsValid ? default : new List<string> { updateResponse.ServerError.Error.Reason }
            };
        }

        public async Task<ELSSearchResponse<MovieDocument>> SearchByMovieTitleOrSynopsisAsync(string searchQuery)
        {
            var searchResponse = await elasticClient.SearchAsync<MovieDocument>(s => s
            .Index(options.Value.MoviesIndexName)
                .Query(q => q
                    .MultiMatch(i => i
                     .Fields(fs => fs.Field(f => f.Title)
                                    .Field(f => f.Synopsis))
                     .Query(searchQuery))
                    )
                );

            return new ELSSearchResponse<MovieDocument>
            {
                Result = searchResponse
                .Hits
                .Select(h => h.Source)
                .ToList()
            };
        }
    }
}
