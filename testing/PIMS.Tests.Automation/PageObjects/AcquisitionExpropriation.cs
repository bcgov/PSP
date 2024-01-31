using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionExpropriation : PageObjectBase
    {
        //Acquisition Files Tab Element
        private By expropriationTab = By.CssSelector("a[data-rb-event-key='expropriation']");

        //Form 1 Elements
        private By form1Title = By.XPath("//div[@data-testid='form-1-section']/h2/div/div[contains(text(),'Form 1 - Notice of Expropriation')]");
        private By form1ExpandBttn = By.XPath("//div[@data-testid='form-1-section']/h2/div/div[2]");
        private By form1ExpAuthorityLabel = By.XPath("//div[@data-testid='form-1-section']/div/div/div/label[contains(text(),'Expropriation authority')]");
        private By form1ExpAuthorityInput = By.XPath("//div[@data-testid='form-1-section']/div/div/div/div/div/div/div/input[@id='input-expropriationAuthority.contact.id']/parent::div/parent::div");
        private By form1SelectContactBttn = By.XPath("//div[@data-testid='form-1-section']/div/div/div/div/div/div/button[@title='Select Contact']");
        private By form1ImpactPropertiesLabel = By.XPath("//div[@data-testid='form-1-section']/div/div/div/label[contains(text(),'Impacted properties')]");
        private By form1ImpactPropertiesTable = By.XPath("//div[@data-testid='form-1-section']/div/div/div/div/div[@data-testid='selectableFileProperties']");
        private By form1NatureInterestLabel = By.XPath("//div[@data-testid='form-1-section']/div/div/div/label[contains(text(),'Nature of interest')]");
        private By form1NatureInterestInput = By.Id("input-landInterest");
        private By form1PurposeExpropLabel = By.XPath("//div[@data-testid='form-1-section']/div/div/div/label[contains(text(),'Purpose of expropriation')]");
        private By form1PurposeExpropInput = By.Id("input-purpose");
        private By form1CancelBttn = By.XPath("//div[@data-testid='form-1-section']/div/div/div/button/div[contains(text(),'Cancel')]");
        private By form1GenerateBttn = By.XPath("//div[@data-testid='form-1-section']/div/div/div/button/div[contains(text(),'Generate')]");

        //Form 5 Elements
        private By form5Title = By.XPath("//div[@data-testid='form-5-section']/h2/div/div[contains(text(),'Form 5 - Certificate of Approval of Expropriation')]");
        private By form5ExpandBttn = By.XPath("//div[@data-testid='form-5-section']/h2/div/div[2]/*");
        private By form5ExpAuthorityLabel = By.XPath("//div[@data-testid='form-5-section']/div/div/div/label[contains(text(),'Expropriation authority')]");
        private By form5ExpAuthorityInput = By.XPath("//div[@data-testid='form-5-section']/div/div/div/div/div/div/div/input[@id='input-expropriationAuthority.contact.id']/parent::div/parent::div");
        private By form5SelectContactBttn = By.XPath("//div[@data-testid='form-5-section']/div/div/div/div/div/div/button[@title='Select Contact']");
        private By form5ImpactPropertiesLabel = By.XPath("//div[@data-testid='form-5-section']/div/div/div/label[contains(text(),'Impacted properties')]");
        private By form5ImpactPropertiesTable = By.XPath("//div[@data-testid='form-5-section']/div/div/div/div/div[@data-testid='selectableFileProperties']");
        private By form5CancelBttn = By.XPath("//div[@data-testid='form-5-section']/div/div/div/button/div[contains(text(),'Cancel')]");
        private By form5GenerateBttn = By.XPath("//div[@data-testid='form-5-section']/div/div/div/button/div[contains(text(),'Generate')]");

        //Form 8 Elements
        private By form8Title = By.XPath("//div[@data-testid='form-8-section']/h2/div/div/div/div[contains(text(),'Form 8 - Notice of Advance Payment')]");
        private By form8AddBttn = By.XPath("//div[@data-testid='form-8-section']/h2/div/div/div/div/button");

        private By form8CreateTitle = By.XPath("//div[contains(text(),'Form 8 Notice of Advance Payment')]");
        private By form8PayeeLabel = By.XPath("//label[contains(text(),'Payee')]");
        private By form8PayeeSelect = By.Id("input-payeeKey");
        private By form8ExpAuthorityLabel = By.XPath("//label[contains(text(),'Expropriation authority')]");
        private By form8ExpAuthorityInput = By.XPath("//input[@id='input-expropriationAuthority.contact.id']/parent::div/parent::div");
        private By form8ExpAuthorityContactBttn = By.CssSelector("button[title='Select Contact']");
        private By form8DescriptionLabel = By.XPath("//label[contains(text(),'Description')]");
        private By form8DescriptionTextarea = By.Id("input-description");

        private By form8PaymentDetailsTitle = By.XPath("//div[contains(text(),'Payment details')]");
        private By form8PaymentAddBttn = By.CssSelector("button[data-testid='add-payment-item']");
        private By form8DeletePaymentsTotal = By.CssSelector("button[title='Delete Payment Item']");

        private By form8TotalCount = By.XPath("//div[contains(text(),'Form 8 - Notice of Advance Payment')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div");
        private By form8PaymentsTotal = By.CssSelector("div[data-testid='paymentItems'] div[class='tbody'] div[class='tr-wrapper']");

        //Form 9 Elements
        private By form9Title = By.XPath("//div[@data-testid='form-9-section']/h2/div/div[contains(text(),'Form 9 - Vesting Notice')]");
        private By form9ExpandBttn = By.XPath("//div[@data-testid='form-9-section']/h2/div/div[2]/*");
        private By form9ExpAuthorityLabel = By.XPath("//div[@data-testid='form-9-section']/div/div/div/label[contains(text(),'Expropriation authority')]");
        private By form9ExpAuthorityInput = By.XPath("//div[@data-testid='form-9-section']/div/div/div/div/div/div/div/input[@id='input-expropriationAuthority.contact.id']/parent::div/parent::div");
        private By form9SelectContactBttn = By.XPath("//div[@data-testid='form-9-section']/div/div/div/div/div/div/button[@title='Select Contact']");
        private By form9ImpactPropertiesLabel = By.XPath("//div[@data-testid='form-9-section']/div/div/div/label[contains(text(),'Impacted properties')]");
        private By form9ImpactPropertiesTable = By.XPath("//div[@data-testid='form-9-section']/div/div/div/div/div[@data-testid='selectableFileProperties']");
        private By form9ShownPlanLabel = By.XPath("//div[@data-testid='form-9-section']/div/div/div/label[contains(text(),'Shown on plan(s)')]");
        private By form9ShownPlanInput = By.Id("input-registeredPlanNumbers");
        private By form9CancelBttn = By.XPath("//div[@data-testid='form-9-section']/div/div/div/button/div[contains(text(),'Cancel')]");
        private By form9GenerateBttn = By.XPath("//div[@data-testid='form-9-section']/div/div/div/button/div[contains(text(),'Generate')]");


        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSelectContact sharedSelectContact;
        private SharedModals sharedModals;

        public AcquisitionExpropriation(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToExpropriationTab()
        {
            Wait();
            webDriver.FindElement(expropriationTab).Click();
        }

        public void AddForm8Button()
        {
            WaitUntilClickable(form8AddBttn);
            webDriver.FindElement(form8AddBttn).Click();

            WaitUntilSpinnerDisappear();
        }

        public void EditNthForm8Button(int index)
        {
            WaitUntilSpinnerDisappear();

            WaitUntilClickable(By.CssSelector("button[data-testid='form8["+ index +"].edit-form8']"));
            webDriver.FindElement(By.CssSelector("button[data-testid='form8["+ index +"].edit-form8']")).Click();
        }

        public void DeleteNthForm8(int index)
        {
            WaitUntilSpinnerDisappear();

            WaitUntilClickable(By.CssSelector("button[data-testid='form8["+ index +"].delete-form8']"));
            webDriver.FindElement(By.CssSelector("button[data-testid='form8["+ index +"].delete-form8']")).Click();

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Remove Form 8", sharedModals.ModalHeader());
                Assert.Equal("Do you wish to remove this Form 8?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }
        }

        public void DeleteFirstPayment()
        {
            WaitUntilSpinnerDisappear();

            webDriver.FindElement(By.CssSelector("button[data-testid='paymentItems[0].delete-button']")).Click();

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Remove Payment Item", sharedModals.ModalHeader());
                Assert.Equal("Do you wish to remove this payment item?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }
        }

        public void SaveExpropriation()
        {
            Wait();
            ButtonElement("Save");
        }

        public void CancelExpropriation()
        {
            Wait();
            ButtonElement("Cancel");

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                Assert.Equal("You have made changes on this form. Do you wish to leave without saving?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }

        }

        public void CreateForm8(AcquisitionExpropriationForm8 expropriation)
        {
            Wait();

            ChooseSpecificSelectOption(form8PayeeSelect, expropriation.Form8Payee);

            webDriver.FindElement(form8ExpAuthorityContactBttn).Click();
            sharedSelectContact.SelectContact(expropriation.Form8ExpropriationAuthority, "");

            if (expropriation.Form8Description != "")
            {
                webDriver.FindElement(form8DescriptionTextarea).Click();
                webDriver.FindElement(form8DescriptionTextarea).SendKeys(expropriation.Form8Description);
                webDriver.FindElement(form8DescriptionLabel).Click();
            }    

            if (expropriation.ExpropriationPayments.Count > 0)
            {
                for (int i = 0; i < expropriation.ExpropriationPayments.Count; i++)
                    AddPayments(expropriation.ExpropriationPayments[i], i);
            }
        }

        public void UpdateForm8(AcquisitionExpropriationForm8 expropriation)
        {
            WaitUntilSpinnerDisappear();

            ChooseSpecificSelectOption(form8PayeeSelect, expropriation.Form8Payee);

            webDriver.FindElement(form8ExpAuthorityContactBttn).Click();
            sharedSelectContact.SelectContact(expropriation.Form8ExpropriationAuthority, null);

            if (expropriation.Form8Description != "")
            {
                ClearInput(form8DescriptionTextarea);
                webDriver.FindElement(form8DescriptionTextarea).SendKeys(expropriation.Form8Description);
                webDriver.FindElement(form8DescriptionLabel).Click();
            }

            if (expropriation.ExpropriationPayments.Count > 0)
            {
                while (webDriver.FindElements(form8DeletePaymentsTotal).Count > 0)
                    DeleteFirstPayment();

                for (int i = 0; i < expropriation.ExpropriationPayments.Count; i++)
                    AddPayments(expropriation.ExpropriationPayments[i], i);
            }   
        }

        public int TotalPaymentsCount()
        {
            WaitUntilSpinnerDisappear();
            return webDriver.FindElements(form8PaymentsTotal).Count();
        }

        public int TotalExpropriationCount()
        {
            Wait();
            return webDriver.FindElements(form8TotalCount).Count();
        }

        public void VerifySection3InitExpropriationTab()
        {
            Wait();

            //Form 8
            AssertTrueIsDisplayed(form8Title);
            AssertTrueIsDisplayed(form8AddBttn);    
        }

        public void VerifySection6InitExpropriationTab()
        {
            Wait();

            //Form 1
            AssertTrueIsDisplayed(form1Title);
            AssertTrueIsDisplayed(form1ExpandBttn);
            AssertTrueIsDisplayed(form1ExpAuthorityLabel);
            AssertTrueIsDisplayed(form1ExpAuthorityInput);
            AssertTrueIsDisplayed(form1SelectContactBttn);
            AssertTrueIsDisplayed(form1ImpactPropertiesLabel);
            AssertTrueIsDisplayed(form1ImpactPropertiesTable);
            AssertTrueIsDisplayed(form1NatureInterestLabel);
            AssertTrueIsDisplayed(form1NatureInterestInput);
            AssertTrueIsDisplayed(form1PurposeExpropLabel);
            AssertTrueIsDisplayed(form1PurposeExpropInput);
            AssertTrueIsDisplayed(form1CancelBttn);
            AssertTrueIsDisplayed(form1GenerateBttn);

            //Form 5
            webDriver.FindElement(form5ExpandBttn).Click();

            AssertTrueIsDisplayed(form5Title);
            AssertTrueIsDisplayed(form5ExpandBttn);
            AssertTrueIsDisplayed(form5ExpAuthorityLabel);
            AssertTrueIsDisplayed(form5ExpAuthorityInput);
            AssertTrueIsDisplayed(form5SelectContactBttn);
            AssertTrueIsDisplayed(form5ImpactPropertiesLabel);
            AssertTrueIsDisplayed(form5ImpactPropertiesTable);
            AssertTrueIsDisplayed(form5CancelBttn);
            AssertTrueIsDisplayed(form5GenerateBttn);

            //Form 8
            AssertTrueIsDisplayed(form8Title);
            AssertTrueIsDisplayed(form8AddBttn);

            //Form 9
            webDriver.FindElement(form9ExpandBttn).Click();

            AssertTrueIsDisplayed(form9Title);
            AssertTrueIsDisplayed(form9ExpandBttn);
            AssertTrueIsDisplayed(form9ExpAuthorityLabel);
            AssertTrueIsDisplayed(form9ExpAuthorityInput);
            AssertTrueIsDisplayed(form9SelectContactBttn);
            AssertTrueIsDisplayed(form9ImpactPropertiesLabel);
            AssertTrueIsDisplayed(form9ImpactPropertiesTable);
            AssertTrueIsDisplayed(form9ShownPlanLabel);
            AssertTrueIsDisplayed(form9ShownPlanInput);
            AssertTrueIsDisplayed(form9CancelBttn);
            AssertTrueIsDisplayed(form9GenerateBttn);
        }

        public void VerifyInitCreateForm8()
        {
            Wait();

            AssertTrueIsDisplayed(form8CreateTitle);
            AssertTrueIsDisplayed(form8PayeeLabel);
            AssertTrueIsDisplayed(form8PayeeSelect);
            AssertTrueIsDisplayed(form8ExpAuthorityLabel);
            AssertTrueIsDisplayed(form8ExpAuthorityInput);
            AssertTrueIsDisplayed(form8ExpAuthorityContactBttn);
            AssertTrueIsDisplayed(form8DescriptionLabel);
            AssertTrueIsDisplayed(form8DescriptionTextarea);

            AssertTrueIsDisplayed(form8PaymentDetailsTitle);
            AssertTrueIsDisplayed(form8PaymentAddBttn);
        }

        public void VerifyCreatedForm8View(AcquisitionExpropriationForm8 expropriation, int index)
        {
            WaitUntilSpinnerDisappear();

            AssertTrueIsDisplayed(By.CssSelector("button[data-testid='form8["+ index +"].generate-form8']"));
            AssertTrueIsDisplayed(By.CssSelector("button[data-testid='form8["+ index +"].edit-form8']"));
            AssertTrueIsDisplayed(By.CssSelector("button[data-testid='form8["+ index +"].delete-form8']"));

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div/div/label[contains(text(),'Payee')]"));
            if(webDriver.FindElements(By.CssSelector("div[data-testid='form8["+ index +"].payee-name'] div a span")).Count > 0)
                AssertTrueContentEquals(By.CssSelector("div[data-testid='form8["+ index +"].payee-name'] div a span"), expropriation.Form8PayeeDisplay);
            else
                AssertTrueContentEquals(By.CssSelector("div[data-testid='form8["+ index +"].payee-name'] div label"), expropriation.Form8PayeeDisplay);

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div/div/label[contains(text(),'Expropriation Authority')]"));
            AssertTrueContentEquals(By.CssSelector("div[data-testid='form8["+ index +"].exp-authority'] a span"), expropriation.Form8ExpropriationAuthority);

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div/div/label[contains(text(),'Description')]"));
            AssertTrueContentEquals(By.CssSelector("div[data-testid='form8["+ index +"].description']"), expropriation.Form8Description);

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::h3[contains(text(), 'Payment Details')]"));
            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div[@data-testid='paymentItems']"));

            if (expropriation.ExpropriationPayments.Count > 0)
            {
                for (int i = 0; i < expropriation.ExpropriationPayments.Count; i++)
                {
                    var elementIndex = i + 1;

                    AssertTrueContentEquals(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div[@data-testid='paymentItems']/div[@class='tbody']/div[@class='tr-wrapper']["+ elementIndex +"]/div/div[1]"), expropriation.ExpropriationPayments[i].ExpPaymentItem);
                    AssertTrueContentEquals(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div[@data-testid='paymentItems']/div[@class='tbody']/div[@class='tr-wrapper']["+ elementIndex +"]/div/div[2]"), TransformCurrencyFormat(expropriation.ExpropriationPayments[i].ExpPaymentAmount));
                    AssertTrueContentEquals(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div[@data-testid='paymentItems']/div[@class='tbody']/div[@class='tr-wrapper']["+ elementIndex +"]/div/div[3]"), TransformCurrencyFormat(expropriation.ExpropriationPayments[i].ExpPaymentGSTAmount));
                    AssertTrueContentEquals(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div[@data-testid='paymentItems']/div[@class='tbody']/div[@class='tr-wrapper']["+ elementIndex +"]/div/div[4]"), TransformCurrencyFormat(expropriation.ExpropriationPayments[i].ExpPaymentTotalAmount));

                }
            }
            else
                AssertTrueIsDisplayed(By.XPath("//button[@data-testid='form8["+ index +"].generate-form8']/parent::div/following-sibling::div[@data-testid='paymentItems']/div[@class='no-rows-message']")); 
        }

        private void AddPayments(ExpropriationPayment payment, int index)
        {
            WaitUntilClickable(form8PaymentAddBttn);
            webDriver.FindElement(form8PaymentAddBttn).Click();

            WaitUntilVisible(By.Id("input-paymentItems["+ index +"].paymentItemTypeCode"));
            ChooseSpecificSelectOption(By.Id("input-paymentItems["+ index +"].paymentItemTypeCode"), payment.ExpPaymentItem);

            ClearInput(By.Id("input-paymentItems["+ index +"].pretaxAmount"));
            webDriver.FindElement(By.Id("input-paymentItems["+ index +"].pretaxAmount")).SendKeys(payment.ExpPaymentAmount);

            ChooseSpecificSelectOption(By.Id("input-paymentItems["+ index +"].isGstRequired"), payment.ExpPaymentGSTApplicable);

            if(webDriver.FindElements(By.Id("input-paymentItems["+ index +"].taxAmount")).Count > 0)
                AssertTrueElementValueEquals(By.Id("input-paymentItems["+ index +"].taxAmount"), TransformCurrencyFormat(payment.ExpPaymentGSTAmount));

            AssertTrueElementValueEquals(By.Id("input-paymentItems["+ index +"].totalAmount"),TransformCurrencyFormat(payment.ExpPaymentTotalAmount));
        }
    }
}
