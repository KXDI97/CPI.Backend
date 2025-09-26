using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Transaction;
using TransactionEntity = TransactionService.Domain.Entities.Transaction;   
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure.Transaction;

public class TransactionService : ITransactionService
{
    private readonly CpiDbContext _db;

    public TransactionService(CpiDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Transactions
            .Select(t => new TransactionDto(
                t.TransactionNumber,
                t.PurchaseOrderId,
                t.InvoiceNumber,
                t.Reminder,
                t.TransactionStatus,
                t.PaymentDate
            ))
            .ToListAsync(ct);
    }

    public async Task<TransactionDto?> GetByIdAsync(int transactionNumber, CancellationToken ct = default)
    {
        return await _db.Transactions
            .Where(t => t.TransactionNumber == transactionNumber)
            .Select(t => new TransactionDto(
                t.TransactionNumber,
                t.PurchaseOrderId,
                t.InvoiceNumber,
                t.Reminder,
                t.TransactionStatus,
                t.PaymentDate
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto, CancellationToken ct = default)
    {
        var entity = new TransactionEntity
        {
            PurchaseOrderId = dto.PurchaseOrderId,
            InvoiceNumber = dto.InvoiceNumber,
            Reminder = dto.Reminder,
            TransactionStatus = dto.TransactionStatus,
            PaymentDate = dto.PaymentDate
        };

        _db.Transactions.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new TransactionDto(
            entity.TransactionNumber,
            entity.PurchaseOrderId,
            entity.InvoiceNumber,
            entity.Reminder,
            entity.TransactionStatus,
            entity.PaymentDate
        );
    }

    public async Task<bool> UpdateAsync(int transactionNumber, UpdateTransactionDto dto, CancellationToken ct = default)
    {
        var entity = await _db.Transactions.FindAsync(new object[] { transactionNumber }, ct);
        if (entity == null) return false;

        entity.Reminder = dto.Reminder ?? entity.Reminder;
        entity.TransactionStatus = dto.TransactionStatus;
        entity.PaymentDate = dto.PaymentDate;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int transactionNumber, CancellationToken ct = default)
    {
        var entity = await _db.Transactions.FindAsync(new object[] { transactionNumber }, ct);
        if (entity == null) return false;

        _db.Transactions.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
