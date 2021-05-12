using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    public enum EmploymentLevel { A, B, C, D, E }

    public class Position
    {
        public EmploymentLevel EmploymentLevel { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }




    }
}
