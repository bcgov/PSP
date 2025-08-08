const { SharedSelectContact } = require("./SharedSelectContacts");

class SharedActivities {
  constructor(page) {
    this.page = page;
    this.sharedSelectContacts = new SharedSelectContact(page);
  }

  async updateSelectedActivityBttn() {
    await this.page.locator("button[title='Edit property activity']").click();
  }

  async insertNewPropertyActivity(activity) {
    //Choosing Activity type, Sub-type, status
    await this.page
      .locator("#input-activityTypeCode")
      .selectOption({ label: activity.PropertyActivityType });

    if (
      (await this.page
        .locator(
          "div[id='multiselect-activitySubtypeCodes'] i[class='custom-close']"
        )
        .count()) > 0
    ) {
      while (
        (await this.page
          .locator(
            "div[id='multiselect-activitySubtypeCodes'] i[class='custom-close']"
          )
          .count()) > 0
      ) {
        await this.page
          .locator(
            "div[id='multiselect-activitySubtypeCodes'] i[class='custom-close']"
          )
          .first()
          .click();
      }
    }

    if (activity.PropertyActivitySubTypeList.count() > 0) {
      activity.PropertyActivitySubTypeList.foreach(async (subType) => {
        await this.page.locator("#multiselect-activitySubtypeCodes").click();
        await this.page
          .locator("div[id='multiselect-activitySubtypeCodes'] ul li")
          .filter({ hasText: subType })
          .click();
      });
    }

    await this.page
      .locator("#input-activityStatusCode")
      .selectOption({ label: activity.PropertyActivityStatus });

    //Inserting Requested Added Date
    await this.page.locator("#datepicker-requestedCommenceDate").fill("");
    await this.page
      .locator("#datepicker-requestedCommenceDate")
      .fill(activity.PropertyActivityRequestedCommenceDate);
    await this.page.locator("#datepicker-requestedCommenceDate").press("Enter");

    //Inserting Completion Date
    if (activity.PropertyActivityCompletionDate != null) {
      await this.page
        .locator("#datepicker-completionDate")
        .fill(activity.PropertyActivityCompletionDate);
      await this.page.locator("#datepicker-completionDate").press("Enter");
    }

    //Inserting Description
    await this.page.locator("#input-description").fill("");
    await this.page
      .locator("#input-description")
      .fill(activity.PropertyActivityDescription);

    //Deleting previous Ministry Contacts and adding new
    while (
      (await this.page
        .locator(
          "//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/button"
        )
        .count()) > 0
    ) {
      await this.page
        .locator(
          "//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/button"
        )
        .first()
        .click();
    }

    if (activity.PropertyActivityMinistryContactList.count() > 0) {
      await this.page.locator("#input-ministryContact").click();
      this.sharedSelectContacts.selectContact(
        activity.PropertyActivityMinistryContactList[0],
        ""
      );

      if (activity.PropertyActivityMinistryContactList.count() > 1) {
        activity.PropertyActivityMinistryContactList.forEach(
          (ministryContact, index) => {
            this.page.getByTestId("ministry-contacts-add-link").click();
            this.page.locator(`#input-ministryContacts[${index}].id`).click();
            this.sharedSelectContacts.selectContact(ministryContact, "");
          }
        );
      }
    }

    //Inserting Requestor
    if (activity.PropertyActivityRequestorContactMngr != null) {
      this.page.locator("#input-requestedSource").fill("");
      this.page
        .locator("#input-requestedSource")
        .fill(activity.PropertyActivityRequestorContactMngr);
    }

    //Deleting Involved parties and adding new
    while (
      (await this.page
        .locator(
          "//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/button"
        )
        .count()) > 0
    ) {
      await this.page
        .locator(
          "//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/button"
        )
        .first()
        .click();
    }

    if (activity.PropertyActivityInvolvedPartiesExtContactsList.count() > 0) {
      await this.page
        .locator(
          "//input[@id='input-involvedParties[0].id']/parent::div/parent::div/following-sibling::div/button"
        )
        .click();
      await this.sharedSelectContacts.selectContact(
        activity.PropertyActivityInvolvedPartiesExtContactsList[0],
        ""
      );

      if (activity.PropertyActivityInvolvedPartiesExtContactsList.count() > 1) {
        activity.PropertyActivityInvolvedPartiesExtContactsList.forEach(
          (involvedContact, index) => {
            this.page.getByTestId("external-contacts-add-link").click();
            this.page.locator(`#input-involvedParties[${index}].id`).click();
            this.sharedSelectContacts.selectContact(involvedContact, "");
          }
        );
      }
    }

     if (activity.PropertyActivityServiceProvider != null) {
        await this.page.locator("#input-serviceProvider.id").click();
        await this.sharedSelectContacts.selectContact(activity.PropertyActivityServiceProvider, "");
     }

     if (activity.ManagementPropertyActivityInvoices.count() > 0) {
        activity.ManagementPropertyActivityInvoices.forEach((invoice, index) => {
          this.addInvoice(invoice, index);
        });
     }
  }

