using System;

namespace SupplierService.Application.Supplier;

public record SupplierDto(
    int SupplierId,
    string Name,
    string? Contact,
    string? Phone,
    string? Email,
    string? Address
);

public record CreateSupplierDto(
    string Name,
    string? Contact,
    string? Phone,
    string? Email,
    string? Address
);

public record UpdateSupplierDto(
    string Name,
    string? Contact,
    string? Phone,
    string? Email,
    string? Address
);

public record DeleteSupplierDto(
    int SupplierId
);