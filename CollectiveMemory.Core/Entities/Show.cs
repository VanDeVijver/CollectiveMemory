using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectiveMemory.Core.Entities
{
    public class Show : BaseEntity
    {
        public string Venue {  get; set; }
        public string City { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public DateTime Date { get; set; }

        public string? AdditionalInfo { get; set; }
        public List<string>? AdditionalLinks { get; set; }

        public double Price { get; set; }
    }
}
