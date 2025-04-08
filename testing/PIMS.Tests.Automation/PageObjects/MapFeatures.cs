using OpenQA.Selenium;
namespace PIMS.Tests.Automation.PageObjects
{
    public class MapFeatures: PageObjectBase
    {
        private readonly By mapFiltersButton = By.CssSelector("button[title='advanced-filter-button']");
        private readonly By mapLayersButton = By.Id("layersControlButton");

        private readonly By mapFiltersTitle = By.XPath("//p[contains(text(),'Filter By')]");
        private readonly By mapFilterCloseBttn = By.XPath("//p[contains(text(),'Filter By')]/following-sibling::*");
        private readonly By mapFilterResetButton = By.CssSelector("button[data-testid='reset-button']");
        private readonly By mapFilterResetInstructions = By.XPath("//div[contains(text(),'Reset to Default')]");

        private readonly By mapFilterOwnershipSubtitle = By.XPath("//div[contains(text(),'Show Ownership')]");
        private readonly By mapFilterOwnershipCollapseBttn = By.XPath("//div[contains(text(),'Show Ownership')]/following-sibling::*");
        private readonly By mapFilterIsCoreInventoryCheck = By.Id("input-isCoreInventory");
        private readonly By mapFilterIsCoreInventoryLabel = By.XPath("//span[contains(text(),'Core Inventory')]");
        private readonly By mapFilterIsPropertyOfInterestCheck = By.Id("input-isPropertyOfInterest");
        private readonly By mapFilterIsPropertyOfInterestLabel = By.XPath("//span[contains(text(),'Property of Interest')]");
        private readonly By mapFilterIsOtherInterestCheck = By.Id("input-isOtherInterest");
        private readonly By mapFilterIsOtherInterestLabel = By.XPath("//span[contains(text(),'Other Interest')]");
        private readonly By mapFilterIsDisposedCheck = By.Id("input-isDisposed");
        private readonly By mapFilterIsDisposedLabel = By.XPath("//span[contains(text(),'Disposed')]");
        private readonly By mapFilterIsRetiredCheck = By.Id("input-isRetired");
        private readonly By mapFilterIsRetiredLabel = By.XPath("//span[contains(text(),'Retired')]");

        private readonly By mapFilterProjectSubtitle = By.XPath("//div[contains(text(),'Project')]");
        private readonly By mapFilterProjectCollapseBttn = By.XPath("//div[contains(text(),'Project')]/following-sibling::*");
        private readonly By mapFilterProjectInput = By.Id("typeahead-projectPrediction");

        private readonly By mapFilterTenureSubtitle = By.XPath("//div[contains(text(),'Tenure')]");
        private readonly By mapFilterTenureCollapseBttn = By.XPath("//div[contains(text(),'Tenure')]/following-sibling::*");
        private readonly By mapFilterTenureStatusLabel = By.XPath("//div[contains(text(),'Tenure')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]");
        private readonly By mapFilterTenureStatusInput = By.Id("multiselect-tenureStatuses_input");
        private readonly By mapFilterProvinceHighwayLabel = By.XPath("//label[contains(text(),'Province Public Highway')]");
        private readonly By mapFilterProvinceHighwayInput = By.Id("input-tenurePPH");
        private readonly By mapFilterHighwayDetailsLabel = By.XPath("//label[contains(text(),'Highway / Road Details')]");
        private readonly By mapFilterHighwayDetailsInput = By.Id("multiselect-tenureRoadTypes_input");

