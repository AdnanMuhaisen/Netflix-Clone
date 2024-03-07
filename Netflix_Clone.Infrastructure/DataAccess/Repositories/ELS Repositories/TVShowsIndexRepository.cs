using Microsoft.Extensions.Options;
using Nest;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories
{
    public class TVShowsIndexRepository(IElasticClient elasticClient,IOptions<ELSIndicesOptions> options)
        : ELSIndexRepository<TVShowDocument>(elasticClient), ITVShowsIndexRepository
    {
        private readonly IOptions<ELSIndicesOptions> options = options;

        public async Task<ELSUpdateDocumentResponse> UpdateDocumentAsync(TVShowDocument document)
        {
            var updateResponse = await elasticClient.UpdateAsync<TVShowDocument, object>(document.Id, u => u.
                    Index(options.Value.TVShowsIndexName)
                    .Doc(document));

            return new ELSUpdateDocumentResponse
            {
                IsUpdated = updateResponse.IsValid,
                ErrorList = updateResponse.IsValid ? default : new List<string> { updateResponse.ServerError.Error.Reason }
            };
        }

        public async Task<ELSSearchResponse<TVShowDocument>> SearchByTVShowTitleOrSynopsisAsync(string searchQuery)
        {
            var searchResponse = await elasticClient.SearchAsync<TVShowDocument>(s => s
            .Index(options.Value.TVShowsIndexName)
                .Query(q => q
                        .MultiMatch(fs => fs
                            .Fields(f => f
                                .Field(x => x.Title)
                                .Field(x => x.Synopsis))
                            .Query(searchQuery)
                            )
                        )
            );

            return new ELSSearchResponse<TVShowDocument>
            {
                Result = searchResponse
                .Hits
                .Select(h => h.Source)
                .ToList()
            };
        }

    }
}
