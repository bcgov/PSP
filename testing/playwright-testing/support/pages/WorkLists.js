const { expect } = require("@playwright/test");

class WorkLists {
  constructor(page) {
    this.page = page;
  }

  async navigateWorkLists() {
    await this.page.locator("#worklistControlButton").click();
  }

  async verifyWorkListForm() {
    await expect(
      this.page.locator("//p[contains(text(),'Working list')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'CTRL + Click to add a property')]"
      )
    ).toBeVisible();
  }

  async verifyWorklistWithProps() {
    this.page.locator("#worklistControlButton").click();

    await expect(
      this.page.locator("//p[contains(text(),'Working list')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("search-property-0")).toBeVisible();
  }

  async countItemsOnWorklist(propertyCountData) {
    const countWorklistItems = await this.page
      .locator("//button[@id='worklistControlButton']/following-sibling::div");
    await countWorklistItems.waitFor({state: "visible"});
    const worklistItemNumber = await countWorklistItems.textContent();
    return  worklistItemNumber == propertyCountData;
  }

  async deleteNthElementWorklist(index) {
    const propertyItem = await this.page.locator(
      `div[data-testid='search-property-${index}']`
    );
    await propertyItem.hover();

    const deleteBttn = await this.page.locator(
      `div[data-testid='search-property-${index}'] button[title='Delete parcel from list']`
    );
    await expect(deleteBttn).toBeVisible();
    await deleteBttn.click();
  }
}

module.exports = WorkLists;
