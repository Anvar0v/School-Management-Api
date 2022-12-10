using ClassRoom_Api.Context;
using ClassRoom_Api.Entities;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(Program));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resourses";
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlite("Data source=school.db");
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCors(option =>
{
    option.DefaultPolicyName = "AllOrigin";
    option.AddPolicy("AllOrigin", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(path: "classroom.log", rollingInterval: RollingInterval.Year,
    outputTemplate:
    "{Timestamp:yyyy-MM-dd HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddScoped<LocalizedStringEntity>();
builder.Services.AddMemoryCache();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseHttpsRedirection();

app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new RequestCulture(new CultureInfo("Ru"));
    options.SupportedUICultures = new List<CultureInfo>()
    {
        new CultureInfo("Ru"),
        new CultureInfo("Uz"),
        new CultureInfo("En"),
    };

    options.SupportedCultures = new List<CultureInfo>()
    {
        new CultureInfo("Ru"),
        new CultureInfo("Uz"),
        new CultureInfo("En"),
    };

});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
