using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC;
using Calenderapp.MVC.Models;
using Calenderapp.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Connection string is read from configuration only — never hardcoded. Locally that's
// User Secrets (`dotnet user-secrets set "ConnectionStrings:LocalConnection" "..."`),
// in production it's an environment variable / the hosting platform's config.
var connectionString = builder.Configuration.GetConnectionString("LocalConnection")
	?? throw new InvalidOperationException("Connection string 'LocalConnection' not found.");

builder.Services.AddDbContext<CalenderAppContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<CalenderAppContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;
	options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.Cookie.Name = "CalenderAppCookies";
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
	options.LoginPath = "/Identity/Account/Login";
	options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
	options.SlidingExpiration = true;
});

builder.Services.AddScoped<ISearchEngine, SearchEngine>();

// Generic per-entity service, one registration per entity that implements
// IAuditableEntity — see Services/AuditableEntityService.cs and
// Controllers/EntityCrudController.cs for what this replaces.
builder.Services.AddScoped(typeof(AuditableEntityService<>));
builder.Services.AddScoped<RoomAvailabilityService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
