using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Shared.DTOs;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories
{
    public interface IUsersIndexRepository : IELSIndexRepository<UserDocument>
    {
        /// <summary>
        /// Search by the user name (both FirstName and LastName) 
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns>Return list of users based on the search result</returns>
        Task<ELSSearchResponse<UserDocument>> SearchByUserNameAsync(string searchQuery);
    }
}