        private readonly By mapFilterLeaseLicenseSubtitle = By.XPath("//div[contains(text(),'Lease / Licence')]");
        private readonly By mapFilterLeaseCollapseBttn = By.XPath("//div[contains(text(),'Lease / Licence')]/following-sibling::*");
        private readonly By mapFilterLeaseTransactionLabel = By.XPath("//label[contains(text(),'Lease Transaction')]");
        private readonly By mapFilterLeaseTransactionInput = By.Id("input-leasePayRcvblType");
        private readonly By mapFilterLeaseStatusLabel = By.XPath("//div[contains(text(),'Lease / Licence')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]");
        private readonly By mapFilterLeaseStatusInput = By.Id("input-leaseStatus");
        private readonly By mapFilterLeaseTypeLabel = By.XPath("//label[contains(text(),'Type(s)')]");
        private readonly By mapFilterLeaseTypeInput = By.Id("multiselect-leaseTypes_input");
        private readonly By mapFilterLeasePurposeLabel = By.XPath("//label[contains(text(),'Purpose(s)')]");
        private readonly By mapFilterLeasePurposeInput = By.Id("multiselect-leasePurposes_input");

        private readonly By mapFilterAnomalySubtitle = By.XPath("//div[contains(text(),'Anomaly')]");
        private readonly By mapFilterAnomalyCollapseBttn = By.XPath("//div[contains(text(),'Anomaly')]/following-sibling::*");
        private readonly By mapFilterAnomalyInput = By.Id("multiselect-anomalies_input");

        private readonly By mapLayersTitle = By.XPath("//p[contains(text(),'View Layer By')]");
        private readonly By mapLayersAdminBoundariesCheck = By.XPath("//label[contains(text(),'Administrative Boundaries')]/preceding-sibling::input");
        private readonly By mapLayersAdminBoundariesLabel = By.XPath("//label[contains(text(),'Administrative Boundaries')]");
        private readonly By mapLayersAdminBoundariesCollapseBttn = By.XPath("//div[@id='Administrative Boundaries']/*[1]");
        private readonly By mapLayersCurrentCensusCheck = By.CssSelector("div[id='Administrative Boundaries/currentEconomicRegions'] input");
        private readonly By mapLayersCurrentCensusLabel = By.CssSelector("div[id='Administrative Boundaries/currentEconomicRegions'] label");
        private readonly By mapLayersMOTIRegionsCheck = By.CssSelector("div[id='Administrative Boundaries/moti'] input");
        private readonly By mapLayersMOTIRegionsLabel = By.CssSelector("div[id='Administrative Boundaries/moti'] label");
        private readonly By mapLayersMOTIHighwayCheck = By.CssSelector("div[id='Administrative Boundaries/motiHighwayDistricts'] input");
        private readonly By mapLayersMOTIHighwayLabel = By.CssSelector("div[id='Administrative Boundaries/motiHighwayDistricts'] label");
        private readonly By mapLayersMunicipalitiesCheck = By.CssSelector("div[id='Administrative Boundaries/municipalities'] input");
        private readonly By mapLayersMunicipalitiesLabel = By.CssSelector("div[id='Administrative Boundaries/municipalities'] label");
        private readonly By mapLayersRegionalDistrictsCheck = By.CssSelector("div[id='Administrative Boundaries/regionalDistricts '] input");
        private readonly By mapLayersRegionalDistrictsLabel = By.CssSelector("div[id='Administrative Boundaries/regionalDistricts '] label");

