namespace CatalogService.Domain.Entities;

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientType { get; set; } = "Empresa"; // DF en BD
    public string DocumentType { get; set; } = "RUT";   // DF en BD
    public string DocumentID { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
}
