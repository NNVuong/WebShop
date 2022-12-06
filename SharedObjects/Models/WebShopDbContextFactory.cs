using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SharedObjects.Models
{
    public class WebShopDbContextFactory : IDesignTimeDbContextFactory<WebShopDbContext>
    {
        public WebShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<WebShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new WebShopDbContext(optionsBuilder.Options);
        }
    }
}
