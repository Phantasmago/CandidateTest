using System;
using System.Threading.Tasks;

namespace Candidate.Test.Data.Manager
{
    #region classDetails  
    ///*****************************************************************
    ///  Machine Name : CANDIDATE-PC
    ///  Author       : new candidate
    ///  Date         : 12/26/2022 12:33:15 PM        
    /// *****************************************************************/
    /// <summary>
    /// </summary>
    #endregion

    public class UserMessages
    {
        #region ============================================ MEMBERS ==================================================
        
        private readonly Analyzer _analyzer;

        #endregion

        #region ========================================== CONSTRUCTORS ===============================================

        public UserMessages(Analyzer analyzer)
        {
            _analyzer = analyzer;
        }

        #endregion

        #region ======================================== PUBLIC FUNCTIONS =============================================

        public async Task PrintFewest(int year)
        {
            Console.WriteLine($"The fewest date of recalls in {year} is : {await _analyzer.GetFewestInYear(year)}");
        }

        public async Task PrintSortedJson(int year)
        {
            Console.WriteLine($"Json of fewest recalls in {year} is : {await _analyzer.GetFewestInYearSortedJson(year)}");
        }

        public async Task PrintMostRepeated(int year)
        {
            Console.WriteLine($"Most repeated word in {year} is : {await _analyzer.GetFewestInYearSortedJsonByWords(year)}");
        }

        #endregion
    }
}