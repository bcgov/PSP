using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class AcquisitionFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly GenericSteps genericSteps;
        private readonly AcquisitionDetails acquisitionFilesDetails;
        private readonly SearchAcquisitionFiles searchAcquisitionFiles;
        private readonly SharedFileProperties sharedFileProperties;
        private readonly SharedPagination sharedPagination;
        private readonly SearchProperties searchProperties;
        private readonly AcquisitionTakes acquisitionTakes;
        private readonly PropertyInformation propertyInformation;
        private readonly AcquisitionChecklist checklist;
        private readonly AcquisitionAgreements agreements;
        private readonly AcquisitionStakeholders stakeholders;
        private readonly AcquisitionCompensations h120;
        private readonly AcquisitionExpropriation expropriation;
        private readonly Notes notes;

        private readonly string userName = "TRANPSP1";

        private AcquisitionFile acquisitionFile;
        protected string acquisitionFileCode = "";
        protected string compensationNumber = "";

        public AcquisitionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            genericSteps = new GenericSteps(driver);

            acquisitionFilesDetails = new AcquisitionDetails(driver.Current);
            searchAcquisitionFiles = new SearchAcquisitionFiles(driver.Current);
            sharedFileProperties = new SharedFileProperties(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            acquisitionTakes = new AcquisitionTakes(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            checklist = new AcquisitionChecklist(driver.Current);
            agreements = new AcquisitionAgreements(driver.Current);
            stakeholders = new AcquisitionStakeholders(driver.Current);
            h120 = new AcquisitionCompensations(driver.Current);
            expropriation = new AcquisitionExpropriation(driver.Current);
            notes = new Notes(driver.Current);

            acquisitionFile = new AcquisitionFile();
        }

        [StepDefinition(@"I create a new Acquisition File from row number (.*)")]
        public void CreateAcquisitionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4163, PSP-4164, PSP-4165, PSP-4323, PSP-4472, PSP-4553 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            PopulateAcquisitionFile(rowNumber);
            acquisitionFilesDetails.NavigateToCreateNewAcquisitionFile();

            //Validate Acquisition File Details Create Form
            acquisitionFilesDetails.VerifyAcquisitionFileCreate();

            //Create basic Acquisition File
            acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Get Research File code
            acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();
        }

        [StepDefinition(@"I add additional information to the Acquisition File Details")]
        public void AddAdditionalInfoAcquisitionFile()
        {
            /* TEST COVERAGE:  PSP-4469, PSP-4471, PSP-4553, PSP-5308, PSP-5590, PSP-5634, PSP-5637, PSP-5790, PSP-5979, PSP-6041 */

            //Enter to Edit mode of Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();

            //Verify Maximum fields
            acquisitionFilesDetails.VerifyMaximumFields();

            //Add Additional Optional information to the acquisition file
            acquisitionFilesDetails.AddAdditionalInformation(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Validate View File Details View Mode
            acquisitionFilesDetails.VerifyAcquisitionFileView(acquisitionFile);

            //Verify automatic note created when
            if (acquisitionFile.AcquisitionStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Acquisition File", "Active", acquisitionFile.AcquisitionStatus);
            }
        }

        [StepDefinition(@"I update the File details from an existing Acquisition File from row number (.*)")]
        public void UpdateFileDetails(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4331, PSP-4544, PSP-4545, PSP-5638, PSP-5639, PSP-5970, PSP-5979s */

            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Update existing Acquisition file
            acquisitionFilesDetails.EditAcquisitionFileBttn();
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile);

            //Cancel changes
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Edit Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Get Research File code
            acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();

            //Validate View File Details View Mode
            acquisitionFilesDetails.VerifyAcquisitionFileView(acquisitionFile);

            //Verify automatic note created when
            if (acquisitionFile.AcquisitionStatus != "Active")
            {
               notes.NavigateNotesTab();
               notes.VerifyAutomaticNotes("Acquisition File", "Hold", acquisitionFile.AcquisitionStatus);
            }
        }

        [StepDefinition(@"I add Properties to the Acquisition File")]
        public void AddProperties()
       {
            /* TEST COVERAGE: PSP-4163, PSP-4325, PSP-4326, PSP-4327, PSP-4328, PSP-4329, PSP-4334, PSP-4593, PSP-6268 */

            //Navigate to Properties for Acquisition File
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (acquisitionFile.AcquisitionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by PIN
            if (acquisitionFile.AcquisitionSearchProperties.PIN != "")
            {
                sharedFileProperties.SelectPropertyByPIN(acquisitionFile.AcquisitionSearchProperties.PIN);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Plan
            if (acquisitionFile.AcquisitionSearchProperties.PlanNumber != "")
            {
                sharedFileProperties.SelectPropertyByPlan(acquisitionFile.AcquisitionSearchProperties.PlanNumber);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Address
            if (acquisitionFile.AcquisitionSearchProperties.Address != "")
            {
                sharedFileProperties.SelectPropertyByAddress(acquisitionFile.AcquisitionSearchProperties.Address);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Legal Description
            //if (acquisitionFile.SearchProperties.LegalDescription != "")
            //{
            //    sharedSearchProperties.SelectPropertyByLegalDescription(acquisitionFile.SearchProperties.LegalDescription);
            //    sharedSearchProperties.SelectFirstOption();
            //}

            //Search for Multiple PIDs
            if(acquisitionFile.AcquisitionSearchProperties.MultiplePIDS.Count > 0)
            {
                foreach (string prop in acquisitionFile.AcquisitionSearchProperties.MultiplePIDS)
                {
                    sharedFileProperties.SelectPropertyByPID(prop);
                    sharedFileProperties.SelectFirstOptionFromSearch();
                }
            }

            //Search for a duplicate property
            if (acquisitionFile.AcquisitionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Save Research File
            sharedFileProperties.SaveFileProperties();
        }

        [StepDefinition(@"I update an Acquisition File's Properties from row number (.*)")]
        public void UpdateProperties(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-4590, PSP-4591, PSP-4600, PSP-4689, PSP-5003, PSP-5006, PSP-5007  */

            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Navigate to Edit Acquisition File's Properties
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Search for a property by Legal Description
            //sharedSearchProperties.NavigateToSearchTab();
            //sharedSearchProperties.SelectPropertyByLegalDescription(acquisitionFile.SearchProperties.LegalDescription);
            //sharedSearchProperties.SelectFirstOption();

            //Save changes
            sharedFileProperties.SaveFileProperties();

            //Delete Property
            sharedFileProperties.NavigateToAddPropertiesToFile();
            sharedFileProperties.DeleteLastPropertyFromFile();

            //Save Acquisition File changes
            sharedFileProperties.SaveFileProperties();

            //Select 1st Property
            sharedFileProperties.SelectFirstPropertyOptionFromFile();
        }

        [StepDefinition(@"I create Takes within Acquisition File's Properties")]
        public void CreateTakes()
        {
            /* TEST COVERAGE:  PSP-5892, PSP-5893, PSP-5896, PSP-5898 */

            for (int i = 0; i < acquisitionFile.AcquisitionTakes.Count; i++)
            {
                //Choose Take's Property
                sharedFileProperties.SelectNthPropertyOptionFromFile(acquisitionFile.AcquisitionTakes[i].FromProperty);

                //Navigate to the Takes Tab
                acquisitionTakes.NavigateTakesTab();

                //Verify Init form
                acquisitionTakes.VerifyInitTakesView();

                //Click on Edit button
                acquisitionTakes.ClickEditTakesButton();

                //Insert Take
                if (acquisitionFile.AcquisitionTakes[i].TakeCounter.Equals(0))
                {
                    acquisitionTakes.VerifyInitCreateForm();
                    acquisitionTakes.InsertTake(acquisitionFile.AcquisitionTakes[i]);
                }
                else
                {
                    acquisitionTakes.ClickCreateNewTakeBttn();
                    acquisitionTakes.InsertTake(acquisitionFile.AcquisitionTakes[i]);
                }

                //Save Take
                acquisitionTakes.SaveTake();

                //Verify View Form
                acquisitionTakes.VerifyCreatedTakeViewForm(acquisitionFile.AcquisitionTakes[i]);
            }
        }

        [StepDefinition(@"I update Takes within Acquisition File's Properties from row number (.*)")]
        public void UpdateTakes(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-5894, PSP-5895 */

            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Choose Take's Property
            sharedFileProperties.SelectNthPropertyOptionFromFile(acquisitionFile.AcquisitionTakes[0].FromProperty);

            //Navigate to Takes Tab
            acquisitionTakes.NavigateTakesTab();

            //Update Take
            acquisitionTakes.ClickEditTakesButton();
            acquisitionTakes.InsertTake(acquisitionFile.AcquisitionTakes[0]);

            //Save Take
            acquisitionTakes.SaveTake();

            //Verify View Form
            acquisitionTakes.VerifyCreatedTakeViewForm(acquisitionFile.AcquisitionTakes[0]);

            //Choose Take's Property
            sharedFileProperties.SelectNthPropertyOptionFromFile(acquisitionFile.AcquisitionTakes[0].FromProperty);

            //Navigate to Takes Tab
            acquisitionTakes.NavigateTakesTab();

            //Delete Take
            acquisitionTakes.ClickEditTakesButton();
            acquisitionTakes.DeleteTake(acquisitionFile.AcquisitionTakes[0].TakeCounter);

            //Save Take
            acquisitionTakes.SaveTake();
        }

        [StepDefinition(@"I insert Checklist information to an Acquisition File")]
        public void CreateChecklist()
        {
            /* TEST COVERAGE: PSP-5899, PSP-5900, PSP-5904, PSP-5921 */

            //Navigate to Checklist Tab
            checklist.NavigateChecklistTab();

            //Verify View Checklist form
            checklist.VerifyChecklistInitViewForm();

            //Edit Checklist button
            checklist.EditChecklistButton();

            //Verify Edit Checklist form
            checklist.VerifyChecklistEditForm();

            //Update Checklist Form

            checklist.UpdateChecklist(acquisitionFile.AcquisitionFileChecklist);

            //Save changes
            checklist.SaveAcquisitionFileChecklist();
        }

        [StepDefinition(@"I create Agreements within an Acquisition File")]
        public void CreateAgreement()
        {
            /* TEST COVERAGE: PSP-5965, PSP-5966, PSP-5991, PSP-5993, PSP-6000, PSP-6095 */

            //Navigate to Agreements Tab
            agreements.NavigateAgreementsTab();

            //Verify initial Agreement Tab View
            agreements.VerifyInitAgreementTab();

            if (acquisitionFile.AgreementCount > 0)
            {
                for (int i = 0; i < acquisitionFile.AcquisitionAgreements.Count; i++)
                {
                    //Create Agreement button
                    agreements.CreateNewAgreementBttn();

                    //Verify Create Agreement form
                    agreements.VerifyCreateAgreementForm();

                    //Add a new Agreement
                    agreements.CreateUpdateAgreement(acquisitionFile.AcquisitionAgreements[i]);

                    //Save new agreement
                    agreements.SaveAcquisitionFileAgreement();

                    //Verify Edit Agreement form
                    agreements.VerifyViewAgreementForm(acquisitionFile.AcquisitionAgreements[i], i);
                }
            }
        }

        [StepDefinition(@"I update an Agreement within an Acquisition File from row number (.*)")]
        public void UpdateAgreement(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5967, PSP-5997, PSP-5998 */

            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Navigate to Agreements Tab
            agreements.NavigateAgreementsTab();

            //Create Agreement button
            agreements.CreateNewAgreementBttn();

            //Verify Create Agreement form
            agreements.VerifyCreateAgreementForm();

            //Add a new Agreement
            agreements.CreateUpdateAgreement(acquisitionFile.AcquisitionAgreements[0]);

            //Cancel agreements
            agreements.CancelAcquisitionFileAgreement();

            //Create Agreement button
            agreements.CreateNewAgreementBttn();

            //Add a new Agreement
            agreements.CreateUpdateAgreement(acquisitionFile.AcquisitionAgreements[0]);

            //Save new agreement
            agreements.SaveAcquisitionFileAgreement();

            //Verify new added Agreement form
            agreements.VerifyViewAgreementForm(acquisitionFile.AcquisitionAgreements[0], 4);

            //Edit Agreement button
            agreements.EditAgreementButton(0);

            //Update created agreement
            agreements.CreateUpdateAgreement(acquisitionFile.AcquisitionAgreements[1]);

            //Save new agreement
            agreements.SaveAcquisitionFileAgreement();

            //Verify Edit Agreement form
            agreements.VerifyViewAgreementForm(acquisitionFile.AcquisitionAgreements[1], 0);

            var agreementsBeforeDelete = agreements.TotalAgreementsCount();

            //Delete last agreement
            agreements.DeleteLastAgreement();

            var agreementsAfterDelete = agreements.TotalAgreementsCount();
            Assert.True(agreementsBeforeDelete - agreementsAfterDelete == 1);
        }

        [StepDefinition(@"I create Stakeholders within an Acquisition File")]
        public void CreateStakeholder()
        {
            /* TEST COVERAGE: PSP-6394 */

            //Navigate to Stakeholders Tab
            stakeholders.NavigateStakeholderTab();

            //Verify initial Stakeholder Tab View
            stakeholders.VerifyStakeholdersInitView();

            if (acquisitionFile.StakeholderCount > 0)
            {
                for (int i = 0; i < acquisitionFile.AcquisitionStakeholders.Count; i++)
                {
                    if (acquisitionFile.AcquisitionStakeholders[i].StakeholderType.Equals("Interest"))
                    {
                        //Click on edit the Interest Stakeholder button
                        stakeholders.EditStakeholderInterestsButton();

                        //Add new Interest Stakeholder to the Acquisition File
                        stakeholders.AddInterestStakeholderButton();
                        stakeholders.CreateInterestsStakeholder(acquisitionFile.AcquisitionStakeholders[i], acquisitionFile.AcquisitionStakeholders[i].StakeholderIndex);

                        //Save new Interest Stakeholder
                        stakeholders.SaveAcquisitionFileStakeholder();

                        //Verify added Interest Stakeholder
                        stakeholders.VerifyInterestStakeholderViewForm(acquisitionFile.AcquisitionStakeholders[i]);
                    }
                    else
                    {
                        //Click on edit the Interest Stakeholder button
                        stakeholders.EditStakeholderNonInterestsButton();

                        //Add new Interest Stakeholder to the Acquisition File
                        stakeholders.AddNonInterestStakeholderButton();
                        stakeholders.CreateNonInterestsStakeholder(acquisitionFile.AcquisitionStakeholders[i], acquisitionFile.AcquisitionStakeholders[i].StakeholderIndex);

                        //Save new Interest Stakeholder
                        stakeholders.SaveAcquisitionFileStakeholder();

                        //Verify added Interest Stakeholder
                        stakeholders.VerifyNonInterestStakeholderViewForm(acquisitionFile.AcquisitionStakeholders[i]);
                    }
                }
            }
        }

        [StepDefinition(@"I update Stakeholders within an Acquisition File")]
        public void UpdateStakeholder()
        {
            /* TEST COVERAGE: PSP-6398 */

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Navigate to Stakeholders Tab
            stakeholders.NavigateStakeholderTab();

            //Delete last Interest stakeholder
            var interestStakeholdersBeforeDelete = stakeholders.TotalInterestHolders();
            stakeholders.DeleteLastInterestHolder();

            //Save Interest Stakeholder changes
            stakeholders.SaveAcquisitionFileStakeholder();

            var interestStakeholdersAfterDelete = stakeholders.TotalInterestHolders();
            Assert.True(interestStakeholdersBeforeDelete - interestStakeholdersAfterDelete == 1);

            //Delete last Non-Interest stakeholder
            var nonInterestStakeholderBeforeDelete = stakeholders.TotalNonInterestHolders();
            stakeholders.DeleteLastNonInterestHolder();

            //Save Interest Stakeholder changes
            stakeholders.SaveAcquisitionFileStakeholder();

            var nonInterestStakeholderAfterDelete = stakeholders.TotalNonInterestHolders();
            Assert.True(nonInterestStakeholderBeforeDelete - nonInterestStakeholderAfterDelete == 1);
        }

        [StepDefinition(@"I create Compensation Requisition within an Acquisition File")]
        public void CreateCompensationRequisition()
        {
            /* TEST COVERAGE: PSP-6066, PSP-6067, PSP-6274, PSP-6277, PSP-6355 */

            //Navigate to Compensation Requisition Tab
            h120.NavigateCompensationTab();

            //Verify initial Compensation Tab List View
            h120.VerifyCompensationInitTabView();

            //Update Allowable Compensation Amount
            //h120.UpdateTotalAllowableCompensation(acquisitionFile.CompensationTotalAllowableAmount);

            //Create Compensation Requisition Forms
            if (acquisitionFile.AcquisitionCompensations.Count > 0)
            {
                for (int i = 0; i < acquisitionFile.AcquisitionCompensations.Count; i++)
                {
                    //Click on Add new Compensation
                    h120.AddCompensationBttn();

                    //Open the created Compensation Requisition details
                    h120.OpenCompensationDetails(i);

                    //Verify Initial View Form
                    h120.VerifyCompensationDetailsInitViewForm();

                    //Add Details to the Compensation Requisition
                    h120.EditCompensationDetails();
                    //h120.VerifyCompensationDetailsInitCreateForm();
                    h120.UpdateCompensationDetails(acquisitionFile.AcquisitionCompensations[i]);

                    //Save new Compensation Requisition Details
                    h120.SaveAcquisitionFileCompensation();

                    //Verify added Compensation Requisition List View and Details
                    h120.VerifyCompensationDetailsViewForm(acquisitionFile.AcquisitionCompensations[i]);
                    h120.VerifyCompensationListView(acquisitionFile.AcquisitionCompensations[i]);
                }
            }
        }

        [StepDefinition(@"I update Compensation Requisition within an Acquisition File from row number (.*)")]
        public void UpdateCompensationRequisition(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-6275, PSP-6282, PSP-6356, PSP-6360, PSP-6483, PSP-6484 */

            //Populate data
            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Navigate to Compensation Tab
            h120.NavigateCompensationTab();

            //Select first created compensation requisition
            h120.OpenCompensationDetails(0);

            //Edit Compensation button
            h120.EditCompensationDetails();

            //Make changes on created Compensation Requisition Form
            h120.UpdateCompensationDetails(acquisitionFile.AcquisitionCompensations[0]);

            //Cancel changes
            h120.CancelAcquisitionFileCompensation();

            //Make changes on created Compensation Requisition Form
            h120.EditCompensationDetails();
            h120.UpdateCompensationDetails(acquisitionFile.AcquisitionCompensations[0]);

            //Save changes
            h120.SaveAcquisitionFileCompensation();

            //Get updated compensation number
            compensationNumber = h120.GetCompensationFileNumber(1);

            //Verify automatic note
            notes.NavigateNotesTab();
            notes.VerifyAutomaticNotesCompensation(compensationNumber, "Draft", "Final");

            //Navigate to Acquisition File Details
            acquisitionFilesDetails.NavigateToFileDetailsTab();

            //Edit the Acquisition File Details
            acquisitionFilesDetails.EditAcquisitionFileBttn();

            //Delete the MoTI Solicitor that is associated to a compensation requisition
            acquisitionFilesDetails.DeleteFirstStaffMember();

            //Save Acquisition File Details changes
            acquisitionFilesDetails.SaveAcquisitionFileDetailsWithExpectedErrors();

            //Cancel Acquisition File changes
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Navigate back to Compensation Tab
            h120.NavigateCompensationTab();

            //Open Requisition File
            h120.OpenCompensationDetails(0);

            //Edit Compensation Button
            h120.EditCompensationDetails();

            //Delete Financial Activity
            var activitiesBeforeDelete = h120.TotalActivitiesCount();
            h120.DeleteFirstActivity();

            var activitiesAfterDelete = h120.TotalActivitiesCount();
            Assert.True(activitiesBeforeDelete - activitiesAfterDelete == 1);

            //Save Compensation changes
            h120.SaveAcquisitionFileCompensation();

            //Create a new Compensation Requisition
            h120.AddCompensationBttn();

            var compensationsBeforeDelete = h120.TotalCompensationCount();
            h120.DeleteCompensationRequisition(1);

            var compensationsAfterDelete = h120.TotalCompensationCount();


            Assert.True(compensationsBeforeDelete - compensationsAfterDelete == 1);
        }

        [StepDefinition(@"I create Expropriations within an Acquisition File")]
        public void CreateExpropriation()
        {
            /* TEST COVERAGE:  PSP-6555, PSP-6559, PSP-6560 */

            //Navigate to Expropriation Requisition Tab
            expropriation.NavigateToExpropriationTab();

            //Verify initial Expropriation Tab List View
            if (acquisitionFile.AcquisitionType.Equals("Section 3 Agreement"))
                expropriation.VerifySection3InitExpropriationTab();
            else
                expropriation.VerifySection6InitExpropriationTab();

            //Create Compensation Requisition Forms
            if (acquisitionFile.AcquisitionExpropriationForm8s.Count > 0)
            {
                for (int i = 0; i < acquisitionFile.AcquisitionExpropriationForm8s.Count; i++)
                {
                    //Click on Add new Form 8
                    expropriation.AddForm8Button();

                    //Verify Initial Create Form
                    expropriation.VerifyInitCreateForm8();

                    //Add Details to the Expropriation Form 8
                    expropriation.CreateForm8(acquisitionFile.AcquisitionExpropriationForm8s[i]);

                    //Save the Form 8
                    expropriation.SaveExpropriation();

                    //Verify Created Form 8 View
                    expropriation.VerifyCreatedForm8View(acquisitionFile.AcquisitionExpropriationForm8s[i], i);
                }
            }
        }

        [StepDefinition(@"I update Expropriation within an Acquisition File from row number (.*)")]
        public void UpdateExpropriation(int rowNumber)
        {

            //Populate data
            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Update type of Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile);
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Navigate to Expropriation Requisition Tab
            expropriation.NavigateToExpropriationTab();

            //Verify initial Expropriation Tab List View
            if (acquisitionFile.AcquisitionType.Equals("Section 3 Agreement"))
                expropriation.VerifySection3InitExpropriationTab();
            else
                expropriation.VerifySection6InitExpropriationTab();

            //Edit Expropriation button
            expropriation.EditNthForm8Button(0);

            //Make changes on created Expropriation Form 8
            expropriation.UpdateForm8(acquisitionFile.AcquisitionExpropriationForm8s[0]);

            //Cancel changes
            expropriation.CancelExpropriation();

            //Make changes on created Compensation Requisition Form
            expropriation.EditNthForm8Button(0);
            expropriation.UpdateForm8(acquisitionFile.AcquisitionExpropriationForm8s[0]);

            //Save changes
            expropriation.SaveExpropriation();

            //Verify Created Form 8 View
            expropriation.VerifyCreatedForm8View(acquisitionFile.AcquisitionExpropriationForm8s[0], 0);

            //Edit First Form 8
            var paymentsBeforeDelete = expropriation.TotalPaymentsCount();
            expropriation.EditNthForm8Button(0);

            //Delete Payment
            expropriation.DeleteFirstPayment();

            //Save Compensation changes
            expropriation.SaveExpropriation();
            var paymentsAfterDelete = expropriation.TotalPaymentsCount();
            Assert.True(paymentsBeforeDelete - paymentsAfterDelete == 1);

            //Delete Form 8
            var expropriationsBeforeDelete = expropriation.TotalExpropriationCount();
            expropriation.DeleteNthForm8(1);

            var expropriationsAfterDelete = expropriation.TotalExpropriationCount();
            Assert.True(expropriationsBeforeDelete - expropriationsAfterDelete == 1);
        }

        [StepDefinition(@"I create an Acquisition File from a pin on map from row number (.*)")]
        public void CreateAcquisitionFileFromPin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-1546, PSP-1556, PSP-4164, PSP-4165, PSP-4167, PSP-4472, PSP-4601, PSP-4704, PSP-5308  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            PopulateAcquisitionFile(rowNumber);
            searchProperties.SearchPropertyByPINPID(acquisitionFile.AcquisitionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

            //Validate Acquisition File Details Create Form
            acquisitionFilesDetails.VerifyAcquisitionFileCreate();

            //Cancel empty acquisition file
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Verify Form is no longer visible
            Assert.Equal(0, acquisitionFilesDetails.IsCreateAcquisitionFileFormVisible());

            //Search for a property
            searchProperties.SearchPropertyByPINPID(acquisitionFile.AcquisitionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

            //Fill basic Acquisition File information
            acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

            //Cancel Creation
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Search for a property
            searchProperties.SearchPropertyByPINPID(acquisitionFile.AcquisitionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

            //Fill basic Acquisition File information
            acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Get Research File code
            acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();

            //Edit Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();

            //Add additional information
            acquisitionFilesDetails.AddAdditionalInformation(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();
        }

        [StepDefinition(@"I search for an existing Acquisition File from row number (.*)")]
        public void SearchExistingAcquisitionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4252, PSP-4255, PSP-5589 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File Search
            PopulateAcquisitionFile(rowNumber);
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            //Verify Column Sorting by File Number
            searchAcquisitionFiles.OrderByAcquisitionFileNumber();
            var firstFileNbrDescResult = searchAcquisitionFiles.FirstAcquisitionFileNumber();

            searchAcquisitionFiles.OrderByAcquisitionFileNumber();
            var firstFileNbrAscResult = searchAcquisitionFiles.FirstAcquisitionFileNumber();

            Assert.NotEqual(firstFileNbrDescResult, firstFileNbrAscResult);

            //Verify Column Sorting by Historical File Number
            searchAcquisitionFiles.OrderByAcquisitionFileHistoricalNumber();
            var firstHistoricalDescResult = searchAcquisitionFiles.FirstAcquisitionLegacyFileNumber();

            searchAcquisitionFiles.OrderByAcquisitionFileHistoricalNumber();
            var firstHistoricalAscResult = searchAcquisitionFiles.FirstAcquisitionLegacyFileNumber();

            Assert.NotEqual(firstHistoricalDescResult, firstHistoricalAscResult);

            //Verify Column Sorting by File Name
            searchAcquisitionFiles.OrderByAcquisitionFileName();
            var firstFileNameDescResult = searchAcquisitionFiles.FirstAcquisitionFileName();

            searchAcquisitionFiles.OrderByAcquisitionFileName();
            var firstFileNameAscResult = searchAcquisitionFiles.FirstAcquisitionFileName();

            Assert.NotEqual(firstFileNameDescResult, firstFileNameAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstAcquisitionPage1 = searchAcquisitionFiles.FirstAcquisitionFileNumber();
            sharedPagination.GoNextPage();
            var firstAcquisitionPage2 = searchAcquisitionFiles.FirstAcquisitionFileNumber();
            Assert.NotEqual(firstAcquisitionPage1, firstAcquisitionPage2);

            sharedPagination.ResetSearch();

            //Filter Acquisition Files
            searchAcquisitionFiles.FilterAcquisitionFiles("003-549-551", "", "", "Acquisition from Jonathan Doe", "", "Cancelled", "");
            Assert.False(searchAcquisitionFiles.SearchFoundResults());

            //Look for the last created Acquisition File
            searchAcquisitionFiles.FilterAcquisitionFiles("", "", "", acquisitionFile.AcquisitionFileName, "", acquisitionFile.AcquisitionStatus, "");
        }

        [StepDefinition(@"A new Acquisition file is created successfully")]
        public void NewAcquisitionFileCreated()
        {
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);

            Assert.True(searchAcquisitionFiles.SearchFoundResults());
            searchAcquisitionFiles.Dispose();
        }

        [StepDefinition(@"An existing Acquisition file has been edited successfully")]
        public void EditAcquisitionFileSuccess()
        {
            acquisitionFilesDetails.NavigateToFileDetailsTab();
            acquisitionFilesDetails.VerifyAcquisitionFileView(acquisitionFile);
            searchAcquisitionFiles.Dispose();
        }

        [StepDefinition(@"Expected Acquisition File Content is displayed on Acquisition File Table")]
        public void VerifyAcquisitionFileTableContent()
        {
            /* TEST COVERAGE: PSP-4253 */

            //Verify List View
            searchAcquisitionFiles.VerifyAcquisitionFileListView();
            searchAcquisitionFiles.VerifyAcquisitionFileTableContent(acquisitionFile);
            searchAcquisitionFiles.Dispose();

        }

        [StepDefinition(@"Acquisition File's Checklist has been saved successfully")]
        public void VerifyChecklistChanges()
        {
            //Verify Checklist Content after update
            checklist.VerifyChecklistViewForm(acquisitionFile.AcquisitionFileChecklist);
            searchAcquisitionFiles.Dispose();
        }

        private void PopulateAcquisitionFile(int rowNumber)
        {
            DataTable acquisitionSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionFiles"]!;
            ExcelDataContext.PopulateInCollection(acquisitionSheet);
            acquisitionFile = new AcquisitionFile();

            //Acquisition Status
            acquisitionFile.AcquisitionStatus = ExcelDataContext.ReadData(rowNumber, "AcquisitionStatus");

            //Project
            acquisitionFile.AcquisitionProject = ExcelDataContext.ReadData(rowNumber, "AcquisitionProject");
            acquisitionFile.AcquisitionProjCode = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjCode");
            acquisitionFile.AcquisitionProjProductCode = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjProductCode");
            acquisitionFile.AcquisitionProjProduct = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjProduct");
            acquisitionFile.AcquisitionProjFunding = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjFunding");
            acquisitionFile.AcquisitionFundingOther = ExcelDataContext.ReadData(rowNumber, "AcquisitionFundingOther");

            //Schedule
            acquisitionFile.AssignedDate = ExcelDataContext.ReadData(rowNumber, "AssignedDate");
            acquisitionFile.DeliveryDate = ExcelDataContext.ReadData(rowNumber, "DeliveryDate");
            acquisitionFile.AcquisitionCompletedDate = ExcelDataContext.ReadData(rowNumber, "AcquisitionCompletedDate");

            //Acquisition Details
            acquisitionFile.AcquisitionFileName = ExcelDataContext.ReadData(rowNumber, "AcquisitionFileName");
            acquisitionFile.HistoricalFileNumber = ExcelDataContext.ReadData(rowNumber, "HistoricalFileNumber");
            acquisitionFile.PhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "PhysicalFileStatus");
            acquisitionFile.AcquisitionType = ExcelDataContext.ReadData(rowNumber, "AcquisitionType");
            acquisitionFile.AcquisitionMOTIRegion = ExcelDataContext.ReadData(rowNumber, "AcquisitionMOTIRegion");

            acquisitionFile.AcquisitionTeamStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionTeamStartRow"));
            acquisitionFile.AcquisitionTeamCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionTeamCount"));

            //Acquisition Team
            if (acquisitionFile.AcquisitionTeamStartRow != 0 && acquisitionFile.AcquisitionTeamCount != 0)
            {
                PopulateTeamsCollection(acquisitionFile.AcquisitionTeamStartRow, acquisitionFile.AcquisitionTeamCount);
            }

            //Owner
            acquisitionFile.OwnerStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerStartRow"));
            acquisitionFile.OwnerCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerCount"));
            if (acquisitionFile.OwnerStartRow != 0 && acquisitionFile.OwnerCount != 0)
                PopulateOwnersCollection(acquisitionFile.OwnerStartRow, acquisitionFile.OwnerCount);
            
            acquisitionFile.OwnerSolicitor = ExcelDataContext.ReadData(rowNumber, "OwnerSolicitor");
            acquisitionFile.OwnerRepresentative = ExcelDataContext.ReadData(rowNumber, "OwnerRepresentative");
            acquisitionFile.OwnerComment = ExcelDataContext.ReadData(rowNumber, "OwnerComment");

            //Properties Search
            acquisitionFile.AcquisitionSearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcqSearchPropertiesIndex"));
            if (acquisitionFile.AcquisitionSearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"]!;
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                acquisitionFile.AcquisitionSearchProperties.PID = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "PID");
                acquisitionFile.AcquisitionSearchProperties.PIN = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "PIN");
                acquisitionFile.AcquisitionSearchProperties.Address = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "Address");
                acquisitionFile.AcquisitionSearchProperties.PlanNumber = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "PlanNumber");
                acquisitionFile.AcquisitionSearchProperties.LegalDescription = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "LegalDescription");
                acquisitionFile.AcquisitionSearchProperties.MultiplePIDS = genericSteps.PopulateLists(ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "MultiplePIDS"));
            }

            //Acquisition's Properties' Takes
            acquisitionFile.TakesStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "TakesStartRow"));
            acquisitionFile.TakesCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "TakesCount"));
            if (acquisitionFile.TakesStartRow != 0 && acquisitionFile.TakesCount != 0)
                PopulateTakesCollection(acquisitionFile.TakesStartRow, acquisitionFile.TakesCount);

            //Acquisition File Checklist
            acquisitionFile.AcquisitionFileChecklistIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionFileChecklistIndex"));
            if (acquisitionFile.AcquisitionFileChecklistIndex > 0)
            {
                DataTable acquisitionFileChecklistSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionChecklist"]!;
                ExcelDataContext.PopulateInCollection(acquisitionFileChecklistSheet);

                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "FileInitiationSelect1");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "FileInitiationSelect2");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "FileInitiationSelect3");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "FileInitiationSelect4");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "FileInitiationSelect5");

                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect1");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect2");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect3");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect4");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect5");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect6 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect6");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect7 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect7");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect8 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect8");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect9 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect9");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect10 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect10");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect11 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect11");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect12 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect12");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect13 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect13");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect14 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect14");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect15 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect15");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect16 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect16");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect17 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "ActiveFileManagementSelect17");

                acquisitionFile.AcquisitionFileChecklist.CrownLandSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "CrownLandSelect1");
                acquisitionFile.AcquisitionFileChecklist.CrownLandSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "CrownLandSelect2");
                acquisitionFile.AcquisitionFileChecklist.CrownLandSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "CrownLandSelect3");

                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect1");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect2");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect3");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect4");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect5");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect6 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect6");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect7 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect7");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect8 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect8");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect9 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section3AgreementSelect9");

                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect1");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect2");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect3");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect4");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect5");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect6 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect6");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect7 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect7");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect8 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect8");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect9 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect9");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect10 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect10");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect11 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect11");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect12 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "Section6ExpropriationSelect12");

                acquisitionFile.AcquisitionFileChecklist.AcquisitionCompletionSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcquisitionCompletionSelect1");
            }

            //Acquisition Agreements
            acquisitionFile.AgreementStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "AgreementStartRow"));
            acquisitionFile.AgreementCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "AgreementCount"));
            if (acquisitionFile.AgreementStartRow != 0 && acquisitionFile.AgreementCount != 0)
                PopulateAgreementsCollection(acquisitionFile.AgreementStartRow, acquisitionFile.AgreementCount);

            //Acquisition Stakeholders
            acquisitionFile.StakeholderStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "StakeholderStartRow"));
            acquisitionFile.StakeholderCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "StakeholderCount"));
            if (acquisitionFile.StakeholderStartRow != 0 && acquisitionFile.StakeholderCount != 0)
                PopulateStakeholdersCollection(acquisitionFile.StakeholderStartRow, acquisitionFile.StakeholderCount);

            //Acquisition Compensation Requisition
            acquisitionFile.CompensationStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "CompensationStartRow"));
            acquisitionFile.CompensationCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "CompensationCount"));
            acquisitionFile.CompensationTotalAllowableAmount = ExcelDataContext.ReadData(rowNumber, "CompensationTotalAllowableAmount");
            if (acquisitionFile.CompensationStartRow != 0 && acquisitionFile.CompensationCount != 0)
                PopulateCompensationsCollection(acquisitionFile.CompensationStartRow, acquisitionFile.CompensationCount);

            //Acquisition Expropriation
            acquisitionFile.ExpropriationStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "ExpropriationStartRow"));
            acquisitionFile.ExpropriationCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ExpropriationCount"));
            if (acquisitionFile.ExpropriationStartRow != 0 && acquisitionFile.ExpropriationCount != 0)
                PopulateExpropriationCollection(acquisitionFile.ExpropriationStartRow, acquisitionFile.ExpropriationCount);
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

                acquisitionFile.AcquisitionTeam.Add(teamMember);
            }
        }

        private void PopulateOwnersCollection(int startRow, int rowsCount)
        {
            DataTable ownersSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionOwners"]!;
            ExcelDataContext.PopulateInCollection(ownersSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                AcquisitionOwner owner = new AcquisitionOwner();
                owner.OwnerContactType = ExcelDataContext.ReadData(i, "OwnerContactType");
                owner.OwnerIsPrimary = bool.Parse(ExcelDataContext.ReadData(i, "OwnerIsPrimary"));
                owner.OwnerGivenNames = ExcelDataContext.ReadData(i, "OwnerGivenNames");
                owner.OwnerLastName = ExcelDataContext.ReadData(i, "OwnerLastName");
                owner.OwnerOtherName = ExcelDataContext.ReadData(i, "OwnerOtherName");
                owner.OwnerCorporationName = ExcelDataContext.ReadData(i, "OwnerCorporationName");
                owner.OwnerIncorporationNumber = ExcelDataContext.ReadData(i, "OwnerIncorporationNumber");
                owner.OwnerRegistrationNumber = ExcelDataContext.ReadData(i, "OwnerRegistrationNumber");
                owner.OwnerMailAddress.AddressLine1 = ExcelDataContext.ReadData(i, "OwnerMailAddressLine1");
                owner.OwnerMailAddress.AddressLine2 = ExcelDataContext.ReadData(i, "OwnerMailAddressLine2");
                owner.OwnerMailAddress.AddressLine3 = ExcelDataContext.ReadData(i, "OwnerMailAddressLine3");
                owner.OwnerMailAddress.City = ExcelDataContext.ReadData(i, "OwnerMailCity");
                owner.OwnerMailAddress.Province = ExcelDataContext.ReadData(i, "OwnerMailProvince");
                owner.OwnerMailAddress.Country = ExcelDataContext.ReadData(i, "OwnerMailCountry");
                owner.OwnerMailAddress.OtherCountry = ExcelDataContext.ReadData(i, "OwnerMailOtherCountry");
                owner.OwnerMailAddress.PostalCode = ExcelDataContext.ReadData(i, "OwnerMailPostalCode");
                owner.OwnerEmail = ExcelDataContext.ReadData(i, "OwnerEmail");
                owner.OwnerPhone = ExcelDataContext.ReadData(i, "OwnerPhone");

                acquisitionFile.AcquisitionOwners.Add(owner);
            }
        }

        private void PopulateTakesCollection(int startRow, int rowsCount)
        {
            DataTable takeSheet = ExcelDataContext.GetInstance().Sheets["Takes"]!;
            ExcelDataContext.PopulateInCollection(takeSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Take take = new Take();

                take.TakeType = ExcelDataContext.ReadData(i, "TakeType");
                take.TakeStatus = ExcelDataContext.ReadData(i, "TakeStatus");
                take.SiteContamination = ExcelDataContext.ReadData(i, "SiteContamination");
                take.TakeDescription = ExcelDataContext.ReadData(i, "TakeDescription");

                take.IsNewHighwayDedication = ExcelDataContext.ReadData(i, "IsNewHighwayDedication");
                take.IsNewHighwayDedicationArea = ExcelDataContext.ReadData(i, "IsNewHighwayDedicationArea");
                take.IsMotiInventory = ExcelDataContext.ReadData(i, "IsMotiInventory");

                take.IsNewInterestLand = ExcelDataContext.ReadData(i, "IsNewInterestLand");
                take.IsNewInterestLandArea = ExcelDataContext.ReadData(i, "IsNewInterestLandArea");
                take.IsNewInterestLandDate = ExcelDataContext.ReadData(i, "IsNewInterestLandDate");

                take.IsLandActTenure = ExcelDataContext.ReadData(i, "IsLandActTenure");
                take.IsLandActTenureDetail = ExcelDataContext.ReadData(i, "IsLandActTenureDetail");
                take.IsLandActTenureArea = ExcelDataContext.ReadData(i, "IsLandActTenureArea");
                take.IsLandActTenureDate = ExcelDataContext.ReadData(i, "IsLandActTenureDate");

                take.IsLicenseConstruct = ExcelDataContext.ReadData(i, "IsLicenseConstruct");
                take.IsLicenseConstructArea = ExcelDataContext.ReadData(i, "IsLicenseConstructArea");
                take.IsLicenseConstructDate = ExcelDataContext.ReadData(i, "IsLicenseConstructDate");

                take.IsSurplus = ExcelDataContext.ReadData(i, "IsSurplus");
                take.IsSurplusArea = ExcelDataContext.ReadData(i, "IsSurplusArea");

                take.FromProperty = int.Parse(ExcelDataContext.ReadData(i, "FromProperty"));
                take.TakeCounter = int.Parse(ExcelDataContext.ReadData(i, "TakeCounter"));

                acquisitionFile.AcquisitionTakes.Add(take);
            }
        }

        private void PopulateAgreementsCollection(int startRow, int rowsCount)
        {
            DataTable agreementSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionAgreement"]!;
            ExcelDataContext.PopulateInCollection(agreementSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                AcquisitionAgreement agreement = new AcquisitionAgreement();

                agreement.AgreementStatus = ExcelDataContext.ReadData(i, "AgreementStatus");
                agreement.AgreementCancellationReason = ExcelDataContext.ReadData(i, "AgreementCancellationReason");
                agreement.AgreementLegalSurveyPlan = ExcelDataContext.ReadData(i, "AgreementLegalSurveyPlan");
                agreement.AgreementType = ExcelDataContext.ReadData(i, "AgreementType");
                agreement.AgreementDate = ExcelDataContext.ReadData(i, "AgreementDate");
                agreement.AgreementCommencementDate = ExcelDataContext.ReadData(i, "AgreementCommencementDate");
                agreement.AgreementCompletionDate = ExcelDataContext.ReadData(i, "AgreementCompletionDate");
                agreement.AgreementTerminationDate = ExcelDataContext.ReadData(i, "AgreementTerminationDate");
                agreement.AgreementPossessionDate = ExcelDataContext.ReadData(i, "AgreementPossessionDate");
                agreement.AgreementPurchasePrice = ExcelDataContext.ReadData(i, "AgreementPurchasePrice");
                agreement.AgreementDepositDue = ExcelDataContext.ReadData(i, "AgreementDepositDue");
                agreement.AgreementDepositAmount = ExcelDataContext.ReadData(i, "AgreementDepositAmount");

                acquisitionFile.AcquisitionAgreements.Add(agreement);
            }
        }

        private void PopulateStakeholdersCollection(int startRow, int rowsCount)
        {
            DataTable stakeholderSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionStakeholder"]!;
            ExcelDataContext.PopulateInCollection(stakeholderSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                AcquisitionStakeholder stakeholder = new AcquisitionStakeholder();

                stakeholder.StakeholderType = ExcelDataContext.ReadData(i, "StakeholderType");
                stakeholder.InterestHolder = ExcelDataContext.ReadData(i, "InterestHolder");
                stakeholder.InterestType = ExcelDataContext.ReadData(i, "InterestType");
                stakeholder.StakeholderContactType = ExcelDataContext.ReadData(i, "StakeholderContactType");
                stakeholder.PrimaryContact = ExcelDataContext.ReadData(i, "PrimaryContact");
                stakeholder.PayeeName = ExcelDataContext.ReadData(i, "PayeeName");
                stakeholder.StakeholderIndex = int.Parse(ExcelDataContext.ReadData(i, "StakeholderIndex"));

                acquisitionFile.AcquisitionStakeholders.Add(stakeholder);
            }
        }

        private void PopulateCompensationsCollection(int startRow, int rowsCount)
        {
            DataTable compensationSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionCompensation"]!;
            ExcelDataContext.PopulateInCollection(compensationSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                AcquisitionCompensation compensation = new AcquisitionCompensation();

                compensation.CompensationAmount = ExcelDataContext.ReadData(i, "CompensationAmount");
                compensation.CompensationGSTAmount = ExcelDataContext.ReadData(i, "CompensationGSTAmount");
                compensation.CompensationTotalAmount = ExcelDataContext.ReadData(i, "CompensationTotalAmount");
                compensation.CompensationStatus = ExcelDataContext.ReadData(i, "CompensationStatus");
                compensation.CompensationAlternateProject = ExcelDataContext.ReadData(i, "CompensationAlternateProject");
                compensation.CompensationAgreementDate = ExcelDataContext.ReadData(i, "CompensationAgreementDate");
                compensation.CompensationExpropriationNoticeDate = ExcelDataContext.ReadData(i, "CompensationExpropriationNoticeDate");
                compensation.CompensationExpropriationVestingDate = ExcelDataContext.ReadData(i, "CompensationExpropriationVestingDate");
                compensation.CompensationAdvancedPaymentDate = ExcelDataContext.ReadData(i, "CompensationAdvancedPaymentDate");
                compensation.CompensationSpecialInstructions = ExcelDataContext.ReadData(i, "CompensationSpecialInstructions");
                compensation.CompensationFiscalYear = ExcelDataContext.ReadData(i, "CompensationFiscalYear");
                compensation.CompensationSTOB = ExcelDataContext.ReadData(i, "CompensationSTOB");
                compensation.CompensationServiceLine = ExcelDataContext.ReadData(i, "CompensationServiceLine");
                compensation.CompensationResponsibilityCentre = ExcelDataContext.ReadData(i, "CompensationResponsibilityCentre");
                compensation.CompensationPayee = ExcelDataContext.ReadData(i, "CompensationPayee");
                compensation.CompensationPayeeDisplay = ExcelDataContext.ReadData(i, "CompensationPayeeDisplay"); 
                compensation.CompensationPaymentInTrust = Boolean.Parse(ExcelDataContext.ReadData(i, "CompensationPaymentInTrust"));
                compensation.CompensationGSTNumber = ExcelDataContext.ReadData(i, "CompensationGSTNumber");
                compensation.CompensationDetailedRemarks = ExcelDataContext.ReadData(i, "CompensationDetailedRemarks");
                compensation.ActivitiesStartRow = int.Parse(ExcelDataContext.ReadData(i, "ActivitiesStartRow"));
                compensation.ActivitiesCount = int.Parse(ExcelDataContext.ReadData(i, "ActivitiesCount"));

                if (compensation.ActivitiesStartRow != 0 && compensation.ActivitiesCount != 0)
                {
                    PopulateActivitiesCollection(compensation.ActivitiesStartRow, compensation.ActivitiesCount, compensation.CompensationActivities);
                }

                acquisitionFile.AcquisitionCompensations.Add(compensation);
            }
        }

        private void PopulateActivitiesCollection(int startRow, int rowsCount, List<CompensationActivity> activities)
        {
            DataTable activitiesSheet = ExcelDataContext.GetInstance().Sheets["CompensationActivities"]!;
            ExcelDataContext.PopulateInCollection(activitiesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                CompensationActivity activity = new CompensationActivity();

                activity.ActCodeDescription = ExcelDataContext.ReadData(i, "ActCodeDescription");
                activity.ActAmount = ExcelDataContext.ReadData(i, "ActAmount");
                activity.ActGSTEligible = ExcelDataContext.ReadData(i, "ActGSTEligible");
                activity.ActGSTAmount = ExcelDataContext.ReadData(i, "ActGSTAmount");
                activity.ActTotalAmount = ExcelDataContext.ReadData(i, "ActTotalAmount");

                activities.Add(activity);
            }
        }

        private void PopulateExpropriationCollection(int startRow, int rowsCount)
        {
            DataTable expropriationSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionExpropriationForm8"]!;
            ExcelDataContext.PopulateInCollection(expropriationSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                AcquisitionExpropriationForm8 expropriation = new AcquisitionExpropriationForm8();

                expropriation.Form8Payee = ExcelDataContext.ReadData(i, "Form8Payee");
                expropriation.Form8PayeeDisplay = ExcelDataContext.ReadData(i, "Form8PayeeDisplay");
                expropriation.Form8ExpropriationAuthority = ExcelDataContext.ReadData(i, "Form8ExpropriationAuthority");
                expropriation.Form8Description = ExcelDataContext.ReadData(i, "Form8Description");
                expropriation.ExpPaymentStartRow = int.Parse(ExcelDataContext.ReadData(i, "ExpPaymentStartRow"));
                expropriation.ExpPaymentCount = int.Parse(ExcelDataContext.ReadData(i, "ExpPaymentCount"));
                
                if (expropriation.ExpPaymentStartRow != 0 && expropriation.ExpPaymentCount != 0)
                {
                    PopulateExpropPaymentsCollection(expropriation.ExpPaymentStartRow, expropriation.ExpPaymentCount, expropriation.ExpropriationPayments);
                }

                acquisitionFile.AcquisitionExpropriationForm8s.Add(expropriation);
            }
        }

        private void PopulateExpropPaymentsCollection(int startRow, int rowsCount, List<ExpropriationPayment> payments)
        {
            DataTable paymentsSheet = ExcelDataContext.GetInstance().Sheets["ExpropriationPayment"]!;
            ExcelDataContext.PopulateInCollection(paymentsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                ExpropriationPayment payment = new ExpropriationPayment();

                payment.ExpPaymentItem = ExcelDataContext.ReadData(i, "ExpPaymentItem");
                payment.ExpPaymentAmount = ExcelDataContext.ReadData(i, "ExpPaymentAmount");
                payment.ExpPaymentGSTApplicable = ExcelDataContext.ReadData(i, "ExpPaymentGSTApplicable");
                payment.ExpPaymentGSTAmount = ExcelDataContext.ReadData(i, "ExpPaymentGSTAmount");
                payment.ExpPaymentTotalAmount = ExcelDataContext.ReadData(i, "ExpPaymentTotalAmount");

                payments.Add(payment);
            }
        }
    }
}
