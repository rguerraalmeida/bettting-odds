using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDataImporter
{
    public class Mappings
    {
        public static Dictionary<string,string> Competitions = new Dictionary<string, string>()
        {
            {"E0", "Premier League"},
            {"P1", "Liga NOS"},
            {"D1", "Bundesliga 1"},
        };
    }
}
