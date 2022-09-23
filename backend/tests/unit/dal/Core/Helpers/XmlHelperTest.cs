using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClosedXML.Excel;
using Pims.Core.Helpers;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class XmlHelperTest
    {
        #region Data
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new[] { new { Id = 1, Name = "test1" }, new { Id = 2, Name = "test2" } }, 2 },
                new object[]
                {
                    new[] {
                        new Entity.PimsPropertyType("test1"),
                        new Entity.PimsPropertyType("test2"),
                    },
                    10,
                },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Data))]
        public void ConvertToXLWorkbook(IEnumerable<object> data, int expectedColumns)
        {
            // Arrange
            var datatable = data.ConvertToDataTable("test");

            // Act
            var result = datatable.ConvertToXLWorkbook();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<XLWorkbook>(result);
            Assert.Equal(1, result.Worksheets.Count);
            Assert.Equal(expectedColumns, result.Worksheets.First().Columns().Count());
        }
        #endregion
    }
}
