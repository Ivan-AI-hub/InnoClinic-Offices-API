using FluentValidation;
using OfficesAPI.Persistence;
using OfficesAPI.Services.Mappings;
using OfficesAPI.Services.Settings;
using OfficesAPI.Services.Validators;
using OfficesAPI.Web.Extentions;
using OfficesAPI.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOfficeValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(ServicesMappingProfile));
builder.Services.Configure<BlobStorageSettings>(builder.Configuration.GetSection("BlobStorageConfig"));
builder.Services.Configure<OfficesDatabaseSettings>(builder.Configuration.GetSection("OfficesDatabaseConfig"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
