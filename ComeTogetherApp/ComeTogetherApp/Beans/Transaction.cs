using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    public class Transaction
    {
        public string receiver { get; set; }
        public string sender { get; set; }
        public string type { get; set; }
        public string receiverName { get; set; }
        public string senderName { get; set; }
        public int amount { get; set; }

        public Transaction(string receiver, string sender, string type, int amount)
        {
            this.receiver = receiver;
            this.sender = sender;
            this.type = type;
            this.amount = amount;
        }
    }
}
