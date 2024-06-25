using WrestlingInfo.API;
using WrestlingInfo.API.DbContexts;
using WrestlingInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/wrestlinginfo.txt", rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(
	options => {
		options.ReturnHttpNotAcceptable = true; // status 406 if client asks unsupported representation
	}
).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Add details to errors
builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
builder.Services.AddSingleton<WrestlingDataStore>();

builder.Services.AddDbContext<WrestlingInfoContext>(
	dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:WrestlingInfoDBConnectionString"])
);

builder.Services.AddScoped<IWrestlingInfoRepository, WrestlingInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();