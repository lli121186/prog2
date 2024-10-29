using System;
namespace prog2.Models
{
    public class Adresse
    {
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Firma { get; set; }
        public string Strasse { get; set; }
        public int PLZ { get; set; }
        public int OrtschaftId { get; set; }
        public Ortschaft Ortschaft { get; set; }
    }
}

