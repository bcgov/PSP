using OpenQA.Selenium;
namespace PIMS.Tests.Automation.PageObjects
{
    public class MapFeatures: PageObjectBase
    {
        private readonly By mapLayersButton = By.Id("layersControlButton");
     
        private readonly By mapLayersTitle = By.XPath("//p[contains(text(),'Map Layers:')]");
        private readonly By mapLayersPIMSSubtitle = By.XPath("//div[contains(text(),'PIMS')]");
        private readonly By mapLayersPIMSResearchInput = By.CssSelector("input[id='research']");
        private readonly By mapLayersPIMSResearchLabel = By.CssSelector("label[for='research']");
        private readonly By mapLayersPIMSAcquisitionInput = By.CssSelector("input[id='acquisition']");
        private readonly By mapLayersPIMSAcquisitionLabel = By.CssSelector("label[for='acquisition']");
        private readonly By mapLayersPIMSManagementInput = By.CssSelector("input[id='management']");
        private readonly By mapLayersPIMSManagementLabel = By.CssSelector("label[for='management']");
        private readonly By mapLayersPIMSDispositionInput = By.CssSelector("input[id='disposition']");
        private readonly By mapLayersPIMSDispositionLabel = By.CssSelector("label[for='disposition']");
        private readonly By mapLayersPIMSLeasesBttn = By.XPath("//input[@id='LeaseLayers']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersPIMSLeasesInput = By.CssSelector("input[id='LeaseLayers']");
        private readonly By mapLayersPIMSLeasesLabel = By.CssSelector("label[for='LeaseLayers']");
        private readonly By mapLayersPIMSLeaseReceivableInput = By.CssSelector("input[id='lease_receivable']");
        private readonly By mapLayersPIMSLeaseReceivableLabel = By.CssSelector("label[for='lease_receivable']");
        private readonly By mapLayersPIMSLeasePayableInput = By.CssSelector("input[id='lease_payable']");
        private readonly By mapLayersPIMSLeasePayableLabel = By.CssSelector("label[for='lease_payable']");
        private readonly By mapLayersPIMSInterestBttn = By.XPath("//input[@id='InterestLayers']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersPIMSInterestInput = By.CssSelector("input[id='InterestLayers']");
        private readonly By mapLayersPIMSInterestLabel = By.CssSelector("label[for='InterestLayers']");
        private readonly By mapLayersPIMSLicenceConstructInput = By.CssSelector("input[id='license_to_construct_take']");
        private readonly By mapLayersPIMSLicenceConstructLabel = By.CssSelector("label[for='license_to_construct_take']");
        private readonly By mapLayersPIMSLandActInput = By.CssSelector("input[id='land_act_take']");
        private readonly By mapLayersPIMSLandActLabel = By.CssSelector("label[for='land_act_take']");
        private readonly By mapLayersPIMSSRWTakeInput = By.CssSelector("input[id='srw_take']");
        private readonly By mapLayersPIMSSRWTakeLabel = By.CssSelector("label[for='srw_take']");
        private readonly By mapLayersPIMSSurplusTakeInput = By.CssSelector("input[id='surplus_take']");
        private readonly By mapLayersPIMSSurplusTakeLabel = By.CssSelector("label[for='surplus_take']");
        private readonly By mapLayersPIMSInventoryTakeInput = By.CssSelector("input[id='inventory_take']");
        private readonly By mapLayersPIMSInventoryTakeLabel = By.CssSelector("label[for='inventory_take']");

        private readonly By mapLayersExternalSubtitle = By.XPath("//div[contains(text(),'External')]");
        private readonly By mapLayersAdminBoundariesCheck = By.XPath("//label[contains(text(),'Administrative Boundaries')]/preceding-sibling::input");
        private readonly By mapLayersAdminBoundariesLabel = By.XPath("//label[contains(text(),'Administrative Boundaries')]");
        private readonly By mapLayersAdminBoundariesCollapseBttn = By.XPath("//input[@id='administrative_group']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersCurrentCensusCheck = By.CssSelector("input[id='currentEconomicRegions']");
        private readonly By mapLayersCurrentCensusLabel = By.CssSelector("label[for='currentEconomicRegions']");
        private readonly By mapLayersMOTIRegionsCheck = By.CssSelector("input[id='moti']");
        private readonly By mapLayersMOTIRegionsLabel = By.CssSelector("label[for='moti']");
        private readonly By mapLayersMOTIHighwayCheck = By.CssSelector("input[id='motiHighwayDistricts']");
        private readonly By mapLayersMOTIHighwayLabel = By.CssSelector("label[for='motiHighwayDistricts']");
        private readonly By mapLayersMunicipalitiesCheck = By.CssSelector("input[id='municipalities']");
        private readonly By mapLayersMunicipalitiesLabel = By.CssSelector("label[for='municipalities']");
        private readonly By mapLayersRegionalDistrictsCheck = By.CssSelector("input[id='regionalDistricts ']");
        private readonly By mapLayersRegionalDistrictsLabel = By.CssSelector("label[for='regionalDistricts ']");

        private readonly By mapLayersLegalHighwayResearchCheck = By.CssSelector("input[id='legalHighwayResearch']");
        private readonly By mapLayersLegalHighwayResearchLabel = By.CssSelector("label[for='legalHighwayResearch']");
        private readonly By mapLayersLegalHighwayResearchCollapseBttn = By.XPath("//input[@id='legalHighwayResearch']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersGazettedHighwayCheck = By.CssSelector("input[id='gazettedHighway']");
        private readonly By mapLayersGazettedHighwayLabel = By.CssSelector("label[for='gazettedHighway']");
        private readonly By mapLayersClosedHighwayCheck = By.CssSelector("input[id='closedHighway']");
        private readonly By mapLayersClosedHighwayLabel = By.CssSelector("label[for='closedHighway']");
        private readonly By mapLayersParentParcelAcquisitionCheck = By.CssSelector("input[id='parentParcelAcquisition']");
        private readonly By mapLayersParentParcelAcquisitionLabel = By.CssSelector("label[for='parentParcelAcquisition']");
        private readonly By mapLayers107PlanCheck = By.CssSelector("input[id='section107Plan']");
        private readonly By mapLayers107PlanLabel = By.CssSelector("label[for='section107Plan']");
        private readonly By mapLayersMoTIPlanCheck = By.CssSelector("input[id='motiPlan']");
        private readonly By mapLayersMoTIPlanLabel = By.CssSelector("label[for='motiPlan']");
        private readonly By mapLayersMoTIPlanFootprintCheck = By.CssSelector("input[id='motiPlanFootprint']");
        private readonly By mapLayersMoTIPlanFootprintLabel = By.CssSelector("label[for='motiPlanFootprint']");
        private readonly By mapLayersPlansCheck = By.CssSelector("input[id='plans']");
        private readonly By mapLayersPlansLabel = By.CssSelector("label[for='plans']");

        private readonly By mapLayersFirstNationsCheck = By.CssSelector("input[id='firstNations']");
        private readonly By mapLayersFirstNationsLabel = By.CssSelector("label[for='firstNations']");
        private readonly By mapLayersFirstNationsCollapseBttn = By.XPath("//input[@id='firstNations']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersFirstNationsReservesCheck = By.CssSelector("input[id='firstNationsReserves']");
        private readonly By mapLayersFirstNationsReservesLabel = By.CssSelector("label[for='firstNationsReserves']");
        private readonly By mapLayersFirstNationTreatyAreasCheck = By.CssSelector("input[id='firstNationTreatyAreas']");
        private readonly By mapLayersFirstNationTreatyAreasLabel = By.CssSelector("label[for='firstNationTreatyAreas']");
        private readonly By mapLayersFirstNationsTreatyLandsCheck = By.CssSelector("input[id='firstNationTreatyLands']");
        private readonly By mapLayersFirstNationsTreatyLandsLabel = By.CssSelector("label[for='firstNationTreatyLands']");
        private readonly By mapLayersFirstNationsTreatyRelatedLandsCheck = By.CssSelector("input[id='firstNationTreatyRelatedLands']");
        private readonly By mapLayersFirstNationsTreatyRelatedLandsLabel = By.CssSelector("label[for='firstNationTreatyRelatedLands']");
        private readonly By mapLayersFirstNationTreatySideAgreementsCheck = By.CssSelector("input[id='firstNationTreatySideAgreement']");
        private readonly By mapLayersFirstNationTreatySideAgreementsLabel = By.CssSelector("label[for='firstNationTreatySideAgreement']");

        private readonly By mapLayersLandOwnershipCheck = By.CssSelector("input[id='landOwnership']");
        private readonly By mapLayersLandOwnershipLabel = By.CssSelector("label[for='landOwnership']");
        private readonly By mapLayersLandOwnershipCollapseBttn = By.XPath("//input[@id='landOwnership']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersCrownLeasesCheck = By.CssSelector("input[id='crownLeases']");
        private readonly By mapLayersCrownLeasesLabel = By.CssSelector("label[for='crownLeases']");
        private readonly By mapLayersCrownInventoryCheck = By.CssSelector("input[id='crownInventory']");
        private readonly By mapLayersCrownInventoryLabel = By.CssSelector("label[for='crownInventory']");
        private readonly By mapLayersCrownInclusionsCheck = By.CssSelector("input[id='crownInclusions']");
        private readonly By mapLayersCrownInclusionsLabel = By.CssSelector("label[for='crownInclusions']");
        private readonly By mapLayersCrownLandLicensesCheck = By.CssSelector("input[id='crownLandLicenses']");
        private readonly By mapLayersCrownLandLicensesLabel = By.CssSelector("label[for='crownLandLicenses']");
        private readonly By mapLayersCrownTenuresCheck = By.CssSelector("input[id='crownTenures']");
        private readonly By mapLayersCrownTenuresLabel = By.CssSelector("label[for='crownTenures']");
        private readonly By mapLayersSurveyedParcelsCheck = By.CssSelector("input[id='crownSurveyParcels']");
        private readonly By mapLayersSurveyedParcelsLabel = By.CssSelector("label[for='crownSurveyParcels']");
        private readonly By mapLayersParcelBoundariesCheck = By.CssSelector("input[id='parcelBoundaries']");
        private readonly By mapLayersParcelBoundariesLabel = By.CssSelector("label[for='parcelBoundaries']");
        private readonly By mapLayersInterestParcelsSRWCheck = By.CssSelector("input[id='srwInterestParcels']");
        private readonly By mapLayersInterestParcelsSRWLabel = By.CssSelector("label[for='srwInterestParcels']");

        private readonly By mapLayersZoningCheck = By.CssSelector("input[id='zoning']");
        private readonly By mapLayersZoningLabel = By.CssSelector("label[for='zoning']");
        private readonly By mapLayersZoningCollapseBttn = By.XPath("//input[@id='zoning']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersAgriculturalLandReserveCheck = By.CssSelector("input[id='agriculturalLandReserve']");
        private readonly By mapLayersAgriculturalLandReserveLabel = By.CssSelector("label[for='agriculturalLandReserve']");

        private readonly By mapLayersElectoralCheck = By.CssSelector("input[id='electoral']");
        private readonly By mapLayersElectoralLabel = By.CssSelector("label[for='electoral']");
        private readonly By mapLayersElectoralCollapseBttn = By.XPath("//input[@id='electoral']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersCurrentProvincialElectoralDistrictsBCCheck = By.CssSelector("input[id='currentElectoralDistricts']");
        private readonly By mapLayersCurrentProvincialElectoralDistrictsBCLabel = By.CssSelector("label[for='currentElectoralDistricts']");

        private readonly By mapLayersFederalBCParksCheck = By.CssSelector("input[id='federal_bc_parks']");
        private readonly By mapLayersFederalBCParksLabel = By.CssSelector("label[for='federal_bc_parks']");
        private readonly By mapLayersFederalBCParksCollapseBttn = By.XPath("//input[@id='federal_bc_parks']/parent::div/parent::div/preceding-sibling::div");
        private readonly By mapLayersFederalParksCheck = By.CssSelector("input[id='federalParks']");
        private readonly By mapLayersFederalParksLabel = By.CssSelector("label[for='federalParks']");
        private readonly By mapLayersBCParksCheck = By.CssSelector("input[id='bcParks']");
        private readonly By mapLayersBCParksLabel = By.CssSelector("label[for='bcParks']");


        public MapFeatures(IWebDriver driver) : base(driver)
        {}

        public void OpenMapLayers()
        {
            Wait();
            webDriver.FindElement(mapLayersButton).Click();
        }

        public void VerifyMapLayers()
        {
            Wait();
            AssertTrueIsDisplayed(mapLayersTitle);
            AssertTrueIsDisplayed(mapLayersPIMSSubtitle);
            AssertTrueIsDisplayed(mapLayersPIMSResearchInput);
            AssertTrueIsDisplayed(mapLayersPIMSResearchLabel);
            AssertTrueIsDisplayed(mapLayersPIMSAcquisitionInput);
            AssertTrueIsDisplayed(mapLayersPIMSAcquisitionLabel);
            AssertTrueIsDisplayed(mapLayersPIMSManagementInput);
            AssertTrueIsDisplayed(mapLayersPIMSManagementLabel);
            AssertTrueIsDisplayed(mapLayersPIMSDispositionInput);
            AssertTrueIsDisplayed(mapLayersPIMSDispositionLabel);

            webDriver.FindElement(mapLayersPIMSLeasesBttn).Click();
            AssertTrueIsDisplayed(mapLayersPIMSLeasesInput);
            AssertTrueIsDisplayed(mapLayersPIMSLeasesLabel);
            AssertTrueIsDisplayed(mapLayersPIMSLeaseReceivableInput);
            AssertTrueIsDisplayed(mapLayersPIMSLeaseReceivableLabel);
            AssertTrueIsDisplayed(mapLayersPIMSLeasePayableInput);
            AssertTrueIsDisplayed(mapLayersPIMSLeasePayableLabel);

            webDriver.FindElement(mapLayersPIMSInterestBttn).Click();
            AssertTrueIsDisplayed(mapLayersPIMSInterestInput);
            AssertTrueIsDisplayed(mapLayersPIMSInterestLabel);
            AssertTrueIsDisplayed(mapLayersPIMSLicenceConstructInput);
            AssertTrueIsDisplayed(mapLayersPIMSLicenceConstructLabel);
            AssertTrueIsDisplayed(mapLayersPIMSLandActInput);
            AssertTrueIsDisplayed(mapLayersPIMSLandActLabel);
            AssertTrueIsDisplayed(mapLayersPIMSSRWTakeInput);
            AssertTrueIsDisplayed(mapLayersPIMSSRWTakeLabel);
            AssertTrueIsDisplayed(mapLayersPIMSSurplusTakeInput);
            AssertTrueIsDisplayed(mapLayersPIMSSurplusTakeLabel);
            AssertTrueIsDisplayed(mapLayersPIMSInventoryTakeInput);
            AssertTrueIsDisplayed(mapLayersPIMSInventoryTakeLabel);

            AssertTrueIsDisplayed(mapLayersExternalSubtitle);
            AssertTrueIsDisplayed(mapLayersAdminBoundariesCheck);
            AssertTrueIsDisplayed(mapLayersAdminBoundariesLabel);
            webDriver.FindElement(mapLayersAdminBoundariesCollapseBttn).Click();

            Wait(1000);
            ScrollToElement(mapLayersRegionalDistrictsCheck);
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
            AssertTrueIsDisplayed(mapLayersSurveyedParcelsCheck);
            AssertTrueIsDisplayed(mapLayersSurveyedParcelsLabel);
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
        }
    }
}
