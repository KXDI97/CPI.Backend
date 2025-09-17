namespace CatalogService.Domain.Entities;


public class Client
{
    public int    ClientId     { get; set; }
    public string Name         { get; set; } = null!;
    public string ClientType   { get; set; } = "Empresa"; // Empresa | Persona
    public string DocumentType { get; set; } = "RUT";
    public string DocumentID   { get; set; } = null!;
    public string? Email       { get; set; }
    public string? Phone       { get; set; }
    public string? Website     { get; set; }
    public string? Address     { get; set; }
}
