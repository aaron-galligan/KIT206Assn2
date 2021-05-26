using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    class PublicationController
    {
        public static List<Publication> loadPublications(Researcher researcher)
        {
            String name = researcher.GivenName + " " + researcher.FamilyName;
            List<Publication> publications;

            publications = DbAdaptor.fetchBasicPublicationDetails(name);

            publications.Sort((p1, p2) => p1.Year.CompareTo(p2.Year));
            publications.Reverse();

            return publications;
        }

        public static Publication loadPublicationDetails(Publication publication)
        {
            return DbAdaptor.completePublicationDetails(publication);
        }


        public static List<Publication> filterByYearRange(List<Publication> publications, int startyear, int endyear)
        {
            for (int i = 0; i < publications.Count(); i++)
            {
                if (publications[i].Year < startyear || publications[i].Year > endyear)
                {
                    publications.Remove(publications[i]);
                }
            }

            return publications;
        }

        public static List<Publication> invertSort(List<Publication> publications)
        {
            return publications.Reverse();
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
