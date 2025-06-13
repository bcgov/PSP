using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ManagementDetails : PageObjectBase
    {
        //Management File Menu
        private readonly By managementMainMenu = By.XPath("//div[@data-testid= 'nav-tooltip-management']");
        private readonly By managementMainMenuCreateLink = By.XPath("//a[contains(text(),'Create Management File')]");

        //Management File Header
        private readonly By managementFileHeaderFileNbrLabel = By.XPath("//label[contains(text(),'File #')]");
        private readonly By managementFileHeaderFileNbrContent = By.XPath("//label[contains(text(),'File #')]/parent::div/following-sibling::div");
        private readonly By managementFileHeaderProjectLabel = By.XPath("//label[contains(text(), 'Ministry project')]");
        private readonly By managementFileHeaderProjectContent = By.XPath("//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]");
        private readonly By managementFileHeaderProductLabel = By.XPath("//label[contains(text(), 'Ministry product')]");
        private readonly By managementFileHeaderProductContent = By.XPath("//label[contains(text(), 'Ministry product')]/parent::div/following-sibling::div[1]");
        private readonly By managementFileHeaderCreatedDateLabel = By.XPath("//strong[contains(text(), 'Created')]");
        private readonly By managementFileHeaderCreatedDateContent = By.XPath("//strong[contains(text(), 'Created')]/parent::span");
        private readonly By managementFileHeaderCreatedByContent = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@id='userNameTooltip']/strong");
        private readonly By managementFileHeaderLastUpdateLabel = By.XPath("//strong[contains(text(), 'Updated')]");
        private readonly By managementFileHeaderLastUpdateContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span");
        private readonly By managementFileHeaderLastUpdateByContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span/span[@id='userNameTooltip']/strong");
        private readonly By managementFileHeaderHistoricalFileLabel = By.XPath("//label[contains(text(),'Historical file #')]");
        private readonly By managementFileHeaderHistoricalFileContent = By.XPath("//label[contains(text(),'Historical file #:')]/parent::div/following-sibling::div/div/span");
        private readonly By managementFileHeaderStatusContent = By.XPath("//b[contains(text(),'File')]/parent::span/following-sibling::div");

        //Create / Update / View Management File
        private readonly By createManagementTitle = By.XPath("//h1[contains(text(),'Create Management File')]");
        private readonly By viewManagementTitle = By.XPath("//h1[contains(text(),'Management File')]");
        private readonly By updateManagementTitle = By.XPath("//h1[contains(text(),'Update Management File')]");

        private readonly By managementFileDetailsEditBttn = By.CssSelector("button[title='Edit management file']");

        private readonly By managementFileStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private readonly By managementFileStatusInput = By.Id("input-fileStatusTypeCode");
        private readonly By managementFileStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div");

        private readonly By managementFileProjectSubtitle = By.XPath("//h2/div/div[contains(text(),'Project')]");
        private readonly By managementFileProjectLabel = By.XPath("//label[contains(text(),'Ministry project')]");
        private readonly By managementFileProjectInput = By.CssSelector("input[id='typeahead-project']");
        private readonly By managementFileProjectContent = By.XPath("//label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private readonly By managementFileProject1stOption = By.CssSelector("div[id='typeahead-project'] a");
        private readonly By managementFileProjectProductLabel = By.XPath("//label[contains(text(),'Product')]");
        private readonly By managementFileProjectProductSelect = By.Id("input-productId");
        private readonly By managementFileProjectProductOptions = By.CssSelector("select[id='input-productId'] option");
        private readonly By managementFileProjectProductContent = By.XPath("//label[contains(text(),'Product')]/parent::div/following-sibling::div");
        private readonly By managementFileProjectFundingLabel = By.XPath("//label[contains(text(),'Funding')]");
        private readonly By managementFileProjectFundingInput = By.Id("input-fundingTypeCode");
        private readonly By managementFileProjectFundingContent = By.XPath("//label[contains(text(),'Funding')]/parent::div/following-sibling::div");

        private readonly By managementFileDetailsSubtitle = By.XPath("//h2/div/div[contains(text(),'Management Details')]");
        private readonly By managementFileNameLabel = By.XPath("//label[contains(text(),'File name')]");
        private readonly By managementFileNameLabelView = By.XPath("//label[contains(text(),'Management file name')]");
        private readonly By managementFileNameInput = By.Id("input-fileName");
        private readonly By managementFileNameContent = By.XPath("//label[contains(text(),'Management file name')]/parent::div/following-sibling::div");
        private readonly By managementFileHistoricalFileLabel = By.XPath("//label[contains(text(),'Historical file number')]");
        private readonly By managementFileHistoricalFileInput = By.Id("input-legacyFileNum");
        private readonly By managementFileHistoricalFileContent = By.XPath("//label[contains(text(),'Historical file number')]/parent::div/following-sibling::div");
        private readonly By managementFilePurposeLabel = By.XPath("//label[contains(text(),'Purpose')]");
        private readonly By managementFilePurposeSelect = By.Id("input-purposeTypeCode");
        private readonly By managemenetFilePurposeContent = By.XPath("//label[contains(text(),'Purpose')]/parent::div/following-sibling::div");
        private readonly By managementFileAdditionalDetailsLabel = By.XPath("//label[contains(text(),'Additional details')]");
        private readonly By managementFileAdditionalDetailsInput = By.Id("input-additionalDetails");
        private readonly By managementFileAdditionalDetailsContent = By.XPath("//label[contains(text(),'Additional details')]/parent::div/following-sibling::div");

        private readonly By managementFileTeamSubtitle = By.XPath("//h2/div/div[contains(text(),'Management Team')]");
        private readonly By managementFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private readonly By managementFileTeamMembersGroup = By.XPath("//div[contains(text(),'Management Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private readonly By managementFileViewTeamMembersGroup = By.XPath("//div[contains(text(),'Management Team')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By managementFileTeamFirstMemberDeleteBttn = By.XPath("//div[contains(text(),'Management Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div[3]/button");
        private readonly By managementFileTeamInvalidTeamMemberMessage = By.XPath("//div[contains(text(),'Select a team member')]");
        private readonly By managementFileTeamInvalidProfileMessage = By.XPath("//div[contains(text(),'Select a profile')]");

        private readonly By managementEditPropertiesBttn = By.CssSelector("button[title='Change properties']");

        private readonly By managementFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private readonly SharedFileProperties sharedSearchProperties;
        private readonly SharedTeamMembers sharedTeamMembers;
        private readonly SharedModals sharedModals;

        public ManagementDetails(IWebDriver webDriver) : base(webDriver)
        {
            sharedSearchProperties = new SharedFileProperties(webDriver);
            sharedTeamMembers = new SharedTeamMembers(webDriver);
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToCreateNewManagementFile()
        {
            Wait();
            FocusAndClick(managementMainMenu);

            WaitUntilClickable(managementMainMenuCreateLink);
            FocusAndClick(managementMainMenuCreateLink);
        }

        public void NavigateToAddPropertiesManagementFile()
        {
            Wait();
            webDriver.FindElement(managementEditPropertiesBttn).Click();
        }

        public void CreateMinimumManagementDetails(ManagementFile mgmtFile)
        {
            Wait();

            //Management File Name
            webDriver.FindElement(managementFileNameInput).SendKeys(mgmtFile.ManagementName);

            //Purpose
            ChooseSpecificSelectOption(managementFilePurposeSelect, mgmtFile.ManagementPurpose);
        }

        public void UpdateManagementFileDetails(ManagementFile mgmtFile)
        {
            Wait();

            AssertTrueIsDisplayed(updateManagementTitle);

            //Status
            AssertTrueIsDisplayed(managementFileStatusLabel);
            if(mgmtFile.ManagementStatus != "")
                ChooseSpecificSelectOption(managementFileStatusInput, mgmtFile.ManagementStatus);

            //PROJECT
            //Project
            AssertTrueIsDisplayed(managementFileProjectLabel);
            if (mgmtFile.ManagementMinistryProject != "")
            {
                ClearInput(managementFileProjectInput);
                webDriver.FindElement(managementFileProjectInput).SendKeys(mgmtFile.ManagementMinistryProject);
                webDriver.FindElement(managementFileProjectInput).SendKeys(Keys.Enter);
                webDriver.FindElement(managementFileProjectInput).SendKeys(Keys.Backspace);

                Wait();
                webDriver.FindElement(managementFileProject1stOption).Click();
            }

            //Product
            AssertTrueIsDisplayed(managementFileProjectProductLabel);
            if (mgmtFile.ManagementMinistryProduct != "")
                ChooseSpecificSelectOption(managementFileProjectProductSelect, mgmtFile.ManagementMinistryProduct);

            //Funding
            AssertTrueIsDisplayed(managementFileProjectFundingLabel);
            if (mgmtFile.ManagementMinistryFunding != "")
                ChooseSpecificSelectOption(managementFileProjectFundingInput, mgmtFile.ManagementMinistryFunding);

            //MANAGEMENT DETAILS
            //File Name
            AssertTrueIsDisplayed(managementFileNameLabel);
            if (mgmtFile.ManagementName != "")
            {
                ClearInput(managementFileNameInput);
                webDriver.FindElement(managementFileNameInput).SendKeys(mgmtFile.ManagementName);
            }

            //Historical file number
            AssertTrueIsDisplayed(managementFileHistoricalFileLabel);
            if (mgmtFile.ManagementHistoricalFile != "")
            {
                ClearInput(managementFileHistoricalFileInput);
                webDriver.FindElement(managementFileHistoricalFileInput).SendKeys(mgmtFile.ManagementHistoricalFile);
            }

            //Purpose
            AssertTrueIsDisplayed(managementFilePurposeLabel);
            if (mgmtFile.ManagementPurpose != "")
                ChooseSpecificSelectOption(managementFilePurposeSelect, mgmtFile.ManagementPurpose);

            //Additional details
            AssertTrueIsDisplayed(managementFileAdditionalDetailsLabel);
            if (mgmtFile.ManagementAdditionalDetails != "")
            {
                ClearInput(managementFileAdditionalDetailsInput);
                webDriver.FindElement(managementFileAdditionalDetailsInput).SendKeys(mgmtFile.ManagementAdditionalDetails);
            }

            //MANAGEMENT TEAM
            if (mgmtFile.ManagementTeam!.Count > 0)
            {
                while (webDriver.FindElements(managementFileViewTeamMembersGroup).Count > 0)
                    sharedTeamMembers.DeleteFirstStaffMember();

                for (var i = 0; i < mgmtFile.ManagementTeam.Count; i++)
                    sharedTeamMembers.AddMgmtTeamMembers(mgmtFile.ManagementTeam[i]);
            }
        }

        public void EditMgmtFileDetailsBttn()
        {
            Wait();
            webDriver.FindElement(managementFileDetailsEditBttn).Click();
        }

        public void SaveManagementFile()
        {
            //Save
            ButtonElement("Save");

            Wait();
            while (webDriver.FindElements(managementFileConfirmationModal).Count > 0)
            {
                if (sharedModals.ModalContent().Contains("The selected property already exists in the system's inventory"))
                {
                    Assert.Equal("User Override Required", sharedModals.ModalHeader());
                    Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.ModalContent());
                    Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();

                    Wait();
                }
                else if (sharedModals.ModalContent().Contains("You have made changes to the properties in this file."))
                {
                    Assert.Equal("Confirm changes", sharedModals.ModalHeader());
                    Assert.Contains("You have made changes to the properties in this file.", sharedModals.ModalContent());
                    Assert.Contains("Do you want to save these changes?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();

                    Wait();
                }
                else if (sharedModals.ModalHeader() == "Confirm status change")
                {
                    Assert.Equal("Confirm status change", sharedModals.ModalHeader());
                    Assert.Contains("If you save it, only the administrator can turn it back on. You will still see it in the management table.", sharedModals.ConfirmationModalParagraph1());
                    Assert.Equal("Do you want to acknowledge and proceed?", sharedModals.ConfirmationModalParagraph2());
                    sharedModals.ModalClickOKBttn();

                    Wait();
                }
            }
        }

        public void CancelManagementFile()
        {
            ButtonElement("Cancel");
            sharedModals.CancelActionModal();
        }

        public string GetManagementCode()
        {
            Wait();
            return webDriver.FindElement(managementFileHeaderFileNbrContent).Text;
        }

        public void VerifyManagementFileInitCreateForm()
        {
            Wait();

            //Create Title
            AssertTrueIsDisplayed(createManagementTitle);

            //Project
            AssertTrueIsDisplayed(managementFileProjectSubtitle);
            AssertTrueIsDisplayed(managementFileProjectLabel);
            AssertTrueIsDisplayed(managementFileProjectInput);
            AssertTrueIsDisplayed(managementFileProjectFundingLabel);
            AssertTrueIsDisplayed(managementFileProjectFundingInput);

            //Properties to include in this file
            sharedSearchProperties.VerifyLocateOnMapFeature("Management");

            //Management Details
            AssertTrueIsDisplayed(managementFileDetailsSubtitle);
            AssertTrueIsDisplayed(managementFileNameLabel);
            AssertTrueIsDisplayed(managementFileNameInput);
            AssertTrueIsDisplayed(managementFileHistoricalFileLabel);
            AssertTrueIsDisplayed(managementFileHistoricalFileInput);
            AssertTrueIsDisplayed(managementFilePurposeLabel);
            AssertTrueIsDisplayed(managementFilePurposeSelect);
            AssertTrueIsDisplayed(managementFileAdditionalDetailsLabel);
            AssertTrueIsDisplayed(managementFileAdditionalDetailsInput);

            //Management Team
            AssertTrueIsDisplayed(managementFileTeamSubtitle);
            AssertTrueIsDisplayed(managementFileAddAnotherMemberLink);
        }

        public void VerifyManagementUpdateForm()
        {
            //Title
            AssertTrueIsDisplayed(updateManagementTitle);

            //Status
            AssertTrueIsDisplayed(managementFileStatusLabel);
            AssertTrueIsDisplayed(managementFileStatusInput);

            //Project
            AssertTrueIsDisplayed(managementFileProjectSubtitle);
            AssertTrueIsDisplayed(managementFileProjectLabel);
            AssertTrueIsDisplayed(managementFileProjectInput);
            AssertTrueIsDisplayed(managementFileProjectFundingLabel);
            AssertTrueIsDisplayed(managementFileProjectFundingInput);

            //Management Details
            AssertTrueIsDisplayed(managementFileDetailsSubtitle);
            AssertTrueIsDisplayed(managementFileNameLabel);
            AssertTrueIsDisplayed(managementFileNameInput);
            AssertTrueIsDisplayed(managementFileHistoricalFileLabel);
            AssertTrueIsDisplayed(managementFileHistoricalFileInput);
            AssertTrueIsDisplayed(managementFilePurposeLabel);
            AssertTrueIsDisplayed(managementFilePurposeSelect);
            AssertTrueIsDisplayed(managementFileAdditionalDetailsLabel);
            AssertTrueIsDisplayed(managementFileAdditionalDetailsInput);

            //Management Team
            AssertTrueIsDisplayed(managementFileTeamSubtitle);
            AssertTrueIsDisplayed(managementFileAddAnotherMemberLink);
        }

        public void VerifyManagementDetailsViewForm(ManagementFile mgmtFile)
        {
            //Title
            AssertTrueIsDisplayed(viewManagementTitle);

            //Header
            AssertTrueIsDisplayed(managementFileHeaderFileNbrLabel);
            AssertTrueIsDisplayed(managementFileHeaderFileNbrContent);
            AssertTrueIsDisplayed(managementFileHeaderProjectLabel);

            if(mgmtFile.ManagementMinistryProject != "")
                AssertTrueIsDisplayed(managementFileHeaderProjectContent);

            AssertTrueIsDisplayed(managementFileHeaderProductLabel);
            if(mgmtFile.ManagementMinistryProduct != "")
                AssertTrueIsDisplayed(managementFileHeaderProductContent);

            AssertTrueIsDisplayed(managementFileHeaderCreatedDateLabel);
            AssertTrueIsDisplayed(managementFileHeaderCreatedDateContent);
            AssertTrueIsDisplayed(managementFileHeaderCreatedByContent);
            AssertTrueIsDisplayed(managementFileHeaderLastUpdateLabel);
            AssertTrueIsDisplayed(managementFileHeaderLastUpdateContent);
            AssertTrueIsDisplayed(managementFileHeaderLastUpdateByContent);

            AssertTrueIsDisplayed(managementFileHeaderHistoricalFileLabel);
            if(mgmtFile.ManagementSearchPropertiesIndex != 0)
                AssertTrueIsDisplayed(managementFileHeaderHistoricalFileContent);

            AssertTrueIsDisplayed(managementFileHeaderStatusContent);

            //Edit Icon
            AssertTrueIsDisplayed(managementFileDetailsEditBttn);

            //Status
            AssertTrueIsDisplayed(managementFileStatusLabel);
            AssertTrueContentEquals(managementFileStatusContent, mgmtFile.ManagementStatus);

            //PROJECT
            AssertTrueIsDisplayed(managementFileProjectSubtitle);
            if (mgmtFile.ManagementMinistryProject != "")
                AssertTrueContentEquals(managementFileProjectContent, mgmtFile.ManagementMinistryProjectCode + " - " + mgmtFile.ManagementMinistryProject);

            AssertTrueIsDisplayed(managementFileProjectProductLabel);
            if (mgmtFile.ManagementMinistryProduct != "")
                AssertTrueContentEquals(managementFileProjectProductContent, mgmtFile.ManagementMinistryProduct);

            //Funding
            AssertTrueIsDisplayed(managementFileProjectFundingLabel);
            if(mgmtFile.ManagementMinistryFunding != "")
                AssertTrueContentEquals(managementFileProjectFundingContent, mgmtFile.ManagementMinistryFunding);

            //MANAGEMENT DETAILS
            AssertTrueIsDisplayed(managementFileDetailsSubtitle);

            //Name
            AssertTrueIsDisplayed(managementFileNameLabelView);
            AssertTrueContentEquals(managementFileNameContent, mgmtFile.ManagementName);

            //Historical file number
            AssertTrueIsDisplayed(managementFileHistoricalFileLabel);
            if(mgmtFile.ManagementHistoricalFile != "")
                AssertTrueContentEquals(managementFileHistoricalFileContent, mgmtFile.ManagementHistoricalFile);

            //Purpose
            AssertTrueIsDisplayed(managementFilePurposeLabel);
            if (mgmtFile.ManagementPurpose != "")
                AssertTrueContentEquals(managemenetFilePurposeContent, mgmtFile.ManagementPurpose);

            //Additional details
            AssertTrueIsDisplayed(managementFileAdditionalDetailsLabel);
            if (mgmtFile.ManagementAdditionalDetails != "")
                AssertTrueContentEquals(managementFileAdditionalDetailsContent, mgmtFile.ManagementAdditionalDetails);

            //MANAGEMENT TEAM
            AssertTrueIsDisplayed(managementFileTeamSubtitle);
            if(mgmtFile.ManagementTeam!.Count > 0)
                sharedTeamMembers.VerifyTeamMembersViewForm(mgmtFile.ManagementTeam);
        }
    }
}
