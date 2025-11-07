using GymManagementBLL;
using GymManagementBLL.Services.AttachmentServices;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interface;
using GymManangementDAL.Data.Contexts;
using GymManangementDAL.Data.DataSeed;
using GymManangementDAL.Entities;
using GymManangementDAL.Repositories.Classes;
using GymManangementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"])

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberServices, MemberService>();
            builder.Services.AddScoped<ITrainerServices, TrainerService>();
            builder.Services.AddScoped<IPlanServices, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>( c =>
            {
                c.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IMemberSessionService, MemberSessionService>();

            var app = builder.Build();

            #region Migrate Database - Data Seeding
            using var Scope = app.Services.CreateScope();
            var dbContextObj = Scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigrations = dbContextObj.Database.GetPendingMigrations();
            if (pendingMigrations?.Any() ?? false)
            {
                dbContextObj.Database.Migrate();
            }
            GymDbContextSeeding.SeedData(dbContextObj);
            IdentityDbContextSeeding.SeedData(roleManager, userManager);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
