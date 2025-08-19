const SharedSelectContact = require("./SharedSelectContacts");
const {
  getViewFieldListContent,
  transformDateFormat,
  transformCurrencyFormat,
  clickCancelButton,
  clickSaveButton,
} = require("../../support/common.js");

class SharedActivities {
  constructor(page) {
    this.page = page;
    this.sharedSelectContacts = new SharedSelectContact(page);
  }

  async navigateActivitiesTab() {
    await this.page.locator("a[data-rb-event-key='activities']").click();
  }

  async addActivityBttn() {
    await this.page.getByTestId("add-activity-button").click();
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
      await this.sharedSelectContacts.selectContact(
        activity.PropertyActivityServiceProvider,
        ""
      );
    }

    if (activity.ManagementPropertyActivityInvoices.count() > 0) {
      activity.ManagementPropertyActivityInvoices.forEach((invoice, index) => {
        this.addInvoice(invoice, index);
      });
    }
  }

  async saveActivity() {
    clickSaveButton(this.page);
    await expect(
      this.page.locator("button[title='Edit property activity']")
    ).toBeVisible();
  }

  async cancelPropertyManagement() {
    clickCancelButton(this.page);
    await expect(
      this.page.locator("div[class='modal-header'] div[class='modal-title h4']")
    ).toHaveTextContent("Confirm Changes");
    await expect(
      this.page.locator("div[class='modal-body']")
    ).toHaveTextContent(
      /If you choose to cancel now, your changes will not be saved./
    );
    await expect(
      this.page.locator("div[class='modal-body']")
    ).toHaveTextContent(/Do you want to proceed?/);
    await this.page.getByTestId("ok-modal-button").click();
  }

  async closeActivityTray() {
    await this.page.locator("button[id='close-tray']").click();
  }

  async verifyInsertedActivity(activity, activityType) {
    //Activity Details section
    await expect(
      this.page.locator("//div[contains(text(),'Activity Details')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Activity type')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//label[contains(text(),'Activity type')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(activity.PropertyActivityType);

    await this.page
      .locator("//label[contains(text(),'Sub-type')]")
      .toBeVisible();
    if ((await activity.PropertyActivitySubTypeList.count()) > 0) {
      let subTypesUI = await getViewFieldListContent(
        this.page,
        "#multiselectContainerReact"
      );
      await expect([...subTypesUI]).toEqual(
        [...activity.PropertyActivitySubTypeList].sort()
      );
    }

    await expect(
      this.page.getByRole("label").filter({ hasText: "Activity status" })
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//label[contains(text(),'Activity status')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(activity.PropertyActivityStatus);

    if (activityType == "Management File") {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Commencement" })
      ).toBeVisible();
      await expect(
        this.page.locator(
          "//label[contains(text(),'Activity status')]/parent::div/following-sibling::div"
        )
      ).toHaveTextContent(activity.PropertyActivityStatus);
      await expect(
        this.page.locator(
          "//label[contains(text(),'Commencement')]/parent::div/following-sibling::div"
        )
      ).toHaveTextContent(
        transformDateFormat(activity.PropertyActivityRequestedCommenceDate)
      );
    } else {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Requested added date" })
      ).toBeVisible();
      await expect(
        this.page.locator(
          "//label[contains(text(),'Requested added date')]/parent::div/following-sibling::div"
        )
      ).toHaveTextContent(
        transformDateFormat(activity.PropertyActivityRequestedCommenceDate)
      );
    }

    if (activity.PropertyActivityCompletionDate != null) {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Completion" })
      ).toBeVisible();
      await expect(
        this.page.locator(
          "//label[contains(text(),'Completion date')]/parent::div/following-sibling::div"
        )
      ).toHaveTextContent(
        transformDateFormat(activity.PropertyActivityCompletionDate)
      );
    }

    await expect(
      this.page.locator(
        "//div[contains(text(),'Activity Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Activity Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(activity.PropertyActivityDescription);

    await expect(
      this.page.getByRole("label").filter({ hasText: "Ministry contacts" })
    ).toBeVisible();
    if ((await activity.PropertyActivityMinistryContactList.count()) > 0) {
      await activity.PropertyActivityMinistryContactList.forEach(
        (ministryContact, index) => {
          expect(
            this.page
              .locator(
                "//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/a/span"
              )
              .nth(index + 1)
          ).toHaveTextContent(ministryContact);
        }
      );
    }

    if (activityType == "Management File") {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Contact manager" })
      ).toBeVisible();
      await expect(
        this.page.getByTestId("tooltip-icon-section-field-tooltip")
      ).toBeVisible();

      if (activity.PropertyActivityRequestorContactMngr != null) {
        await expect(
          this.page.locator(
            "//label[contains(text(),'Contact manager')]/parent::div/following-sibling::div"
          )
        ).toHaveTextContent(activity.PropertyActivityRequestorContactMngr);
      }
    } else {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Requestor" })
      ).toBeVisible();
      await expect(
        this.page.getByTestId("tooltip-icon-section-field-tooltip")
      ).toBeVisible();

      if (activity.PropertyActivityRequestorContactMngr != "") {
        await expect(
          this.page.locator(
            "//label[contains(text(),'Requestor')]/parent::div/following-sibling::div"
          )
        ).toHaveTextContent(activity.PropertyActivityRequestorContactMngr);
      }
    }

    if (activityType == "Management File") {
      await expect(
        this.page.getByRole("label").filter({ hasText: "External contacts" })
      ).toBeVisible();
      if (activity.PropertyActivityInvolvedPartiesExtContactsList.count() > 0) {
        activity.PropertyActivityInvolvedPartiesExtContactsList.forEach(
          (involvedParty, index) => {
            expect(
              this.page
                .locator(
                  "//label[contains(text(),'External contacts')]/parent::div/following-sibling::div/a"
                )
                .nth(index + 1)
            ).toHaveTextContent(involvedParty);
          }
        );
      }
    } else {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Involved parties" })
      ).toBeVisible();
      if (activity.PropertyActivityInvolvedPartiesExtContactsList.count() > 0) {
        await activity.PropertyActivityInvolvedPartiesExtContactsList.forEach(
          (involvedParty, index) => {
            expect(
              this.page
                .locator(
                  "//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/a/span"
                )
                .nth(index + 1)
            ).toHaveTextContent(involvedParty);
          }
        );
      }
    }

    await expect(
      this.page.getByRole("label").filter({ hasText: "Service provider" })
    ).toBeVisible();
    if (activity.PropertyActivityServiceProvider != null) {
      expect(
        this.page.locator(
          "//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/a/span"
        )
      ).toHaveTextContent(activity.PropertyActivityServiceProvider);
    }

    //Invoices section
    await expect(this.page.getByText("Invoices Total")).toBeVisible();

    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total (before tax)')]/parent::div/following-sibling::div"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total (before tax)')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(
      transformCurrencyFormat(activity.ManagementPropertyActivityTotalPreTax)
    );

    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(
      transformCurrencyFormat(activity.ManagementPropertyActivityTotalGST)
    );

    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PST amount')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PST amount')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(
      transformCurrencyFormat(activity.ManagementPropertyActivityTotalPST)
    );

    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]/parent::div/following-sibling::div"
      )
    ).toHaveTextContent(
      transformCurrencyFormat(activity.ManagementPropertyActivityGrandTotal)
    );
  }

  async verifyCreateActivityInitForm(activityType, propsCount) {
    //Selected Properties
    if (activityType == "Management File") {
      await expect(this.page.getByText("Select File Properties")).toBeVisible();
      await expect(
        this.page.locator(
          "div[data-testid='selectableFileProperties'] div[class='tr-wrapper']"
        )
      )
        .count()
        .toBe(propsCount);
    }

    //Activity Details
    await expect(this.page.getByText("Activity Details")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Activity type" })
    ).toBeVisible();
    await expect(this.page.locator("#input-activityTypeCode")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Sub-type(s)" })
    ).toBeVisible();
    await expect(
      this.page.locator("#multiselect-activitySubtypeCodes")
    ).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Activity status" })
    ).toBeVisible();
    await expect(this.page.locator("#input-activityStatusCode")).toBeVisible();

    if (activityType == "Management File") {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Commencement" })
      ).toBeVisible();
    } else {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Requested added date" })
      ).toBeVisible();
    }
    await expect(this.page.locator("#datepicker-requestedDate")).toBeVisible();

    await expect(
      this.page.getByRole("label").filter({ hasText: "Completion" })
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-completionDate")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Description" })
    ).toBeVisible();
    await expect(this.page.locator("#input-description")).toBeVisible();

    await expect(
      this.page.getByRole("label").filter({ hasText: "Ministry contacts" })
    ).toBeVisible();
    await expect(
      this.page.locator("#input-ministryContacts[0].id")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("ministry-contacts-add-link")
    ).toBeVisible();

    if (activityType == "Management File") {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Contact manager" })
      ).toBeVisible();
      await expect(
        this.page.getByTestId("tooltip-icon-section-field-tooltip")
      ).toBeVisible();
      await expect(
        this.page.locator(
          "//label[contains(text(),'Contact manager')]/parent::div/following-sibling::div"
        )
      ).toBeVisible();
    } else {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Requestor" })
      ).toBeVisible();
      await expect(
        this.page.getByTestId("tooltip-icon-section-field-tooltip")
      ).toBeVisible();
      await expect(this.page.locator("#input-requestedSource")).toBeVisible();
    }

    if (activityType == "Management File") {
      await expect(
        this.page.getByRole("label").filter({ hasText: "External contacts" })
      ).toBeVisible();
      await expect(
        this.page.locator("#input-involvedParties[0].id")
      ).toBeVisible();
    } else {
      await expect(
        this.page.getByRole("label").filter({ hasText: "Involved parties" })
      ).toBeVisible();
      //await expect(this.page.locator("#input-requestedSource")).toBeVisible();
      //await expect(this.page.getByTestId("tooltip-icon-section-field-tooltip")).toBeVisible();
      //await expect(this.page.getByTestId("tooltip-icon-section-field-tooltip")).toBeVisible();
    }

    await expect(
      this.page.getByRole("label").filter({ hasText: "Service provider" })
    ).toBeVisible();
    await expect(this.page.locator("#input-serviceProvider.id")).toBeVisible();

    //Invoice
    await expect(this.page.getByTestId("add-invoice-button")).toBeVisible();
    await expect(this.page.getByText("Invoices Total")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Total (before tax)" })
    ).toBeVisible();
    await expect(this.page.locator("#input-pretaxAmount")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Total (before tax)" })
    ).toBeVisible();
    await expect(this.page.locator("#input-pretaxAmount")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "GST amount" })
    ).toBeVisible();
    await expect(this.page.locator("#input-gstAmount")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "PST amount" })
    ).toBeVisible();
    await expect(this.page.locator("#input-pstAmount")).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Total amount" })
    ).toBeVisible();
    await expect(this.page.locator("#input-totalAmount")).toBeVisible();
  }

  async verifyMgmtActivitiesInitListsView() {
    //Activity List
    await expect(this.page.getByText("Activity List")).toBeVisible();
    await expect(this.page.getByTestId("add-activity-button")).toBeVisible();
    await expect(
      this.page.locator(
        "//p[contains(text(),' You can attach a document after creating the activity. Create, then edit and attach a file if needed.')]"
      )
    ).toBeVisible();
    await expect(this.page.getByTestId("mgmt-activity-list")).toBeVisible();
    await expect(
      this.page.getByTestId("mgmt-activity-list", { text: "Activity type" })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("mgmt-activity-list", { text: "Activity sub-type" })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("mgmt-activity-list", { text: "Activity status" })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("mgmt-activity-list", { text: "Commencement" })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("mgmt-activity-list", { text: "Actions" })
    ).toBeVisible();

    //Ad-Hoc Activity List
    await expect(this.page.getByText("Ad-hoc Activities List")).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-property-file-activity-summary")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Ad-hoc Activities List')]/parent::div/parent::div/parent::div/following-sibling::div"
      )
    ).toBeVisible();
    await expect(
      this.page.getByTestId("adhoc-activity-list-readonly", {
        text: "Activity type",
      })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("adhoc-activity-list-readonly", {
        text: "Activity sub-type",
      })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("adhoc-activity-list-readonly", {
        text: "Activity status",
      })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("adhoc-activity-list-readonly", {
        text: "Commencement",
      })
    ).toBeVisible();
    await expect(
      this.page.getByTestId("adhoc-activity-list-readonly", {
        text: "Navigation",
      })
    ).toBeVisible();

    await expect(
      this.page.locator(
        "//div[contains(text(),'No property management activities found')]"
      )
    ).toBe(2);
  }

  async viewLastActivityFromList() {
    await this.page
      .locator(
        "//div[@data-testid='mgmt-activity-list']/following-sibling::div/div/ul[@class='pagination']/li[3]"
      )
      .click();
  }

  async viewLastActivityButton() {
    await this.page
      .locator(
        "div[data-testid='mgmt-activity-list'] div[class='tr-wrapper']:last-child button[title='property-activity view details']"
      )
      .click();
  }

  async verifyLastInsertedMgmtActivityTable(activity) {
    await expect(
      this.page.locator(
        "div[data-testid='mgmt-activity-list'] div[class='tr-wrapper']:last-child div[role='cell']:nth-child(1)"
      )
    ).toHaveTextContent(activity.PropertyActivityType);
    //await expect(this.page.locator("div[data-testid='mgmt-activity-list'] div[class='tr-wrapper']:last-child div[role='cell']:nth-child(2)")).toHaveTextContent(activity.PropertyActivitySubType);
    await expect(
      this.page.locator(
        "div[data-testid='mgmt-activity-list'] div[class='tr-wrapper']:last-child div[role='cell']:nth-child(3)"
      )
    ).toHaveTextContent(activity.PropertyActivityStatus);
    await expect(
      this.page.locator(
        "div[data-testid='mgmt-activity-list'] div[class='tr-wrapper']:last-child div[role='cell']:nth-child(4)"
      )
    ).toHaveTextContent(
      transformDateFormat(activity.PropertyActivityRequestedCommenceDate)
    );
  }

  async addInvoice(invoice, index) {
    await this.page.getByTestId("add-invoice-button").click();

    await this.page
      .locator("${#input-invoices." + index + ".invoiceNum}")
      .fill(invoice.PropertyActivityInvoiceNumber);
    await this.page
      .locator("${#datepicker-invoices." + index + ".invoiceDateTime}")
      .fill(invoice.PropertyActivityInvoiceDate);
    await this.page
      .locator("${#datepicker-invoices." + index + ".invoiceDateTime}")
      .press("Enter");

    await this.page
      .locator("${#input-invoices." + index + ".description}")
      .fill(invoice.PropertyActivityInvoiceDescription);

    await this.page
      .locator("${#input-invoices." + index + ".pretaxAmount}")
      .fill("");
    await this.page
      .locator("${#input-invoices." + index + ".pretaxAmount}")
      .fill(invoice.PropertyActivityInvoicePretaxAmount);

    await this.page
      .locator("${#input-invoices." + index + ".isPstRequired}")
      .selectOption({ label: invoice.PropertyActivityInvoicePSTApplicable });
    await this.page
      .locator("${#input-invoices." + index + ".gstAmount}")
      .toHaveValue(
        transformCurrencyFormat(invoice.PropertyActivityInvoiceGSTAmount)
      );

    if (invoice.PropertyActivityInvoicePSTAmount != "0.00") {
      await this.page
        .locator("${#input-invoices." + index + ".pstAmount}")
        .toHaveValue(
          transformCurrencyFormat(invoice.PropertyActivityInvoicePSTAmount)
        );
    }
  }
}

module.exports = SharedActivities;
