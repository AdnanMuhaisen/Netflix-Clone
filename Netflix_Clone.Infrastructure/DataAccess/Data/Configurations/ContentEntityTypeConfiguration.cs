using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Infrastructure.DataAccess.Data.Configurations
{
    internal class ContentEntityTypeConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.ToTable("tbl_Contents");

            builder.HasKey(e => e.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // inheritance mapping strategy
            builder.UseTphMappingStrategy();

            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(x => x.Synopsis)
                .HasMaxLength(300)
                .IsRequired(true);

            builder.Property(x => x.Rating)
                .HasMaxLength(40)
                .IsRequired(true);

            builder.Property(x => x.Location)
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(x => x.TotalNumberOfDownloads)
                .HasDefaultValue(0)
                .IsRequired(true);

            builder.Property(x => x.IsAvailableToDownload)
                .HasDefaultValue(true)
                .IsRequired(true);

            //relationships

            builder.HasOne(x => x.ContentLanguage)
                .WithMany(x => x.LanguageContents)
                .HasForeignKey(x => x.LanguageId)
                .IsRequired();

            builder.HasMany(x => x.ContentAwards)
                .WithMany(x => x.AwardContents)
                .UsingEntity<ContentAward>(
                l => l.HasOne(x => x.Award).WithMany(x => x.ContentsAwards).IsRequired(false),
                l => l.HasOne(x => x.Content).WithMany(x => x.ContentsAwards).IsRequired(false)
                );

            //builder.HasOne(x => x.Genre)
            //    .WithMany(x => x.GenreContents)
            //    .HasForeignKey(x => x.GenreId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.Director)
            //    .WithMany(x => x.Contents)
            //    .HasForeignKey(x => x.DirectorId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Actors)
                .WithMany(x => x.Contents)
                .UsingEntity<ContentActor>(
                l => l.HasOne(x => x.Actor).WithMany(x => x.ContentsActors).IsRequired(false),
                l => l.HasOne(x => x.Content).WithMany(x => x.ContentsActors).IsRequired(true)
                );

            builder.HasMany(x => x.WatchedBy)
                .WithMany(x => x.UserHistory)
                .UsingEntity<UserWatchHistory>(
                l => l.HasOne(x => x.ApplicationUser).WithMany(x => x.UsersHistory),
                l => l.HasOne(x => x.Content).WithMany(x => x.UsersHistory)
                );

            builder.HasMany(x => x.Tags)
                .WithMany(x => x.TagContents)
                .UsingEntity<ContentTag>(
                l => l.HasOne(x => x.Tag).WithMany(x => x.ContentsTags),
                l => l.HasOne(x => x.Content).WithMany(x => x.ContentsTags).IsRequired()
                );


            builder.HasMany(x => x.DownloadedBy)
                .WithMany(x => x.Downloads)
                .UsingEntity<ContentDownload>(
                l => l.HasOne(x => x.ApplicationUser).WithMany(y => y.ContentsDownloads).HasForeignKey(y => y.ApplicationUserId),
                l => l.HasOne(x => x.Content).WithMany(y => y.ContentsDownloads).HasForeignKey(y => y.ContentId)
                );
        }
    }
}