  async cancelPropertyManagement() {
    await expect(this.page.locator("div[class='modal-header'] div[class='modal-title h4']")).toHaveTextContent("Confirm Changes");
    await expect(this.page.locator("div[class='modal-body']")).toHaveTextContent( /If you choose to cancel now, your changes will not be saved./);
    await expect(this.page.locator("div[class='modal-body']")).toHaveTextContent( /Do you want to proceed?/);
    await this.page.getByTestId("ok-modal-button").click();
  }

  async closeActivityTray() {
    await this.page.locator("button[id='close-tray']").click();
  }

  async verifyInsertedActivity(activity, activityType) {

    //Activity Details section
    await expect(this.page.locator("//div[contains(text(),'Activity Details')]")).toBeVisible();
    await expect(this.page.locator("//label[contains(text(),'Activity type')]")).toBeVisible();
    await expect(this.page.locator("//label[contains(text(),'Activity type')]/parent::div/following-sibling::div")).toHaveTextContent(activity.PropertyActivityType);

    AssertTrueIsDisplayed(managementActSubTypeLabel);
    if (activity.PropertyActivitySubTypeList.First() != "") {
      var subTypesUI = GetViewFieldListContent(managementActSubTypeContents);
      Assert.True(
        Enumerable.SequenceEqual(
          subTypesUI,
          activity.PropertyActivitySubTypeList
        )
      );
    }

    AssertTrueIsDisplayed(managementActStatusLabel);
    AssertTrueContentEquals(
      managementActStatusContent,
      activity.PropertyActivityStatus
    );

    if (activityType == "Management File") {
      AssertTrueIsDisplayed(managementActCommencementLabel);
      AssertTrueContentEquals(
        managementActCommencementContent,
        TransformDateFormat(activity.PropertyActivityRequestedCommenceDate)
      );
    } else {
      AssertTrueIsDisplayed(managementActRequestAddedDateLabel);
      AssertTrueContentEquals(
        managementActRequestAddedDateContent,
        TransformDateFormat(activity.PropertyActivityRequestedCommenceDate)
      );
    }

    if (activity.PropertyActivityCompletionDate != "") {
      AssertTrueIsDisplayed(managementActCompletionDateLabel);
      AssertTrueContentEquals(
        managementActCompletionDateContent,
        TransformDateFormat(activity.PropertyActivityCompletionDate)
      );
    }

    AssertTrueIsDisplayed(managementActDescriptionLabel);
    AssertTrueContentEquals(
      managementActDescriptionContent,
      activity.PropertyActivityDescription
    );

    AssertTrueIsDisplayed(managementActMinistryContactLabel);
    //  if (activity.PropertyActivityMinistryContactList.First() != "")
    //      for (int i = 0; i < activity.PropertyActivityMinistryContactList.Count; i++)
    //          Assert.Equal(webDriver.FindElements(managementActMinistryContactContent)[i].Text, activity.PropertyActivityMinistryContactList[i]);

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

    //  if (activityType == "Management File")
    //  {
    //      AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsLabel);
    //      if (activity.PropertyActivityInvolvedPartiesExtContactsList.First() != "")
    //         //  for (int i = 0; i < activity.PropertyActivityInvolvedPartiesExtContactsList.Count; i++)
    //         //      Assert.Equal(webDriver.FindElements(managementActDetailsActivityExternalContactsCount)[i].Text, activity.PropertyActivityInvolvedPartiesExtContactsList[i]);
    //  }
    //  else
    //  {
    //      AssertTrueIsDisplayed(managementActInvolvedPartiesLabel);
    //      if (activity.PropertyActivityInvolvedPartiesExtContactsList.First() != "")
    //         //  for (int i = 0; i < activity.PropertyActivityInvolvedPartiesExtContactsList.Count; i++)
    //         //      Assert.Equal(webDriver.FindElements(managementActInvolvedPartiesContent)[i].Text, activity.PropertyActivityInvolvedPartiesExtContactsList[i]);
    //  }

    AssertTrueIsDisplayed(managementActServiceProviderLabel);
    if (activity.PropertyActivityServiceProvider != "")
      AssertTrueContentEquals(
        managementActServiceProviderContent,
        activity.PropertyActivityServiceProvider
      );

    //Invoices section
    AssertTrueIsDisplayed(managementActInvoiceTotalsSubtitle);

    AssertTrueIsDisplayed(managementActInvoiceTotalPretaxLabel);
    AssertTrueContentEquals(
      managementActInvoiceTotalPretaxContent,
      TransformCurrencyFormat(activity.ManagementPropertyActivityTotalPreTax)
    );

    AssertTrueIsDisplayed(managementActInvoiceTotalGSTLabel);
    AssertTrueContentEquals(
      managementActInvoiceTotalGSTContent,
      TransformCurrencyFormat(activity.ManagementPropertyActivityTotalGST)
    );

    AssertTrueIsDisplayed(managementActInvoiceTotalPSTLabel);
    AssertTrueContentEquals(
      managementActInvoiceTotalPSTContent,
      TransformCurrencyFormat(activity.ManagementPropertyActivityTotalPST)
    );

    AssertTrueIsDisplayed(managementActInvoiceGrandTotalLabel);
    AssertTrueContentEquals(
      managementActInvoiceGrandTotalContent,
      TransformCurrencyFormat(activity.ManagementPropertyActivityGrandTotal)
    );
  }

