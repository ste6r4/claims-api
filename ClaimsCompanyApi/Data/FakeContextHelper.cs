using ClaimsCompanyApi.Common;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Data;

public class FakeContextHelper
{
    public static AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(Constants.InMemoryConnectionString).Options;
        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
        return context;
    }
}