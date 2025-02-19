using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Pims.Core.Helpers;
using Pims.Core.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class CsvHelperTest
    {
        #region Data
        public static IEnumerable<object[]> DataWithoutDelimiter =>
            new List<object[]>
            {
                new object[] { new[] { new { Id = 1, Name = "test1" }, new { Id = 2, Name = "test2" } }, "Id,Name\r\n1,test1\r\n2,test2\r\n" },
                new object[] { new[] { new { Id = 1, Name = "test,1" }, new { Id = 2, Name = "test,2" } }, "Id,Name\r\n1,\"test,1\"\r\n2,\"test,2\"\r\n" },
                new object[]
                {
                    new[] { new Entity.PimsPropertyType("test1"), new Entity.PimsPropertyType("test2") },
                    "PropertyTypeCode,Description,IsDisabled,DisplayOrder,ConcurrencyControlNumber,DbCreateTimestamp,DbCreateUserid,DbLastUpdateTimestamp,DbLastUpdateUserid,Id\r\ntest1,,False,,0,01/01/0001 00:00:00,,01/01/0001 00:00:00,,test1\r\ntest2,,False,,0,01/01/0001 00:00:00,,01/01/0001 00:00:00,,test2\r\n",
                },
            };

        public static IEnumerable<object[]> DataWithDelimiter =>
            new List<object[]>
            {
                new object[] { new[] { new { Id = 1, Name = "test1" }, new { Id = 2, Name = "test2" } }, ";", "Id;Name\r\n1;test1\r\n2;test2\r\n" },
                new object[] { new[] { new { Id = 1, Name = "test;1" }, new { Id = 2, Name = "test;2" } }, ";", "Id;Name\r\n1;\"test;1\"\r\n2;\"test;2\"\r\n" },
                new object[]
                {
                    new[] { new Entity.PimsPropertyType("test1"), new Entity.PimsPropertyType("test2") },
                    "-",
                    "PropertyTypeCode-Description-IsDisabled-DisplayOrder-ConcurrencyControlNumber-DbCreateTimestamp-DbCreateUserid-DbLastUpdateTimestamp-DbLastUpdateUserid-Id\r\ntest1--False--0-01/01/0001 00:00:00--01/01/0001 00:00:00--test1\r\ntest2--False--0-01/01/0001 00:00:00--01/01/0001 00:00:00--test2\r\n",
                },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(DataWithoutDelimiter))]
        public void ConvertToCSV(IEnumerable<object> data, string expectedResult)
        {
            // Arrange
            // Act
            var result = data.ConvertToCSV();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(DataWithDelimiter))]
        public void ConvertToCSV_WithDelimiter(IEnumerable<object> data, string delimiter, string expectedResult)
        {
            // Arrange
            // Act
            var result = data.ConvertToCSV(delimiter);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConvertToDataTable()
        {
            // Arrange
            var items = new[] { new { Id = 1, Name = "test1" }, new { Id = 2, Name = "test2" } };

            // Act
            var result = items.ConvertToDataTable("test");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal("test", result.TableName);
        }

        [Fact]
        public void ConvertToDataTable_Enums()
        {
            // Arrange
            var items = new[] { new { Id = 1, Name = "test1", Permission = Permissions.AdminProperties }, new { Id = 2, Name = "test2", Permission = Permissions.AdminRoles } };

            // Act
            var result = items.ConvertToDataTable("test");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal(typeof(string), result.Columns[2].DataType);
            Assert.Equal($"{Permissions.AdminProperties}", result.Rows[0][2]);
        }

        [Fact]
        public void ConvertToDataTable_NullableType()
        {
            // Arrange
            var items = new[] { new { Id = (int?)1, Name = "test1" }, new { Id = (int?)null, Name = "test2" } };

            // Act
            var result = items.ConvertToDataTable("test");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal(DBNull.Value, result.Rows[1][0]);
            Assert.True(result.Columns[0].AllowDBNull);
            Assert.True(result.Columns[1].AllowDBNull);
        }
        #endregion
    }
}
