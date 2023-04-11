using Microsoft.EntityFrameworkCore;
using servis.Models;
using Microsoft.AspNetCore.Identity;
using servis.Data;
using servis.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<PsychologistDBContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("MainBase")));

builder.Services.AddDbContext<servisContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainBase")));

builder.Services.AddIdentity<servisUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<servisContext>();


builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
    opt.LoginPath = new PathString("/Identity/Account/Login");
});

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

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
