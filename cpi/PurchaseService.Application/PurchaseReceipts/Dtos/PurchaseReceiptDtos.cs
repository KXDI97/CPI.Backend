using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PurchaseService.Application.PurchaseReceipts.Dtos;

public record PurchaseReceiptDto(
    int ReceiptId,
    int PurchaseOrderId,
    DateTime ReceiptDate
);

public record CreatePurchaseReceiptDto(
    int PurchaseOrderId,
    DateTime ReceiptDate
);

public record UpdatePurchaseReceiptDto(
    int PurchaseOrderId,
    DateTime ReceiptDate
);
