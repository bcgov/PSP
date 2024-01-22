

using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class DispositionFileSteps
    {
        private readonly LoginSteps loginSteps;
        public readonly DispositionFileDetails dispositionFileDetails;

        private readonly string userName = "TRANPSP1";
        private DispositionFile dispositionFile;

        public DispositionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            dispositionFileDetails = new DispositionFileDetails(driver.Current);
        }

        [StepDefinition(@"I create a new Disposition File from row number (.*)")]
        public void CreateDispositionFile(int rowNumber)
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            PopulateDispositionFile(rowNumber);
            dispositionFileDetails.NavigateToCreateNewDipositionFile();

            //Validate Acquisition File Details Create Form
            dispositionFileDetails.VerifyDispositionFileCreate();

            //Create basic Acquisition File
            dispositionFileDetails.CreateMinimumDispositionFile(dispositionFile);

            //Save Acquisition File
            dispositionFileDetails.SaveDispositionFileDetails();
        }

        private void PopulateDispositionFile(int rowNumber)
        {
            DataTable dispositionSheet = ExcelDataContext.GetInstance().Sheets["DispositionFiles"];
            ExcelDataContext.PopulateInCollection(dispositionSheet);
            dispositionFile = new DispositionFile();

            //Disposition File Status
            dispositionFile.DispositionFileStatus = ExcelDataContext.ReadData(rowNumber, "DispositionFileStatus");

            //Schedule
            dispositionFile.AssignedDate = ExcelDataContext.ReadData(rowNumber, "AssignedDate");
            dispositionFile.DispositionCompletedDate = ExcelDataContext.ReadData(rowNumber, "DispositionCompletedDate");

            //Disposition Details
            dispositionFile.DispositionFileName = ExcelDataContext.ReadData(rowNumber, "DispositionFileName");
            dispositionFile.ReferenceNumber = ExcelDataContext.ReadData(rowNumber, "ReferenceNumber");
            dispositionFile.DispositionStatus = ExcelDataContext.ReadData(rowNumber, "DispositionStatus"); 
            dispositionFile.DispositionType = ExcelDataContext.ReadData(rowNumber, "DispositionType");
            dispositionFile.PhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "PhysicalFileStatus");       
            dispositionFile.DispositionMOTIRegion = ExcelDataContext.ReadData(rowNumber, "DispositionMOTIRegion");

            //Properties Search
            //acquisitionFile.SearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "SearchPropertiesIndex"));
            //if (acquisitionFile.SearchPropertiesIndex > 0)
            //{
            //    DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"];
            //    ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

            //    acquisitionFile.SearchProperties.PID = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "PID");
            //    acquisitionFile.SearchProperties.PIN = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "PIN");
            //    acquisitionFile.SearchProperties.Address = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "Address");
            //    acquisitionFile.SearchProperties.PlanNumber = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "PlanNumber");
            //    acquisitionFile.SearchProperties.LegalDescription = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "LegalDescription");
            //}
        }
    }
}
