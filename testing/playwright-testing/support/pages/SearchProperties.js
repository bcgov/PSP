class SearchProperties {
  constructor(page) {
    this.page = page;
  }

  async searchPropertyByPID(pid) {
    await this.page.locator("#input-searchBy").selectOption({ label: "PID" });
    await this.page.locator("#input-pid").fill("");
    await this.page.locator("#input-pid").fill(pid);
    await this.page.getByTestId("search").click();
  }

  async searchPropertyByPIN(pin) {
    await this.page.locator("#input-searchBy").selectOption({ label: "PIN" });
    await this.page.locator("#input-pin").fill("");
    await this.page.locator("#input-pin").fill(pin);
    await this.page.getByTestId("search").click();
  }

  async searchPropertyByAddress(address) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Address" });
    await this.page.locator("#input-address").fill("");
    await this.page.locator("#input-address").fill(address);
    await this.page
      .locator("ul[class='suggestionList'] li:first-child")
      .click();
    await this.page.getByTestId("search").click();
  }

  async searchPropertyByPlan(plan) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Plan #" });
    await this.page.locator("#input-planNumber").fill("");
    await this.page.locator("#input-planNumber").fill(plan);
    await this.page.getByTestId("search").click();
  }

  async searchPropertyByLegalDescription(legalDescription) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Legal Description" });
    await this.page.locator("#input-legalDescription").fill("");
    await this.page.locator("#input-legalDescription").fill(legalDescription);
    await this.page.getByTestId("search").click();
  }

  async selectPropertyByLongLant(coordinates) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Lat/Long" });

    await this.page
      .locator("#number-input-coordinates\\.latitude\\.degrees")
      .fill(`${coordinates.LatitudeDegree}`);
    await this.page
      .locator("#number-input-coordinates\\.latitude\\.minutes")
      .fill(`${coordinates.LatitudeMinutes}`);
    await this.page
      .locator("#number-input-coordinates\\.latitude\\.seconds")
      .fill(`${coordinates.LatitudeSeconds}`);
    await this.page
      .locator("#input-coordinates\\.latitude\\.direction")
      .selectOption({ label: coordinates.LatitudeDirection });

    await this.page
      .locator("#number-input-coordinates\\.longitude\\.degrees")
      .fill(`${coordinates.LongitudeDegree}`);
    await this.page
      .locator("#number-input-coordinates\\.longitude\\.minutes")
      .fill(`${coordinates.LongitudeMinutes}`);
    await this.page
      .locator("#number-input-coordinates\\.longitude\\.seconds")
      .fill(`${coordinates.LongitudeSeconds}`);
    await this.page
      .locator("#input-coordinates\\.longitude\\.direction")
      .selectOption({ label: coordinates.LongitudeDirection });

    await this.page.getByTestId("search").click();
  }

  async resetSearch() {
    await this.page.getByTestId("reset-button").click();
  }

  async selectNthSearchResult(index) {
    await this.page
      .locator(
        `div[data-testid="search-property-${index}"] div:nth-child(1) div`
      )
      .click();
  }

  async addPropertyInfoToOpenFile() {
    await this.page.locator("#dropdown-ellipsis").click();
    await this.page.locator("a[aria-label='Add to Open File']").click();
  }

  async selectPinOnMap() {
    await this.page
      .locator("div[class='leaflet-pane leaflet-marker-pane'] img")
      .click();
  }
}

module.exports = SearchProperties;
