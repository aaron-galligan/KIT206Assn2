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
        // use https://zetcode.com/csharp/sortlist/ to learn how to sort all the stuff (using linq)
        public static List<Researcher> filterByLevel(List<Researcher> researchers)
        {
            char level;
            int length = researchers.Count;

            researchers = addLevel(researchers);

            researchers.Sort((r1, r2) => r1.level.CompareTo(r2.level));

            return researchers;
        }


        //used to add the level variable to the basic information in the list of researchers
        public static List<Researcher> addLevel(List<Researcher> researchers)
        {
            int length = researchers.Count;
            char level;


            for (int i = 0; i < length; i++)
            {
                researchers[i].level = DbAdaptor.getLevel(researchers[i].Id);
            }

            return researchers;
        }


        //user inputs name and then a list of 
        public static List<Researcher> filterByName(List<Researcher> researchers, String filterName)
        {
            List<Researcher> filteredRschrs = new List<Researcher>();
            int length = researchers.Count;
            String name;

            for(int i = 0; i < length; i++)
            {
                name = researchers[i].GivenName + " " + researchers[i].FamilyName;
                
                if (name.Contains(filterName))
                {
                    filteredRschrs.Add(new Researcher
                    {
                        Id = researchers[i].Id,
                        GivenName = researchers[i].GivenName,
                        FamilyName = researchers[i].FamilyName,
                        Title = researchers[i].Title
                    });
                }
            }




            return null;
        }


        // returns a performance report where element 0 is poor performers, 
        //          1 is below average, 2 is minimum, 3 is star performers
        public static List<Researcher>[] generateReports(List<Researcher> researchers)
        {
            List<Researcher> poorPerformers = new List<Researcher>();
            List<Researcher> belowExptn = new List<Researcher>();
            List<Researcher> minimum = new List<Researcher>();
            List<Researcher> starPerformers = new List<Researcher>();
            List<Researcher>[] performanceReport = new List<Researcher>[4];
            int[] s = new int[4];
            double performance;

            for (int i = 0; i < researchers.Count(); i++)
            {
                performance = getPerformance(researchers[i]);

                if (performance <= 0.7)
                {
                    poorPerformers.Add(researchers[i]);
                } 
                else if (performance > 0.7 && performance < 1.1)
                {
                    belowExptn.Add(researchers[i]);
                }
                else if (performance >= 1.1 && performance < 2)
                {
                    minimum.Add(researchers[i]);
                }
                else if (performance > 2)
                {
                    starPerformers.Add(researchers[i]);
                }
            }

            performanceReport[0] = poorPerformers;
            performanceReport[1] = belowExptn;
            performanceReport[2] = minimum;
            performanceReport[3] = starPerformers;

            return performanceReport;
        }


        //divides num of publications in the last 3 year by the expected number (returns 0 if they're a student)
        public static double getPerformance(Researcher researcher)
        {
            List<Publication> publications = PublicationController.loadPublications(researcher);

            int currentYear = DateTime.Today.Year;
            int threeYearsAgo = DateTime.Today.Year - 3;
            int numOfPublications;
            double performance = 0;

            publications = PublicationController.filterByYearRange(publications, threeYearsAgo, currentYear);

            numOfPublications = publications.Count();

            if (researcher.level == 'a')
            {
                performance = numOfPublications / 0.5;
            } 
            else if (researcher.level == 'b')
            {
                performance = numOfPublications;
            }
            else if (researcher.level == 'c')
            {
                performance = numOfPublications / 2;
            }
            else if (researcher.level == 'd')
            {
                performance = numOfPublications / 3.2;
            }
            else if (researcher.level == 'e')
            {
                performance = numOfPublications / 4;
            }

            return performance;
        }


    }
}
