using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;

namespace twitter_importer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Authentication
            Auth.SetUserCredentials("3tjzS2x9XLlJ2RWkyE2iXUX0f", "8ilABtkFSZ5iZnsi88tN5Adjp5jZk4ROIDUbX8olVAcB7KDZpV", "67496063-g28hGeRqHPFb9YO4W6O7hwWCBXUPT1bchOjavwZpa", "qqeebhuFmW925DXy5tIGHsZWWQQSVDqmOE8apcMcs2Tgh");

            // Get json directly
            //var tweetsJson = SearchJson.SearchTweets("setubal");

            // Get json from ITweet objects
            var tweets = Search.SearchTweets("beira mar vs feirense");
            // JSON Convert from Newtonsoft available with Tweetinvi
            var json = JsonConvert.SerializeObject(tweets.Select(x => x.TweetDTO));
            var tweetDTOsFromJson = JsonConvert.DeserializeObject<ITweetDTO[]>(json, JsonPropertiesConverterRepository.Converters);
            var tweetsFromJson = Tweet.GenerateTweetsFromDTO(tweetDTOsFromJson);





            /*
             * Setubal	Leixoes	2009-10-25 00:00:00.000
             */

            Console.ReadLine();
        }
    }
}