        private readonly By mapLayersLegalHighwayResearchCheck = By.XPath("//label[contains(text(),'Legal Highway Research')]/preceding-sibling::input");
        private readonly By mapLayersLegalHighwayResearchLabel = By.XPath("//label[contains(text(),'Legal Highway Research')]");
        private readonly By mapLayersLegalHighwayResearchCollapseBttn = By.XPath("//div[@id='legalHighwayResearch']/*[1]");
        private readonly By mapLayersGazettedHighwayCheck = By.CssSelector("div[id='legalHighwayResearch/gazettedHighway'] input");
        private readonly By mapLayersGazettedHighwayLabel = By.CssSelector("div[id='legalHighwayResearch/gazettedHighway'] label");
        private readonly By mapLayersClosedHighwayCheck = By.CssSelector("div[id='legalHighwayResearch/closedHighway'] input");
        private readonly By mapLayersClosedHighwayLabel = By.CssSelector("div[id='legalHighwayResearch/closedHighway'] label");
        private readonly By mapLayersParentParcelAcquisitionCheck = By.CssSelector("div[id='legalHighwayResearch/parentParcelAcquisition'] input");
        private readonly By mapLayersParentParcelAcquisitionLabel = By.CssSelector("div[id='legalHighwayResearch/parentParcelAcquisition'] label");
        private readonly By mapLayers107PlanCheck = By.CssSelector("div[id='legalHighwayResearch/section107Plan'] input");
        private readonly By mapLayers107PlanLabel = By.CssSelector("div[id='legalHighwayResearch/section107Plan'] label");
        private readonly By mapLayersMoTIPlanCheck = By.CssSelector("div[id='legalHighwayResearch/motiPlan'] input");
        private readonly By mapLayersMoTIPlanLabel = By.CssSelector("div[id='legalHighwayResearch/motiPlan'] label");
        private readonly By mapLayersMoTIPlanFootprintCheck = By.CssSelector("div[id='legalHighwayResearch/motiPlanFootprint'] input");
        private readonly By mapLayersMoTIPlanFootprintLabel = By.CssSelector("div[id='legalHighwayResearch/motiPlanFootprint'] label");
        private readonly By mapLayersPlansCheck = By.CssSelector("div[id='legalHighwayResearch/plans'] input");
        private readonly By mapLayersPlansLabel = By.CssSelector("div[id='legalHighwayResearch/plans'] label");

        private readonly By mapLayersFirstNationsCheck = By.XPath("//label[contains(text(),'First Nations')]/preceding-sibling::input");
        private readonly By mapLayersFirstNationsLabel = By.XPath("//label[contains(text(),'First Nations')]");
        private readonly By mapLayersFirstNationsCollapseBttn = By.XPath("//div[@id='firstNations']/*[1]");
        private readonly By mapLayersFirstNationsReservesCheck = By.CssSelector("div[id='firstNations/firstNationsReserves'] input");
        private readonly By mapLayersFirstNationsReservesLabel = By.CssSelector("div[id='firstNations/firstNationsReserves'] label");
        private readonly By mapLayersFirstNationTreatyAreasCheck = By.CssSelector("div[id='firstNations/firstNationsReserves'] input");
        private readonly By mapLayersFirstNationTreatyAreasLabel = By.CssSelector("div[id='firstNations/firstNationsReserves'] label");
        private readonly By mapLayersFirstNationsTreatyLandsCheck = By.CssSelector("div[id='firstNations/firstNationTreatyLands'] input");
        private readonly By mapLayersFirstNationsTreatyLandsLabel = By.CssSelector("div[id='firstNations/firstNationTreatyLands'] label");
        private readonly By mapLayersFirstNationsTreatyRelatedLandsCheck = By.CssSelector("div[id='firstNations/firstNationTreatyRelatedLands'] input");
        private readonly By mapLayersFirstNationsTreatyRelatedLandsLabel = By.CssSelector("div[id='firstNations/firstNationTreatyRelatedLands'] label");
        private readonly By mapLayersFirstNationTreatySideAgreementsCheck = By.CssSelector("div[id='firstNations/firstNationTreatySideAgreement'] input");
        private readonly By mapLayersFirstNationTreatySideAgreementsLabel = By.CssSelector("div[id='firstNations/firstNationTreatySideAgreement'] label");

