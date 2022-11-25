using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rocky_Models;

namespace Rocky.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Название таблицы "Category"
        public DbSet<Category> Category {get; set;}

        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product  { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        // Добавление миграции:
        // 1. Установить Microsoft.EntityFrameworkCore.Tools
        // 2. Создание миграции: Средства -> Диспетчер пакетов NuGet ->
        // -> Консоль диспетчера пакетов -> add-migration addCategoryToDatabase
        // 3. Отправка миграции в базу данных: update-database
    }
}
