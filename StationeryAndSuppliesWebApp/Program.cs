using DataBaseContextLibrary;
using Microsoft.EntityFrameworkCore;
using StationeryAndSuppliesWebApp.Services;
using System.Reflection.Metadata.Ecma335;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Logging
//builder.Host.UseSerilog((context, configuration)
//    => configuration.ReadFrom.Configuration(
//        context.Configuration.GetSection("Serilog").GetSection("Serilog"))); 

// Only needed if you are adding configuration manager manually
builder.Configuration.AddUserSecrets<Program>();

// get connection string from environment variables in production
string connectionString = builder.Configuration["ConnectionStrings:StationeryAndSuppliesDatabase"]
    ?? throw new InvalidOperationException("Failed to get connection string");

// Add Database service
builder.Services.AddDbContext<StationeryAndSuppliesDatabaseContext>(options
    => options.UseMySQL(connectionString));

builder.Services.AddDatabaseServices();
builder.Services.AddAccountService();

// Add cookie authentication 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // change it later for production
        options.SlidingExpiration = true;

        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Account/AccessDenied";
        
        //options.SessionStore  need to use it later to store cart items in session
    });

// To access HttpContext for custom components
builder.Services.AddHttpContextAccessor();

// Add Razor page services
builder.Services.AddRazorPages();


WebApplication app = builder.Build();

//Add support to logging request with SERILOG
//app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
switch (app.Environment.IsDevelopment())
{
    case true:
        //app.UseHttpLogging();
        app.UseDeveloperExceptionPage();
        break;
    case false:
        app.UseExceptionHandler("/Error"); // need to add error page to handle 404 and 500 erorr
        app.UseHsts();
        break;
}


app.UseExceptionHandler("/Error");// fortesting 

app.UseHttpsRedirection(); 
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseStatusCodePagesWithReExecute("/"); 

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets(); 

app.Run();
