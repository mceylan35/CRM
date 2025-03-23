  

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CRM.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Yapılandırma için appsettings.json dosyasını oku




        string connectionString = "";
        // DbContextOptionsBuilder oluştur
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        
            connectionString = "Host=localhost;Port=5433;Database=CRMDB;Username=postgres;Password=postgres;";
        
        
        builder.UseNpgsql(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
} 