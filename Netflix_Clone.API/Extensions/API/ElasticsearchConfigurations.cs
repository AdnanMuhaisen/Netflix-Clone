using Elasticsearch.Net;
using Nest;

namespace Netflix_Clone.API.Extensions.API
{
    public static class ElasticsearchConfigurations
    {
        public static void RegisterElasticsearchClient(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IElasticClient>(implementationFactory: c =>
            {
                var settings = new ConnectionSettings(cloudId: builder.Configuration["CloudId"],
                    new BasicAuthenticationCredentials(username: builder.Configuration["Username"],
                    password: builder.Configuration["Password"]))
                .ApiKeyAuthentication(id: builder.Configuration["ApiKeyId"], apiKey: builder.Configuration["ApiKey"])
                .DefaultIndex("default_index");
                // do the mappings

                return new ElasticClient(settings);
            });
        }
    }
}
