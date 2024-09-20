using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;
using System.Linq.Expressions;
using Xunit;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class DigitalDocumentSteps
    {
        private readonly DigitalDocuments digitalDocumentsTab;
        private readonly SharedPagination sharedPagination;
        private readonly IEnumerable<DocumentFile> documentFiles;
        private int documentsRowStart;
        private int documentsRowsQuantity;
        private List<DigitalDocument> digitalDocumentList;


        public DigitalDocumentSteps(BrowserDriver driver)
        {
            digitalDocumentsTab = new DigitalDocuments(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);
            documentFiles = UploadFileDocuments();
            documentsRowStart = 0;
            documentsRowsQuantity = 0;

            digitalDocumentList = new List<DigitalDocument>();
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

            int uploadTurns = (digitalDocumentList.Count + 10 - 1) / 10;
            int documentIdx = 0;
            string documentURLs = "";
            int documentStartIdx = 0;
            int documentEndIdx = 9;

            //Add new documents
            for (var i = 0; i < uploadTurns; i++)
            {
                //Add a New Document Button
                digitalDocumentsTab.AddNewDocumentButton(fileType);

                //Verify and create a new Document
                digitalDocumentsTab.VerifyInitUploadDocumentForm();

                //Prepare Documents' names string
                for (var j = documentStartIdx; j < documentEndIdx; j++)
                {
                    documentIdx = (documentIdx >= 11) ? 0 : documentIdx;
                    var document = documentFiles.ElementAt(documentIdx);

                    documentURLs = (j + 1 == documentEndIdx) ? documentURLs + document.Url : documentURLs + document.Url + "\n";
                    documentIdx++;
                }

                //Upload several documents per time
                digitalDocumentsTab.UploadDocument(documentURLs);

                //Fill document type and status of uploaded documents
                int documentRoundIdx = 0;
                for (var k = documentStartIdx; k <= documentEndIdx; k++)
                {
                    digitalDocumentsTab.InsertDocumentTypeStatus(digitalDocumentList[k], documentRoundIdx);
                    documentRoundIdx++;
                }
                    
                //Save documents
                digitalDocumentsTab.SaveDigitalDocumentUpload();
                documentStartIdx = documentEndIdx + 1;
                documentEndIdx = (documentEndIdx * (i+2) > digitalDocumentList.Count) ? digitalDocumentList.Count : documentEndIdx * (i+2);
                documentURLs = "";
            }

            //Insert Document Details to previously uploaded documents

            for (var l = 0; l < digitalDocumentList.Count; l++)
            {
                digitalDocumentsTab.ViewUploadedDocument(l);
                digitalDocumentsTab.EditDocument();
                digitalDocumentsTab.VerifyDocumentFields(digitalDocumentList[l].DocumentType);
                digitalDocumentsTab.InsertDocumentTypeDetails(digitalDocumentList[l]);
                digitalDocumentsTab.SaveDigitalDocumentUpdate();
            }

            //Verify Details View Form of previously uploaded documents
            for (var m = 0; m < digitalDocumentList.Count; m++)
            {
                digitalDocumentsTab.ViewUploadedDocument(m);
                digitalDocumentsTab.VerifyDocumentDetailsViewForm(digitalDocumentList[m]);
                digitalDocumentsTab.CloseDigitalDocumentViewDetails();
            }
        }

        [StepDefinition(@"I create Digital Documents for a Property Management row number (.*)")]
        public void DocumentActivityCreate(int rowNumber)
        {
            //Verify Initial List View
            digitalDocumentsTab.VerifyDocumentsListView("Property Management");

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            for (var i = 0; i < digitalDocumentList.Count; i++)
            {
                //Add a New Document
                digitalDocumentsTab.AddNewDocumentButton("Property Management");

                //Verify and create a new Document
                digitalDocumentsTab.VerifyInitUploadDocumentForm();
                //digitalDocumentsTab.VerifyDocumentFields(digitalDocumentList[i].DocumentType);
                //digitalDocumentsTab.InsertDocumentTypeStatus(digitalDocumentList[i]);

                //Upload one digital document
                Random random = new Random();
                var index = random.Next(0, documentFiles.Count());
                var document = documentFiles.ElementAt(index);

                digitalDocumentsTab.UploadDocument(document.Url);

                //Save digital document
                digitalDocumentsTab.SaveDigitalDocumentUpload();

                //Verify Details View Form
                digitalDocumentsTab.ViewUploadedDocument(i);
                digitalDocumentsTab.VerifyDocumentDetailsViewForm(digitalDocumentList[i]);
                digitalDocumentsTab.CloseDigitalDocumentViewDetails();
            }
        }

        [StepDefinition(@"I edit a Digital Document for a ""(.*)"" from row number (.*)")]
        public void UpdateDigitalDocuments(string fileType, int rowNumber)
        {
            /* TEST COVERAGE: PSP-4026, PSP-4027, PSP-4030, PSP-4168, PSP-4335, PSP-4336, PSP-4338, PSP-5417, PSP-5418, PSP-5420, PSP-5436, PSP-5437, PSP-5439, PSP-5459,
             *                PSP-5762, PSP-5765, PSP-5930 */

            //Access the documents tab
            digitalDocumentsTab.NavigateDocumentsTab();

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            //Add new digital document
            digitalDocumentsTab.AddNewDocumentButton(fileType);
            //digitalDocumentsTab.InsertDocumentTypeStatus(digitalDocumentList[0]);

            Random random = new Random();
            var index2 = random.Next(0, documentFiles.Count());
            var document2 = documentFiles.ElementAt(index2);

            digitalDocumentsTab.UploadDocument(document2.Url);
            
            //Cancel uploading a new document
            digitalDocumentsTab.CancelDigitalDocument();

            //Edit digital document's details
            digitalDocumentsTab.NavigateToFirstPageDocumentsTable();
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
            //digitalDocumentsTab.VerifyDocumentDetailsUpdateViewForm(digitalDocumentList[0]);

            //Close Digital Documents Details View
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Verify Pagination Elements
            digitalDocumentsTab.VerifyPaginationElements();

            //Verify Pagination Functionality
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, digitalDocumentsTab.DigitalDocumentsTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, digitalDocumentsTab.DigitalDocumentsTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.True(digitalDocumentsTab.DigitalDocumentsTableResultNumber() <= 20);

            //Verify Column Sorting by Document Type
            digitalDocumentsTab.OrderByDocumentFileType();
            var firstFileTypeDescResult = digitalDocumentsTab.FirstDocumentFileType();

            digitalDocumentsTab.OrderByDocumentFileType();
            var firstFileTypeAscResult = digitalDocumentsTab.FirstDocumentFileType();

            Assert.NotEqual(firstFileTypeDescResult, firstFileTypeAscResult);

            //Verify Column Sorting by Document Type
            digitalDocumentsTab.OrderByDocumentFileName();
            var firstFileNameDescResult = digitalDocumentsTab.FirstDocumentFileName();

            digitalDocumentsTab.OrderByDocumentFileName();
            var firstFileNameAscResult = digitalDocumentsTab.FirstDocumentFileName();

            Assert.NotEqual(firstFileNameDescResult, firstFileNameAscResult);

            //Verify Column Sorting by File Name
            digitalDocumentsTab.OrderByDocumentFileStatus();
            var firstFileStatusDescResult = digitalDocumentsTab.FirstDocumentFileStatus();

            digitalDocumentsTab.OrderByDocumentFileStatus();
            var firstFileStatusAscResult = digitalDocumentsTab.FirstDocumentFileStatus();

            Assert.NotEqual(firstFileStatusDescResult,  firstFileStatusAscResult);
            digitalDocumentsTab.OrderByDocumentFileStatus();

            //Filter Documents by Type
            digitalDocumentsTab.FilterByType(digitalDocumentList[0].DocumentType);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Documents by Name
            digitalDocumentsTab.FilterByName("PSP");
            //Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Documents by Status
            digitalDocumentsTab.FilterByStatus(digitalDocumentList[0].DocumentStatus);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Delete digital document
            digitalDocumentsTab.Delete1stDocument();
        }

        public List<DocumentFile> UploadFileDocuments()
        {
            var digitalJPG = new DocumentFile();
            var digitalExcel = new DocumentFile();
            var digitalPDF = new DocumentFile();
            var digitalDOCX = new DocumentFile();
            var digitalDOC = new DocumentFile();
            var digitalHTML = new DocumentFile();
            var digitalPNG = new DocumentFile();
            var digitalXLS = new DocumentFile();
            var digitalTXT = new DocumentFile();
            var digitalBMP = new DocumentFile();
            var digitalMSG = new DocumentFile();

            var digitalDocumentList = new List<DocumentFile>();

            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jpg = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\High-def-image.jpg");
            string xlsx = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\PSP-4719 ETL Validation.xlsx");
            string pdf = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\RemoteAccessUserGuide2022.pdf");
            string docx = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\H120 Template.docx");
            string doc = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\H179P Template.doc");
            string html = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\html test page.html");
            string png = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\PSP-6438 - Evidence.png");
            string xls = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\Takes Logic.xls");
            string txt = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\Testing file docs.txt");
            string bmp = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\PSP-8044.bmp");
            string msg = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\testing_message.msg");

            digitalJPG.Url = Path.GetFullPath(jpg);
            digitalExcel.Url = Path.GetFullPath(xlsx);
            digitalPDF.Url = Path.GetFullPath(pdf);
            digitalDOCX.Url = Path.GetFullPath(docx);
            digitalDOC.Url = Path.GetFullPath(doc);
            digitalHTML.Url = Path.GetFullPath(html);
            digitalPNG.Url = Path.GetFullPath(png);
            digitalXLS.Url = Path.GetFullPath(xls);
            digitalTXT.Url = Path.GetFullPath(txt);
            digitalBMP.Url = Path.GetFullPath(bmp);
            digitalMSG.Url = Path.GetFullPath(msg);
           

            digitalDocumentList.Add(digitalJPG);
            digitalDocumentList.Add(digitalExcel);
            digitalDocumentList.Add(digitalPDF);
            digitalDocumentList.Add(digitalDOCX);
            digitalDocumentList.Add(digitalDOC);
            digitalDocumentList.Add(digitalHTML);
            digitalDocumentList.Add(digitalPNG);
            digitalDocumentList.Add(digitalXLS);
            digitalDocumentList.Add(digitalTXT);
            digitalDocumentList.Add(digitalBMP);
            digitalDocumentList.Add(digitalMSG);

            return digitalDocumentList;
        }

        private void PopulateDigitalDocumentIndex(int rowNumber)
        {
            DataTable documentsIndexSheet = ExcelDataContext.GetInstance().Sheets["DocumentsIndex"]!;
            ExcelDataContext.PopulateInCollection(documentsIndexSheet);

            digitalDocumentList = new List<DigitalDocument>();

            documentsRowStart = int.Parse(ExcelDataContext.ReadData(rowNumber, "DigitalDocumentDetailsRowStart"));
            documentsRowsQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "DigitalDocumentsRowEnd"));

            for (int i = documentsRowStart; i < documentsRowStart + documentsRowsQuantity; i++)
            {
                PopulateDigitalDocumentsDetails(i);
            }
        }

        private void PopulateDigitalDocumentsDetails(int rowNumber)
        {
            DataTable documentDetailsSheet = ExcelDataContext.GetInstance().Sheets["DocumentsDetails"]!;
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



