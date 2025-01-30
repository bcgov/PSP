using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyInformation : PageObjectBase
    {
        //Map Property LTSA ParcelMap Data Pop-Up Elements
        private readonly By propertyLeafletCloseLink = By.CssSelector("a[class='leaflet-popup-close-button']");
        private readonly By propertyLeafletTitle = By.XPath("//h5[contains(text(),'LTSA ParcelMap data')]");
        private readonly By propertyLeafletPIDLabel = By.XPath("//b[contains(text(),'Parcel PID:')]");
        private readonly By propertyLeafletPINLabel = By.XPath("//b[contains(text(),'Parcel PIN:')]");
        private readonly By propertyLeafletPlanNbrLabel = By.XPath("//b[contains(text(),'Plan number:')]");
        private readonly By propertyLeafletOwnerTypeLabel = By.XPath("//b[contains(text(),'Owner type:')]");
        private readonly By propertyLeafletMunicipalityLabel = By.XPath("//b[contains(text(),'Municipality:')]");
        private readonly By propertyLeafletAreaLabel = By.XPath("//b[contains(text(),'Area:')]");
        private readonly By propertyLeafletZoomMapZoomBttn = By.XPath("//button/div[contains(text(),'Zoom map')]");
        private readonly By propertyLeafletEllipsisBttn = By.CssSelector("button[data-testid='fly-out-ellipsis']");

        private readonly By propertyHideWindowBttn = By.XPath("//div[@class='col']/h1/parent::div/following-sibling::div/span/button");
        private readonly By propertyShowWindowBttn = By.XPath("//div/div/div/div/div/div/span/button");
        private readonly By propertyMoreOptionsMenu = By.CssSelector("div[class='list-group list-group-flush']");
        private readonly By propertyViewInfoBttn = By.XPath("//div[contains(text(),'View Property Info')]/parent::button");
        private readonly By propertyNewResearchFileBttn = By.XPath("//div[contains(text(),'Research File')]/parent::button");
        private readonly By propertyNewAcquisitionFileBttn = By.XPath("//div[contains(text(),'Acquisition File')]/parent::button");
        private readonly By propertyNewLeaseFileBttn = By.XPath("//div[contains(text(),'Lease/Licence File')]/parent::button");
        private readonly By propertyNewDispositionFileBttn = By.XPath("//div[contains(text(),'Disposition File')]/parent::button");
        private readonly By propertyNewSubdivisionFileBttn = By.XPath("//div[contains(text(),'Create Subdivision')]/parent::button");
        private readonly By propertyNewConsolidationFileBttn = By.XPath("//div[contains(text(),'Create Consolidation')]/parent::button");

        //Property Information Tabs Elements
        private readonly By propertyInformationTabsTotal = By.CssSelector("nav[role='tablist'] a");
        private readonly By propertyInformationTitleTab = By.XPath("//a[contains(text(),'Title')]");
        private readonly By propertyInformationValueTab = By.XPath("//a[contains(text(),'Value')]");

        //Property Information Header Elements
        private readonly By propertyInformationHeaderTitle = By.XPath("//div[@class='col']/h1[contains(text(),'Property Information')]");
        private readonly By propertyInformationHeaderAddressLabel = By.XPath("//label[contains(text(),'Civic Address')]");
        private readonly By propertyInformationHeaderAddressContent = By.XPath("//label[contains(text(),'Civic Address')]/parent::div/following-sibling::div");
        private readonly By propertyInformationHeaderPlanLabel = By.XPath("//label[contains(text(),'Plan')]");
        private readonly By propertyInformationHeaderPlanContent = By.XPath("//label[contains(text(),'Plan #')]/parent::div/following-sibling::div");
        private readonly By propertyInformationHeaderHistoricFileLabel = By.XPath("//label[contains(text(),'Historical file #:')]");
        private readonly By propertyInformationHeaderHistoricFileContent = By.XPath("//label[contains(text(),'Historical file #:')]/parent::div/following-sibling::div/span");
        private readonly By propertyInformationHeaderPIDLabel = By.XPath("//label[contains(text(),'PID')]");
        private readonly By propertyInformationHeaderPIDContent = By.XPath("//label[contains(text(),'PID')]/parent::div/following-sibling::div");
        private readonly By propertyInformationHeaderLandTypeLabel = By.XPath("//h1/parent::div/parent::div/following-sibling::div/div/div/div[2]/div/div/div/label[contains(text(),'Land parcel type')]");
        private readonly By propertyInformationHeaderLandTypeContent = By.XPath("//h1/parent::div/parent::div/following-sibling::div/div/div/div[2]/div/div/div/label[contains(text(),'Land parcel type')]/parent::div/following-sibling::div");
        private readonly By propertyInformationHeaderZoomBttn = By.CssSelector("button[title='Zoom Map']");

        //Title Tab Elements
        private readonly By propertyTitleInfo = By.XPath("//div[contains(text(),'This data was retrieved from LTSA')]");
        private readonly By propertyTitleDetailsTitle = By.XPath("//div[contains(text(),'Title Details')]");
        private readonly By propertyTitleNumberLabel = By.XPath("//label[contains(text(),'Title number')]");
        private readonly By propertyTitleLandTitleLabel = By.XPath("//label[contains(text(),'Land title district')]");
        private readonly By propertyTitleTaxationAuthoritiesLabel = By.XPath("//label[contains(text(),'Taxation authorities')]");

        private readonly By propertyTitleLandTitle = By.XPath("//h2/div/div[contains(text(),'Land')]");
        private readonly By propertyTitlePIDLabel = By.XPath("//div[contains(text(),'Land')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PID')]");
        private readonly By propertyTitleLandDescriptionLabel = By.XPath("//label[contains(text(),'Legal description')]");

        private readonly By propertyOwnershipInformationTitle = By.XPath("//h2/div/div[contains(text(),'Ownership Information')]");
        private readonly By propertyFractionalOwnershipLabel = By.XPath("//label[contains(text(),'Fractional ownership')]");
        private readonly By propertyJointTenancyLabel = By.XPath("//label[contains(text(),'Joint tenancy')]");
        private readonly By propertyOwnershipRemarksLabel = By.XPath("//label[contains(text(),'Ownership remarks')]");
        private readonly By propertyOwnerNameLabel = By.XPath("//label[contains(text(),'Owner name')]");
        private readonly By propertyIncorporationNbrLabel = By.XPath("//label[contains(text(),'Incorporation number')]");
        private readonly By propertyOccupationLabel = By.XPath("//label[contains(text(),'Occupation')]");
        private readonly By propertyAddressLabel = By.XPath("//div[contains(text(),'Ownership Information')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Address')]");

        private readonly By propertyChargesLienInterestsTitle = By.XPath("//h2/div/div[contains(text(),'Charges, Liens and Interests')]");
        private readonly By propertyNatureLabel = By.XPath("//label[contains(text(),'Nature')]");
        private readonly By propertyRegistrationLabel = By.XPath("//label[contains(text(),'Registration #')]");
        private readonly By propertyRegisteredDateLabel = By.XPath("//label[contains(text(),'Registered date')]");
        private readonly By propertyRegisteredOwnerLabel = By.XPath("//label[contains(text(),'Registered owner')]");

        private readonly By propertyDuplicateIndefeasibleTitle = By.XPath("//h2/div/div[contains(text(),'Duplicate Indefeasible Title')]");
        private readonly By propertyDuplicateIndefeasibleNoneContent = By.XPath("//div[contains(text(),'Duplicate Indefeasible Title')]/parent::div/parent::h2/following-sibling::div[contains(text(),'None')]");

        private readonly By propertyTransfersTitle = By.XPath("//h2/div/div[contains(text(),'Transfers')]");
        private readonly By propertyTransfersNoneContent = By.XPath("//div[contains(text(),'Transfers')]/parent::div/parent::h2/following-sibling::div[contains(text(),'None')]");

        private readonly By propertyNotesTitle = By.XPath("//h2/div/div[contains(text(),'Transfers')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Notes')]");
        private readonly By propertyMiscellaneousNotesLabel = By.XPath("//label[contains(text(),'Miscellaneous notes')]");
        private readonly By propertyParcelStatusLabel = By.XPath("//label[contains(text(),'Parcel status')]");

        //Property Value Elements
        private readonly By propertyValueInfo = By.XPath("//div[contains(text(),'This data was retrieved from BC Assessment on')]");

        private readonly By propertyAssessmentOverviewTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Overview')]");
        private readonly By propertyAssessmentPIDLabel = By.XPath("//div[contains(text(),'Assessment Overview')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PID')]");
        private readonly By propertyAssessmentJurisdictionLabel = By.XPath("//label[contains(text(),'Jurisdiction')]");
        private readonly By propertyAssessmentNeighbourhoodLabel = By.XPath("//label[contains(text(),'Neighbourhood')]");
        private readonly By propertyAssessmentOwnershipYearLabel = By.XPath("//label[contains(text(),'Ownership year')]");
        private readonly By propertyAssessmentRollNumberLabel = By.XPath("//label[contains(text(),'Roll number')]");
        private readonly By propertyAssessmentRollYearLabel = By.XPath("//label[contains(text(),'Roll year')]");
        private readonly By propertyAssessmentDocumentNumberLabel = By.XPath("//label[contains(text(),'Document number')]");

        private readonly By propertyAssessmentAddressTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Overview')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Property Address')]");
        private readonly By propertyValueAddressInfo = By.XPath("//div/p[contains(text(),'This is the property address as per BC Assessment (for reference)')]");
        private readonly By propertyValueAddressLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'Address'))]");
        private readonly By propertyValueCityLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'City'))]");
        private readonly By propertyValueProvinceLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'Province'))]");
        private readonly By propertyValuePostalCodeLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'Postal code'))]");

        private readonly By propertyAssessedValueTitle = By.XPath("//h2/div/div[contains(text(),'Assessed Value')]");
        private readonly By propertyAssessedTable = By.CssSelector("div[data-testid='Assessed Values Sales']");

        private readonly By propertyAssessmentDetailsTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Details')]");
        private readonly By propertyAssessmentManualClassLabel = By.XPath("//label[contains(text(),'Manual class')]");
        private readonly By propertyAssessmentActualUseLabel = By.XPath("//label[contains(text(),'Actual use')]");
        private readonly By propertyAssessmentALRLabel = By.XPath("//label[contains(text(),'ALR')]");
        private readonly By propertyAssessmentLandDimensionLabel = By.XPath("//label[contains(text(),'Land dimension')]");

        private readonly By propertyAssessmentSalesTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Details')]");
        private readonly By propertySalesDescription = By.XPath("//div[contains(text(),'Description')]");


        //Property Details Elements
        private readonly By propertyDetailsTab = By.XPath("//a[contains(text(),'Property Details')]");
        private readonly By propertyDetailsEditBttn = By.CssSelector("button[title='Edit property details']");

        private readonly By propertyDetailsAddressTitle = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/parent::div/preceding-sibling::div/h2/div/div[contains(text(),'Property Address')]");
        private readonly By propertyDetailsEditAddressTitle = By.XPath("//div[contains(text(),'Property Address')]");
        private readonly By propertyDetailsAddressNotAvailable = By.XPath("//b[contains(text(),'Property address not available')]");
        private readonly By propertyDetailsAddressLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div/label");
        private readonly By propertyDetailsAddressLine1Label = By.XPath("//label[contains(text(),'Address (line 1)')]");
        private readonly By propertyDetailsAddressLine1Content = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]/div[1]");
        private readonly By propertyDetailsAddressLine2Content = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]/div[2]");
        private readonly By propertyDetailsAddressLine3Content = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]/div[3]");
        private readonly By propertyDetailsCityLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[2]/div/label[contains(text(),'City')]");
        private readonly By propertyDetailsCityContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[2]/div/label[contains(text(),'City')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsEditCityLabel = By.XPath("//Label[contains(text(),'City')]");
        private readonly By propertyDetailsProvinceLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[3]/div/label[contains(text(),'Province')]");
        private readonly By propertyDetailsProvinceContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[3]/div/label[contains(text(),'Province')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsPostalCodeLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[4]/div/label[contains(text(),'Postal code')]");
        private readonly By propertyDetailsPostalCodeContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[4]/div/label[contains(text(),'Postal code')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsEditPostalCodeLabel = By.XPath("//Label[contains(text(),'Postal code')]");
        private readonly By propertyDetailsGeneralLocationLabel = By.XPath("//label[contains(text(),'General location')]");
        private readonly By propertyDetailsGeneralLocationContent = By.XPath("//label[contains(text(),'General location')]/parent::div/following-sibling::div");

        private readonly By propertyDetailsAttributesTitle = By.XPath("//div[contains(text(),'Property Attributes')]");
        private readonly By propertyDetailsAttrLegalDescLabel = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Legal description')]");
        private readonly By propertyDetailsAttrLegalDescContent = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Legal description')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrRegionLabel = By.XPath("//label[contains(text(),'MOTI region')]");
        private readonly By propertyDetailsAttrRegionDiv = By.XPath("//label[contains(text(),'MOTI region')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrHighwayLabel = By.XPath("//label[contains(text(),'Highways district')]");
        private readonly By propertyDetailsAttrHighwayDiv = By.XPath("//label[contains(text(),'Highways district')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrElectoralLabel = By.XPath("//label[contains(text(),'Electoral district')]");
        private readonly By propertyDetailsAttrElectoralDiv = By.XPath("//label[contains(text(),'Electoral district')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrAgriLandLabel = By.XPath("//label[contains(text(),'Agricultural land reserve')]");
        private readonly By propertyDetailsAttrAgriLandDiv = By.XPath("//label[contains(text(),'Agricultural land reserve')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrRailwayLabel = By.XPath("//label[contains(text(),'Railway belt / Dominion patent')]");
        private readonly By propertyDetailsAttrRailwayDiv = By.XPath("//label[contains(text(),'Railway belt / Dominion patent')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrLandParcelLabel = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Land parcel type')]");
        private readonly By propertyDetailsAttrLandParcelDiv = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Land parcel type')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrMunicipalLabel = By.XPath("//label[contains(text(),'Municipal zoning')]");
        private readonly By propertyDetailsAttrMunicipalDiv = By.XPath("//label[contains(text(),'Municipal zoning')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsAttrAnomaliesLabel = By.XPath("//label[contains(text(),'Anomalies')]");
        private readonly By propertyDetailsAttrAnomaliesDiv = By.XPath("//label[contains(text(),'Anomalies')]/parent::div/following-sibling::div/div/div/div");
        private readonly By propertyDetailsAttrCoordinatesLabel = By.XPath("//label[contains(text(),'Coordinates')]");
        private readonly By propertyDetailsAttrCoordinatesDiv = By.XPath("//label[contains(text(),'Coordinates')]/parent::div/following-sibling::div");

        private readonly By propertyDetailsTenureTitle = By.XPath("//div[contains(text(),'Tenure Status')]");
        private readonly By propertyDetailsTenureStatusLabel = By.XPath("//label[contains(text(),'Tenure status')]");
        private readonly By propertyDetailsTenureStatusDiv = By.XPath("//label[contains(text(),'Tenure status')]/parent::div/following-sibling::div/div/div/div");
        private readonly By propertyDetailsPublicHwyLabel = By.XPath("//label[contains(text(),'Provincial public hwy')]");
        private readonly By propertyDetailsPublicHwyDiv = By.XPath("//label[contains(text(),'Provincial public hwy')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsHighwayRoadEstablishLabel = By.XPath("//label[contains(text(),'Highway / Road established by')]");
        private readonly By propertyDetailsHighwayRoadEstablishDiv = By.XPath("//label[contains(text(),'Highway / Road established by')]/parent::div/following-sibling::div");

        private readonly By propertyDetailsAdjacentLandTypeLabel = By.XPath("//label[contains(text(),'Adjacent Land type')]");
        private readonly By propertyDetailsAdjacentLandTypeDiv = By.XPath("//label[contains(text(),'Adjacent Land type')]/parent::div/following-sibling::div");

        private readonly By propertyDetailsFirstNationTitle = By.XPath("//div[contains(text(),'First Nations Information')]");
        private readonly By propertyDetailsFirstNationBandNameLabel = By.XPath("//label[contains(text(),'Band name')]");
        private readonly By propertyDetailsFirstNationBandNameDiv = By.XPath("//label[contains(text(),'Band name')]/parent::div/following-sibling::div");
        private readonly By propertyDetailsFirstNationReserveLabel = By.XPath("//label[contains(text(),'Reserve name')]");
        private readonly By propertyDetailsFirstNationReserveDiv = By.XPath("//label[contains(text(),'Reserve name')]/parent::div/following-sibling::div");

        private readonly By propertyDetailsMeasurementsTitle = By.XPath("//div[contains(text(),'Measurements')]");
        private readonly By propertyDetailsMeasurementsAreaLabel = By.XPath("//label[contains(text(),'Area')]");
        private readonly By propertyDetailsAreaSqMtsLabel = By.XPath("//div[contains(text(),'sq. metres')]");
        private readonly By propertyDetailsAreaSqMtsContent = By.XPath("//label[contains(text(),'Area')]/parent::div/following-sibling::div/div/div[1]/div/div[1]/div[1]");
        private readonly By propertyDetailsAreaHtsLabel = By.XPath("//div[contains(text(),'hectares')]");
        private readonly By propertyDetailsAreaSqFeetLabel = By.XPath("//div[contains(text(),'sq. feet')]");
        private readonly By propertyDetailsAreaAcresLabel = By.XPath("//div[contains(text(),'acres')]");
        private readonly By propertyDetailsMeasurementVolumeParcelLabel = By.XPath("//label[contains(text(),'Is this a volumetric parcel?')]");
        private readonly By propertyDetailsMeasurementVolumeLabel = By.XPath("//label[contains(text(),'Volume')]");
        private readonly By propertyDetailsAreaMtsCubeLabel = By.XPath("//span[contains(text(),'metres')]");
        private readonly By propertyDetailsAreaMtsCubeContent = By.XPath("//label[contains(text(),'Volume')]/parent::div/following-sibling::div/div/div[1]/div/div[1]/div[1]/div[1]/div[1]");
        private readonly By propertyDetailsAreaFeetCubeLabel = By.XPath("//span[contains(text(),'feet')]");
        private readonly By propertyDetailsMeasurementTypeLabel = By.XPath("//label[contains(text(),'Type')]");
        private readonly By propertyDetailsMeasurementTypeContent = By.XPath("//label[contains(text(),'Type')]/parent::div/following-sibling::div");

        private readonly By propertyDetailsViewNotesTitle = By.XPath("//div[contains(text(),'Measurements')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Comments')]");
        private readonly By propertyDetailsEditNotesTitle = By.XPath("//h2/div/div[contains(text(),'Comments')]");
        private readonly By propertyDetailsViewNotesContent = By.XPath("//div[contains(text(),'Comments')]/parent::div/parent::h2/following-sibling::div/p");

        private readonly By propertyDetailsSubdivisionTitle = By.XPath("//div[contains(text(),'Subdivision History')]");
        private readonly By propertyDetailsSubdivisionNoneContent = By.XPath("//div[contains(text(),'This property is not part of a subdivision')]");

        private readonly By propertyDetailsConsolidationTitle = By.XPath("//div[contains(text(),'Consolidation History')]");
        private readonly By propertyDetailsConsolidationNoneContent = By.XPath("//div[contains(text(),'This property is not part of a consolidation')]");

        //Create Form elements
        private readonly By propertyDetailsAddressAddLineBttn = By.XPath("//div[contains(text(),'Add an address line')]/parent::button");
        private readonly By propertyDetailsAddressLine1Input = By.Id("input-address.streetAddress1");
        private readonly By propertyDetailsAddressLine2Input = By.Id("input-address.streetAddress2");
        private readonly By propertyDetailsAddressLine3Input = By.Id("input-address.streetAddress3");
        private readonly By propertyDetailsAddressLineDeleteBttn = By.XPath("//*[@data-testid='remove-button']/parent::div/parent::button");
        private readonly By propertyDetailsAddressCityInput = By.Id("input-address.municipality");
        private readonly By propertyDetailsPostalCodeInput = By.Id("input-address.postal");
        private readonly By propertyDetailsGeneralLocationInput = By.Id("input-generalLocation");

        private readonly By propertyAttrAddHistoricalFileButton = By.CssSelector("button[data-testid='add-historical-number']");
        private readonly By propertyAttrHistoricalFilesTotalCount = By.XPath("//label[contains(text(),'Historical file #')]/parent::div/following-sibling::div/div");
        private readonly By propertyAttributesHistoricalFile1stDeleteButton = By.CssSelector("div[data-testid='historical-number-row-0'] button");
        private readonly By propertyAttributesLegalDescriptionInput = By.Id("input-landLegalDescription");
        private readonly By propertyDetailsMotiRegionSelect = By.Id("input-regionTypeCode");
        private readonly By propertyDetailsHighwayDistrictSelect = By.Id("input-districtTypeCode");
        private readonly By propertyDetailsRailwaySelect = By.Id("input-isRwyBeltDomPatent");
        private readonly By propertyDetailsLandTypeSelect = By.Id("input-propertyTypeCode");
        private readonly By propertyDetailsMunicipalZoneInput = By.Id("input-municipalZoning");
        private readonly By propertyDetailsAnomaliesInput = By.Id("multiselect-anomalies_input");
        private readonly By propertyDetailsAnomaliesOptions = By.XPath("//input[@id='multiselect-anomalies_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private readonly By propertyDetailsAttrAnomaliesDeleteBttns = By.CssSelector("div[id='multiselect-anomalies'] i[class='custom-close']");

        private readonly By propertyDetailsTenureStatusInput = By.Id("multiselect-tenures_input");
        private readonly By propertyDetailsTenureOptions = By.XPath("//input[@id='multiselect-tenures_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private readonly By propertyDetailsTenureDeleteBttns = By.CssSelector("div[id='multiselect-tenures'] i[class='custom-close']");
        private readonly By propertyDetailsProvPublicHwy = By.Id("input-pphStatusTypeCode");
        private readonly By propertyDetailsRoadEstablishInput = By.Id("multiselect-roadTypes_input");
        private readonly By propertyDetailsRoadEstablishOptions = By.XPath("//input[@id='multiselect-roadTypes_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");

        private readonly By propertyDetailsAreaSqMtsInput = By.Name("area-sq-meters");
        private readonly By propertyDetailsAreaHctInput = By.Name("area-hectares");
        private readonly By propertyDetailsAreaSqFtInput = By.Name("area-sq-feet");
        private readonly By propertyDetailsAreaAcrInput = By.Name("area-acres");
        private readonly By propertyDetailsIsVolumeRadioYes = By.Id("input-true");
        private readonly By propertyDetailsIsVolumeRadioNo = By.Id("input-false");
        private readonly By propertyDetailsVolCubeMtsInput = By.Name("volume-cubic-meters");
        private readonly By propertyDetailsVolCubeFeetInput = By.Name("volume-cubic-feet");
        private readonly By propertyDetailsVolTypeSelect = By.Id("input-volumetricParcelTypeCode");

        private readonly By propertyDetailsNotesTextarea = By.Id("input-notes");

        //Property Information Confirmation Modal
        private readonly By propertyInformationConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public PropertyInformation(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void HideLeftSideForms()
        {
            WaitUntilSpinnerDisappear();

            WaitUntilVisible(propertyHideWindowBttn);
            webDriver.FindElement(propertyHideWindowBttn).Click();
        }

        public void ShowLeftSideForms()
        {
            WaitUntilSpinnerDisappear();

            WaitUntilVisible(propertyShowWindowBttn);
            webDriver.FindElement(propertyShowWindowBttn).Click();
        }

        public void CloseLTSAPopUp()
        {
            WaitUntilClickable(propertyLeafletCloseLink);
            webDriver.FindElement(propertyLeafletCloseLink).Click();
        }

        public void OpenMoreOptionsPopUp()
        {
            WaitUntilVisible(propertyLeafletEllipsisBttn);
            FocusAndClick(propertyLeafletEllipsisBttn);
        }

        public void ChooseCreationOptionFromPin(string option)
        {
            Wait();
            switch(option)
            {
                case "View Property Info":
                    ButtonElement(propertyViewInfoBttn);
                    break;
                case "Research File":
                    ButtonElement(propertyNewResearchFileBttn);
                    break;
                case "Acquisition File":
                    ButtonElement(propertyNewAcquisitionFileBttn);
                    break;
                case "Lease/License":
                    ButtonElement(propertyNewLeaseFileBttn);
                    break;
                case "Disposition File":
                    ButtonElement(propertyNewDispositionFileBttn);
                    break;
                case "Subdivision":
                    ButtonElement(propertyNewSubdivisionFileBttn);
                    break;
                case "Consolidation":
                    ButtonElement(propertyNewConsolidationFileBttn);
                    break;
            }

            Wait();
            while (webDriver.FindElements(propertyInformationConfirmationModal).Count > 0)
            {
                Assert.Equal("User Override Required", sharedModals.ModalHeader());

                if (sharedModals.ModalContent().Contains("This property has already been added to one or more acquisition files."))
                {
                    Assert.Contains("This property has already been added to one or more acquisition files.", sharedModals.ModalContent());
                    Assert.Contains("Do you want to acknowledge and proceed?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                    break;
                }
                if (sharedModals.ModalContent().Contains("This property has already been added to one or more research files."))
                {
                    Assert.Contains("This property has already been added to one or more research files.", sharedModals.ModalContent());
                    Assert.Contains("Do you want to acknowledge and proceed?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                    break;
                }
                if (sharedModals.ModalContent().Contains("This property has already been added to one or more disposition files."))
                {
                    Assert.Contains("This property has already been added to one or more disposition files.", sharedModals.ModalContent());
                    Assert.Contains("Do you want to acknowledge and proceed?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                    break;
                }

                Wait();
            }
        }

        public void NavigatePropertyDetailsTab()
        {
            WaitUntilClickable(propertyDetailsTab);
            webDriver.FindElement(propertyDetailsTab).Click();
        }

        public void NavigatePropertyTitleTab()
        {
            WaitUntilClickable(propertyInformationTitleTab);
            webDriver.FindElement(propertyInformationTitleTab).Click();
        }

        public void NavigatePropertyValueTab()
        {
            WaitUntilClickable(propertyInformationValueTab);
            webDriver.FindElement(propertyInformationValueTab).Click();
        }

        public void EditPropertyInfoBttn()
        {
            Wait();
            FocusAndClick(propertyDetailsEditBttn);
        }

        public void SavePropertyDetails()
        {
            ButtonElement("Save");
        }

        public void CancelPropertyDetails()
        {
            ButtonElement("Cancel");

            Wait();
            if (webDriver.FindElements(propertyInformationConfirmationModal).Count > 0)
                sharedModals.ModalClickOKBttn();
        }

        public void UpdatePropertyDetails(Property property)
        {
            Wait();

            //PROPERTY ADDRESS
            //Delete previous Line 2 or Line 3 if existing
            while (webDriver.FindElements(propertyDetailsAddressLineDeleteBttn).Count > 0)
            {
                webDriver.FindElements(propertyDetailsAddressLineDeleteBttn)[0].Click();
            }

            if (property.Address.AddressLine1 != "")
            {
                ClearInput(propertyDetailsAddressLine1Input);
                webDriver.FindElement(propertyDetailsAddressLine1Input).SendKeys(property.Address.AddressLine1);
            }
            if (property.Address.AddressLine2 != "")
            {
                webDriver.FindElement(propertyDetailsAddressAddLineBttn).Click();
                webDriver.FindElement(propertyDetailsAddressLine2Input).SendKeys(property.Address.AddressLine2);
            }
            if (property.Address.AddressLine3 != "")
            {
                webDriver.FindElement(propertyDetailsAddressAddLineBttn).Click();
                webDriver.FindElement(propertyDetailsAddressLine3Input).SendKeys(property.Address.AddressLine3);
            }
            if (property.Address.City != "")
            {
                ClearInput(propertyDetailsAddressCityInput);
                webDriver.FindElement(propertyDetailsAddressCityInput).SendKeys(property.Address.City);
            }
            if (property.Address.PostalCode != "")
            {
                ClearInput(propertyDetailsPostalCodeInput);
                webDriver.FindElement(propertyDetailsPostalCodeInput).SendKeys(property.Address.PostalCode);
            }
            if (property.GeneralLocation != "")
            {
                ClearInput(propertyDetailsGeneralLocationInput);
                webDriver.FindElement(propertyDetailsGeneralLocationInput).SendKeys(property.GeneralLocation);
            }

            //ATTRIBUTES
            while (webDriver.FindElements(propertyAttributesHistoricalFile1stDeleteButton).Count > 0)
                DeleteFirstHistoricalFile();

            if (property.PropertyHistoricalFiles!.Count > 0)
            {
                for (var i = 0; i < property.PropertyHistoricalFiles.Count; i++)
                    AddHistoricalFile(property.PropertyHistoricalFiles[i]);
            }

            if (property.LegalDescription != "")
            {
                ClearInput(propertyAttributesLegalDescriptionInput);
                webDriver.FindElement(propertyAttributesLegalDescriptionInput).SendKeys(property.LegalDescription);
            }

            if (property.MOTIRegion != "")
                ChooseSpecificSelectOption(propertyDetailsMotiRegionSelect, property.MOTIRegion);
            
            if (property.HighwaysDistrict != "")
                ChooseSpecificSelectOption(propertyDetailsHighwayDistrictSelect, property.HighwaysDistrict);
            
            if (property.RailwayBelt != "")
                ChooseSpecificSelectOption(propertyDetailsRailwaySelect, property.RailwayBelt);
            
            if (property.LandParcelType != "")
                ChooseSpecificSelectOption(propertyDetailsLandTypeSelect, property.LandParcelType);
            
            if (property.MunicipalZoning != "")
            {
                ClearInput(propertyDetailsMunicipalZoneInput);
                webDriver.FindElement(propertyDetailsMunicipalZoneInput).SendKeys(property.MunicipalZoning);
            }

            //Delete Annomalies previously selected if any
            if (webDriver.FindElements(propertyDetailsAttrAnomaliesDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(propertyDetailsAttrAnomaliesDeleteBttns).Count > 0)
                    webDriver.FindElements(propertyDetailsAttrAnomaliesDeleteBttns)[0].Click(); 
            }

            if (property.Anomalies.First() != "")
            {
                foreach (string anomaly in property.Anomalies)
                {
                    FocusAndClick(propertyDetailsAnomaliesInput);

                    WaitUntilClickable(propertyDetailsAnomaliesOptions);
                    ChooseMultiSelectSpecificOption(propertyDetailsAnomaliesOptions, anomaly);
                }
            }

            //Delete Tenure status previously selected if any
            if (webDriver.FindElements(propertyDetailsTenureDeleteBttns).Count > 0)
            {
                FocusAndClick(propertyDetailsTenureStatusInput);
                while (webDriver.FindElements(propertyDetailsTenureDeleteBttns).Count > 0)
                {
                    webDriver.FindElement(propertyDetailsTenureStatusLabel).Click();
                    webDriver.FindElements(propertyDetailsTenureDeleteBttns)[0].Click();
                }
                webDriver.FindElement(propertyDetailsTenureStatusLabel).Click();
            }

            //TENURE STATUS
            if (property.TenureStatus.First() != "")
            {
                foreach (string status in property.TenureStatus)
                {
                    Wait(2000);
                    webDriver.FindElement(propertyDetailsTenureStatusLabel).Click();
                    FocusAndClick(propertyDetailsTenureStatusInput);

                    WaitUntilClickable(propertyDetailsTenureOptions);
                    ChooseMultiSelectSpecificOption(propertyDetailsTenureOptions, status);
                    webDriver.FindElement(propertyDetailsTenureStatusLabel).Click();
                }
            }

            if (property.ProvincialPublicHwy != "")
                ChooseSpecificSelectOption(propertyDetailsProvPublicHwy, property.ProvincialPublicHwy);  

            if (property.HighwayEstablishedBy.First() != "")
            {
                ClearMultiSelectInput(propertyDetailsRoadEstablishInput);
                foreach (string status in property.HighwayEstablishedBy)
                {
                    FocusAndClick(propertyDetailsRoadEstablishInput);
                    ChooseMultiSelectSpecificOption(propertyDetailsRoadEstablishOptions, status);
                }
            }

            //MEASUREMENTS
            if (property.SqrMeters != "")
            {
                ClearDigitsInput(propertyDetailsAreaSqMtsInput);
                webDriver.FindElement(propertyDetailsAreaSqMtsInput).SendKeys(property.SqrMeters);
            }

            if (property.IsVolumetric)
                FocusAndClick(propertyDetailsIsVolumeRadioYes);
            else
                FocusAndClick(propertyDetailsIsVolumeRadioNo);
            
            if (property.Volume != "")
            {
                ClearInput(propertyDetailsVolCubeMtsInput);
                webDriver.FindElement(propertyDetailsVolCubeMtsInput).SendKeys(property.Volume);
            }

            if (property.VolumeType != "")
                ChooseSpecificSelectOption(propertyDetailsVolTypeSelect, property.VolumeType);

            //NOTES
            if (property.PropertyNotes != "")
            {
                ClearInput(propertyDetailsNotesTextarea);
                webDriver.FindElement(propertyDetailsNotesTextarea).SendKeys(property.PropertyNotes);
            }
        }

        public void VerifyPropertyMapPopUpView()
        {
            WaitUntilVisible(propertyLeafletTitle);
            AssertTrueIsDisplayed(propertyLeafletCloseLink);
            AssertTrueIsDisplayed(propertyLeafletTitle);
            AssertTrueIsDisplayed(propertyLeafletPIDLabel);
            AssertTrueIsDisplayed(propertyLeafletPINLabel);
            AssertTrueIsDisplayed(propertyLeafletPlanNbrLabel);
            AssertTrueIsDisplayed(propertyLeafletOwnerTypeLabel);
            AssertTrueIsDisplayed(propertyLeafletMunicipalityLabel);
            AssertTrueIsDisplayed(propertyLeafletAreaLabel);
            AssertTrueIsDisplayed(propertyLeafletZoomMapZoomBttn);
            AssertTrueIsDisplayed(propertyLeafletEllipsisBttn);
        }

        public void VerifyTitleTab()
        {
            AssertTrueIsDisplayed(propertyTitleInfo);
            AssertTrueIsDisplayed(propertyTitleDetailsTitle);
            AssertTrueIsDisplayed(propertyTitleNumberLabel);
            AssertTrueIsDisplayed(propertyTitleNumberLabel);
            AssertTrueIsDisplayed(propertyTitleLandTitleLabel);
            AssertTrueIsDisplayed(propertyTitleTaxationAuthoritiesLabel);

            AssertTrueIsDisplayed(propertyTitleLandTitle);
            AssertTrueIsDisplayed(propertyTitlePIDLabel);
            AssertTrueIsDisplayed(propertyTitleLandDescriptionLabel);

            AssertTrueIsDisplayed(propertyTitleDetailsTitle);
            AssertTrueIsDisplayed(propertyFractionalOwnershipLabel);
            AssertTrueIsDisplayed(propertyJointTenancyLabel);
            AssertTrueIsDisplayed(propertyOwnershipRemarksLabel);
            AssertTrueIsDisplayed(propertyOwnerNameLabel);
            AssertTrueIsDisplayed(propertyIncorporationNbrLabel);
            AssertTrueIsDisplayed(propertyOccupationLabel);
            AssertTrueIsDisplayed(propertyAddressLabel);

            AssertTrueIsDisplayed(propertyChargesLienInterestsTitle);
            if (webDriver.FindElements(propertyNatureLabel).Count >= 1)
            {
                Assert.True(webDriver.FindElements(propertyNatureLabel).First().Displayed);
                Assert.True(webDriver.FindElements(propertyRegistrationLabel).First().Displayed);
                Assert.True(webDriver.FindElements(propertyRegisteredDateLabel).First().Displayed);
                Assert.True(webDriver.FindElements(propertyRegisteredOwnerLabel).First().Displayed);
            }

            AssertTrueIsDisplayed(propertyDuplicateIndefeasibleTitle);
            AssertTrueIsDisplayed(propertyDuplicateIndefeasibleNoneContent);

            AssertTrueIsDisplayed(propertyTransfersTitle);
            AssertTrueIsDisplayed(propertyTransfersNoneContent);

            AssertTrueIsDisplayed(propertyNotesTitle);
            AssertTrueIsDisplayed(propertyMiscellaneousNotesLabel);
            AssertTrueIsDisplayed(propertyParcelStatusLabel);
       
        }

        public void VerifyValueTab()
        {
            Wait();

            AssertTrueIsDisplayed(propertyValueInfo);

            AssertTrueIsDisplayed(propertyAssessmentOverviewTitle);
            AssertTrueIsDisplayed(propertyAssessmentPIDLabel);
            AssertTrueIsDisplayed(propertyAssessmentJurisdictionLabel);
            AssertTrueIsDisplayed(propertyAssessmentNeighbourhoodLabel);
            AssertTrueIsDisplayed(propertyAssessmentOwnershipYearLabel);
            AssertTrueIsDisplayed(propertyAssessmentRollNumberLabel);
            AssertTrueIsDisplayed(propertyAssessmentRollYearLabel);
            AssertTrueIsDisplayed(propertyAssessmentDocumentNumberLabel);

            AssertTrueIsDisplayed(propertyAssessmentAddressTitle);
            AssertTrueIsDisplayed(propertyValueAddressInfo);
            AssertTrueIsDisplayed(propertyValueAddressLabel);
            AssertTrueIsDisplayed(propertyValueCityLabel);
            AssertTrueIsDisplayed(propertyValueProvinceLabel);
            AssertTrueIsDisplayed(propertyValuePostalCodeLabel);

            AssertTrueIsDisplayed(propertyAssessedValueTitle);
            AssertTrueIsDisplayed(propertyAssessedTable);

            AssertTrueIsDisplayed(propertyAssessmentDetailsTitle);
            AssertTrueIsDisplayed(propertyAssessmentManualClassLabel);
            AssertTrueIsDisplayed(propertyAssessmentActualUseLabel);
            AssertTrueIsDisplayed(propertyAssessmentALRLabel);
            AssertTrueIsDisplayed(propertyAssessmentLandDimensionLabel);

            AssertTrueIsDisplayed(propertyAssessmentSalesTitle);
            AssertTrueIsDisplayed(propertySalesDescription);
        }

        public void VerifyPropertyInformationHeader(bool hasHistoricalFile)
        {
            Wait();

            AssertTrueIsDisplayed(propertyInformationHeaderTitle);

            AssertTrueIsDisplayed(propertyInformationHeaderAddressLabel);
            AssertTrueContentNotEquals(propertyInformationHeaderAddressContent, "");

            AssertTrueIsDisplayed(propertyInformationHeaderPlanLabel);
            //AssertTrueContentNotEquals(propertyInformationHeaderPlanContent, "");

            AssertTrueIsDisplayed(propertyInformationHeaderHistoricFileLabel);
            if(hasHistoricalFile)
                Assert.True(webDriver.FindElements(propertyInformationHeaderHistoricFileContent).Count > 0);

            AssertTrueIsDisplayed(propertyInformationHeaderPIDLabel);
            AssertTrueContentNotEquals(propertyInformationHeaderPIDContent, "");

            AssertTrueIsDisplayed(propertyInformationHeaderLandTypeLabel);
            AssertTrueContentNotEquals(propertyInformationHeaderLandTypeContent, "");

            AssertTrueIsDisplayed(propertyInformationHeaderZoomBttn);
        }

        public void VerifyPropertyDetailsView()
        {
            Wait();

            AssertTrueIsDisplayed(propertyDetailsAddressTitle);

            if (webDriver.FindElements(propertyDetailsAddressNotAvailable).Count() > 0)
                AssertTrueIsDisplayed(propertyDetailsAddressNotAvailable);
            
            else
            {
                AssertTrueIsDisplayed(propertyDetailsAddressLabel);
                AssertTrueIsDisplayed(propertyDetailsAddressLine1Content);
                AssertTrueIsDisplayed(propertyDetailsCityLabel);
                AssertTrueIsDisplayed(propertyDetailsCityContent);
                AssertTrueIsDisplayed(propertyDetailsProvinceLabel);
                AssertTrueIsDisplayed(propertyDetailsProvinceContent);
                AssertTrueIsDisplayed(propertyDetailsPostalCodeLabel);
                AssertTrueIsDisplayed(propertyDetailsPostalCodeContent);
            }

            AssertTrueIsDisplayed(propertyDetailsAttributesTitle);
            AssertTrueIsDisplayed(propertyDetailsAttrRegionLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrRegionDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrHighwayLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrHighwayDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrElectoralLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrElectoralDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrAgriLandLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrAgriLandDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrRailwayLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrRailwayDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrLandParcelLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrLandParcelDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrMunicipalLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrMunicipalDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrAnomaliesLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrAnomaliesDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrCoordinatesLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrCoordinatesDiv);

            AssertTrueIsDisplayed(propertyDetailsTenureTitle);
            AssertTrueIsDisplayed(propertyDetailsTenureStatusLabel);
            AssertTrueIsDisplayed(propertyDetailsTenureStatusDiv);
            AssertTrueIsDisplayed(propertyDetailsPublicHwyLabel);
            AssertTrueIsDisplayed(propertyDetailsPublicHwyDiv);

            if (webDriver.FindElements(propertyDetailsHighwayRoadEstablishLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsHighwayRoadEstablishLabel);
                AssertTrueIsDisplayed(propertyDetailsHighwayRoadEstablishDiv);
            }

            if (webDriver.FindElements(propertyDetailsAdjacentLandTypeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandTypeLabel);
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandTypeDiv);
            }

            if (webDriver.FindElements(propertyDetailsFirstNationTitle).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsFirstNationTitle);
                AssertTrueIsDisplayed(propertyDetailsFirstNationBandNameLabel);
                AssertTrueIsDisplayed(propertyDetailsFirstNationBandNameDiv);
                AssertTrueIsDisplayed(propertyDetailsFirstNationReserveLabel);
                AssertTrueIsDisplayed(propertyDetailsFirstNationReserveDiv);
            }

            AssertTrueIsDisplayed(propertyDetailsMeasurementsTitle);
            AssertTrueIsDisplayed(propertyDetailsMeasurementsAreaLabel);
            AssertTrueIsDisplayed(propertyDetailsMeasurementVolumeParcelLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaSqMtsLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaHtsLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaSqFeetLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaAcresLabel);

            if (webDriver.FindElements(propertyDetailsMeasurementVolumeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsMeasurementVolumeLabel);
                AssertTrueIsDisplayed(propertyDetailsMeasurementTypeLabel);
                AssertTrueIsDisplayed(propertyDetailsAreaMtsCubeLabel);
                AssertTrueIsDisplayed(propertyDetailsAreaFeetCubeLabel);
            }
            AssertTrueIsDisplayed(propertyDetailsViewNotesTitle);
        }

        public void VerifyUpdatePropertyDetailsView(Property property)
        {
            WaitUntilVisible(propertyDetailsEditBttn);

            //ADDRESS
            AssertTrueIsDisplayed(propertyDetailsAddressTitle);

            if (webDriver.FindElements(propertyDetailsAddressNotAvailable).Count() > 0)
                AssertTrueIsDisplayed(propertyDetailsAddressNotAvailable);
            else
            {
                AssertTrueIsDisplayed(propertyDetailsAddressLabel);
                if(property.Address.AddressLine1 != "")
                    AssertTrueContentEquals(propertyDetailsAddressLine1Content, property.Address.AddressLine1);

                if (property.Address.AddressLine2 != "")
                    AssertTrueContentEquals(propertyDetailsAddressLine2Content, property.Address.AddressLine2);

                if (property.Address.AddressLine3 != "")
                    AssertTrueContentEquals(propertyDetailsAddressLine3Content, property.Address.AddressLine3);

                AssertTrueIsDisplayed(propertyDetailsCityLabel);
                if(property.Address.City != "")
                    AssertTrueContentEquals(propertyDetailsCityContent, property.Address.City);

                AssertTrueIsDisplayed(propertyDetailsProvinceLabel);
                if(property.Address.Province != null)
                    AssertTrueContentEquals(propertyDetailsProvinceContent, "British Columbia");

                AssertTrueIsDisplayed(propertyDetailsPostalCodeLabel);
                if(property.Address.PostalCode != "")
                    AssertTrueContentEquals(propertyDetailsPostalCodeContent, property.Address.PostalCode);
            }

            AssertTrueIsDisplayed(propertyDetailsGeneralLocationLabel);
            if(property.GeneralLocation != "")
                AssertTrueContentEquals(propertyDetailsGeneralLocationContent, property.GeneralLocation);

            //ATTRIBUTES
            AssertTrueIsDisplayed(propertyDetailsAttributesTitle);

            AssertTrueIsDisplayed(propertyDetailsAttrLegalDescLabel);
            if (property.LegalDescription != "")
                AssertTrueContentEquals(propertyDetailsAttrLegalDescContent, property.LegalDescription);

            AssertTrueIsDisplayed(propertyDetailsAttrRegionLabel);
            AssertTrueContentEquals(propertyDetailsAttrRegionDiv, property.MOTIRegion);

            AssertTrueIsDisplayed(propertyDetailsAttrHighwayLabel);
            var highWayDistrictText = webDriver.FindElement(propertyDetailsAttrHighwayDiv).Text;
            Assert.Equal(property.HighwaysDistrict, GetSubstring(highWayDistrictText, 2, highWayDistrictText.Length));

            AssertTrueIsDisplayed(propertyDetailsAttrElectoralLabel);
            if(property.ElectoralDistrict != "")
                AssertTrueContentEquals(propertyDetailsAttrElectoralDiv,property.ElectoralDistrict);

            AssertTrueIsDisplayed(propertyDetailsAttrAgriLandLabel);
            if(property.AgriculturalLandReserve != "")
                AssertTrueContentEquals(propertyDetailsAttrAgriLandDiv, property.AgriculturalLandReserve);

            AssertTrueIsDisplayed(propertyDetailsAttrRailwayLabel);
            if(property.RailwayBelt != "")
                AssertTrueContentEquals(propertyDetailsAttrRailwayDiv, property.RailwayBelt);

            AssertTrueIsDisplayed(propertyDetailsAttrLandParcelLabel);
            if(property.LandParcelType != "")
                AssertTrueContentEquals(propertyDetailsAttrLandParcelDiv,property.LandParcelType);

            AssertTrueIsDisplayed(propertyDetailsAttrMunicipalLabel);
            if(property.MunicipalZoning != "")
                AssertTrueContentEquals(propertyDetailsAttrMunicipalDiv,property.MunicipalZoning);

            AssertTrueIsDisplayed(propertyDetailsAttrAnomaliesLabel);
            if (property.Anomalies.First() != "")
            {
                var anomaliesUI = GetViewFieldListContent(propertyDetailsAttrAnomaliesDiv);
                Assert.True(Enumerable.SequenceEqual(anomaliesUI, property.Anomalies));
            }

            AssertTrueIsDisplayed(propertyDetailsAttrCoordinatesLabel);

            //TENURE STATUS
            AssertTrueIsDisplayed(propertyDetailsTenureTitle);
            AssertTrueIsDisplayed(propertyDetailsTenureStatusLabel);

            if (property.TenureStatus.First() != "")
            {
                var tenureStatusUI = GetViewFieldListContent(propertyDetailsTenureStatusDiv);
                Assert.True(Enumerable.SequenceEqual(tenureStatusUI, property.TenureStatus));
            }

            AssertTrueIsDisplayed(propertyDetailsPublicHwyLabel);
            AssertTrueContentEquals(propertyDetailsPublicHwyDiv, property.ProvincialPublicHwy);

            if (webDriver.FindElements(propertyDetailsHighwayRoadEstablishLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsHighwayRoadEstablishLabel);

                var highwayEstablishedUI = GetViewFieldListContent(propertyDetailsHighwayRoadEstablishDiv);
                Assert.True(Enumerable.SequenceEqual(highwayEstablishedUI, property.HighwayEstablishedBy));
            }

            if (webDriver.FindElements(propertyDetailsFirstNationTitle).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsFirstNationTitle);
                AssertTrueIsDisplayed(propertyDetailsFirstNationBandNameLabel);
                AssertTrueIsDisplayed(propertyDetailsFirstNationBandNameDiv);
                AssertTrueIsDisplayed(propertyDetailsFirstNationReserveLabel);
                AssertTrueIsDisplayed(propertyDetailsFirstNationReserveDiv);
            }

            //MEASUREMENTS
            AssertTrueIsDisplayed(propertyDetailsMeasurementsTitle);
            AssertTrueIsDisplayed(propertyDetailsMeasurementsAreaLabel);
            if(property.SqrMeters != "")
                AssertTrueContentEquals(propertyDetailsAreaSqMtsContent, property.SqrMeters);
            AssertTrueIsDisplayed(propertyDetailsAreaSqMtsLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaHtsLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaSqFeetLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaAcresLabel);

            AssertTrueIsDisplayed(propertyDetailsMeasurementVolumeParcelLabel);
            if(property.Volume!= "")
                AssertTrueContentEquals(propertyDetailsAreaMtsCubeContent, property.Volume);

            if (webDriver.FindElements(propertyDetailsMeasurementVolumeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsMeasurementVolumeLabel);
                AssertTrueIsDisplayed(propertyDetailsMeasurementTypeLabel);
                if(property.VolumeType != "")
                    AssertTrueContentEquals(propertyDetailsMeasurementTypeContent, property.VolumeType);
                AssertTrueIsDisplayed(propertyDetailsAreaMtsCubeLabel);
                AssertTrueIsDisplayed(propertyDetailsAreaFeetCubeLabel);
            }

            //NOTES
            AssertTrueIsDisplayed(propertyDetailsViewNotesTitle);
            if(property.PropertyNotes != "")
                AssertTrueContentEquals(propertyDetailsViewNotesContent, property.PropertyNotes);

            //SUBDIVISION HISTORY
            AssertTrueIsDisplayed(propertyDetailsSubdivisionTitle);
            AssertTrueIsDisplayed(propertyDetailsSubdivisionNoneContent);

            //CONSOLIDATION HISTORY
            AssertTrueIsDisplayed(propertyDetailsConsolidationTitle);
            AssertTrueIsDisplayed(propertyDetailsConsolidationNoneContent);
        }

        public void VerifyPropertyDetailsEditForm()
        {
            WaitUntilVisible(propertyDetailsEditAddressTitle);

            AssertTrueIsDisplayed(propertyDetailsEditAddressTitle);
            AssertTrueIsDisplayed(propertyDetailsAddressLine1Label);
            AssertTrueIsDisplayed(propertyDetailsAddressLine1Input);

            if (webDriver.FindElements(propertyDetailsAddressLine3Input).Count == 0)
                AssertTrueIsDisplayed(propertyDetailsAddressAddLineBttn);
            
            AssertTrueIsDisplayed(propertyDetailsEditCityLabel);
            AssertTrueIsDisplayed(propertyDetailsAddressCityInput);
            AssertTrueIsDisplayed(propertyDetailsEditPostalCodeLabel);
            AssertTrueIsDisplayed(propertyDetailsPostalCodeInput);

            AssertTrueIsDisplayed(propertyDetailsAttributesTitle);
            AssertTrueIsDisplayed(propertyDetailsAttrRegionLabel);
            AssertTrueIsDisplayed(propertyDetailsMotiRegionSelect);
            AssertTrueIsDisplayed(propertyDetailsAttrHighwayLabel);
            AssertTrueIsDisplayed(propertyDetailsHighwayDistrictSelect);
            AssertTrueIsDisplayed(propertyDetailsAttrElectoralLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrElectoralDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrAgriLandLabel);
            AssertTrueIsDisplayed(propertyDetailsAttrAgriLandDiv);
            AssertTrueIsDisplayed(propertyDetailsAttrRailwayLabel);
            AssertTrueIsDisplayed(propertyDetailsRailwaySelect);
            AssertTrueIsDisplayed(propertyDetailsAttrLandParcelLabel);
            AssertTrueIsDisplayed(propertyDetailsLandTypeSelect);
            AssertTrueIsDisplayed(propertyDetailsAttrMunicipalLabel);
            AssertTrueIsDisplayed(propertyDetailsMunicipalZoneInput);
            AssertTrueIsDisplayed(propertyDetailsAttrAnomaliesLabel);
            AssertTrueIsDisplayed(propertyDetailsAnomaliesInput);

            AssertTrueIsDisplayed(propertyDetailsTenureTitle);
            AssertTrueIsDisplayed(propertyDetailsTenureStatusLabel);
            AssertTrueIsDisplayed(propertyDetailsTenureStatusInput);
            AssertTrueIsDisplayed(propertyDetailsPublicHwyLabel);
            AssertTrueIsDisplayed(propertyDetailsProvPublicHwy);

            if (webDriver.FindElements(propertyDetailsHighwayRoadEstablishLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsHighwayRoadEstablishLabel);
                AssertTrueIsDisplayed(propertyDetailsRoadEstablishInput);
            }

            if (webDriver.FindElements(propertyDetailsFirstNationTitle).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsFirstNationTitle);
                AssertTrueIsDisplayed(propertyDetailsFirstNationBandNameLabel);
                AssertTrueIsDisplayed(propertyDetailsFirstNationBandNameDiv);
                AssertTrueIsDisplayed(propertyDetailsFirstNationReserveLabel);
                AssertTrueIsDisplayed(propertyDetailsFirstNationReserveDiv);
            }

            AssertTrueIsDisplayed(propertyDetailsMeasurementsTitle);
            AssertTrueIsDisplayed(propertyDetailsMeasurementsAreaLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaSqMtsLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaSqMtsInput);
            AssertTrueIsDisplayed(propertyDetailsAreaHtsLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaHctInput);
            AssertTrueIsDisplayed(propertyDetailsAreaSqFeetLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaSqFtInput);
            AssertTrueIsDisplayed(propertyDetailsAreaSqFtInput);
            AssertTrueIsDisplayed(propertyDetailsAreaAcresLabel);
            AssertTrueIsDisplayed(propertyDetailsAreaAcrInput);

            AssertTrueIsDisplayed(propertyDetailsMeasurementVolumeParcelLabel);
            AssertTrueIsDisplayed(propertyDetailsIsVolumeRadioYes);
            AssertTrueIsDisplayed(propertyDetailsIsVolumeRadioNo);

            if (webDriver.FindElements(propertyDetailsMeasurementVolumeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsMeasurementVolumeLabel);
                AssertTrueIsDisplayed(propertyDetailsMeasurementTypeLabel);
                AssertTrueIsDisplayed(propertyDetailsVolTypeSelect);
                AssertTrueIsDisplayed(propertyDetailsAreaMtsCubeLabel);
                AssertTrueIsDisplayed(propertyDetailsVolCubeMtsInput);
                AssertTrueIsDisplayed(propertyDetailsAreaFeetCubeLabel);
                AssertTrueIsDisplayed(propertyDetailsVolCubeFeetInput);
            }

            AssertTrueIsDisplayed(propertyDetailsEditNotesTitle);
            AssertTrueIsDisplayed(propertyDetailsNotesTextarea);
        }

        public void VerifyNonInventoryPropertyTabs()
        {
            WaitUntilVisible(propertyInformationTitleTab);
            AssertTrueIsDisplayed(propertyInformationTitleTab);
            AssertTrueIsDisplayed(propertyInformationValueTab);
        }

        public int PropertyTabs()
        {
            Wait(5000);
            return webDriver.FindElements(propertyInformationTabsTotal).Count();
        }

        public void DeleteFirstHistoricalFile()
        {
            WaitUntilClickable(propertyAttributesHistoricalFile1stDeleteButton);
            webDriver.FindElement(propertyAttributesHistoricalFile1stDeleteButton).Click();
        }

        private void AddHistoricalFile(HistoricalFile historicalFile)
        {
            WaitUntilClickable(propertyAttrAddHistoricalFileButton);
            FocusAndClick(propertyAttrAddHistoricalFileButton);

            Wait();
            var historicalFileIndex = webDriver.FindElements(propertyAttrHistoricalFilesTotalCount).Count() -1;

            WaitUntilVisible(By.Id("input-historicalNumbers."+ historicalFileIndex +".historicalNumber"));
            webDriver.FindElement(By.Id("input-historicalNumbers."+ historicalFileIndex +".historicalNumber")).SendKeys(historicalFile.HistoricalFileNumber);

            ChooseSpecificSelectOption(By.Id("input-historicalNumbers."+ historicalFileIndex +".historicalNumberType"), historicalFile.HistoricalFileType);

            if(historicalFile.HistoricalFileOtherDetails != "")
                webDriver.FindElement(By.Id("input-historicalNumbers."+ historicalFileIndex +".otherHistoricalNumberType")).SendKeys(historicalFile.HistoricalFileOtherDetails);

            Wait();
        }
    }
}
