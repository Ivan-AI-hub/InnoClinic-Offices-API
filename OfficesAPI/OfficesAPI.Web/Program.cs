using FluentValidation;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.DAL;
using OfficesAPI.Services.Settings;
using OfficesAPI.Web.Extentions;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOffice).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<CreateOffice>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<BlobStorageSettings>(builder.Configuration.GetSection("BlobStorageConfig"));
builder.Services.Configure<OfficesDatabaseSettings>(builder.Configuration.GetSection("OfficesDatabaseConfig"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
