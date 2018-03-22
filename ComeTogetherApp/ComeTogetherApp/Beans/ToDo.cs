using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{
    public class ToDo
    {
        public string Beschreibung { get; set; }
        public string Datum { get; set; }
        public string Name { get; set; }
        public string Kosten { get; set; }
        public string Status { get; set; }
        public string Ort { get; set; }

        public ToDo(string Beschreibung, string Datum, string Name, string Kosten, string Status, string Ort)
        {
            this.Beschreibung = Beschreibung;
            this.Datum = Datum;
            this.Name = Name;
            this.Kosten = Kosten;
            this.Status = Status;
            this.Ort = Ort;
        }
    }
}
