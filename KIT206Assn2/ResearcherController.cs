using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    class ResearcherController
    {

        // loads all basic researcher details into a list
        public static List<Researcher> loadResearchers()
        {
            return DbAdaptor.fetchBasicResearcherDetails();
        }


        // loads the details for one spesific researcher given an Id
        public static Researcher loadResearcherDetails(int Id)
        {
            return DbAdaptor.fetchFullResearcherDetails(Id);
        }


        // returns a list of researchers with the following order, A, B, C, D, E, Student
        public static List<Researcher> filterByLevel(List<Researcher> researchers)
        {
            List<Researcher> filteredResearchers = new List<Researcher>()

            int length = researchers.Count;

            for (int i = 0; i < length; i++)
            {
                DbAdapter.getLevel(researchers[i].Id)

                

            }
            


            return null;
        }

           
        public static List<Researcher> filterByName(List<Researcher> researchers, String name)
        {



            return null;
        }


        public static void generateReports(List<Researcher> researchers)
        {

        }




    }
}
