using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    class PublicationController
    {

        public static List<Publication> loadPublications()
        {



            return null;
        }
        
        public static void sortPublications()
        {

        }

        public static void invertSort() 
        {

        }

        public static void filterByYearRande(int startYear, int finishYear)
        {

        }


        // returns a list of how many publications a researcher has published for a given start year finish
        public static List<int[]> CumulativeCount(DateTime startDate, DateTime finishDate, int Id)
        {

            int count = 0; // how many publications for a particular year
            int year = startDate.Year; // the year being counted

            List<Publication> publications = DbAdaptor.fetchPublicaionCount(Id);

            List<int[]> PubCount = new List<int[]>();

            while (year <= finishDate.Year)
            {
                count = 0;
                for (int i = 0; i < publications.Count; i++)
                {
                    if (publications[i].Year == year)
                    {
                        count++;
                    }
                }

                int[] yearCount = { year, count };
                PubCount.Add(yearCount);
                year++;
            }


            return PubCount;
        }
    }
}
