using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    public enum OutputType { Conference, Journal, Other }
    public class Publication
    {
        public String Doi { get; set; }
        public String Title { get; set; }
        public String Authors { get; set; }
        public int Year { get; set; } //sure if we want this as a DateTime or just an int
        public OutputType OutputType { get; set; }
        public String Citation { get; set; }
        public DateTime Available { get; set; }

        //Days elapes since the publication became available, not sure if it works
        public static int Age()
        {

            int age = 0;
            DateTime today = DateTime.Today;
            //age = Convert.ToInt32((today - Available).TotalDays);       //--fix this, available can't be static

            return age;
        }
    }
}
