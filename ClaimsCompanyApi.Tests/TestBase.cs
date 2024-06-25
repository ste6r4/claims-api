using ClaimsCompanyApi.Data;

namespace ClaimsCompanyApi.Tests
{
    public class TestBase : IDisposable
    {
        public const string TestRunSetup = "TestRunSetUp";

        public static readonly AppDbContext AppDbContext = FakeContextHelper.CreateInMemoryDbContext();

        public TestBase()
        {
            FakeData.InitializeData();
        }

        public void Dispose()
        {
            AppDbContext.Dispose();
        }
    }

    [CollectionDefinition(TestBase.TestRunSetup)]
    public class AssemblyCollection : ICollectionFixture<TestBase>;
}
