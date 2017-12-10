using DataModels;
using football_data_importer.Model;
using Newtonsoft.Json;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace football_data_importer
{
    class Program
    {
        private static string football_data_org_ApiKey = "998a209446b9405bacbdd0fa1bd111";
        private static Dictionary<string, string> links = new Dictionary<string, string>() {
            { "competitions", "https://api.football-data.org/v1/competitions/?season="  },
            { "fixtures", "http://api.football-data.org/v1/competitions/?season="  },

        };
        private static Dictionary<string, string> mappings = new Dictionary<string, string>()
        {
            {"PL", "Premier League"},
            {"PPL", "Liga NOS"},
            {"BL1", "Bundesliga 1"},
        };

        static void Main(string[] args)
        {
            var database = Database.OpenNamedConnection("main-database");
            List<SportMatch> matchs = database.SportMatch.All().ToList<SportMatch>();

            //foreach (var album in albums) { Console.WriteLine(album.Title); }
            var maxDate = matchs.Max(m => m.Date);
            var minDate = matchs.Min(m => m.Date);

            for (int year = maxDate.Year; year >= minDate.Year; year--)
            {
                System.Threading.Thread.Sleep(2000);

                string url = "https://api.football-data.org/v1/competitions/?season=" + year;
                
                string json = MakeWebRequest(url);
                if (json == null) break;


                List<Competition> competitions = JsonConvert.DeserializeObject<List<Competition>>(json);

                foreach (var comp in competitions.Where(c => mappings.ContainsKey(c.league)))
                {
                    System.Threading.Thread.Sleep(2000);

                    if (comp._links?.fixtures?.href != null)
                    {
                        json = MakeWebRequest(comp._links.fixtures.href);

                        if (json == null) break;

                        Fixtures fixtures = JsonConvert.DeserializeObject<Fixtures>(json);
                        //Console.WriteLine("{0}-{1}-{2}", comp.caption, comp._links.fixtures.href, "OK");

                        foreach (var fix in fixtures.fixtures)
                        {
                            if (fix?.date != null)
                            {
                                mappings.TryGetValue(comp.league, out string dbCompetitionName);

                                if (matchs.Any(m => m.Date.Date == fix.date.Date && m.Competition == dbCompetitionName))
                                {
                                    var match = matchs.Where(mat => IsSameMatch(mat, fix, dbCompetitionName)).FirstOrDefault();
                                    if (match != null)
                                    {
                                        Console.WriteLine("update match");
                                    }
                                }

                                //Console.WriteLine("{0}-{1}-{2}-{3}-{4}", fix.date, fix.result, fix.homeTeamName, fix.awayTeamName, "OK");
                            }
                            else
                            {
                                Console.WriteLine("{0}-{1}", "fix", "NOLINK");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0}-{1}", comp.caption, "NOLINK");
                    }
                }
            }

            Console.WriteLine("goodbye");
            Console.ReadKey();
        }

        static string MakeWebRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Add("X-Auth-Token", "998a209446b9405bacbdd0fa1bd11172");
                request.Headers.Add("X-Response-Control", "full");
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                string json = sr.ReadToEnd();
                response.Close();

                return json;
            }
            catch (Exception ex)
            {
                if (ex is System.Net.WebException)
                {
                    var wex = (System.Net.WebException)ex;
                    if (wex != null)
                    {
                        var exresp = (HttpWebResponse)wex.Response;
                        if (exresp != null)
                        {
                            Console.WriteLine("{0}-{1}", url, HttpStatusCode.Forbidden.ToString());
                        }
                    }
                    
                }
            }

            return null;
        }

        static bool IsSameMatch(SportMatch mat, Fixture fix, string league)
        {
            mappings.TryGetValue(league, out string dbCompetitionName);

            if (mat.Competition != dbCompetitionName || mat.Date.Date != fix.date.Date)
            {
                return false;
            }

            if (NameComparator(mat.HomeTeam, fix.homeTeamName) && NameComparator(mat.AwayTeam, fix.awayTeamName))
            {
                Console.WriteLine("Games are equal:{0}vs{1} || {2}vs{3}", mat.HomeTeam, mat.AwayTeam , fix.homeTeamName, fix.awayTeamName);
                return true;
            }


            return false;
        }

        static bool NameComparator(string a, string b)
        {
            String[] aNames = a.Split(' ');
            String[] bNames = b.Split(' ');

            return aNames.Any(name => b.Contains(name));
        }

    #region OLDMAIN

    //           for (int year = 2016; year >= 2004; year--)
    //            {
    //                System.Threading.Thread.Sleep(2000);

    //                string url = "https://api.football-data.org/v1/competitions/?season=" + year;

    //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    //        request.Method = "GET";
    //                request.Headers.Add("X-Auth-Token", "998a209446b9405bacbdd0fa1bd11172");
    //                request.Headers.Add("X-Response-Control", "full");
    //                WebResponse response = request.GetResponse();
    //        Stream responseStream = response.GetResponseStream();
    //        StreamReader sr = new StreamReader(responseStream);
    //        string json = sr.ReadToEnd();
    //        response.Close();


    //                List<Competition> competitions = JsonConvert.DeserializeObject<List<Competition>>(json);

    //                //HttpWebRequest fixturesRequest = (HttpWebRequest)WebRequest.Create("http://api.football-data.org/v1/competitions/354/fixtures/?matchday=22");
    //                //fixturesRequest.Method = "GET";
    //                //fixturesRequest.Headers.Add("X-Auth-Token", "998a209446b9405bacbdd0fa1bd11172");
    //                //fixturesRequest.Headers.Add("X-Response-Control", "minified");

    //                //WebResponse fixturesResponse = fixturesRequest.GetResponse();
    //                //Stream fixturesResponseStream = fixturesResponse.GetResponseStream();
    //                //StreamReader fixturesResponseStreamReader = new StreamReader(fixturesResponseStream);
    //                //string fjson = fixturesResponseStreamReader.ReadToEnd();
    //                //fixturesResponse.Close();


    //                foreach (var comp in competitions)
    //                {
    //                    System.Threading.Thread.Sleep(2000);

    //                    try
    //                    {
    //                        if (comp._links?.fixtures?.href != null)
    //                        {
    //                            HttpWebRequest fixturesRequest = (HttpWebRequest)WebRequest.Create(comp._links.fixtures.href);
    //        fixturesRequest.Method = "GET";
    //                            fixturesRequest.Headers.Add("X-Auth-Token", "998a209446b9405bacbdd0fa1bd11172");
    //                            fixturesRequest.Headers.Add("X-Response-Control", "full");
    //                            WebResponse fixturesResponse = fixturesRequest.GetResponse();
    //        Stream fixturesResponseStream = fixturesResponse.GetResponseStream();
    //        StreamReader fixturesResponseStreamReader = new StreamReader(fixturesResponseStream);
    //        string fjson = fixturesResponseStreamReader.ReadToEnd();
    //        fixturesResponse.Close();

    //                            Console.WriteLine("{0}-{1}-{2}", comp.caption, comp._links.fixtures.href, "OK");
    //                        }
    //                        else
    //                        {
    //                            Console.WriteLine("{0}-{1}", comp.caption, "NOLINK");
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        if (ex is System.Net.WebException)
    //                        {
    //                            var wex = (System.Net.WebException)ex;
    //var exresp = (HttpWebResponse)wex.Response;

    //                            if (exresp.StatusCode != HttpStatusCode.Forbidden)
    //                            {
    //                                throw;
    //                            }

    //                            Console.WriteLine("{0}-{1}-{2}", comp.caption, comp._links.fixtures.href, HttpStatusCode.Forbidden.ToString());
    //                        }
    //                        else
    //                        {
    //                            Console.WriteLine("{0}-{1}", comp.caption, ex.Message);
    //                        }
    //                    }
    //                }

    //                //var pl2015 = competitions.Where(c => c.league == "PL").FirstOrDefault();
    //                //if (pl2015 != null)
    //                //{
    //                //    try
    //                //    {
    //                //        HttpWebRequest fixturesRequest = (HttpWebRequest)WebRequest.Create(pl2015._links.fixtures.href);
    //                //        fixturesRequest.Method = "GET";
    //                //        fixturesRequest.Headers.Add("X-Auth-Token", "998a209446b9405bacbdd0fa1bd11172");
    //                //        WebResponse fixturesResponse = fixturesRequest.GetResponse();
    //                //        Stream fixturesResponseStream = fixturesResponse.GetResponseStream();
    //                //        StreamReader fixturesResponseStreamReader = new StreamReader(fixturesResponseStream);
    //                //        string fjson = sr.ReadToEnd();
    //                //        fixturesResponse.Close();

    //                //    }
    //                //    catch (Exception ex)
    //                //    {
    //                //        if (ex is System.Net.WebException)
    //                //        {
    //                //            var wex = (System.Net.WebException)ex;
    //                //            var exresp = (HttpWebResponse)wex.Response;

    //                //            if (exresp.StatusCode != HttpStatusCode.Forbidden)
    //                //            {
    //                //                throw;
    //                //            }
    //                //        }
    //                //    }


    //                //}

    //                //Console.WriteLine(json);

    //            }

    //            Console.WriteLine("goodbye");
    //            Console.ReadKey();
    #endregion
}
}
