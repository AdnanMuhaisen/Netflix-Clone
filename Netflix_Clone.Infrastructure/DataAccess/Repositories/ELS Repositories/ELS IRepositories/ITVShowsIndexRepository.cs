using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories
{
    public interface ITVShowsIndexRepository : IELSIndexRepository<TVShowDocument>
    {
        Task<ELSUpdateDocumentResponse> UpdateDocumentAsync(TVShowDocument document);

        Task<ELSSearchResponse<TVShowDocument>> SearchByTVShowTitleOrSynopsisAsync(string searchQuery);
    }
}
