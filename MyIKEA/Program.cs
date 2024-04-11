using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MyIKEA.Data;
using MyIKEA.Utility;

namespace MyIKEA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //** DEN HÄR KOD BITEN KAN VI TA OCH LÄGGA IN I PROGRAM I EN ANNAN APP VI GJORT OCH KÖRA EN NY MIGRATION FÖR ATT FÅ IN LOGIN MALLEN ÄVEN DÄR
            //** HÄR lägger vi om från AddUserIdentity till AddIdentity och lägger till IdentityRole inuti < > tecknen.
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                //** Vi lägger till AddDefaultTokenbProviders
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IEmailSender, EmailSender>(); //** Vi måstre lägga till dessa och och här behöver vi se till att vi får med using för både UI.Service och Utility.
            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureApplicationCookie(option => //** Här lägger vi så den ska hitta till "rätt" sida om man försöker klistra in länken till den li vi har dolt för alla utom admin.
            {
                option.LoginPath = $"/Identity/Account/Login";
                option.LogoutPath = $"/Identity/Account/Logout";
                option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            
            builder.Services.AddRazorPages(); //** Vi lägger till att vi vill kunna köra RazorPages för att kuinna nå allt med inloggningen.
            //** TILL DEN HIT 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //** Denna vill vi också lägga till, alltid FÖRE Authorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.MapRazorPages(); //**Det är denna som mappar fram våra Razor sidor.

            app.Run();
        }
    }
}
