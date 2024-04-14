﻿namespace MedicalFurnitureAccounting.Models;

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    // Другие свойства поставщика

    public ICollection<Product> Products { get; set; }
}