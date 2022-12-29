using System.Collections.Generic;
using System.Threading.Tasks;
using Candidate.Test.Data.Dtos;

namespace Candidate.Test.Data.Interfaces
{
    public interface IWebApiClient
    {
        Task<T> Get<T>(string apiCall);
        Task<List<Result>> GetWithLimit<T>(string apiCall);
    }
}