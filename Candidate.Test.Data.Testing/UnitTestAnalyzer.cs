using System.Collections.Generic;
using System.Threading.Tasks;
using Candidate.Test.Data.Dtos;
using Candidate.Test.Data.Interfaces;
using Candidate.Test.Data.Manager;
using Moq;
using Xunit;

namespace Candidate.Test.Data.Testing
{
    public class UnitTestAnalyzer
    {
        #region ============================================ MEMBERS ==================================================

        private readonly Mock<IWebApiClient> _mockWebApiClient;
        private readonly Analyzer _analyzer;

        #endregion

        #region ========================================== CONSTRUCTORS ===============================================

        public UnitTestAnalyzer()
        {
            _mockWebApiClient = new Mock<IWebApiClient>();

            _analyzer = new(_mockWebApiClient.Object);
        }

        #endregion

        #region ======================================== PUBLIC FUNCTIONS =============================================

        [Fact]
        public async Task TestFewestInYearEmptyResult_ReturnsNull()
        {
            _mockWebApiClient.Setup(t => 
                    t.GetWithLimit<FdaResult>(It.IsAny<string>()))
                        .ReturnsAsync(new List<Result>());

            var res = await _analyzer.GetFewestInYear(2020);

            Assert.Null(res);
        }

        [Fact]
        public async Task TestFewestInYearArrayResult_ReturnsValue()
        {
            _mockWebApiClient.Setup(t =>
                    t.GetWithLimit<FdaResult>(It.IsAny<string>()))
                        .ReturnsAsync(new List<Result>{ 
                            new () { country = "Israel", report_date = "yesterday"},
                            new () { country = "Spain", report_date = "Godzilla"},
                            new () { country = "Brazil", report_date = "messi"}
                        });

            var res = await _analyzer.GetFewestInYear(2020);

            Assert.NotNull(res);
        }

        #endregion
    }
}
