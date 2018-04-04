using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{
    public class Event
    {
        public string Beschreibung { get; set; }
        public string Datum { get; set; }
        public string Name { get; set; }
        public string Ort { get; set; }
        public string Bild { get; set; }
        public string ID { get; set; }
        public string adminID { get; set; }
        public string Status { get; set; }
        public Event(string Beschreibung, string Datum, string Name, string Ort, string Bild, string ID, string adminID, string Status)
        {
            this.Beschreibung = Beschreibung;
            this.Datum = Datum;
            this.Name = Name;
            this.Ort = Ort;
            this.Bild = Bild;
            this.ID = ID;
            this.adminID = adminID;
            this.Status = Status;
        }
    }
}
