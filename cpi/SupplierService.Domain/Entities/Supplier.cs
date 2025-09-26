namespace SupplierService.Domain.Entities
{
    public class Supplier
    {
    public int SupplierId { get; set; }     

    public string Name { get; set; } = string.Empty; 

    public string? Contact { get; set; }   
    public string? Phone { get; set; }    
    public string? Email { get; set; }     
    public string? Address { get; set; }   
    }
}
