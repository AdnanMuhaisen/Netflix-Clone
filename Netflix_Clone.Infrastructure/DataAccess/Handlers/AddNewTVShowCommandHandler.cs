using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain.DTOs;
using Netflix_Clone.Domain.Entities;
using Netflix_Clone.Domain.Exceptions;
using Netflix_Clone.Domain.Options;
using Netflix_Clone.Infrastructure.DataAccess.Commands;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;

namespace Netflix_Clone.Infrastructure.DataAccess.Handlers
{
    public class AddNewTVShowCommandHandler : IRequestHandler<AddNewTVShowCommand, TVShowDto>
    {
        private readonly ILogger<AddNewTVShowCommandHandler> logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IOptions<ContentTVShowOptions> options;

        public AddNewTVShowCommandHandler(ILogger<AddNewTVShowCommandHandler> logger,
            ApplicationDbContext applicationDbContext,
            IOptions<ContentTVShowOptions> options)
        {
            this.logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.options = options;
        }


        public async Task<TVShowDto> Handle(AddNewTVShowCommand request, CancellationToken cancellationToken)
        {
            var IsTheTargetShowExist = applicationDbContext
                .TVShows
                .Where(x => x.Title == request.tVShowToInsertDto.Title)
                .Any();

            if(IsTheTargetShowExist)
            {
                throw new InsertionException($"The TV Show with title : {request.tVShowToInsertDto.Title} is already exist");
            }

            var tvShowToInsert = request.tVShowToInsertDto.Adapt<TVShow>();

            //create an empty folder to save the tv show seasons and episodes:
            string pathOfDirectoryToInsert = Path.Combine(options.Value.TargetDirectoryToSaveTo,
                    request.tVShowToInsertDto.Title);
            try
            {
                var info = Directory.CreateDirectory(pathOfDirectoryToInsert);

                applicationDbContext.TVShows.Add(tvShowToInsert);
                await applicationDbContext.SaveChangesAsync();

                return tvShowToInsert.Adapt<TVShowDto>();
            }
            catch (Exception ex)
            {
                Directory.Delete(pathOfDirectoryToInsert);

                throw new InsertionException($"Can not add the TV-Show with title : {request.tVShowToInsertDto.Title} because this exception : {ex.Message}");
            }
        }
    }
}
