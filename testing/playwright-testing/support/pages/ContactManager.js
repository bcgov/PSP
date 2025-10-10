const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class ContactManager{
    constructor(page) {
    this.page = page;
  }
  async navigateCreateContact() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-contacts'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Add a Contact']").click();
  }

  async navigateContactsListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-contacts'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Contacts']").click();
  }

  async verifyCreateContactForm() {
    await expect(
      this.page.locator("//h1[contains(text(),'Add Contact')]")
    ).toBeVisible();

    await expect(this.page.locator("#contact-individual")).toBeVisible();
    await expect(this.page.locator("label[for='contact-individual']")).toBeVisible();
    await expect(this.page.locator("#contact-organization")).toBeVisible();
    await expect(this.page.locator("label[for='contact-organization']")).toBeVisible();

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
    await expect(
      this.page.locator("#input-middleNames")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Last name')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-surname")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Preferred name')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-preferredName")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Link to an existing organization')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#typeahead-organization")
    ).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Contact Info')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-contactInfoToolTip")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Email')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-emailContactMethods.0.value")).toBeVisible();
    await expect(this.page.locator("#input-emailContactMethods.0.contactMethodTypeCode")).toBeVisible();
    await expect(this.page.getByTestId("#email-remove-button-0")).toBeVisible();
    await expect(this.page.getByTestId("#add-email-button")).toBeVisible();
     await expect(
      this.page.locator("//label[contains(text(),'Phone')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-phoneContactMethods.0.value")).toBeVisible();
    await expect(this.page.locator("#input-emailContactMethods.0.contactMethodTypeCode")).toBeVisible();
    await expect(this.page.getByTestId("#phone-delete-button-0")).toBeVisible();
    await expect(this.page.getByTestId("#add-phone-button")).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Mailing Address')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-useOrganizationAddress")
    ).toBeVisible();
    await expect(this.page.locator("label[text()='Use mailing address from organization']")).toBeVisible();
    await expect(this.page.locator("#input-mailingAddress.streetAddress1")).toBeVisible();
    await expect(this.page.locator("#input-mailingAddress.countryId")).toBeVisible();
    await expect(
      this.page.locator("#input-mailingAddress.municipality")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-mailingAddress.provinceId")
    ).toBeVisible();
    await expect(this.page.locator("#input-mailingAddress.postal")).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Property Address')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-propertyAddress.streetAddress1")
    ).toBeVisible();
    await expect(this.page.locator("#input-propertyAddress.countryId")).toBeVisible();
    await expect(this.page.locator("#input-propertyAddress.municipality")).toBeVisible();
    await expect(
      this.page.locator("#input-propertyAddress.provinceId")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-propertyAddress.postal")
    ).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Billing Address')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-billingAddress.streetAddress1")).toBeVisible();
    await expect(this.page.locator("#input-billingAddress.countryId")).toBeVisible();
    await expect(
      this.page.locator("#input-billingAddress.municipality")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-billingAddress.provinceId")
    ).toBeVisible();
    await expect(this.page.locator("#input-billingAddress.postal")).toBeVisible();

    const addressLineLabels = await this.page.locator("//label[contains(text(),'Address (line 1)')]");
    expect(addressLineLabels).toHaveCount(3);

    const countryLabels = await this.page.locator("//label[contains(text(),'Country')]");
    expect(countryLabels).toHaveCount(3);

    const cityLabels = await this.page.locator("//label[contains(text(),'City')]");
    expect(cityLabels).toHaveCount(3);

    const provinceLabels= await this.page.locator("//label[contains(text(),'Province')]");
    expect(provinceLabels).toHaveCount(3);

    const postalCodeLabels = await this.page.locator("//label[contains(text(),'Postal code')]");
    expect(postalCodeLabels).toHaveCount(3);

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Comments')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("input-comment")).toBeVisible();

    const tooltips = await this.page.getByTestId(
      "tooltip-icon-lease-status-tooltip"
    );
    expect(tooltips).toHaveCount(3);

    await expect(this.page.getByTestId("cancel-button")).toBeVisible();
    await expect(this.page.getByTestId("save-button")).toBeVisible();
  }

  async verifyDispositionListView() {
    await expect(
      this.page.locator("//span[text()='Contacts']")
    ).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(
      this.page.locator("//string[text()='Search by:']")
    ).toBeVisible();
    await expect(this.page.locator("#input-organizations")).toBeVisible();
    await expect(this.page.getByRole("span").filter({ hasText: " Organizations" })).toBeVisible();
    await expect(this.page.locator("#input-persons")).toBeVisible();
    await expect(this.page.getByRole("span").filter({ hasText: " Individuals" })).toBeVisible();
    await expect(this.page.locator("#input-all")).toBeVisible();
    await expect(this.page.getByRole("span").filter({ hasText: " All" })).toBeVisible();
    await expect(
      this.page.locator("#input-summary")
    ).toBeVisible();
    await expect(this.page.getByRole("label").filter({ hasText: "City" })).toBeVisible();
    await expect(this.page.locator("#input-municipality")).toBeVisible();
    await expect(this.page.locator("#input-activeContactsOnly")).toBeVisible();
    await expect(this.page.getByRole("span").filter({ hasText: "Show active only" })).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Summary')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-summary")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'First name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-firstName")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last name')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-surname")
    ).toBeVisible();
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

    const contactsCount = await this.page.locator(
      "div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
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