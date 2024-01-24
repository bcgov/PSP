
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
        private readonly DispositionChecklist checklist;

        private readonly string userName = "TRANPSP1";
        private DispositionFile dispositionFile;

        public DispositionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            dispositionFileDetails = new DispositionFileDetails(driver.Current);
            checklist = new DispositionChecklist(driver.Current);
        }

        [StepDefinition(@"I create a new Disposition File from row number (.*)")]
        public void CreateDispositionFile(int rowNumber)
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Disposition File
            PopulateDispositionFile(rowNumber);
            dispositionFileDetails.NavigateToCreateNewDipositionFile();

            //Validate Disposition File Details Create Form
            dispositionFileDetails.VerifyDispositionFileCreate();

            //Create basic Disposition File
            dispositionFileDetails.CreateMinimumDispositionFile(dispositionFile);

            //Save Disposition File
            dispositionFileDetails.SaveDispositionFileDetails();
        }

        [StepDefinition(@"I insert Checklist information to an Disposition File")]
        public void CreateChecklist()
        {
            /* TEST COVERAGE: PSP-7537 ,PSP-7538,PSP-7539 */

            //Navigate to Checklist Tab
            checklist.NavigateChecklistTab();

            //Verify View Checklist form
            checklist.VerifyChecklistInitViewForm();

            //Edit Checklist button
            checklist.EditChecklistButton();

            //Verify Edit Checklist form
            checklist.VerifyChecklistEditForm();

            //Update Checklist Form

            checklist.UpdateChecklist(dispositionFile.DispositionFileChecklist);

            //Save changes
            checklist.SaveDispositionFileChecklist();
        }

        [StepDefinition(@"Disposition File's Checklist has been saved successfully")]
        public void VerifyChecklistChanges()
        {
            /* TEST COVERAGE: PSP-7538 */

            //Verify Checklist Content after update
            checklist.VerifyChecklistViewForm(dispositionFile.DispositionFileChecklist);
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

            //Disposition DetailsDisposition
            dispositionFile.DispositionFileName = ExcelDataContext.ReadData(rowNumber, "DispositionFileName");
            dispositionFile.ReferenceNumber = ExcelDataContext.ReadData(rowNumber, "ReferenceNumber");
            dispositionFile.DispositionStatus = ExcelDataContext.ReadData(rowNumber, "DispositionStatus");
            dispositionFile.DispositionType = ExcelDataContext.ReadData(rowNumber, "DispositionType");
            dispositionFile.PhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "PhysicalFileStatus");
            dispositionFile.DispositionMOTIRegion = ExcelDataContext.ReadData(rowNumber, "DispositionMOTIRegion");

            //Properties Search
            //dispositionFile.SearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "SearchPropertiesIndex"));
            //if (dispositionFile.SearchPropertiesIndex > 0)
            //{
            //    DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"];
            //    ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

            //    dispositionFile.SearchProperties.PID = ExcelDataContext.ReadData(dispositionFile.SearchPropertiesIndex, "PID");
            //    dispositionFile.SearchProperties.PIN = ExcelDataContext.ReadData(dispositionFile.SearchPropertiesIndex, "PIN");
            //    dispositionFile.SearchProperties.Address = ExcelDataContext.ReadData(dispositionFile.SearchPropertiesIndex, "Address");
            //    dispositionFile.SearchProperties.PlanNumber = ExcelDataContext.ReadData(dispositionFile.SearchPropertiesIndex, "PlanNumber");
            //    dispositionFile.SearchProperties.LegalDescription = ExcelDataContext.ReadData(dispositionFile.SearchPropertiesIndex, "LegalDescription");
            //}

            //Disposition File Checklist

            dispositionFile.DispositionFileChecklistIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionFileChecklistIndex"));
            if (dispositionFile.DispositionFileChecklistIndex > 0)
            {
                DataTable dispositionFileChecklistSheet = ExcelDataContext.GetInstance().Sheets["DispositionChecklist"];
                ExcelDataContext.PopulateInCollection(dispositionFileChecklistSheet);

                dispositionFile.DispositionFileChecklist.FileInitiationSelect1 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "FileInitiationSelect1");
                dispositionFile.DispositionFileChecklist.FileInitiationSelect2 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "FileInitiationSelect2");
                dispositionFile.DispositionFileChecklist.FileInitiationSelect3 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "FileInitiationSelect3");
                dispositionFile.DispositionFileChecklist.FileInitiationSelect4 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "FileInitiationSelect4");
                dispositionFile.DispositionFileChecklist.FileInitiationSelect5 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "FileInitiationSelect5");

                dispositionFile.DispositionFileChecklist.DispositionPreparationSelect1 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DispositionPreparationSelect1");
                dispositionFile.DispositionFileChecklist.DispositionPreparationSelect2 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DispositionPreparationSelect2");
                dispositionFile.DispositionFileChecklist.DispositionPreparationSelect3 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DispositionPreparationSelect3");
                dispositionFile.DispositionFileChecklist.DispositionPreparationSelect4 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DispositionPreparationSelect4");


                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect1 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect1");
                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect2 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect2");
                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect3 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect3");
                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect4 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect4");
                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect5 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect5");
                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect6 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect6");
                dispositionFile.DispositionFileChecklist.ReferralsAndConsultationsSelect7 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "ReferralsAndConsultationsSelect7");

                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect1 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect1");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect2 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect2");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect3 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect3");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect4 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect4");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect5 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect5");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect6 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect6");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect7 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect7");
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect8 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect8");

                dispositionFile.DispositionFileChecklist.SaleInformationSelect1 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect1");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect2 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect2");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect3 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect3");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect4 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect4");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect5 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect5");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect6 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect6");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect7 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect7");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect8 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect8");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect9 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect9");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect10 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect10");
                dispositionFile.DispositionFileChecklist.SaleInformationSelect11 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "SaleInformationSelect11");
            }

        }
    }
}
