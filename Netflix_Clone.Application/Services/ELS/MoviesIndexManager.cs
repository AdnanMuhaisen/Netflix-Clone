using Microsoft.Extensions.Options;
using Nest;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Domain.Options;

namespace Netflix_Clone.Application.Services.ELS
{
    public class MoviesIndexManager(IElasticClient elasticClient,
        IOptions<ELSIndicesOptions> options
        ) : IMoviesIndexManager
    {
        private readonly IElasticClient elasticClient = elasticClient;
        private readonly IOptions<ELSIndicesOptions> options = options;

        public async Task<bool> CreateIndexAsync(string? IndexName = default)
        {
            var createIndexResponse = await elasticClient.Indices.CreateAsync(IndexName ?? options.Value.MoviesIndexName, c => c
            .Map<MovieDocument>(m => m.AutoMap()));

            return createIndexResponse.IsValid ? true : false;
        }

        public async Task<bool> DeleteIndexAsync(string? IndexName = default)
        {
            var deleteIndexResponse = await elasticClient.Indices.DeleteAsync(IndexName ?? options.Value.MoviesIndexName);

            return deleteIndexResponse.IsValid ? true : false;
        }
    }
}
