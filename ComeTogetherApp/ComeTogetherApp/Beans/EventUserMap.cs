using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{
    class EventUserMap
    {
        public string UserID { get; set; }
        public string EventID { get; set; }
        public EventUserMap(string UserID, string EventID)
        {
            this.UserID = UserID;
            this.EventID = EventID;
        }

    }
}
