using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

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
        private By productDeleteButton = By.CssSelector("button[title='Delete Project']");

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

        //Modals Elements
        private By deleteProductModal = By.CssSelector("div[class='modal-dialog']");
        private By duplicateProjectToast = By.CssSelector("div[class='Toastify__toast-body']");

        private SharedModals sharedModals;

        public Projects(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Create a new Project
        public void NavigateToCreateNewProject()
        {
            Wait();
            webDriver.FindElement(projectMenuBttn).Click();

            Wait();
            webDriver.FindElement(createProjectButton).Click();
        }
        public void CreateProject(Project project)
        {
            Wait();

            webDriver.FindElement(projectNameInput).SendKeys(project.Name);
            if (project.Number != "")
            {
                webDriver.FindElement(projectNumberInput).SendKeys(project.Number);
            }
            ChooseSpecificSelectOption(projectStatusSelect, project.Status);
            ChooseSpecificSelectOption(projectMOTIRegionInput, project.MOTIRegion);
            if (project.Summary != "")
            {
                webDriver.FindElement(projectSummaryTextarea).SendKeys(project.Summary);
            }
        }

        public void CreateProduct(Product product, int index)
        {
            By productCodeDynamicInput = By.Id("input-products."+ index +".code");
            By productNameDynamicInput = By.Id("input-products."+ index +".description");
            By productStartDateDynamicInput = By.Id("datepicker-products."+ index +".startDate");
            By productCostEstimateDynamicInput = By.Id("input-products."+ index +".costEstimate");
            By productEstimateDateDynamicInput = By.Id("datepicker-products."+ index +".costEstimateDate");
            By productObjectiveDynamicInput = By.Id("input-products."+ index +".objective");
            By productScopeDynamicInput = By.Id("input-products."+ index +".scope");

            Wait();
            webDriver.FindElement(projectAddProductButton).Click();

            Wait();
            webDriver.FindElement(productCodeDynamicInput).SendKeys(product.ProductCode);
            webDriver.FindElement(productNameDynamicInput).SendKeys(product.ProductName);
            if (product.StartDate != "")
            {
                webDriver.FindElement(productStartDateDynamicInput).SendKeys(product.StartDate);
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
        public void UpdateProject(Project project)
        {
            Wait();
            webDriver.FindElement(projectEditButton).Click();

            //Cleaning previous Project Data
            ClearInput(projectNameInput);
            ClearInput(projectNumberInput);
            ClearInput(projectSummaryTextarea);

            Wait();
            if (project.Name != "")
            {
                webDriver.FindElement(projectNameInput).SendKeys(project.Name);
            }
            if (project.Number != "")
            {
                webDriver.FindElement(projectNumberInput).SendKeys(project.Number);
            }
            if (project.Status != "") { ChooseSpecificSelectOption(projectStatusSelect, project.Status); }
            if (project.MOTIRegion != "") { ChooseSpecificSelectOption(projectMOTIRegionInput, project.MOTIRegion); }
            if (project.Summary != "")
            {
                webDriver.FindElement(projectSummaryTextarea).SendKeys(project.Summary);
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

            Wait();
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
            Wait();
            By deleteButtonElement = By.XPath("//div[@class='collapse show']/div["+ productIndex +"]/div/div/button[@title='Delete Project']");
            webDriver.FindElement(deleteButtonElement).Click();

            if (webDriver.FindElements(deleteProductModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader().Equals("Remove Product"));
                Assert.True(sharedModals.ModalContent().Equals("Deleting this product will remove it from all \"Product\" dropdowns. Are you certain you wish to proceed?"));
                ButtonElement("Remove");
            }
        }

        public void SaveProject()
        {
            Wait();
            FocusAndClick(projectSaveButton);
        }

        public void CancelProject()
        {
            Wait();
            FocusAndClick(projectCancelButton);
        }

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
            webDriver.FindElement(projectAddProductButton).Click();

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

            DeleteProduct(1);
        }

        public void VerifyProjectViewForm(Project project)
        {
            DateTime thisDay = DateTime.Today;
            string today = thisDay.ToString("MMM dd, yyyy");

            Wait();

            //Header
            Assert.True(webDriver.FindElement(projectViewTitle).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderProjectNameLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderProjectNameContent).Text.Equals(project.CodeName));
            Assert.True(webDriver.FindElement(projectHeaderMoTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderMoTIRegionContent).Text.Equals(project.MOTIRegion));
            Assert.True(webDriver.FindElement(projectHeaderCreatedLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderCreatedContent).Text.Equals(today));
            Assert.True(webDriver.FindElement(projectHeaderCreatedBy).Text.Equals(project.CreatedBy));
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedContent).Text.Equals(today));
            Assert.True(webDriver.FindElement(projectHeaderLastUpdatedBy).Text.Equals(project.UpdatedBy));
            Assert.True(webDriver.FindElement(projectHeaderStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(projectHeaderStatusContent).Text.Equals(project.Status));

            //Edit Button
            Assert.True(webDriver.FindElement(projectEditButton).Displayed);

            //Project Details
            Assert.True(webDriver.FindElement(projectDetailsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(projectSummaryLabel).Displayed);
            Assert.True(webDriver.FindElement(projectDetailsSummaryContent).Text.Equals(project.Summary));
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

            Wait();
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
            Wait(500);
            return webDriver.FindElements(duplicateProjectToast).Count > 0;
        }
    }
}