        private readonly By mapLayersLandOwnershipCheck = By.XPath("//label[contains(text(),'Land Ownership')]/preceding-sibling::input");
        private readonly By mapLayersLandOwnershipLabel = By.XPath("//label[contains(text(),'Land Ownership')]");
        private readonly By mapLayersLandOwnershipCollapseBttn = By.XPath("//div[@id='landOwnership']/*[1]");
        private readonly By mapLayersCrownLeasesCheck = By.CssSelector("div[id='landOwnership/crownLeases'] input");
        private readonly By mapLayersCrownLeasesLabel = By.CssSelector("div[id='landOwnership/crownLeases'] label");
        private readonly By mapLayersCrownInventoryCheck = By.CssSelector("div[id='landOwnership/crownInventory'] input");
        private readonly By mapLayersCrownInventoryLabel = By.CssSelector("div[id='landOwnership/crownInventory'] label");
        private readonly By mapLayersCrownInclusionsCheck = By.CssSelector("div[id='landOwnership/crownInclusions'] input");
        private readonly By mapLayersCrownInclusionsLabel = By.CssSelector("div[id='landOwnership/crownInclusions'] label");
        private readonly By mapLayersCrownLandLicensesCheck = By.CssSelector("div[id='landOwnership/crownLandLicenses'] input");
        private readonly By mapLayersCrownLandLicensesLabel = By.CssSelector("div[id='landOwnership/crownLandLicenses'] label");
        private readonly By mapLayersCrownTenuresCheck = By.CssSelector("div[id='landOwnership/crownTenures'] input");
        private readonly By mapLayersCrownTenuresLabel = By.CssSelector("div[id='landOwnership/crownTenures'] label");
        private readonly By mapLayersParcelBoundariesCheck = By.CssSelector("div[id='landOwnership/parcelBoundaries'] input");
        private readonly By mapLayersParcelBoundariesLabel = By.CssSelector("div[id='landOwnership/parcelBoundaries'] label");
        private readonly By mapLayersInterestParcelsSRWCheck = By.CssSelector("div[id='landOwnership/srwInterestParcels'] input");
        private readonly By mapLayersInterestParcelsSRWLabel = By.CssSelector("div[id='landOwnership/srwInterestParcels'] label");

        private readonly By mapLayersZoningCheck = By.XPath("//label[contains(text(),'Zoning')]/preceding-sibling::input");
        private readonly By mapLayersZoningLabel = By.XPath("//label[contains(text(),'Zoning')]");
        private readonly By mapLayersZoningCollapseBttn = By.XPath("//div[@id='zoning']/*[1]");
        private readonly By mapLayersAgriculturalLandReserveCheck = By.CssSelector("div[id='zoning/agriculturalLandReserve'] input");
        private readonly By mapLayersAgriculturalLandReserveLabel = By.CssSelector("div[id='zoning/agriculturalLandReserve'] label");

        private readonly By mapLayersElectoralCheck = By.XPath("//label[contains(text(),'Electoral')]/preceding-sibling::input");
        private readonly By mapLayersElectoralLabel = By.XPath("//label[contains(text(),'Electoral')]");
        private readonly By mapLayersElectoralCollapseBttn = By.XPath("//div[@id='electoral']/*[1]");
        private readonly By mapLayersCurrentProvincialElectoralDistrictsBCCheck = By.CssSelector("div[id='electoral/currentElectoralDistricts'] label");
        private readonly By mapLayersCurrentProvincialElectoralDistrictsBCLabel = By.CssSelector("div[id='electoral/currentElectoralDistricts'] input");

        private readonly By mapLayersFederalBCParksCheck = By.XPath("//label[contains(text(),'Federal/BC Parks')]/preceding-sibling::input");
        private readonly By mapLayersFederalBCParksLabel = By.XPath("//label[contains(text(),'Federal/BC Parks')]");
        private readonly By mapLayersFederalBCParksCollapseBttn = By.XPath("//div[@id='federal_bc_parks']/*[1]");
        private readonly By mapLayersFederalParksCheck = By.CssSelector("div[id='federal_bc_parks/federalParks'] input");
        private readonly By mapLayersFederalParksLabel = By.CssSelector("div[id='federal_bc_parks/federalParks'] label");
        private readonly By mapLayersBCParksCheck = By.CssSelector("div[id='federal_bc_parks/bcParks'] input");
        private readonly By mapLayersBCParksLabel = By.CssSelector("div[id='federal_bc_parks/bcParks'] label");

