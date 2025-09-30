namespace CatalogService.Api.Clients;

public record ClientDto(
    int ClientId,
    string Name,
    string ClientType,
    string DocumentType,
    string DocumentID,
    string? Email,
    string? Phone,
    string? Website,
    string? Address
);

public record ClientCreateDto(
    string Name,
    string ClientType,
    string DocumentType,
    string DocumentID,
    string? Email,
    string? Phone,
    string? Website,
    string? Address
);

public record ClientUpdateDto(
    string Name,
    string ClientType,
    string DocumentType,
    string DocumentID,
    string? Email,
    string? Phone,
    string? Website,
    string? Address
);

public record PagedResult<T>(IEnumerable<T> Items, int Total);
