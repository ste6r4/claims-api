using ClaimsCompanyApi.Common;
using ClaimsCompanyApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClaimsCompanyApi.Tests
{
    public class TestBase : IDisposable
    {
        public const string TestRunSetup = "TestRunSetUp";
        public static AppDbContext AppDbContext = null!;
        public IServiceProvider ServiceProvider { get; }

        public TestBase()
        {
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(Constants.InMemoryConnectionString));
            ServiceProvider = services.BuildServiceProvider();
            FakeData.InitializeData();
            AppDbContext = InitializeDatabase();
        }

        protected AppDbContext InitializeDatabase()
        {
            var context = ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
            return context;
        }

        public void Dispose()
        {
            AppDbContext.Dispose();
        }
    }

    [CollectionDefinition(TestBase.TestRunSetup)]
    public class AssemblyCollection : ICollectionFixture<TestBase>;
}
