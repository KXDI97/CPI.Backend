using System.Threading;

namespace CatalogService.Application.Clients;


public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllAsync(CancellationToken ct = default);
    Task<ClientDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ClientDto> CreateAsync(CreateClientDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateClientDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
