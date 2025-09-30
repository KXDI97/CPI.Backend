namespace PurchaseOrderService.Application.Purchase;

 public class LogicalCostDto
    {
        public int OrderNumber { get; set; }
        public decimal InternationalTransport { get; set; }
        public decimal LocalTransport { get; set; }
        public decimal Nationalization { get; set; }
        public decimal CargoInsurance { get; set; }
        public decimal Storage { get; set; }
        public decimal Others { get; set; }
    }

public class CreateLogicalCostDto
{
     public int OrderNumber { get; set; }
    public decimal InternationalTransport { get; set; }
    public decimal LocalTransport { get; set; }
    public decimal Nationalization { get; set; }
    public decimal CargoInsurance { get; set; }
    public decimal Storage { get; set; }
    public decimal Others { get; set; }
}

 public class UpdateLogicalCostDto
    {
        public int OrderNumber { get; set; }
        public decimal InternationalTransport { get; set; }
        public decimal LocalTransport { get; set; }
        public decimal Nationalization { get; set; }
        public decimal CargoInsurance { get; set; }
        public decimal Storage { get; set; }
        public decimal Others { get; set; }
    }

public class DeleteLogicalCostDto
{
    public int OrderNumber { get; set; }
}