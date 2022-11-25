using EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocky_DataAccess.Data;
using System;
using Microsoft.AspNetCore.Http.Features;
using UIServices = Microsoft.AspNetCore.Identity.UI.Services;

namespace Rocky
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ���� ����� ���������� ������ ����������. ����������� ���� �����
        // ��� ���������� �������� � ���������.
        public void ConfigureServices(IServiceCollection services)
        {
            // ������ ��� �������� ��������� ���� ������
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // ��������� ��������� ����������� ��� �������� � ApplicationDbContext
                // ��������� ��������� ��� ����������� � �� MSSQL
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddIdentity<IdentityUser, IdentityRole>()
                // ��������� ������ ��� �������������� ������
                .AddDefaultTokenProviders()
                // �������� Razor ��� �������������
                .AddDefaultUI()
                // ������������ ��� �������� ������ �����������
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // ���������� �������� ������������ �� ����� appsettings
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<Configuration>();
            // ����������� EmailConfiguration ��� ��������
            // Singleton: ������ ������� ��������� ��� ������ ��������� � ����
            services.AddSingleton(emailConfig);
            // ����������� ������� �������� �����
            services.AddScoped<ISender, Sender>();
            // ����������� �������-������� ��� �������� ����� 
            // Scoped: ��� ������� ������� ��������� ���� ������ �������
            services.AddScoped<UIServices.IEmailSender, Rocky_Utility.EmailSender>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddHttpContextAccessor();

            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

        }

        // ���� ����� ���������� ������ ����������. ����������� ���� ����� ��� ��������� ��������� HTTP-��������.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