        private readonly By mapLayersPimsCheck = By.XPath("//label[contains(text(),'Pims')]/preceding-sibling::input");
        private readonly By mapLayersPimsLabel = By.XPath("//label[contains(text(),'Pims')]");
        private readonly By mapLayersPimsCollapseBttn = By.XPath("//div[@id='pims_property_boundary']/*[1]");
        private readonly By mapLayersPropertyBoundariesCheck = By.CssSelector("div[id='pims_property_boundary/PIMS_PROPERTY_BOUNDARY_KEY'] input");
        private readonly By mapLayersPropertyBoundariesLabel = By.CssSelector("div[id='pims_property_boundary/PIMS_PROPERTY_BOUNDARY_KEY'] label");


        public MapFeatures(IWebDriver driver) : base(driver)
        {
        }

        public void OpenMapFilters()
        {
            Wait();
            webDriver.FindElement(mapFiltersButton).Click();

        }

        public void OpenMapLayers()
        {
            Wait();
            webDriver.FindElement(mapLayersButton).Click();
        }

        public void VerifyMapFilters()
        {
            Wait();

            AssertTrueIsDisplayed(mapFiltersTitle);
            AssertTrueIsDisplayed(mapFilterCloseBttn);
            AssertTrueIsDisplayed(mapFilterResetButton);
            AssertTrueIsDisplayed(mapFilterResetInstructions); 

            AssertTrueIsDisplayed(mapFilterOwnershipSubtitle);
            AssertTrueIsDisplayed(mapFilterOwnershipCollapseBttn);
            AssertTrueIsDisplayed(mapFilterIsCoreInventoryCheck);
            AssertTrueIsDisplayed(mapFilterIsCoreInventoryLabel);
            AssertTrueIsDisplayed(mapFilterIsPropertyOfInterestCheck);
            AssertTrueIsDisplayed(mapFilterIsPropertyOfInterestLabel);
            AssertTrueIsDisplayed(mapFilterIsOtherInterestCheck);
            AssertTrueIsDisplayed(mapFilterIsOtherInterestLabel);
            AssertTrueIsDisplayed(mapFilterIsDisposedCheck);
            AssertTrueIsDisplayed(mapFilterIsDisposedLabel);
            AssertTrueIsDisplayed(mapFilterIsRetiredCheck);
            AssertTrueIsDisplayed(mapFilterIsRetiredLabel); 

            AssertTrueIsDisplayed(mapFilterProjectSubtitle);
            AssertTrueIsDisplayed(mapFilterProjectCollapseBttn);
            AssertTrueIsDisplayed(mapFilterProjectInput);

            AssertTrueIsDisplayed(mapFilterTenureSubtitle);
            AssertTrueIsDisplayed(mapFilterTenureCollapseBttn);
            AssertTrueIsDisplayed(mapFilterTenureStatusLabel);
            AssertTrueIsDisplayed(mapFilterTenureStatusInput);
            AssertTrueIsDisplayed(mapFilterProvinceHighwayLabel);
            AssertTrueIsDisplayed(mapFilterProvinceHighwayInput);
            AssertTrueIsDisplayed(mapFilterHighwayDetailsLabel);
            AssertTrueIsDisplayed(mapFilterHighwayDetailsInput);

            AssertTrueIsDisplayed(mapFilterLeaseLicenseSubtitle);
            AssertTrueIsDisplayed(mapFilterLeaseCollapseBttn);
            AssertTrueIsDisplayed(mapFilterLeaseTransactionLabel);
            AssertTrueIsDisplayed(mapFilterLeaseTransactionInput);
            AssertTrueIsDisplayed(mapFilterLeaseStatusLabel);
            AssertTrueIsDisplayed(mapFilterLeaseStatusInput);
            AssertTrueIsDisplayed(mapFilterLeaseTypeLabel);
            AssertTrueIsDisplayed(mapFilterLeaseTypeInput);
            AssertTrueIsDisplayed(mapFilterLeasePurposeLabel);
            AssertTrueIsDisplayed(mapFilterLeasePurposeInput);

            AssertTrueIsDisplayed(mapFilterAnomalySubtitle);
            AssertTrueIsDisplayed(mapFilterAnomalyCollapseBttn);
            AssertTrueIsDisplayed(mapFilterAnomalyInput);
        }

