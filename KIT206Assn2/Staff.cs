using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    public class Staff : Researcher
    {
        public double ThreeYearAvg { get; set; }
        public double Performance { get; set; }
        public int Supervisions { get; set; }
        public List<String> StudentsSupervised { get; set; }
        
    }
}
