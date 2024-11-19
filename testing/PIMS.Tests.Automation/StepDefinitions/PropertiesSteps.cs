﻿using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class PropertiesSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly SearchProperties searchProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly PropertyManagementTab propertyManagementTab;
        private readonly PropertyPIMSFiles pimsFiles;
        private readonly SharedPagination sharedPagination;

        private readonly GenericSteps genericSteps;

        private readonly string userName = "TRANPSP1";

        private Property property;
        private SearchProperty searchProperty;
        private PropertyManagement propertyManagement;

        public PropertiesSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            propertyManagementTab = new PropertyManagementTab(driver.Current);
            pimsFiles = new PropertyPIMSFiles(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);
            genericSteps = new GenericSteps(driver);

            property = new Property();
            searchProperty = new SearchProperty();
            propertyManagement = new PropertyManagement();

        }

        [StepDefinition(@"I search for a Property in the Map by different filters from row number (.*)")]
        public void SearchInventoryPropertyOnMap(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-1546, PSP-5090, PSP-5091, PSP-5092, PSP-6693 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a valid Address with the Search Bar
            PopulateSearchProperty(rowNumber);
            searchProperties.SearchPropertyByAddressMap(searchProperty.Address);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesMapFoundCount() == 1);

            //Search for a valid Plan in Inventory
            searchProperties.SearchPropertyReset();
            searchProperties.SearchPropertyByPlan(searchProperty.PlanNumber);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesClustersFoundCount() == 1);

            //Search for a valid PIN in Inventory
            searchProperties.SearchPropertyReset();
            searchProperties.SearchPropertyByPINPID(searchProperty.PIN);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesMapFoundCount() == 1);

            //Search for a valid PID in Inventory
            searchProperties.SearchPropertyReset();
            searchProperties.SearchPropertyByPINPID(searchProperty.PID);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesMapFoundCount() == 1);
        }

        [StepDefinition(@"I search for an Invalid Property from row number (.*)")]
        public void SearchInvalidPropertyOnMap(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5004 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for an invalid Address with the Search Bar
            PopulateSearchProperty(rowNumber);
            searchProperties.SearchPropertyByPINPID(searchProperty.PID);
        }

        [StepDefinition(@"I review a Property's Information from row number (.*)")]
        public void ReviewPropertyInformation(int rowNumber)
        {
            /* TEST COVERAGE: PSP-1558, PSP-3153, PSP-3184, PSP-3589, PSP-4903, PSP-5163 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to the Inventory List View
            PopulateProperty(rowNumber);
            searchProperties.NavigatePropertyListView();

            //Search for a property
            searchProperties.SearchPropertyByPINPID(property.PID);

            //Select the first property from the list
            searchProperties.ChooseFirstPropertyFromList();

            //Validate Title Tab
            propertyInformation.NavigatePropertyTitleTab();
            //propertyInformation.VerifyTitleTab();

            //Validate Value Tab
            propertyInformation.NavigatePropertyValueTab();
            //propertyInformation.VerifyValueTab();

            //Validate the Property Details View
            propertyInformation.NavigatePropertyDetailsTab();
            propertyInformation.VerifyPropertyDetailsView();
        }

        [StepDefinition(@"I search for a Property in the Properties List by different filters from row number (.*)")]
        public void ReviewPropertyInformationList(int rowNumber)
        {
            /* TEST COVERAGE: PSP-1558, PSP-3153, PSP-3184, PSP-3589, PSP-4903, PSP-4905, PSP-5163, PSP-7815 */

            //Navigate to the Inventory List View
            PopulateSearchProperty(rowNumber);
            searchProperties.NavigatePropertyListView();

            //Validate List View Elements
            searchProperties.SearchPropertyReset();
            searchProperties.ValidatePropertyListView();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchProperties.PropertiesListFoundCount());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchProperties.PropertiesListFoundCount());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, searchProperties.PropertiesListFoundCount());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, searchProperties.PropertiesListFoundCount());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, searchProperties.PropertiesListFoundCount());

            //Verify Column Sorting by Location
            sharedPagination.ChoosePaginationOption(10);
            searchProperties.OrderByPropertyLocation();
            var firstLocationDescResult = searchProperties.FirstPropertyLocation();

            searchProperties.OrderByPropertyLocation();
            var firstLocationAscResult = searchProperties.FirstPropertyLocation();

            Assert.NotEqual(firstLocationDescResult, firstLocationAscResult);

            //Verify Column Sorting by Lot Size
            searchProperties.OrderByPropertyLotSize();
            var firstLotSizeDescResult = searchProperties.FirstPropertyLotSize();

            searchProperties.OrderByPropertyLotSize();
            var firstLotSizeAscResult = searchProperties.FirstPropertyLotSize();

            Assert.NotEqual(firstLotSizeDescResult, firstLotSizeAscResult);

            //Verify Column Sorting by Ownership
            searchProperties.OrderByPropertyOwnership();
            var firstOwnershipDescResult = searchProperties.FirstPropertyOwnership();

            searchProperties.OrderByPropertyOwnership();
            var firstOwnershipAscResult = searchProperties.FirstPropertyOwnership();

            Assert.NotEqual(firstOwnershipDescResult, firstOwnershipAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstPropertyPage1 = searchProperties.FirstPropertyPID();
            sharedPagination.GoNextPage();
            var firstPropertyPage2 = searchProperties.FirstPropertyPID();
            Assert.NotEqual(firstPropertyPage1, firstPropertyPage2);

            sharedPagination.ResetSearch();

            //Search for a valid Address with the Search Bar
            searchProperties.SearchPropertyByAddressList(searchProperty.Address);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesListFoundCount() >= 1);

            //Search for a valid PIN in Inventory
            searchProperties.SearchPropertyReset();
            searchProperties.SearchPropertyByPINPID(searchProperty.PIN);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesListFoundCount() == 1);

            //Search for a valid PID in Inventory
            searchProperties.SearchPropertyReset();
            searchProperties.SearchPropertyByPINPID(searchProperty.PID);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesListFoundCount() == 1);

            //Search for a valid Plan in Inventory
            searchProperties.SearchPropertyReset();
            searchProperties.SearchPropertyByPlan(searchProperty.PlanNumber);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesListFoundCount() == 1);
        }

        [StepDefinition(@"I search for a non MOTI property from row number (.*)")]
        public void NonInventoryProperty(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3414 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a non-inventory property
            PopulateSearchProperty(rowNumber);
            searchProperties.SearchPropertyByAddressMap(searchProperty.Address);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesMapFoundCount() == 1);

            //Click on the founf property
            searchProperties.SelectFoundPin();
        }

        [StepDefinition(@"I search for a property in the inventory by PID from row number (.*)")]
        public void InventoryProperty(int rowNumber)
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a non-inventory property
            PopulateSearchProperty(rowNumber);
            searchProperties.SearchPropertyByPINPID(searchProperty.PID);

            //Click on the found property
            searchProperties.SelectFoundPin();
        }

        [StepDefinition(@"I update a Property details from a file from row number (.*)")]
        public void EditPropertyInformationDetailsFromFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3460, PSP-3462, PSP-3590, PSP-3591, PSP-3599, PSP-3600, PSP-3612, PSP-3722, PSP-4791, PSP-4794, PSP-5162, PSP-5163, PSP-5164, PSP-5165 */

            //Populate Property Information
            PopulateProperty(rowNumber);

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoBttn();

            //Verify Property Information Edit Form
            propertyInformation.VerifyPropertyDetailsEditForm();

            //Apply changes on the Property Information Form
            propertyInformation.UpdatePropertyDetails(property);

            //Cancel changes
            propertyInformation.CancelPropertyDetails();

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoBttn();

            //Apply changes on the Property Information Form
            propertyInformation.UpdatePropertyDetails(property);

            //Save changes
            propertyInformation.SavePropertyDetails();
        }

        [StepDefinition(@"I update a Property details")]
        public void EditPropertyInformationDetails()
        {
            /* TEST COVERAGE: PSP-3460, PSP-3462, PSP-3590, PSP-3591, PSP-3599, PSP-3600, PSP-3612, PSP-3722, PSP-4791, PSP-4794, PSP-5162, PSP-5163, PSP-5164, PSP-5165 */

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoBttn();

            //Verify Property Information Edit Form
            propertyInformation.VerifyPropertyDetailsEditForm();

            //Apply changes on the Property Information Form
            propertyInformation.UpdatePropertyDetails(property);

            //Cancel changes
            propertyInformation.CancelPropertyDetails();

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoBttn();

            //Apply changes on the Property Information Form
            propertyInformation.UpdatePropertyDetails(property);

            //Save changes
            propertyInformation.SavePropertyDetails();
        }

        [StepDefinition(@"I insert information in the Property Management Tab from row number (.*)")]
        public void InsertManagementPropertyTab(int rowNumber)
        {
            //Grab data from excel
            PopulateManagementProperty(rowNumber);

            //Go to the Property Management Tab
            propertyManagementTab.NavigateManagementTab();
            propertyManagementTab.VerifyInitManagementTabView();

            //Click on Edit Summary
            propertyManagementTab.UpdateManagementSummaryButton();

            //Insert and cancel Summary Information
            propertyManagementTab.InsertManagementSummaryInformation(propertyManagement);
            propertyManagementTab.CancelPropertyManagement();

            //Click on Edit Summary
            propertyManagementTab.UpdateManagementSummaryButton();

            //Insert and save Summary Information
            propertyManagementTab.VerifyCreateSummaryInitForm();
            propertyManagementTab.InsertManagementSummaryInformation(propertyManagement);
            propertyManagementTab.SavePropertyManagement();
            propertyManagementTab.VerifyInsertedSummaryForm(propertyManagement);   

            //Insert Contacts
            for (int i = 0; i < propertyManagement.ManagementPropertyContacts.Count; i++)
            {
                if (i == 0)
                {
                    //Checking Initial Contact form and cancel changes
                    propertyManagementTab.AddNewPropertyContactButton();
                    propertyManagementTab.VerifyCreateContactsInitForm();
                    propertyManagementTab.InsertNewPropertyContact(propertyManagement.ManagementPropertyContacts[i]);
                    propertyManagementTab.CancelPropertyManagement();
                }

                propertyManagementTab.AddNewPropertyContactButton();
                propertyManagementTab.InsertNewPropertyContact(propertyManagement.ManagementPropertyContacts[i]);
                propertyManagementTab.SavePropertyManagement();
                propertyManagementTab.VerifyLastInsertedPropertyContactTable(propertyManagement.ManagementPropertyContacts[i]);
            }

            //Insert Activities
            for (int j = 0; j < propertyManagement.ManagementPropertyActivities.Count; j++)
            {
                if (j == 0)
                {
                    //Checking Initial Activity form and cancel changes
                    propertyManagementTab.AddNewPropertyActivityButton();
                    propertyManagementTab.VerifyCreateActivityInitForm();
                    propertyManagementTab.InsertNewPropertyActivity(propertyManagement.ManagementPropertyActivities[j]);
                    propertyManagementTab.CancelPropertyManagement();
                }

                propertyManagementTab.AddNewPropertyActivityButton();
                propertyManagementTab.InsertNewPropertyActivity(propertyManagement.ManagementPropertyActivities[j]);
                propertyManagementTab.SavePropertyManagement();
                propertyManagementTab.VerifyInsertedActivity(propertyManagement.ManagementPropertyActivities[j]);
                propertyManagementTab.ViewLastActivityFromList();
                propertyManagementTab.VerifyLastInsertedActivityTable(propertyManagement.ManagementPropertyActivities[j]);
            }
        }

        [StepDefinition(@"I insert activities to the Property Management Tab from row number (.*)")]
        public void InsertActivityManagementPropertyTab(int rowNumber)
        {
            //Grab data from excel
            PopulateManagementProperty(rowNumber);

            //Go to the Property Management Tab
            propertyManagementTab.NavigateManagementTab();
            //propertyManagementTab.VerifyInitManagementTabView();

            //Insert Activities
            for (int j = 0; j < propertyManagement.ManagementPropertyActivities.Count; j++)
            {
                propertyManagementTab.AddNewPropertyActivityButton();
                propertyManagementTab.VerifyCreateActivityInitForm();
                propertyManagementTab.InsertNewPropertyActivity(propertyManagement.ManagementPropertyActivities[j]);
                propertyManagementTab.SavePropertyManagement();
                propertyManagementTab.VerifyInsertedActivity(propertyManagement.ManagementPropertyActivities[j]);
                propertyManagementTab.ViewLastActivityFromList();
                propertyManagementTab.VerifyLastInsertedActivityTable(propertyManagement.ManagementPropertyActivities[j]);
            }

        }

        [StepDefinition(@"I update information in the Property Management Tab from row number (.*)")]
        public void UpdateManagementPropertyTab(int rowNumber)
        {
            //Grab data from excel
            PopulateManagementProperty(rowNumber);

            //Close Activity Tray
            propertyManagementTab.CloseActivityTray();

            //Update Summary section
            propertyManagementTab.UpdateManagementSummaryButton();
            propertyManagementTab.InsertManagementSummaryInformation(propertyManagement);
            propertyManagementTab.SavePropertyManagement();
            propertyManagementTab.VerifyInsertedSummaryForm(propertyManagement);

            //Update a Contact
            propertyManagementTab.UpdateLastContactButton();
            propertyManagementTab.UpdatePropertyContact(propertyManagement.ManagementPropertyContacts[0]);
            propertyManagementTab.SavePropertyManagement();
            propertyManagementTab.VerifyLastInsertedPropertyContactTable(propertyManagement.ManagementPropertyContacts[0]);

            //Update an activity
            propertyManagementTab.ViewLastActivityFromList();
            propertyManagementTab.ViewLastActivityButton();
            propertyManagementTab.UpdateSelectedActivity();
            propertyManagementTab.InsertNewPropertyActivity(propertyManagement.ManagementPropertyActivities[0]);
            propertyManagementTab.SavePropertyManagement();
            propertyManagementTab.VerifyInsertedActivity(propertyManagement.ManagementPropertyActivities[0]);
            propertyManagementTab.ViewLastActivityFromList();
            propertyManagementTab.VerifyLastInsertedActivityTable(propertyManagement.ManagementPropertyActivities[0]);

        }

        [StepDefinition(@"I clean up the Property Management Tab from row number (.*)")]
        public void CleanUpManagementPropertyTab(int rowNumber)
        {
            //Grab data from excel
            PopulateManagementProperty(rowNumber);

            //Close Activity Tray
            propertyManagementTab.CloseActivityTray();

            //Clean up Summary section
            propertyManagementTab.UpdateManagementSummaryButton();
            propertyManagementTab.InsertManagementSummaryInformation(propertyManagement);
            propertyManagementTab.SavePropertyManagement();

            //Delete all Contacts
            propertyManagementTab.DeleteAllContacts();

            //Delete all Activities
            propertyManagementTab.DeleteAllActivities();
        }

        [StepDefinition(@"I delete all activities from the Property Management Tab")]
        public void DeleteActivitiesManagementPropertyTab()
        {
            //Close Activity Tray
            propertyManagementTab.CloseActivityTray();

            //Delete all Activities
            propertyManagementTab.DeleteAllActivities();
        }

        [StepDefinition(@"I verify the PIMS Files Tab")]
        public void VerifyPIMSFilesTab()
        {
            pimsFiles.NavigatePIMSFiles();
            pimsFiles.VerifyPimsFiles();
        }

        [StepDefinition(@"No Properties were found")]
        public void NonPropertyFound()
        {
            /* TEST COVERAGE: PSP-1548 */

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesMapFoundCount() == 0);
            searchProperties.Dispose();
        }

        [StepDefinition(@"A Property Information is saved successfully")]
        public void PropertyInfoSucess()
        {
            //Validate Property Information View Form after changes
            propertyInformation.NavigatePropertyDetailsTab();
            propertyInformation.VerifyPropertyInformationHeader(true);
            propertyInformation.VerifyUpdatePropertyDetailsView(property);
            propertyInformation.Dispose();
        }

        [StepDefinition(@"Non-Inventory property renders correctly")]
        public void NonInventoryPropertySucess()
        {
            //Validate tabs counting
            Assert.Equal(2, propertyInformation.PropertyTabs());

            //Validate correct tabs are displayed
            propertyInformation.VerifyNonInventoryPropertyTabs();
            propertyInformation.Dispose();
        }

        [StepDefinition(@"Property Management Tab has been updated successfully")]
        public void PropertyManagementSuccess()
        {
            propertyManagementTab.VerifyInitManagementTabView();
            propertyManagementTab.Dispose();
        }

        [StepDefinition(@"PIMS Files Tab has rendered successfully")]
        public void VerifySuccessPIMSFile()
        {
            Assert.True(pimsFiles.GetResearchFilesCount() > 0);
            Assert.True(pimsFiles.GetAcquisitionFilesCount() > 0);
            Assert.True(pimsFiles.GetLeasesCount() > 0);
            Assert.True(pimsFiles.GetDispositionFilesCount() > 0);

            pimsFiles.Dispose();
        }

        [StepDefinition(@"Properties filters works successfully")]
        public void PropertySearchBarSuccess()
        {
            searchProperties.SearchPropertyReset();
            searchProperties.Dispose();
        }

        private void PopulateProperty(int rowNumber)
        {
            DataTable propertiesSheet = ExcelDataContext.GetInstance().Sheets["Properties"]!;
            ExcelDataContext.PopulateInCollection(propertiesSheet);

            property = new Property();

            property.PID = ExcelDataContext.ReadData(rowNumber, "PropertyPID");
            property.PlanNumber = ExcelDataContext.ReadData(rowNumber, "PropertyPlanNumber");
            property.PropertyName = ExcelDataContext.ReadData(rowNumber, "PropertyName");
            property.Address.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine1");
            property.Address.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine2");
            property.Address.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine3");
            property.Address.City = ExcelDataContext.ReadData(rowNumber, "PropertyCity");
            property.Address.PostalCode = ExcelDataContext.ReadData(rowNumber, "PropertyPostalCode");
            property.GeneralLocation = ExcelDataContext.ReadData(rowNumber, "PropertyGeneralLocation");

            property.HistoricalFileStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyHistoricalFileStartRow"));
            property.HistoricalFileCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyHistoricalFileCount"));
            if (property.HistoricalFileStartRow != 0 && property.HistoricalFileCount != 0)
                PopulateHistoricalFiles(property.HistoricalFileStartRow, property.HistoricalFileCount);

            property.LegalDescription = ExcelDataContext.ReadData(rowNumber, "PropertyLegalDescription");
            property.MOTIRegion = ExcelDataContext.ReadData(rowNumber, "PropertyMoTIRegion");
            property.HighwaysDistrict = ExcelDataContext.ReadData(rowNumber, "PropertyHighwaysDistrict");
            property.ElectoralDistrict = ExcelDataContext.ReadData(rowNumber, "PropertyElectoralDistrict");
            property.AgriculturalLandReserve = ExcelDataContext.ReadData(rowNumber, "PropertyAgriculturalLandReserve");
            property.RailwayBelt = ExcelDataContext.ReadData(rowNumber, "PropertyRailwayBelt");
            property.LandParcelType = ExcelDataContext.ReadData(rowNumber, "PropertyLandParcelType");
            property.MunicipalZoning = ExcelDataContext.ReadData(rowNumber, "PropertyMunicipalZoning");
            property.Anomalies = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "PropertyAnomalies"));

            property.TenureStatus = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "PropertyTenureStatus"));
            property.ProvincialPublicHwy = ExcelDataContext.ReadData(rowNumber, "PropertyProvincialPublicHwy");
            property.HighwayEstablishedBy = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "PropertyHighwayEstablishedBy"));
            property.SqrMeters = ExcelDataContext.ReadData(rowNumber, "PropertySqrMeters");
            property.IsVolumetric = bool.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyIsVolumetric"));
            property.Volume = ExcelDataContext.ReadData(rowNumber, "PropertyVolume");
            property.VolumeType = ExcelDataContext.ReadData(rowNumber, "PropertyVolumeType");
            property.PropertyNotes = ExcelDataContext.ReadData(rowNumber, "PropertyNotes");
        }

        private void PopulateHistoricalFiles(int startRow, int rowsCount)
        {
            DataTable propertyHistoricalFilesSheet = ExcelDataContext.GetInstance().Sheets["PropertiesHistoricalFile"]!;
            ExcelDataContext.PopulateInCollection(propertyHistoricalFilesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                HistoricalFile historicalFile = new HistoricalFile();
                historicalFile.HistoricalFileNumber = ExcelDataContext.ReadData(i, "PropertyHistoricalFileNumber");
                historicalFile.HistoricalFileType = ExcelDataContext.ReadData(i, "PropertyHistoricalFileType");
                historicalFile.HistoricalFileOtherDetails = ExcelDataContext.ReadData(i, "PropertyHistoricalFileOtherDetails");

                property.PropertyHistoricalFiles.Add(historicalFile);
            }
        }

        private void PopulateSearchProperty(int rowNumber)
        {
            DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"]!;
            ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

            searchProperty = new SearchProperty();

            searchProperty.PID = ExcelDataContext.ReadData(rowNumber, "PID");
            searchProperty.PIN = ExcelDataContext.ReadData(rowNumber, "PIN");
            searchProperty.Address = ExcelDataContext.ReadData(rowNumber, "Address");
            searchProperty.PlanNumber = ExcelDataContext.ReadData(rowNumber, "PlanNumber");
            searchProperty.LegalDescription = ExcelDataContext.ReadData(rowNumber, "LegalDescription");
            searchProperty.MultiplePIDS = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "MultiplePIDS"));
        }

        private void PopulateManagementProperty(int rowNumber)
        {
            DataTable propertyManagementSheet = ExcelDataContext.GetInstance().Sheets["PropertyManagement"]!;
            ExcelDataContext.PopulateInCollection(propertyManagementSheet);

            propertyManagement = new PropertyManagement();

            propertyManagement.ManagementPropertyPurpose = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "ManagementPropertyPurpose"));
            propertyManagement.ManagementUtilitiesPayable = ExcelDataContext.ReadData(rowNumber, "ManagementUtilitiesPayable");
            propertyManagement.ManagementTaxesPayable = ExcelDataContext.ReadData(rowNumber, "ManagementTaxesPayable");
            propertyManagement.ManagementPropertyAdditionalDetails = ExcelDataContext.ReadData(rowNumber, "ManagementPropertyAdditionalDetails");
            propertyManagement.ManagementPropertyContactsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementPropertyContactsStartRow"));
            propertyManagement.ManagementPropertyContactsStartCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementPropertyContactsStartCount"));
            propertyManagement.ManagementPropertyActivitiesStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementPropertyActivitiesStartRow"));
            propertyManagement.ManagementPropertyActivitiesCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementPropertyActivitiesCount"));

            if (propertyManagement.ManagementPropertyContactsStartRow != 0 && propertyManagement.ManagementPropertyContactsStartCount != 0)
                PopulateManagementContactsCollection(propertyManagement.ManagementPropertyContactsStartRow, propertyManagement.ManagementPropertyContactsStartCount);

            if (propertyManagement.ManagementPropertyActivitiesStartRow != 0 && propertyManagement.ManagementPropertyActivitiesCount != 0)
                PopulateManagementActivitiesCollection(propertyManagement.ManagementPropertyActivitiesStartRow, propertyManagement.ManagementPropertyActivitiesCount);
        }

        private void PopulateManagementContactsCollection(int startRow, int rowsCount)
        {
            DataTable managementContactsSheet = ExcelDataContext.GetInstance().Sheets["PropertyManagementContact"]!;
            ExcelDataContext.PopulateInCollection(managementContactsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                PropertyContact propertyContact = new PropertyContact();
                propertyContact.PropertyContactFullName = ExcelDataContext.ReadData(i, "PropertyContactFullName");
                propertyContact.PropertyContactType = ExcelDataContext.ReadData(i, "PropertyContactType");
                propertyContact.PropertyPrimaryContact = ExcelDataContext.ReadData(i, "PropertyPrimaryContact");
                propertyContact.PropertyContactPurposeDescription = ExcelDataContext.ReadData(i, "PropertyContactPurposeDescription");

                propertyManagement.ManagementPropertyContacts.Add(propertyContact);
            }
        }

        private void PopulateManagementActivitiesCollection(int startRow, int rowsCount)
        {
            DataTable managementActivitesSheet = ExcelDataContext.GetInstance().Sheets["PropertyManagementActivity"]!;
            ExcelDataContext.PopulateInCollection(managementActivitesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                PropertyActivity propertyActivity = new PropertyActivity();
               
                propertyActivity.PropertyActivityType = ExcelDataContext.ReadData(i, "PropertyActivityType");
                propertyActivity.PropertyActivitySubType = ExcelDataContext.ReadData(i, "PropertyActivitySubType");
                propertyActivity.PropertyActivityStatus = ExcelDataContext.ReadData(i, "PropertyActivityStatus");
                propertyActivity.PropertyActivityRequestedDate = ExcelDataContext.ReadData(i, "PropertyActivityRequestedDate");
                propertyActivity.PropertyActivityDescription = ExcelDataContext.ReadData(i, "PropertyActivityDescription");
                propertyActivity.PropertyActivityMinistryContact = genericSteps.PopulateLists(ExcelDataContext.ReadData(i, "PropertyActivityMinistryContact"));
                propertyActivity.PropertyActivityRequestedSource = ExcelDataContext.ReadData(i, "PropertyActivityRequestedSource");
                propertyActivity.PropertyActivityInvolvedParties = genericSteps.PopulateLists(ExcelDataContext.ReadData(i, "PropertyActivityInvolvedParties"));
                propertyActivity.PropertyActivityServiceProvider = ExcelDataContext.ReadData(i, "PropertyActivityServiceProvider");
                propertyActivity.ManagementPropertyActivityInvoicesStartRow = int.Parse(ExcelDataContext.ReadData(i, "ManagementPropertyActivityInvoicesStartRow"));
                propertyActivity.ManagementPropertyActivityInvoicesCount = int.Parse(ExcelDataContext.ReadData(i, "ManagementPropertyActivityInvoicesCount"));
                propertyActivity.ManagementPropertyActivityTotalPreTax = ExcelDataContext.ReadData(i, "ManagementPropertyActivityTotalPreTax");
                propertyActivity.ManagementPropertyActivityTotalGST = ExcelDataContext.ReadData(i, "ManagementPropertyActivityTotalGST");
                propertyActivity.ManagementPropertyActivityTotalPST = ExcelDataContext.ReadData(i, "ManagementPropertyActivityTotalPST");
                propertyActivity.ManagementPropertyActivityGrandTotal = ExcelDataContext.ReadData(i, "ManagementPropertyActivityGrandTotal");

                if (propertyActivity.ManagementPropertyActivityInvoicesStartRow != 0 && propertyActivity.ManagementPropertyActivityInvoicesCount != 0)
                    PopulateManagementActivitiesInvoiceCollection(propertyManagement.ManagementPropertyActivitiesStartRow, propertyManagement.ManagementPropertyActivitiesCount, propertyActivity.ManagementPropertyActivityInvoices);
                else
                    propertyActivity.ManagementPropertyActivityInvoices = new List<ManagementPropertyActivityInvoice>();

                propertyManagement.ManagementPropertyActivities.Add(propertyActivity);
            }
        }

        private void PopulateManagementActivitiesInvoiceCollection(int startRow, int rowsCount, List<ManagementPropertyActivityInvoice> invoices)
        {
            DataTable invoicesSheet = ExcelDataContext.GetInstance().Sheets["ManagementPropActivityInvoice"]!;
            ExcelDataContext.PopulateInCollection(invoicesSheet);

            invoices = new List<ManagementPropertyActivityInvoice>();

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                ManagementPropertyActivityInvoice invoice = new ManagementPropertyActivityInvoice();

                invoice.PropertyActivityInvoiceNumber = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceNumber");
                invoice.PropertyActivityInvoiceDate = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceDate");
                invoice.PropertyActivityInvoiceDescription = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceDescription");
                invoice.PropertyActivityInvoicePretaxAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoicePretaxAmount");
                invoice.PropertyActivityInvoiceGSTAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceGSTAmount");
                invoice.PropertyActivityInvoicePSTApplicable = ExcelDataContext.ReadData(i, "PropertyActivityInvoicePSTApplicable");
                invoice.PropertyActivityInvoicePSTAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoicePSTAmount");
                invoice.PropertyActivityInvoiceTotalAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceTotalAmount");

                invoices.Add(invoice);
            }
        }
    }
}