        public void VerifyMapLayers()
        {
            Wait();
            AssertTrueIsDisplayed(mapLayersTitle);

            AssertTrueIsDisplayed(mapLayersAdminBoundariesCheck);
            AssertTrueIsDisplayed(mapLayersAdminBoundariesLabel);
            webDriver.FindElement(mapLayersAdminBoundariesCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersCurrentCensusCheck);
            AssertTrueIsDisplayed(mapLayersCurrentCensusLabel);
            AssertTrueIsDisplayed(mapLayersMOTIRegionsCheck);
            AssertTrueIsDisplayed(mapLayersMOTIRegionsLabel);
            AssertTrueIsDisplayed(mapLayersMOTIHighwayCheck);
            AssertTrueIsDisplayed(mapLayersMOTIHighwayLabel);
            AssertTrueIsDisplayed(mapLayersMunicipalitiesCheck);
            AssertTrueIsDisplayed(mapLayersMunicipalitiesLabel);
            AssertTrueIsDisplayed(mapLayersRegionalDistrictsCheck);
            AssertTrueIsDisplayed(mapLayersRegionalDistrictsLabel);

            webDriver.FindElement(mapLayersAdminBoundariesCollapseBttn).Click();
            webDriver.FindElement(mapLayersLegalHighwayResearchCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersLegalHighwayResearchCheck);
            AssertTrueIsDisplayed(mapLayersLegalHighwayResearchLabel);
            AssertTrueIsDisplayed(mapLayersGazettedHighwayCheck);
            AssertTrueIsDisplayed(mapLayersGazettedHighwayLabel);
            AssertTrueIsDisplayed(mapLayersClosedHighwayCheck);
            AssertTrueIsDisplayed(mapLayersClosedHighwayLabel);
            AssertTrueIsDisplayed(mapLayersParentParcelAcquisitionCheck);
            AssertTrueIsDisplayed(mapLayersParentParcelAcquisitionLabel);
            AssertTrueIsDisplayed(mapLayers107PlanCheck);
            AssertTrueIsDisplayed(mapLayers107PlanLabel);
            AssertTrueIsDisplayed(mapLayersMoTIPlanCheck);
            AssertTrueIsDisplayed(mapLayersMoTIPlanLabel);
            AssertTrueIsDisplayed(mapLayersMoTIPlanFootprintCheck);
            AssertTrueIsDisplayed(mapLayersMoTIPlanFootprintLabel);
            AssertTrueIsDisplayed(mapLayersPlansCheck);
            AssertTrueIsDisplayed(mapLayersPlansLabel);

            webDriver.FindElement(mapLayersLegalHighwayResearchCollapseBttn).Click();
            webDriver.FindElement(mapLayersFirstNationsCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersFirstNationsCheck);
            AssertTrueIsDisplayed(mapLayersFirstNationsLabel);
            AssertTrueIsDisplayed(mapLayersFirstNationsReservesCheck);
            AssertTrueIsDisplayed(mapLayersFirstNationsReservesLabel);
            AssertTrueIsDisplayed(mapLayersFirstNationTreatyAreasCheck);
            AssertTrueIsDisplayed(mapLayersFirstNationTreatyAreasLabel);
            AssertTrueIsDisplayed(mapLayersFirstNationsTreatyLandsCheck);
            AssertTrueIsDisplayed(mapLayersFirstNationsTreatyLandsLabel);
            AssertTrueIsDisplayed(mapLayersFirstNationsTreatyRelatedLandsCheck);
            AssertTrueIsDisplayed(mapLayersFirstNationsTreatyRelatedLandsLabel);
            AssertTrueIsDisplayed(mapLayersFirstNationTreatySideAgreementsCheck);
            AssertTrueIsDisplayed(mapLayersFirstNationTreatySideAgreementsLabel);

            webDriver.FindElement(mapLayersFirstNationsCollapseBttn).Click();
            webDriver.FindElement(mapLayersLandOwnershipCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersLandOwnershipCheck);
            AssertTrueIsDisplayed(mapLayersLandOwnershipLabel);
            AssertTrueIsDisplayed(mapLayersCrownLeasesCheck);
            AssertTrueIsDisplayed(mapLayersCrownLeasesLabel);
            AssertTrueIsDisplayed(mapLayersCrownInventoryCheck);
            AssertTrueIsDisplayed(mapLayersCrownInventoryLabel);
            AssertTrueIsDisplayed(mapLayersCrownInclusionsCheck);
            AssertTrueIsDisplayed(mapLayersCrownInclusionsLabel);
            AssertTrueIsDisplayed(mapLayersCrownLandLicensesCheck);
            AssertTrueIsDisplayed(mapLayersCrownLandLicensesLabel);
            AssertTrueIsDisplayed(mapLayersCrownTenuresCheck);
            AssertTrueIsDisplayed(mapLayersCrownTenuresLabel);
            AssertTrueIsDisplayed(mapLayersParcelBoundariesCheck);
            AssertTrueIsDisplayed(mapLayersParcelBoundariesLabel);
            AssertTrueIsDisplayed(mapLayersInterestParcelsSRWCheck);
            AssertTrueIsDisplayed(mapLayersInterestParcelsSRWLabel);

            webDriver.FindElement(mapLayersLandOwnershipCollapseBttn).Click();
            webDriver.FindElement(mapLayersZoningCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersZoningCheck);
            AssertTrueIsDisplayed(mapLayersZoningLabel);
            AssertTrueIsDisplayed(mapLayersAgriculturalLandReserveCheck);
            AssertTrueIsDisplayed(mapLayersAgriculturalLandReserveLabel);

            webDriver.FindElement(mapLayersZoningCollapseBttn).Click();
            webDriver.FindElement(mapLayersElectoralCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersElectoralCheck);
            AssertTrueIsDisplayed(mapLayersElectoralLabel);
            AssertTrueIsDisplayed(mapLayersCurrentProvincialElectoralDistrictsBCCheck);
            AssertTrueIsDisplayed(mapLayersCurrentProvincialElectoralDistrictsBCLabel);

            webDriver.FindElement(mapLayersElectoralCollapseBttn).Click();
            webDriver.FindElement(mapLayersFederalBCParksCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersFederalBCParksCheck);
            AssertTrueIsDisplayed(mapLayersFederalBCParksLabel);
            AssertTrueIsDisplayed(mapLayersFederalParksCheck);
            AssertTrueIsDisplayed(mapLayersFederalParksLabel);
            AssertTrueIsDisplayed(mapLayersBCParksCheck);
            AssertTrueIsDisplayed(mapLayersBCParksLabel);

            webDriver.FindElement(mapLayersFederalBCParksCollapseBttn).Click();
            webDriver.FindElement(mapLayersPimsCollapseBttn).Click();

            Wait(1000);
            AssertTrueIsDisplayed(mapLayersPimsCheck);
            AssertTrueIsDisplayed(mapLayersPimsLabel);
            AssertTrueIsDisplayed(mapLayersPropertyBoundariesCheck);
            AssertTrueIsDisplayed(mapLayersPropertyBoundariesLabel);

            webDriver.FindElement(mapLayersPimsCollapseBttn).Click();
        }

        public void ResetMapFeatures()
        {
            Wait();
            webDriver.FindElement(mapFilterResetButton).Click();
        }
    }
}
