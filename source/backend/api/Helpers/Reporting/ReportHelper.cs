using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Constants;
using Pims.Core.Helpers;

namespace Pims.Api.Helpers.Reporting
{
    /// <summary>
    /// ReportHelper static class, provides helper functions to generate reports.
    /// </summary>
    public static class ReportHelper
    {
        #region Methods

        /// <summary>
        /// Generates a CSV file for the specified 'items'.
        /// </summary>
        /// <typeparam name="T">The type of the passed items.</typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static ContentResult GenerateCsv<T>(IEnumerable<T> items)
        {
            var csv = items.ConvertToCSV();
            var result = new ContentResult
            {
                Content = csv,
                ContentType = ContentTypes.CONTENTTYPECSV,
            };
            return result;
        }

        /// <summary>
        /// Generates an Excel document for the specified 'items'.
        /// </summary>
        /// <typeparam name="T">The type of the passed items.</typeparam>
        /// <param name="items"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static FileStreamResult GenerateExcel<T>(IEnumerable<T> items, string sheetName)
        {
            var data = items.ConvertToDataTable(sheetName);
            var excel = data.ConvertToXLWorkbook(sheetName);
            var stream = new MemoryStream();
            excel.SaveAs(stream);
            stream.Position = 0;

            return new FileStreamResult(stream, ContentTypes.CONTENTTYPEEXCELX);
        }

        /// <summary>
        /// Generates an Excel document for the specified 'items'.
        /// </summary>
        /// <typeparam name="T">The type of the items in the array of arrays.</typeparam>
        /// <param name="itemArray"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static FileStreamResult GenerateExcel<T>(IEnumerable<IEnumerable<T>> itemArray, string sheetName)
        {
            var stream = new MemoryStream();
            using (var wb = new XLWorkbook())
            {

                foreach (var items in itemArray)
                {
                    var data = items.ConvertToDataTable(sheetName);
                    wb.Worksheets.Add(data, sheetName);
                }

                wb.SaveAs(stream);
                stream.Position = 0;
            }
            return new FileStreamResult(stream, ContentTypes.CONTENTTYPEEXCELX);
        }
        #endregion
    }
}
