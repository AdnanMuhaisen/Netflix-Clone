using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.UnitOfWork
{
    public class UnitOfWork(ApplicationDbContext applicationDbContext) : IUnitOfWork
    {
        public IContentRepository ContentRepository { get; set; } = new ContentRepository(applicationDbContext);
        public IMoviesRepository MoviesRepository { get; set; } = new MoviesRepository(applicationDbContext);
        public ITVShowRepository TVShowRepository { get; set; } = new TVShowRepository(applicationDbContext);
        public ITVShowSeasonRepository TVShowSeasonRepository { get; set; } = new TVShowSeasonRepository(applicationDbContext);
        public ITVShowEpisodeRepository TVShowEpisodeRepository { get; set; } = new TVShowEpisodeRepository(applicationDbContext);
        public IContentGenreRepository ContentGenreRepository { get; set; } = new ContentGenreRepository(applicationDbContext);
        public IContentLanguageRepository ContentLanguageRepository { get; set; } = new ContentLanguageRepository(applicationDbContext);
        public IContentTagRepository ContentTagRepository { get; set; } = new ContentTagRepository(applicationDbContext);
        public ISubscriptionPlanRepository SubscriptionPlanRepository { get; set; } = new SubscriptionPlanRepository(applicationDbContext);
        public ISubscriptionPlanFeatureRepository SubscriptionPlanFeatureRepository { get; set; } = new SubscriptionPlanFeatureRepository(applicationDbContext);
        public IUserSubscriptionPlanRepository UserSubscriptionPlanRepository { get; set; } = new UserSubscriptionPlanRepository(applicationDbContext);
        public IUserWatchHistoryRepository UserWatchHistoryRepository { get; set; } = new UserWatchHistoryRepository(applicationDbContext);
        public IUserWatchListRepository UserWatchListRepository { get; set; } = new UserWatchListRepository(applicationDbContext);
        public IWatchListContentRepository WatchListContentRepository { get; set; } = new WatchListContentRepository(applicationDbContext);
        public ITagRepository TagRepository { get; set; } = new TagRepository(applicationDbContext);
        public IContentDownloadRepository ContentDownloadRepository { get; set; } = new ContentDownloadRepository(applicationDbContext);

        public void SaveChanges()
        {
            applicationDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
