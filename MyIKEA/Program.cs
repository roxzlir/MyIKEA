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

            //** DEN H�R KOD BITEN KAN VI TA OCH L�GGA IN I PROGRAM I EN ANNAN APP VI GJORT OCH K�RA EN NY MIGRATION F�R ATT F� IN LOGIN MALLEN �VEN D�R
            //** H�R l�gger vi om fr�n AddUserIdentity till AddIdentity och l�gger till IdentityRole inuti < > tecknen.
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                //** Vi l�gger till AddDefaultTokenbProviders
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IEmailSender, EmailSender>(); //** Vi m�stre l�gga till dessa och och h�r beh�ver vi se till att vi f�r med using f�r b�de UI.Service och Utility.
            builder.Services.AddControllersWithViews();

            builder.Services.ConfigureApplicationCookie(option => //** H�r l�gger vi s� den ska hitta till "r�tt" sida om man f�rs�ker klistra in l�nken till den li vi har dolt f�r alla utom admin.
            {
                option.LoginPath = $"/Identity/Account/Login";
                option.LogoutPath = $"/Identity/Account/Logout";
                option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            
            builder.Services.AddRazorPages(); //** Vi l�gger till att vi vill kunna k�ra RazorPages f�r att kuinna n� allt med inloggningen.
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

            app.UseAuthentication(); //** Denna vill vi ocks� l�gga till, alltid F�RE Authorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.MapRazorPages(); //**Det �r denna som mappar fram v�ra Razor sidor.

            app.Run();
        }
    }
}
