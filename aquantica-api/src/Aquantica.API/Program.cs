using Aquantica.API;
using Aquantica.DAL;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseServiceProviderFactory(factory: new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new DiModule()));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.BuildServiceProvider()
    .GetService<ApplicationDbContext>()
    .Database
    .Migrate();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Host.UseSerilog();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    loggingBuilder.AddSerilog(dispose: true);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();

Log.CloseAndFlush();
