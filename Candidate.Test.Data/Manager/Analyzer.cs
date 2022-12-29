using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candidate.Test.Data.Dtos;
using Candidate.Test.Data.Extensions;
using Candidate.Test.Data.Interfaces;
using Newtonsoft.Json;

namespace Candidate.Test.Data.Manager
{
    #region classDetails  
    ///*****************************************************************
    ///  Machine Name : CANDIDATE-PC
    ///  Author       : new candidate
    ///  Date         : 12/26/2022 10:56:31 AM        
    /// *****************************************************************/
    /// <summary>
    /// </summary>
    #endregion

    public class Analyzer
    {
        #region ============================================ MEMBERS ==================================================
        
        private readonly IWebApiClient _webApiClient;

        private List<Result> _listCache;

        #endregion

        #region ========================================== CONSTRUCTORS ===============================================

        public Analyzer(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        #endregion

        #region ======================================== PUBLIC FUNCTIONS =============================================

        public async Task<string> GetFewestInYear(int year, int limit = 1000)
        {
            var dicOfYear = await GetResults(year, limit);

            return dicOfYear.OrderByDescending(key => key.Value.Count).LastOrDefault().Key;
        }

        public async Task<string> GetFewestInYearSortedJson(int year, int limit = 1000)
        {
            var dicOfYear = await GetResults(year, limit);

            var lowestList = dicOfYear.
                OrderByDescending(key => key.Value.Count).
                    LastOrDefault().Value.
                        OrderByDescending(t=> t.recall_initiation_date);

            return JsonConvert.SerializeObject(lowestList);
        }

        public async Task<string> GetFewestInYearSortedJsonByWords(int year, int limit = 1000)
        {
            var dicOfYear = await GetResults(year, limit);

            var lowestList = dicOfYear.
                OrderByDescending(key => key.Value.Count).
                    LastOrDefault().Value.
                        OrderByDescending(t => t.recall_initiation_date);

            ConcurrentBag<KeyValuePair<string, int>> resultsBag = new();

            Parallel.ForEach(lowestList,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                t =>
                {
                    resultsBag.Add(OccurrencesAnalyzer(t.reason_for_recall));
                });
            return resultsBag.OrderByDescending(t=> t.Value).First().Key;
        }

        #endregion

        #region ======================================== PRIVATE FUNCTIONS ============================================

        private async Task<Dictionary<string, List<Result>>> GetResults(int year, int limit)
        {
            if (_listCache.IsNullOrEmpty())
            {
                _listCache = await _webApiClient.GetWithLimit<FdaResult>($"?search=report_date:[{year}0101+TO+{year}1231]&limit={limit}");
            }
            Dictionary<string, List<Result>> dicOfYear = new();

            foreach (var result in _listCache)
            {
                if (dicOfYear.ContainsKey(result.report_date))
                {
                    dicOfYear[result.report_date].Add(result);
                }
                else
                {
                    List<Result> resList = new() { result };

                    dicOfYear.Add(result.report_date, resList);
                }
            }
            return dicOfYear;
        }

        private KeyValuePair<string, int> OccurrencesAnalyzer(string stringToAnalyze, int minWordLen = 4)
        {
            Dictionary<string, int> occurrences = new();

            var splitted = stringToAnalyze.Split(" ")
                .Where(t=> t.Length >= minWordLen)
                .ToList();

            foreach (var word in splitted)
            {
                if (occurrences.ContainsKey(word))
                {
                    occurrences[word]++;
                }
                else
                {
                    occurrences.Add(word, 1);
                }
            }
            return occurrences.OrderByDescending(t => t.Value).FirstOrDefault();
        }

        #endregion

    }
}