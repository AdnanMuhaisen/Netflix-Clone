using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories
{
    public interface IELSIndexRepository<T> where T : Document
    {
        Task<ELSAddDocumentResponse> IndexDocumentAsync(T document);
        Task<ELSAddDocumentResponse> IndexRangeAsync(IEnumerable<T> documents);
        Task<ELSDeleteDocumentResponse> DeleteDocumentAsync(int documentId);
    }
}
