using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedActivities : PageObjectBase
    {  
        //Create Activity Elements
        private readonly By managementActCloseTrayBttn = By.CssSelector("button[id='close-tray']");
        private readonly By managementActEditButton = By.CssSelector("button[title='Edit property activity']");

        //Select File Properties Elements
        private readonly By managementActFilePropertiesTitle = By.XPath("//div[contains(text(),'Select File Properties')]");
        private readonly By managementActFileSelectedPropsLabel = By.XPath("//div[contains(text(),'Select File Properties')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Selected Properties')]");
        private readonly By managementActFilePropertiesCount = By.CssSelector("div[data-testid='selectableFileProperties'] div[class='tr-wrapper']");

        //Activity Details Elements
        private readonly By managementActivityDetailsTitle = By.XPath("//div[contains(text(),'Activity Details')]");
        private readonly By managementActTypeLabel = By.XPath("//label[contains(text(),'Activity type')]");
        private readonly By managementActTypeInput = By.Id("input-activityTypeCode");
        private readonly By managementActTypeContent = By.XPath("//label[contains(text(),'Activity type')]/parent::div/following-sibling::div");
        private readonly By managementActSubTypeLabel = By.XPath("//label[contains(text(),'Sub-type')]");
        private readonly By managementActSubTypeSelect = By.Id("multiselect-activitySubtypeCodes");
        private readonly By managementActSubTypeSelectOptions = By.CssSelector("div[id='multiselect-activitySubtypeCodes'] div[class='optionListContainer displayNone']");
        private readonly By managementActSubTypeSelect1stOption = By.CssSelector("div[id='multiselect-activitySubtypeCodes'] div[class='optionListContainer displayNone'] ul li:first-child");
        private readonly By managementActSubTypeDeleteBttns = By.CssSelector("div[id='multiselect-activitySubtypeCodes'] i[class='custom-close']");
        private readonly By managementActSubTypeContents = By.CssSelector("div[id='multiselectContainerReact']");
        private readonly By managementActStatusLabel = By.XPath("//label[contains(text(),'Activity status')]");
        private readonly By managementActStatusInput = By.Id("input-activityStatusCode");
        private readonly By managementActStatusContent = By.XPath("//label[contains(text(),'Activity status')]/parent::div/following-sibling::div");
        private readonly By managementActRequestAddedDateLabel = By.XPath("//label[contains(text(),'Requested added date')]");
        private readonly By managementActRequestAddedCommenceDateInput = By.Id("datepicker-requestedDate");
        private readonly By managementActRequestAddedDateContent = By.XPath("//label[contains(text(),'Requested added date')]/parent::div/following-sibling::div");
        private readonly By managementActCommencementLabel = By.XPath("//label[contains(text(),'Commencement')]");
        private readonly By managementActCommencementContent = By.XPath("//label[contains(text(),'Commencement')]/parent::div/following-sibling::div");
        private readonly By managementActCompletionDateLabel = By.XPath("//label[contains(text(),'Completion')]");
        private readonly By managementActCompletionDateInput = By.Id("datepicker-completionDate");
        private readonly By managementActCompletionDateContent = By.XPath("//label[contains(text(),'Completion date')]/parent::div/following-sibling::div");
        private readonly By managementActDescriptionLabel = By.XPath("//div[contains(text(),'Activity Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private readonly By managementActDescriptionInput = By.Id("input-description");
        private readonly By managementActDescriptionContent = By.XPath("//div[contains(text(),'Activity Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div");
        private readonly By managementActMinistryContactLabel = By.XPath("//label[contains(text(),'Ministry contacts')]");
        private readonly By managementActMinistryContactInput = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/div/div/div/div[contains(text(),'Select from contacts')]");
        private readonly By managementActMinistryContactBttn = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/div/div/div/button");
        private readonly By managementActMinistryContactAddContactLink = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/following-sibling::button");
        private readonly By managementActMinistryContactDeleteBttns = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/button");
        private readonly By managementActMinistryContactContent = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/a/span");
        private readonly By managementActRequestorLabel = By.XPath("//label[contains(text(),'Requestor')]");
        private readonly By managementActRequestorTooltip = By.XPath("//label[contains(text(),'Requestor')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private readonly By managementActRequestorInput = By.Id("input-requestedSource");
        private readonly By managementActRequestorContent = By.XPath("//label[contains(text(),'Requestor')]/parent::div/following-sibling::div");
        private readonly By managementActContactManagerLabel = By.XPath("//label[contains(text(),'Contact manager')]");
        private readonly By managementActContactManagerContent = By.XPath("//label[contains(text(),'Contact manager')]/parent::div/following-sibling::div");
        private readonly By managementActContactManagerTooltip = By.XPath("//label[contains(text(),'Contact manager')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private readonly By managementActInvolvedPartiesLabel = By.XPath("//label[contains(text(),'Involved parties')]");
        private readonly By managementActInvolvedPartiesInput = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/div/div/div//div[contains(text(),'Select from contacts')]");
        private readonly By managementActInvolvedPartiesExtContactsBttn = By.XPath("//input[@id='input-involvedParties[0].id']/parent::div/parent::div/following-sibling::div/button");
        private readonly By managementActInvolvedPartiesExtContactsAddContactLink = By.XPath("//input[@id='input-involvedParties[0].id']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::button");
        private readonly By managementActInvolvedPartiesDeleteBttns = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/button");
        private readonly By managementActInvolvedPartiesContent = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/a/span");
        private readonly By managementActDetailsActivityExternalContactsLabel = By.XPath("//label[contains(text(),'External contacts')]");
        private readonly By managementActDetailsActivityExternalContactsInput = By.XPath("//label[contains(text(),'External contacts')]/parent::div/following-sibling::div/div/div/div/div/div//div[contains(text(),'Select from contacts')]");
        private readonly By managementActDetailsActivityExternalContactsAddBttn = By.XPath("//label[contains(text(),'External contacts')]/parent::div/following-sibling::div/div/div/div/div/div/button");
        private readonly By managementActDetailsActivityExternalContactsCount = By.XPath("//label[contains(text(),'External contacts')]/parent::div/following-sibling::div/a");
        private readonly By managementActDetailsActivityExternalContactCreateCount = By.XPath("//label[contains(text(),'External contacts')]/parent::div/following-sibling::div/div");
        private readonly By managementActDetailsActivityExternalContacts1stDeleteBttn = By.XPath("//label[contains(text(),'External contacts')]/parent::div/following-sibling::div/div[1]/div[2]/button");
        private readonly By managementActServiceProviderLabel = By.XPath("//label[contains(text(),'Service provider')]");
        private readonly By managementActServiceProviderInput = By.XPath("//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/div/div/div/div[contains(text(),'Select from contacts')]");
        private readonly By managementActServiceProviderBttn = By.XPath("//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/div/div/div/button");
        private readonly By managementActServiceProviderContent = By.XPath("//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/a/span");

        //Activity Invoices Elements
        private readonly By managementActInvoiceTotalsSubtitle = By.XPath("//div[contains(text(),'Invoices Total')]");
        private readonly By managementActInvoiceTotalPretaxLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total (before tax)')]");
        private readonly By managementActInvoiceTotalPretaxContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total (before tax)')]/parent::div/following-sibling::div");
        private readonly By managementActInvoiceTotalGSTLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]");
        private readonly By managementActInvoiceTotalGSTContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]/parent::div/following-sibling::div");
        private readonly By managementActInvoiceTotalPSTLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PST amount')]");
        private readonly By managementActInvoiceTotalPSTContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PST amount')]/parent::div/following-sibling::div");
        private readonly By managementActInvoiceGrandTotalLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]");
        private readonly By managementActInvoiceGrandTotalContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]/parent::div/following-sibling::div");

        //Invoices Elements
        private readonly By managementAddInvoiceBttn = By.XPath("//div[contains(text(),'Invoices Total')]/following-sibling::div/button");
        private readonly By managementInvoicesTotalSubtitle = By.XPath("//div[contains(text(),'Invoices Total')]");
        private readonly By managementActPretaxAmountLabel = By.XPath("//label[contains(text(),'Total (before tax)')]");
        private readonly By managementActPretaxAmountInput = By.Id("input-pretaxAmount");
        private readonly By managementActGSTAmountLabel = By.XPath("//label[contains(text(),'GST amount')]");
        private readonly By managementActGSTAmountInput = By.Id("input-gstAmount");
        private readonly By managementActPSTAmountLabel = By.XPath("//label[contains(text(),'PST amount')]");
        private readonly By managementActPSTAmountInput = By.Id("input-pstAmount");
        private readonly By managementActTotalAmountLabel = By.XPath("//label[contains(text(),'Total amount')]");
        private readonly By managementActTotalAmountInput = By.Id("input-totalAmount");

        private SharedModals sharedModals;
        private SharedSelectContact sharedSelectContact;

        public SharedActivities(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void UpdateSelectedActivityBttn()
        {
            WaitUntilClickable(managementActEditButton);
            webDriver.FindElement(managementActEditButton).Click();
        }

        public void SaveManagementActivity()
        {
            ButtonElement("Save");
            WaitUntilVisible(managementActEditButton);
        }

        public void InsertNewPropertyActivity(PropertyActivity activity)
        {
            Wait();

            //Choosing Activity type, Sub-type, status
            ChooseSpecificSelectOption(managementActTypeInput, activity.PropertyActivityType);

            Wait();
            if (webDriver.FindElements(managementActSubTypeDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(managementActSubTypeDeleteBttns).Count > 0)
                    webDriver.FindElements(managementActSubTypeDeleteBttns)[0].Click();
            }

            if (activity.PropertyActivitySubTypeList.First() != "")
            {
                foreach (string subtype in activity.PropertyActivitySubTypeList)
                {
                    webDriver.FindElement(managementActSubTypeSelect).Click();

                    WaitUntilVisible(managementActSubTypeSelectOptions);
                    ChooseMultiSelectSpecificOption(managementActSubTypeSelectOptions, subtype);
                }
            }

            ChooseSpecificSelectOption(managementActStatusInput, activity.PropertyActivityStatus);

            //Inserting Requested Added Date
            ClearInput(managementActRequestAddedCommenceDateInput);
            webDriver.FindElement(managementActRequestAddedCommenceDateInput).Click();
            webDriver.FindElement(managementActRequestAddedCommenceDateInput).SendKeys(activity.PropertyActivityRequestedCommenceDate);
            webDriver.FindElement(managementActRequestAddedCommenceDateInput).SendKeys(Keys.Enter);

            //Inserting Completion Date
            if (activity.PropertyActivityCompletionDate != "")
            {
                webDriver.FindElement(managementActCompletionDateInput).SendKeys(activity.PropertyActivityCompletionDate);
                webDriver.FindElement(managementActCompletionDateInput).SendKeys(Keys.Enter);
            }

            //Inserting Description
            ClearInput(managementActDescriptionInput);
            webDriver.FindElement(managementActDescriptionInput).SendKeys(activity.PropertyActivityDescription);

            //Deleting previous Ministry Contacts and adding new
            while (webDriver.FindElements(managementActMinistryContactDeleteBttns).Count > 0)
                webDriver.FindElements(managementActMinistryContactDeleteBttns)[0].Click();

            if (activity.PropertyActivityMinistryContactList.First() != "")
            {
                webDriver.FindElement(managementActMinistryContactBttn).Click();
                sharedSelectContact.SelectContact(activity.PropertyActivityMinistryContactList[0], "");

                if (activity.PropertyActivityMinistryContactList.Count > 1)
                {
                    for (int i = 1; i < activity.PropertyActivityMinistryContactList.Count; i++)
                    {
                        var elementNumber = i + 1;
                        webDriver.FindElement(managementActMinistryContactAddContactLink).Click();

                        WaitUntilVisible(By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div["+ elementNumber +"]/div/div/div/div/button"));
                        webDriver.FindElement(By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div["+ elementNumber +"]/div/div/div/div/button")).Click();
                        sharedSelectContact.SelectContact(activity.PropertyActivityMinistryContactList[i], "");
                    }
                }
            }

            //Inserting Requestor
            if (activity.PropertyActivityRequestorContactMngr != string.Empty)
            {
                ClearInput(managementActRequestorInput);
                webDriver.FindElement(managementActRequestorInput).Click();
                webDriver.FindElement(managementActRequestorInput).SendKeys(activity.PropertyActivityRequestorContactMngr);
            }

            //Deleting Involved parties and adding new
            while (webDriver.FindElements(managementActInvolvedPartiesDeleteBttns).Count > 0)
                webDriver.FindElements(managementActInvolvedPartiesDeleteBttns)[0].Click();

            if (activity.PropertyActivityInvolvedPartiesExtContactsList.First() != "")
            {
                webDriver.FindElement(managementActInvolvedPartiesExtContactsBttn).Click();
                sharedSelectContact.SelectContact(activity.PropertyActivityInvolvedPartiesExtContactsList[0], "");

                if (activity.PropertyActivityInvolvedPartiesExtContactsList.Count > 1)
                {
                    for (int idx = 1; idx < activity.PropertyActivityInvolvedPartiesExtContactsList.Count; idx++)
                    {
                        var elementNumber = idx + 1;
                        webDriver.FindElement(managementActInvolvedPartiesExtContactsAddContactLink).Click();

                        WaitUntilVisible(By.XPath("//input[@id='input-involvedParties["+ idx +"].id']/parent::div/parent::div/following-sibling::div/button"));
                        webDriver.FindElement(By.XPath("//input[@id='input-involvedParties["+ idx +"].id']/parent::div/parent::div/following-sibling::div/button")).Click();
                        sharedSelectContact.SelectContact(activity.PropertyActivityInvolvedPartiesExtContactsList[idx], "");
                    }
                }
            }

            if (activity.PropertyActivityServiceProvider != "")
            {
                webDriver.FindElement(managementActServiceProviderBttn).Click();
                sharedSelectContact.SelectContact(activity.PropertyActivityServiceProvider, "");
            }

            if (activity.ManagementPropertyActivityInvoices.Count > 0)
                for (int i = 0; i < activity.ManagementPropertyActivityInvoices.Count; i++)
                    AddInvoice(activity.ManagementPropertyActivityInvoices[i], i);
        }

        public void CancelPropertyManagement()
        {
            ButtonElement("Cancel");

            Wait();
            Assert.Equal("Confirm Changes", sharedModals.ModalHeader());
            Assert.Contains("If you choose to cancel now, your changes will not be saved.", sharedModals.ModalContent());
            Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());

            sharedModals.ModalClickOKBttn();
        }

        public void CloseActivityTray()
        {
            WaitUntilClickable(managementActCloseTrayBttn);
            webDriver.FindElement(managementActCloseTrayBttn).Click();
        }

        public void VerifyInsertedActivity(PropertyActivity activity, string activityType)
        {
            Wait(2000);

            //Activity Details section
            AssertTrueIsDisplayed(managementActivityDetailsTitle);

            AssertTrueIsDisplayed(managementActTypeLabel);
            AssertTrueContentEquals(managementActTypeContent, activity.PropertyActivityType);

            AssertTrueIsDisplayed(managementActSubTypeLabel);
            if (activity.PropertyActivitySubTypeList.First() != "")
            {
                var subTypesUI = GetViewFieldListContent(managementActSubTypeContents);
                Assert.True(Enumerable.SequenceEqual(subTypesUI, activity.PropertyActivitySubTypeList));
            }

            AssertTrueIsDisplayed(managementActStatusLabel);
            AssertTrueContentEquals(managementActStatusContent, activity.PropertyActivityStatus);

            if (activityType == "Management File")
            {
                AssertTrueIsDisplayed(managementActCommencementLabel);
                AssertTrueContentEquals(managementActCommencementContent, TransformDateFormat(activity.PropertyActivityRequestedCommenceDate));
            }
            else
            {
                AssertTrueIsDisplayed(managementActRequestAddedDateLabel);
                AssertTrueContentEquals(managementActRequestAddedDateContent, TransformDateFormat(activity.PropertyActivityRequestedCommenceDate));
            }

            if (activity.PropertyActivityCompletionDate != "")
            {
                AssertTrueIsDisplayed(managementActCompletionDateLabel);
                AssertTrueContentEquals(managementActCompletionDateContent, TransformDateFormat(activity.PropertyActivityCompletionDate));
            }

            AssertTrueIsDisplayed(managementActDescriptionLabel);
            AssertTrueContentEquals(managementActDescriptionContent, activity.PropertyActivityDescription);

            AssertTrueIsDisplayed(managementActMinistryContactLabel);
            if (activity.PropertyActivityMinistryContactList.First() != "")
                for (int i = 0; i < activity.PropertyActivityMinistryContactList.Count; i++)
                    Assert.Equal(webDriver.FindElements(managementActMinistryContactContent)[i].Text, activity.PropertyActivityMinistryContactList[i]);

            if (activityType == "Management File")
            {
                AssertTrueIsDisplayed(managementActContactManagerLabel);
                AssertTrueIsDisplayed(managementActContactManagerTooltip);
                if (activity.PropertyActivityRequestorContactMngr != "")
                    AssertTrueContentEquals(managementActContactManagerContent, activity.PropertyActivityRequestorContactMngr);
            }
            else
            {
                AssertTrueIsDisplayed(managementActRequestorLabel);
                AssertTrueIsDisplayed(managementActRequestorTooltip);
                if (activity.PropertyActivityRequestorContactMngr != "")
                    AssertTrueContentEquals(managementActRequestorContent, activity.PropertyActivityRequestorContactMngr);
            }

            if (activityType == "Management File")
            {
                AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsLabel);
                if (activity.PropertyActivityInvolvedPartiesExtContactsList.First() != "")
                    for (int i = 0; i < activity.PropertyActivityInvolvedPartiesExtContactsList.Count; i++)
                        Assert.Equal(webDriver.FindElements(managementActDetailsActivityExternalContactsCount)[i].Text, activity.PropertyActivityInvolvedPartiesExtContactsList[i]);
            }
            else
            {
                AssertTrueIsDisplayed(managementActInvolvedPartiesLabel);
                if (activity.PropertyActivityInvolvedPartiesExtContactsList.First() != "")
                    for (int i = 0; i < activity.PropertyActivityInvolvedPartiesExtContactsList.Count; i++)
                        Assert.Equal(webDriver.FindElements(managementActInvolvedPartiesContent)[i].Text, activity.PropertyActivityInvolvedPartiesExtContactsList[i]);
            }


            AssertTrueIsDisplayed(managementActServiceProviderLabel);
            if (activity.PropertyActivityServiceProvider != "")
                AssertTrueContentEquals(managementActServiceProviderContent, activity.PropertyActivityServiceProvider);

            //Invoices section
            AssertTrueIsDisplayed(managementActInvoiceTotalsSubtitle);

            AssertTrueIsDisplayed(managementActInvoiceTotalPretaxLabel);
            AssertTrueContentEquals(managementActInvoiceTotalPretaxContent, TransformCurrencyFormat(activity.ManagementPropertyActivityTotalPreTax));

            AssertTrueIsDisplayed(managementActInvoiceTotalGSTLabel);
            AssertTrueContentEquals(managementActInvoiceTotalGSTContent, TransformCurrencyFormat(activity.ManagementPropertyActivityTotalGST));

            AssertTrueIsDisplayed(managementActInvoiceTotalPSTLabel);
            AssertTrueContentEquals(managementActInvoiceTotalPSTContent, TransformCurrencyFormat(activity.ManagementPropertyActivityTotalPST));

            AssertTrueIsDisplayed(managementActInvoiceGrandTotalLabel);
            AssertTrueContentEquals(managementActInvoiceGrandTotalContent, TransformCurrencyFormat(activity.ManagementPropertyActivityGrandTotal));
        }

        public void VerifyCreateActivityInitForm(string activityType = "", int propsCount = 0)
        {
            //Selected Properties
            if (activityType == "Management File")
            {
                AssertTrueIsDisplayed(managementActFilePropertiesTitle);
                AssertTrueIsDisplayed(managementActFileSelectedPropsLabel);
                Assert.Equal(webDriver.FindElements(managementActFilePropertiesCount).Count,propsCount);
            }
            //Activity Details
            AssertTrueIsDisplayed(managementActivityDetailsTitle);
            AssertTrueIsDisplayed(managementActTypeLabel);
            AssertTrueIsDisplayed(managementActTypeInput);
            AssertTrueIsDisplayed(managementActSubTypeLabel);
            AssertTrueIsDisplayed(managementActSubTypeSelect);
            AssertTrueIsDisplayed(managementActStatusLabel);
            AssertTrueIsDisplayed(managementActStatusInput);

            if (activityType == "Management File")
                AssertTrueIsDisplayed(managementActCommencementLabel);
            else
                AssertTrueIsDisplayed(managementActRequestAddedDateLabel);
            AssertTrueIsDisplayed(managementActRequestAddedCommenceDateInput);

            AssertTrueIsDisplayed(managementActCompletionDateLabel);
            AssertTrueIsDisplayed(managementActCompletionDateInput);
            AssertTrueIsDisplayed(managementActDescriptionLabel);
            AssertTrueIsDisplayed(managementActDescriptionInput);

            AssertTrueIsDisplayed(managementActMinistryContactLabel);
            AssertTrueIsDisplayed(managementActMinistryContactInput);
            AssertTrueIsDisplayed(managementActMinistryContactBttn);
            AssertTrueIsDisplayed(managementActMinistryContactAddContactLink);

            if (activityType == "Management File")
            {
                AssertTrueIsDisplayed(managementActContactManagerLabel);
                AssertTrueIsDisplayed(managementActContactManagerTooltip);
                AssertTrueIsDisplayed(managementActContactManagerContent);
            }
            else
            {
                AssertTrueIsDisplayed(managementActRequestorLabel);
                AssertTrueIsDisplayed(managementActRequestorTooltip);
                AssertTrueIsDisplayed(managementActRequestorInput);
            }

            if (activityType == "Management File")
            {
                AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsLabel);
                AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsInput);
                AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsAddBttn); 
            }
            else
            {
                AssertTrueIsDisplayed(managementActInvolvedPartiesLabel);
                AssertTrueIsDisplayed(managementActInvolvedPartiesInput);
                AssertTrueIsDisplayed(managementActInvolvedPartiesExtContactsBttn);
                AssertTrueIsDisplayed(managementActInvolvedPartiesExtContactsAddContactLink);
            }

            AssertTrueIsDisplayed(managementActServiceProviderLabel);
            AssertTrueIsDisplayed(managementActServiceProviderInput);
            AssertTrueIsDisplayed(managementActServiceProviderBttn);

            //Invoice
            AssertTrueIsDisplayed(managementAddInvoiceBttn);
            AssertTrueIsDisplayed(managementInvoicesTotalSubtitle);
            AssertTrueIsDisplayed(managementActPretaxAmountLabel);
            AssertTrueIsDisplayed(managementActPretaxAmountInput);
            AssertTrueIsDisplayed(managementActGSTAmountLabel);
            AssertTrueIsDisplayed(managementActGSTAmountInput);
            AssertTrueIsDisplayed(managementActPSTAmountLabel);
            AssertTrueIsDisplayed(managementActPSTAmountInput);
            AssertTrueIsDisplayed(managementActTotalAmountLabel);
            AssertTrueIsDisplayed(managementActTotalAmountInput);
        }

        private void AddInvoice(ManagementPropertyActivityInvoice invoice, int index)
        {
            Wait();
            webDriver.FindElement(managementAddInvoiceBttn).Click();

            Wait();
            webDriver.FindElement(By.Id("input-invoices."+ index +".invoiceNum")).SendKeys(invoice.PropertyActivityInvoiceNumber);

            webDriver.FindElement(By.Id("datepicker-invoices."+ index +".invoiceDateTime")).SendKeys(invoice.PropertyActivityInvoiceDate);
            webDriver.FindElement(By.Id("datepicker-invoices."+ index +".invoiceDateTime")).SendKeys(Keys.Enter);

            webDriver.FindElement(By.Id("input-invoices."+ index +".description")).SendKeys(invoice.PropertyActivityInvoiceDescription);

            CleanUpCurrencyInput(By.Id("input-invoices."+ index +".pretaxAmount"));
            SendKeysToCurrencyInput(By.Id("input-invoices."+ index +".pretaxAmount"), invoice.PropertyActivityInvoicePretaxAmount);

            ChooseSpecificSelectOption(By.Id("input-invoices."+ index +".isPstRequired"), invoice.PropertyActivityInvoicePSTApplicable);

            AssertTrueElementValueEquals(By.Id("input-invoices."+ index +".gstAmount"), TransformCurrencyFormat(invoice.PropertyActivityInvoiceGSTAmount));

            if (invoice.PropertyActivityInvoicePSTAmount != "0.00")
                AssertTrueElementValueEquals(By.Id("input-invoices."+ index +".pstAmount"), TransformCurrencyFormat(invoice.PropertyActivityInvoicePSTAmount));
        }
    }
}
