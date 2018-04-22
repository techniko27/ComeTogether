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

        public User(string userName, string email, string PayPal_me_link)
        {
            this.userName = userName;
            this.email = email;
            this.PayPal_me_link = PayPal_me_link;
        }
        public override bool Equals(object obj)
        {
            var user = obj as User;
            if (user == null)
                return false;
            return ID.Equals(user.ID);
        }
    }
}
