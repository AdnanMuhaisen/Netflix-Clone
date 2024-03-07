using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories
{
    public interface IMoviesIndexRepository : IELSIndexRepository<MovieDocument>
    {
        Task<ELSUpdateDocumentResponse> UpdateDocumentAsync(MovieDocument document);
        Task<ELSSearchResponse<MovieDocument>> SearchByMovieTitleOrSynopsisAsync(string searchQuery);
    }
}
