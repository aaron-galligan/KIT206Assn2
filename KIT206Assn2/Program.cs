﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    class Program
    {
        static void Main(string[] args)
        {

            List<object> researchers = DbAdaptor.LoadResearchers();




            Console.WriteLine("Press the any key to exit");
            Console.ReadLine();
        }
    }
}