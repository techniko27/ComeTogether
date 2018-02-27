using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{
    class Event
    {
        public string Beschreibung { get; set; }
        public string Datum { get; set; }
        public string Name { get; set; }
        public string Ort { get; set; }
        public Event(string Beschreibung, string Datum, string Name, string Ort)
        {
            this.Beschreibung = Beschreibung;
            this.Datum = Datum;
            this.Name = Name;
            this.Ort = Ort;
        }
    }
}
