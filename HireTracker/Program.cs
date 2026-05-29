using HireTracker.Controllers;
using HireTracker.Models;
using HireTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//register resend
builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = builder.Configuration["ResendSettings:ApiKey"];
});
builder.Services.AddTransient<IResend, ResendClient>();

//register _emailServices
builder.Services.AddTransient<EmailServices>();

//Register DbContext, use sql server as db provider and pull connection string from appsettings.jason
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register identity service with user and role support
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //users don't need to verify email to sign in
    options.SignIn.RequireConfirmedAccount = false;
})
    //tells identity to use AppDbContext to store user data
    .AddEntityFrameworkStores<AppDbContext>()
    //ennables token generation for password reset email
    .AddDefaultTokenProviders();

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

//Register authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
