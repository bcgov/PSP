using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionTakes : PageObjectBase
    {
        //Tab and Edit elements
        private By takesTabLink = By.CssSelector("a[data-rb-event-key='takes']");
        private By takesEditButton = By.CssSelector("button[title='Edit takes']");

        //Initial View Elements
        private By takesTitle = By.XPath("//h2[contains(text(),'Takes')]");
        private By takesForThisPropertyCurrentFileLabel = By.XPath("//label[contains(text(),'Takes for this property in the current file')]");
        private By takesForThisPropertyCurrentFileContent = By.XPath("//label[contains(text(),'Takes for this property in the current file')]/parent::div/following-sibling::div");
        private By takesForThisPropertyOtherFileLabel = By.XPath("//label[contains(text(),'Takes for this property in other files')]");
        private By takesForThisPropertyOtherFileContent = By.XPath("//label[contains(text(),'Takes for this property in other files')]/parent::div/following-sibling::div");

        //Initial View Create Form
        private By takePropertyTitle = By.XPath("//div[contains(text(),'Takes for')]");
        private By takeNewSubtitle = By.XPath("//h2/div/div[contains(text(),'New Take')]");
        private By takeTypeLabel = By.XPath("//label[contains(text(),'Take type')]");
        private By take1TypeSelect = By.Id("input-takes.0.takeTypeCode");
        private By take1DeleteButton = By.CssSelector("button[title='delete take']");
        private By takeStatusLabel = By.XPath("//label[contains(text(),'Take status')]");
        private By take1StatusSelect = By.Id("input-takes.0.takeStatusTypeCode");
        private By takeSiteContaminationLabel = By.XPath("//label[contains(text(),'Site contamination')]");
        private By take1SiteContaminationSelect = By.Id("input-takes.0.takeSiteContamTypeCode");
        private By takeDescriptionLabel = By.XPath("//label[contains(text(),'Description of this Take')]");
        private By take1DescriptionInput = By.Id("input-takes.0.description");
        private By takeAreaSubtitle = By.XPath("//h2/div/div[contains(text(),'Area')]");
        private By takeRightOfWayLabel = By.XPath("//label[contains(text(),'Is there a new highway dedication?')]");
        private By takeRightOfWayRadioBttnGroup = By.CssSelector("input[name='takes.0.isNewHighwayDedication']");
        private By takeMOTIInventoryLabel = By.XPath("//label[contains(text(),'Is this being acquired for MoTI inventory? *')]");
        private By takeMOTIInventoryBttnGroup = By.CssSelector("input[name='takes.0.isAcquiredForInventory']");
        private By takeSRWLabel = By.XPath("//label[contains(text(),'Is there a new registered interest in land (SRW, Easement or Covenant)?')]");
        private By takeSRWRadioBttnGroup = By.CssSelector("input[name='takes.0.isNewInterestInSrw']");
        private By takeLandActLabel = By.XPath("//label[contains(text(),'Is a there a new Land Act tenure?')]");
        private By takeLandActRadioBttnGroup = By.CssSelector("input[name='takes.0.isNewLandAct']");
        private By takeLicenseConstructLabel = By.XPath("//label[contains(text(),'Is there a new License for Construction Access (TLCA/LTC)?')]");
        private By takeLicenseConstructRadioBttnGroup = By.CssSelector("input[name='takes.0.isNewLicenseToConstruct']");
        private By takeSurplusSubtitle = By.XPath("//h2/div/div[contains(text(),'Surplus')]");
        private By takeSurplusLabel = By.XPath("//label[contains(text(),'Is there a Surplus?')]");
        private By takeSurplusRadioBttnGroup = By.CssSelector("input[name='takes.0.isThereSurplus']");

        private By createTakeBttn = By.XPath("//div/div/div[2]/div/div/div[3]/div/div/div/div/div/button");

        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public AcquisitionTakes(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateTakesTab()
        {
            WaitUntilClickable(takesTabLink);
            webDriver.FindElement(takesTabLink).Click();
        }

        public void ClickEditTakesButton()
        {
            WaitUntilSpinnerDisappear();
            WaitUntilClickable(takesEditButton);
            webDriver.FindElement(takesEditButton).Click();
        }

        public void ClickCreateNewTakeBttn()
        {
            Wait(2000);
            FocusAndClick(createTakeBttn);
        }

        public void SaveTake()
        {
            ButtonElement("Save");
            Wait(3000);
        }

        public void CancelTake()
        {
            ButtonElement("Cancel");
            sharedModals.CancelActionModal();
        }

        public void InsertTake(Take take)
        {
            Wait(2000);

            var index = take.TakeCounter;

            //Takes Details
            if (take.TakeType != "")
                ChooseSpecificSelectOption(By.Id("input-takes."+ index +".takeTypeCode"), take.TakeType);

            if (take.TakeStatus != "")
                ChooseSpecificSelectOption(By.Id("input-takes."+ index +".takeStatusTypeCode"), take.TakeStatus);

            if (take.SiteContamination != "")
                ChooseSpecificSelectOption(By.Id("input-takes."+ index +".takeSiteContamTypeCode"), take.SiteContamination);

            if (take.TakeDescription != "")
            {
                ClearInput(By.Id("input-takes."+ index +".description"));
                webDriver.FindElement(By.Id("input-takes."+ index +".description")).SendKeys(take.TakeDescription);
            }

            //Areas
            //New Highway Dedication
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isNewHighwayDedication"), take.IsNewHighwayDedication);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsNewHighwayDedication.Equals("true") && take.IsNewHighwayDedicationArea != "")
            {
                ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsNewHighwayDedicationArea);

                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsNewHighwayDedicationArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsNewHighwayDedicationArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsNewHighwayDedicationArea));
            }

            //MOTI Inventory
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isAcquiredForInventory"), take.IsMotiInventory);

            //New Registered Interest in Land
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isNewInterestInSrw"), take.IsNewInterestLand);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsNewInterestLand.Equals("true") && take.IsNewInterestLandArea != "")
            {
                ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsNewInterestLandArea);

                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsNewInterestLandArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsNewInterestLandArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsNewInterestLandArea));

                ClearInput(By.Id("datepicker-takes."+ index +".srwEndDt"));
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".srwEndDt")).SendKeys(take.IsNewInterestLandDate);
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".srwEndDt")).SendKeys(Keys.Enter);

            }

            //Land Act Tenure
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isNewLandAct"), take.IsLandActTenure);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsLandActTenure.Equals("true"))
            {
                ChooseSpecificSelectOption(By.Id("input-takes."+ index +".landActTypeCode"), take.IsLandActTenureDetail);

                if (take.IsLandActTenureArea != "")
                {
                    ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                    webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsLandActTenureArea);

                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsLandActTenureArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsLandActTenureArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsLandActTenureArea));
                }

                ClearInput(By.Id("datepicker-takes."+ index +".landActEndDt"));
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".landActEndDt")).SendKeys(take.IsLandActTenureDate);
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".landActEndDt")).SendKeys(Keys.Enter);
            }

            //License to Construct
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isNewLicenseToConstruct"), take.IsLicenseConstruct);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsLicenseConstruct.Equals("true"))
            {
                if (take.IsLicenseConstructArea != "")
                {
                    ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                    webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsLicenseConstructArea);

                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsLicenseConstructArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsLicenseConstructArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsLicenseConstructArea));
                }

                ClearInput(By.Id("datepicker-takes."+ index +".ltcEndDt"));
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".ltcEndDt")).SendKeys(take.IsLicenseConstructDate);
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".ltcEndDt")).SendKeys(Keys.Enter);
            }

            //Surplus
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isThereSurplus"), take.IsSurplus);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsSurplus.Equals("true") && take.IsSurplusArea != "")
            {
                ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsSurplusArea);

                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsSurplusArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsSurplusArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsSurplusArea));
            }
        }

        public void DeleteTake(int index)
        {
            Wait();

            WaitUntilClickable(By.XPath("//select[@id='input-takes."+ index +".takeTypeCode']/parent::div/parent::div/parent::div/preceding-sibling::button"));
            webDriver.FindElement(By.XPath("//select[@id='input-takes."+ index +".takeTypeCode']/parent::div/parent::div/parent::div/preceding-sibling::button")).Click();

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm Delete", sharedModals.ModalHeader());
                Assert.Contains("Are you sure you want to delete this item?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }
        }

        public void VerifyInitTakesView()
        {
            Wait();

            AssertTrueIsDisplayed(takesTitle);
            AssertTrueIsDisplayed(takesForThisPropertyCurrentFileLabel);
            AssertTrueIsDisplayed(takesForThisPropertyCurrentFileContent);
            AssertTrueIsDisplayed(takesForThisPropertyOtherFileLabel);
            AssertTrueIsDisplayed(takesForThisPropertyOtherFileContent);
        }

        public void VerifyInitCreateForm()
        {
            Wait();

            AssertTrueIsDisplayed(takePropertyTitle);
            AssertTrueIsDisplayed(takeNewSubtitle);
            AssertTrueIsDisplayed(takeTypeLabel);
            AssertTrueIsDisplayed(take1TypeSelect);
            AssertTrueIsDisplayed(take1DeleteButton);
            AssertTrueIsDisplayed(takeStatusLabel);
            AssertTrueIsDisplayed(take1StatusSelect);
            AssertTrueIsDisplayed(takeSiteContaminationLabel);
            AssertTrueIsDisplayed(take1SiteContaminationSelect);
            AssertTrueIsDisplayed(takeDescriptionLabel);
            AssertTrueIsDisplayed(take1DescriptionInput);
            AssertTrueIsDisplayed(takeAreaSubtitle);
            AssertTrueIsDisplayed(takeRightOfWayLabel);
            AssertTrueIsDisplayed(takeRightOfWayRadioBttnGroup);
            AssertTrueIsDisplayed(takeMOTIInventoryLabel);
            AssertTrueIsDisplayed(takeMOTIInventoryBttnGroup);
            AssertTrueIsDisplayed(takeSRWLabel);
            AssertTrueIsDisplayed(takeSRWRadioBttnGroup);
            AssertTrueIsDisplayed(takeLandActLabel);
            AssertTrueIsDisplayed(takeLandActRadioBttnGroup);
            AssertTrueIsDisplayed(takeLicenseConstructLabel);
            AssertTrueIsDisplayed(takeLicenseConstructRadioBttnGroup);

            AssertTrueIsDisplayed(takeSurplusSubtitle);
            AssertTrueIsDisplayed(takeSurplusLabel);
            AssertTrueIsDisplayed(takeSurplusRadioBttnGroup);
        }

        public void VerifyCreatedTakeViewForm(Take take)
        {
            var index = 0;

            //Take Details
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Take added on')]"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Take type')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Take type')]/parent::div/following-sibling::div"), take.TakeType);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Take status')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Take status')]/parent::div/following-sibling::div"), take.TakeStatus);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Site contamination')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Site contamination')]/parent::div/following-sibling::div"), take.SiteContamination);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Description')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div"), take.TakeDescription);

            //Take Area
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/h2/div/div[contains(text(),'Area')]"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new highway dedication?')]"));
            Assert.Equal(2, webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-newRightOfWayToggle']")).Count);
            if (take.IsNewHighwayDedication.Equals("True") && take.IsNewHighwayDedicationArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new highway dedication?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsNewHighwayDedicationArea);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is this being acquired for MoTI inventory?')]"));
            Assert.Equal(2, webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-addPropertyToggle']")).Count);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new registered interest in land (SRW, Easement or Covenant)')]"));
            Assert.Equal(2, webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-newInterestInSrwToggle']")).Count);
            if (take.IsNewInterestLand.Equals("True") && take.IsNewInterestLandArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is a there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsNewInterestLandArea);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is a there a new Land Act tenure?')]"));
            Assert.Equal(2, webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-landActToggle']")).Count);
            if (take.IsLandActTenure.Equals("True"))
            {
                AssertTrueContentEquals(By.XPath("(//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is a there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'Land Act')]/parent::div/following-sibling::div)[2]"),take.IsLandActTenureDetail);

                if(take.IsLandActTenureArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is a there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsNewInterestLandArea);

                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is a there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'End date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLandActTenureDate));
            }

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new License for Construction Access (TLCA/LTC)')]"));
            Assert.Equal(2, webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-licenseToConstructToggle']")).Count);
            if (take.IsLicenseConstruct.Equals("True"))
            {
                if (take.IsLandActTenureArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new License for Construction Access (TLCA/LTC)?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsLicenseConstructArea);

                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new License for Construction Access (TLCA/LTC)?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'LTC end date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLicenseConstructDate));
            }

            //Surplus
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/h2/div/div[contains(text(),'Surplus')]"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Surplus?')]"));
            Assert.Equal(2, webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-surplusToggle']")).Count);
            if (take.IsNewHighwayDedication.Equals("True") && take.IsSurplusArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Surplus?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsSurplusArea);
        }

        private double TransformSqMtToSqFt(string sqmt)
        {
            double sqmtNbr = double.Parse(sqmt) * 10.76391041671;
            double sqftRounded = Math.Round(sqmtNbr, 2, MidpointRounding.ToEven);

            if (sqftRounded.Equals(0.00))
                return 0;
            else
                return sqftRounded;
        }

        private double TransformSqMtToHectares(string sqmt)
        {
            double sqmtNbr = double.Parse(sqmt) * 0.0001;
            double hectaresRounded = Math.Round(sqmtNbr, 2, MidpointRounding.ToEven);

            if (hectaresRounded.Equals(0.00))
                return 0;
            else
                return hectaresRounded;
        }

        private double TransformSqMtToAcres(string sqmt)
        {
            double sqmtNbr = double.Parse(sqmt) * 0.000247105;
            double acresRounded = Math.Round(sqmtNbr, 2, MidpointRounding.ToEven);

            if (acresRounded.Equals(0.00))
                return 0;
            else
                return acresRounded;
        }
    }
}
