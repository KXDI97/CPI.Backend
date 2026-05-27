using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PurchaseService.Application.PurchaseReceipts.Dtos;

public record PurchaseReceiptDetailDto(
    int ReceiptDetailId,
    int ReceiptId,
    string ProductId,
    decimal QuantityReceived,
    decimal UnitCost
);

public record CreatePurchaseReceiptDetailDto(
    int ReceiptId,
    string ProductId,
    decimal QuantityReceived,
    decimal UnitCost
);

public record UpdatePurchaseReceiptDetailDto(
    string ProductId,
    decimal QuantityReceived,
    decimal UnitCost
);
