const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class ContactManager {
  constructor(page) {
    this.page = page;
  }
  async navigateCreateContact() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-contacts'] a",
      "div[data-testid='side-tray']"
    );
    await this.page.locator("//a[text()='Add a Contact']").click();
  }

  async navigateContactsListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );

    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-contacts'] a",
      "//a[text()='Manage Contacts']"
    );
    await this.page.locator("//a[text()='Manage Contacts']").click();
  }

  async verifyCreateContactForm() {
    await expect(
      this.page.locator("//h1[contains(text(),'Add Contact')]")
    ).toBeVisible();

    //Contact Type
    await expect(this.page.locator("#contact-individual")).toBeVisible();
    await expect(
      this.page.locator("label[for='contact-individual']")
    ).toBeVisible();
    await expect(this.page.locator("#contact-organization")).toBeVisible();
    await expect(
      this.page.locator("label[for='contact-organization']")
    ).toBeVisible();

    //Contact Details
    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Contact Details')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'First name')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-firstName")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Middle')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-middleNames")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Last name')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-surname")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Preferred name')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-preferredName")).toBeVisible();
    await expect(
      this.page.locator(
        "//label[contains(text(),'Link to an existing organization')]"
      )
    ).toBeVisible();
    await expect(this.page.locator("#typeahead-organization")).toBeVisible();

    //Contact Info
    await expect(
      this.page.locator(
        "//h2/div/div/div/span[contains(text(),'Contact Info')]"
      )
    ).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-contactInfoToolTip")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Email')]")
    ).toBeVisible();
    await expect(
      this.page.locator("input[id='input-emailContactMethods.0.value']")
    ).toBeVisible();

    const emailSelect = await this.page.locator(
      "select[id='input-emailContactMethods.0.contactMethodTypeCode']"
    );
    expect(emailSelect).toBeVisible();
    const emailOptions = await emailSelect.locator("option").count();
    expect(emailOptions).toBeGreaterThan(0);

    await expect(this.page.getByTestId("email-remove-button-0")).toBeVisible();
    await expect(this.page.getByTestId("add-email-button")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Phone')]")
    ).toBeVisible();
    await expect(
      this.page.locator("input[id='input-phoneContactMethods.0.value']")
    ).toBeVisible();
    const phoneTypeSelect = await this.page.locator(
      "select[id='input-emailContactMethods.0.contactMethodTypeCode']"
    );
    expect(phoneTypeSelect).toBeVisible();
    const phoneOptions = await phoneTypeSelect.locator("option").count();
    expect(phoneOptions).toBeGreaterThan(0);

    await expect(this.page.getByTestId("phone-delete-button-0")).toBeVisible();
    await expect(this.page.getByTestId("add-phone-button")).toBeVisible();

    //Mailing Address
    await expect(
      this.page.locator(
        "//h2/div/div/div/span[contains(text(),'Mailing Address')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("#input-useOrganizationAddress")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//label[text()='Use mailing address from organization']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("input[id='input-mailingAddress.streetAddress1']")
    ).toBeVisible();
    const mailingCountrySelect = await this.page.locator(
      "select[id='input-mailingAddress.countryId']"
    );
    expect(mailingCountrySelect).toBeVisible();
    const mailingCountryOptions = await mailingCountrySelect
      .locator("option")
      .count();
    expect(mailingCountryOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("input[id='input-mailingAddress.municipality']")
    ).toBeVisible();
    const provinceSelect = await this.page.locator(
      "select[id='input-mailingAddress.provinceId']"
    );
    expect(provinceSelect).toBeVisible();

    await expect(
      this.page.locator("input[id='input-mailingAddress.postal']")
    ).toBeVisible();

    //Property Address
    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Property Address')]")
    ).toBeVisible();
    await expect(
      this.page.locator("input[id='input-propertyAddress.streetAddress1']")
    ).toBeVisible();
    const propertyCountrySelect = await this.page.locator(
      "select[id='input-propertyAddress.countryId']"
    );
    expect(propertyCountrySelect).toBeVisible();
    const propertyCountryOptions = await propertyCountrySelect
      .locator("option")
      .count();
    expect(propertyCountryOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("input[id='input-propertyAddress.municipality']")
    ).toBeVisible();
    const propertyProvinceSelect = await this.page.locator(
      "select[id='input-propertyAddress.provinceId']"
    );
    expect(propertyProvinceSelect).toBeVisible();

    await expect(
      this.page.locator("input[id='input-propertyAddress.postal']")
    ).toBeVisible();

    //Billing Address
    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Billing Address')]")
    ).toBeVisible();
    await expect(
      this.page.locator("input[id='input-billingAddress.streetAddress1']")
    ).toBeVisible();
    const billingCountrySelect = await this.page.locator(
      "select[id='input-billingAddress.countryId']"
    );
    expect(billingCountrySelect).toBeVisible();
    const billingCountryOptions = await billingCountrySelect
      .locator("option")
      .count();
    expect(billingCountryOptions).toBeGreaterThan(0);
    await expect(
      this.page.locator("input[id='input-billingAddress.municipality']")
    ).toBeVisible();
    const billingProvinceSelect = await this.page.locator(
      "select[id='input-billingAddress.provinceId']"
    );
    expect(billingProvinceSelect).toBeVisible();

    await expect(
      this.page.locator("input[id='input-billingAddress.postal']")
    ).toBeVisible();

    const addressLineLabels = await this.page.locator(
      "//label[contains(text(),'Address (line 1)')]"
    );
    expect(addressLineLabels).toHaveCount(3);

    const countryLabels = await this.page.locator(
      "//label[contains(text(),'Country')]"
    );
    expect(countryLabels).toHaveCount(3);

    const cityLabels = await this.page.locator(
      "//label[contains(text(),'City')]"
    );
    expect(cityLabels).toHaveCount(3);

    const provinceLabels = await this.page.locator(
      "//label[contains(text(),'Province')]"
    );
    expect(provinceLabels).toHaveCount(3);

    const postalCodeLabels = await this.page.locator(
      "//label[contains(text(),'Postal Code')]"
    );
    expect(postalCodeLabels).toHaveCount(3);

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Comments')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-comment")).toBeVisible();

    await expect(
      this.page.locator("//div[text()='Cancel']/parent::button")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[text()='Save']/parent::button")
    ).toBeVisible();
  }

  async verifyDispositionListView() {
    await expect(this.page.locator("//span[text()='Contacts']")).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(this.page.locator("//b[text()='Search by:']")).toBeVisible();
    await expect(this.page.locator("#input-organizations")).toBeVisible();
    await expect(
      this.page.locator("//span[text()=' Organizations']")
    ).toBeVisible();
    await expect(this.page.locator("#input-persons")).toBeVisible();
    await expect(
      this.page.locator("//span[text()=' Individuals']")
    ).toBeVisible();
    await expect(this.page.locator("#input-all")).toBeVisible();
    await expect(this.page.locator("//span[text()=' All']")).toBeVisible();
    await expect(this.page.locator("#input-summary")).toBeVisible();
    await expect(
      this.page.locator("label[for='input-municipality']")
    ).toBeVisible();
    await expect(this.page.locator("#input-municipality")).toBeVisible();
    await expect(this.page.locator("#input-activeContactsOnly")).toBeVisible();
    await expect(
      this.page.locator("//span[text()='Show active only']")
    ).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Summary')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-summary")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'First name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-firstName")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-surname")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Organization')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-organizationName")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'E-mail')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Mailing address')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'City')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-municipalityName")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Prov')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Edit/View')]")
    ).toBeVisible();

    await this.page
      .locator(
        "div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .first()
      .waitFor({
        state: "visible",
        timeout: 10000,
      });

    const contactsCount = await this.page
      .locator(
        "div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
    expect(contactsCount).toBeGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async cancelCreateContact() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }
}
module.exports = ContactManager;
