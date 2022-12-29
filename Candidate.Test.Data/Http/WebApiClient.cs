using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Candidate.Test.Data.Dtos;
using Candidate.Test.Data.Interfaces;

namespace Candidate.Test.Data.Http
{
    #region classDetails  
    ///*****************************************************************
    ///  Machine Name : CANDIDATE-PC
    ///  Author       : new candidate
    ///  Date         : 12/26/2022 9:46:53 AM        
    /// *****************************************************************/
    /// <summary>
    /// </summary>
    #endregion

    public class WebApiClient : IWebApiClient
    {
        #region ============================================ MEMBERS ==================================================

        private readonly string _baseAddress = "https://api.fda.gov/food/enforcement.json";

        #endregion

        #region ========================================== CONSTRUCTORS ===============================================

        public WebApiClient(string baseAddress = "")
        {
            _baseAddress = string.IsNullOrWhiteSpace(baseAddress) ? 
                _baseAddress : 
                baseAddress;
        }

        #endregion

        #region ======================================== PUBLIC FUNCTIONS =============================================

        public async Task<T> Get<T>(string apiCall)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(apiCall);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }
            try
            {
                return await response.Content.ReadAsAsync<T>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return default;
            }
        }

        public async Task<List<Result>> GetWithLimit<T>(string apiCall) 
        {
            if (typeof(T) == typeof(FdaResult))
            {
                List<Result> dataList = new();

                var skip = 0;

                var data = await Get<T>(apiCall);

                dataList.AddRange((data as FdaResult).results);

                while ((data as FdaResult).meta.results.total > dataList.Count)
                {
                    skip += (data as FdaResult).meta.results.limit;

                    data = await Get<T>(apiCall + $"&skip={skip}");

                    dataList.AddRange((data as FdaResult).results);
                }
                return dataList;
            }
            return null;
        }

        #endregion
    }
}