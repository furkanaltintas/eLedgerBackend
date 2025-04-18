namespace Domain.Entities.Partners;

public class CompanyUser
{
    public Guid CompanyId { get; set; }
    public Guid AppUserId { get; set; }
    public Company? Company { get; set; }
}