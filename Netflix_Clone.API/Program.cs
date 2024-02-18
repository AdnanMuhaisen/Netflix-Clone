using Netflix_Clone.API.Extensions.API;
using Netflix_Clone.API.Extensions.Application;
using Netflix_Clone.API.Extensions.Domain;
using Netflix_Clone.API.Extensions.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.RegisterDomainServices();
builder.RegisterAPIServices();
builder.RegisterInfrastructureServices();
builder.RegisterApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
