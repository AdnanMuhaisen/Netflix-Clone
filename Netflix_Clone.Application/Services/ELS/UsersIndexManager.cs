using Microsoft.Extensions.Options;
using Nest;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Domain.Options;

namespace Netflix_Clone.Application.Services.ELS
{
    public class UsersIndexManager(IElasticClient elasticClient,IOptions<ELSIndicesOptions> options) : IUsersIndexManager
    {
        private readonly IElasticClient elasticClient = elasticClient;
        private readonly IOptions<ELSIndicesOptions> options = options;

        public async Task<bool> CreateIndexAsync(string? IndexName = null)
        {
            var createIndexResponse = await elasticClient.Indices.CreateAsync(IndexName ?? options.Value.UsersIndexName, c => c
                .Map<UserDocument>(m => m.AutoMap()));

            return createIndexResponse.IsValid;
        }

        public async Task<bool> DeleteIndexAsync(string? IndexName = null)
        {
            var deleteIndexResponse = await elasticClient.Indices.DeleteAsync(IndexName ?? options.Value.UsersIndexName);

            return deleteIndexResponse.IsValid;
        }
    }
}
