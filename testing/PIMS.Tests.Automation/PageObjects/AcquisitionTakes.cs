using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionTakes : PageObjectBase
    {
        //Tab and Edit elements
        private readonly By takesTabLink = By.CssSelector("a[data-rb-event-key='takes']");
        private readonly By takesAddButton = By.CssSelector("h2 button");

        //Initial View Elements
        private readonly By takesTitle = By.XPath("//h2[contains(text(),'Takes')]");
        private readonly By takesForThisPropertyCurrentFileLabel = By.XPath("//label[contains(text(),'Takes for this property in the current file')]");
        private readonly By takesForThisPropertyCurrentFileContent = By.XPath("//label[contains(text(),'Takes for this property in the current file')]/parent::div/following-sibling::div");
        private readonly By takesForThisPropertyOtherFileLabel = By.XPath("//label[contains(text(),'Takes for this property in other files')]");
        private readonly By takesForThisPropertyOtherFileContent = By.XPath("//label[contains(text(),'Takes for this property in other files')]/parent::div/following-sibling::div");

        //Initial View Create Form
        private readonly By takeNewSubtitle = By.XPath("//h2/div/div[contains(text(),'New Take')]");
        private readonly By takeTypeLabel = By.XPath("//label[contains(text(),'Take type')]");
        private readonly By takeTypeSelect = By.Id("input-takeTypeCode");
        private readonly By takeStatusLabel = By.XPath("//label[contains(text(),'Take status')]");
        private readonly By takeStatusSelect = By.Id("input-takeStatusTypeCode");
        private readonly By takeCompletionDateLabel = By.XPath("//label[contains(text(),'Completion date')]");
        private readonly By takeCompletionDateInput = By.Id("datepicker-completionDt");
        private readonly By takeSiteContaminationLabel = By.XPath("//label[contains(text(),'Site contamination')]");
        private readonly By takeSiteContaminationSelect = By.Id("input-takeSiteContamTypeCode");
        private readonly By takeDescriptionLabel = By.XPath("//label[contains(text(),'Description of this Take')]");
        private readonly By takeDescriptionInput = By.Id("input-description");

        private readonly By takeAreaSubtitle = By.XPath("//h2/div/div[contains(text(),'Area')]");

        private readonly By takeRightOfWayLabel = By.XPath("//label[contains(text(),'Is there a new highway dedication?')]");
        private readonly By takeRightOfWayRadioBttnGroup = By.CssSelector("input[name='isNewHighwayDedication']");
        private readonly By takeRightOfWaySqMetresInput = By.XPath("//input[@data-testid='radio-isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']");
        private readonly By takeRightOfWaySqFeetInput = By.XPath("//input[@data-testid='radio-isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']");
        private readonly By takeRightOfWayHectaresInput = By.XPath("//input[@data-testid='radio-isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']");
        private readonly By takeRightOfWayAcresInput = By.XPath("//input[@data-testid='radio-isnewhighwaydedication-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']");

        private readonly By takeMOTIInventoryLabel = By.XPath("//label[contains(text(),'Is this being acquired for MOTT inventory? *')]");
        private readonly By takeMOTIInventoryBttnGroup = By.CssSelector("input[name='isAcquiredForInventory']");

        private readonly By takeSRWLabel = By.XPath("//label[contains(text(),'Is there a new registered interest in land (SRW, Easement or Covenant)?')]");
        private readonly By takeSRWRadioBttnGroup = By.CssSelector("input[name='isNewInterestInSrw']");
        private readonly By takeSRWSqMetresInput = By.XPath("//input[@data-testid='radio-isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']");
        private readonly By takeSRWSqFeetInput = By.XPath("//input[@data-testid='radio-isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']");
        private readonly By takeSRWSqHectaresInput = By.XPath("//input[@data-testid='radio-isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']");
        private readonly By takeSRWSqAcresInput = By.XPath("//input[@data-testid='radio-isnewinterestinsrw-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']");
        private readonly By takeSRWEndDateInput = By.Id("datepicker-srwEndDt");

        private readonly By takeLandActLabel = By.XPath("//label[contains(text(),'Is there a new Land Act tenure?')]");
        private readonly By takeLandActRadioBttnGroup = By.CssSelector("input[name='isNewLandAct']");
        private readonly By takeLandActTypeSelect = By.Id("input-landActTypeCode");
        private readonly By takeLandActSqMetresInput = By.XPath("//input[@data-testid='radio-isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']");
        private readonly By takeLandActSqFeetInput = By.XPath("//input[@data-testid='radio-isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']");
        private readonly By takeLandActHectaresInput = By.XPath("//input[@data-testid='radio-isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']");
        private readonly By takeLandActAcresInput = By.XPath("//input[@data-testid='radio-isnewlandact-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']");
        private readonly By takeLandActEndDateInput = By.Id("datepicker-landActEndDt");

        private readonly By takeLicenseConstructLabel = By.XPath("//label[contains(text(),'Is there a new Licence for Construction Access (TLCA/LTC)?')]");
        private readonly By takeLicenseConstructRadioBttnGroup = By.CssSelector("input[name='isNewLicenseToConstruct']");
        private readonly By takeLicenseConstructSqMetresInput = By.XPath("//input[@data-testid='radio-isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']");
        private readonly By takeLicenseConstructSqFeetInput = By.XPath("//input[@data-testid='radio-isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']");
        private readonly By takeLicenseConstructHectaresInput = By.XPath("//input[@data-testid='radio-isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']");
        private readonly By takeLicenseConstructAcresInput = By.XPath("//input[@data-testid='radio-isnewlicensetoconstruct-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']");
        private readonly By takeLicenseConstructEndDateInput = By.Id("datepicker-ltcEndDt");

        private readonly By takeLeaseLabel = By.XPath("//label[contains(text(),'Is there a Lease (Payable)?')]");
        private readonly By takeLeaseRadioBttnGroup = By.CssSelector("input[name='isLeasePayable']");
        private readonly By takeLeaseSqMetresInput = By.XPath("//input[@data-testid='radio-isleasepayable-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']");
        private readonly By takeLeaseSqFeetInput = By.XPath("//input[@data-testid='radio-isleasepayable-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']");
        private readonly By takeLeaseHectaresInput = By.XPath("//input[@data-testid='radio-isleasepayable-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']");
        private readonly By takeLeaseAcresInput = By.XPath("//input[@data-testid='radio-isleasepayable-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']");
        private readonly By takeLeaseEndDateInput = By.Id("datepicker-leasePayableEndDt");

        private readonly By takeSurplusSubtitle = By.XPath("//h2/div/div[contains(text(),'Surplus')]");
        private readonly By takeSurplusLabel = By.XPath("//label[contains(text(),'Is there a Surplus?')]");
        private readonly By takeSurplusRadioBttnGroup = By.CssSelector("input[name='isThereSurplus']");
        private readonly By takeSurplusSqMetresInput = By.XPath("//input[@data-testid='radio-istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-meters']");
        private readonly By takeSurplusSqFeetInput = By.XPath("//input[@data-testid='radio-istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-sq-feet']");
        private readonly By takeSurplusHectaresInput = By.XPath("//input[@data-testid='radio-istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-hectares']");
        private readonly By takeSurplusAcresInput = By.XPath("//input[@data-testid='radio-istheresurplus-yes']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/div/div/div/div/input[@name='area-acres']");

        //Acquisition File Confirmation Modal Elements
        private readonly By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public AcquisitionTakes(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateTakesTab()
        {
            Wait();
            webDriver.FindElement(takesTabLink).Click();
        }

        public void ClickAddTakesButton()
        {
            WaitUntilSpinnerDisappear();
            WaitUntilClickable(takesAddButton);
            webDriver.FindElement(takesAddButton).Click();
        }

        public void ClickEditTakesButton(int index)
        {
            By editButton = By.CssSelector("div[data-testid='take-" + index + "'] button[data-testid='edit-button']");

            WaitUntilSpinnerDisappear();
            WaitUntilClickable(editButton);
            webDriver.FindElement(editButton).Click();
        }

        public void SaveTake()
        {
            ButtonElement("Save");
            Wait();
        }

        public void CancelTake()
        {
            ButtonElement("Cancel");
            sharedModals.CancelActionModal();
        }

        public void InsertTake(Take take)
        {
            Wait();

            //Takes Details
            if (take.TakeType != "")
                ChooseSpecificSelectOption(takeTypeSelect, take.TakeType);

            if (take.TakeStatus != "")
                ChooseSpecificSelectOption(takeStatusSelect, take.TakeStatus);

            if (take.TakeCompleteDate != "")
            {
                webDriver.FindElement(takeCompletionDateInput).SendKeys(take.TakeCompleteDate);
                webDriver.FindElement(takeCompletionDateInput).SendKeys(Keys.Enter);
            }

            if (take.SiteContamination != "")
                ChooseSpecificSelectOption(takeSiteContaminationSelect, take.SiteContamination);

            if (take.TakeDescription != "")
            {
                ClearInput(takeDescriptionInput);
                webDriver.FindElement(takeDescriptionInput).SendKeys(take.TakeDescription);
            }

            //Areas
            //New Highway Dedication
            ChooseSpecificRadioButton(takeRightOfWayRadioBttnGroup, take.IsNewHighwayDedication);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsNewHighwayDedication.Equals("true") && take.IsNewHighwayDedicationArea != "")
            {
                ClearDigitsInput(takeRightOfWaySqMetresInput);
                webDriver.FindElement(takeRightOfWaySqMetresInput).SendKeys(take.IsNewHighwayDedicationArea);

                AssertTrueDoublesEquals(takeRightOfWayHectaresInput, TransformSqMtToHectares(take.IsNewHighwayDedicationArea));
                AssertTrueDoublesEquals(takeRightOfWaySqFeetInput, TransformSqMtToSqFt(take.IsNewHighwayDedicationArea));
                AssertTrueDoublesEquals(takeRightOfWayAcresInput, TransformSqMtToAcres(take.IsNewHighwayDedicationArea));
            }

            //MOTI Inventory
            ChooseSpecificRadioButton(takeMOTIInventoryBttnGroup, take.IsMotiInventory);

            //New Registered Interest in Land
            ChooseSpecificRadioButton(takeSRWRadioBttnGroup, take.IsNewInterestLand);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsNewInterestLand.Equals("true") && take.IsNewInterestLandArea != "")
            {
                ClearDigitsInput(takeSRWSqMetresInput);
                webDriver.FindElement(takeSRWSqMetresInput).SendKeys(take.IsNewInterestLandArea);

                AssertTrueDoublesEquals(takeSRWSqHectaresInput, TransformSqMtToHectares(take.IsNewInterestLandArea));
                AssertTrueDoublesEquals(takeSRWSqFeetInput, TransformSqMtToSqFt(take.IsNewInterestLandArea));
                AssertTrueDoublesEquals(takeSRWSqAcresInput, TransformSqMtToAcres(take.IsNewInterestLandArea));

                ClearInput(takeSRWEndDateInput);
                webDriver.FindElement(takeSRWEndDateInput).SendKeys(take.IsNewInterestLandDate);
                webDriver.FindElement(takeSRWEndDateInput).SendKeys(Keys.Enter);
            }

            //Land Act Tenure
            ChooseSpecificRadioButton(takeLandActRadioBttnGroup, take.IsLandActTenure);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsLandActTenure.Equals("true"))
            {
                ChooseSpecificSelectOption(takeLandActTypeSelect, take.IsLandActTenureDetail);

                if (take.IsLandActTenureArea != "")
                {
                    ClearDigitsInput(takeLandActSqMetresInput);
                    webDriver.FindElement(takeLandActSqMetresInput).SendKeys(take.IsLandActTenureArea);

                    AssertTrueDoublesEquals(takeLandActHectaresInput, TransformSqMtToHectares(take.IsLandActTenureArea));
                    AssertTrueDoublesEquals(takeLandActSqFeetInput, TransformSqMtToSqFt(take.IsLandActTenureArea));
                    AssertTrueDoublesEquals(takeLandActAcresInput, TransformSqMtToAcres(take.IsLandActTenureArea));
                }

                if (take.IsLandActTenureDate != "")
                {
                    ClearInput(takeLandActEndDateInput);
                    webDriver.FindElement(takeLandActEndDateInput).SendKeys(take.IsLandActTenureDate);
                    webDriver.FindElement(takeLandActEndDateInput).SendKeys(Keys.Enter);
                }
            }

            //License to Construct
            ChooseSpecificRadioButton(takeLicenseConstructRadioBttnGroup, take.IsLicenseConstruct);

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
                    ClearDigitsInput(takeLicenseConstructSqMetresInput);
                    webDriver.FindElement(takeLicenseConstructSqMetresInput).SendKeys(take.IsLicenseConstructArea);

                    AssertTrueDoublesEquals(takeLicenseConstructHectaresInput, TransformSqMtToHectares(take.IsLicenseConstructArea));
                    AssertTrueDoublesEquals(takeLicenseConstructSqFeetInput, TransformSqMtToSqFt(take.IsLicenseConstructArea));
                    AssertTrueDoublesEquals(takeLicenseConstructAcresInput, TransformSqMtToAcres(take.IsLicenseConstructArea));
                }

                ClearInput(takeLicenseConstructEndDateInput);
                webDriver.FindElement(takeLicenseConstructEndDateInput).SendKeys(take.IsLicenseConstructDate);
                webDriver.FindElement(takeLicenseConstructEndDateInput).SendKeys(Keys.Enter);
            }

            //Lease Payable
            ChooseSpecificRadioButton(takeLeaseRadioBttnGroup, take.IsLeasePayable);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                if (sharedModals.ModalContent().Contains("You have created a Lease (Payable) Take"))
                {
                    Assert.Contains("Acknowledgement", sharedModals.ModalHeader());
                    Assert.Contains("You have created a Lease (Payable) Take. You also need to create a Lease/Licence File.", sharedModals.ModalContent());
                }
                else
                {
                    Assert.Contains("Confirm change", sharedModals.ModalHeader());
                    Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());
                }


                sharedModals.ModalClickOKBttn();
            }

            Wait();
            if (take.IsLeasePayable.Equals("true"))
            {
                if (take.IsLeasePayableArea != "")
                {
                    ClearDigitsInput(takeLeaseSqMetresInput);
                    webDriver.FindElement(takeLeaseSqMetresInput).SendKeys(take.IsLeasePayableArea);

                    AssertTrueDoublesEquals(takeLeaseHectaresInput, TransformSqMtToHectares(take.IsLeasePayableArea));
                    AssertTrueDoublesEquals(takeLeaseSqFeetInput, TransformSqMtToSqFt(take.IsLeasePayableArea));
                    AssertTrueDoublesEquals(takeLeaseAcresInput, TransformSqMtToAcres(take.IsLeasePayableArea));
                }

                ClearInput(takeLeaseEndDateInput);
                webDriver.FindElement(takeLeaseEndDateInput).SendKeys(take.IsLeasePayableDate);
                webDriver.FindElement(takeLeaseEndDateInput).SendKeys(Keys.Enter);
            }

            //Surplus
            ChooseSpecificRadioButton(takeSurplusRadioBttnGroup, take.IsSurplus);

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Contains("Confirm change", sharedModals.ModalHeader());
                Assert.Contains("The area, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (take.IsSurplus.Equals("true") && take.IsSurplusArea != "")
            {
                ClearDigitsInput(takeSurplusSqMetresInput);
                webDriver.FindElement(takeSurplusSqMetresInput).SendKeys(take.IsSurplusArea);

                AssertTrueDoublesEquals(takeSurplusHectaresInput, TransformSqMtToHectares(take.IsSurplusArea));
                AssertTrueDoublesEquals(takeSurplusSqFeetInput, TransformSqMtToSqFt(take.IsSurplusArea));
                AssertTrueDoublesEquals(takeSurplusAcresInput, TransformSqMtToAcres(take.IsSurplusArea));
            }
        }

        public void DeleteTake(int index)
        {
            By deleteButton = By.CssSelector("div[data-testid='take-" + index + "'] button[title='Remove take']");

            WaitUntilSpinnerDisappear();
            WaitUntilClickable(deleteButton);
            webDriver.FindElement(deleteButton).Click();

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

            AssertTrueIsDisplayed(takeNewSubtitle);
            AssertTrueIsDisplayed(takeTypeLabel);
            AssertTrueIsDisplayed(takeTypeSelect);
            AssertTrueIsDisplayed(takeStatusLabel);
            AssertTrueIsDisplayed(takeStatusSelect);
            AssertTrueIsDisplayed(takeSiteContaminationLabel);
            AssertTrueIsDisplayed(takeSiteContaminationSelect);
            AssertTrueIsDisplayed(takeDescriptionLabel);
            AssertTrueIsDisplayed(takeDescriptionInput);

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
            AssertTrueIsDisplayed(takeLeaseLabel);
            AssertTrueIsDisplayed(takeLeaseRadioBttnGroup);
            AssertTrueIsDisplayed(takeLeaseLabel);
            AssertTrueIsDisplayed(takeLeaseRadioBttnGroup);

            AssertTrueIsDisplayed(takeSurplusSubtitle);
            AssertTrueIsDisplayed(takeSurplusLabel);
            AssertTrueIsDisplayed(takeSurplusRadioBttnGroup);
        }

        public void VerifyCreatedTakeViewForm(Take take)
        {
            Wait(4000);
            var index = 0;

            //Take Details
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Take added on')]"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Take type')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Take type')]/parent::div/following-sibling::div"), take.TakeType);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Take status')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Take status')]/parent::div/following-sibling::div"), take.TakeStatus);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Site contamination')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Site contamination')]/parent::div/following-sibling::div"), take.SiteContamination);

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Description')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div"), take.TakeDescription);

            //Take Area
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/h2/div/div[contains(text(),'Area')]"));

            //Highway Dedication
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new highway dedication?')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new highway dedication')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsNewHighwayDedication));

            if (take.IsNewHighwayDedication.Equals("true") && take.IsNewHighwayDedicationArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new highway dedication?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), TransformAreaNumberFormat(take.IsNewHighwayDedicationArea));

            //MoTI Inventory
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is this being acquired for MOTT inventory?')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is this being acquired for MOTT inventory')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsMotiInventory));

            //Interest in Land
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new registered interest in land (SRW, Easement or Covenant)')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new registered interest in land (SRW, Easement or Covenant)')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsNewInterestLand));

            if (take.IsNewInterestLand.Equals("true") && take.IsNewInterestLandArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new registered interest in land (SRW, Easement or Covenant)')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), TransformAreaNumberFormat(take.IsNewInterestLandArea));

            //Land Acture Tenure
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Land Act tenure?')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Land Act tenure?')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsLandActTenure));
            if (take.IsLandActTenure.Equals("true"))
            {
                AssertTrueContentEquals(By.XPath("(//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'Land Act')]/parent::div/following-sibling::div)[2]"), take.IsLandActTenureDetail);

                if (take.IsLandActTenureArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), TransformAreaNumberFormat(take.IsLandActTenureArea));

            }
            if (take.IsLandActTenureDate != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Land Act tenure?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'End date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLandActTenureDate));


            //License for Construction
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Licence for Construction Access (TLCA/LTC)?')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Licence for Construction Access (TLCA/LTC)?')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsLicenseConstruct));

            if (take.IsLicenseConstruct.Equals("true"))
            {
                if (take.IsLicenseConstructArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Licence for Construction Access (TLCA/LTC)?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), TransformAreaNumberFormat(take.IsLicenseConstructArea));

                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a new Licence for Construction Access (TLCA/LTC)?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'LTC end date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLicenseConstructDate));
            }

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Lease (Payable)?')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Lease (Payable)?')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsLeasePayable));

            if (take.IsLeasePayable.Equals("true"))
            {
                if (take.IsLeasePayableArea != "")
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Lease (Payable)?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), TransformAreaNumberFormat(take.IsLeasePayableArea));

                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Lease (Payable)?')]/parent::div/parent::div/parent::div/div/div/label[contains(text(),'End date')]/parent::div/following-sibling::div"), TransformDateFormat(take.IsLeasePayableDate));
            }

            //Surplus
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/h2/div/div[contains(text(),'Surplus')]"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Surplus?')]"));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='take-"+ index +"']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Surplus?')]/parent::div/following-sibling::div"), TransformBooleanFormat(take.IsSurplus));
            if (take.IsNewHighwayDedication.Equals("True") && take.IsSurplusArea != "")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='take-" + index + "']/div/div/div/div/div/div/div/label[contains(text(),'Is there a Surplus?')]/parent::div/parent::div/parent::div/div/div/div/div/div/div/div[contains(text(),'sq. metres')]/preceding-sibling::div"), TransformAreaNumberFormat(take.IsSurplusArea));
        }
    }
}
