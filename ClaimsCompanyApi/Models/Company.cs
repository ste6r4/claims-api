using System.ComponentModel.DataAnnotations;

namespace ClaimsCompanyApi.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Postcode { get; set; }
        public string? Country { get; set; }
        public bool Active { get; set; }
        public DateTime? InsuranceEndDate { get; set; }
        public bool HasActivePolicy => InsuranceEndDate.HasValue && InsuranceEndDate.Value > DateTime.Now;
        public List<Claim> Claims { get; set; } = new();

        public static Company CompanyWithoutClaimsAttached(Company? company)
        {
            if (company == null) return new Company();
            return new Company
            {
                Id = company.Id,
                Name = company.Name,
                Address1 = company.Address1,
                Address2 = company.Address2,
                Address3 = company.Address3,
                Postcode = company.Postcode,
                Country = company.Country,
                Active = company.Active,
                InsuranceEndDate = company.InsuranceEndDate
            };
        }
    }
}
