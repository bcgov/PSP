class SharedFileProperties {
  constructor(page) {
    this.page = page;
  }

  async verifyPropertiesToIncludeInFileInitForm() {
    //Title and instructions
    await this.page
      .locator('div:has-text("Properties to include in this file")')
      .isVisible();
    await this.page
      .locator(
        'p:has-text("Select one or more properties that you want to include in this disposition. You can choose a location from the map, or search by other criteria.")'
      )
      .isVisible();

    //Tabs search options
    await page.getByRole("tab", { name: "Locate on Map" }).isVisible();
    await page.getByRole("tab", { name: "Search" }).isVisible();

    //Select a Property Details
    await page.getByText("PID:").isVisible();
    await page.getByText("PIN:").isVisible();
    await page.getByText("Plan #:").isVisible();
    await page.getByText("Address:").isVisible();
    await page.getByText("Region:").isVisible();
    await page.getByText("District:").isVisible();
    await page.getByLabel("Legal Description:").isVisible();

    //Selected Properties section
    await page.getByText("Selected properties").isVisible();
    await page
      .getByTestId("tooltip-icon-property-selector-tooltip")
      .isVisible();
    await page.getByText("No Properties selected").isVisible();
  }
}

module.exports = { SharedFileProperties };
