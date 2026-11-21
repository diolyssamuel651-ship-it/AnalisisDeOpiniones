using AnalisisDeOpiniones.Data.Configuracion;
using AnalisisDeOpiniones.Data.Interfaces;
using AnalisisDeOpiniones.Data.Persistence.Repositories.Api;
using AnalisisDeOpiniones.Data.Persistence.Repositories.Csv;
using AnalisisDeOpiniones.Data.Persistence.Repositories.Db;
using AnalisisDeOpiniones.service;
using AnalisisDeOpiniones.service.Extractores;
using AnalisisDeOpiniones.service.Interfaces;
using AnalisisDeOpiniones.service.Services;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<EtlOptions>(builder.Configuration.GetSection("Etl"));

// Capa Data
builder.Services.AddScoped(typeof(IFileReaderRepository<>), typeof(CsvFileReaderRepository<>));
builder.Services.AddScoped<IOpinionesDbRepository, OpinionesDbRepository>();
builder.Services.AddHttpClient<IComentariosApiClient, ComentariosApiClient>((sp, client) =>
{
    var api = sp.GetRequiredService<IOptions<EtlOptions>>().Value.Api;
    client.BaseAddress = new Uri(api.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(api.TimeoutSegundos);
});

// Capa Process
builder.Services.AddSingleton<IStagingWriter, StagingWriter>();
builder.Services.AddScoped<IExtractor, CsvExtractor>();
builder.Services.AddScoped<IExtractor, DatabaseExtractor>();
builder.Services.AddScoped<IExtractor, ApiExtractor>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();