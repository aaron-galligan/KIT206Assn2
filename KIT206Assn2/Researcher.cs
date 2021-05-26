using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignemt_2
{
    public enum Campus { Hobart, Launcestion, CradleCoast };
    public enum Title { Mr, Miss, Mrs, Ms, Mx, Dr }
    public enum CurrentJobTitle { PostDoc, Lecturer, SeniorLecturer, AssociateProfessor, Professor }


    public class Researcher
    {

        public int Id { get; set; }
        public String GivenName { get; set; }
        public String FamilyName { get; set; }
        public Title Title { get; set; }
        public String Unit { get; set; }
        public double Tenure { get; set; }
        public DateTime CommenceInstDate { get; set; }
        public DateTime CommencePosDate { get; set; }

        /*need to work out if we want to store all the publications in the researcher as objects or as a list of names
         -storing them as objects means we will have to load up all the information for all the publications for all researchers on startup
         -or we can just load up the names and dates of their publications (we may need to use a 2d array or list)
         */
        public List<Publication> Publications { get; set; }

        public Campus Campus { get; set; }
        public String Email { get; set; }
        public CurrentJobTitle CurrentJobTitle { get; set; }
        public String Photo { get; set; } //its a url, might be a different way to store it but string will work for now
        public char level { get; set; } // if its a student then the char will be 's'

        public int PublicationsCount()
        {
            return Publications.Count;
        }


    }
}
