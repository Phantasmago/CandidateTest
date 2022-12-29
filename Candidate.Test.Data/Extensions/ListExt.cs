using System.Collections.Generic;
using System.Linq;

namespace Candidate.Test.Data.Extensions
{
    #region classDetails  
    ///*****************************************************************
    ///  Machine Name : CANDIDATE-PC
    ///  Author       : new candidate
    ///  Date         : 12/26/2022 1:55:33 PM        
    /// *****************************************************************/
    /// <summary>
    /// </summary>
    #endregion

    public static class ListExt
    {
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || !list.Any();
        }
    }
}