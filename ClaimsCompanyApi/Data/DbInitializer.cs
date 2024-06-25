namespace ClaimsCompanyApi.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        SeedClaimTypes(context);
        SeedCompanies(context);
        SeedClaims(context);
    }

    private static void SeedClaimTypes(AppDbContext context)
    {
        if (context.ClaimTypes.Any())
        {
            return;
        }
        context.ClaimTypes.AddRange(FakeData.AddClaimTypes());
        context.SaveChanges();
    }

    private static void SeedCompanies(AppDbContext context)
    {
        if (context.Companies.Any())
        {
            return;
        }
        context.Companies.AddRange(FakeData.AddCompanies());
        context.SaveChanges();
    }

    private static void SeedClaims(AppDbContext context)
    {
        if (context.Claims.Any())
        {
            return;
        }
        context.Claims.AddRange(FakeData.AddClaims());
        context.SaveChanges();
    }
}