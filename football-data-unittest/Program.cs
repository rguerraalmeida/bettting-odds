using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace football_data_unittest
{
    class Program
    {
        private static string football_data_org_ApiKey = "998a209446b9405bacbdd0fa1bd111";
        static void Main(string[] args)
        {
            for (; ; )
            {
                System.Threading.Thread.Sleep(1500);

                var max = 15;
                Random random = new Random();
                var year = Enumerable.Range(2005, max).Select(x => x).ToArray()[random.Next(max)];

                string url = "https://api.football-data.org/v1/competitions/?season=" + year;
                Console.WriteLine("Competitions for:{0}", year);

                string response = MakeWebRequest(url);
                if (response == null)
                {
                    Console.WriteLine("OOOps in competitions for year:{0}", year);
                }
                else
                {
                    List<Competition> competitions = JsonConvert.DeserializeObject<List<Competition>>(response);

                    max = competitions.Count();
                    var competitionID = competitions.ToArray()[random.Next(max)].id;

                    url = string.Format("https://api.football-data.org/v1/competitions/{0}/fixtures/", competitionID);

                    response = MakeWebRequest(url);
                    if (response == null)
                    {
                        Console.WriteLine("OOOps in fixtures for copetition:{0}", competitionID);
                    }
                    else
                    {
                        Fixtures fixturesColletion = JsonConvert.DeserializeObject<Fixtures>(response);

                        max = fixturesColletion.fixtures.Count();
                        var ownHref = fixturesColletion.fixtures.ToArray()[random.Next(max)]._links.self;

                        response = MakeWebRequest(ownHref.href);
                        if (response == null) Console.WriteLine("OOOps in fixture:{0}", ownHref.href);
                    }
                }
            }

               

            Console.ReadKey();
        }


        static string MakeWebRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Add("X-Auth-Token", football_data_org_ApiKey);
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
                else
                {
                    Console.WriteLine("EXCEPTION: {0}", ex.Message);
                }
            }

            return null;
        }
    }
}
