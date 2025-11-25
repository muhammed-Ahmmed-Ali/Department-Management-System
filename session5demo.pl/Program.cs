using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using session5demo.dl.Contexts;
using session5demo.dl.Models.AuthModel;
using session5demo.bl.Common.MapperProfile;
using session5demo.bl.Sevices.DpartmentServices;
using session5demo.bl.Sevices.EmployeeServices;
using session5demo.dl.Reposatory.DepartmentReposartoy.IdepartmentReposatory;
using session5demo.dl.Reposatory.Iemployeerepo;
using session5demo.dl.UOW;
using session5demo.bl.Common.attachmentCommon;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<demoContexsts>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseLazyLoadingProxies();
});

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<demoContexsts>()
.AddDefaultTokenProviders();

// Cookie Authentication (default for Identity)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// App Services / DI
builder.Services.AddScoped<IUOW, UOW>();
builder.Services.AddScoped<Iattachmentservice, Attachmentservice>();
builder.Services.AddScoped<IdepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IemployeeServices, EmployeeServices>();
builder.Services.AddAutoMapper(m => m.AddMaps(typeof(DepartmentProfile).Assembly));

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();  // Must be before Authorization
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
