using Tienda_Celulares_APP.Web;
using Tienda_Celulares_APP.Web.Components;
using Tienda_Celulares_APP.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();
// Register EF Core DbContext and inventory service
// Allow overriding the connection string with an environment variable for local/CI scenarios
var connectionString = Environment.GetEnvironmentVariable("TIENDA_DEFAULT_CONNECTION")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("No database connection string configured. Set 'ConnectionStrings:DefaultConnection' in appsettings.json or the TIENDA_DEFAULT_CONNECTION environment variable.");
}

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<InventoryService>();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

var app = builder.Build();

var seedOnStartup = Environment.GetEnvironmentVariable("TIENDA_SEED_ON_STARTUP");
var seedFlagConfigured = builder.Configuration.GetValue<bool?>("Database:SeedOnStartup");
var doSeed = (seedFlagConfigured ?? false) || (!string.IsNullOrEmpty(seedOnStartup) && (seedOnStartup == "1" || seedOnStartup.Equals("true", StringComparison.OrdinalIgnoreCase)));
if (doSeed && app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        db.Database.EnsureCreated();

        var inventory = scope.ServiceProvider.GetRequiredService<InventoryService>();
        inventory.SeedAsync().GetAwaiter().GetResult();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
