using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SubdivisionProperties : PageObjectBase
    {
        //Subdivision Files Menu Elements
        private By menuSubdivisionButton = By.CssSelector("div[data-testid='nav-tooltip-acquisition'] a");
        private By createSubdivisionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        //Subdivision Properties Selection Elements
        private By subdivisionFileCreateTitle = By.XPath("//h1[contains(text(),'Create a Subdivision')]");
        private By subdivisionFileCreateSubtitle = By.XPath("//h2[contains(text(),'Properties in Subdivision')]");
        private By subdivisionParentInstructionsParagraph = By.XPath("//p[contains(text(),'Select the parent property that was subdivided:')]");

        private By subdivisionParentSearchAnchor = By.CssSelector("a[data-rb-event-key='parent-property']");
        private By subdivisionSearchByPIDSelect = By.Id("input-searchBy");
        private By subdivisionSearchByPIDInput = By.Id("input-pid");
        private By subdivisionSelectedParentSubtitle = By.XPath("");
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

        private By subdivisionSelectedChildrenSubtitle = By.XPath("//h2/div/div[contains(text(),'Selected Children')]");
        private By subdivisionChildrenResultIdentifierColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By subdivisionChildrenResultPlanColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]");
        private By subdivisionChildrenResultAreaColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]");
        private By subdivisionChildrenResultAddressColumn = By.XPath("//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]");

        private By subdivisionPropertiesCreateButton = By.XPath("//div[contains(text(),'Create Subdivision')]/parent::button");

        public SubdivisionProperties(IWebDriver webDriver) : base(webDriver)
        {}

        public void VerifyInitCreateSubdivisionForm()
        {
            AssertTrueIsDisplayed(subdivisionFileCreateTitle);
            AssertTrueIsDisplayed(subdivisionFileCreateSubtitle);
            AssertTrueIsDisplayed(subdivisionParentInstructionsParagraph);

            AssertTrueIsDisplayed(subdivisionParentSearchAnchor);
            AssertTrueIsDisplayed(subdivisionSearchByPIDSelect);
            AssertTrueIsDisplayed(subdivisionSearchByPIDInput);
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
    }
}
