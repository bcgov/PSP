using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ResearchFile : PageObjectBase
    {
        //Research File Menu options
        private By menuResearchButton = By.XPath("//a/label[contains(text(),'Research')]/parent::a");
        private By createResearchFileButton = By.XPath("//a[contains(text(),'Create a Research File')]");

        //Research File Edit Form Button
        private By researchFileEditButton = By.CssSelector("button[title='Edit research file']");

        //Research File Headers
        private By researchFileCreateHeader = By.XPath("//h1[contains(text(),'Create Research File')]");

        //Research File Main Form Elements
        private By researchFileStatusSelect = By.Id("input-statusTypeCode");
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
        private By researchRFileSummaryLink = By.XPath("//div[contains(text(),'RFile Summary')]");

        //Research File Confirmation Modal
        private By researchFileConfirmationModal = By.CssSelector("div[class='modal-content']");
        private By researchFileContentModal1 = By.CssSelector("div[class='modal-body'] div");
        private By researchFileContentModal2 = By.CssSelector("div[class='modal-body'] strong");
        private By researchFileConfirmationOkButton = By.XPath("//button[@title='ok-modal']/div[contains(text(),'Save')]/ancestor::button");

        //Research File Details Tab View Elements
        private By researchFileDetailsRoadSubtitle = By.XPath("//div[contains(text(),'Roads')]");
        private By researchFileDetailsRoadNameLabel = By.XPath("//label[contains(text(),'Road name')]");
        private By researchFileDetailsRoadNameInput = By.XPath("//label[contains(text(),'Road name')]/parent::div/following-sibling::div");
        private By researchFileDetailsRoadAliasLabel = By.XPath("//label[contains(text(),'Road alias')]");
        private By researchFileDetailsRoadAliasInput = By.XPath("//label[contains(text(),'Road alias')]/parent::div/following-sibling::div");
        private By researchFileDetailsResearchRequestSubtitle = By.XPath("//div[contains(text(),'Research Request')]");
        private By researchFileDetailsRequestPurposeLabel = By.XPath("//label[contains(text(),'Research purpose')]");
        private By researchFileDetailsRequestPurposeInput = By.XPath("//label[contains(text(),'Research purpose')]/parent::div/following-sibling::div");
        private By researchFileDetailsRequestDateLabel = By.XPath("//label[contains(text(),'Request date')]");
        private By researchFileDetailsRequestDateInput = By.XPath("//label[contains(text(),'Request date')]/parent::div/following-sibling::div");
        private By researchFileDetailsRequestSourceLabel = By.XPath("//label[contains(text(),'Source of request')]");
        private By researchFileDetailsRequestSourceInput = By.XPath("//label[contains(text(),'Source of request')]/parent::div/following-sibling::div");
        private By researchFileDetailsRquesterLabel = By.XPath("//label[contains(text(),'Requester')]");
        private By researchFileDetailsRequesterInput = By.XPath("//label[contains(text(),'Requester')]/parent::div/following-sibling::div");
        private By researchFileDetailsRequestDescripLabel = By.XPath("//label[contains(text(),'Description of request')]");
        private By researchFileDetailsResultSubtitle = By.XPath("//div[@class='row']/div[contains(text(),'Result')]");
        private By researchFileDetailsResultCompleteLabel = By.XPath("//label[contains(text(),'Research completed on')]");
        private By researchFileDetailsResultCompleteInput = By.XPath("//label[contains(text(),'Research completed on')]/parent::div/following-sibling::div");
        private By researchFileDetailsResultRequestLabel = By.XPath("//label[contains(text(),'Result of request')]");
        private By researchFileDetailsExpropriationSubtitle = By.XPath("//div[@class='row']/div[contains(text(),'Expropriation')]");
        private By researchFileDetailsExpropriationLabel = By.XPath("//label[contains(text(),'Expropriation?')]");
        private By researchFileDetailsExpropriationInput = By.XPath("//label[contains(text(),'Expropriation?')]/parent::div/following-sibling::div");
        private By researchFileDetailsExpropriationNotesLabel = By.XPath("//label[contains(text(),'Expropriation notes')]");

        private By selectContactButton = By.CssSelector("div[class='pl-0 col-auto'] button");

        private By researchFileHeaderCode = By.XPath("//strong[contains(text(),'R-')]");

        //Research File - Properties Elements
        private By researchProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");
        private By researchPropertyResearchEditBttn = By.XPath("(//button[@class='btn btn-link'])[1]");
        private By researchPropertyNameInput = By.Id("input-propertyName");
        private By researchPropertyPurposeSelect = By.Id("purpose-selector_input");
        private By researchPropertyPurposeOptions = By.CssSelector("ul[class='optionContainer']");
        private By researchPropertyLegalOpinionReqSelect = By.Id("input-isLegalOpinionRequired");
        private By researchPropertyLegalOpinionObtSelect = By.Id("input-isLegalOpinionObtained");
        private By researchPropertyDocReferenceInput = By.Id("input-documentReference");
        private By researchPropertyNotesTextArea = By.Id("input-researchSummary");

        // Research File - Property UI/UX Elements
        private By researchFilePropertyCountProps = By.XPath("//div[@data-testid='menu-item-row-0']/parent::div");

        // Research File - Property Research Elements
        private By researchPropertyInterestLabel = By.XPath("//div[contains(text(),'Property of Interest')]");
        private By researchPropertyNameLabel = By.XPath("//label[contains(text(),'Descriptive name')]");
        private By researchPropertyNameViewInput = By.XPath("//label[contains(text(),'Descriptive name')]/parent::div/following-sibling::div");
        private By researchPropertyPurposeLabel = By.XPath("//label[contains(text(),'Purpose')]");
        private By researchPropertyPurposeViewInput = By.XPath("//label[contains(text(),'Purpose')]/parent::div/following-sibling::div");
        private By researchProperty1stPurposeDeleteLink = By.CssSelector("div[id='purpose-selector'] div span:nth-child(1) i");
        private By researchPropertyLegalReqLabel = By.XPath("//label[contains(text(),'Legal opinion req')]");
        private By researchPropertyLegalReqViewInput = By.XPath("//label[contains(text(),'Legal opinion req')]/parent::div/following-sibling::div");
        private By researchPropertyLegalObtLabel = By.XPath("//label[contains(text(),'Legal opinion obtained')]");
        private By researchPropertyLegalObtViewInput = By.XPath("//label[contains(text(),'Legal opinion obtained')]/parent::div/following-sibling::div");
        private By researchPropertyDocRefLabel = By.XPath("//label[contains(text(),'Document reference')]");
        private By researchPropertyDocRefInput = By.XPath("//label[contains(text(),'Document reference')]/parent::div/following-sibling::div");

        private By researchPropertySummaryLabel = By.XPath("//div[contains(text(),'Research Summary')]");
        private By researchPropertyNotesLabel = By.XPath("//label[contains(text(),'Summary notes')]");
        private By researchPropertyNotesViewInput = By.XPath("//label[contains(text(),'Summary notes')]/parent::div/following-sibling::div");

        private SharedSelectContact sharedSelectContact;
        private SharedModals sharedModals;

        private int totalAssociatedProps = 0;

        public ResearchFile(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
        }
        public void NavigateToCreateNewResearchFile()
        {
            Wait(2000);
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
            Wait(2000);
            totalAssociatedProps = webDriver.FindElements(researchFilePropertyCountProps).Count() -1;

            Wait(2000);
            if (webDriver.FindElements(researchRFileSummaryLink).Count() > 0)
            {
                webDriver.FindElement(researchRFileSummaryLink).Click();
            }
            webDriver.FindElement(researchEditPropertiesBttn).Click();
        }

        public void ChooseFirstPropertyOption()
        {
            Wait();
            webDriver.FindElement(researchProperty1stPropLink).Click();
        }

        public void AddAdditionalResearchFileInfo(string roadName, string roadAlias, int purposes, string requestDate, string requester, string descriptionRequest, string researchCompletedDate, string resultRequest, Boolean expropiation, string expropiationNotes)
        {
            WaitUntil(researchFileEditButton);
            webDriver.FindElement(researchFileEditButton).Click();

            //Roads
            webDriver.FindElement(researchRoadNameInput).SendKeys(roadName);
            webDriver.FindElement(researchRoadAliasInput).SendKeys(roadAlias);

            webDriver.FindElement(researchPurposeMultiselect).Click();
            ChooseMultiSelectRandomOptions(researchRequestPurposeOptions, purposes);

            //Research Request
            webDriver.FindElement(researchRequestDateInput).SendKeys(requestDate);

            ChooseRandomSelectOption(researchRequestSourceSelect, 2);

            Wait();
            webDriver.FindElement(selectContactButton).Click();
            sharedSelectContact.SelectContact(requester);

            Wait();
            webDriver.FindElement(researchDescriptionRequestTextarea).SendKeys(descriptionRequest);

            //Result
            webDriver.FindElement(researchCompleteDateInput).SendKeys(researchCompletedDate);
            webDriver.FindElement(researchCompleteDateInput).SendKeys(Keys.Enter);
          
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

        public void EditResearchFileForm()
        {
            Wait();

            WaitUntil(researchFileEditButton);
            webDriver.FindElement(researchFileEditButton).Click();

            ChooseSpecificSelectOption(researchFileStatusSelect, "Archived");
            webDriver.FindElement(researchFileNameInput).SendKeys(" - Automated Edition.");
            ClearInput(researchExpropiationNotes);
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

        public void CancelResearchFile()
        {
            Wait();
            ButtonElement("Cancel");

            try {
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(3));
                if (wait.Until(ExpectedConditions.AlertIsPresent()) != null)
                {
                    webDriver.SwitchTo().Alert().Accept();
                }
            } catch (WebDriverTimeoutException e)
            {
                if (webDriver.FindElements(researchFileConfirmationModal).Count() > 0)
                {
                    Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
                    Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this research file will not be saved."));
                    Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));

                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        public void CancelResearchFileProps()
        {
            Wait();
            ButtonElement("Cancel");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this file will not be saved."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));

            sharedModals.ModalClickOKBttn();
        }

        public void CancelResearchFilePropertyDetails()
        {
            Wait();
            ButtonElement("Cancel");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this research file will not be saved."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));

            sharedModals.ModalClickOKBttn();
        }

        public void AddPropertyResearchMinInfo(string propertyName)
        {

            Wait();
            webDriver.FindElement(researchPropertyResearchEditBttn).Click();

            Wait();
            webDriver.FindElement(researchPropertyNameInput).SendKeys(propertyName);

        }

        public void AddPropertyResearchMaxInfo(string propertyName, string docReference, string notes)
        {

            Wait();
            webDriver.FindElement(researchPropertyResearchEditBttn).Click();

            Wait();
            webDriver.FindElement(researchPropertyNameInput).SendKeys(propertyName);
            webDriver.FindElement(researchPropertyPurposeSelect).Click();
            ChooseMultiSelectRandomOptions(researchPropertyPurposeOptions, 3);
            ChooseRandomSelectOption(researchPropertyLegalOpinionReqSelect, 1);
            ChooseRandomSelectOption(researchPropertyLegalOpinionObtSelect, 1);
            webDriver.FindElement(researchPropertyDocReferenceInput).SendKeys(docReference);
            webDriver.FindElement(researchPropertyNotesTextArea).SendKeys(notes);

        }

        public void EditPropertyResearch(string notes)
        {
            Wait();
            webDriver.FindElement(researchPropertyResearchEditBttn).Click();

            Wait();
            webDriver.FindElement(researchPropertyPurposeSelect).Click();
            ChooseMultiSelectRandomOptions(researchPropertyPurposeOptions, 1);
            webDriver.FindElement(researchProperty1stPurposeDeleteLink).Click();
            ClearInput(researchPropertyNotesTextArea);
            webDriver.FindElement(researchPropertyNotesTextArea).SendKeys(notes);
        }

        //Get the research file number
        public string GetResearchFileCode()
        {
            WaitUntil(researchFileHeaderCode);
            return webDriver.FindElement(researchFileHeaderCode).Text;
        }

        public Boolean PropertiesCountChange()
        {
            var currentPropsCount = webDriver.FindElements(researchFilePropertyCountProps).Count() - 1;
            return !currentPropsCount.Equals(totalAssociatedProps);
        }

        //Verify UI/UX Elements - Research Main Form - View Form
        public void VerifyResearchFileMainFormView(string roadName, string roadAlias, string requestDate, string requester, string completeDate, Boolean expropriation)
        {
            Wait();
            Assert.True(webDriver.FindElement(researchFileDetailsRoadSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadNameInput).Text.Equals(roadName));
            Assert.True(webDriver.FindElement(researchFileDetailsRoadAliasLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadAliasInput).Text.Equals(roadAlias));
            Assert.True(webDriver.FindElement(researchFileDetailsResearchRequestSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestPurposeInput).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDateLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDateInput).Text.Equals(requestDate));
            Assert.True(webDriver.FindElement(researchFileDetailsRequestSourceLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestSourceInput).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRquesterLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequesterInput).Text.Equals(requester));
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDescripLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsResultSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsResultCompleteLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsResultCompleteInput).Text.Equals(completeDate));
            Assert.True(webDriver.FindElement(researchFileDetailsResultRequestLabel).Displayed);

            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationLabel).Displayed);
            if (expropriation)
            {
                Assert.True(webDriver.FindElement(researchFileDetailsExpropriationInput).Text.Equals("Yes"));
            }
            else
            {
                Assert.True(webDriver.FindElement(researchFileDetailsExpropriationInput).Text.Equals("No"));
            }
            
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationNotesLabel).Displayed);
        }

        //Verify UI/UX Elements - Research Properties - Property Research - View Form
        public void VerifyPropResearchTabFormView(string propertyName, string referenceDoc)
        {
            Wait();
            Assert.True(webDriver.FindElement(researchPropertyInterestLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyNameLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyNameViewInput).Text.Equals(propertyName));
            Assert.True(webDriver.FindElement(researchPropertyPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyPurposeViewInput).Text != null);
            Assert.True(webDriver.FindElement(researchPropertyLegalReqLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyLegalReqViewInput).Text != null);
            Assert.True(webDriver.FindElement(researchPropertyLegalObtLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyLegalObtViewInput).Text != null);
            Assert.True(webDriver.FindElement(researchPropertyDocRefLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyDocRefInput).Text.Equals(referenceDoc));
            Assert.True(webDriver.FindElement(researchPropertySummaryLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyNotesLabel).Displayed);
        }

        public int HeaderIsDisplayed()
        {
            return webDriver.FindElements(researchFileCreateHeader).Count();
        }

    }
}
