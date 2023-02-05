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
using Rocky_DataAccess.Repository.IRepository;
using Rocky_DataAccess.Repository;

namespace Rocky
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Этот метод вызывается средой выполнения. Используйте этот метод
        // для добавления сервисов в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            // Сервис для создания контекста базы данных
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Настройка параметра подключения для передачи в ApplicationDbContext
                // Настройка контекста для подключения к бд MSSQL
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddIdentity<IdentityUser, IdentityRole>()
                // Получение токена для восстановления пароля
                .AddDefaultTokenProviders()
                // Страницы Razor для идентификации
                .AddDefaultUI()
                // Конфигурация для создания таблиц авторизации
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Извлечение значения конфигурации из файла appsettings
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<Configuration>();
            // Регистрация EmailConfiguration как синглтон
            // Singleton: объект сервиса создается при первом обращении к нему
            services.AddSingleton(emailConfig);
            // Регистрация сервиса отправки почты
            services.AddScoped<ISender, Sender>();
            // Регистрация сервиса-утилиты для отправки почты 
            // Scoped: для каждого запроса создается свой объект сервиса
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

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
            services.AddScoped<IInquiryDetailRepository, InquiryDetailRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            services.AddControllersWithViews();

        }

        // Этот метод вызывается средой выполнения. Используйте этот метод для настройки конвейера HTTP-запросов.
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
