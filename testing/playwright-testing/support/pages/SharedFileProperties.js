class SharedFileProperties {
  constructor(page) {
    this.page = page;
  }

  async verifyPropertiesToIncludeInFileInitForm() {
    //Title and instructions
    await this.page
      .locator('h2 div:has-text("Properties to include in this file")').first()
      .isVisible();
    await this.page
      .locator(
        'p:has-text("Select one or more properties that you want to include in this disposition. You can choose a location from the map, or search by other criteria.")'
      )
      .isVisible();

    //Tabs search options
    await this.page.getByRole("tab", { name: "Locate on Map" }).isVisible();
    await this.page.getByRole("tab", { name: "Search" }).isVisible();

    //Select a Property Details
    await this.page.getByText("PID:").isVisible();
    await this.page.getByText("PIN:").isVisible();
    await this.page.getByText("Plan #:").isVisible();
    await this.page.getByText("Address:").isVisible();
    await this.page.getByText("Region:").isVisible();
    await this.page.getByText("District:").isVisible();
    await this.page.getByLabel("Legal Description:").isVisible();

    //Selected Properties section
    await this.page.getByText("Selected properties").isVisible();
    await this.page
      .getByTestId("tooltip-icon-property-selector-tooltip")
      .isVisible();
    await this.page.getByText("No Properties selected").isVisible();
  }
}

module.exports = { SharedFileProperties };