  async VerifyCreateActivityInitForm(activityType, propsCount) {
    //Selected Properties
    if (activityType == "Management File") {
      AssertTrueIsDisplayed(managementActFilePropertiesTitle);
      AssertTrueIsDisplayed(managementActFileSelectedPropsLabel);
      Assert.Equal(
        webDriver.FindElements(managementActFilePropertiesCount).Count,
        propsCount
      );
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
    else AssertTrueIsDisplayed(managementActRequestAddedDateLabel);
    AssertTrueIsDisplayed(managementActRequestAddedCommenceDateInput);

    AssertTrueIsDisplayed(managementActCompletionDateLabel);
    AssertTrueIsDisplayed(managementActCompletionDateInput);
    AssertTrueIsDisplayed(managementActDescriptionLabel);
    AssertTrueIsDisplayed(managementActDescriptionInput);

    AssertTrueIsDisplayed(managementActMinistryContactLabel);
    AssertTrueIsDisplayed(managementActMinistryContactInput);
    AssertTrueIsDisplayed(managementActMinistryContactBttn);
    AssertTrueIsDisplayed(managementActMinistryContactAddContactLink);

    if (activityType == "Management File") {
      AssertTrueIsDisplayed(managementActContactManagerLabel);
      AssertTrueIsDisplayed(managementActContactManagerTooltip);
      AssertTrueIsDisplayed(managementActContactManagerContent);
    } else {
      AssertTrueIsDisplayed(managementActRequestorLabel);
      AssertTrueIsDisplayed(managementActRequestorTooltip);
      AssertTrueIsDisplayed(managementActRequestorInput);
    }

    if (activityType == "Management File") {
      AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsLabel);
      AssertTrueIsDisplayed(managementActDetailsActivityExternalContactsInput);
      AssertTrueIsDisplayed(
        managementActDetailsActivityExternalContactsAddBttn
      );
    } else {
      AssertTrueIsDisplayed(managementActInvolvedPartiesLabel);
      AssertTrueIsDisplayed(managementActInvolvedPartiesInput);
      AssertTrueIsDisplayed(managementActInvolvedPartiesExtContactsBttn);
      AssertTrueIsDisplayed(
        managementActInvolvedPartiesExtContactsAddContactLink
      );
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

  async addInvoice(invoice, index) {
    Wait();
    webDriver.FindElement(managementAddInvoiceBttn).Click();

    Wait();
    webDriver
      .FindElement(By.Id("input-invoices." + index + ".invoiceNum"))
      .SendKeys(invoice.PropertyActivityInvoiceNumber);

    webDriver
      .FindElement(By.Id("datepicker-invoices." + index + ".invoiceDateTime"))
      .SendKeys(invoice.PropertyActivityInvoiceDate);
    webDriver
      .FindElement(By.Id("datepicker-invoices." + index + ".invoiceDateTime"))
      .SendKeys(Keys.Enter);

    webDriver
      .FindElement(By.Id("input-invoices." + index + ".description"))
      .SendKeys(invoice.PropertyActivityInvoiceDescription);

    CleanUpCurrencyInput(By.Id("input-invoices." + index + ".pretaxAmount"));
    SendKeysToCurrencyInput(
      By.Id("input-invoices." + index + ".pretaxAmount"),
      invoice.PropertyActivityInvoicePretaxAmount
    );

    ChooseSpecificSelectOption(
      By.Id("input-invoices." + index + ".isPstRequired"),
      invoice.PropertyActivityInvoicePSTApplicable
    );

    AssertTrueElementValueEquals(
      By.Id("input-invoices." + index + ".gstAmount"),
      TransformCurrencyFormat(invoice.PropertyActivityInvoiceGSTAmount)
    );

    if (invoice.PropertyActivityInvoicePSTAmount != "0.00")
      AssertTrueElementValueEquals(
        By.Id("input-invoices." + index + ".pstAmount"),
        TransformCurrencyFormat(invoice.PropertyActivityInvoicePSTAmount)
      );
  }
}

module.exports = { SharedActivities };
