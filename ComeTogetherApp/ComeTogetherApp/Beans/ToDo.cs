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
        public int Kosten { get; set; }
        public string Status { get; set; }
        public string Ort { get; set; }
        public string ID { get; set; }
        public string OrganisatorID { get; set; }

        public ToDo(string Beschreibung, string Datum, string Name, int Kosten, string Status, string Ort, string OrganisatorID)
        {
            this.Beschreibung = Beschreibung;
            this.Datum = Datum;
            this.Name = Name;
            this.Kosten = Kosten;
            this.Status = Status;
            this.Ort = Ort;
            this.OrganisatorID = OrganisatorID;
        }

        public override bool Equals(object obj)
        {
            var toDo = obj as ToDo;
            if (toDo == null)
                return false;
            return ID.Equals(toDo.ID);
        }
    }
}
