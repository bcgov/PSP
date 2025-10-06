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
      .locator(
        `div[data-testid="search-property-${index}"] div:nth-child(1) div`
      )
      .click();
  }

  async addPropertyInfoToOpenFile() {
    await this.page.locator("#dropdown-ellipsis").click();
    await this.page.locator("a[aria-label='Add to Open File']").click();
  }

  async addPropertyInfoToWorklist() {
    await this.page.locator("#dropdown-ellipsis").click();
    await this.page.locator("a[aria-label='Add to Worklist']").click();
  }

  async selectPinOnMap() {
    await this.page
      .locator("div[class='leaflet-pane leaflet-marker-pane'] img")
      .click();
  }

  async verifySearchControlForm() {
    const searchPropertiesSidebar = await this.page.getByTestId("search-sidebar");
    expect(searchPropertiesSidebar).toBeVisible();

    const searchTitle = await this.page.locator("div[data-testid='search-sidebar'] p").textContent();
    expect(searchTitle).toEqual("Search");

    const pimsPropsListBttn = await this.page.locator("//button/div[text()='Search PIMS information']");
    expect(pimsPropsListBttn).toBeVisible();

    const searchPropsTypesSelect = await this.page.locator("#input-searchBy");
    expect(searchPropsTypesSelect).toBeVisible();

    const searchPropsAllOptions = searchPropsTypesSelect.locator('option');
    await expect(searchPropsAllOptions).toHaveText(['PID', 'PIN', 'Address', 'Plan #', 'Historical File #', 'POI Name', 'Lat/Long', 'Survey Parcel']);

    const searchPIDText = await this.page.locator("#input-searchBy");
    expect(searchPIDText).toBeVisible();

    const searchButton = await this.page.locator("#search-button");
    expect(searchButton).toBeVisible();

    const resetButton = await this.page.getByTestId("reset-button");
    expect(resetButton).toBeVisible();

    const PMBCResultsTitle = await this.page.locator("//div[text()='Results (PMBC)']");
    expect(PMBCResultsTitle).toBeVisible();

    const PMBCResultsMoreOptions = await this.page.locator("div[aria-label='search pmbc results more options']");
    expect(PMBCResultsMoreOptions).toBeVisible();

    const PIMSResultsTitle = await this.page.locator("//div[text()='Results (PIMS)']");
    expect(PMBCResultsTitle).toBeVisible();

    const PIMSResultsMoreOptions = await this.page.locator("div[aria-label='search pims results more options']");
    expect(PIMSResultsMoreOptions).toBeVisible();
  }

  async verifyPropertyQuickInfo() {
    const quickInfoLeaflet = this.page.getByTestId("quick-info");
    expect(quickInfoLeaflet).toBeVisible();

    const quickInfoViewIcon = this.page.getByTestId("view-property-icon");
    expect(quickInfoViewIcon).toBeVisible();

    const quickInfoMorOptions = this.page.getByTestId("quick-info-more-options");
    expect(quickInfoMorOptions).toBeVisible();

    const quickInfoHeader = this.page.getByTestId("quick-info-header");
    expect(quickInfoHeader).toBeVisible();

    const quickInfoZoom = this.page.getByTestId("zoom-to-location");
    expect(quickInfoZoom).toBeVisible();

    const quickInfoMinimize = this.page.getByTestId("toggle-icon");
    expect(quickInfoMinimize).toBeVisible();

    const quickInfoCloseIcon = this.page.getByTestId("close-icon");
    expect(quickInfoCloseIcon).toBeVisible();
  }
}

module.exports = SearchProperties;
