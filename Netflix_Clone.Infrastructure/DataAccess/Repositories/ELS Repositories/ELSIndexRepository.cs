using Nest;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories
{
    public class ELSIndexRepository<T>(IElasticClient elasticClient) : IELSIndexRepository<T> where T : Domain.Documents.Document
    {
        protected readonly IElasticClient elasticClient = elasticClient;

        public async Task<ELSAddDocumentResponse> IndexDocumentAsync(T document)
        {
            var indexResponse = await elasticClient.IndexDocumentAsync(document);
            return new ELSAddDocumentResponse
            {
                IsAdded = indexResponse.IsValid,
                ErrorList = indexResponse.IsValid ? default : new List<string> { indexResponse.ServerError.Error.Reason }
            };
        }

        public async Task<ELSAddDocumentResponse> IndexRangeAsync(IEnumerable<T> documents)
        {
            var indexResponse = await elasticClient.IndexManyAsync(documents);
            return new ELSAddDocumentResponse
            {
                IsAdded = indexResponse.IsValid,
                ErrorList = indexResponse.IsValid ? default : new List<string> { indexResponse.ServerError.Error.Reason }
            };
        }

        public async Task<ELSDeleteDocumentResponse> DeleteDocumentAsync(int documentId)
        {
            var deleteResponse = await elasticClient.DeleteAsync<T>(documentId);
            return new ELSDeleteDocumentResponse
            {
                IsDeleted = deleteResponse.IsValid,
                ErrorList = deleteResponse.IsValid ? default : new List<string> { deleteResponse.ServerError.Error.Reason }
            };
        }
    }
}
