using System.ComponentModel.DataAnnotations;

namespace ClaimsCompanyApi.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }
        public string UCR { get; set; }
        public int CompanyId { get; set; }
        public DateTime ClaimDate { get; set; }
        public DateTime LossDate { get; set; }
        public string AssuredName { get; set; } 
        public decimal IncurredLoss { get; set; }
        public bool Closed { get; set; }
        public int DaysOld => (DateTime.Now - ClaimDate).Days;
        public Company? Company { get; set; } 
    }
}
