using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IContentGenreRepository ContentGenreRepository { get; set; }
        IContentLanguageRepository ContentLanguageRepository { get; set; }
        IContentRepository ContentRepository { get; set; }
        IContentDownloadRepository ContentDownloadRepository { get; set; }
        IContentTagRepository ContentTagRepository { get; set; }
        IMoviesRepository MoviesRepository { get; set; }
        ISubscriptionPlanFeatureRepository SubscriptionPlanFeatureRepository { get; set; }
        ISubscriptionPlanRepository SubscriptionPlanRepository { get; set; }
        ITVShowEpisodeRepository TVShowEpisodeRepository { get; set; }
        ITVShowRepository TVShowRepository { get; set; }
        ITVShowSeasonRepository TVShowSeasonRepository { get; set; }
        IUserSubscriptionPlanRepository UserSubscriptionPlanRepository { get; set; }
        IUserWatchHistoryRepository UserWatchHistoryRepository { get; set; }
        IUserWatchListRepository UserWatchListRepository { get; set; }
        IWatchListContentRepository WatchListContentRepository { get; set; }
        ITagRepository TagRepository { get; set; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}