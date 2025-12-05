const { Given, When, Then } = require("@cucumber/cucumber");
const searchPropertiesData = require("../../data/SearchProperties.json");

let searchPropertiesData = [];

Given("I navigate to the Search Control", async function () {
  await this.searchProperties.navigateSearchProperties();
});

When(
  "I search for several properties from row number {int}",
  async function (rowNbr) {
    searchPropertiesData = searchPropertiesData[rowNbr];
    if (!searchPropertiesData.PID.equals("")) {
      await this.searchProperties.searchPropertyByPID(searchPropertiesData.PID);
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.PIN.equals("")) {
      await this.searchProperties.searchPropertyByPIN(searchPropertiesData.PIN);
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.Address.equals("")) {
      await this.searchProperties.searchPropertyByAddress(
        searchPropertiesData.Address
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.PlanNumber.equals("")) {
      await this.searchProperties.searchPropertyByPlan(
        searchPropertiesData.PlanNumber
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.HistoricalFile.equals("")) {
      await this.searchProperties.searchPropertyByHistoricalFile(
        searchPropertiesData.HistoricalFile
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.POIName.equals("")) {
      await this.searchProperties.searchPropertyByPOIName(
        searchPropertiesData.POIName
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.Coordinates.LatitudeDegree.equals("")) {
      await this.searchProperties.selectPropertyByLongLant(
        searchPropertiesData.Coordinates
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.SurveyParcel.equals("")) {
      await this.searchProperties.searchPropertyBySurveyParcel(
        searchPropertiesData.SurveyParcel
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }

    if (!searchPropertiesData.Project.equals("")) {
      await this.searchProperties.searchPropertyByProject(
        searchPropertiesData.Project
      );
      await this.searchProperties.selectNthPMBCSearchResult(0);
      await this.searchProperties.addPropertyToWorklistFromQuickInfo();
      await this.searchProperties.resetSearch();
    }
  }
);
