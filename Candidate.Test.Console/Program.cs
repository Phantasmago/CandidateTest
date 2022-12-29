using System.Threading.Tasks;
using Candidate.Test.Data.Http;
using Candidate.Test.Data.Manager;

namespace Candidate.Test.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Analyzer analyzer = new (new WebApiClient());

            UserMessages userMessages = new (analyzer);

            // 1. printing fewest
            await userMessages.PrintFewest(2012);

            // 3. printing sorted json
            await userMessages.PrintSortedJson(2012);

            // 4. printing most repeated word
            await userMessages.PrintMostRepeated(2012);
        }
    }
}
