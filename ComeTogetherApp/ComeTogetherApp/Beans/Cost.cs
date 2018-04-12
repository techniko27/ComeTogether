using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComeTogetherApp
{

    public class Cost
    {
        public string Description { get; set; }

        public int Costs { get; set; }

        public string ID { get; set; }

        public Cost(string descr, int costs)
        {
            Description = descr;
            Costs = costs;
        }
    }
}
