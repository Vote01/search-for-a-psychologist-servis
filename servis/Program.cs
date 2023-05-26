using Microsoft.EntityFrameworkCore;
using servis.Models;
using Microsoft.AspNetCore.Identity;
using servis.Data;
using servis.Areas.Identity.Data;
using OfficeOpenXml;
using Quartz;
using servis.Jobs;

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

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("ReportSender");
    var jobKeyP = new JobKey("ReportSenderP");
    var jobKeyC = new JobKey("ReportSenderC");

    q.AddJob<ReportSender>(opts => opts.WithIdentity(jobKey));
    q.AddJob<ReportSenderP>(opts => opts.WithIdentity(jobKeyP));
    q.AddJob<ReportSenderC>(opts => opts.WithIdentity(jobKeyC));

    q.AddTrigger(t => t
    .ForJob(jobKey)
    .WithIdentity("ReportSender-trigger")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).WithRepeatCount(0)
    )
    );
    q.AddTrigger(t => t
    .ForJob(jobKeyP)
    .WithIdentity("ReportSenderP-trigger")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).WithRepeatCount(0)
    )
    );
    q.AddTrigger(t => t
    .ForJob(jobKeyC)
    .WithIdentity("ReportSenderC-trigger")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).WithRepeatCount(0)
    )
    );
}
);
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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
