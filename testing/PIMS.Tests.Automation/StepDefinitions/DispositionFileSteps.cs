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
        private readonly DispositionFileDetails dispositionFileDetails;
        private readonly SearchDispositionFiles searchDispositionFiles;
        private readonly SharedFileProperties sharedFileProperties;
        private readonly SearchProperties searchProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly Notes notes;
        private readonly DispositionChecklist checklist;
        private readonly DispositionOfferSale offerSale;
        private readonly SharedPagination sharedPagination;

        private readonly string userName = "TRANPSP1";
        private string dispositionFileCode = "";
        private DispositionFile dispositionFile;

        public DispositionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            dispositionFileDetails = new DispositionFileDetails(driver.Current);
            searchDispositionFiles = new SearchDispositionFiles(driver.Current);
            sharedFileProperties = new SharedFileProperties(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            notes = new Notes(driver.Current);
            checklist = new DispositionChecklist(driver.Current);
            offerSale = new DispositionOfferSale(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);

            dispositionFile = new DispositionFile();
        }

        [StepDefinition(@"I create a new Disposition File from row number (.*)")]
        public void CreateDispositionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-7504, PSP-7507 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Disposition File
            PopulateDispositionFile(rowNumber);
            dispositionFileDetails.NavigateToCreateNewDipositionFile();

            //Validate Disposition File Details Create Form
            dispositionFileDetails.VerifyDispositionFileInitCreate();

            //Create basic Disposition File
            dispositionFileDetails.CreateMinimumDispositionFile(dispositionFile);

            //Save Disposition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Get Disposition File code
            dispositionFileCode = dispositionFileDetails.GetDispositionFileCode();
        }

        [StepDefinition(@"I add additional information to the Disposition File Details")]
        public void AddAdditionalInfoDispositionFile()
        {
            /* TEST COVERAGE:  PSP-7505, PSP-7558 */

            //Enter to Edit mode of Disposition File
            dispositionFileDetails.EditDispositionFileBttn();

            //Verify maximum fields within the form
            dispositionFileDetails.VerifyMaximumFields();

            //Add Additional Optional information to the disposition file
            dispositionFileDetails.AddAdditionalInformation(dispositionFile);

            //Save Disposition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Validate View File Details View Mode
            dispositionFileDetails.VerifyDispositionFileView(dispositionFile);

            //Verify automatic note created when the status is changed
            if (dispositionFile.DispositionFileStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Disposition File", "Active", dispositionFile.DispositionFileStatus);
            }
        }

        [StepDefinition(@"I update the File details from an existing Disposition File from row number (.*)")]
        public void UpdateDispositionFileDetails(int rowNumber)
        {
            /* TEST COVERAGE: PSP-7506, PSP-7507, PSP-7522, PSP-7559 */

            PopulateDispositionFile(rowNumber);

            //Search for an existing Disposition File
            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);
            searchDispositionFiles.SelectFirstOption();

            //Update existing Disposition File
            dispositionFileDetails.EditDispositionFileBttn();
            dispositionFileDetails.UpdateDispositionFile(dispositionFile);

            //Cancel changes
            dispositionFileDetails.CancelDispositionFile();

            //Edit Disposition File
            dispositionFileDetails.EditDispositionFileBttn();
            dispositionFileDetails.UpdateDispositionFile(dispositionFile);

            //Save Disposition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Get Disposition File code
            dispositionFileCode = dispositionFileDetails.GetDispositionFileCode();

            //Validate View File Details View Mode
            dispositionFileDetails.VerifyDispositionFileView(dispositionFile);

            //Verify automatic note created when the status changes
            if (dispositionFile.DispositionFileStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Disposition File", "Hold", dispositionFile.DispositionFileStatus);
            }
        }

        [StepDefinition(@"I add Properties to the Disposition File")]
        public void AddProperties()
        {
            /* TEST COVERAGE:  */

            //Navigate to Properties for Disposition File
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (dispositionFile.DispositionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(dispositionFile.DispositionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by PIN
            if (dispositionFile.DispositionSearchProperties.PIN != "")
            {
                sharedFileProperties.SelectPropertyByPIN(dispositionFile.DispositionSearchProperties.PIN);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Plan
            if (dispositionFile.DispositionSearchProperties.PlanNumber != "")
            {
                sharedFileProperties.SelectPropertyByPlan(dispositionFile.DispositionSearchProperties.PlanNumber);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Address
            if (dispositionFile.DispositionSearchProperties.Address != "")
            {
                sharedFileProperties.SelectPropertyByAddress(dispositionFile.DispositionSearchProperties.Address);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Legal Description
            //if (dispositionFile.DispositionSearchProperties.LegalDescription != "")
            //{
            //    sharedSearchProperties.SelectPropertyByLegalDescription(acquisitionFile.SearchProperties.LegalDescription);
            //    sharedSearchProperties.SelectFirstOption();
            //}

            //Search for a duplicate property
            if (dispositionFile.DispositionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(dispositionFile.DispositionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Save Research File
            sharedFileProperties.SaveFileProperties();
        }

        [StepDefinition(@"I update a Disposition File's Properties from row number (.*)")]
        public void UpdateProperties(int rowNumber)
        {
            /* TEST COVERAGE:  */

            PopulateDispositionFile(rowNumber);

            //Search for an existing Disposition File
            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);
            searchDispositionFiles.SelectFirstOption();

            //Navigate to Edit Disposition File's Properties
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Search for a property by PIN
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.SelectPropertyByPID(dispositionFile.DispositionSearchProperties.PID);
            sharedFileProperties.SelectFirstOptionFromSearch();

            //Delete last Property
            sharedFileProperties.DeleteLastPropertyFromFile();

            //Save changes
            sharedFileProperties.SaveFileProperties();

            //Select 1st Property
            sharedFileProperties.SelectFirstPropertyOptionFromFile();
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

        [StepDefinition(@"I create Appraisal, Assessment, Offers and Sales Details within a Disposition File")]
        public void CreateOfferAndSalesAppraisalAndAssessment()
        {
            /* TEST COVERAGE:  */

            //Navigate to Offers and sale Tab
            offerSale.NavigateoffersAndSaleTab();

            //verify Inital screen of Offers and sale Tab
            offerSale.VerifyInitOffersAndSaleTab();

            // Create AppraisalAndAssessment section by clicking edit button
            offerSale.EditAppraisalAndAssessmentButton();
            offerSale.VerifyInitAppraisalAndAssessmentForm();
            offerSale.CreateNewAppraisalAndAssessment(dispositionFile);

            //Save the AppraisalAndAssessment section from
            offerSale.SaveDispositionFileOffersAndSale();

            //Verify Created Appraisal and Assessment form
            offerSale.VerifyCreatedAppraisalAndAssessment(dispositionFile);

            //Add and verify Offers
            if (dispositionFile.DispositionOfferAndSale.Count > 0)
            {
                for(var i = 0; i < dispositionFile.DispositionOfferAndSale.Count; i++)
                {
                    offerSale.CreateNewOffer(dispositionFile.DispositionOfferAndSale[i]);
                    offerSale.SaveDispositionFileOffersAndSale();

                    offerSale.VerifyCreatedOffer(dispositionFile.DispositionOfferAndSale[i], i);
                }     
            }

            // Create Sales Details section by clicking edit button
            offerSale.EditSalesDetailsButton();
            offerSale.VerifyInitSalesDetailsForm();
            offerSale.InsertSalesDetails(dispositionFile);

            //Save the  Sales Details section from
            offerSale.SaveDispositionFileOffersAndSale();

            //Verify Created  Sales Details form
            offerSale.VerifyCreatedSalesDetails(dispositionFile);
        }

        [StepDefinition(@"I update Appraisal, Assessment and Offers section within Disposition File from row number (.*)")]
        public void UpdateOfferAndSalesAppraisalAndAssessment(int rowNumber)
        {
            /* TEST COVERAGE:  */

            PopulateDispositionFile(rowNumber);

            //Search for an existing Disposition File
            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);
            searchDispositionFiles.SelectFirstOption();

            //Navigate to Offers and sale Tab
            offerSale.NavigateoffersAndSaleTab();

            // Create AppraisalAndAssessment section by clicking edit button
            offerSale.EditAppraisalAndAssessmentButton();
            offerSale.UpdateAppraisalAndAssessment(dispositionFile);

            //Cancel Appraisal changes
            offerSale.CancelDispositionFileOffersAndSale();

            // Create AppraisalAndAssessment section by clicking edit button
            offerSale.EditAppraisalAndAssessmentButton();
            offerSale.UpdateAppraisalAndAssessment(dispositionFile);

            //Save the AppraisalAndAssessment section from
            offerSale.SaveDispositionFileOffersAndSale();

            //Verify Created Appraisal and Assessment form
            offerSale.VerifyCreatedAppraisalAndAssessment(dispositionFile);

            //Update first existing offer
            if (dispositionFile.DispositionOfferAndSale.Count > 0)
            {
                offerSale.UpdateOffers(dispositionFile.DispositionOfferAndSale[0], 0);

                //Cancel Offer changes
                offerSale.CancelDispositionFileOffersAndSale();

                //Update first existing offer
                offerSale.UpdateOffers(dispositionFile.DispositionOfferAndSale[0], 0);

                //Save changes on offer
                offerSale.SaveDispositionFileOffersAndSale();

                //Verify updated offer
                offerSale.VerifyCreatedOffer(dispositionFile.DispositionOfferAndSale[0], 0);
            }

            //Delete Offer
            offerSale.DeleteOffer(0);

            //Update Sales Details and cancel changes
            offerSale.EditSalesDetailsButton();
            offerSale.InsertSalesDetails(dispositionFile);
            offerSale.CancelDispositionFileOffersAndSale();

            //Update Sales Details and save changes
            offerSale.EditSalesDetailsButton();
            offerSale.InsertSalesDetails(dispositionFile);
            offerSale.SaveDispositionFileOffersAndSale();

            //Verify Sales Details
            offerSale.VerifyCreatedSalesDetails(dispositionFile);
        }

        [StepDefinition(@"I create a Disposition File from a pin on map from row number (.*)")]
        public void CreateAcquisitionFileFromPin(int rowNumber)
        {
            /* TEST COVERAGE:   */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            PopulateDispositionFile(rowNumber);
            searchProperties.SearchPropertyByPINPID(dispositionFile.DispositionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Disposition File");

            //Validate Acquisition File Details Create Form
            dispositionFileDetails.VerifyDispositionFileInitCreate();

            //Cancel empty acquisition file
            dispositionFileDetails.CancelDispositionFile();

            //Verify Form is no longer visible
            Assert.Equal(0, dispositionFileDetails.IsCreateDispositionFileFormVisible());

            //Search for a property
            searchProperties.SearchPropertyByPINPID(dispositionFile.DispositionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Disposition File");

            //Fill basic Acquisition File information
            dispositionFileDetails.CreateMinimumDispositionFile(dispositionFile);

            //Cancel Creation
            dispositionFileDetails.CancelDispositionFile();

            //Search for a property
            searchProperties.SearchPropertyByPINPID(dispositionFile.DispositionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Disposition File");

            //Fill basic Acquisition File information
            dispositionFileDetails.CreateMinimumDispositionFile(dispositionFile);

            //Save Acquisition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Get Research File code
            dispositionFileCode = dispositionFileDetails.GetDispositionFileCode();

            //Edit Acquisition File
            dispositionFileDetails.EditDispositionFileBttn();

            //Add additional information
            dispositionFileDetails.AddAdditionalInformation(dispositionFile);

            //Save Acquisition File
            dispositionFileDetails.SaveDispositionFileDetails();
        }

        [StepDefinition(@"I search for an existing Disposition File from row number (.*)")]
        public void SearchExistingAcquisitionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-7562, PSP-7563, PSP-7564 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File Search
            PopulateDispositionFile(rowNumber);
            searchDispositionFiles.NavigateToSearchDispositionFile();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchDispositionFiles.DispositionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchDispositionFiles.DispositionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, searchDispositionFiles.DispositionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(50);
            Assert.True(searchDispositionFiles.DispositionFileTableResultNumber() > 21);

            sharedPagination.ChoosePaginationOption(100);
            Assert.True(searchDispositionFiles.DispositionFileTableResultNumber() > 51);

            //Reset Pagination to 10
            sharedPagination.ChoosePaginationOption(10);

            //Verify Column Sorting by File Number
            searchDispositionFiles.OrderByDispositionFileNumber();
            var firstFileNbrDescResult = searchDispositionFiles.FirstDispositionFileNumber();

            searchDispositionFiles.OrderByDispositionFileNumber();
            var firstFileNbrAscResult = searchDispositionFiles.FirstDispositionFileNumber();

            Assert.NotEqual(firstFileNbrDescResult, firstFileNbrAscResult);

            //Verify Column Sorting by Reference Number
            searchDispositionFiles.OrderByDispositionFileReferenceNumber();
            var firstReferenceDescResult = searchDispositionFiles.FirstDispositionReferenceNumber();

            searchDispositionFiles.OrderByDispositionFileReferenceNumber();
            var firstReferenceAscResult = searchDispositionFiles.FirstDispositionReferenceNumber();

            Assert.NotEqual(firstReferenceDescResult, firstReferenceAscResult);

            //Verify Column Sorting by File Name
            searchDispositionFiles.OrderByDispositionFileName();
            var firstFileNameDescResult = searchDispositionFiles.FirstDispositionFileName();

            searchDispositionFiles.OrderByDispositionFileName();
            var firstFileNameAscResult = searchDispositionFiles.FirstDispositionFileName();

            Assert.NotEqual(firstFileNameDescResult, firstFileNameAscResult);

            //Verify Column Sorting by File Type
            searchDispositionFiles.OrderByDispositionFileType();
            var firstFileTypeDescResult = searchDispositionFiles.FirstDispositionFileType();

            searchDispositionFiles.OrderByDispositionFileType();
            var firstFileTypeAscResult = searchDispositionFiles.FirstDispositionFileType();

            Assert.NotEqual(firstFileTypeDescResult, firstFileTypeAscResult);

            //Verify Column Sorting by Disposition Status
            searchDispositionFiles.OrderByDispositionStatus();
            var firstFileDispStatusDescResult = searchDispositionFiles.FirstDispositionStatus();

            searchDispositionFiles.OrderByDispositionStatus();
            var firstFileDispStatusAscResult = searchDispositionFiles.FirstDispositionStatus();

            Assert.NotEqual(firstFileDispStatusDescResult, firstFileDispStatusAscResult);

            //Verify Column Sorting by File Status
            searchDispositionFiles.OrderByDispositionFileStatus();
            var firstFileStatusDescResult = searchDispositionFiles.FirstDispositionFileStatus();

            searchDispositionFiles.OrderByDispositionFileStatus();
            var firstFileStatusAscResult = searchDispositionFiles.FirstDispositionFileStatus();

            Assert.NotEqual(firstFileStatusDescResult, firstFileStatusAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstDispositionPage1 = searchDispositionFiles.FirstDispositionFileNumber();
            sharedPagination.GoNextPage();
            var firstDispositionPage2 = searchDispositionFiles.FirstDispositionFileNumber();
            Assert.NotEqual(firstDispositionPage1, firstDispositionPage2);

            sharedPagination.ResetSearch();

            //Filter Disposition Files
            searchDispositionFiles.FilterDispositionFiles("003-549-551", "Disposition Example File", "", "Archived", "On Hold", "");
            Assert.False(searchDispositionFiles.SearchFoundResults());

            //Look for the last created Disposition File
            searchDispositionFiles.FilterDispositionFiles("", dispositionFile.DispositionFileName, "", dispositionFile.DispositionFileStatus, dispositionFile.DispositionStatus, dispositionFile.DispositionType);
        }

        [StepDefinition(@"A new Disposition file is created successfully")]
        public void NewDispositionFileCreated()
        {
            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);

            Assert.True(searchDispositionFiles.SearchFoundResults());
            searchDispositionFiles.Dispose();
        }

        [StepDefinition(@"Disposition File's Checklist has been saved successfully")]
        public void VerifyChecklistChanges()
        {
            /* TEST COVERAGE: PSP-7538 */

            //Verify Checklist Content after update
            checklist.VerifyChecklistViewForm(dispositionFile.DispositionFileChecklist);
            checklist.Dispose();
        }

        [StepDefinition(@"Expected Disposition File Content is displayed on Disposition File List View")]
        public void VerifyAcquisitionFileTableContent()
        {
            /* TEST COVERAGE: PSP-4253 */

            //Verify List View
            searchDispositionFiles.VerifyDispositionFileListView();
            searchDispositionFiles.VerifyDispositionFileTableContent(dispositionFile);
            searchDispositionFiles.Dispose();

        }

        private void PopulateDispositionFile(int rowNumber)
        {
            DataTable dispositionSheet = ExcelDataContext.GetInstance().Sheets["DispositionFiles"]!;
            ExcelDataContext.PopulateInCollection(dispositionSheet);
            dispositionFile = new DispositionFile();

            //Disposition File Status
            dispositionFile.DispositionFileStatus = ExcelDataContext.ReadData(rowNumber, "DispositionFileStatus");

            //Project
            dispositionFile.DispositionProject = ExcelDataContext.ReadData(rowNumber, "DispositionProject");
            dispositionFile.DispositionProjProduct = ExcelDataContext.ReadData(rowNumber, "DispositionProjProduct");
            dispositionFile.DispositionProjFunding = ExcelDataContext.ReadData(rowNumber, "DispositionProjFunding");

            //Schedule
            dispositionFile.DispositionAssignedDate = ExcelDataContext.ReadData(rowNumber, "DispositionAssignedDate");
            dispositionFile.DispositionCompletedDate = ExcelDataContext.ReadData(rowNumber, "DispositionCompletedDate");

            //Disposition DetailsDisposition
            dispositionFile.DispositionFileName = ExcelDataContext.ReadData(rowNumber, "DispositionFileName");
            dispositionFile.DispositionReferenceNumber = ExcelDataContext.ReadData(rowNumber, "ReferenceNumber");
            dispositionFile.DispositionStatus = ExcelDataContext.ReadData(rowNumber, "DispositionStatus"); 
            dispositionFile.DispositionType = ExcelDataContext.ReadData(rowNumber, "DispositionType");
            dispositionFile.DispositionOtherTransferType = ExcelDataContext.ReadData(rowNumber, "DispositionOtherTransferType");
            dispositionFile.InitiatingDocument = ExcelDataContext.ReadData(rowNumber, "InitiatingDocument");
            dispositionFile.OtherInitiatingDocument = ExcelDataContext.ReadData(rowNumber, "OtherInitiatingDocument");
            dispositionFile.InitiatingDocumentDate = ExcelDataContext.ReadData(rowNumber, "InitiatingDocumentDate");
            dispositionFile.DispositionPhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "DispositionPhysicalFileStatus");
            dispositionFile.InitiatingBranch = ExcelDataContext.ReadData(rowNumber, "InitiatingBranch");
            dispositionFile.DispositionMOTIRegion = ExcelDataContext.ReadData(rowNumber, "DispositionMOTIRegion");

            //Disposition Team
            dispositionFile.DispositionTeamStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionTeamStartRow"));
            dispositionFile.DispositionTeamCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionTeamCount"));

            if (dispositionFile.DispositionTeamStartRow != 0 && dispositionFile.DispositionTeamCount != 0)
                PopulateTeamsCollection(dispositionFile.DispositionTeamStartRow, dispositionFile.DispositionTeamCount);

            //Properties Search
            dispositionFile.DispositionSearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionSearchPropertiesIndex"));
            if (dispositionFile.DispositionSearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"]!;
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                dispositionFile.DispositionSearchProperties.PID = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "PID");
                dispositionFile.DispositionSearchProperties.PIN = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "PIN");
                dispositionFile.DispositionSearchProperties.Address = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "Address");
                dispositionFile.DispositionSearchProperties.PlanNumber = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "PlanNumber");
                dispositionFile.DispositionSearchProperties.LegalDescription = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "LegalDescription");
            }

            //Disposition File Checklist
            dispositionFile.DispositionFileChecklistIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionFileChecklistIndex"));
            if (dispositionFile.DispositionFileChecklistIndex > 0)
            {
                DataTable dispositionFileChecklistSheet = ExcelDataContext.GetInstance().Sheets["DispositionChecklist"]!;
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
                dispositionFile.DispositionFileChecklist.DirectSaleRoadClosureSelect9 = ExcelDataContext.ReadData(dispositionFile.DispositionFileChecklistIndex, "DirectSaleRoadClosureSelect9");

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

            // Disposition Offer and Sales - Appraisal & Assessment
            dispositionFile.AppraisalAndAssessmentValue = ExcelDataContext.ReadData(rowNumber, "AppraisalAndAssessmentValue");
            dispositionFile.AppraisalAndAssessmentDate = ExcelDataContext.ReadData(rowNumber, "AppraisalAndAssessmentDate");
            dispositionFile.AppraisalAndAssessmentBcAssessmentValue = ExcelDataContext.ReadData(rowNumber, "AppraisalAndAssessmentBcAssessmentValue");
            dispositionFile.AppraisalAndAssessmentBcAssessmentRollYear = ExcelDataContext.ReadData(rowNumber, "AppraisalAndAssessmentBcAssessmentRollYear");
            dispositionFile.AppraisalAndAssessmentListPrice = ExcelDataContext.ReadData(rowNumber, "AppraisalAndAssessmentListPrice");

            // Disposition Offer and Sales - Offers
            dispositionFile.OfferSaleStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "OfferSaleStartRow"));
            dispositionFile.OfferSaleTotalCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "OfferSaleTotalCount"));
            if (dispositionFile.OfferSaleStartRow > 0 && dispositionFile.OfferSaleTotalCount > 0)
                PopulateOfferSaleCollection(dispositionFile.OfferSaleStartRow, dispositionFile.OfferSaleTotalCount);

            // Disposition Offer and Sales - Sales Details
            dispositionFile.PurchaseNameStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "PurchaseNameStartRow"));
            dispositionFile.PurchaseNameTotalCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "PurchaseNameTotalCount"));
            if (dispositionFile.PurchaseNameStartRow > 0 && dispositionFile.PurchaseNameTotalCount > 0)
                PopulatePurchaseNamesCollection(dispositionFile.PurchaseNameStartRow, dispositionFile.PurchaseNameTotalCount);

            dispositionFile.PurchaserAgent = ExcelDataContext.ReadData(rowNumber, "PurchaserAgent");
            dispositionFile.PurchaserAgentType = ExcelDataContext.ReadData(rowNumber, "PurchaserAgentType");
            dispositionFile.PurchaserAgentPrimaryContact = ExcelDataContext.ReadData(rowNumber, "PurchaserAgentPrimaryContact");
            dispositionFile.PurchaserSolicitor = ExcelDataContext.ReadData(rowNumber, "PurchaserSolicitor");
            dispositionFile.PurchaserSolicitorType = ExcelDataContext.ReadData(rowNumber, "PurchaserSolicitorType");
            dispositionFile.PurchaserSolicitorPrimaryContact = ExcelDataContext.ReadData(rowNumber, "PurchaserSolicitorPrimaryContact");
            dispositionFile.LastConditionRemovalDate = ExcelDataContext.ReadData(rowNumber, "LastConditionRemovalDate");
            dispositionFile.SaleCompletionDate = ExcelDataContext.ReadData(rowNumber, "SaleCompletionDate");
            dispositionFile.FiscalYearOfSale = ExcelDataContext.ReadData(rowNumber, "FiscalYearOfSale");
            dispositionFile.FinalSalePrice = ExcelDataContext.ReadData(rowNumber, "FinalSalePrice");
            dispositionFile.RealtorCommission = ExcelDataContext.ReadData(rowNumber, "RealtorCommission");
            dispositionFile.GSTRequired = ExcelDataContext.ReadData(rowNumber, "GSTRequired");
            dispositionFile.GSTCollected = ExcelDataContext.ReadData(rowNumber, "GSTCollected");
            dispositionFile.NetBookValue = ExcelDataContext.ReadData(rowNumber, "NetBookValue");
            dispositionFile.TotalCostOfSales = ExcelDataContext.ReadData(rowNumber, "TotalCostOfSales");
            dispositionFile.NetProceedsBeforeSPP = ExcelDataContext.ReadData(rowNumber, "NetProceedsBeforeSPP");
            dispositionFile.SPPAmount = ExcelDataContext.ReadData(rowNumber, "SPPAmount");
            dispositionFile.NetProceedsAfterSPP = ExcelDataContext.ReadData(rowNumber, "NetProceedsAfterSPP");
            dispositionFile.RemediationCost = ExcelDataContext.ReadData(rowNumber, "RemediationCost");

        }

        private void PopulateTeamsCollection(int startRow, int rowsCount)
        {
            DataTable teamsSheet = ExcelDataContext.GetInstance().Sheets["TeamMembers"]!;
            ExcelDataContext.PopulateInCollection(teamsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                TeamMember teamMember = new TeamMember();
                teamMember.TeamMemberRole = ExcelDataContext.ReadData(i, "TeamMemberRole");
                teamMember.TeamMemberContactName = ExcelDataContext.ReadData(i, "TeamMemberContactName");
                teamMember.TeamMemberContactType = ExcelDataContext.ReadData(i, "TeamMemberContactType");
                teamMember.TeamMemberPrimaryContact = ExcelDataContext.ReadData(i, "TeamMemberPrimaryContact");

                dispositionFile.DispositionTeam.Add(teamMember);
            }
        }

        private void PopulateOfferSaleCollection(int startRow, int rowsCount)
        {
            DataTable OfferSaleSheet = ExcelDataContext.GetInstance().Sheets["DispositionOfferSale"]!;
            ExcelDataContext.PopulateInCollection(OfferSaleSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                DispositionOfferAndSale offerAndSale = new DispositionOfferAndSale();
                offerAndSale.OfferOfferStatus = ExcelDataContext.ReadData(i, "OfferOfferStatus");
                offerAndSale.OfferOfferName = ExcelDataContext.ReadData(i, "OfferOfferName");
                offerAndSale.OfferOfferDate = ExcelDataContext.ReadData(i, "OfferOfferDate");
                offerAndSale.OfferOfferExpiryDate = ExcelDataContext.ReadData(i, "OfferOfferExpiryDate");
                offerAndSale.OfferPrice = ExcelDataContext.ReadData(i, "OfferPrice");
                offerAndSale.OfferNotes = ExcelDataContext.ReadData(i, "OfferNotes");

                dispositionFile.DispositionOfferAndSale.Add(offerAndSale);
            }
        }

        private void PopulatePurchaseNamesCollection(int startRow, int rowsCount)
        {
            DataTable purchaseSheet = ExcelDataContext.GetInstance().Sheets["PurchaserNames"]!;
            ExcelDataContext.PopulateInCollection(purchaseSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                PurchaseMember purchaseMember = new PurchaseMember();
                purchaseMember.PurchaserName = ExcelDataContext.ReadData(i, "PurchaserName");
                purchaseMember.PurchaseMemberContactType = ExcelDataContext.ReadData(i, "PurchaseMemberContactType");
                purchaseMember.PurchaseMemberPrimaryContact = ExcelDataContext.ReadData(i, "PurchaseMemberPrimaryContact");

               dispositionFile.PurchaserNames.Add(purchaseMember);
            }
        }


    }
}
