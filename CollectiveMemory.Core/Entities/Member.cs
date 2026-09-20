using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectiveMemory.Core.Entities
{
    public class Member : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<string> Bands { get; set; }
        public List<string> FavoriteMusic { get; set; }
        public List<string> Instruments {  get; set; }
        public string Bio {  get; set; }
        public string? Image { get; set; }

    }
}
