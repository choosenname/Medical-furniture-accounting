using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalFurnitureAccounting.Models
{
    public class Shelving
    {
        public int ShelvingId { get; set; }
        public int MaxWeight { get; set; }

        public int CellId { get; set; }
        public virtual Cell Cell { get; set; }
    }
}
