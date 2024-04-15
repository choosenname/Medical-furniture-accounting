using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalFurnitureAccounting.Models
{
    public class AcceptanceAct
    {
        public int AcceptanceActId { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public string Room { get; set; }
        public string SupplierName { get; set; }
    }
}
