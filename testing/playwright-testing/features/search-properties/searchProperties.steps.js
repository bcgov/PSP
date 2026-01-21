const { Given, When, Then, setDefaultTimeout } = require("@cucumber/cucumber");
const searchPropertiesJson = require("../../data/SearchProperties.json");

let searchPropertiesData = [];
setDefaultTimeout(15000);

Given("I navigate to the Search Control", async function () {
  await this.searchProperties.navigateSearchProperties();
});

Given(
  "I search for several properties from row number {int} and add to the worklist",
  async function (rowNbr) {
    searchPropertiesData = searchPropertiesJson[rowNbr];
    if (searchPropertiesData.PID != "") {
      await this.searchProperties.searchPropertyByPID(searchPropertiesData.PID);
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (searchPropertiesData.PIN != "") {
      await this.searchProperties.searchPropertyByPIN(searchPropertiesData.PIN);
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (searchPropertiesData.Address != "") {
      await this.searchProperties.searchPropertyByAddress(
        searchPropertiesData.Address
      );
      await this.searchProperties.closePropertyLeaflet();
      await this.searchProperties.selectPinOnMap();
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (searchPropertiesData.PlanNumber != "") {
      await this.searchProperties.searchPropertyByPlan(
        searchPropertiesData.PlanNumber
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (searchPropertiesData.HistoricalFile != "") {
      await this.searchProperties.searchPropertyByHistoricalFile(
        searchPropertiesData.HistoricalFile
      );
      await this.searchProperties.selectNthPIMSSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (searchPropertiesData.POIName != "") {
      await this.searchProperties.searchPropertyByPOIName(
        searchPropertiesData.POIName
      );
      await this.searchProperties.closePropertyLeaflet();
      await this.searchProperties.selectPinOnMap();
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (searchPropertiesData.Coordinates.LatitudeDegree != "") {
      await this.searchProperties.searchPropertyByLongLant(
        searchPropertiesData.Coordinates
      );
      await this.searchProperties.closePropertyLeaflet();
      await this.searchProperties.selectPinOnMap();
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    // if (searchPropertiesData.SurveyParcel != "") {
    //   await this.searchProperties.searchPropertyBySurveyParcel(
    //     searchPropertiesData.SurveyParcel
    //   );
    //   await this.searchProperties.selectNthPMBCSearchResult(0);
    //   await this.searchProperties.addPropertyToWorklistFromQuickInfo();
    //   await this.searchProperties.resetSearch();
    // }

    if (searchPropertiesData.Project != "") {
      await this.searchProperties.searchPropertyByProject(
        searchPropertiesData.Project
      );
      await this.searchProperties.selectNthPIMSSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }
  }
);
