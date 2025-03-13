using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WatchListPruebas.Data;
using WatchListPruebas.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(x => x.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

builder.Services.AddTransient<RepositoryWatchlist>();

string connectionString = builder.Configuration.GetConnectionString("SqlWatchlist");

builder.Services.AddDbContext<WatchlistContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddAuthentication(x =>
{
    x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}
).AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Privacy}/{id?}");

    
});

app.Run();
