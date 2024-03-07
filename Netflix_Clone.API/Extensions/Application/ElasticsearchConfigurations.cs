using Elasticsearch.Net;
using Nest;
using Netflix_Clone.Application.Services.ELS;
using Netflix_Clone.Application.Services.IServices;
using Netflix_Clone.Domain.Documents;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.ELS_Repositories.ELS_IRepositories;

namespace Netflix_Clone.API.Extensions.Application
{
    public static class ElasticsearchConfigurations
    {
        /// <summary>
        /// Configure the ELS Cloud connection and mappings
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterElasticsearchClient(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IElasticClient>(implementationFactory: c =>
            {
                var settings = new ConnectionSettings(cloudId: builder.Configuration["ELS:CloudId"],
                    new BasicAuthenticationCredentials(username: builder.Configuration["ELS:Username"],
                    password: builder.Configuration["ELS:Password"]))
                .ApiKeyAuthentication(id: builder.Configuration["ELS:ApiKeyId"], apiKey: builder.Configuration["ELS:ApiKey"])
                .DefaultMappingFor<MovieDocument>(m => m.IndexName(builder.Configuration["ELSIndices:MoviesIndexName"]))
                .DefaultMappingFor<TVShowDocument>(m => m.IndexName(builder.Configuration["ELSIndices:TVShowsIndexName"]))
                .DefaultMappingFor<UserDocument>(m => m.IndexName(builder.Configuration["ELSIndices:UsersIndexName"]));

                return new ElasticClient(settings);
            });
        }

        /// <summary>
        /// Register the Indices managers and the indices repositories
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterElasticsearchIndices(this WebApplicationBuilder builder)
        {
            //repositories and managers
            builder.Services.AddScoped<IMoviesIndexManager, MoviesIndexManager>();
            builder.Services.AddScoped<IMoviesIndexRepository, MoviesIndexRepository>();

            builder.Services.AddScoped<ITVShowsIndexManager, TVShowsIndexManager>();
            builder.Services.AddScoped<ITVShowsIndexRepository, TVShowsIndexRepository>();

            builder.Services.AddScoped<IUsersIndexManager, UsersIndexManager>();
            builder.Services.AddScoped<IUsersIndexRepository, UsersIndexRepository>();
        }
    }
}
