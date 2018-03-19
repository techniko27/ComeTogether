using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{
    public class User
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string ID { get; set; }
        public string PayPal_me_link { get; set; }

        public User(string userName, string email)
        {
            this.userName = userName;
            this.email = email;
        }
    }
}
