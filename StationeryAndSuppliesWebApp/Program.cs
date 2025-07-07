using DataBaseContextLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using StationeryAndSuppliesWebApp.Services;
using System.Runtime.InteropServices;


ILoggerFactory factory = LoggerFactory.Create(c => c.AddConsole());
ILogger<Program> logger = factory.CreateLogger<Program>();


// Check If application is running in supported OS 

bool IsWindowsOS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
bool IsLinuxOS = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

if(!(IsLinuxOS || IsWindowsOS))
{
    logger.LogCritical("Current OS is not supported, Only Windows OS or Linux OS is supported."); 
    throw new PlatformNotSupportedException("Current OS is not supported"); 
}


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? connectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = IsWindowsOS ? 
        builder.Configuration["ConnectionStrings:StationeryAndSuppliesDatabase"] :
        builder.Configuration["ConnectionStrings__StationeryAndSuppliesDatabase"];

    if (connectionString is null)
    {
        logger.LogCritical("Failed to get connection string in {Environment}", builder.Environment.EnvironmentName);
        throw new InvalidOperationException("Failed to get connection string");
    }
}
else
{
    // Configuring Serilog logger
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
        .WriteTo.Console()
        .WriteTo.File(
            path: Path.Combine("Logs","log-.json"),
            restrictedToMinimumLevel: LogEventLevel.Information,
            fileSizeLimitBytes: 100_000_000, // file limit is 100 MB
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true,
            retainedFileCountLimit: 30,
            formatter: new CompactJsonFormatter())
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Host.UseSerilog();

    // get connection string from environment variables in production
    connectionString = builder.Configuration.GetConnectionString("Default");

    if (connectionString is null)
    {
        factory = LoggerFactory.Create(c => c.AddSerilog());
        logger = factory.CreateLogger<Program>();
        logger.LogCritical("Failed to get connection string in {Environment}", builder.Environment.EnvironmentName);
        throw new InvalidOperationException("Failed to get connection string");
    }


    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
}

factory.Dispose(); 

// Add Database service
builder.Services.AddDbContext<StationeryAndSuppliesDatabaseContext>(options
    => options.UseMySQL(connectionString));

builder.Services.AddDatabaseServices();
builder.Services.AddAccountService();

// Add cookie authentication 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
        options.SlidingExpiration = true;

        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Account/AccessDenied";
        
        //options.SessionStore
    });

// To access HttpContext for custom components
builder.Services.AddHttpContextAccessor();

// Add Razor page services
builder.Services.AddRazorPages();


WebApplication app = builder.Build();

//Add support to logging request with SERILOG
//app.UseSerilogRequestLogging();

// To configure forward headers from proxy 
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
}); 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Reverse proxy will handle SSL 
// app.UseHttpsRedirection(); 
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/MissingPage");

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets(); 

app.Run();
