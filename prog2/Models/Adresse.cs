using SQLite;

namespace prog2.Models
{
    public class Address
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Vorname { get; set; }
        public string Name { get; set; }
        public string Firma { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }

        public int LocationId { get; set; }

        [Ignore]
        public Locations Location { get; set; }
    }
}
