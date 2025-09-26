using System.Threading;

namespace TransactionService.Application.Transaction;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken ct = default);
    Task<TransactionDto?> GetByIdAsync(int transactionNumber, CancellationToken ct = default);
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int transactionNumber, UpdateTransactionDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int transactionNumber, CancellationToken ct = default);
}
