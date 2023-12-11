using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Projects : PageObjectBase
    {
        //Menu Elements
        private By projectMenuBttn = By.XPath("//a/label[contains(text(),'Project')]/parent::a");
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
        private By projectCostTypeSelect = By.Id("input-costTypeCode");
        private By projectWorkActivityLabel = By.XPath("//label[contains(text(),'Work activity')]");
        private By projectWorkActivitySelect = By.Id("input-workActivityCode");
        private By projectBusinessFunctionLabel = By.XPath("//label[contains(text(),'Business function')]");
        private By projectBusinessFunctionSelect = By.Id("input-businessFunctionCode");

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
        private By deleteProductModal = By.CssSelector("div[class='modal-dialog']");
        private By duplicateProjectToast = By.CssSelector("div[class='Toastify__toast-body']");

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
                ChooseSpecificSelectOption(projectCostTypeSelect, project.CostType);
            }
            if (project.WorkActivity != "")
            {
                ChooseSpecificSelectOption(projectWorkActivitySelect, project.WorkActivity);
            }
            if (project.BusinessFunction!= "")
            {
                ChooseSpecificSelectOption(projectBusinessFunctionSelect, project.BusinessFunction);
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
                ChooseSpecificSelectOption(projectCostTypeSelect, project.CostType);
            }
            if (project.WorkActivity != "")
            {
                ChooseSpecificSelectOption(projectWorkActivitySelect, project.WorkActivity);
            }
            if (project.BusinessFunction!= "")
            {
                ChooseSpecificSelectOption(projectBusinessFunctionSelect, project.BusinessFunction);
            }
        }

        public void UpdateProduct(Product product, int index)
        {
            By productCodeDynamicInput = By.Id("input-products."+ index +".code");
            By productNameDynamicInput = By.Id("input-products."+ index +".description");
            By productStartDateDynamicInput = By.Id("datepicker-products."+ index +".startDate");
            By productCostEstimateDynamicInput = By.Id("input-products."+ index +".costEstimate");
            By productEstimateDateDynamicInput = By.Id("datepicker-products."+ index +".costEstimateDate");
            By productObjectiveDynamicInput = By.Id("input-products."+ index +".objective");
            By productScopeDynamicInput = By.Id("input-products."+ index +".scope");

            WaitUntilClickable(productCodeDynamicInput);
            //Cleaning previous input
            if (webDriver.FindElements(productEstimateDateDynamicInput).Count > 0) { ClearInput(productEstimateDateDynamicInput); }
            ClearInput(productCodeDynamicInput);
            ClearInput(productCodeDynamicInput);
            ClearInput(productNameDynamicInput);
            ClearInput(productStartDateDynamicInput);
            ClearInput(productCostEstimateDynamicInput);
            ClearInput(productObjectiveDynamicInput);
            ClearInput(productScopeDynamicInput);
           

            if (product.ProductCode != "")
            {
                webDriver.FindElement(productCodeDynamicInput).SendKeys(product.ProductCode);
            }
            if (product.ProductName != "")
            {
                webDriver.FindElement(productNameDynamicInput).SendKeys(product.ProductName);
            }
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

            if (webDriver.FindElements(deleteProductModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader().Equals("Remove Product"));
                Assert.True(sharedModals.ModalContent().Equals("Deleting this product will remove it from all \"Product\" dropdowns. Are you certain you wish to proceed?"));
                sharedModals.ModalClickOKBttn();
            }
        }

        public void SaveProject()
        {
            WaitUntilClickable(projectSaveButton);
            FocusAndClick(projectSaveButton);
        }

        public void CancelProject()
        {
            WaitUntilClickable(projectCancelButton);
            FocusAndClick(projectCancelButton);
        }

        public void VerifyCreateProjectForm()
        {
            WaitUntilVisible(projectNameLabel);
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

            Assert.True(webDriver.FindElement(projectCostTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(projectCostTypeSelect).Displayed);
            Assert.True(webDriver.FindElement(projectWorkActivityLabel).Displayed);
            Assert.True(webDriver.FindElement(projectWorkActivitySelect).Displayed);
            Assert.True(webDriver.FindElement(projectBusinessFunctionLabel).Displayed);
            Assert.True(webDriver.FindElement(projectBusinessFunctionSelect).Displayed);

            Assert.True(webDriver.FindElement(projectAssociatedProdsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(projectAddProductButton).Displayed);

            Assert.True(webDriver.FindElement(projectCancelButton).Displayed);
            Assert.True(webDriver.FindElement(projectSaveButton).Displayed);
        }

        public void VerifyCreateProductForm()
        {
            WaitUntilClickable(projectAddProductButton);
            webDriver.FindElement(projectAddProductButton).Click();

            WaitUntilVisible(productCodeLabel);
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

            DeleteProduct(1);
        }

        public void VerifyProjectViewForm(Project project)
        {
            DateTime thisDay = DateTime.Today;
            string today = thisDay.ToString("MMM d, yyyy");

            WaitUntilVisible(projectHeaderProjectNameContent);

            //Header
            Assert.True(webDriver.FindElement(projectViewTitle).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderProjectNameLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderProjectNameContent).Text.Equals(project.CodeName));
            Assert.True(webDriver.FindElement(projectHeaderMoTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderMoTIRegionContent).Text.Equals(project.ProjectMOTIRegion));
            Assert.True(webDriver.FindElement(projectHeaderCreatedLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderCreatedContent).Text.Equals(today));
            Assert.True(webDriver.FindElement(projectHeaderCreatedBy).Text.Equals(project.CreatedBy));
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedContent).Text.Equals(today));
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedBy).Text.Equals(project.UpdatedBy));
            Assert.True(webDriver.FindElement(projectHeaderStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderStatusContent).Text.Equals(project.ProjectStatus));

            //Edit Button
            Assert.True(webDriver.FindElement(projectEditButton).Displayed);

            //Project Details
            Assert.True(webDriver.FindElement(projectDetailsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(projectSummaryLabel).Displayed);
            Assert.True(webDriver.FindElement(projectDetailsSummaryContent).Text.Equals(project.Summary));

            //Associated Codes
            Assert.True(webDriver.FindElement(projectCodesSubtitle).Displayed);
            Assert.True(webDriver.FindElement(projectCostTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(projectCodesCostTypeContent).Displayed);
            Assert.True(webDriver.FindElement(projectWorkActivityLabel).Displayed);
            Assert.True(webDriver.FindElement(projectCodesWorkActivityContent).Displayed);
            Assert.True(webDriver.FindElement(projectBusinessFunctionLabel).Displayed);
            Assert.True(webDriver.FindElement(projectCodesBusinessFunctionContent).Displayed);
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
                Assert.True(webDriver.FindElement(productHeader).Displayed);
                Assert.True(webDriver.FindElement(productStartDateLabel).Displayed);
                Assert.True(webDriver.FindElement(productStartDateContent).Text.Equals(TransformDateFormat(product.StartDate)));
                Assert.True(webDriver.FindElement(productCostEstimateLabel).Displayed);
                if (product.EstimateDate != "")
                {
                    Assert.True(webDriver.FindElement(productCostEstimateContent).Text.Equals(TransformCurrencyFormat(product.CostEstimate) + " as of " + TransformDateFormat(product.EstimateDate)));
                }
                else
                {
                    Assert.True(webDriver.FindElement(productCostEstimateContent).Text.Equals(TransformCurrencyFormat(product.CostEstimate) + " no estimate date entered"));
                }
                Assert.True(webDriver.FindElement(productObjectivesLabel).Displayed);
                if (product.Objectives != "")
                {
                    Assert.True(webDriver.FindElement(productObjectivesContent).Text.Equals(product.Objectives));
                }
                else
                {
                    Assert.True(webDriver.FindElement(productObjectivesContent).Text.Equals("no objective entered"));
                }
                Assert.True(webDriver.FindElement(productScopeLabel).Displayed);
                if (product.Scope != "")
                {
                    Assert.True(webDriver.FindElement(productScopeContent).Text.Equals(product.Scope));
                }
                else
                {
                    Assert.True(webDriver.FindElement(productScopeContent).Text.Equals("no scope entered"));
                }
            }
            else
            {
                Assert.True(webDriver.FindElement(productHeader).Displayed);
                Assert.True(webDriver.FindElement(productStartDateLabel).Displayed);
                Assert.True(webDriver.FindElement(productStartDateContent).Text.Equals(TransformDateFormat(product.StartDate)));
                Assert.True(webDriver.FindElement(productCostEstimateLabel).Displayed);
                if (product.EstimateDate != "")
                {
                    Assert.True(webDriver.FindElement(productCostEstimateContent).Text.Equals(TransformCurrencyFormat(product.CostEstimate) + " as of " + TransformDateFormat(product.EstimateDate)));
                }
                else
                {
                    Assert.True(webDriver.FindElement(productCostEstimateContent).Text.Equals(TransformCurrencyFormat(product.CostEstimate) + " no estimate date entered"));
                }
                Assert.True(webDriver.FindElement(productObjectivesLabel).Displayed);
                if (product.Objectives != "")
                {
                    Assert.True(webDriver.FindElement(productObjectivesContent).Text.Equals(product.Objectives));
                }
                else if (product.Objectives != "")
                {
                    Assert.True(webDriver.FindElement(productObjectivesContent).Text.Equals(product.Objectives));
                }
                else
                {
                    Assert.True(webDriver.FindElement(productObjectivesContent).Text.Equals("no objective entered"));
                }
                Assert.True(webDriver.FindElement(productScopeLabel).Displayed);
                if (product.Scope != "")
                {
                    Assert.True(webDriver.FindElement(productScopeContent).Text.Equals(product.Scope));
                }
                else if (product.Scope != "")
                {
                    Assert.True(webDriver.FindElement(productScopeContent).Text.Equals(product.Scope));
                }
                else
                {
                    Assert.True(webDriver.FindElement(productScopeContent).Text.Equals("no scope entered"));
                }
            }
        }

        public Boolean duplicateProject()
        {
            Wait();
            return webDriver.FindElements(duplicateProjectToast).Count > 0;
        }
    }
}
