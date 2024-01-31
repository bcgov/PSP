using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ResearchFiles : PageObjectBase
    {
        //Research File Menu options
        private By menuResearchButton = By.CssSelector("div[data-testid='nav-tooltip-research'] a");
        private By createResearchFileButton = By.XPath("//a[contains(text(),'Create a Research File')]");

        //File Details Tab Element
        private By fileDetailsTab = By.CssSelector("a[data-rb-event-key='fileDetails']");

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

        private By researchFileHeaderCode = By.XPath("//label[contains(text(),'File #:')]/parent::div/following-sibling::div/strong");

        //Research File - Properties Elements
        private By researchProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");
        private By researchPropertyResearchEditBttn = By.XPath("(//button[@class='btn btn-link'])[2]");
        private By researchPropertyNameInput = By.Id("input-propertyName");
        private By researchPropertyPurposeSelect = By.Id("purpose-selector_input");
        private By researchPropertyPurposeInput = By.XPath("//input[@id='purpose-selector_input']/parent::div");
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
        private By researchPropertyPurposeDeleteLinks = By.CssSelector("div[id='purpose-selector'] div span i");
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
        private SharedFileProperties sharedSearchProperties;

        private int totalAssociatedProps = 0;

        public ResearchFiles(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
            sharedSearchProperties = new SharedFileProperties(webDriver);
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

        public void NavigateToFileDetailsTab()
        {
            WaitUntilClickable(fileDetailsTab);
            webDriver.FindElement(fileDetailsTab).Click();
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
        }

        public void CreateResearchFile(ResearchFile researchFile)
        {
            WaitUntilVisible(researchFileNameInput);
            webDriver.FindElement(researchFileNameInput).SendKeys(researchFile.ResearchFileName);
        }   

        public void AddAdditionalResearchFileInfo(ResearchFile researchFile)
        {
            Wait(2000);
            webDriver.FindElement(researchFileEditButton).Click();

            //Status
            if (researchFile.Status != "")
                ChooseSpecificSelectOption(researchFileStatusSelect, researchFile.Status);

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
                webDriver.FindElement(researchRoadNameInput).SendKeys(researchFile.RoadName);
          
            if (researchFile.RoadAlias != "")
                webDriver.FindElement(researchRoadAliasInput).SendKeys(researchFile.RoadAlias);

            //Result
            if (researchFile.ResearchCompletedDate != "")
            {
                webDriver.FindElement(researchCompleteDateInput).SendKeys(researchFile.ResearchCompletedDate);
                webDriver.FindElement(researchCompleteDateInput).SendKeys(Keys.Enter);
            }
            if (researchFile.RequestResult != "")
                webDriver.FindElement(researchResultTextarea).SendKeys(researchFile.RequestResult);
 
            //Expropiation
            if (researchFile.Expropriation)
                FocusAndClick(researchResultExpropiationYesRadioBttn);
             else
                FocusAndClick(researchResultExpropiationNoRadioBttn);
            
            if (researchFile.ExpropriationNotes != "")
                webDriver.FindElement(researchExpropiationNotes).SendKeys(researchFile.ExpropriationNotes);

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

            if (researchFile.RequestDescription != "")
            {
                WaitUntilVisible(researchDescriptionRequestTextarea);
                webDriver.FindElement(researchDescriptionRequestTextarea).SendKeys(researchFile.RequestDescription);
            }
            if (researchFile.Requester != "")
            {
                webDriver.FindElement(selectContactButton).Click();
                sharedSelectContact.SelectContact(researchFile.Requester, "");
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
            if (propertyResearch.PropertyResearchPurpose.Count > 0)
            {
                for (int i = 0; i < propertyResearch.PropertyResearchPurpose.Count; i++)
                {
                    webDriver.FindElement(researchPropertyPurposeSelect).Click();

                    WaitUntilVisible(researchPropertyPurposeOptions);
                    ChooseMultiSelectSpecificOption(researchPropertyPurposeOptions, propertyResearch.PropertyResearchPurpose[i]);
                }
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

            //Status
            if (researchFile.Status != "")
                ChooseSpecificSelectOption(researchFileStatusSelect, researchFile.Status);

            //File Name
            if (researchFile.ResearchFileName != "")
            {
                ClearInput(researchFileNameInput);
                webDriver.FindElement(researchFileNameInput).SendKeys(researchFile.ResearchFileName);
            }
                
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
            if (propertyResearch.PropertyResearchPurpose.First() != "")
            {
                //Delete Purposes previously selected if any
                if (webDriver.FindElements(researchPropertyPurposeDeleteLinks).Count > 0)
                {
                    FocusAndClick(researchPropertyPurposeDiv);
                    while (webDriver.FindElements(researchPropertyPurposeDeleteLinks).Count > 0)
                    {
                        webDriver.FindElements(researchPropertyPurposeDeleteLinks)[0].Click();
                    }
                }
                foreach (string purpose in propertyResearch.PropertyResearchPurpose)
                {
                    FocusAndClick(researchPropertyPurposeInput);

                    WaitUntilClickable(researchPropertyPurposeOptions);
                    ChooseMultiSelectSpecificOption(researchPropertyPurposeOptions, purpose);
                }
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

            Assert.Equal("Confirm changes", sharedModals.ModalHeader());
            Assert.Equal("You have made changes to the properties in this file.", sharedModals.ConfirmationModalText1());
            Assert.Equal("Do you want to save these changes?", sharedModals.ConfirmationModalText2());

            sharedModals.ModalClickOKBttn();

            Wait();
            if (webDriver.FindElements(researchFileConfirmationModal).Count() > 1)
            {
                Assert.Equal("User Override Required", sharedModals.SecondaryModalHeader());
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
                    Assert.Equal("Confirm changes", sharedModals.ModalHeader());
                    Assert.Equal("If you choose to cancel now, your changes will not be saved.", sharedModals.ConfirmationModalText1());
                    Assert.Equal("Do you want to proceed?", sharedModals.ConfirmationModalText2());

                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        //Get the research file number
        public string GetResearchFileCode()
        {
            Wait(2000);
            return webDriver.FindElement(researchFileHeaderCode).Text;
        }

        //Verify Create Research Init Form
        public void VerifyResearchFileCreateInitForm()
        {
            Wait(2000);

            //Title and Name
            AssertTrueIsDisplayed(researchFileCreateHeader);
            AssertTrueIsDisplayed(researchFileNameLabel);
            AssertTrueIsDisplayed(researchFileNameInput);
            AssertTrueIsDisplayed(researchFileHelpNameTooltip);

            //Project
            AssertTrueIsDisplayed(researchFileDetailsProjectSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsProjectAddBttn);

            //Properties to include
            sharedSearchProperties.VerifyLocateOnMapFeature();
        }

        //Verify Edit Research File Init Form
        public void VerifyResearchFileEditInitForm(ResearchFile researchFile, string user)
        {
            WaitUntilVisible(researchFileSummaryBttn);

            //Header
            VerifyResearchFileHeader(researchFile, user);
            AssertTrueContentEquals(researchFileHeaderStatusContent,"Active");

            //Left Bar Elements
            AssertTrueIsDisplayed(researchFileSummaryBttn);
            AssertTrueIsDisplayed(researchFilePropertiesLeftSection);
            AssertTrueIsDisplayed(researchEditPropertiesBttn);

            //Main section Elements
            AssertTrueIsDisplayed(researchFileDetailsTab);
            AssertTrueIsDisplayed(researchFileDocumentsTab);
            AssertTrueIsDisplayed(researchFileNotesTab);

            //File Details Elements
            //Project
            AssertTrueIsDisplayed(researchFileDetailsProjectSubtitle);
            AssertTrueIsDisplayed(reserachFileDetailsProjectLabel);

            //Roads
            AssertTrueIsDisplayed(researchFileDetailsRoadSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsRoadNameLabel);
            AssertTrueIsDisplayed(researchFileDetailsRoadAliasLabel);

            //Research Request
            AssertTrueIsDisplayed(researchFileDetailsResearchRequestSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsRequestPurposeLabel);
            AssertTrueIsDisplayed(researchFileDetailsRequestDateLabel);
            AssertTrueIsDisplayed(researchFileDetailsRequestSourceLabel);
            AssertTrueIsDisplayed(researchFileDetailsRquesterLabel);
            AssertTrueIsDisplayed(researchFileDetailsRequestDescripLabel);

            //Result
            AssertTrueIsDisplayed(researchFileDetailsResultSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsResultCompleteLabel);
            AssertTrueIsDisplayed(researchFileDetailsResultRequestLabel);

            //Expropriation
            AssertTrueIsDisplayed(researchFileDetailsExpropriationSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsExpropriationLabel);
            AssertTrueIsDisplayed(researchFileDetailsExpropriationNotesLabel);
        }

        //Verify UI/UX Elements - Research Main Form - View Form
        public void VerifyResearchFileMainFormView(ResearchFile researchFile, string user)
        {
            Wait(2000);

            //Header
            VerifyResearchFileHeader(researchFile, user);
            AssertTrueContentEquals(researchFileHeaderStatusContent, researchFile.Status);

            //Project
            AssertTrueIsDisplayed(researchFileDetailsProjectSubtitle);
            AssertTrueIsDisplayed(reserachFileDetailsProjectLabel);

            if (researchFile.Projects.First() != "")
            {
                var projectsUI = GetProjects(researchFileDetailsProjectsCount);
                Assert.True(Enumerable.SequenceEqual(projectsUI, researchFile.Projects));
            }

            //Roads
            AssertTrueIsDisplayed(researchFileDetailsRoadSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsRoadNameLabel);
            AssertTrueContentEquals(researchFileDetailsRoadNameInput,researchFile.RoadName);
            AssertTrueIsDisplayed(researchFileDetailsRoadAliasLabel);
            AssertTrueContentEquals(researchFileDetailsRoadAliasInput,researchFile.RoadAlias);

            //Research Request
            AssertTrueIsDisplayed(researchFileDetailsResearchRequestSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsRequestPurposeLabel);
            if (researchFile.ResearchPurpose.First() != "")
            {
                var purposesUI = GetProjects(researchFileDetailsProjectsCount);
                Assert.True(Enumerable.SequenceEqual(purposesUI, researchFile.Projects));
            }
            AssertTrueIsDisplayed(researchFileDetailsRequestDateLabel);
            AssertTrueContentEquals(researchFileDetailsRequestDateInput, TransformDateFormat(researchFile.RequestDate));
            AssertTrueIsDisplayed(researchFileDetailsRequestSourceLabel);
            AssertTrueContentEquals(researchFileDetailsRequestSourceInput, researchFile.RequestSource);
            AssertTrueIsDisplayed(researchFileDetailsRquesterLabel);
            AssertTrueContentEquals(researchFileDetailsRequesterInput, researchFile.Requester);
            AssertTrueIsDisplayed(researchFileDetailsRequestDescripLabel);

            //Result
            AssertTrueIsDisplayed(researchFileDetailsResultSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsResultCompleteLabel);
            if (researchFile.ResearchCompletedDate != "")
            {
                AssertTrueContentEquals(researchFileDetailsResultCompleteInput, TransformDateFormat(researchFile.ResearchCompletedDate));
            }
            else
            {
                AssertTrueContentEquals(researchFileDetailsResultCompleteInput,"not complete");
            }
            AssertTrueIsDisplayed(researchFileDetailsResultRequestLabel);

            //Expropriation
            AssertTrueIsDisplayed(researchFileDetailsExpropriationSubtitle);
            AssertTrueIsDisplayed(researchFileDetailsExpropriationLabel);

            if (researchFile.Expropriation)
            {
                AssertTrueContentEquals(researchFileDetailsExpropriationInput, "Yes");
            }
            else
            {
                AssertTrueContentEquals(researchFileDetailsExpropriationInput, "No");
            }
            AssertTrueIsDisplayed(researchFileDetailsExpropriationNotesLabel);
        }

        //Verify UI/UX Elements - Research Properties - Property Research - View Form
        public void VerifyPropResearchTabFormView(PropertyResearch propertyResearch)
        {
            Wait(2000);

            AssertTrueIsDisplayed(researchPropertyInterestLabel);
            AssertTrueIsDisplayed(researchPropertyNameLabel);

            if (propertyResearch.DescriptiveName != "")
                AssertTrueContentEquals(researchPropertyNameViewInput, propertyResearch.DescriptiveName);

            AssertTrueIsDisplayed(researchPropertyPurposeLabel);
            AssertTrueContentEquals(researchPropertyPurposeViewInput, TransformListToText(propertyResearch.PropertyResearchPurpose));
            AssertTrueIsDisplayed(researchPropertyLegalReqLabel);

            if (propertyResearch.LegalOpinionRequest == "Unknown")
                AssertTrueContentEquals(researchPropertyLegalReqViewInput,"");
           
            else
                AssertTrueContentEquals(researchPropertyLegalReqViewInput, propertyResearch.LegalOpinionRequest);
            
            AssertTrueIsDisplayed(researchPropertyLegalObtLabel);

            if (propertyResearch.LegalOpinionObtained == "Unknown")
                AssertTrueContentEquals(researchPropertyLegalObtViewInput, "");

            else
                AssertTrueContentEquals(researchPropertyLegalObtViewInput, propertyResearch.LegalOpinionObtained);

            AssertTrueIsDisplayed(researchPropertyDocRefLabel);
            AssertTrueContentEquals(researchPropertyDocRefInput, propertyResearch.DocumentReference);
            AssertTrueIsDisplayed(researchPropertySummaryLabel);
            AssertTrueIsDisplayed(researchPropertyNotesLabel);
        }

        private void VerifyResearchFileHeader(ResearchFile researchFile, string user)
        {
            WaitUntilVisible(researchFileHeaderNbrContent);

            AssertTrueIsDisplayed(researchFileViewTitle);

            AssertTrueIsDisplayed(researchFileHeaderNbrLabel);
            AssertTrueIsDisplayed(researchFileHeaderNbrContent);
            AssertTrueIsDisplayed(researchFileHeaderNameLabel);
            AssertTrueContentEquals(researchFileHeaderNameContent,researchFile.ResearchFileName);

            AssertTrueIsDisplayed(researchFileHeaderMOTIRegionLabel);
            AssertTrueIsDisplayed(researchFileHeaderDistrictLabel);

            AssertTrueIsDisplayed(researchFileHeaderCreatedLabel);
            AssertTrueContentEquals(researchFileHeaderCreatedDateContent, GetTodayFormattedDate());

            AssertTrueContentEquals(researchFileHeaderCreatedByContent,user);

            AssertTrueIsDisplayed(researchFileHeaderLastUpdatedLabel);
            AssertTrueContentEquals(researchFileHeaderLastUpdatedDateContent,GetTodayFormattedDate());
            AssertTrueContentEquals(researchFileHeaderLastUpdatedByContent, user);

            AssertTrueIsDisplayed(researchFileHeaderStatusLabel);
        }

    }
}
