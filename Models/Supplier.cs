namespace MedicalFurnitureAccounting.Models;

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string RegistrationNumber { get; set; }

    public string Addres { get; set; }
    
    public string Country { get; set; }

    public virtual ICollection<Supply> Supply { get; set; }
}