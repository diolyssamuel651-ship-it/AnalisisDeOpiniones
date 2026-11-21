using AnalisisDeOpiniones.Data.Interfaces;
using AnalisisDeOpiniones.Data.Persistence.Repositories.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// PropertyNamingPolicy = null -> el JSON sale con los mismos nombres (ID_COMENTARIO, FECHA...)
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

// Repositorio que lee de Oracle
builder.Services.AddScoped<IOpinionesDbRepository, OpinionesDbRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
