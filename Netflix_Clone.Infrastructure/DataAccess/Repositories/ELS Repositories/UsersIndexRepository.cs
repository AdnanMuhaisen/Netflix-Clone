using Microsoft.Extensions.Options;
using Nest;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories
{
    public class UsersIndexRepository(IElasticClient elasticClient,IOptions<ELSIndicesOptions> options) 
        : ELSIndexRepository<UserDocument>(elasticClient), IUsersIndexRepository
    {
        private readonly IOptions<ELSIndicesOptions> options = options;

        public async Task<ELSSearchResponse<UserDocument>> SearchByUserNameAsync(string searchQuery)
        {
            var searchResponse = await elasticClient.SearchAsync<UserDocument>(s => s
            .Index(options.Value.UsersIndexName)
                .Query(q => q
                        .MultiMatch(mm => mm
                        .Fields(fs => fs
                            .Field(f => f.FirstName)
                            .Field(f => f.LastName))
                        .Query(searchQuery)
                        .MinimumShouldMatch(1))
                        )
                );

            return new ELSSearchResponse<UserDocument>
            {
                Result = searchResponse.Hits.Select(h => h.Source).ToList()
            };
        }
    }
}
