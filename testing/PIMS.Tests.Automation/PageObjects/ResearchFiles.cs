using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ResearchFiles : PageObjectBase
    {
        //Research File Menu options
        private By menuResearchButton = By.XPath("//a/label[contains(text(),'Research')]/parent::a");
        private By createResearchFileButton = By.XPath("//a[contains(text(),'Create a Research File')]");

        //Research File Edit Form Button
        private By researchFileEditButton = By.CssSelector("button[title='Edit research file']");

        //Research Create Init Elements
        private By researchFileCreateHeader = By.XPath("//h1[contains(text(),'Create Research File')]");
        private By researchFileNameLabel = By.XPath("//strong[contains(text(),'Name this research file')]");
        private By researchFileHelpNameTooltip = By.XPath("//div[contains(text(),'Help with choosing a name')]");

        //Research File Tabs and File Summary Elements
        private By researchFileSummaryBttn = By.XPath("//div[contains(text(),'File Summary')]");
        private By researchFileDetailsTab = By.CssSelector("a[data-rb-event-key='fileDetails']");
        private By researchFileDocumentsTab = By.CssSelector("a[data-rb-event-key='documents']");
        private By researchFileNotesTab = By.CssSelector("a[data-rb-event-key='notes']");

        //Research File Main Form Elements
        private By researchFileDetailsProjectAddBttn = By.CssSelector("button[data-testid='add-project']");
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

        private By researchFilePropertiesLeftSection = By.XPath("//span[contains(text(),'Properties')]");
        private By researchEditPropertiesBttn = By.CssSelector("button[title='Change properties']");

        //Research File Confirmation Modal
        private By researchFileConfirmationModal = By.CssSelector("div[class='modal-content']");
        private By researchFileConfirmationOkButton = By.XPath("//button[@title='ok-modal']/div[contains(text(),'Save')]/ancestor::button");

        //Research File View Form Elements
        //Header
        private By researchFileViewTitle = By.XPath("//h1[contains(text(),'Research File')]");
        private By researchFileHeaderNbrLabel = By.XPath("//label[contains(text(),'File #')]");
        private By researchFileHeaderNbrContent = By.XPath("//label[contains(text(),'File #')]/parent::div/following-sibling::div/strong");
        private By researchFileHeaderNameLabel = By.XPath("//label[contains(text(),'File name')]");
        private By researchFileHeaderNameContent = By.XPath("//label[contains(text(),'File name')]/parent::div/following-sibling::div/strong");
        private By researchFileHeaderMOTIRegionLabel = By.XPath("//label[contains(text(),'MoTI region')]");
        private By researchFileHeaderMOTIRegionContent = By.XPath("//label[contains(text(),'MoTI region')]/parent::div/following-sibling::div/strong");
        private By researchFileHeaderDistrictLabel = By.XPath("//label[contains(text(),'Ministry district')]");
        private By researchFileHeaderDistrictContent = By.XPath("//label[contains(text(),'Ministry district')]/parent::div/following-sibling::div/strong");
        private By researchFileHeaderCreatedLabel = By.XPath("//span[contains(text(),'Created')]");
        private By researchFileHeaderCreatedDateContent = By.XPath("//span[contains(text(),'Created')]/strong");
        private By researchFileHeaderCreatedByContent = By.XPath("//span[contains(text(),'Created')]/span/strong");
        private By researchFileHeaderLastUpdatedLabel = By.XPath("//span[contains(text(),'Last updated')]");
        private By researchFileHeaderLastUpdatedDateContent = By.XPath("//span[contains(text(),'Last updated')]/strong");
        private By researchFileHeaderLastUpdatedByContent = By.XPath("//span[contains(text(),'Last updated')]/span/strong");
        private By researchFileHeaderStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private By researchFileHeaderStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div/strong");

        //Research File Details Tab View Elements
        private By researchFileDetailsProjectSubtitle = By.XPath("//h2/div/div[contains(text(),'Project')]");
        private By reserachFileDetailsProjectLabel = By.XPath("//label[contains(text(),'Ministry project')]");
        private By researchFileDetailsProjectsCount = By.XPath("//div[contains(text(),'Project')]/parent::div/parent::h2/following-sibling::div/div/div/div");
        private By researchFileDetailsProjectsRemoveBttn = By.CssSelector("svg[data-testid='remove-button']");

        private By researchFileDetailsRoadSubtitle = By.XPath("//div[contains(text(),'Roads')]");
        private By researchFileDetailsRoadNameLabel = By.XPath("//label[contains(text(),'Road name')]");
        private By researchFileDetailsRoadNameInput = By.XPath("//label[contains(text(),'Road name')]/parent::div/following-sibling::div");
        private By researchFileDetailsRoadAliasLabel = By.XPath("//label[contains(text(),'Road alias')]");
        private By researchFileDetailsRoadAliasInput = By.XPath("//label[contains(text(),'Road alias')]/parent::div/following-sibling::div");

        private By researchFileDetailsResearchRequestSubtitle = By.XPath("//div[contains(text(),'Research Request')]");
        private By researchFileDetailsRequestPurposeLabel = By.XPath("//label[contains(text(),'Research purpose')]");
        private By researchFileDetailsRequestPurposeInput = By.XPath("//label[contains(text(),'Research purpose')]/parent::div/following-sibling::div");
        private By researchFileDetailsRequestPurposeRemoveBttn = By.CssSelector("i[class='custom-close']");
        private By researchFileDetailsRequestDateLabel = By.XPath("//label[contains(text(),'Request date')]");
        private By researchFileDetailsRequestDateInput = By.XPath("//label[contains(text(),'Request date')]/parent::div/following-sibling::div");
        private By researchFileDetailsRequestSourceLabel = By.XPath("//label[contains(text(),'Source of request')]");
        private By researchFileDetailsRequestSourceInput = By.XPath("//label[contains(text(),'Source of request')]/parent::div/following-sibling::div");
        private By researchFileDetailsRquesterLabel = By.XPath("//label[contains(text(),'Requester')]");
        private By researchFileDetailsRequesterInput = By.XPath("//label[contains(text(),'Requester')]/parent::div/following-sibling::div");
        private By researchFileDetailsRequestDescripLabel = By.XPath("//label[contains(text(),'Description of request')]");

        private By researchFileDetailsResultSubtitle = By.XPath("//div[@class='no-gutters row']/div[contains(text(),'Result')]");
        private By researchFileDetailsResultCompleteLabel = By.XPath("//label[contains(text(),'Research completed on')]");
        private By researchFileDetailsResultCompleteInput = By.XPath("//label[contains(text(),'Research completed on')]/parent::div/following-sibling::div");
        private By researchFileDetailsResultRequestLabel = By.XPath("//label[contains(text(),'Result of request')]");
        private By researchFileDetailsExpropriationSubtitle = By.XPath("//div[@class='no-gutters row']/div[contains(text(),'Expropriation')]");
        private By researchFileDetailsExpropriationLabel = By.XPath("//label[contains(text(),'Expropriation?')]");
        private By researchFileDetailsExpropriationInput = By.XPath("//label[contains(text(),'Expropriation?')]/parent::div/following-sibling::div");
        private By researchFileDetailsExpropriationNotesLabel = By.XPath("//label[contains(text(),'Expropriation notes')]");

        private By selectContactButton = By.CssSelector("div[class='pl-0 col-auto'] button");

        private By researchFileHeaderCode = By.XPath("//strong[contains(text(),'R-')]");

        //Research File - Properties Elements
        private By researchProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");
        private By researchPropertyResearchEditBttn = By.XPath("(//button[@class='btn btn-link'])[2]");
        private By researchPropertyNameInput = By.Id("input-propertyName");
        private By researchPropertyPurposeSelect = By.Id("purpose-selector_input");
        private By researchPropertyPurposeDiv = By.XPath("//input[@id='purpose-selector_input']/parent::div");
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
        private SharedSearchProperties sharedSearchProperties;

        private int totalAssociatedProps = 0;

        public ResearchFiles(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
            sharedSearchProperties = new SharedSearchProperties(webDriver);
        }
        public void NavigateToCreateNewResearchFile()
        {
            Wait(3000);
            FocusAndClick(menuResearchButton);

            Wait(3000);
            FocusAndClick(createResearchFileButton);
        }

        public void NavigateToFileSummary()
        {
            WaitUntilClickable(researchFileSummaryBttn);
            webDriver.FindElement(researchFileSummaryBttn).Click();
        }

        public void NavigateToAddPropertiesReseachFile()
        {
            Wait(2000);
            totalAssociatedProps = webDriver.FindElements(researchFilePropertyCountProps).Count() - 1;

            WaitUntilClickable(researchEditPropertiesBttn);
            webDriver.FindElement(researchEditPropertiesBttn).Click();
        }

        public void ChooseFirstPropertyOption()
        {
            WaitUntilClickable(researchProperty1stPropLink);
            webDriver.FindElement(researchProperty1stPropLink).Click();

            //sharedModals.SiteMinderModal();
        }

        public void CreateResearchFile(ResearchFile researchFile)
        {
            WaitUntilVisible(researchFileNameInput);
            webDriver.FindElement(researchFileNameInput).SendKeys(researchFile.ResearchFileName);
        }   

        public void AddAdditionalResearchFileInfo(ResearchFile researchFile)
        {
            WaitUntilVisible(researchFileEditButton);
            webDriver.FindElement(researchFileEditButton).Click();

            //Status
            if (researchFile.Status != "")
            {
                ChooseSpecificSelectOption(researchFileStatusSelect, researchFile.Status);
            }

            //Projects
            if (researchFile.Projects.First() != "")
            {
                for (int i = 0; i < researchFile.Projects.Count; i++)
                {
                    webDriver.FindElement(researchFileDetailsProjectAddBttn).Click();

                    By projectInput = By.Id("typeahead-researchFileProjects["+ i +"].project");
                    webDriver.FindElement(projectInput).SendKeys(researchFile.Projects[i]);

                    Wait();
                    webDriver.FindElement(projectInput).SendKeys(Keys.Space);

                    Wait();
                    webDriver.FindElement(projectInput).SendKeys(Keys.Backspace);

                    By projectOptions = By.Id("typeahead-researchFileProjects["+ i +"].project-item-0");
                    WaitUntilVisible(projectOptions);
                    webDriver.FindElement(projectOptions).Click();
                }
            }

            //Roads
            if (researchFile.RoadName != "")
            {
                webDriver.FindElement(researchRoadNameInput).SendKeys(researchFile.RoadName);
            }
            if (researchFile.RoadAlias != "")
            {
                webDriver.FindElement(researchRoadAliasInput).SendKeys(researchFile.RoadAlias);
            }

            //Research Request
            if (researchFile.ResearchPurpose.First() != "")
            {
                foreach (string purpose in researchFile.ResearchPurpose)
                {
                    webDriver.FindElement(researchPurposeMultiselect).Click();
                    ChooseMultiSelectSpecificOption(researchRequestPurposeOptions, purpose);
                }
            }
            if (researchFile.RequestDate != "")
            {
                webDriver.FindElement(researchRequestDateInput).SendKeys(researchFile.RequestDate);
                webDriver.FindElement(researchRequestDateInput).SendKeys(Keys.Enter);
            }
            if (researchFile.RequestSource != "")
            {
                ChooseSpecificSelectOption(researchRequestSourceSelect, researchFile.RequestSource);
            }
            if (researchFile.Requester != "")
            {
                webDriver.FindElement(selectContactButton).Click();
                sharedSelectContact.SelectContact(researchFile.Requester, "");
            }
            if (researchFile.RequestDescription != "")
            {
                WaitUntilVisible(researchDescriptionRequestTextarea);
                webDriver.FindElement(researchDescriptionRequestTextarea).SendKeys(researchFile.RequestDescription);
            }

            //Result
            if (researchFile.ResearchCompletedDate != "")
            {
                webDriver.FindElement(researchCompleteDateInput).SendKeys(researchFile.ResearchCompletedDate);
                webDriver.FindElement(researchCompleteDateInput).SendKeys(Keys.Enter);
            }
            if (researchFile.RequestResult != "")
            {
                webDriver.FindElement(researchResultTextarea).SendKeys(researchFile.RequestResult);
            }  

            //Expropiation
            if (researchFile.Expropriation)
            {
                FocusAndClick(researchResultExpropiationYesRadioBttn);
            } else
            {
                FocusAndClick(researchResultExpropiationNoRadioBttn);
            }
            if (researchFile.ExpropriationNotes != "")
            {
                webDriver.FindElement(researchExpropiationNotes).SendKeys(researchFile.ExpropriationNotes);
            }
        }

        public void AddPropertyResearchInfo(PropertyResearch propertyResearch, int index)
        {
            //Pick Property
            var elementIndex = index + 1;
            By propertyLink = By.CssSelector("div[data-testid='menu-item-row-" + elementIndex + "'] div:nth-child(3)");
            WaitUntilClickable(propertyLink);
            webDriver.FindElement(propertyLink).Click();

            //Add Property Research Information
            WaitUntilClickable(researchPropertyResearchEditBttn);
            webDriver.FindElement(researchPropertyResearchEditBttn).Click();

            if (propertyResearch.DescriptiveName != "")
            {
                WaitUntilClickable(researchPropertyNameInput);
                webDriver.FindElement(researchPropertyNameInput).SendKeys(propertyResearch.DescriptiveName);
            }
            if (propertyResearch.Purpose != "")
            {
                webDriver.FindElement(researchPropertyPurposeSelect).Click();

                WaitUntilVisible(researchPropertyPurposeOptions);
                ChooseMultiSelectSpecificOption(researchPropertyPurposeOptions, propertyResearch.Purpose);
            }
            if (propertyResearch.LegalOpinionRequest != "")
            {
                ChooseSpecificSelectOption(researchPropertyLegalOpinionReqSelect, propertyResearch.LegalOpinionRequest);
            }
            if (propertyResearch.LegalOpinionObtained != "")
            {
                ChooseSpecificSelectOption(researchPropertyLegalOpinionObtSelect, propertyResearch.LegalOpinionObtained);
            }
            if (propertyResearch.DocumentReference != "")
            {
                webDriver.FindElement(researchPropertyDocReferenceInput).SendKeys(propertyResearch.DocumentReference);
            }
            if (propertyResearch.SummaryNotes != "")
            {
                webDriver.FindElement(researchPropertyNotesTextArea).SendKeys(propertyResearch.SummaryNotes);
            }
        }

        public void EditResearchFileForm(ResearchFile researchFile)
        {
            WaitUntilVisible(researchFileEditButton);
            webDriver.FindElement(researchFileEditButton).Click();

            //Projects
            //Delete previous projects if any
            if (webDriver.FindElements(researchFileDetailsProjectsRemoveBttn).Count > 0)
            {
                while (webDriver.FindElements(researchFileDetailsProjectsRemoveBttn).Count > 0)
                {
                    webDriver.FindElements(researchFileDetailsProjectsRemoveBttn)[0].Click();
                }
            }
            if (researchFile.Projects.First() != "")
            {
                //Add new projects
                for (int i = 0; i < researchFile.Projects.Count; i++)
                {
                    webDriver.FindElement(researchFileDetailsProjectAddBttn).Click();

                    By projectInput = By.Id("typeahead-researchFileProjects[" + i + "].project");
                    webDriver.FindElement(projectInput).SendKeys(researchFile.Projects[i]);

                    By projectOptions = By.Id("typeahead-researchFileProjects[" + i + "].project-item-0");
                    WaitUntilClickable(projectOptions);
                    webDriver.FindElement(projectOptions).Click();
                }
            }

            //Roads
            if (researchFile.RoadName != "")
            {
                ClearInput(researchRoadNameInput);
                webDriver.FindElement(researchRoadNameInput).SendKeys(researchFile.RoadName);
            }
            if (researchFile.RoadAlias != "")
            {
                ClearInput(researchRoadAliasInput);
                webDriver.FindElement(researchRoadAliasInput).SendKeys(researchFile.RoadAlias);
            }

            //Research Request

            //Delete previous research purposes if any
            if (webDriver.FindElements(researchFileDetailsRequestPurposeRemoveBttn).Count > 0)
            {
                while (webDriver.FindElements(researchFileDetailsRequestPurposeRemoveBttn).Count > 0)
                {
                    webDriver.FindElements(researchFileDetailsRequestPurposeRemoveBttn)[0].Click();
                }
            }
            if (researchFile.ResearchPurpose.First() != "")
            {
                //Add new Research Purpose
                foreach (string purpose in researchFile.ResearchPurpose)
                {
                    webDriver.FindElement(researchPurposeMultiselect).Click();
                    ChooseMultiSelectSpecificOption(researchRequestPurposeOptions, purpose);
                }
            }
            if (researchFile.RequestDate != "")
            {
                ClearInput(researchRequestDateInput);
                webDriver.FindElement(researchRequestDateInput).SendKeys(researchFile.RequestDate);
                webDriver.FindElement(researchRequestDateInput).SendKeys(Keys.Enter);
            }
            if (researchFile.RequestSource != "")
            {
                ChooseSpecificSelectOption(researchRequestSourceSelect, researchFile.RequestSource);
            }
            if (researchFile.Requester != "")
            {
                webDriver.FindElement(selectContactButton).Click();
                sharedSelectContact.SelectContact(researchFile.Requester, "");
            }
            if (researchFile.RequestDescription != "")
            {
                ClearInput(researchDescriptionRequestTextarea);
                webDriver.FindElement(researchDescriptionRequestTextarea).SendKeys(researchFile.RequestDescription);
            }

            //Result
            if (researchFile.ResearchCompletedDate != "")
            {
                ClearInput(researchCompleteDateInput);
                webDriver.FindElement(researchCompleteDateInput).SendKeys(researchFile.ResearchCompletedDate);
                webDriver.FindElement(researchCompleteDateInput).SendKeys(Keys.Enter);
            }
            if (researchFile.RequestResult != "")
            {
                ClearInput(researchResultTextarea);
                webDriver.FindElement(researchResultTextarea).SendKeys(researchFile.RequestResult);
            }

            //Expropiation
            if (researchFile.Expropriation)
            {
                FocusAndClick(researchResultExpropiationYesRadioBttn);
            }
            else
            {
                FocusAndClick(researchResultExpropiationNoRadioBttn);
            }
            if (researchFile.ExpropriationNotes != "")
            {
                ClearInput(researchExpropiationNotes);
                webDriver.FindElement(researchExpropiationNotes).SendKeys(researchFile.ExpropriationNotes);
            }
        }

        public void EditPropertyResearchInfo(PropertyResearch propertyResearch, int index)
        {
            //Pick Property
            var elementIndex = index + 1;
            By propertyLink = By.CssSelector("div[data-testid='menu-item-row-" + elementIndex + "'] div:nth-child(3)");
            WaitUntilClickable(propertyLink);
            webDriver.FindElement(propertyLink).Click();

            //Add Property Research Information
            WaitUntilClickable(researchPropertyResearchEditBttn);
            webDriver.FindElement(researchPropertyResearchEditBttn).Click();

            if (propertyResearch.DescriptiveName != "")
            {
                ClearInput(researchPropertyNameInput);
                webDriver.FindElement(researchPropertyNameInput).SendKeys(propertyResearch.DescriptiveName);
            }
            if (propertyResearch.Purpose != "")
            {
                ClearMultiSelectInput(researchPropertyPurposeDiv);
                webDriver.FindElement(researchPropertyPurposeSelect).Click();
                ChooseMultiSelectSpecificOption(researchPropertyPurposeOptions, propertyResearch.Purpose);
            }
            if (propertyResearch.LegalOpinionRequest != "")
            {
                ChooseSpecificSelectOption(researchPropertyLegalOpinionReqSelect, propertyResearch.LegalOpinionRequest);
            }
            if (propertyResearch.LegalOpinionObtained != "")
            {
                ChooseSpecificSelectOption(researchPropertyLegalOpinionObtSelect, propertyResearch.LegalOpinionObtained);
            }
            if (propertyResearch.DocumentReference != "")
            {
                ClearInput(researchPropertyDocReferenceInput);
                webDriver.FindElement(researchPropertyDocReferenceInput).SendKeys(propertyResearch.DocumentReference);
            }
            if (propertyResearch.SummaryNotes != "")
            {
                ClearInput(researchPropertyNotesTextArea);
                webDriver.FindElement(researchPropertyNotesTextArea).SendKeys(propertyResearch.SummaryNotes);
            }
        }

        public void SaveResearchFile()
        {
            ButtonElement("Save");
        }

        public void SaveResearchFileProperties()
        {
            Wait();
            ButtonElement("Save");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("You have made changes to the properties in this file."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Do you want to save these changes?"));

            sharedModals.ModalClickOKBttn();

            Wait();
            if (webDriver.FindElements(researchFileConfirmationModal).Count() > 1)
            {
                Assert.True(sharedModals.SecondaryModalHeader().Equals("User Override Required"));
                Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.SecondaryModalContent());
                Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.SecondaryModalContent());
                sharedModals.SecondaryModalClickOKBttn();
            }
        }

        public void CancelResearchFile()
        {
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
            ButtonElement("Cancel");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this file will not be saved."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));

            sharedModals.ModalClickOKBttn();
        }

        public void CancelResearchFilePropertyDetails()
        {
            ButtonElement("Cancel");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this research file will not be saved."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));

            sharedModals.ModalClickOKBttn();
        }

        //Get the research file number
        public string GetResearchFileCode()
        {
            WaitUntilVisible(researchFileHeaderCode);
            return webDriver.FindElement(researchFileHeaderCode).Text;
        }

        //Verify Create Research Init Form
        public void VerifyResearchFileCreateInitForm()
        {
            Wait(2000);

            //Title and Name
            Assert.True(webDriver.FindElement(researchFileCreateHeader).Displayed);
            Assert.True(webDriver.FindElement(researchFileNameLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileNameInput).Displayed);
            Assert.True(webDriver.FindElement(researchFileHelpNameTooltip).Displayed);

            //Project
            Assert.True(webDriver.FindElement(researchFileDetailsProjectSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsProjectAddBttn).Displayed);

            //Properties to include
            sharedSearchProperties.VerifyLocateOnMapFeature();
        }

        //Verify Edit Research File Init Form
        public void VerifyResearchFileEditInitForm(ResearchFile researchFile, string user)
        {
            WaitUntilVisible(researchFileSummaryBttn);

            //Header
            VerifyResearchFileHeader(researchFile, user);
            Assert.True(webDriver.FindElement(researchFileHeaderStatusContent).Text.Equals("Active"));

            //Left Bar Elements
            Assert.True(webDriver.FindElement(researchFileSummaryBttn).Displayed);
            Assert.True(webDriver.FindElement(researchFilePropertiesLeftSection).Displayed);
            Assert.True(webDriver.FindElement(researchEditPropertiesBttn).Displayed);

            //Main section Elements
            Assert.True(webDriver.FindElement(researchFileDetailsTab).Displayed);
            Assert.True(webDriver.FindElement(researchFileDocumentsTab).Displayed);
            Assert.True(webDriver.FindElement(researchFileNotesTab).Displayed);

            //File Details Elements
            //Project
            Assert.True(webDriver.FindElement(researchFileDetailsProjectSubtitle).Displayed);
            Assert.True(webDriver.FindElement(reserachFileDetailsProjectLabel).Displayed);

            //Roads
            Assert.True(webDriver.FindElement(researchFileDetailsRoadSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadAliasLabel).Displayed);

            //Research Request
            Assert.True(webDriver.FindElement(researchFileDetailsResearchRequestSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDateLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestSourceLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRquesterLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDescripLabel).Displayed);

            //Result
            Assert.True(webDriver.FindElement(researchFileDetailsResultSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsResultCompleteLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsResultRequestLabel).Displayed);

            //Expropriation
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationNotesLabel).Displayed);
        }

        //Verify UI/UX Elements - Research Main Form - View Form
        public void VerifyResearchFileMainFormView(ResearchFile researchFile, string user)
        {
            Wait(2000);

            //Header
            VerifyResearchFileHeader(researchFile, user);
            Assert.True(webDriver.FindElement(researchFileHeaderStatusContent).Text.Equals(researchFile.Status));

            //Project
            Assert.True(webDriver.FindElement(researchFileDetailsProjectSubtitle).Displayed);
            Assert.True(webDriver.FindElement(reserachFileDetailsProjectLabel).Displayed);

            if (researchFile.Projects.First() != "")
            {
                var projectsUI = GetProjects(researchFileDetailsProjectsCount);
                Assert.True(Enumerable.SequenceEqual(projectsUI, researchFile.Projects));
            }

            //Roads
            Assert.True(webDriver.FindElement(researchFileDetailsRoadSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadNameInput).Text.Equals(researchFile.RoadName));
            Assert.True(webDriver.FindElement(researchFileDetailsRoadAliasLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRoadAliasInput).Text.Equals(researchFile.RoadAlias));

            //Research Request
            Assert.True(webDriver.FindElement(researchFileDetailsResearchRequestSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestPurposeLabel).Displayed);
            if (researchFile.ResearchPurpose.First() != "")
            {
                var purposesUI = GetProjects(researchFileDetailsProjectsCount);
                Assert.True(Enumerable.SequenceEqual(purposesUI, researchFile.Projects));
            }
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDateLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDateInput).Text.Equals(TransformDateFormat(researchFile.RequestDate)));
            Assert.True(webDriver.FindElement(researchFileDetailsRequestSourceLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequestSourceInput).Text.Equals(researchFile.RequestSource));
            Assert.True(webDriver.FindElement(researchFileDetailsRquesterLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsRequesterInput).Text.Equals(researchFile.Requester));
            Assert.True(webDriver.FindElement(researchFileDetailsRequestDescripLabel).Displayed);

            //Result
            Assert.True(webDriver.FindElement(researchFileDetailsResultSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsResultCompleteLabel).Displayed);
            if (researchFile.ResearchCompletedDate != "")
            {
                Assert.True(webDriver.FindElement(researchFileDetailsResultCompleteInput).Text.Equals(TransformDateFormat(researchFile.ResearchCompletedDate)));
            }
            else
            {
                Assert.True(webDriver.FindElement(researchFileDetailsResultCompleteInput).Text.Equals("not complete"));
            }
            Assert.True(webDriver.FindElement(researchFileDetailsResultRequestLabel).Displayed);

            //Expropriation
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationSubtitle).Displayed);
            Assert.True(webDriver.FindElement(researchFileDetailsExpropriationLabel).Displayed);
            if (researchFile.Expropriation)
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
        public void VerifyPropResearchTabFormView(PropertyResearch propertyResearch)
        {
            Wait(2000);

            Assert.True(webDriver.FindElement(researchPropertyInterestLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyNameLabel).Displayed);
            if (propertyResearch.DescriptiveName != "")
            {
                Assert.True(webDriver.FindElement(researchPropertyNameViewInput).Text.Equals(propertyResearch.DescriptiveName));
            }  
            Assert.True(webDriver.FindElement(researchPropertyPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyPurposeViewInput).Text.Equals(propertyResearch.Purpose));
            Assert.True(webDriver.FindElement(researchPropertyLegalReqLabel).Displayed);
            if (propertyResearch.LegalOpinionRequest == "Unknown")
            {
                Assert.True(webDriver.FindElement(researchPropertyLegalReqViewInput).Text.Equals(""));
            }
            else
            {
                Assert.True(webDriver.FindElement(researchPropertyLegalReqViewInput).Text.Equals(propertyResearch.LegalOpinionRequest));
            }
            Assert.True(webDriver.FindElement(researchPropertyLegalObtLabel).Displayed);
            if (propertyResearch.LegalOpinionObtained == "Unknown")
            {
                Assert.True(webDriver.FindElement(researchPropertyLegalObtViewInput).Text.Equals(""));
            }
            else
            {
                Assert.True(webDriver.FindElement(researchPropertyLegalObtViewInput).Text.Equals(propertyResearch.LegalOpinionObtained));
            }
            Assert.True(webDriver.FindElement(researchPropertyDocRefLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyDocRefInput).Text.Equals(propertyResearch.DocumentReference));
            Assert.True(webDriver.FindElement(researchPropertySummaryLabel).Displayed);
            Assert.True(webDriver.FindElement(researchPropertyNotesLabel).Displayed);
        }

        private void VerifyResearchFileHeader(ResearchFile researchFile, string user)
        {
            WaitUntilVisible(researchFileHeaderNbrContent);
            
            Assert.True(webDriver.FindElement(researchFileViewTitle).Displayed);

            Assert.True(webDriver.FindElement(researchFileHeaderNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileHeaderNbrContent).Displayed);
            Assert.True(webDriver.FindElement(researchFileHeaderNameLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileHeaderNameContent).Text.Equals(researchFile.ResearchFileName));

            Assert.True(webDriver.FindElement(researchFileHeaderMOTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileHeaderDistrictLabel).Displayed);

            Assert.True(webDriver.FindElement(researchFileHeaderCreatedLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileHeaderCreatedDateContent).Text.Equals(GetTodayFormattedDate()));

            Assert.True(webDriver.FindElement(researchFileHeaderCreatedByContent).Text.Equals(user));

            Assert.True(webDriver.FindElement(researchFileHeaderLastUpdatedLabel).Displayed);
            Assert.True(webDriver.FindElement(researchFileHeaderLastUpdatedDateContent).Text.Equals(GetTodayFormattedDate()));
            Assert.True(webDriver.FindElement(researchFileHeaderLastUpdatedByContent).Text.Equals(user));

            Assert.True(webDriver.FindElement(researchFileHeaderStatusLabel).Displayed);
        }

    }
}
