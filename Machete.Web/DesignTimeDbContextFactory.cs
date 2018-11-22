using System.IO;
using Machete.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

// https://codingblast.com/entityframework-core-idesigntimedbcontextfactory/
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MacheteContext>
{
    public MacheteContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
 
        var builder = new DbContextOptionsBuilder<MacheteContext>();
 
        var connectionString = configuration.GetConnectionString("DefaultConnection");
 
        builder.UseSqlServer(connectionString);
 
        return new MacheteContext(builder.Options);
    }
}