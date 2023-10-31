using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V115.Network;
using PIMS.Tests.Automation.Classes;
using Sprache;

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
        private By take1Subtitle = By.XPath("//h2/div/div[contains(text(),'Take 1')]");
        private By takeTypeLabel = By.XPath("//label[contains(text(),'Take type')]");
        private By take1TypeSelect = By.Id("input-takes.0.takeTypeCode");
        private By take1DeleteButton = By.CssSelector("button[title='delete take']");
        private By takeStatusLabel = By.XPath("//label[contains(text(),'Take status')]");
        private By take1StatusSelect = By.Id("input-takes.0.takeStatusTypeCode");
        private By takeSiteContaminationLabel = By.XPath("//label[contains(text(),'Site Contamination')]");
        private By take1SiteContaminationSelect = By.Id("input-takes.0.takeSiteContamTypeCode");
        private By takeDescriptionLabel = By.XPath("//label[contains(text(),'Description of this Take')]");
        private By take1DescriptionInput = By.Id("input-takes.0.description");
        private By takeAreaSubtitle = By.XPath("//h2/div/div[contains(text(),'Area')]");
        private By takeRightOfWayLabel = By.XPath("//label[contains(text(),'Is there a new right of way?')]");
        private By takeRightOfWayRadioBttnGroup = By.CssSelector("input[name='takes.0.isNewRightOfWay']");
        private By takeSRWLabel = By.XPath("//label[contains(text(),'Is there a Statutory Right of Way: (SRW)?')]");
        private By takeSRWRadioBttnGroup = By.CssSelector("input[name='takes.0.isStatutoryRightOfWay']");
        private By takeLandActLabel = By.XPath("//label[contains(text(),'Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)?')]");
        private By takeLandActRadioBttnGroup = By.CssSelector("input[name='takes.0.isLandAct']");
        private By takeLicenseConstructLabel = By.XPath("//label[contains(text(),'Is there a License to Construct (LTC)?')]");
        private By takeLicenseConstructRadioBttnGroup = By.CssSelector("input[name='takes.0.isLicenseToConstruct']");
        private By takeSurplusSubtitle = By.XPath("//h2/div/div[contains(text(),'Surplus')]");
        private By takeSurplusLabel = By.XPath("//label[contains(text(),'Is there a Surplus?')]");
        private By takeSurplusRadioBttnGroup = By.CssSelector("input[name='takes.0.isSurplus']");

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
            //New Right of Way
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isNewRightOfWay"), take.IsNewRightWay);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsNewRightWay.Equals("true") && take.IsNewRightWayArea != "")
            {
                ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsNewRightWayArea);

                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsNewRightWayArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsNewRightWayArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isnewrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsNewRightWayArea));
            }

            //Statutory Right of Way
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isStatutoryRightOfWay"), take.IsStatutoryRightWay);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsStatutoryRightWay.Equals("true") && take.IsStatutoryRightWayArea != "")
            {
                ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".isstatutoryrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".isstatutoryrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsStatutoryRightWayArea);

                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isstatutoryrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsStatutoryRightWayArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isstatutoryrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsStatutoryRightWayArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".isstatutoryrightofway-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsStatutoryRightWayArea));
            }

            //Land Act-Reserve
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isLandAct"), take.IsLandNotation);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsLandNotation.Equals("true"))
            {
                ChooseSpecificSelectOption(By.Id("input-takes."+ index +".landActTypeCode"), take.IsLandNotationDetail);

                if (take.IsLandNotationArea != "")
                {
                    ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".islandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                    webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".islandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsLandNotationArea);

                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".islandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsLandNotationArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".islandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsLandNotationArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".islandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsLandNotationArea));
                }

                ClearInput(By.Id("datepicker-takes."+ index +".landActEndDt"));
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".landActEndDt")).SendKeys(take.IsLandNotationDate);
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".landActEndDt")).SendKeys(Keys.Enter);
            }

            //License to Construct
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isLicenseToConstruct"), take.IsLicenseConstruct);

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
                    ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".islicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                    webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".islicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsLicenseConstructArea);

                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".islicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsLicenseConstructArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".islicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsLicenseConstructArea));
                    AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".islicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsLicenseConstructArea));
                }

                ClearInput(By.Id("datepicker-takes."+ index +".ltcEndDt"));
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".ltcEndDt")).SendKeys(take.IsLicenseConstructDate);
                webDriver.FindElement(By.Id("datepicker-takes."+ index +".ltcEndDt")).SendKeys(Keys.Enter);
            }

            //Surplus
            ChooseSpecificRadioButton(By.Name("takes."+ index +".isSurplus"), take.IsSurplus);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsSurplus.Equals("true") && take.IsSurplusArea != "")
            {
                ClearDigitsInput(By.XPath("//input[@data-testid='radio-takes."+ index +".issurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']"));
                webDriver.FindElement(By.XPath("//input[@data-testid='radio-takes."+ index +".issurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']")).SendKeys(take.IsSurplusArea);

                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".issurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']"), TransformSqMtToHectares(take.IsSurplusArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".issurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']"), TransformSqMtToSqFt(take.IsSurplusArea));
                AssertTrueDoublesEquals(By.XPath("//input[@data-testid='radio-takes."+ index +".issurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']"), TransformSqMtToAcres(take.IsSurplusArea));
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
            AssertTrueIsDisplayed(take1Subtitle);
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
            var index = take.TakeCounter;

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

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new right of way?')]"));
            Assert.True(webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-newRightOfWayToggle']")).Count.Equals(2));
            if (take.IsNewRightWay.Equals("True") && take.IsNewRightWayArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new right of way?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsNewRightWayArea);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Statutory Right of Way: (SRW)?')]"));
            Assert.True(webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-statutoryRightOfWayToggle']")).Count.Equals(2));
            if (take.IsStatutoryRightWay.Equals("True") && take.IsStatutoryRightWayArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsStatutoryRightWayArea);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)?')]"));
            Assert.True(webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-statutoryRightOfWayToggle']")).Count.Equals(2));
            if (take.IsLandNotation.Equals("True"))
            {
                AssertTrueContentEquals(By.XPath("(//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'Land Act')]/parent::div/following-sibling::div)[2]"),take.IsLandNotationDetail);

                if(take.IsLandNotationArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsStatutoryRightWayArea);

                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'End date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLandNotationDate));
            }

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a License to Construct (LTC)?')]"));
            Assert.True(webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-licenseToConstructToggle']")).Count.Equals(2));
            if (take.IsLicenseConstruct.Equals("True"))
            {
                if (take.IsLandNotationArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a License to Construct (LTC)?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), take.IsLicenseConstructArea);

                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a License to Construct (LTC)?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'LTC end date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLicenseConstructDate));
            }

            //Surplus
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/h2/div/div[contains(text(),'Surplus')]"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Surplus?')]"));
            Assert.True(webDriver.FindElements(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/div/div/div/div/input[@id='input-surplusToggle']")).Count.Equals(2));
            if (take.IsNewRightWay.Equals("True") && take.IsSurplusArea != "")
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
