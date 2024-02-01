using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System.Text.RegularExpressions;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Projects : PageObjectBase
    {
        //Menu Elements
        private By projectMenuBttn = By.CssSelector("div[data-testid='nav-tooltip-project'] a");
        private By createProjectButton = By.XPath("//a[contains(text(),'Create Project')]");

        private By projectDetailTabLink = By.CssSelector("a[data-rb-event-key='projectDetails]");

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
        private By projectCostTypeLabel = By.XPath("//label[contains(text(),'Cost type')]");
        private By projectCostTypeInput = By.Id("typeahead-select-costTypeCode");
        private By projectCostTypeOptions = By.CssSelector("div[id='typeahead-select-costTypeCode']");
        private By projectCostType1stOption = By.CssSelector("div[id='typeahead-select-costTypeCode'] a:nth-child(1)");
        private By projectWorkActivityLabel = By.XPath("//label[contains(text(),'Work activity')]");
        private By projectWorkActivityInput = By.Id("typeahead-select-workActivityCode");
        private By projectWorkActivityOptions = By.CssSelector("div[id='typeahead-select-workActivityCode']");
        private By projectWorkActivity1stOption = By.CssSelector("div[id='typeahead-select-workActivityCode'] a:nth-child(1)");
        private By projectBusinessFunctionLabel = By.XPath("//label[contains(text(),'Business function')]");
        private By projectBusinessFunctionInput = By.Id("typeahead-select-businessFunctionCode");
        private By projectBusinessFunctionOptions = By.CssSelector("div[id='typeahead-select-businessFunctionCode']");
        private By projectBusinessFunction1stOption = By.CssSelector("div[id='typeahead-select-businessFunctionCode'] a:nth-child(1)");

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
        private By productDeleteButton = By.CssSelector("button[title='Delete Project']");

        //View Project Form Elements
        private By projectViewTitle = By.XPath("//h1[contains(text(),'Project')]");
        private By projectHeaderProjectNameLabel = By.XPath("//h1[contains(text(),'Project')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/label[contains(text(),'Project:')]");
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

        private By projectCodesSubtitle = By.XPath("//div[contains(text(),'Associated Codes')]");
        private By projectCodesCostTypeContent = By.XPath("//label[contains(text(),'Cost type')]/parent::div/following-sibling::div");
        private By projectCodesWorkActivityContent = By.XPath("//label[contains(text(),'Work activity')]/parent::div/following-sibling::div");
        private By projectCodesBusinessFunctionContent = By.XPath("//label[contains(text(),'Business function')]/parent::div/following-sibling::div");

        //Buttons Elements
        private By projectCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");
        private By projectSaveButton = By.XPath("//div[contains(text(),'Save')]/parent::button");

        //Modals Elements
        private By productDeleteModal = By.CssSelector("div[class='modal-content']");
        private By projectDuplicateToast = By.CssSelector("div[class='Toastify__toast-body']");
        private By projectOverrideConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public Projects(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToCreateNewProject()
        {
            Wait(3000);

            WaitUntilClickable(projectMenuBttn);
            FocusAndClick(projectMenuBttn);

            WaitUntilClickable(createProjectButton);
            FocusAndClick(createProjectButton);
        }

        public void NavigateProjectDetails()
        {
            WaitUntilClickable(projectDetailTabLink);
            FocusAndClick(projectDetailTabLink);
        }

        public void CreateProject(Project project)
        {
            WaitUntilVisible(projectNameInput);

            webDriver.FindElement(projectNameInput).SendKeys(project.Name);
            if (project.Number != "")
            {
                webDriver.FindElement(projectNumberInput).SendKeys(project.Number);
            }
            ChooseSpecificSelectOption(projectStatusSelect, project.ProjectStatus);
            ChooseSpecificSelectOption(projectMOTIRegionInput, project.ProjectMOTIRegion);
            if (project.Summary != "")
            {
                webDriver.FindElement(projectSummaryTextarea).SendKeys(project.Summary);
            }
            if (project.CostType != "")
            {
                webDriver.FindElement(projectCostTypeInput).SendKeys(project.CostType);
                WaitUntilVisible(projectCostTypeOptions);
                webDriver.FindElement(projectCostType1stOption).Click();
            }
            if (project.WorkActivity != "")
            {
                webDriver.FindElement(projectWorkActivityInput).SendKeys(project.WorkActivity);
                WaitUntilVisible(projectWorkActivityOptions);
                webDriver.FindElement(projectWorkActivity1stOption).Click();
            }
            if (project.BusinessFunction!= "")
            {
                webDriver.FindElement(projectBusinessFunctionInput).SendKeys(project.BusinessFunction);
                WaitUntilVisible(projectBusinessFunctionOptions);
                webDriver.FindElement(projectBusinessFunction1stOption).Click();
            }
        }

        public void CreateProduct(Product product, int index)
        {
            Wait(2000);

            By productCodeDynamicInput = By.Id("input-products."+ index +".code");
            By productNameDynamicInput = By.Id("input-products."+ index +".description");
            By productStartDateDynamicInput = By.Id("datepicker-products."+ index +".startDate");
            By productCostEstimateDynamicInput = By.Id("input-products."+ index +".costEstimate");
            By productEstimateDateDynamicInput = By.Id("datepicker-products."+ index +".costEstimateDate");
            By productObjectiveDynamicInput = By.Id("input-products."+ index +".objective");
            By productScopeDynamicInput = By.Id("input-products."+ index +".scope");

            WaitUntilClickable(projectAddProductButton);
            FocusAndClick(projectAddProductButton);

            webDriver.FindElement(productCodeDynamicInput).SendKeys(product.ProductCode);
            webDriver.FindElement(productNameDynamicInput).SendKeys(product.ProductName);
            if (product.StartDate != "")
            {
                webDriver.FindElement(productStartDateDynamicInput).SendKeys(product.StartDate);
                webDriver.FindElement(productStartDateDynamicInput).SendKeys(Keys.Enter);
            }
            if (product.CostEstimate != "")
            {
                webDriver.FindElement(productCostEstimateDynamicInput).SendKeys(product.CostEstimate);
            }
            if (product.EstimateDate != "")
            {
                WaitUntilClickable(productEstimateDateDynamicInput);
                webDriver.FindElement(productEstimateDateDynamicInput).SendKeys(product.EstimateDate);
                webDriver.FindElement(productEstimateDateDynamicInput).SendKeys(Keys.Enter);
            }
            if (product.Objectives != "")
            {
                webDriver.FindElement(productObjectiveDynamicInput).SendKeys(product.Objectives);
            }
            if (product.Scope != "")
            {
                webDriver.FindElement(productScopeDynamicInput).SendKeys(product.Scope);
            }
        }

        public void UpdateProject(Project project)
        {
            WaitUntilClickable(projectEditButton);
            FocusAndClick(projectEditButton);

            //Cleaning previous Project Data
            ClearInput(projectNameInput);
            ClearInput(projectNumberInput);
            ClearInput(projectSummaryTextarea);

            WaitUntilVisible(projectNameInput);
            if (project.Name != "")
            {
                webDriver.FindElement(projectNameInput).SendKeys(project.Name);
            }
            if (project.Number != "")
            {
                webDriver.FindElement(projectNumberInput).SendKeys(project.Number);
            }
            if (project.ProjectStatus != "") { ChooseSpecificSelectOption(projectStatusSelect, project.ProjectStatus); }
            if (project.ProjectMOTIRegion != "") { ChooseSpecificSelectOption(projectMOTIRegionInput, project.ProjectMOTIRegion); }
            if (project.Summary != "")
            {
                webDriver.FindElement(projectSummaryTextarea).SendKeys(project.Summary);
            }
            if (project.CostType != "")
            {
                webDriver.FindElement(projectCostTypeInput).SendKeys(project.CostType);
                WaitUntilVisible(projectCostTypeOptions);
                webDriver.FindElement(projectCostType1stOption).Click();
            }
            if (project.WorkActivity != "")
            {
                webDriver.FindElement(projectWorkActivityInput).SendKeys(project.WorkActivity);
                WaitUntilVisible(projectWorkActivityOptions);
                webDriver.FindElement(projectWorkActivity1stOption).Click();
            }
            if (project.BusinessFunction != "")
            {
                webDriver.FindElement(projectBusinessFunctionInput).SendKeys(project.BusinessFunction);
                WaitUntilVisible(projectBusinessFunctionOptions);
                webDriver.FindElement(projectBusinessFunction1stOption).Click();
            }
        }

        public void UpdateProduct(Product product, int index)
        {
            By productStartDateDynamicInput = By.Id("datepicker-products."+ index +".startDate");
            By productCostEstimateDynamicInput = By.Id("input-products."+ index +".costEstimate");
            By productEstimateDateDynamicInput = By.Id("datepicker-products."+ index +".costEstimateDate");
            By productObjectiveDynamicInput = By.Id("input-products."+ index +".objective");
            By productScopeDynamicInput = By.Id("input-products."+ index +".scope");

            
            //Cleaning previous input
            if (webDriver.FindElements(productEstimateDateDynamicInput).Count > 0)
                ClearInput(productEstimateDateDynamicInput);
            
            ClearInput(productStartDateDynamicInput);
            ClearInput(productCostEstimateDynamicInput);
            ClearInput(productObjectiveDynamicInput);
            ClearInput(productScopeDynamicInput);
           
            if (product.StartDate != "")
            {
                webDriver.FindElement(productStartDateDynamicInput).SendKeys(product.StartDate);
                webDriver.FindElement(productStartDateLabel).Click();
            }
            if (product.CostEstimate != "")
            {
                webDriver.FindElement(productCostEstimateDynamicInput).SendKeys(product.CostEstimate);
            }
            if (product.EstimateDate != "")
            {
                webDriver.FindElement(productEstimateDateDynamicInput).SendKeys(product.EstimateDate);
            }
            if (product.Objectives != "")
            {
                webDriver.FindElement(productObjectiveDynamicInput).SendKeys(product.Objectives);
            }
            if (product.Scope != "")
            {
                webDriver.FindElement(productScopeDynamicInput).SendKeys(product.Scope);
            }
        }

        public void DeleteProduct(int productIndex)
        {
            By deleteButtonElement = By.XPath("//div[@class='collapse show']/div["+ productIndex +"]/div/div/button[@title='Delete Project']");
            WaitUntilClickable(deleteButtonElement);
            webDriver.FindElement(deleteButtonElement).Click();

            if (webDriver.FindElements(productDeleteModal).Count > 0)
            {
                Wait();
                Assert.Equal("Remove Product", sharedModals.ModalHeader());
                Assert.Equal("Deleting this product will remove it from all \"Product\" dropdowns. Are you certain you wish to proceed?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }
        }

        public void SaveProject()
        {
            WaitUntilClickable(projectSaveButton);
            FocusAndClick(projectSaveButton);

            Wait();
            if (webDriver.FindElements(projectOverrideConfirmationModal).Count() > 0)
            {
                if (sharedModals.ModalHeader().Equals("User Override Required"))
                {
                    Assert.Contains("can also be found in one or more other projects. Please verify the correct product is being added", sharedModals.ModalContent());
                }
                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(projectEditButton);
        }

        public void CancelProject()
        {
            WaitUntilClickable(projectCancelButton);
            FocusAndClick(projectCancelButton);
        }

        public void VerifyCreateProjectForm()
        {
            Wait(2000);

            AssertTrueIsDisplayed(projectCreateTitle);
            AssertTrueIsDisplayed(projectInstructionParagraph);

            AssertTrueIsDisplayed(projectNameLabel);
            AssertTrueIsDisplayed(projectNameInput);
            AssertTrueIsDisplayed(projectNumberLabel);
            AssertTrueIsDisplayed(projectNumberInput);
            AssertTrueIsDisplayed(projectStatusLabel);
            AssertTrueIsDisplayed(projectStatusSelect);
            AssertTrueIsDisplayed(projectMOTIRegionLabel);
            AssertTrueIsDisplayed(projectMOTIRegionInput);
            AssertTrueIsDisplayed(projectSummaryLabel);
            AssertTrueIsDisplayed(projectSummaryTextarea);

            AssertTrueIsDisplayed(projectCostTypeLabel);
            AssertTrueIsDisplayed(projectCostTypeInput);
            AssertTrueIsDisplayed(projectWorkActivityLabel);
            AssertTrueIsDisplayed(projectWorkActivityInput);
            AssertTrueIsDisplayed(projectBusinessFunctionLabel);
            AssertTrueIsDisplayed(projectBusinessFunctionInput);

            AssertTrueIsDisplayed(projectAssociatedProdsSubtitle);
            AssertTrueIsDisplayed(projectAddProductButton);

            AssertTrueIsDisplayed(projectCancelButton);
            AssertTrueIsDisplayed(projectSaveButton);
        }

        public void VerifyCreateProductForm()
        {
            WaitUntilClickable(projectAddProductButton);
            webDriver.FindElement(projectAddProductButton).Click();

            Wait();
            AssertTrueIsDisplayed(productCodeLabel);
            AssertTrueIsDisplayed(productCodeInput);
            AssertTrueIsDisplayed(productNameLabel);
            AssertTrueIsDisplayed(productNameInput);
            AssertTrueIsDisplayed(productStartDateLabel);
            AssertTrueIsDisplayed(productStartDateInput);
            AssertTrueIsDisplayed(productCostEstimateLabel);
            AssertTrueIsDisplayed(productCostEstimateInput);
            AssertTrueIsDisplayed(productObjectiveLabel);
            AssertTrueIsDisplayed(productObjectiveInput);
            AssertTrueIsDisplayed(productScopeLabel);
            AssertTrueIsDisplayed(productScopeInput);
            AssertTrueIsDisplayed(productDeleteButton);

            DeleteProduct(1);
        }

        public void VerifyProjectViewForm(Project project)
        {
            DateTime thisDay = DateTime.Today;
            string today = thisDay.ToString("MMM d, yyyy");

            WaitUntilVisible(projectHeaderProjectNameContent);

            //Header
            AssertTrueIsDisplayed(projectViewTitle);
            AssertTrueIsDisplayed(projectHeaderProjectNameLabel);
            AssertTrueContentEquals(projectHeaderProjectNameContent,project.CodeName);
            AssertTrueIsDisplayed(projectHeaderMoTIRegionLabel);
            AssertTrueContentEquals(projectHeaderMoTIRegionContent,project.ProjectMOTIRegion);
            AssertTrueIsDisplayed(projectHeaderCreatedLabel);
            AssertTrueContentEquals(projectHeaderCreatedContent,today);
            AssertTrueContentEquals(projectHeaderCreatedBy,project.CreatedBy);
            AssertTrueIsDisplayed(projectHeaderLastUpdatedLabel);
            AssertTrueContentEquals(projectHeaderLastUpdatedContent,today);
            AssertTrueContentEquals(projectHeaderLastUpdatedBy,project.UpdatedBy);
            AssertTrueIsDisplayed(projectHeaderStatusLabel);
            AssertTrueContentEquals(projectHeaderStatusContent,project.ProjectStatus);

            //Edit Button
            AssertTrueIsDisplayed(projectEditButton);

            //Project Details
            AssertTrueIsDisplayed(projectDetailsSubtitle);
            AssertTrueIsDisplayed(projectSummaryLabel);
            AssertTrueContentEquals(projectDetailsSummaryContent, project.Summary);

            //Associated Codes
            AssertTrueIsDisplayed(projectCodesSubtitle);
            AssertTrueIsDisplayed(projectCostTypeLabel);
            AssertTrueIsDisplayed(projectCodesCostTypeContent);
            AssertTrueIsDisplayed(projectWorkActivityLabel);
            AssertTrueIsDisplayed(projectCodesWorkActivityContent);
            AssertTrueIsDisplayed(projectBusinessFunctionLabel);
            AssertTrueIsDisplayed(projectCodesBusinessFunctionContent);
        }

        public void VerifyProductViewForm(Product product, int index, string validationType)
        {
            DateTime thisDay = DateTime.Today;
            string today = thisDay.ToString("MMM dd, yyyy");
            int child = index + 1;

            By productHeader = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child + "]/div[1]");
            By productStartDateLabel = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child + "]/div/div/label[contains(text(),'Start Date')]");
            By productStartDateContent = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child  + "]/div/div/label[contains(text(),'Start Date')]/parent::div/following-sibling::div");
            By productCostEstimateLabel = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child  + "]/div/div/label[contains(text(),'Cost estimate')]");
            By productCostEstimateContent = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child  + "]/div/div/label[contains(text(),'Cost estimate')]/parent::div/following-sibling::div");
            By productObjectivesLabel = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child  + "]/div/div/label[contains(text(),'Objectives')]");
            By productObjectivesContent = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child  + "]/div/div/label[contains(text(),'Objectives')]/parent::div/following-sibling::div");
            By productScopeLabel = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child  + "]/div/div/label[contains(text(),'Scope')]");
            By productScopeContent = By.XPath("//div[contains(text(),'Associated Products')]/parent::div/parent::h2/following-sibling::div/div[" + child + "]/div/div/label[contains(text(),'Scope')]/parent::div/following-sibling::div");

            WaitUntilVisible(productStartDateLabel);

            if (validationType == "Create")
            {
                AssertTrueIsDisplayed(productHeader);
                AssertTrueIsDisplayed(productStartDateLabel);
                AssertTrueContentEquals(productStartDateContent, TransformDateFormat(product.StartDate));
                AssertTrueIsDisplayed(productCostEstimateLabel);

                if (product.EstimateDate != "")
                    AssertTrueContentEquals(productCostEstimateContent, TransformCurrencyFormat(product.CostEstimate) + " as of " + TransformDateFormat(product.EstimateDate));
                else
                    AssertTrueContentEquals(productCostEstimateContent, TransformCurrencyFormat(product.CostEstimate) + " no estimate date entered");

                AssertTrueIsDisplayed(productObjectivesLabel);

                if (product.Objectives != "")
                    AssertTrueContentEquals(productObjectivesContent, product.Objectives);
                else
                    AssertTrueContentEquals(productObjectivesContent, "no objective entered");

                AssertTrueIsDisplayed(productScopeLabel);
                if (product.Scope != "")
                
                    AssertTrueContentEquals(productScopeContent, product.Scope);
                else
                    AssertTrueContentEquals(productScopeContent, "no scope entered");
            }
            else
            {
                AssertTrueIsDisplayed(productHeader);
                AssertTrueIsDisplayed(productStartDateLabel);
                AssertTrueContentEquals(productStartDateContent, TransformDateFormat(product.StartDate));
                AssertTrueIsDisplayed(productCostEstimateLabel);

                if (product.EstimateDate != "")
                    AssertTrueContentEquals(productCostEstimateContent, TransformCurrencyFormat(product.CostEstimate) + " as of " + TransformDateFormat(product.EstimateDate));
                else
                    AssertTrueContentEquals(productCostEstimateContent, TransformCurrencyFormat(product.CostEstimate) + " no estimate date entered");

                AssertTrueIsDisplayed(productObjectivesLabel);

                if (product.Objectives != "")
                    AssertTrueContentEquals(productObjectivesContent, product.Objectives);
                else if (product.Objectives != "")
                    AssertTrueContentEquals(productObjectivesContent, product.Objectives);
                else
                    AssertTrueContentEquals(productObjectivesContent, "no objective entered");

                AssertTrueIsDisplayed(productScopeLabel);
                if (product.Scope != "")
                    AssertTrueContentEquals(productScopeContent, product.Scope);
                else if (product.Scope != "")
                    AssertTrueContentEquals(productScopeContent, product.Scope);
                else    
                    AssertTrueContentEquals(productScopeContent, "no scope entered");
            }
        }

        public Boolean DuplicateProject()
        {
            Wait();
            return webDriver.FindElements(projectDuplicateToast).Count > 0;
        }

        public string GetProjectName()
        {
            WaitUntilVisible(projectHeaderProjectNameContent);

            var totalProjectName = webDriver.FindElement(projectHeaderProjectNameContent).Text;
            return Regex.Match(totalProjectName, "[^ ]* (.*)").Groups[1].Value;
        }

    }
}
