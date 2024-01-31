using Microsoft.Extensions.Configuration;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class AdminToolSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly DigitalDocumentSteps digitalDocumentSteps;
        private readonly HelpDesk helpDesk;
        private readonly ManageUsers manageUsers;
        private readonly DigitalDocuments digitalDocuments;
        private readonly CDOGSTemplates cdogsTemplates;
        private readonly FinancialCodes financialCodes;
        private readonly SharedPagination sharedPagination;
        private readonly IEnumerable<DocumentFile> documentFiles;

        private readonly string userName = "TRANPSP1";

        private FinancialCode financialCode;

        public AdminToolSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            digitalDocumentSteps = new DigitalDocumentSteps(driver);
            helpDesk = new HelpDesk(driver.Current);
            manageUsers = new ManageUsers(driver.Current);
            digitalDocuments = new DigitalDocuments(driver.Current);
            financialCodes = new FinancialCodes(driver.Current);
            cdogsTemplates = new CDOGSTemplates(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);
            documentFiles = digitalDocumentSteps.UploadFileDocuments();
            financialCode = new FinancialCode();
        }

        [StepDefinition(@"I review the Help Desk Section")]
        public void ReviewHelpDeskSection()
        {
            /* TEST COVERAGE: PSP-1759, PSP-1761, PSP-1762, PSP-1764 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Help Desk Section
            helpDesk.NavigateToHelpDesk();

            //Verify Help Desk Modal
            helpDesk.VerifyHelpDeskModal();
        }

        [StepDefinition(@"I enter to the User Management List View")]
        public void UserManagement()
        {
            /* TEST COVERAGE: PSP-5329, PSP-3345 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Admin Tools
            manageUsers.NavigateAdminTools();

            //Navigate to User Management
            manageUsers.NavigateUserManagement();

            //Verify Filters
            manageUsers.FilterUsers("TRANPSP1", "Cannot determine");
            Assert.Equal(0, manageUsers.TotalUsersResult());

            manageUsers.FilterUsers("TRANPSP1", "");
            Assert.Equal(1, manageUsers.TotalUsersResult());

            manageUsers.ResetDefaultListView();
        }

        [StepDefinition(@"I create a CDOGS template")]
        public void CDOGSTemplate()
        {
            /* TEST COVERAGE: PSP-4667, PSP-4668, PSP-4677, PSP-4680, PSP-4681, PSP-5329 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Admin Tools
            manageUsers.NavigateAdminTools();

            //Navigate to User Management
            cdogsTemplates.NavigateAdminTemplates();

            //Select Templates type
            cdogsTemplates.SelectTemplateType("General Letter");

            //Add new template
            cdogsTemplates.AddNewTemplate();

            //Upload a new template
            Random random = new Random();
            var index = random.Next(0, documentFiles.Count());
            var template = documentFiles.ElementAt(index);
            digitalDocuments.UploadDocument(template.Url);

            //Save new template
            digitalDocuments.SaveCDOGTemplate();

            //Verify Document List
            digitalDocuments.VerifyDocumentsListView("CDOGS Templates");

            //Add new template
            cdogsTemplates.AddNewTemplate();

            //Upload a new template
            var index2 = random.Next(0, documentFiles.Count());
            var template2 = documentFiles.ElementAt(index2);
            digitalDocuments.UploadDocument(template2.Url);

            //Cancel upload
            digitalDocuments.CancelDigitalDocument();

            //Delete template
            cdogsTemplates.Delete1stTemplate();
        }

        [StepDefinition(@"I create a Financial Code from row number (.*)")]
        public void CreateFinancialCode(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5311, PSP-5426, PSP-5427 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Admin Tools
            PopulateFinancialCode(rowNumber);
            manageUsers.NavigateAdminTools();

            //Navigate to Financial Codes
            financialCodes.NavigateAdminFinancialCodes();

            //Go to Create New Financial Code Form
            financialCodes.CreateNewFinancialCodeBttn();

            //Verify New Financial Code Form
            financialCodes.VerifyCreateNewFinancialCodeForm();

            //Create New Financial Code
            financialCodes.CreateNewFinancialCode(financialCode);

            //Cancel creation of Financial Code
            financialCodes.CancelFinancialCode();

            //Create New Financial Code
            financialCodes.CreateNewFinancialCodeBttn();
            financialCodes.CreateNewFinancialCode(financialCode);

            //Save new Financial Code
            financialCodes.SaveFinancialCode();

            //Filter Financial Code
            financialCodes.FilterFinancialCode(financialCode.FinnCodeValue);

            //Verify new code exists
            Assert.True(financialCodes.CountTotalFinancialCodeResults() == 1);
        }

        [StepDefinition(@"I update a Financial Code from row number (.*)")]
        public void UpdateFinancialCode(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5311, PSP-5424, PSP-5425 */

            //Choose first result from search
            PopulateFinancialCode(rowNumber);
            financialCodes.ChooseFirstSearchCodeValue();

            //Verify Update Financial Code Form
            financialCodes.VerifyUpdateFinancialCodeForm();

            //Edit Financial Code
            financialCodes.UpdateFinancialCode(financialCode);

            //Cancel changes
            financialCodes.CancelFinancialCode();

            //Filter for non-existent Financial Code
            financialCodes.FilterFinancialCode("Non-existent");

            //Verify new code exists
            Assert.True(financialCodes.CountTotalFinancialCodeResults() == 0);

            //Filter Financial Code
            financialCodes.FilterFinancialCode(financialCode.FinnCodeValue);

            //Choose first result from search
            financialCodes.ChooseFirstSearchCodeValue();

            //Edit Financial Code
            financialCodes.UpdateFinancialCode(financialCode);

            //Save changes
            financialCodes.SaveFinancialCode();
        }

        [StepDefinition(@"I attempt to duplicate a Financial Code from row number (.*)")]
        public void DuplicateFinancialCode(int rowNumber)
        {
            /* TEST COVERAGE:  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Admin Tools
            PopulateFinancialCode(rowNumber);
            manageUsers.NavigateAdminTools();

            //Navigate to Financial Codes
            financialCodes.NavigateAdminFinancialCodes();

            //Create New Financial Code
            financialCodes.CreateNewFinancialCodeBttn();
            financialCodes.CreateNewFinancialCode(financialCode);

            //Save new Financial Code
            financialCodes.SaveFinancialCode();
        }

        [StepDefinition(@"I search for an existing Financial Code from row number (.*)")]
        public void SearchFinancialCodes(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-5310, PSP-5318 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Admin Tools
            PopulateFinancialCode(rowNumber);
            manageUsers.NavigateAdminTools();

            //Navigate to Financial Codes
            financialCodes.NavigateAdminFinancialCodes();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, financialCodes.CountTotalFinancialCodeResults());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, financialCodes.CountTotalFinancialCodeResults());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, financialCodes.CountTotalFinancialCodeResults());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, financialCodes.CountTotalFinancialCodeResults());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, financialCodes.CountTotalFinancialCodeResults());


            //Verify Column Sorting by Financial Code Value
            financialCodes.OrderByFinancialCodeValue();
            var firstCodeValueDescResult = financialCodes.FirstFinancialCodeValue();

            financialCodes.OrderByFinancialCodeValue();
            var firstCodeValueAscResult = financialCodes.FirstFinancialCodeValue();

            Assert.NotEqual(firstCodeValueDescResult, firstCodeValueAscResult);

            //Verify Column Sorting by Financial Code Description
            financialCodes.OrderByFinancialCodeDescription();
            var firstFinCodeDescriptionDescResult = financialCodes.FirstFinancialCodeDescription();

            financialCodes.OrderByFinancialCodeDescription();
            var firstFinCodeDescriptionAscResult = financialCodes.FirstFinancialCodeDescription();

            Assert.NotEqual(firstFinCodeDescriptionDescResult, firstFinCodeDescriptionAscResult);

            //Verify Column Sorting by Financial Code Type Date
            financialCodes.OrderByFinancialCodeType();
            var firstFinCodeTypeDescResult = financialCodes.FirstFinancialCodeType();

            financialCodes.OrderByFinancialCodeType();
            var firstFinCodeTypeAscResult = financialCodes.FirstFinancialCodeType();

            Assert.NotEqual(firstFinCodeTypeDescResult, firstFinCodeTypeAscResult);

            //Verify Column Sorting by Financial Code Effective Date
            financialCodes.OrderByFinancialCodeEffectiveDate();
            var firstFinCodeEffectiveDateDescResult = financialCodes.FirstFinancialCodeEffectiveDate();

            financialCodes.OrderByFinancialCodeEffectiveDate();
            var firstFinCodeEffectiveAscResult = financialCodes.FirstFinancialCodeEffectiveDate();

            Assert.NotEqual(firstFinCodeEffectiveDateDescResult, firstFinCodeEffectiveAscResult);

            //Verify Column Sorting by Financial Code Expiry Date
            financialCodes.OrderByFinancialCodeExpiryDate();
            var firstFinCodeExpiryDateDescResult = financialCodes.FirstFinancialCodeExpiryDate();

            financialCodes.OrderByFinancialCodeExpiryDate();
            var firstFinCodeExpiryDateAscResult = financialCodes.FirstFinancialCodeExpiryDate();

            Assert.NotEqual(firstFinCodeExpiryDateDescResult, firstFinCodeExpiryDateAscResult);


            //Filter Financial Codes by Cost Types
            financialCodes.FilterFinancialCodeByType("Business function");
            Assert.Equal("Business function", financialCodes.FirstFinancialCodeType());

            financialCodes.FilterFinancialCodeByType("Cost types");
            Assert.Equal("Cost types", financialCodes.FirstFinancialCodeType());

            financialCodes.FilterFinancialCodeByType("Work activity");
            Assert.Equal("Work activity", financialCodes.FirstFinancialCodeType());

            financialCodes.FilterFinancialCodeByType("Chart of accounts");
            Assert.Equal("Chart of accounts", financialCodes.FirstFinancialCodeType());

            financialCodes.FilterFinancialCodeByType("Financial activity");
            Assert.Equal("Financial activity", financialCodes.FirstFinancialCodeType());

            financialCodes.FilterFinancialCodeByType("Responsibility");
            Assert.Equal("Responsibility", financialCodes.FirstFinancialCodeType());

            financialCodes.FilterFinancialCodeByType("Yearly financial");
            Assert.Equal("Yearly financial", financialCodes.FirstFinancialCodeType());

            //Filter by Code Value
            financialCodes.FilterFinancialCode(financialCode.FinnCodeValue);
        }

        [StepDefinition(@"Help Desk rendered successfully")]
        public void HelpDeskRenderSuccessfully()
        {
            /* TEST COVERAGE: PSP-1764 */

            helpDesk.CancelHelpDeskModal();
        }

        [StepDefinition(@"User Management rendered successfully")]
        public void UserManagementRenderSuccessfully()
        {
            /* TEST COVERAGE: PSP-3745 */

            //Verify List View
            manageUsers.VerifyManageUserListView();

        }

        [StepDefinition(@"CDOGS rendered successfully")]
        public void CDOGSRenderSuccessfully()
        {
            /* TEST COVERAGE: PSP-4675 */

            //Verify CDOGS List View
            cdogsTemplates.VerifyCDOGSListView();

        }

        [StepDefinition(@"Financial Codes rendered successfully")]
        public void FinancialCodesListSuccessfully()
        {
            /* TEST COVERAGE: PSP-5309 */

            //Verify Financial Codes List View
            financialCodes.VerifyFinancialCodeListView();
        }

        [StepDefinition(@"Financial Code cannot be duplicated successfully")]
        public void FinancialCodesCannotDuplicateSuccessfully()
        {
            Assert.True(financialCodes.DuplicateErrorMessageDisplayed());
        }

        private void PopulateFinancialCode(int rowNumber)
        {
            DataTable financialCodesSheet = ExcelDataContext.GetInstance().Sheets["FinancialCodes"];
            ExcelDataContext.PopulateInCollection(financialCodesSheet);

            financialCode.FinnCodeType = ExcelDataContext.ReadData(rowNumber, "FinnCodeType");
            financialCode.FinnCodeValue = ExcelDataContext.ReadData(rowNumber, "FinnCodeValue");
            financialCode.FinnCodeDescription = ExcelDataContext.ReadData(rowNumber, "FinnCodeDescription");
            financialCode.FinnEffectiveDate = ExcelDataContext.ReadData(rowNumber, "FinnEffectiveDate");
            financialCode.FinnExpiryDate = ExcelDataContext.ReadData(rowNumber, "FinnExpiryDate");
            financialCode.FinnDisplayOrder = ExcelDataContext.ReadData(rowNumber, "FinnDisplayOrder");
        }

    }


}
