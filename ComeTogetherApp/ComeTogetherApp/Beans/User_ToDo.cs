using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{
    class User_ToDo
    {
        public string isOrganizer { get; set; }
        public string isPaying { get; set; }

        public User_ToDo(string isOrganizer, string isPaying)
        {
            this.isOrganizer = isOrganizer;
            this.isPaying = isPaying;
        }
    }
}
