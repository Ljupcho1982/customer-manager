using blazor_project.Components;
using blazor_project.Data;
using blazor_project.Data.Repositories.Customers;
using blazor_project.Data.Repositories.Interfaces;
using blazor_project.Models.Domain;
using blazor_project.Services;
using blazor_project.Services.Auth;
using blazor_project.Services.Customers;
using blazor_project.Services.Interfaces;
using blazor_project.Validators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string: prefer DATABASE_URL (Render's convention; postgres://user:pass@host:port/db)
// fall back to ConnectionStrings:DefaultConnection (Npgsql-format).
var connectionString = BuildConnectionString(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
    {
        opts.SignIn.RequireConfirmedEmail = true;
        opts.User.RequireUniqueEmail = true;
        opts.Password.RequiredLength = 6;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireDigit = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/login";
    opts.LogoutPath = "/logout";
    opts.AccessDeniedPath = "/login";
    opts.ExpireTimeSpan = TimeSpan.FromHours(8);
    opts.SlidingExpiration = true;
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<IEmailSender<ApplicationUser>, GmailEmailSender>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<CustomerValidator>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();

// Bind to $PORT (Render/Heroku/etc convention) if set; otherwise use the launch profile.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var app = builder.Build();

// Ensure the database exists (dev convenience – use migrations for production).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Render terminates TLS at its edge proxy; the app itself only speaks HTTP.
if (app.Environment.IsDevelopment() && string.IsNullOrEmpty(port))
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/logout", async (HttpContext ctx, SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/login");
});

app.Run();

// ---

static string BuildConnectionString(IConfiguration config)
{
    var url = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (!string.IsNullOrWhiteSpace(url))
    {
        // Convert postgres://user:pass@host:port/db (Render format)
        // → Npgsql key/value connection string with SSL enabled.
        var uri = new Uri(url);
        var userInfo = uri.UserInfo.Split(':', 2);
        var dbName = uri.AbsolutePath.TrimStart('/');
        return string.Join(';',
            $"Host={uri.Host}",
            $"Port={(uri.Port > 0 ? uri.Port : 5432)}",
            $"Database={dbName}",
            $"Username={Uri.UnescapeDataString(userInfo[0])}",
            $"Password={Uri.UnescapeDataString(userInfo.Length > 1 ? userInfo[1] : string.Empty)}",
            "SSL Mode=Require",
            "Trust Server Certificate=true");
    }

    return config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "No database connection configured. Set DATABASE_URL or ConnectionStrings:DefaultConnection.");
}
