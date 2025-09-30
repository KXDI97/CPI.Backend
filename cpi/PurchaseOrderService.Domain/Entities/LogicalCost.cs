using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseOrderService.Domain.Entities
{
    public class LogicalCost
    {
        [Column("Order_Number")]
        public int OrderNumber { get; set; }

        [Column("International_Transport")]
        public decimal InternationalTransport { get; set; }

        [Column("Local_Transport")]
        public decimal LocalTransport { get; set; }

        [Column("Nationalization")]
        public decimal Nationalization { get; set; }

        [Column("Cargo_Insurance")]
        public decimal CargoInsurance { get; set; }

        [Column("Storage")]
        public decimal Storage { get; set; }

        [Column("Others")]
        public decimal Others { get; set; }

        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}
