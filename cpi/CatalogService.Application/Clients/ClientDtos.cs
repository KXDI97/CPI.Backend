namespace CatalogService.Application.Clients;


public record ClientDto(
    int ClientId, string Name, string ClientType,
    string DocumentType, string DocumentID,
    string? Email, string? Phone, string? Website, string? Address
);

public record CreateClientDto(
    string Name, string ClientType, string DocumentType, string DocumentID,
    string? Email, string? Phone, string? Website, string? Address
);

public record UpdateClientDto(
    string Name, string ClientType, string DocumentType, string DocumentID,
    string? Email, string? Phone, string? Website, string? Address
);
