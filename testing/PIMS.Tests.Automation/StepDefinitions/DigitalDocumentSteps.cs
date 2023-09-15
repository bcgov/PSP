using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class DigitalDocumentSteps
    {
        private readonly DigitalDocuments digitalDocumentsTab;
        private readonly IEnumerable<DocumentFile> documentFiles;
        private int documentsRowStart;
        private int documentsRowsQuantity;
        private List<DigitalDocument> digitalDocumentList;


        public DigitalDocumentSteps(BrowserDriver driver)
        {
            digitalDocumentsTab = new DigitalDocuments(driver.Current);
            documentFiles = UploadFileDocuments();
            digitalDocumentList = new List<DigitalDocument>();
            documentsRowStart = 0;
            documentsRowsQuantity = 0;
        }

        [StepDefinition(@"I create Digital Documents for a ""(.*)"" row number (.*)")]
        public void DocumentTabCreate(string fileType, int rowNumber)
        {
            /* TEST COVERAGE: PSP-4159, PSP-4172, PSP-4339, PSP-4340, PSP-4341 PSP-4342, PSP-4343, PSP-4344, PSP-4345, PSP-4346, PSP-4347, PSP-4348, PSP-4349, PSP-4350, PSP-4351, PSP-4352, PSP-4353, 
             *                PSP-4354, PSP-4355, PSP-4356, PSP-4357, PSP-5208, PSP-5435, PSP-5421, PSP-5440, PSP-5755, PSP-5766, PSP-5929, PSP-6018, PSP-6211 */

            //Access the documents tab
            digitalDocumentsTab.NavigateDocumentsTab();

            //Verify Initial List View
            digitalDocumentsTab.VerifyDocumentsListView(fileType);

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            for (var i = 0; i < digitalDocumentList.Count; i++)
            {
                //Add a New Document
                digitalDocumentsTab.AddNewDocument(fileType);

                //Verify and create a new Document
                digitalDocumentsTab.VerifyDocumentFields(digitalDocumentList[i].DocumentType);
                digitalDocumentsTab.CreateNewDocumentType(digitalDocumentList[i]);

                //Upload one digital document
                Random random = new Random();
                var index = random.Next(0, documentFiles.Count());
                var document = documentFiles.ElementAt(index);

                digitalDocumentsTab.UploadDocument(document.Url);

                //Save digital document
                digitalDocumentsTab.SaveDigitalDocument();

                //Verify Details View Form
                digitalDocumentsTab.ViewLastDocument(i);
                digitalDocumentsTab.VerifyDocumentDetailsCreateViewForm(digitalDocumentList[i]);
                digitalDocumentsTab.CloseDigitalDocumentViewDetails();
            }
        }

        [StepDefinition(@"I edit a Digital Document for a ""(.*)"" from row number (.*)")]
        public void UpdateDigitalDocuments(string fileType, int rowNumber)
        {
            /* TEST COVERAGE: PSP-4030, PSP-4168, PSP-4335, PSP-4336, PSP-4338, PSP-5417, PSP-5418, PSP-5420, PSP-5436, PSP-5437, PSP-5439, PSP-5762, PSP-5765, PSP-5930 */

            //Access the documents tab
            digitalDocumentsTab.NavigateDocumentsTab();

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            //Add new digital document
            digitalDocumentsTab.AddNewDocument(fileType);
            Random random = new Random();
            var index2 = random.Next(0, documentFiles.Count());
            var document2 = documentFiles.ElementAt(index2);

            digitalDocumentsTab.UploadDocument(document2.Url);
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Cancel uploading a new document
            digitalDocumentsTab.CancelDigitalDocument();

            //Edit digital document's details
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.EditDocument();
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Cancel digital document's details
            digitalDocumentsTab.CancelEditDigitalDocument();
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Edit digital document's details
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.EditDocument();
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Save document's changes
            digitalDocumentsTab.SaveEditDigitalDocument();

            //Verify Details View Form
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.VerifyDocumentDetailsUpdateViewForm(digitalDocumentList[0]);

            //Close Digital Documents Details View
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Delete digital document
            digitalDocumentsTab.Delete1stDocument();

            //Filter Documents by Type
            digitalDocumentsTab.FilterByType(digitalDocumentList[0].DocumentType);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Documents by Name
            digitalDocumentsTab.FilterByName("PSP");
            //Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Fouments by Status
            digitalDocumentsTab.FilterByStatus(digitalDocumentList[0].DocumentStatus);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);
        }

        public List<DocumentFile> UploadFileDocuments()
        {
            var digitalImage = new DocumentFile();
            var digitalExcel = new DocumentFile();
            var digitalPDF = new DocumentFile();
            var digitalDocumentList = new List<DocumentFile>();

            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imageFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\TestDocuments\High-def-image.jpg");
            string excelFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\TestDocuments\PSP-4719 ETL Validation.xlsx");
            string pdfFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\TestDocuments\RemoteAccessUserGuide2022.pdf");

            digitalImage.Url = Path.GetFullPath(imageFile);
            digitalExcel.Url = Path.GetFullPath(excelFile);
            digitalPDF.Url = Path.GetFullPath(pdfFile);

            digitalDocumentList.Add(digitalImage);
            digitalDocumentList.Add(digitalExcel);
            digitalDocumentList.Add(digitalPDF);

            return digitalDocumentList;
        }

        private void PopulateDigitalDocumentIndex(int rowNumber)
        {
            DataTable documentsIndexSheet = ExcelDataContext.GetInstance().Sheets["DocumentsIndex"];
            ExcelDataContext.PopulateInCollection(documentsIndexSheet);

            documentsRowStart = int.Parse(ExcelDataContext.ReadData(rowNumber, "DigitalDocumentDetailsRowStart"));
            documentsRowsQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "DigitalDocumentsRowEnd"));

            for (int i = documentsRowStart; i < documentsRowStart + documentsRowsQuantity; i++)
            {
                PopulateDigitalDocumentsDetails(i);
            }
        }

        private void PopulateDigitalDocumentsDetails(int rowNumber)
        {
            DataTable documentDetailsSheet = ExcelDataContext.GetInstance().Sheets["DocumentsDetails"];
            ExcelDataContext.PopulateInCollection(documentDetailsSheet);

            DigitalDocument digitalDocument = new DigitalDocument();

            digitalDocument.DocumentType = ExcelDataContext.ReadData(rowNumber, "DocumentType");
            digitalDocument.DocumentStatus = ExcelDataContext.ReadData(rowNumber, "DocumentStatus");
            digitalDocument.CanadaLandSurvey = ExcelDataContext.ReadData(rowNumber, "CanadaLandSurvey");
            digitalDocument.CivicAddress = ExcelDataContext.ReadData(rowNumber, "CivicAddress");
            digitalDocument.CrownGrant = ExcelDataContext.ReadData(rowNumber, "CrownGrant");
            digitalDocument.Date = ExcelDataContext.ReadData(rowNumber, "Date");
            digitalDocument.DateSigned = ExcelDataContext.ReadData(rowNumber, "DateSigned");
            digitalDocument.DistrictLot = ExcelDataContext.ReadData(rowNumber, "DistrictLot");
            digitalDocument.ElectoralDistrict = ExcelDataContext.ReadData(rowNumber, "ElectoralDistrict");
            digitalDocument.EndDate = ExcelDataContext.ReadData(rowNumber, "EndDate");
            digitalDocument.FieldBook = ExcelDataContext.ReadData(rowNumber, "FieldBook");
            digitalDocument.File = ExcelDataContext.ReadData(rowNumber, "File");
            digitalDocument.GazetteDate = ExcelDataContext.ReadData(rowNumber, "GazetteDate");
            digitalDocument.GazettePage = ExcelDataContext.ReadData(rowNumber, "GazettePage");
            digitalDocument.GazettePublishedDate = ExcelDataContext.ReadData(rowNumber, "GazettePublishedDate");
            digitalDocument.GazetteType = ExcelDataContext.ReadData(rowNumber, "GazetteType");
            digitalDocument.HighwayDistrict = ExcelDataContext.ReadData(rowNumber, "HighwayDistrict");
            digitalDocument.IndianReserveOrNationalPark = ExcelDataContext.ReadData(rowNumber, "IndianReserveOrNationalPark");
            digitalDocument.Jurisdiction = ExcelDataContext.ReadData(rowNumber, "Jurisdiction");
            digitalDocument.LandDistrict = ExcelDataContext.ReadData(rowNumber, "LandDistrict");
            digitalDocument.LegalSurveyPlan = ExcelDataContext.ReadData(rowNumber, "LegalSurveyPlan");
            digitalDocument.LTSAScheduleFiling = ExcelDataContext.ReadData(rowNumber, "LTSAScheduleFiling");
            digitalDocument.MO = ExcelDataContext.ReadData(rowNumber, "MO");
            digitalDocument.MoTIFile = ExcelDataContext.ReadData(rowNumber, "MoTIFile");
            digitalDocument.MoTIPlan = ExcelDataContext.ReadData(rowNumber, "MoTIPlan");
            digitalDocument.OIC = ExcelDataContext.ReadData(rowNumber, "OIC");
            digitalDocument.OICRoute = ExcelDataContext.ReadData(rowNumber, "OICRoute");
            digitalDocument.OICType = ExcelDataContext.ReadData(rowNumber, "OICType");
            digitalDocument.Owner = ExcelDataContext.ReadData(rowNumber, "Owner");
            digitalDocument.PhysicalLocation = ExcelDataContext.ReadData(rowNumber, "PhysicalLocation");
            digitalDocument.PIDNumber = ExcelDataContext.ReadData(rowNumber, "PIDNumber");
            digitalDocument.PINNumber = ExcelDataContext.ReadData(rowNumber, "PINNumber");
            digitalDocument.Plan = ExcelDataContext.ReadData(rowNumber, "Plan");
            digitalDocument.PlanRevision = ExcelDataContext.ReadData(rowNumber, "PlanRevision");
            digitalDocument.PlanType = ExcelDataContext.ReadData(rowNumber, "PlanType");
            digitalDocument.Project = ExcelDataContext.ReadData(rowNumber, "Project");
            digitalDocument.ProjectName = ExcelDataContext.ReadData(rowNumber, "ProjectName");
            digitalDocument.PropertyIdentifier = ExcelDataContext.ReadData(rowNumber, "PropertyIdentifier");
            digitalDocument.PublishedDate = ExcelDataContext.ReadData(rowNumber, "PublishedDate");
            digitalDocument.RelatedGazette = ExcelDataContext.ReadData(rowNumber, "RelatedGazette");
            digitalDocument.RoadName = ExcelDataContext.ReadData(rowNumber, "RoadName");
            digitalDocument.Roll = ExcelDataContext.ReadData(rowNumber, "Roll");
            digitalDocument.Section = ExcelDataContext.ReadData(rowNumber, "Section");
            digitalDocument.ShortDescriptor = ExcelDataContext.ReadData(rowNumber, "ShortDescriptor");
            digitalDocument.StartDate = ExcelDataContext.ReadData(rowNumber, "StartDate");
            digitalDocument.Title = ExcelDataContext.ReadData(rowNumber, "Title");
            digitalDocument.Transfer = ExcelDataContext.ReadData(rowNumber, "Transfer");
            digitalDocument.Year = ExcelDataContext.ReadData(rowNumber, "Year");
            digitalDocument.YearPrivyCouncil = ExcelDataContext.ReadData(rowNumber, "YearPrivyCouncil");

            digitalDocumentList.Add(digitalDocument);
        }
    }
}



