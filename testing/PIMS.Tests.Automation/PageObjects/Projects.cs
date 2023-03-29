using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Projects : PageObjectBase
    {
        //Menu Elements
        private By projectMenuBttn = By.XPath("//a/label[contains(text(),'Project')]/parent::a");
        private By createProjectButton = By.XPath("//a[contains(text(),'Create Project')]");

        //Create Project Form Elements
        private By projectCreateTitle = By.XPath("//h1[contains(text(),'Create Project')]");
        private By projectInstructionParagraph = By.XPath("//p[contains(text(),'Before creating a project')]");

        private By projectNameLabel = By.XPath("//label[contains(text(),'Project name')]");
        private By projectNameInput = By.Id("input-projectName");
        private By projectNumberLabel = By.XPath("//label[contains(text(),'Project number')]");
        private By projectNumberInput = By.Id("input-projectNumber");
        private By projectStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private By projectStatusSelect = By.Id("input-projectStatusType");
        private By projectMOTIRegionLabel = By.XPath("//label[contains(text(),'MoTI region')]");
        private By projectMOTIRegionInput = By.Id("input-region");
        private By projectSummaryLabel = By.XPath("//label[contains(text(),'Project summary')]");
        private By projectSummaryTextarea = By.Id("input-summary");

        private By projectAssociatedProdsSubtitle = By.XPath("//div[contains(text(),'Associated products')]");
        private By projectAddProductButton = By.XPath("//div[contains(text(),'+ Add another product')]/parent::button");

        //Create Product Form Elements
        private By productCodeLabel = By.XPath("//label[contains(text(),'Product code')]");
        private By productCodeInput = By.Id("input-products.0.code");
        private By productNameLabel = By.XPath("//label[contains(text(),'Name')]");
        private By productNameInput = By.Id("input-products.0.description");
        private By productStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private By productStartDateInput = By.Id("datepicker-products.0.startDate");
        private By productCostEstimateLabel = By.XPath("//label[contains(text(),'Cost estimate')]");
        private By productCostEstimateInput = By.Id("input-products.0.costEstimate");
        private By productObjectiveLabel = By.XPath("//label[contains(text(),'Objectives')]");
        private By productObjectiveInput = By.Id("input-products.0.objective");
        private By productScopeLabel = By.XPath("//label[contains(text(),'Scope')]");
        private By productScopeInput = By.Id("input-products.0.scope");
        private By productDeleteButton = By.CssSelector("button[title='Delete Note']");

        //View Project Form Elements
        private By projectViewTitle = By.XPath("//h1[contains(text(),'Project')]");
        private By projectHeaderProjectNameLabel = By.XPath("//h1/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/label[contains(text(),'Project')]");
        private By projectHeaderProjectNameContent = By.XPath("//label[contains(text(),'Project')]/parent::div/following-sibling::div/strong");
        private By projectHeaderMoTIRegionLabel = By.XPath("//label[contains(text(),'MoTI Region')]");
        private By projectHeaderMoTIRegionContent = By.XPath("//label[contains(text(),'MoTI Region')]/parent::div/following-sibling::div/strong");
        private By projectHeaderCreatedLabel = By.XPath("//span[contains(text(),'Created')]");
        private By projectHeaderCreatedContent = By.XPath("//span[contains(text(),'Created')]/strong");
        private By projectHeaderCreatedBy = By.XPath("//span[contains(text(),'Created')]/span[@data-testid='tooltip-icon-userNameTooltip']");
        private By projectHeaderLastUpdatedLabel = By.XPath("//span[contains(text(),'Last updated')]");
        private By projectHeaderLastUpdatedContent = By.XPath("//span[contains(text(),'Last updated')]/strong");
        private By projectHeaderLastUpdatedBy = By.XPath("//span[contains(text(),'Last updated')]/span[@data-testid='tooltip-icon-userNameTooltip']");
        private By projectHeaderStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private By projectHeaderStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div/strong");

        private By projectEditButton = By.CssSelector("button[title='Edit project']");
        private By projectDetailsSubtitle = By.XPath("//div[contains(text(),'Project Details')]");
        private By projectDetailsSummaryContent = By.XPath("//label[contains(text(),'Project summary')]/parent::div/following-sibling::div");

        //Buttons Elements
        private By projectCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");
        private By projectSaveButton = By.XPath("//div[contains(text(),'Save')]/parent::button");

        public Projects(IWebDriver webDriver) : base(webDriver)
        {}

        public void VerifyCreateProjectForm()
        {
            Wait();
            Assert.True(webDriver.FindElement(projectCreateTitle).Displayed);
            Assert.True(webDriver.FindElement(projectInstructionParagraph).Displayed);

            Assert.True(webDriver.FindElement(projectNameLabel).Displayed);
            Assert.True(webDriver.FindElement(projectNameInput).Displayed);
            Assert.True(webDriver.FindElement(projectNumberLabel).Displayed);
            Assert.True(webDriver.FindElement(projectNumberInput).Displayed);
            Assert.True(webDriver.FindElement(projectStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(projectStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(projectMOTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(projectMOTIRegionInput).Displayed);
            Assert.True(webDriver.FindElement(projectSummaryLabel).Displayed);
            Assert.True(webDriver.FindElement(projectSummaryTextarea).Displayed);

            Assert.True(webDriver.FindElement(projectAssociatedProdsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(projectAddProductButton).Displayed);

            Assert.True(webDriver.FindElement(projectCancelButton).Displayed);
            Assert.True(webDriver.FindElement(projectSaveButton).Displayed);
        }

        public void VerifyCreateProductForm()
        {
            Wait();
            Assert.True(webDriver.FindElement(productCodeLabel).Displayed);
            Assert.True(webDriver.FindElement(productCodeInput).Displayed);
            Assert.True(webDriver.FindElement(productNameLabel).Displayed);
            Assert.True(webDriver.FindElement(productNameInput).Displayed);
            Assert.True(webDriver.FindElement(productStartDateLabel).Displayed);
            Assert.True(webDriver.FindElement(productStartDateInput).Displayed);
            Assert.True(webDriver.FindElement(productCostEstimateLabel).Displayed);
            Assert.True(webDriver.FindElement(productCostEstimateInput).Displayed);
            Assert.True(webDriver.FindElement(productObjectiveLabel).Displayed);
            Assert.True(webDriver.FindElement(productObjectiveInput).Displayed);
            Assert.True(webDriver.FindElement(productScopeLabel).Displayed);
            Assert.True(webDriver.FindElement(productScopeInput).Displayed);
            Assert.True(webDriver.FindElement(productDeleteButton).Displayed);
        }

        public void VerifyViewForm(string projectName, string projectRegion, string createdDate, string createdBy, string updatedDate, string updatedBy, string status,
            string projectSummary)
        {
            //Header
            Assert.True(webDriver.FindElement(projectViewTitle).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderProjectNameLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderProjectNameContent).Text.Equals(projectName));
            Assert.True(webDriver.FindElement(projectHeaderMoTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderMoTIRegionContent).Text.Equals(projectRegion));
            Assert.True(webDriver.FindElement(projectHeaderCreatedLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderCreatedContent).Text.Equals(TransformDateFormat(createdDate)));
            Assert.True(webDriver.FindElement(projectHeaderCreatedBy).Text.Equals(createdBy));
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedContent).Text.Equals(TransformDateFormat(updatedDate)));
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedBy).Text.Equals(updatedBy));
            Assert.True(webDriver.FindElement(projectHeaderStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderStatusContent).Text.Equals(status));

            //Edit Button
            Assert.True(webDriver.FindElement(projectEditButton).Displayed);

            //Project Details
            Assert.True(webDriver.FindElement(projectDetailsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(projectSummaryLabel).Displayed);
            Assert.True(webDriver.FindElement(projectDetailsSummaryContent).Text.Equals(projectSummary));

            //Project's Products


        }
    }
}
