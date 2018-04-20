using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    class CostButtonElement
    {
        public Button costButton { get; set; }
        public User member { get; set; }
        public Task<int> memberCostCalculatingTask { get; set; }

        public CostButtonElement(Button costButton, User member, Task<int> memberCostCalculatingTask)
        {
            this.costButton = costButton;
            this.member = member;
            this.memberCostCalculatingTask = memberCostCalculatingTask;
        }
    }
}
