using System.Collections.Generic;

namespace Candidate.Test.Data.Dtos
{
    #region classDetails  
    ///*****************************************************************
    ///  Machine Name : CANDIDATE-PC
    ///  Author       : new candidate
    ///  Date         : 12/26/2022 10:25:46 AM        
    /// *****************************************************************/
    /// <summary>
    /// </summary>
    #endregion

    public class FdaResult
    {
        public Meta meta { get; set; }
        public List<Result> results { get; set; }
    }
}