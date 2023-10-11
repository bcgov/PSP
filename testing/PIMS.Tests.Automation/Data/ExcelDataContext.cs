using ExcelDataReader;
using System.Data;


namespace PIMS.Tests.Automation.Data
{
    public class ExcelDataContext
    {
        // creating an object of ExcelDataContext
        private static ExcelDataContext instance = new ExcelDataContext();

        //Creating the collection we will use to store data 
        private static List<DataCollection> dataCollection = new List<DataCollection>();

        private ExcelDataContext()
        {
            FileStream stream = File.Open(Path.Combine("Data", "PIMS_Testing_Data.xlsx"), FileMode.Open, FileAccess.Read);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            this.Sheets = result.Tables;

            stream.Close();
            stream.Dispose();
        }

        // accessing to ExcelDataContext singleton
        public static ExcelDataContext GetInstance()
        {
            return instance;
        }

        public static void PopulateInCollection(DataTable ExcelSheetFile)
        {
            //Iterate through the columns and rows 
            for (int row = 1; row <= ExcelSheetFile.Rows.Count; row++)
            {
                for (int col = 0; col < ExcelSheetFile.Columns.Count; col++)
                {
                    DataCollection dtTable = new DataCollection()
                    {
                        RowNumber = row,
                        ColumnName = ExcelSheetFile.Columns[col].ColumnName,
                        ColumnValue = ExcelSheetFile.Rows[row - 1][col].ToString()
                    };

                    //Add detaile per row 
                    dataCollection.Add(dtTable);
                }
            }
        }

        public static string ReadData(int rowNumber, string ColumnName)
        {
            try
            {
                //Retriving Data using LINQ 
                string data = (from colData in dataCollection
                               where colData.ColumnName == ColumnName && colData.RowNumber == rowNumber
                               select colData.ColumnValue).FirstOrDefault();

                return data.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        // the dataset of Excel
        public DataTableCollection Sheets { get; private set; }

        //Class properties that will store the Excel data by Row and Column Name 
        public class DataCollection
        {
            public int RowNumber { get; set; } = 0;
            public string ColumnName { get; set; } = null!;
            public string ColumnValue { get; set; } = null!;
        }
    }
}
