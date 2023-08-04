using FluentValidation;
using OfficesAPI.Application.Mappings;
using OfficesAPI.Application.Validators;
using OfficesAPI.Persistence;
using OfficesAPI.Web.Extentions;
using OfficesAPI.Web.Middlewares;
using OfficesAPI.Web.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureLogger(builder.Configuration, builder.Environment, "ElasticConfiguration:Uri");

builder.Services.ConfigureCaching(builder.Configuration.GetSection("RedisConfiguration").Get<RedisSettings>()!);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOfficeValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(ServicesMappingProfile));
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
