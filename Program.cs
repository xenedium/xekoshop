using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using xekoshop.Data;
using System.Net;
using xekoshop.Models;
using Microsoft.AspNetCore.HttpOverrides;
using xekoshop.Interfaces;
using xekoshop.Services;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection (Discord Webhook)
builder.Services.AddSingleton<IDiscordWebhook>(
    new DiscordWebhook(
        builder.Configuration["Discord:WebhookUrl"] ?? throw new InvalidOperationException("Discord Webhook URL not found."), 
        builder.Configuration["Discord:WebhookUser"] ?? throw new InvalidOperationException("Discord Webhook User not found."), 
        builder.Configuration["Discord:WebhookAvatar"] ?? throw new InvalidOperationException("Discord Webhook Avatar not found.")
        )
);
builder.Services.AddScoped<IGeolocationService, GeolocationService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(connectionString);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    // Need to setup SMTP or mail service provider to use these O_o
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));  // No Load Balancers just NGINX Reverse Proxy
    options.ForwardLimit = null;
});

builder.Services.AddAuthentication()
    .AddCookie(options => {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.IsEssential = true;
    })
    .AddGoogle(options => {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Google Client ID not found.");
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Google Client Secret not found.");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseForwardedHeaders();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();