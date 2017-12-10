using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace football_data_unittest
{
    public class Self
    {
        public string href { get; set; }
    }

    public class Teams
    {
        public string href { get; set; }
    }

    public class Fixtures
    {
        public string href { get; set; }
        public Links _links { get; set; }
        public int count { get; set; }
        public IList<Fixture> fixtures { get; set; }
    }

    public class LeagueTable
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Teams teams { get; set; }
        public Fixtures fixtures { get; set; }
        public LeagueTable leagueTable { get; set; }
    }

    public class Competition
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string caption { get; set; }
        public string league { get; set; }
        public string year { get; set; }

    }


    public class HalfTime
    {
        public int? goalsHomeTeam { get; set; }
        public int? goalsAwayTeam { get; set; }
    }

    public class ExtraTime
    {
        public int? goalsHomeTeam { get; set; }
        public int? goalsAwayTeam { get; set; }
    }

    public class PenaltyShootout
    {
        public int? goalsHomeTeam { get; set; }
        public int? goalsAwayTeam { get; set; }
    }

    public class Result
    {
        public int? goalsHomeTeam { get; set; }
        public int? goalsAwayTeam { get; set; }
    }

    public class Fixture
    {
        public Links _links { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public int matchday { get; set; }
        public string homeTeamName { get; set; }
        public string awayTeamName { get; set; }
        public Result result { get; set; }
        public object odds { get; set; }
    }
}
