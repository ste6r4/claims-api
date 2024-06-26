using ClaimsCompanyApi.Models;

namespace ClaimsCompanyApi.Data
{
    public static class FakeData
    {
        public static Company? CompanyOne, CompanyTwo;
        public static ClaimType? ClaimTypeOne, ClaimTypeTwo;
        public static Claim? ClaimOne, ClaimTwo;

        public static void InitializeData()
        {
            AddClaimTypes();
            AddCompanies();
            AddClaims();
            CompanyOne!.Claims = [ClaimOne!];
            CompanyTwo!.Claims = [ClaimTwo!];
        }

        public static List<ClaimType> AddClaimTypes()
        {
            ClaimTypeOne = new ClaimType { Id = 1, Name = "ClaimTypeOne" };
            ClaimTypeTwo = new ClaimType { Id = 2, Name = "ClaimTypeTwo" };
            return
            [
                ClaimTypeOne,
                ClaimTypeTwo
            ];
        }

        public static List<Company> AddCompanies()
        {
            CompanyOne = new Company
            {
                Id = 1,
                Name = "Company One",
                Address1 = "Street 1",
                Address2 = "Place 1",
                Address3 = "Location 1",
                Postcode = "LS1 3RY",
                Country = "UK",
                Active = true,
                InsuranceEndDate = DateTime.Now
            };
            CompanyTwo = new Company
            {
                Id = 2,
                Name = "Company Two",
                Address1 = "Street 2",
                Address2 = "Place 2",
                Address3 = "Location 2",
                Postcode = "LS2 3RY",
                Country = "UK",
                Active = false,
                InsuranceEndDate = DateTime.Now.AddDays(-1)
            };
            return
            [
                CompanyOne,
                CompanyTwo
            ];
        }
        
        public static List<Claim> AddClaims()
        {
            ClaimOne = new Claim
            {
                Id = 1,
                UCR = "UCR1",
                CompanyId = 1,
                ClaimDate = DateTime.Now,
                LossDate = DateTime.Now.AddDays(-1),
                AssuredName = "Assured One",
                IncurredLoss = 1000,
                Closed = false
            };
            ClaimTwo = new Claim
            {
                Id = 2,
                UCR = "UCR2",
                CompanyId = 2,
                ClaimDate = DateTime.Now,
                LossDate = DateTime.Now.AddDays(-1),
                AssuredName = "Assured Two",
                IncurredLoss = 2000,
                Closed = true
            };
            return
            [
                ClaimOne,
                ClaimTwo
            ];
        }
    }
}
