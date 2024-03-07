
using Nest;
using Netflix_Clone.Domain.Documents;

namespace Netflix_Clone.Application.Services.IServices
{
    public interface IElasticsearchIndexManager<T> where T : Document
    {
        /// <summary>
        /// Create an ELS Index 
        /// </summary>
        /// <param name="IndexName"></param>
        /// <returns>Returns boolean value that represents if the index is created or not</returns>
        Task<bool> CreateIndexAsync(string? IndexName = default);

        /// <summary>
        /// Delete an ELS Index 
        /// </summary>
        /// <param name="IndexName"></param>
        /// <returns>Returns boolean value that represents if the index is deleted or not</returns>
        Task<bool> DeleteIndexAsync(string? IndexName = default);
    }
}
