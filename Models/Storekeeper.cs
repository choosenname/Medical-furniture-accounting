namespace MedicalFurnitureAccounting.Models;

public class Storekeeper
{
    public Storekeeper(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public int StorekeeperId { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }
}