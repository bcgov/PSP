using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System.Text.RegularExpressions;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Projects : PageObjectBase
    {
        //Menu Elements
        private readonly By projectMenuBttn = By.CssSelector("div[data-testid='nav-tooltip-project'] a");
        private readonly By createProjectButton = By.XPath("//a[contains(text(),'Create Project')]");

        private readonly By projectNavigationDetailsTab = By.CssSelector("nav[role='tablist'] a[data-rb-event-key='projectDetails']");
        private readonly By projectDetailTabLink = By.CssSelector("a[data-rb-event-key='projectDetails]");

        //Create Project Form Elements
        private readonly By projectCreateTitle = By.XPath("//h1[contains(text(),'Create Project')]");
        private readonly By projectInstructionParagraph = By.XPath("//p[contains(text(),'Before creating a project')]");

        private readonly By projectNameLabel = By.XPath("//label[contains(text(),'Project name')]");
        private readonly By projectNameInput = By.Id("input-projectName");
        private readonly By projectNumberLabel = By.XPath("//label[contains(text(),'Project number')]");
        private readonly By projectNumberInput = By.Id("input-projectNumber");
        private readonly By projectStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private readonly By projectStatusSelect = By.Id("input-projectStatusType");
        private readonly By projectMOTIRegionLabel = By.XPath("//label[contains(text(),'MOTT region')]");
        private readonly By projectMOTIRegionInput = By.Id("input-region");
        private readonly By projectSummaryLabel = By.XPath("//label[contains(text(),'Project summary')]");
        private readonly By projectSummaryTextarea = By.Id("input-summary");
        private readonly By projectCostTypeLabel = By.XPath("//label[contains(text(),'Cost type')]");
        private readonly By projectCostTypeInput = By.Id("typeahead-select-costTypeCode");
        private readonly By projectCostTypeOptions = By.CssSelector("div[id='typeahead-select-costTypeCode']");
        private readonly By projectCostType1stOption = By.CssSelector("div[id='typeahead-select-costTypeCode'] a:nth-child(1)");
        private readonly By projectWorkActivityLabel = By.XPath("//label[contains(text(),'Work activity')]");
        private readonly By projectWorkActivityInput = By.Id("typeahead-select-workActivityCode");
        private readonly By projectWorkActivityOptions = By.CssSelector("div[id='typeahead-select-workActivityCode']");
        private readonly By projectWorkActivity1stOption = By.CssSelector("div[id='typeahead-select-workActivityCode'] a:nth-child(1)");
        private readonly By projectBusinessFunctionLabel = By.XPath("//label[contains(text(),'Business function')]");
        private readonly By projectBusinessFunctionInput = By.Id("typeahead-select-businessFunctionCode");
        private readonly By projectBusinessFunctionOptions = By.CssSelector("div[id='typeahead-select-businessFunctionCode']");
        private readonly By projectBusinessFunction1stOption = By.CssSelector("div[id='typeahead-select-businessFunctionCode'] a:nth-child(1)");

        private readonly By projectAssociatedProdsSubtitle = By.XPath("//div[contains(text(),'Associated products')]");
        private readonly By projectAddProductButton = By.XPath("//div[contains(text(),'+ Add another product')]/parent::button");

        //Create Product Form Elements
        private readonly By productCodeLabel = By.XPath("//label[contains(text(),'Product code')]");
        private readonly By productCodeInput = By.Id("input-products.0.code");
        private readonly By productNameLabel = By.XPath("//label[contains(text(),'Name')]");
        private readonly By productNameInput = By.Id("input-products.0.description");
        private readonly By productStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private readonly By productStartDateInput = By.Id("datepicker-products.0.startDate");
        private readonly By productCostEstimateLabel = By.XPath("//label[contains(text(),'Cost estimate')]");
        private readonly By productCostEstimateInput = By.Id("input-products.0.costEstimate");
        private readonly By productObjectiveLabel = By.XPath("//label[contains(text(),'Objectives')]");
        private readonly By productObjectiveInput = By.Id("input-products.0.objective");
        private readonly By productScopeLabel = By.XPath("//label[contains(text(),'Scope')]");
        private readonly By productScopeInput = By.Id("input-products.0.scope");
        private readonly By productDeleteButton = By.CssSelector("button[title='Delete Product']");

        //Create Team Members Elements
        private readonly By teamMemberSubtitle = By.XPath("//h2/div/div[contains(text(), 'Project Management Team')]");
        private readonly By teamMemberAddBttn = By.CssSelector("button[data-testid='add-team-member']");
        private readonly By teamMemberSelectContactBttn = By.XPath("//div[@data-testid='teamMemberRow-undefined']/div/div/div/div/button[@title='Select Contact']");
        private readonly By teamMembersCount = By.XPath("//div[contains(text(),'Project Management Team')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By teamMember1stDeleteBttn = By.XPath("//div[contains(text(),'Project Management Team')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]/button");

        //View Project Form Elements
        private readonly By projectViewTitle = By.CssSelector("div[data-testid='form-title']");
        private readonly By projectHeaderProjectNameLabel = By.XPath("//label[(text()='Project:')]");
        private readonly By projectHeaderProjectNameContent = By.XPath("//label[text()='Project:']/parent::div/following-sibling::div");
        private readonly By projectHeaderMoTIRegionLabel = By.XPath("//label[contains(text(),'MOTT region')]");
        private readonly By projectHeaderMoTIRegionContent = By.XPath("//label[contains(text(),'MOTT region')]/parent::div/following-sibling::div");
        private readonly By projectHeaderCreatedLabel = By.XPath("//strong[contains(text(),'Created')]");
        private readonly By projectHeaderCreatedContent = By.XPath("//strong[contains(text(),'Created')]/parent::span");
        private readonly By projectHeaderCreatedBy = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@data-testid='tooltip-icon-userNameTooltip']");
        private readonly By projectHeaderLastUpdatedLabel = By.XPath("//strong[contains(text(),'Updated')]");
        private readonly By projectHeaderLastUpdatedContent = By.XPath("//strong[contains(text(),'Updated')]/parent::span");
        private readonly By projectHeaderLastUpdatedBy = By.XPath("//strong[contains(text(),'Updated')]/parent::span/span[@data-testid='tooltip-icon-userNameTooltip']");
        private readonly By projectHeaderStatusContent = By.XPath("//div[@class='col']/div/div[3]/div/div/div");

        private readonly By projectEditButton = By.CssSelector("button[title='Edit project']");
        private readonly By projectDetailsSubtitle = By.XPath("//div[contains(text(),'Project Details')]");
        private readonly By projectDetailsSummaryContent = By.XPath("//label[contains(text(),'Project summary')]/parent::div/following-sibling::div");

        private readonly By projectCodesSubtitle = By.XPath("//div[contains(text(),'Associated Codes')]");
        private readonly By projectCodesCostTypeContent = By.XPath("//label[contains(text(),'Cost type')]/parent::div/following-sibling::div");
        private readonly By projectCodesWorkActivityContent = By.XPath("//label[contains(text(),'Work activity')]/parent::div/following-sibling::div");
        private readonly By projectCodesBusinessFunctionContent = By.XPath("//label[contains(text(),'Business function')]/parent::div/following-sibling::div");

        //Buttons Elements
        private readonly By projectCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");
        private readonly By projectSaveButton = By.XPath("//div[contains(text(),'Save')]/parent::button");

        //Modals Elements
        private readonly By productDeleteModal = By.CssSelector("div[class='modal-content']");
        private readonly By projectDuplicateToast = By.CssSelector("div[class='Toastify__toast-body']");
        private readonly By projectOverrideConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;
        private SharedSelectContact sharedSelectContact;

        public Projects(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
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

        public void AddUpdateTeamMember(string teamMember)
        {
            WaitUntilClickable(teamMemberAddBttn);
            webDriver.FindElement(teamMemberAddBttn).Click();

            webDriver.FindElement(teamMemberSelectContactBttn).Click();
            sharedSelectContact.SelectContact(teamMember, "Individual");
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
                webDriver.FindElement(projectNameInput).SendKeys(project.Name);

            if (project.Number != "")
                webDriver.FindElement(projectNumberInput).SendKeys(project.Number);

            if (project.ProjectStatus != "")
                ChooseSpecificSelectOption(projectStatusSelect, project.ProjectStatus);

            if (project.ProjectMOTIRegion != "")
                ChooseSpecificSelectOption(projectMOTIRegionInput, project.ProjectMOTIRegion);

            if (project.Summary != "")
                webDriver.FindElement(projectSummaryTextarea).SendKeys(project.Summary);

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
            webDriver.FindElement(productStartDateDynamicInput).SendKeys(Keys.Enter);

            ClearInput(productCostEstimateDynamicInput);
            webDriver.FindElement(productCostEstimateDynamicInput).SendKeys(Keys.Enter);

            ClearInput(productObjectiveDynamicInput);
            ClearInput(productScopeDynamicInput);
           
            if (product.StartDate != "")
            {
                webDriver.FindElement(productStartDateDynamicInput).SendKeys(product.StartDate);
                webDriver.FindElement(productScopeDynamicInput).SendKeys(Keys.Enter);
            }
            if (product.CostEstimate != "")
            {
                webDriver.FindElement(productCostEstimateDynamicInput).SendKeys(product.CostEstimate);
            }
            if (product.EstimateDate != "")
            {
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

        public void DeleteProduct(int productIndex)
        {
            By deleteButtonElement = By.XPath("//div[@class='collapse show']/div["+ productIndex +"]/div/div/button[@title='Delete Product']");
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

        public void DeleteTeamMembers()
        {
            
           while (webDriver.FindElements(teamMembersCount).Count > 0)
           {
               WaitUntilClickable(teamMember1stDeleteBttn);
               webDriver.FindElement(teamMember1stDeleteBttn).Click();

               WaitUntilVisible(productDeleteModal);
               Assert.Equal("Remove Team Member", sharedModals.ModalHeader());
               Assert.Equal("Do you wish to remove this team member?", sharedModals.ModalContent());
               sharedModals.ModalClickOKBttn();
           }
            
        }

        public void SaveProject()
        {
            Wait();
            webDriver.FindElement(projectSaveButton).Click();

            Wait();
            if (webDriver.FindElements(projectOverrideConfirmationModal).Count() > 0 && sharedModals.ModalHeader() != "Error")
            {
                if (sharedModals.ModalHeader().Equals("User Override Required"))
                {
                    Assert.Contains("can also be found in one or more other projects. Please verify the correct product is being added", sharedModals.ModalContent());
                }
                sharedModals.ModalClickOKBttn();
                AssertTrueIsDisplayed(projectNavigationDetailsTab);
            }
            else if (webDriver.FindElements(projectOverrideConfirmationModal).Count() > 0 && sharedModals.ModalHeader().Contains("Error"))
            {
                return;
            }
        }

        public void CancelProject()
        {
            WaitUntilClickable(projectCancelButton);
            FocusAndClick(projectCancelButton);
        }

        public void VerifyCreateProjectForm()
        {
            Wait();

            //AssertTrueIsDisplayed(projectCreateTitle);
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

            AssertTrueIsDisplayed(teamMemberSubtitle);
            AssertTrueIsDisplayed(teamMemberAddBttn);
         
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
            AssertTrueElementContains(projectHeaderCreatedContent,today);
            AssertTrueContentEquals(projectHeaderCreatedBy,project.CreatedBy);
            AssertTrueIsDisplayed(projectHeaderLastUpdatedLabel);
            AssertTrueElementContains(projectHeaderLastUpdatedContent,today);
            AssertTrueContentEquals(projectHeaderLastUpdatedBy,project.UpdatedBy);
            AssertTrueContentEquals(projectHeaderStatusContent, GetUppercaseString(project.ProjectStatus));

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
            }
        }

        public void VerifyTeamMemberViewForm(List<string> teamMembers)
        {
            AssertTrueIsDisplayed(teamMemberSubtitle);

            if (teamMembers!.Count > 0)
            {
                for (var i = 0; i < teamMembers.Count; i++)
                {
                    var index = i + 1;
                    AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Project Management Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/label"), "Management team member:");
                    AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Project Management Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a/span"), teamMembers[i]);
                }
            }
        }

        public void DuplicateProject()
        {
            Wait();

            Wait();
            if (webDriver.FindElements(projectOverrideConfirmationModal).Count() > 0 && sharedModals.ModalHeader().Equals("Error"))
            {
              Assert.Contains("Project will not be duplicated.", sharedModals.ModalContent());
              sharedModals.ModalClickOKBttn();
            }
        }

        public string GetProjectName()
        {
           WaitUntilVisible(projectHeaderProjectNameContent);

            var totalProjectName = webDriver.FindElement(projectHeaderProjectNameContent).Text;
            return Regex.Match(totalProjectName, "[^ ]* (.*)").Groups[1].Value;
        }

    }
}
