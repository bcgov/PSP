using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SubdivisionConsolidationProperties : PageObjectBase
    {
        //Subdivision Files Menu Elements
        private By menuSubdivisionButton = By.CssSelector("div[data-testid='nav-tooltip-subdivision&consolidation'] a");
        private By createSubdivisionFileButton = By.XPath("//a[contains(text(),'Create a Subdivision')]");

        //Subdivision Properties Selection Elements
        private By subdivisionFileCreateTitle = By.XPath("//h1[contains(text(),'Create a Subdivision')]");
        private By subdivisionFileCreateSubtitle = By.XPath("//h2[contains(text(),'Properties in Subdivision')]");
        private By subdivisionParentInstructionsParagraph = By.XPath("//p[contains(text(),'Select the parent property that was subdivided:')]");

        private By subdivisionParentSearchAnchor = By.CssSelector("a[data-rb-event-key='parent-property']");
        private By subdivisionSearchParentByPIDSelect = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/div/div/select");
        private By subdivisionSearchParentByPIDInput = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/div/input[@id='input-pid']");
        private By subdivisionSearchParentBttn = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/button[@data-testid='search']");
        private By subdivisionSearchParentResetBttn = By.XPath("//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/button[@data-testid='reset-button']");

        private By subdivisionSelectedParentSubtitle = By.XPath("//div[contains(text(),'Selected Parent')]");
        private By subdivisionParentResultIdentifierColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By subdivisionParentResultPlanColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private By subdivisionParentResultAreaColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private By subdivisionParentResultAddressColumn = By.XPath("//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private By subdivisionChildrenInstructionsParagraph = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]");
        private By subdivisionChildrenlocateOnMapTab = By.XPath("//a[contains(text(),'Locate on Map')]");
        private By subdivisionChildrenlocateOnMapSubtitle = By.XPath("//h3[contains(text(), 'Select a property')]");
        private By subdivisionChildrenlocateOnMapBlueIcon = By.Id("Layer_2");
        private By subdivisionChildrenlocateOnMapInstuction1 = By.XPath("//li[contains(text(),'Single-click blue marker above')]");
        private By subdivisionChildrenlocateOnMapInstuction2 = By.XPath("//li[contains(text(),'Mouse to a parcel on the map')]");
        private By subdivisionChildrenlocateOnMapInstuction3 = By.XPath("//li[contains(text(),'Single-click on parcel to select it')]");
        private By subdivisionChildrenlocateOnMapSelectedLabel = By.XPath("//div[contains(text(),'Selected property attributes')]");
        private By subdivisionChildrenlocateOnMapPIDLabel = By.XPath("//label[contains(text(),'PID')]");
        private By subdivisionChildrenlocateOnMapPlanLabel = By.XPath("//label[contains(text(),'Plan #')]");
        private By subdivisionChildrenlocateOnMapAddressLabel = By.XPath("//label[contains(text(),'Address')]");
        private By subdivisionChildrenlocateOnMapRegionLabel = By.XPath("//label[contains(text(),'Region')]");
        private By subdivisionChildrenlocateOnMapDistrictLabel = By.XPath("//label[contains(text(),'District')]");

        private By subdivisionChildrenSearchTab = By.XPath("//a[contains(text(),'Locate on Map')]/following-sibling::a");
        private By subdivisionChildrenSearchByPIDSelect = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/div/div/select");
        private By subdivisionChildrenSearchByPIDInput = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/div/input");
        private By subdivisionChildrenSearchButton = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/button[@data-testid='search']");
        private By subdivisionChildrenResetButton = By.XPath("//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/button[@data-testid='reset-button']");
        private By subdivisionChildren1stResultCheckbox = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td']:first-child input");
        private By subdivionChildernAddToSelectionBttn = By.XPath("//div[contains(text(),'Add to selection')]/parent::button");

        private By subdivisionSelectedChildrenSubtitle = By.XPath("//h2/div/div[contains(text(),'Selected Children')]");
        private By subdivisionChildrenResultIdentifierColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By subdivisionChildrenResultPlanColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private By subdivisionChildrenResultAreaColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private By subdivisionChildrenResultAddressColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private By subdivisionPropertiesCreateButton = By.XPath("//div[contains(text(),'Create Subdivision')]/parent::button");
        private By subdivisionPropertiesCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");

        private By subdivisionModalWindow = By.CssSelector("div[class='modal-content']");

        //Subdivision/Consolidation History Elements
        private By propertyInformationTitle = By.XPath("//h1[contains(text(),'Property Information')]");

        private By subdivisionHistorySubtitle = By.XPath("//div[contains(text(),'Subdivision History')]");
        private By consolidationHistorySubtitle = By.XPath("//div[contains(text(),'Consolidation History')]");
        private By subdivisionHistoryCreatedOnLabel = By.XPath("//label[contains(text(),'Created on')]");
        private By subdivisionHistoryTableParentColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Parent')]");
        private By subdivisionHistoryTableIDColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Identifier')]");
        private By subdivisionHistoryTablePlanColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Plan #')]");
        private By subdivisionHistoryTableStatusColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By subdivisionHistoryTableAreaColumn = By.XPath("//div[@data-testid='propertyOperationTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Area')]");


        private SharedModals sharedModals;


        public SubdivisionConsolidationProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToCreateNewSubdivision()
        {
            Wait();
            FocusAndClick(menuSubdivisionButton);

            WaitUntilVisible(createSubdivisionFileButton);
            FocusAndClick(createSubdivisionFileButton);
        }

        public void SaveSubdivision()
        {
            WaitUntilClickable(subdivisionPropertiesCreateButton);
            webDriver.FindElement(subdivisionPropertiesCreateButton).Click();

            Wait();
            if (webDriver.FindElements(subdivisionModalWindow).Count > 0)
                sharedModals.ModalClickOKBttn();

        }

        public void CancelSubdivisionConsolidation()
        {
            WaitUntilClickable(subdivisionPropertiesCancelButton);
            webDriver.FindElement(subdivisionPropertiesCancelButton).Click();

            sharedModals.CancelActionModal();
        }

        public void CreateSubdivision(PropertySubdivision subdivision)
        {
            webDriver.FindElement(subdivisionSearchParentByPIDInput).SendKeys(subdivision.SubdivisionSource.PropertyHistoryIdentifier);
            webDriver.FindElement(subdivisionSearchParentBttn).Click();

            webDriver.FindElement(subdivisionChildrenSearchTab).Click();
            foreach (PropertyHistory child in subdivision.SubdivisionDestination)
            {
                Wait();
                webDriver.FindElement(subdivisionChildrenResetButton).Click();
                webDriver.FindElement(subdivisionChildrenSearchByPIDInput).SendKeys(child.PropertyHistoryIdentifier);
                webDriver.FindElement(subdivisionChildrenSearchButton).Click();

                WaitUntilTableSpinnerDisappear();
                webDriver.FindElement(subdivisionChildren1stResultCheckbox).Click();
                webDriver.FindElement(subdivionChildernAddToSelectionBttn).Click();
            }
        }

        public void VerifyInitCreateSubdivisionForm()
        {
            AssertTrueIsDisplayed(subdivisionFileCreateTitle);
            AssertTrueIsDisplayed(subdivisionFileCreateSubtitle);
            AssertTrueIsDisplayed(subdivisionParentInstructionsParagraph);

            AssertTrueIsDisplayed(subdivisionParentSearchAnchor);
            AssertTrueIsDisplayed(subdivisionSearchParentByPIDSelect);
            AssertTrueIsDisplayed(subdivisionSearchParentByPIDInput);
            AssertTrueIsDisplayed(subdivisionSearchParentBttn);
            AssertTrueIsDisplayed(subdivisionSearchParentResetBttn);

            AssertTrueIsDisplayed(subdivisionSelectedParentSubtitle);
            AssertTrueIsDisplayed(subdivisionParentResultIdentifierColumn);
            AssertTrueIsDisplayed(subdivisionParentResultPlanColumn);
            AssertTrueIsDisplayed(subdivisionParentResultAreaColumn);
            AssertTrueIsDisplayed(subdivisionParentResultAddressColumn);

            AssertTrueIsDisplayed(subdivisionChildrenInstructionsParagraph);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapTab);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapSubtitle);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapBlueIcon);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapInstuction1);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapInstuction2);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapInstuction3);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapSelectedLabel);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapPIDLabel);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapPlanLabel);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapAddressLabel);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapRegionLabel);
            AssertTrueIsDisplayed(subdivisionChildrenlocateOnMapDistrictLabel);

            AssertTrueIsDisplayed(subdivisionSelectedChildrenSubtitle);
            AssertTrueIsDisplayed(subdivisionChildrenResultIdentifierColumn);
            AssertTrueIsDisplayed(subdivisionChildrenResultPlanColumn);
            AssertTrueIsDisplayed(subdivisionChildrenResultAreaColumn);
            AssertTrueIsDisplayed(subdivisionChildrenResultAddressColumn);
        }

        public void VerifyInvalidChildrenMessage()
        {
            AssertTrueIsDisplayed(subdivisionModalWindow);
            Assert.Equal("Subdivision children may not already be in the PIMS inventory.", sharedModals.ModalContent());
            sharedModals.ModalClickOKBttn();
        }

        public void VerifySubdivisionConsolidationHistory(string type, PropertySubdivision subdivision)
        {
            WaitUntilVisible(propertyInformationTitle);

            if(type.Equals("Subdivision"))
                AssertTrueIsDisplayed(subdivisionHistorySubtitle);
            else
                AssertTrueIsDisplayed(consolidationHistorySubtitle);

            AssertTrueIsDisplayed(subdivisionHistoryCreatedOnLabel);
            AssertTrueIsDisplayed(subdivisionHistoryTableParentColumn);
            AssertTrueIsDisplayed(subdivisionHistoryTableIDColumn);
            AssertTrueIsDisplayed(subdivisionHistoryTablePlanColumn);
            AssertTrueIsDisplayed(subdivisionHistoryTableStatusColumn);
            AssertTrueIsDisplayed(subdivisionHistoryTableAreaColumn);

            for (int i = 0; i < subdivision.SubdivisionDestination.Count; i++)
            {
                var childElementNbr = i + 1;
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(2) a"), "PID: " + subdivision.SubdivisionDestination[i].PropertyHistoryIdentifier);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(3)"), subdivision.SubdivisionDestination[i].PropertyHistoryPlan);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(4)"), subdivision.SubdivisionDestination[i].PropertyHistoryStatus);
                AssertTrueContentEquals(By.CssSelector("div[data-testid='propertyOperationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ childElementNbr +") div[role='cell']:nth-child(5)"), subdivision.SubdivisionDestination[i].PropertyHistoryArea);
            }  
        } 
    }
    
}
