

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ResearchFile : PageObjectBase
    {
        private By menuResearchButton = By.XPath("//a/label[contains(text(),'Research')]/parent::a");
        private By createResearchFileButton = By.XPath("//a[contains(text(),'Create a Research File')]");

        private By researchFileEditButton = By.CssSelector("button[title='Edit research file']");

        private By researchFileNameInput = By.Id("input-name");
        private By researchRoadNameInput = By.Id("input-roadName");
        private By researchRoadAliasInput = By.Id("input-roadAlias");
        private By researchPurposeMultiselect = By.Id("purpose-selector");
        private By researchRequestPurposeOptions = By.CssSelector("ul[class='optionContainer']");
        private By researchRequestDateInput = By.Id("datepicker-requestDate");
        private By researchRequestSourceSelect = By.Id("input-requestSourceTypeCode");
       
        private By researchDescriptionRequestTextarea = By.Id("input-requestDescription");
        private By researchCompleteDateInput = By.Id("datepicker-researchCompletionDate");
        private By researchResultTextarea = By.Id("input-researchResult");
        private By researchResultExpropiationNoRadioBttn = By.Id("input-false");
        private By researchResultExpropiationYesRadioBttn = By.Id("input-true");
        private By researchExpropiationNotes = By.Id("input-expropriationNotes");

        private By researchEditPropertiesBttn = By.XPath("//button/div[contains(text(),'Edit properties')]");

        private By researchFileConfirmationModal = By.CssSelector("div[class='modal-content']");
        private By researchFileConfirmationOkButton = By.XPath("//button[@title='ok-modal']/div[contains(text(),'Save')]/ancestor::button");

        private By selectContactButton = By.CssSelector("div[class='pl-0 col-auto'] button");

        private By researchFileHeaderCode = By.XPath("//strong[contains(text(),'R-')]");

        private SharedSelectContact sharedSelectContact;

        public ResearchFile(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
        }
        public void NavigateToCreateNewResearchFile()
        {
            Wait();
            webDriver.FindElement(menuResearchButton).Click();

            Wait();
            webDriver.FindElement(createResearchFileButton).Click();
        }

        public void CreateMinimumResearchFile(string researchFileName)
        {
            Wait();
            webDriver.FindElement(researchFileNameInput).SendKeys(researchFileName);
        }

        public void NavigateToAddPropertiesReseachFile()
        {
            WaitUntil(researchEditPropertiesBttn);
            webDriver.FindElement(researchEditPropertiesBttn).Click();
        }

        public void AddAdditionalResearchFileInfo(string roadName, string roadAlias, int purposes, string requestDate, string requester, string descriptionRequest, string researchCompletedDate, string resultRequest, Boolean expropiation, string expropiationNotes)
        {
            WaitUntil(researchFileEditButton);

            webDriver.FindElement(researchFileEditButton).Click();

            //Roads
            webDriver.FindElement(researchRoadNameInput).SendKeys(roadName);
            webDriver.FindElement(researchRoadAliasInput).SendKeys(roadAlias);

            webDriver.FindElement(researchPurposeMultiselect).Click();
            ChooseMultiSelectRandomOpions(researchRequestPurposeOptions, "optionContainer", purposes);

            //Research Request
            webDriver.FindElement(researchRequestDateInput).SendKeys(requestDate);

            ChooseSelectRandomOption(researchRequestSourceSelect, "input-requestSourceTypeCode", 2);

            Wait();
            webDriver.FindElement(selectContactButton).Click();
            sharedSelectContact.SelectContact(requester);

            Wait();
            webDriver.FindElement(researchDescriptionRequestTextarea).SendKeys(descriptionRequest);

            //Result
            webDriver.FindElement(researchCompleteDateInput).SendKeys(researchCompletedDate);
            //webDriver.FindElement(researchDescriptionRequestTextarea).Click();
          
            webDriver.FindElement(researchResultTextarea).SendKeys(resultRequest);

            //Expropiation
            if (expropiation)
            {
                FocusAndClick(researchResultExpropiationYesRadioBttn);
            } else
            {
                FocusAndClick(researchResultExpropiationNoRadioBttn);
            }

            webDriver.FindElement(researchExpropiationNotes).SendKeys(expropiationNotes);
        }

        public void SaveResearchFile()
        {
            Wait();
            ButtonElement("Save");
        }

        public void ConfirmChangesResearchFile()
        {
            WaitUntil(researchFileConfirmationModal);
            webDriver.FindElement(researchFileConfirmationOkButton).Click();
        }

        //Get the research file number
        public string GetResearchFileCode()
        {
            WaitUntil(researchFileHeaderCode);
            return webDriver.FindElement(researchFileHeaderCode).Text;
        }

    }
}
