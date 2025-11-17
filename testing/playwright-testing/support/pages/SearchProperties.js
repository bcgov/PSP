const { expect } = require("@playwright/test");

class SearchProperties {
  constructor(page) {
    this.page = page;
  }

  async navigateSearchProperties() {
    await this.page.locator("#searchControlButton").click();
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
      .locator(`div[data-testid="search-property-${index}"]`)
      .click();
  }

  async addPropertyInfoToOpenFile() {
    await this.page.locator("#dropdown-ellipsis").click();
    await this.page.locator("a[aria-label='Add to Open File']").click();
  }

  async addPropertyInfoToWorklist() {
    await this.page
      .getByTestId("search-property-0")
      .getByLabel("More options")
      .click();
    await this.page.locator("a[aria-label='Add to Worklist']").click();
  }

  async selectPinOnMap() {
    await this.page
      .locator("div[class='leaflet-pane leaflet-marker-pane'] img")
      .click();
  }

  async verifySearchControlForm() {
    const searchPropertiesSidebar = await this.page.getByTestId(
      "search-sidebar"
    );
    expect(searchPropertiesSidebar).toBeVisible();

    const searchTitle = await this.page
      .locator("div[data-testid='search-sidebar'] p")
      .textContent();
    expect(searchTitle).toEqual("Search");

    const pimsPropsListBttn = await this.page.locator(
      "//button/div[text()='Search PIMS information']"
    );
    expect(pimsPropsListBttn).toBeVisible();

    const searchPropsTypesSelect = await this.page.locator("#input-searchBy");
    expect(searchPropsTypesSelect).toBeVisible();

    const searchPropsAllOptions = searchPropsTypesSelect.locator("option");
    await expect(searchPropsAllOptions).toHaveText([
      "PID",
      "PIN",
      "Address",
      "Plan #",
      "Historical File #",
      "POI Name",
      "Lat/Long",
      "Survey Parcel",
      "Project",
    ]);

    const searchPIDText = await this.page.locator("#input-searchBy");
    expect(searchPIDText).toBeVisible();

    const searchButton = await this.page.locator("#search-button");
    expect(searchButton).toBeVisible();

    const resetButton = await this.page.getByTestId("reset-button");
    expect(resetButton).toBeVisible();

    const PMBCResultsTitle = await this.page.locator(
      "//div[text()='Results (PMBC)']"
    );
    expect(PMBCResultsTitle).toBeVisible();

    const PMBCResultsMoreOptions = await this.page.locator(
      "button[aria-label='search pmbc results more options']"
    );
    expect(PMBCResultsMoreOptions).toBeVisible();

    const PIMSResultsTitle = await this.page.locator(
      "//div[text()='Results (PIMS)']"
    );
    expect(PIMSResultsTitle).toBeVisible();

    const PIMSResultsMoreOptions = await this.page.locator(
      "button[aria-label='search pims results more options']"
    );
    expect(PIMSResultsMoreOptions).toBeVisible();
  }

  async verifyPropertyQuickInfo() {
    const quickInfoLeaflet = await this.page.locator(
      "div[data-testid='quick-info']"
    );
    expect(quickInfoLeaflet).toBeVisible({ timeout: 120000 });

    const quickInfoViewIcon = await this.page.locator(
      "div[data-testid='view-property-icon']"
    );
    expect(quickInfoViewIcon).toBeVisible({ timeout: 120000 });

    const quickInfoMorOptions = await this.page.locator(
      "button[data-testid='quick-info-more-options']"
    );
    expect(quickInfoMorOptions).toBeVisible({ timeout: 120000 });

    const quickInfoHeader = await this.page.getByTestId("quick-info-header");
    expect(quickInfoHeader).toBeVisible({ timeout: 120000 });

    const quickInfoZoom = await this.page
      .getByTestId("quick-info-header")
      .getByRole("button", { name: "Fit boundaries button" });
    expect(quickInfoZoom).toBeVisible({ timeout: 120000 });

    const quickInfoMinimize = await this.page.getByTestId(
      "*[data-testid='toggle-icon']"
    );
    expect(quickInfoMinimize).toBeVisible({ timeout: 120000 });

    const quickInfoCloseIcon = await this.page.locator(
      "*[data-testid='close-icon']"
    );
    expect(quickInfoCloseIcon).toBeVisible({ timeout: 120000 });
  }
}

module.exports = SearchProperties;
