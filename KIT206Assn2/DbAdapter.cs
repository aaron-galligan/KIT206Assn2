using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; //for generating a MessageBox upon encountering an error
using MySql.Data.MySqlClient;
using MySql.Data.Types;


namespace Assignemt_2
{
    class DbAdaptor
    {

        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";
        private static MySqlConnection conn = null;



        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }


        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0}; Data Source={1}; User Id={2}; Password={3}", db, server, user, pass);
                //string connectionString = String.Format("Data Source={1}; Database={0}; User Id={2}; Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }


        public static List<Researcher> fetchBasicResearcherDetails()
        {
            List<Researcher> researchers = new List<Researcher>();
            
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                //When referenceing                         0   1     2           3            4
                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title from researcher", conn);
                rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    String Type = rdr.GetString(1);

                    if (Type == "Staff")
                    {
                        researchers.Add(new Staff
                        {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = ParseEnum<Title>(rdr.GetString(4))
                        });
                    }
                    else if (Type == "Student")
                    {
                        researchers.Add(new Student
                        {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = ParseEnum<Title>(rdr.GetString(4))
                        });
                    }
                    else
                    {
                        Console.WriteLine("Does not have a type");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading in basic researcher details:" + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }

            return researchers;
        }


        public static Researcher fetchFullResearcherDetails(int Id)
        {
            Researcher researcher = new Researcher();
            
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                //When referenceing                         0   1     2           3            4      5     6       7      8      9       10             11     12          13
                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title, unit, campus, email, photo, degree, supervisor_id, level, utas_start, current_start from researcher", conn);

                List<String> Students;
                Students = LoadStdntSupervised(cmd, Id);

                List<Publication> publications;
                String name = rdr.GetString(2) + " " + rdr.GetString(3);
                publications = fetchBasicPublicationDetails(conn, name);

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.GetInt32(0) == Id)
                    {
                        if (rdr.GetString(1) == "Staff")
                        {
                            researcher = new Staff
                            {
                                Id = rdr.GetInt32(0),
                                GivenName = rdr.GetString(2),
                                FamilyName = rdr.GetString(3),
                                Title = ParseEnum<Title>(rdr.GetString(4)),
                                Unit = rdr.GetString(5),
                                Campus = ParseEnum<Campus>(rdr.GetString(6)),
                                Email = rdr.GetString(7),
                                Photo = rdr.GetString(8),
                                level = rdr.GetChar(11),
                                CommenceInstDate = DateTime.Parse(rdr.GetString(12)),
                                CommencePosDate = DateTime.Parse(rdr.GetString(13)),
                                StudentsSupervised = Students,
                                Publications = publications
                            };
                        }
                        else
                        {
                            researcher = new Student
                            {
                                Id = rdr.GetInt32(0),
                                GivenName = rdr.GetString(2),
                                FamilyName = rdr.GetString(3),
                                Title = ParseEnum<Title>(rdr.GetString(4)),
                                Unit = rdr.GetString(5),
                                Campus = ParseEnum<Campus>(rdr.GetString(6)),
                                Email = rdr.GetString(7),
                                Photo = rdr.GetString(8),
                                Degree = rdr.GetString(9),
                                CommenceInstDate = DateTime.Parse(rdr.GetString(12)),
                                CommencePosDate = DateTime.Parse(rdr.GetString(13)),
                                Publications = publications
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading in full researcher details:" + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }



            return researcher;
        }

        //the variable reseacher is the researchers name and must be the first and last name with a space in between
        public static List<Publication> fetchBasicPublicationDetails(MySqlConnection conn, String reseacher)
        {           
            //When referenceing                          0    1      2        3
            MySqlCommand cmd = new MySqlCommand("select doi, title, authors, year from publication", conn);
            MySqlDataReader rdr = null;
            List<Publication> publications = new List<Publication>();



            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if ((rdr.GetString(2)).Contains(reseacher))
                    {
                        publications.Add(new Publication
                        {
                            Doi = rdr.GetString(0),
                            Title = rdr.GetString(1),
                            Authors = rdr.GetString(2),
                            Year = rdr.GetInt32(3)
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error loading publications" + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return publications;
        }


        public static Publication completePublicationDetails(Publication publication)
        {
            MySqlConnection conn = GetConnection();
            
            //When referenceing                          0    1      2        3     4     5        6
            MySqlCommand cmd = new MySqlCommand("select doi, title, authors, year, type, cite_as, available from publication", conn);
            MySqlDataReader rdr = null;

            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (rdr.GetString(0) == publication.Doi)
                    {
                        publication = new Publication
                        {
                            Doi = rdr.GetString(0),
                            Title = rdr.GetString(1),
                            Authors = rdr.GetString(2),
                            Year = rdr.GetInt32(3),
                            OutputType = ParseEnum<OutputType>(rdr.GetString(4)),
                            Citation = rdr.GetString(5),
                            Available = DateTime.Parse(rdr.GetString(6))
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error loading publications" + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return publication;
        }

        

        //----------------------not finished---------------------------
        //fetches a list of the number of publications for a researcher starting with the year they started working
        public static List<int> fetchPublicaionCount(DateTime startDate, DateTime finishDate, int Id)
        {
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;
            String name;
            List<int> PubCount = new List<int>();

            try
            {
                conn.Open();
                //When referenceing                         0   1     2           3            4      5     6       7      8      9       10             11     12          13
                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title, unit, campus, email, photo, degree, supervisor_id, level, utas_start, current_start from researcher", conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                name = rdr.GetString(2) + " " + rdr.GetString(3);
                rdr.Close();
                List<Publication> publications = fetchBasicPublicationDetails(conn, name);
                rdr = cmd.ExecuteReader();




            }
            catch (Exception e)
            {
                Console.WriteLine("error counting publications: " + e);
            }
            finally
            {

            }






            return PubCount;
        }
        

        public static List<String> LoadStdntSupervised(MySqlCommand cmd, int supervisor)
        {
            MySqlDataReader srdr = cmd.ExecuteReader();

            List<String> StudentsSupervised = new List<String>();

            while (srdr.Read())
            {
                if (srdr.GetInt32(10) == supervisor)
                {
                    StudentsSupervised.Add(srdr.GetString(2) + " " + srdr.GetString(2));
                }
            }

            return StudentsSupervised;
        }













        /*
        public static List<object> LoadResearchers()
        {
            List<object> Researchers = new List<object>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {

                conn.Open();
                //When referenceing                         0   1     2           3            4      5     6       7      8      9       10             11     12          13
                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title, unit, campus, email, photo, degree, supervisor_id, level, utas_start, current_start from researcher", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    String Type = rdr.GetString(1);

                    String name = rdr.GetString(2) + " " + rdr.GetString(3);

                    rdr.Close();
                    //List<Publication> publications = LoadPublications(conn, name);
                    rdr = cmd.ExecuteReader();

                    if (Type == "Staff")
                    {
                        int staffId = rdr.GetInt32(0);
                        rdr.Close();
                        List<String> Students = LoadStdntSupervised(cmd, staffId);
                        rdr = cmd.ExecuteReader();

                        Researchers.Add(new Staff
                        {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = ParseEnum<Title>(rdr.GetString(4)),
                            Unit = rdr.GetString(5),
                            Campus = ParseEnum<Campus>(rdr.GetString(6)),
                            Email = rdr.GetString(7),
                            Photo = rdr.GetString(8),
                            level = rdr.GetChar(11),
                            CommenceInstDate = DateTime.Parse(rdr.GetString(12)),
                            CommencePosDate = DateTime.Parse(rdr.GetString(13)),
                            StudentsSupervised = Students,
                            //Publications = publications
                        }); ;


                    }
                    else if (Type == "Student")
                    {


                        Researchers.Add(new Student
                        {
                            Id = rdr.GetInt32(0),
                            GivenName = rdr.GetString(2),
                            FamilyName = rdr.GetString(3),
                            Title = ParseEnum<Title>(rdr.GetString(4)),
                            Unit = rdr.GetString(5),
                            Campus = ParseEnum<Campus>(rdr.GetString(6)),
                            Email = rdr.GetString(7),
                            Photo = rdr.GetString(8),
                            Degree = rdr.GetString(9),
                            CommenceInstDate = DateTime.Parse(rdr.GetString(12)),
                            CommencePosDate = DateTime.Parse(rdr.GetString(13)),
                            //Publications = publications
                        });
                    }
                    else
                    {
                        Console.WriteLine("Does not have a type");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error loading in researches" + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }

            return Researchers;
        }


        public static List<Publication> LoadPublications(MySqlConnection conn, String reseacher)
        {
            //When referenceing                          0    1      2        3     4     5        6
            MySqlCommand cmd = new MySqlCommand("select doi, title, authors, year, type, cite_as, available from publication", conn);
            MySqlDataReader prdr = null;
            List<Publication> publications = new List<Publication>();


            try
            {
                prdr = cmd.ExecuteReader();

                while (prdr.Read())
                {
                    if ((prdr.GetString(2)).Contains(reseacher))
                    {
                        publications.Add(new Publication
                        {
                            Doi = prdr.GetString(0),
                            Title = prdr.GetString(1),
                            Authors = prdr.GetString(2),
                            Year = prdr.GetInt32(3),
                            OutputType = ParseEnum<OutputType>(prdr.GetString(4)),
                            Citation = prdr.GetString(5),
                            Available = DateTime.Parse(prdr.GetString(6))
                        });
                    }
                }
            } 
            catch(Exception e)
            {
                Console.WriteLine("error loading publications" + e);
            }
            finally
            {
                if (prdr != null)
                {
                    prdr.Close();
                }
            }

            return publications;
        }
        */

        
    }
}
