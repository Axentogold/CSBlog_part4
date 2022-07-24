using CSBlog.Core;
using CSBlog.Core.Repository;
using Microsoft.AspNetCore.Identity;
using CSBlog.Data;
using CSBlog.Data.Repository;
using CSBlog.Models.Blog;
using CSBlog.Models.User;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);


// ============================================
// Add services to the container.


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
  // NLog: Setup NLog for Dependency injection
  builder.Logging.ClearProviders();
  builder.Logging.SetMinimumLevel(LogLevel.Trace);
  builder.Host.UseNLog();
}
catch (Exception exception)
{
  // NLog: catch setup errors
  logger.Error(exception, "Stopped program because of exception");
  throw;
}
finally
{
  // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
  LogManager.Shutdown();
}

// var connectionString = builder.Configuration.GetConnectionString("DbConnection");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlite(connectionString));
// options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<BlogUser>(options => options.SignIn.RequireConfirmedAccount = true)
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  ;


builder.Services.AddControllersWithViews();


#region Authorization

AddAuthorizationPolicies(builder.Services);

#endregion

AddScoped();

builder.Services.AddRazorPages()
  .AddRazorRuntimeCompilation();
builder.Services.AddLogging(loggingBuilder =>
{
  loggingBuilder.AddConsole()
    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning);

  loggingBuilder.AddDebug();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseMigrationsEndPoint();
}
else
{
  app.UseExceptionHandler("/Home/Oops");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
  var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BlogUser>>();
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  SeedData.Seed(roleManager, userManager);
}

app.MapControllerRoute(
  "default",
  "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

void AddAuthorizationPolicies(IServiceCollection services)
{
  {
    services.AddAuthorization(options =>
    {
      options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
      options.AddPolicy(Constants.Policies.RequireModerator, policy => policy.RequireRole(Constants.Roles.Moderator));
      // options.AddPolicy("RequireUser", policy => policy.RequireRole(Constants.Roles.User));
    });
  }
}

void AddScoped()
{
  builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
  builder.Services.AddScoped<IRepository<Tag>, TagRepository>();
  builder.Services.AddScoped<IRepository<Article>, ArticleRepository>();
  builder.Services.AddScoped<IRepository<Comment>, CommentRepository>();
  builder.Services.AddScoped<IUserRepository, UserRepository>();
  builder.Services.AddScoped<IRoleRepository, RoleRepository>();
  builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}