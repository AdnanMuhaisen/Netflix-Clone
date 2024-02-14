using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using System.Text;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class DeleteTVShowCommandHandler : IRequestHandler<DeleteTVShowCommand, DeletionResultDto>
    {
        private readonly ILogger<DeleteTVShowCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public DeleteTVShowCommandHandler(ILogger<DeleteTVShowCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }

        public async Task<DeletionResultDto> Handle(DeleteTVShowCommand request, CancellationToken cancellationToken)
        {
            var targetTVShowToDelete = await applicationDbContext
                .TVShows
                .SingleOrDefaultAsync(x => x.Id == request.tVShowId);

            if(targetTVShowToDelete is null)
            {
                return new DeletionResultDto
                {
                    IsDeleted = false,
                    Message = $"The target TV Show to delete with id {request.tVShowId} does not exist"
                };
            }

            string pathOfTheTargetTVShow = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                (string.IsNullOrEmpty(targetTVShowToDelete.Location))
                ? $"{targetTVShowToDelete.Title}"
                : Encoding.UTF8.GetString(Convert.FromBase64String(targetTVShowToDelete.Location)));

            if(!Directory.Exists(pathOfTheTargetTVShow))
            {
                return new DeletionResultDto
                {
                    IsDeleted = false,
                    Message = $"Can not find the target TV Show directory"
                };
            }

            //delete
            try
            {
                Directory.Delete(pathOfTheTargetTVShow, true);
            }
            catch(Exception ex)
            {
                return new DeletionResultDto
                {
                    IsDeleted = false,
                    Message = $"Can not delete the target TV Show directory"
                };
            }

            //delete from the database:

            try
            {
                applicationDbContext.TVShows.Remove(targetTVShowToDelete);

                await applicationDbContext.SaveChangesAsync();

                return new DeletionResultDto { IsDeleted = true };
            }
            catch(Exception ex)
            {
                //log
                return new DeletionResultDto
                {
                    IsDeleted = false,
                    Message = "The TV Show is deleted from the disk but can not delete it from the database"
                };
            }
        }
    }
}
