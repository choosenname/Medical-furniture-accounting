namespace MedicalFurnitureAccounting.Models;

public class Supplier
{
    public Supplier(string name, string phone, string email,
        string registrationNumber, string addres)
    {
        Name = name;
        Phone = phone;
        Email = email;
        RegistrationNumber = registrationNumber;
        Addres = addres;
    }

    public Supplier()
    {
    }

    public int SupplierId { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string RegistrationNumber { get; set; }

    public string Addres { get; set; }

    public virtual ICollection<Supply> Supply { get; set; }
}