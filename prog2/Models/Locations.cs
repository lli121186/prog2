using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prog2.Models
{
    public class Locations
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Postleitzahl { get; set; }
        public string Ortschaft { get; set; }
    }
}
