using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;


namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class DigitalDocumentSteps
    {
        private readonly DigitalDocuments digitalDocumentsTab;
        private readonly ManagementDetails managementFilesDetails;
        private readonly IEnumerable<DocumentFile> documentFiles;
        private int documentsRowStart;
        private int documentsRowsQuantity;
        private List<DigitalDocument> digitalDocumentList;


        public DigitalDocumentSteps(IWebDriver driver)
        {
            digitalDocumentsTab = new DigitalDocuments(driver);
            managementFilesDetails = new ManagementDetails(driver);
            documentFiles = UploadFileDocuments();
            documentsRowStart = 0;
            documentsRowsQuantity = 0;

            digitalDocumentList = new List<DigitalDocument>();
        }

        [StepDefinition(@"I create Digital Documents for a ""(.*)"" from row number (.*)")]
        public void DocumentTabCreate(string fileType, int rowNumber)
        {
            //Access the documents tab
            if(fileType != "Management Activity")
                digitalDocumentsTab.NavigateDocumentsTab();

            //Verify Initial List View
            if (fileType == "Property")
                digitalDocumentsTab.VerifyPropertyDocumentsListView();
            else if(fileType == "Management Activity")
                digitalDocumentsTab.VerifyActivityDocumentsListView();
            else if (fileType == "Management File")
                digitalDocumentsTab.VerifyManagementFilesDocumentsListView();
            else
                digitalDocumentsTab.VerifyFileDocumentsListView();

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            int uploadTurns = (digitalDocumentList.Count + 10 - 1) / 10;
            int documentIdx = 0;
            string documentURLs = "";
            int documentStartIdx = 0;
            int documentEndIdx = (digitalDocumentList.Count > 9) ? 9 : digitalDocumentList.Count - 1;

            //Add new documents
            for (var i = 0; i < uploadTurns; i++)
            {
                //Add a New Document Button
                digitalDocumentsTab.AddNewDocumentButton();

                //Verify and create a new Document
                digitalDocumentsTab.VerifyInitUploadDocumentForm();

                //Prepare Documents' names string
                for (var j = documentStartIdx; j <= documentEndIdx; j++)
                {
                    documentIdx = (documentIdx >= 10) ? 0 : documentIdx;
                    var document = documentFiles.ElementAt(documentIdx);

                    documentURLs = (j + 1 > documentEndIdx) ? documentURLs + document.Url : documentURLs + document.Url + "\n";
                    documentIdx++;
                }

                //Upload several documents per time
                digitalDocumentsTab.UploadDocument(documentURLs);

                //Fill document type, status and details of uploaded documents
                int documentRoundIdx = 0;
                for (var k = documentStartIdx; k <= documentEndIdx; k++)
                {
                    digitalDocumentsTab.InsertDocumentTypeStatus(digitalDocumentList[k], documentRoundIdx);
                    documentRoundIdx++;
                }
                    
                //Save documents
                digitalDocumentsTab.SaveDigitalDocumentUpload();
                documentStartIdx = documentEndIdx + 1;
                documentEndIdx = (documentEndIdx * (i+2) > digitalDocumentList.Count -1) ? digitalDocumentList.Count -1 : documentEndIdx * (i+2);
                documentURLs = "";
            }

            //Order Documents by Document Type
            digitalDocumentsTab.WaitUploadDocument();
            digitalDocumentsTab.OrderByDocumentFileType();

            //Insert Document Details to previously uploaded documents
            for (var l = 0; l < digitalDocumentList.Count; l++)
            {
                if ((fileType == "Lease" || fileType == "Property") && l > 0)
                    digitalDocumentsTab.OrderByDocumentFileType();

                digitalDocumentsTab.ViewUploadedDocument(l);
                digitalDocumentsTab.EditDocumentButton();
                digitalDocumentsTab.VerifyDocumentFields(digitalDocumentList[l].DocumentType);
                digitalDocumentsTab.InsertDocumentTypeDetails(digitalDocumentList[l]);
                digitalDocumentsTab.SaveDigitalDocumentUpdate();
            }

            //Go back to 1st page
            digitalDocumentsTab.NavigateToFirstPageDocumentsTable();

            //Verify Details View Form of previously uploaded documents
            if (fileType == "Lease" || fileType == "Property")
                digitalDocumentsTab.OrderByDocumentFileType();

            for (var m = 0; m < digitalDocumentList.Count; m++)
            {
                digitalDocumentsTab.ViewUploadedDocument(m);
                digitalDocumentsTab.VerifyDocumentDetailsViewForm(digitalDocumentList[m]);
                digitalDocumentsTab.CloseDigitalDocumentViewDetails();
            }

            if (fileType == "Lease")
            {
                digitalDocumentsTab.OrderByDocumentFileType();
                digitalDocumentsTab.OrderByDocumentFileType();
            }
                
        }

        [StepDefinition(@"I create Digital Documents for a Property Management row number (.*)")]
        public void DocumentActivityCreate(int rowNumber)
        {
            //Verify Initial List View
            digitalDocumentsTab.VerifyFileDocumentsListView();

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            int uploadTurns = (digitalDocumentList.Count + 10 - 1) / 10;
            int documentIdx = 0;
            string documentURLs = "";
            int documentStartIdx = 0;
            int documentEndIdx = (digitalDocumentList.Count > 9) ? 9 : digitalDocumentList.Count - 1;

            //Add new documents
            for (var i = 0; i < uploadTurns; i++)
            {
                //Add a New Document Button
                digitalDocumentsTab.AddNewDocumentButton();

                //Verify and create a new Document
                digitalDocumentsTab.VerifyInitUploadDocumentForm();

                //Prepare Documents' names string
                for (var j = documentStartIdx; j <= documentEndIdx; j++)
                {
                    documentIdx = (documentIdx >= 11) ? 0 : documentIdx;
                    var document = documentFiles.ElementAt(documentIdx);

                    documentURLs = (j + 1 > documentEndIdx) ? documentURLs + document.Url : documentURLs + document.Url + "\n";
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
                documentEndIdx = (documentEndIdx * (i+2) > digitalDocumentList.Count -1) ? digitalDocumentList.Count -1 : documentEndIdx * (i+2);
                documentURLs = "";
            }

            //Order Documents by Document Type
            digitalDocumentsTab.OrderByDocumentFileType();
            digitalDocumentsTab.WaitUploadDocument();

            //Insert Document Details to previously uploaded documents
            for (var l = 0; l < digitalDocumentList.Count; l++)
            {
                digitalDocumentsTab.ViewUploadedDocument(l);
                digitalDocumentsTab.EditDocumentButton();
                digitalDocumentsTab.VerifyDocumentFields(digitalDocumentList[l].DocumentType);
                digitalDocumentsTab.InsertDocumentTypeDetails(digitalDocumentList[l]);
                digitalDocumentsTab.SaveDigitalDocumentUpdate();
            }

            //Go back to 1st page
            digitalDocumentsTab.NavigateToFirstPageDocumentsTable();

            //Verify Details View Form of previously uploaded documents
            for (var m = 0; m < digitalDocumentList.Count; m++)
            {
                digitalDocumentsTab.ViewUploadedDocument(m);
                digitalDocumentsTab.VerifyDocumentDetailsViewForm(digitalDocumentList[m]);
                digitalDocumentsTab.CloseDigitalDocumentViewDetails();
            }

            //Set order back to default
            digitalDocumentsTab.OrderByDocumentFileType();
            digitalDocumentsTab.OrderByDocumentFileType();
        }

        [StepDefinition(@"I edit a Digital Document for a ""(.*)"" from row number (.*)")]
        public void UpdateDigitalDocuments(string fileType, int rowNumber)
        {
            //Access the documents tab
            digitalDocumentsTab.NavigateDocumentsTab();

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            //Add new digital document
            digitalDocumentsTab.AddNewDocumentButton();

            Random random = new();
            var index2 = random.Next(0, documentFiles.Count());
            var document2 = documentFiles.ElementAt(index2);

            digitalDocumentsTab.UploadDocument(document2.Url);

            //Cancel uploading a new document
            digitalDocumentsTab.CancelDigitalDocument();

            //Edit digital document's details
            digitalDocumentsTab.NavigateToFirstPageDocumentsTable();

            //Pick 1st available digital document
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.EditDocumentButton();
            digitalDocumentsTab.UpdateDocumentName(digitalDocumentList[0].DocumentName);
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Cancel digital document's details
            digitalDocumentsTab.CancelEditDigitalDocument();
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Edit digital document's details
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.EditDocumentButton();
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Save document's changes
            digitalDocumentsTab.SaveEditDigitalDocument();

            //Verify Details View Form
            digitalDocumentsTab.FilterByType(digitalDocumentList[0].DocumentType);
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.VerifyDocumentDetailsViewForm(digitalDocumentList[0]);

            //Close Digital Documents Details View
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Verify Pagination Elements
            digitalDocumentsTab.ResetFilters();
            digitalDocumentsTab.VerifyPaginationElements();

            //Verify Pagination Functionality
            digitalDocumentsTab.ChoosePaginationOption(5);
            Assert.Equal(5, digitalDocumentsTab.DigitalDocumentsTableResultNumber());

            digitalDocumentsTab.ChoosePaginationOption(10);
            Assert.True(digitalDocumentsTab.DigitalDocumentsTableResultNumber() <= 10);

            digitalDocumentsTab.ChoosePaginationOption(20);
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
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Documents by Status
            digitalDocumentsTab.FilterByStatus(digitalDocumentList[0].DocumentStatus);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Delete digital document
            digitalDocumentsTab.Delete1stDocument();
        }

        [StepDefinition(@"I edit a Digital Document for a property from row number (.*)")]
        public void UpdatePropertyDigitalDocuments(int rowNumber)
        {
            //Access the documents tab
            digitalDocumentsTab.NavigatePropertyDocumentsTab();

            //Getting Digital Document Details
            PopulateDigitalDocumentIndex(rowNumber);

            //Add new digital document
            digitalDocumentsTab.AddNewDocumentButton();

            Random random = new();
            var index2 = random.Next(0, documentFiles.Count());
            var document2 = documentFiles.ElementAt(index2);

            digitalDocumentsTab.UploadDocument(document2.Url);

            //Cancel uploading a new document
            digitalDocumentsTab.CancelDigitalDocument();

            //Edit digital document's details
            digitalDocumentsTab.NavigateToFirstPageDocumentsTable();
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.EditDocumentButton();
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Cancel digital document's details
            digitalDocumentsTab.CancelEditDigitalDocument();
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Edit digital document's details
            digitalDocumentsTab.View1stDocument();
            digitalDocumentsTab.EditDocumentButton();
            digitalDocumentsTab.UpdateNewDocumentType(digitalDocumentList[0]);

            //Save document's changes
            digitalDocumentsTab.SaveEditDigitalDocument();

            //Verify Details View Form
            digitalDocumentsTab.View1stDocument();

            //Close Digital Documents Details View
            digitalDocumentsTab.CloseDigitalDocumentViewDetails();

            //Verify Pagination Elements
            digitalDocumentsTab.VerifyPaginationElements();

            //Verify Pagination Functionality
            digitalDocumentsTab.ChoosePaginationOption(5);
            Assert.Equal(5, digitalDocumentsTab.DigitalDocumentsTableResultNumber());

            digitalDocumentsTab.ChoosePaginationOption(10);
            Assert.True(digitalDocumentsTab.DigitalDocumentsTableResultNumber() <= 10);

            digitalDocumentsTab.ChoosePaginationOption(20);
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

            Assert.NotEqual(firstFileStatusDescResult, firstFileStatusAscResult);
            digitalDocumentsTab.OrderByDocumentFileStatus();

            //Filter Documents by Type
            digitalDocumentsTab.FilterByType(digitalDocumentList[0].DocumentType);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Documents by Name
            digitalDocumentsTab.FilterByName("PSP");
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Filter Documents by Status
            digitalDocumentsTab.FilterByStatus(digitalDocumentList[0].DocumentStatus);
            Assert.True(digitalDocumentsTab.TotalSearchDocuments() > 0);

            //Delete digital document
            digitalDocumentsTab.Delete1stDocument();
        }

        [StepDefinition(@"I checked related file documents on properties documents")]
        public void VerifyDocumentsListonProperties()
        {
            //Navigate to the Property File Documents Tab
            digitalDocumentsTab.NavigateDocumentsTab();

            //Verify on related documents list view the previously attached documents
            for (var m = 0; m < digitalDocumentList.Count; m++)
                digitalDocumentsTab.VerifyAdhocDocumentsList(digitalDocumentList[m], m);

            //Navigate back the Management File section
            managementFilesDetails.NavigateToManagementFileSection();
        }

        [StepDefinition(@"The related documents from ""(.*)"" appeared as expected")]
        public void VerifyRelatedDocumentsList(string fileType)
        {
            //Access the documents tab
            if (fileType == "Management File")
                digitalDocumentsTab.NavigateDocumentsTab();

            //Organize documents by type
            digitalDocumentsTab.OrderByActivityRelatedDocumentsType();

            //Verify on related documents list view the previously attached documents
            if (fileType == "Management File")
            {
                for (var m = 0; m < digitalDocumentList.Count; m++)
                    digitalDocumentsTab.VerifyAdhocDocumentsList(digitalDocumentList[m], m);
            }
        }

        public List<DocumentFile> UploadFileDocuments()
        {
            var digitalJPG = new DocumentFile();
            var digitalExcel = new DocumentFile();
            var digitalPDF = new DocumentFile();
            var digitalDOCX = new DocumentFile();
            var digitalDOC = new DocumentFile();
            //var digitalHTML = new DocumentFile();
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
            //string html = System.IO.Path.Combine(currentDirectory, @"..\..\..\TestDocuments\html test page.html");
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
            //digitalHTML.Url = Path.GetFullPath(html);
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
            //digitalDocumentList.Add(digitalHTML);
            digitalDocumentList.Add(digitalPNG);
            digitalDocumentList.Add(digitalXLS);
            digitalDocumentList.Add(digitalTXT);
            digitalDocumentList.Add(digitalBMP);
            digitalDocumentList.Add(digitalMSG);

            return digitalDocumentList;
        }

        private void PopulateDigitalDocumentIndex(int rowNumber)
        {
            System.Data.DataTable documentsIndexSheet = ExcelDataContext.GetInstance().Sheets["DocumentsIndex"]!;
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
            System.Data.DataTable documentDetailsSheet = ExcelDataContext.GetInstance().Sheets["DocumentsDetails"]!;
            ExcelDataContext.PopulateInCollection(documentDetailsSheet);

            DigitalDocument digitalDocument = new();

            digitalDocument.DocumentType = ExcelDataContext.ReadData(rowNumber, "DocumentType");
            digitalDocument.DocumentStatus = ExcelDataContext.ReadData(rowNumber, "DocumentStatus");
            digitalDocument.DocumentName = ExcelDataContext.ReadData(rowNumber, "DocumentName");
            digitalDocument.ApplicationNumber = ExcelDataContext.ReadData(rowNumber, "ApplicationNumber");
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
            digitalDocument.ReferenceAgencyDocumentNbr = ExcelDataContext.ReadData(rowNumber, "ReferenceAgencyDocumentNbr");
            digitalDocument.ReferenceAgencyLandsFileNbr = ExcelDataContext.ReadData(rowNumber, "ReferenceAgencyLandsFileNbr");
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



