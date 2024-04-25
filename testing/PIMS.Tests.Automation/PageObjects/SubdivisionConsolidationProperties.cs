using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SubdivisionConsolidationProperties : PageObjectBase
    {
        //Subdivision/ Consolidation Files Menu Elements
        private readonly By menuSubdivisionConsolidationButton = By.CssSelector("div[data-testid='nav-tooltip-subdivision&consolidation'] a");
        private readonly By createSubdivisionFileButton = By.XPath("//a[contains(text(),'Create a Subdivision')]");
        private readonly By createConsolidationFileButton = By.XPath("//a[contains(text(),'Create a Consolidation')]");

        //Subdivision Properties Selection Elements
        private readonly By subdivisionCreateTitle = By.XPath("//h1[contains(text(),'Create a Subdivision')]");
        private readonly By subdivisionCreateSubtitle = By.XPath("//h2[contains(text(),'Properties in Subdivision')]");
        private readonly By subdivisionParentInstructionsParagraph = By.XPath("//p[contains(text(),'Select the parent property that was subdivided:')]");

        private readonly By subconParentSearchAnchor = By.CssSelector("a[data-rb-event-key='parent-property']");
        private readonly By subconSearchParentByPIDSelect = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/div/div/select");
        private readonly By subdconSearchParentByPIDInput = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/div/input[@id='input-pid']");
        private readonly By subconSearchParentBttn = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/button[@data-testid='search']");
        private readonly By subconSearchParentResetBttn = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/button[@data-testid='reset-button']");

        private readonly By subdivisionSelectedParentSubtitle = By.XPath("//div[contains(text(),'Selected Parent')]");
        private readonly By subconParentResultIdentifierColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private readonly By subconParentResultPlanColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private readonly By subconParentResultAreaColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private readonly By subconParentResultAddressColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private readonly By subdivisionChildrenInstructionsParagraph = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]");
        private readonly By subconChildrenlocateOnMapTab = By.XPath("//a[contains(text(),'Locate on Map')]");
        private readonly By subconChildrenlocateOnMapSubtitle = By.XPath("//h3[contains(text(), 'Select a property')]");
        private readonly By subconChildrenlocateOnMapBlueIcon = By.Id("Layer_2");
        private readonly By subconChildrenlocateOnMapInstuction1 = By.XPath("//li[contains(text(),'Single-click blue marker above')]");
        private readonly By subconChildrenlocateOnMapInstuction2 = By.XPath("//li[contains(text(),'Mouse to a parcel on the map')]");
        private readonly By subconChildrenlocateOnMapInstuction3 = By.XPath("//li[contains(text(),'Single-click on parcel to select it')]");
        private readonly By subconChildrenlocateOnMapSelectedLabel = By.XPath("//div[contains(text(),'Selected property attributes')]");
        private readonly By subconChildrenlocateOnMapPIDLabel = By.XPath("//label[contains(text(),'PID')]");
        private readonly By subconChildrenlocateOnMapPlanLabel = By.XPath("//label[contains(text(),'Plan #')]");
        private readonly By subconChildrenlocateOnMapAddressLabel = By.XPath("//label[contains(text(),'Address')]");
        private readonly By subconChildrenlocateOnMapRegionLabel = By.XPath("//label[contains(text(),'Region')]");
        private readonly By subconChildrenlocateOnMapDistrictLabel = By.XPath("//label[contains(text(),'District')]");

        private By subconChildrenSearchTab = By.XPath("//a[contains(text(),'Locate on Map')]/following-sibling::a");
        private By subconChildrenSearchByPIDSelect = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/div/div/select");
        private By subconChildrenSearchByPIDInput = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/div/input");
        private By subconChildrenSearchButton = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/button[@data-testid='search']");
        private By subconChildrenResetButton = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/button[@data-testid='reset-button']");
        private By subconChildren1stResultCheckbox = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td']:first-child input");
        private By subconChildernAddToSelectionBttn = By.XPath("//div[contains(text(),'Add to selection')]/parent::button");

        private By subdivisionSelectedChildrenSubtitle = By.XPath("//h2/div/div[contains(text(),'Selected Children')]");
        private By subdivisionChildrenResultIdentifierColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By subdivisionChildrenResultPlanColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private By subdivisionChildrenResultAreaColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private By subdivisionChildrenResultAddressColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private By subdivisionPropertiesCreateButton = By.XPath("//div[contains(text(),'Create Subdivision')]/parent::button");
        private By subconPropertiesCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");

        private By subconModalWindow = By.CssSelector("div[class='modal-content']");
        private By subconWarningHeader = By.XPath("//div[@class='modal-header']/div[contains(text(),'Are you sure?')]");
        private By subconErrorHeader = By.XPath("//div[@class='modal-header']/div[contains(text(),'Error')]");
        private By subconModalSaveWarningP1 = By.CssSelector("div[class='modal-body'] p:first-child");
        private By subconModalSaveWarningP2 = By.CssSelector("div[class='modal-body'] p:nth-child(2)");

        //Consolidation Properties Selection Elements
        private By consolidationCreateTitle = By.XPath("//h1[contains(text(),'Create a Consolidation')]");
        private By consolidationCreateSubtitle = By.XPath("//h2[contains(text(),'Properties in Consolidation')]");
        private By consolidationParentsInstructionsParagraph = By.XPath("//p[contains(text(),'Select two or more parent properties that were consolidated:')]");

        private By consolidationSelectedParentsSubtitle = By.XPath("//div[contains(text(),'Selected Parents')]");
        private By consolidationParentsResultIdentifierColumn = By.XPath("//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By consolidationParentsResultPlanColumn = By.XPath("//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private By consolidationParentsResultAreaColumn = By.XPath("//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private By consolidationParentsResultAddressColumn = By.XPath("//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private By consolidationChildInstructionsParagraph = By.XPath("//p[contains(text(),'Select the child property to which parent properties were consolidated:')]");

        private By consolidationSelectedChildSubtitle = By.XPath("//h2/div/div[contains(text(),'Selected Child')]");
        private By consolidationChildResultIdentifierColumn = By.XPath("//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By consolidationChildResultPlanColumn = By.XPath("//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private By consolidationChildResultAreaColumn = By.XPath("//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private By consolidationChildResultAddressColumn = By.XPath("//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private By consolidationPropertiesCreateButton = By.XPath("//div[contains(text(),'Create Consolidation')]/parent::button");

        private By consolidationChooseParentsErrorMsg = By.XPath("//div[contains(text(),'You must select at least two parent properties')]");
        private By subdivisionChooseChildrenErrorMsg = By.XPath("//div[contains(text(),'You must select at least two child properties')]");

        //Subdivision/Consolidation History Elements
        private By propertyInformationTitle = By.XPath("//h1[contains(text(),'Property Information')]");

        private By subdivisionHistorySubtitle = By.XPath("//div[contains(text(),'Subdivision History')]");
        private By consolidationHistorySubtitle = By.XPath("//div[contains(text(),'Consolidation History')]");
        private By subconHistoryCreatedOnLabel = By.XPath("//label[contains(text(),'Created on')]");
        private By subdivisionHistoryTableParentColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Parent')]");
        private By consolidationHistoryTableChildColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Child')]");
        private By subconHistoryTableIDColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Identifier')]");
        private By subconHistoryTablePlanColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Plan #')]");
        private By subconHistoryTableStatusColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By subconHistoryTableAreaColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Area')]");

        private By subdivisionParentIdentifier = By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(3) a");
        private By subdivisionParentPlan = By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(4)");
        private By subdivisionParentStatus = By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(5)");
        private By subdivisionParentArea = By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(6)");

        private By subconTableContent = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='tbody']/div[@class='tr-wrapper']");


        private SharedModals sharedModals;

        public SubdivisionConsolidationProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToCreateNewSubdivision()
        {
            Wait();
            FocusAndClick(menuSubdivisionConsolidationButton);

            WaitUntilVisible(createSubdivisionFileButton);
            FocusAndClick(createSubdivisionFileButton);
        }

        public void NavigateToCreateNewConsolidation()
        {
            Wait();
            FocusAndClick(menuSubdivisionConsolidationButton);

            WaitUntilVisible(createConsolidationFileButton);
            FocusAndClick(createConsolidationFileButton);
        }

        public void SaveSubdivision()
        {
            WaitUntilClickable(subdivisionPropertiesCreateButton);
            webDriver.FindElement(subdivisionPropertiesCreateButton).Click();

            Wait();
            if (webDriver.FindElements(subconModalWindow).Count > 0)
            {
                WaitUntilVisible(subconWarningHeader);
                AssertTrueContentEquals(subconModalSaveWarningP1, "You are subdividing a property into two or more properties. The old parent property record will be retired, and the new child properties will be created");
                AssertTrueContentEquals(subconModalSaveWarningP2, "If you proceed, you will be redirected to the old parent property record, where you can view changes and make updates to the new properties. Do you want to proceed?");

                sharedModals.ModalClickOKBttn();
            }  
        }

        public void SaveConsolidation()
        {
            WaitUntilClickable(consolidationPropertiesCreateButton);
            webDriver.FindElement(consolidationPropertiesCreateButton).Click();

            Wait();
            if (webDriver.FindElements(subconModalWindow).Count > 0)
            {
                WaitUntilVisible(subconWarningHeader);
                AssertTrueContentEquals(subconModalSaveWarningP1, "You are consolidating two or more properties into one. The old parent properties records will be retired, and a new child property will be created.");
                AssertTrueContentEquals(subconModalSaveWarningP2, "If you proceed, you will be redirected to the new child property record, where you can view changes and make updates. Do you want to proceed?");

                sharedModals.ModalClickOKBttn();
            }     
        }

        public void CancelSubdivisionConsolidation()
        {
            Wait();
            webDriver.FindElement(subconPropertiesCancelButton).Click();

            sharedModals.CancelActionModal();
        }

        public void CreateSubdivision(PropertySubdivision subdivision)
        {
            Wait();

            webDriver.FindElement(subdconSearchParentByPIDInput).SendKeys(subdivision.SubdivisionSource.PropertyHistoryIdentifier);
            webDriver.FindElement(subconSearchParentBttn).Click();

            webDriver.FindElement(subconChildrenSearchTab).Click();
            foreach (PropertyHistory child in subdivision.SubdivisionDestination)
            {
                Wait();
                webDriver.FindElement(subconChildrenResetButton).Click();
                webDriver.FindElement(subconChildrenSearchByPIDInput).SendKeys(child.PropertyHistoryIdentifier);
                webDriver.FindElement(subconChildrenSearchButton).Click();

                WaitUntilTableSpinnerDisappear();
                webDriver.FindElement(subconChildren1stResultCheckbox).Click();
                webDriver.FindElement(subconChildernAddToSelectionBttn).Click();
            }
        }

        public void CreateConsolidation(PropertyConsolidation consolidation)
        {
            foreach (PropertyHistory parent in consolidation.ConsolidationSource)
            {
                Wait();
                webDriver.FindElement(subconSearchParentResetBttn).Click();
                webDriver.FindElement(subdconSearchParentByPIDInput).SendKeys(parent.PropertyHistoryIdentifier);
                webDriver.FindElement(subconSearchParentBttn).Click();
            }

            webDriver.FindElement(subconChildrenSearchTab).Click();
            webDriver.FindElement(subconChildrenSearchByPIDInput).SendKeys(consolidation.ConsolidationDestination.PropertyHistoryIdentifier);
            webDriver.FindElement(subconChildrenSearchButton).Click();

            WaitUntilTableSpinnerDisappear();
            webDriver.FindElement(subconChildren1stResultCheckbox).Click();
            webDriver.FindElement(subconChildernAddToSelectionBttn).Click();
        }

        public void VerifyInitCreateSubdivisionForm()
        {
            AssertTrueIsDisplayed(subdivisionCreateTitle);
            AssertTrueIsDisplayed(subdivisionCreateSubtitle);
            AssertTrueIsDisplayed(subdivisionParentInstructionsParagraph);

            AssertTrueIsDisplayed(subconParentSearchAnchor);
            AssertTrueIsDisplayed(subconSearchParentByPIDSelect);
            AssertTrueIsDisplayed(subdconSearchParentByPIDInput);
            AssertTrueIsDisplayed(subconSearchParentBttn);
            AssertTrueIsDisplayed(subconSearchParentResetBttn);

            AssertTrueIsDisplayed(subdivisionSelectedParentSubtitle);
            AssertTrueIsDisplayed(subconParentResultIdentifierColumn);
            AssertTrueIsDisplayed(subconParentResultPlanColumn);
            AssertTrueIsDisplayed(subconParentResultAreaColumn);
            AssertTrueIsDisplayed(subconParentResultAddressColumn);

            AssertTrueIsDisplayed(subdivisionChildrenInstructionsParagraph);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapTab);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapSubtitle);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapBlueIcon);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapInstuction1);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapInstuction2);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapInstuction3);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapSelectedLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapPIDLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapPlanLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapAddressLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapRegionLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapDistrictLabel);

            AssertTrueIsDisplayed(subdivisionSelectedChildrenSubtitle);
            AssertTrueIsDisplayed(subdivisionChildrenResultIdentifierColumn);
            AssertTrueIsDisplayed(subdivisionChildrenResultPlanColumn);
            AssertTrueIsDisplayed(subdivisionChildrenResultAreaColumn);
            AssertTrueIsDisplayed(subdivisionChildrenResultAddressColumn);
        }

        public void VerifyInitCreateConsolidationForm()
        {
            AssertTrueIsDisplayed(consolidationCreateTitle);
            AssertTrueIsDisplayed(consolidationCreateSubtitle);
            AssertTrueIsDisplayed(consolidationParentsInstructionsParagraph);

            AssertTrueIsDisplayed(subconParentSearchAnchor);
            AssertTrueIsDisplayed(subconSearchParentByPIDSelect);
            AssertTrueIsDisplayed(subdconSearchParentByPIDInput);
            AssertTrueIsDisplayed(subconSearchParentBttn);
            AssertTrueIsDisplayed(subconSearchParentResetBttn);

            AssertTrueIsDisplayed(consolidationSelectedParentsSubtitle);
            AssertTrueIsDisplayed(consolidationParentsResultIdentifierColumn);
            AssertTrueIsDisplayed(consolidationParentsResultPlanColumn);
            AssertTrueIsDisplayed(consolidationParentsResultAreaColumn);
            AssertTrueIsDisplayed(consolidationParentsResultAddressColumn);

            AssertTrueIsDisplayed(consolidationChildInstructionsParagraph);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapTab);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapSubtitle);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapBlueIcon);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapInstuction1);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapInstuction2);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapInstuction3);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapSelectedLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapPIDLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapPlanLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapAddressLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapRegionLabel);
            AssertTrueIsDisplayed(subconChildrenlocateOnMapDistrictLabel);

            AssertTrueIsDisplayed(consolidationSelectedChildSubtitle);
            AssertTrueIsDisplayed(consolidationChildResultIdentifierColumn);
            AssertTrueIsDisplayed(consolidationChildResultPlanColumn);
            AssertTrueIsDisplayed(consolidationChildResultAreaColumn);
            AssertTrueIsDisplayed(consolidationChildResultAddressColumn);
        }

        public void VerifyInvalidSubdivisionChildrenMessage()
        {
            AssertTrueIsDisplayed(subconErrorHeader);
            Assert.Equal("Subdivision children may not already be in the PIMS inventory.", sharedModals.ModalContent());
            sharedModals.ModalClickOKBttn();
        }

        public void VerifyInvalidConsolidationChildMessage()
        {
            Wait();
            WaitUntilVisible(subconErrorHeader);
            Assert.Equal("Consolidated child property may not be in the PIMS inventory unless also in the parent property list.", sharedModals.ModalContent());
            sharedModals.ModalClickOKBttn();
        }

        public void VerifyInvalidConsolidationRepeatedParentMessage()
        {
            Wait();

            WaitUntilVisible(subconErrorHeader);
            Assert.Equal("Consolidations must contain at least two different parent properties.", sharedModals.ModalContent());
            sharedModals.ModalClickOKBttn();
        }

        public void VerifyInvalidSubdivisionChildMessage()
        {
            Assert.Equal("A property that the user is trying to select has already been added to the selected properties list", sharedModals.ToastifyText());
        }

        public void VerifyMissingParentMessage()
        {
            WaitUntilVisible(consolidationChooseParentsErrorMsg);
            AssertTrueIsDisplayed(consolidationChooseParentsErrorMsg);
        }

        public void VerifyMissingChildMessage()
        {
            WaitUntilVisible(subdivisionChooseChildrenErrorMsg);
            AssertTrueIsDisplayed(subdivisionChooseChildrenErrorMsg);
        }

        public void VerifySubdivisionHistory(PropertySubdivision subdivision)
        {
            WaitUntilVisible(propertyInformationTitle);
           
            AssertTrueIsDisplayed(subdivisionHistorySubtitle);

            AssertTrueIsDisplayed(subconHistoryCreatedOnLabel);
            AssertTrueIsDisplayed(subdivisionHistoryTableParentColumn);
            AssertTrueIsDisplayed(subconHistoryTableIDColumn);
            AssertTrueIsDisplayed(subconHistoryTablePlanColumn);
            AssertTrueIsDisplayed(subconHistoryTableStatusColumn);
            AssertTrueIsDisplayed(subconHistoryTableAreaColumn);

            AssertTrueContentEquals(subdivisionParentIdentifier, "PID: " + subdivision.SubdivisionSource.PropertyHistoryIdentifier);
            AssertTrueContentEquals(subdivisionParentPlan, subdivision.SubdivisionSource.PropertyHistoryPlan);
            AssertTrueContentEquals(subdivisionParentStatus, subdivision.SubdivisionSource.PropertyHistoryStatus);
            AssertTrueElementContains(subdivisionParentArea, TransformNumberFormat(subdivision.SubdivisionSource.PropertyHistoryArea));

            for (int i = 0; i < subdivision.SubdivisionDestination.Count; i++)
            {
                var childElementNbr = i + 2;
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(3) a"), "PID: " + subdivision.SubdivisionDestination[i].PropertyHistoryIdentifier);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(4)"), subdivision.SubdivisionDestination[i].PropertyHistoryPlan);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(5)"), subdivision.SubdivisionDestination[i].PropertyHistoryStatus);
                AssertTrueElementContains(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(6)"), TransformNumberFormat(subdivision.SubdivisionDestination[i].PropertyHistoryArea));
            }  
        }

        public void VerifyConsolidationHistory(PropertyConsolidation consolidation)
        {
            WaitUntilVisible(propertyInformationTitle);

            AssertTrueIsDisplayed(consolidationHistorySubtitle);

            AssertTrueIsDisplayed(subconHistoryCreatedOnLabel);
            AssertTrueIsDisplayed(consolidationHistoryTableChildColumn);
            AssertTrueIsDisplayed(subconHistoryTableIDColumn);
            AssertTrueIsDisplayed(subconHistoryTablePlanColumn);
            AssertTrueIsDisplayed(subconHistoryTableStatusColumn);
            AssertTrueIsDisplayed(subconHistoryTableAreaColumn);

            Wait();
            for (int i = 0; i < consolidation.ConsolidationSource.Count; i++)
            {
                var parentsElementNbr = i + 1;
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ parentsElementNbr +") div[role='cell']:nth-child(3) a"), "PID: " + consolidation.ConsolidationSource[i].PropertyHistoryIdentifier);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ parentsElementNbr +") div[role='cell']:nth-child(4)"), consolidation.ConsolidationSource[i].PropertyHistoryPlan);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ parentsElementNbr +") div[role='cell']:nth-child(5)"), consolidation.ConsolidationSource[i].PropertyHistoryStatus);
                AssertTrueElementContains(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ parentsElementNbr +") div[role='cell']:nth-child(6)"), TransformNumberFormat(consolidation.ConsolidationSource[i].PropertyHistoryArea));
            }

            var childElement = webDriver.FindElements(subconTableContent).Count;
            AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElement +") div[role='cell']:nth-child(3) a"), "PID: " + consolidation.ConsolidationDestination.PropertyHistoryIdentifier);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElement +") div[role='cell']:nth-child(4)"), consolidation.ConsolidationDestination.PropertyHistoryPlan);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElement +") div[role='cell']:nth-child(5)"), consolidation.ConsolidationDestination.PropertyHistoryStatus);
            AssertTrueElementContains(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElement +") div[role='cell']:nth-child(6)"), TransformNumberFormat(consolidation.ConsolidationDestination.PropertyHistoryArea));
        }
    }
}
