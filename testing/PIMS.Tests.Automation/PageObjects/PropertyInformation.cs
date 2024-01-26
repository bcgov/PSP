using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using PIMS.Tests.Automation.Classes;
using System.Diagnostics;
using System.Threading.Channels;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyInformation : PageObjectBase
    {
        //Map Property LTSA ParcelMap Data Pop-Up Elements
        private By propertyLeafletCloseLink = By.CssSelector("a[class='leaflet-popup-close-button']");
        private By propertyLeafletTitle = By.XPath("//h5[contains(text(),'LTSA ParcelMap data')]");
        private By propertyLeafletPIDLabel = By.XPath("//b[contains(text(),'Parcel PID:')]");
        private By propertyLeafletPINLabel = By.XPath("//b[contains(text(),'Parcel PIN:')]");
        private By propertyLeafletPlanNbrLabel = By.XPath("//b[contains(text(),'Plan number:')]");
        private By propertyLeafletOwnerTypeLabel = By.XPath("//b[contains(text(),'Owner type:')]");
        private By propertyLeafletMunicipalityLabel = By.XPath("//b[contains(text(),'Municipality:')]");
        private By propertyLeafletAreaLabel = By.XPath("//b[contains(text(),'Area:')]");
        private By propertyLeafletZoomMapZoomBttn = By.XPath("//button/div[contains(text(),'Zoom map')]");
        private By propertyLeafletEllipsisBttn = By.CssSelector("button[data-testid='fly-out-ellipsis']");

        private By propertyCloseWindowBttn = By.XPath("//div[@class='col']/h1[contains(text(),'Property Information')]/parent::div/following-sibling::div");
        private By propertyMoreOptionsMenu = By.CssSelector("div[class='list-group list-group-flush']");
        private By propertyViewInfoBttn = By.XPath("//button/div[contains(text(),'View more property info')]");
        private By propertyNewResearchFileBttn = By.XPath("//button/div[contains(text(),'Research File - Create new')]");
        private By propertyNewAcquisitionFileBttn = By.XPath("//button/div[contains(text(),'Acquisition File - Create new')]");
        private By propertyNewLeaseFileBttn = By.XPath("//button/div[contains(text(),'Lease/License - Create new')]");

        //Property Information Tabs Elements
        private By propertyInformationTabsTotal = By.CssSelector("nav[role='tablist'] a");
        private By propertyInformationTitleTab = By.XPath("//a[contains(text(),'Title')]");
        private By propertyInformationValueTab = By.XPath("//a[contains(text(),'Value')]");

        //Property Information Header Elements
        private By propertyInformationHeaderTitle = By.XPath("//div[@class='col']/h1[contains(text(),'Property Information')]");
        private By propertyInformationHeaderAddressLabel = By.XPath("//label[contains(text(),'Civic Address')]");
        private By propertyInformationHeaderAddressContent = By.XPath("//label[contains(text(),'Civic Address')]/parent::div/following-sibling::div");
        private By propertyInformationHeaderPlanLabel = By.XPath("//label[contains(text(),'Plan')]");
        private By propertyInformationHeaderPlanContent = By.XPath("//label[contains(text(),'Plan')]/parent::div/following-sibling::div");
        private By propertyInformationHeaderPIDLabel = By.XPath("//label[contains(text(),'PID')]");
        private By propertyInformationHeaderPIDContent = By.XPath("//label[contains(text(),'PID')]/parent::div/following-sibling::div");
        private By propertyInformationHeaderLandTypeLabel = By.XPath("//div[@class='no-gutters row']/div[@class='col']/div/div//label[contains(text(),'Land parcel type')]");
        private By propertyInformationHeaderLandTypeContent = By.XPath("//div[@class='no-gutters row']/div[@class='col']/div/div//label[contains(text(),'Land parcel type')]/parent::div/following-sibling::div");
        private By propertyInformationHeaderZoomBttn = By.CssSelector("button[title='Zoom Map']");

        //Title Tab Elements
        private By propertyTitleInfo = By.XPath("//div[contains(text(),'This data was retrieved from LTSA')]");
        private By propertyTitleDetailsTitle = By.XPath("//div[contains(text(),'Title Details')]");
        private By propertyTitleNumberLabel = By.XPath("//label[contains(text(),'Title number')]");
        private By propertyTitleLandTitleLabel = By.XPath("//label[contains(text(),'Land title district')]");
        private By propertyTitleTaxationAuthoritiesLabel = By.XPath("//label[contains(text(),'Taxation authorities')]");

        private By propertyTitleLandTitle = By.XPath("//h2/div/div[contains(text(),'Land')]");
        private By propertyTitlePIDLabel = By.XPath("//div[contains(text(),'Land')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PID')]");
        private By propertyTitleLandDescriptionLabel = By.XPath("//label[contains(text(),'Legal description')]");

        private By propertyOwnershipInformationTitle = By.XPath("//h2/div/div[contains(text(),'Ownership Information')]");
        private By propertyFractionalOwnershipLabel = By.XPath("//label[contains(text(),'Fractional ownership')]");
        private By propertyJointTenancyLabel = By.XPath("//label[contains(text(),'Joint tenancy')]");
        private By propertyOwnershipRemarksLabel = By.XPath("//label[contains(text(),'Ownership remarks')]");
        private By propertyOwnerNameLabel = By.XPath("//label[contains(text(),'Owner name')]");
        private By propertyIncorporationNbrLabel = By.XPath("//label[contains(text(),'Incorporation number')]");
        private By propertyOccupationLabel = By.XPath("//label[contains(text(),'Occupation')]");
        private By propertyAddressLabel = By.XPath("//div[contains(text(),'Ownership Information')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Address')]");

        private By propertyChargesLienInterestsTitle = By.XPath("//h2/div/div[contains(text(),'Charges, Liens and Interests')]");
        private By propertyNatureLabel = By.XPath("//label[contains(text(),'Nature')]");
        private By propertyRegistrationLabel = By.XPath("//label[contains(text(),'Registration #')]");
        private By propertyRegisteredDateLabel = By.XPath("//label[contains(text(),'Registered date')]");
        private By propertyRegisteredOwnerLabel = By.XPath("//label[contains(text(),'Registered owner')]");

        private By propertyDuplicateIndefeasibleTitle = By.XPath("//h2/div/div[contains(text(),'Duplicate Indefeasible Title')]");
        private By propertyDuplicateIndefeasibleNoneContent = By.XPath("//div[contains(text(),'Duplicate Indefeasible Title')]/parent::div/parent::h2/following-sibling::div[contains(text(),'None')]");

        private By propertyTransfersTitle = By.XPath("//h2/div/div[contains(text(),'Transfers')]");
        private By propertyTransfersNoneContent = By.XPath("//div[contains(text(),'Transfers')]/parent::div/parent::h2/following-sibling::div[contains(text(),'None')]");

        private By propertyNotesTitle = By.XPath("//h2/div/div[contains(text(),'Transfers')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Notes')]");
        private By propertyMiscellaneousNotesLabel = By.XPath("//label[contains(text(),'Miscellaneous notes')]");
        private By propertyParcelStatusLabel = By.XPath("//label[contains(text(),'Parcel status')]");

        //Property Value Elements
        private By propertyValueInfo = By.XPath("//div[contains(text(),'This data was retrieved from BC Assessment on')]");

        private By propertyAssessmentOverviewTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Overview')]");
        private By propertyAssessmentPIDLabel = By.XPath("//div[contains(text(),'Assessment Overview')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PID')]");
        private By propertyAssessmentJurisdictionLabel = By.XPath("//label[contains(text(),'Jurisdiction')]");
        private By propertyAssessmentNeighbourhoodLabel = By.XPath("//label[contains(text(),'Neighbourhood')]");
        private By propertyAssessmentOwnershipYearLabel = By.XPath("//label[contains(text(),'Ownership year')]");
        private By propertyAssessmentRollNumberLabel = By.XPath("//label[contains(text(),'Roll number')]");
        private By propertyAssessmentRollYearLabel = By.XPath("//label[contains(text(),'Roll year')]");
        private By propertyAssessmentDocumentNumberLabel = By.XPath("//label[contains(text(),'Document number')]");

        private By propertyAssessmentAddressTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Overview')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Property Address')]");
        private By propertyValueAddressInfo = By.XPath("//div/p[contains(text(),'This is the property address as per BC Assessment (for reference)')]");
        private By propertyValueAddressLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'Address'))]");
        private By propertyValueCityLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'City'))]");
        private By propertyValueProvinceLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'Province'))]");
        private By propertyValuePostalCodeLabel = By.XPath("//p[contains(text(),'This is the property address as per BC Assessment (for reference)')]/following-sibling::div/div/label[(contains(text(),'Postal code'))]");

        private By propertyAssessedValueTitle = By.XPath("//h2/div/div[contains(text(),'Assessed Value')]");
        private By propertyAssessedTable = By.CssSelector("div[data-testid='Assessed Values Sales']");

        private By propertyAssessmentDetailsTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Details')]");
        private By propertyAssessmentManualClassLabel = By.XPath("//label[contains(text(),'Manual class')]");
        private By propertyAssessmentActualUseLabel = By.XPath("//label[contains(text(),'Actual use')]");
        private By propertyAssessmentALRLabel = By.XPath("//label[contains(text(),'ALR')]");
        private By propertyAssessmentLandDimensionLabel = By.XPath("//label[contains(text(),'Land dimension')]");

        private By propertyAssessmentSalesTitle = By.XPath("//h2/div/div[contains(text(),'Assessment Details')]");
        private By propertySalesDescription = By.XPath("//div[contains(text(),'Description')]");


        //Property Details Elements
        private By propertyDetailsTab = By.XPath("//a[contains(text(),'Property Details')]");
        private By propertyDetailsEditBttn = By.CssSelector("button[title='Edit property details']");

        private By propertyDetailsAddressTitle = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/parent::div/preceding-sibling::div/h2/div/div[contains(text(),'Property Address')]");
        private By propertyDetailsEditAddressTitle = By.XPath("//div[contains(text(),'Property Address')]");
        private By propertyDetailsAddressNotAvailable = By.XPath("//b[contains(text(),'Property address not available')]");
        private By propertyDetailsAddressLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div/label");
        private By propertyDetailsAddressContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]");
        private By propertyDetailsAddressContentDetails = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]");
        private By propertyDetailsAddressLine1Label = By.XPath("//label[contains(text(),'Address (line 1)')]");

        private By propertyDetailsCityLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[2]/div/label[contains(text(),'City')]");
        private By propertyDetailsCityContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[2]/div/label[contains(text(),'City')]/parent::div/following-sibling::div");
        private By propertyDetailsEditCityLabel = By.XPath("//Label[contains(text(),'City')]");
        private By propertyDetailsProvinceLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[3]/div/label[contains(text(),'Province')]");
        private By propertyDetailsProvinceContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[3]/div/label[contains(text(),'Province')]/parent::div/following-sibling::div");
        private By propertyDetailsPostalCodeLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[4]/div/label[contains(text(),'Postal code')]");
        private By propertyDetailsPostalCodeContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/div/div[4]/div/label[contains(text(),'Postal code')]/parent::div/following-sibling::div");
        private By propertyDetailsEditPostalCodeLabel = By.XPath("//Label[contains(text(),'Postal code')]");

        private By propertyDetailsAttributesTitle = By.XPath("//div[contains(text(),'Property Attributes')]");
        private By propertyDetailsAttrRegionLabel = By.XPath("//label[contains(text(),'MOTI region')]");
        private By propertyDetailsAttrRegionDiv = By.XPath("//label[contains(text(),'MOTI region')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrHighwayLabel = By.XPath("//label[contains(text(),'Highways district')]");
        private By propertyDetailsAttrHighwayDiv = By.XPath("//label[contains(text(),'Highways district')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrElectoralLabel = By.XPath("//label[contains(text(),'Electoral district')]");
        private By propertyDetailsAttrElectoralDiv = By.XPath("//label[contains(text(),'Electoral district')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrAgriLandLabel = By.XPath("//label[contains(text(),'Agricultural Land Reserve')]");
        private By propertyDetailsAttrAgriLandDiv = By.XPath("//label[contains(text(),'Agricultural Land Reserve')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrRailwayLabel = By.XPath("//label[contains(text(),'Railway belt / Dominion patent')]");
        private By propertyDetailsAttrRailwayDiv = By.XPath("//label[contains(text(),'Railway belt / Dominion patent')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrLandParcelLabel = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Land parcel type')]");
        private By propertyDetailsAttrLandParcelDiv = By.XPath("//div[contains(text(),'Property Attributes')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Land parcel type')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrMunicipalLabel = By.XPath("//label[contains(text(),'Municipal zoning')]");
        private By propertyDetailsAttrMunicipalDiv = By.XPath("//label[contains(text(),'Municipal zoning')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrAnomaliesLabel = By.XPath("//label[contains(text(),'Anomalies')]");
        private By propertyDetailsAttrAnomaliesDiv = By.XPath("//label[contains(text(),'Anomalies')]/parent::div/following-sibling::div/div/div");
        private By propertyDetailsAttrCoordinatesLabel = By.XPath("//label[contains(text(),'Coordinates')]");
        private By propertyDetailsAttrCoordinatesDiv = By.XPath("//label[contains(text(),'Coordinates')]/parent::div/following-sibling::div");

        private By propertyDetailsTenureTitle = By.XPath("//div[contains(text(),'Tenure Status')]");
        private By propertyDetailsTenureStatusLabel = By.XPath("//label[contains(text(),'Tenure status')]");
        private By propertyDetailsTenureStatusDiv = By.XPath("//label[contains(text(),'Tenure status')]/parent::div/following-sibling::div");
        private By propertyDetailsPublicHwyLabel = By.XPath("//label[contains(text(),'Provincial Public Hwy')]");
        private By propertyDetailsPublicHwyDiv = By.XPath("//label[contains(text(),'Provincial Public Hwy')]/parent::div/following-sibling::div");
        private By propertyDetailsHighwayRoadEstablishLabel = By.XPath("//label[contains(text(),'Highway / Road established by')]");
        private By propertyDetailsHighwayRoadEstablisDiv = By.XPath("//label[contains(text(),'Highway / Road established by')]/parent::div/following-sibling::div");

        private By propertyDetailsAdjacentLandTypeLabel = By.XPath("//label[contains(text(),'Adjacent Land type')]");
        private By propertyDetailsAdjacentLandTypeDiv = By.XPath("//label[contains(text(),'Adjacent Land type')]/parent::div/following-sibling::div");

        private By propertyDetailsFirstNationTitle = By.XPath("//div[contains(text(),'First Nations Information')]");
        private By propertyDetailsFirstNationBandNameLabel = By.XPath("//label[contains(text(),'Band name')]");
        private By propertyDetailsFirstNationBandNameDiv = By.XPath("//label[contains(text(),'Band name')]/parent::div/following-sibling::div");
        private By propertyDetailsFirstNationReserveLabel = By.XPath("//label[contains(text(),'Reserve name')]");
        private By propertyDetailsFirstNationReserveDiv = By.XPath("//label[contains(text(),'Reserve name')]/parent::div/following-sibling::div");

        private By propertyDetailsMeasurementsTitle = By.XPath("//div[contains(text(),'Measurements')]");
        private By propertyDetailsMeasurementsAreaLabel = By.XPath("//label[contains(text(),'Area')]");
        private By propertyDetailsMeasurementVolumeParcelLabel = By.XPath("//label[contains(text(),'Is this a volumetric parcel?')]");
        private By propertyDetailsMeasurementVolumeLabel = By.XPath("//label[contains(text(),'Volume')]");
        private By propertyDetailsMeasurementTypeLabel = By.XPath("//label[contains(text(),'Type')]");
        private By propertyDetailsAreaSqMtsLabel = By.XPath("//div[contains(text(),'sq. metres')]");
        private By propertyDetailsAreaHtsLabel = By.XPath("//div[contains(text(),'hectares')]");
        private By propertyDetailsAreaSqFeetLabel = By.XPath("//div[contains(text(),'sq. feet')]");
        private By propertyDetailsAreaAcresLabel = By.XPath("//div[contains(text(),'acres')]");
        private By propertyDetailsAreaMtsCubeLabel = By.XPath("//span[contains(text(),'metres')]");
        private By propertyDetailsAreaFeetCubeLabel = By.XPath("//span[contains(text(),'feet')]");

        private By propertyDetailsViewNotesTitle = By.XPath("//div[contains(text(),'Measurements')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Notes')]");
        private By propertyDetailsEditNotesTitle = By.XPath("//h2/div/div[contains(text(),'Notes')]");

        private By propertyDetailsAddressAddLineBttn = By.XPath("//div[contains(text(),'Add an address line')]/parent::button");
        private By propertyDetailsAddressLine1Input = By.Id("input-address.streetAddress1");
        private By propertyDetailsAddressLine2Input = By.Id("input-address.streetAddress2");
        private By propertyDetailsAddressLine3Input = By.Id("input-address.streetAddress3");
        private By propertyDetailsAddressLineDeleteBttn = By.XPath("//*[@data-testid='remove-button']/parent::div/parent::button");
        private By propertyDetailsAddressCityInput = By.Id("input-address.municipality");
        private By propertyDetailsPostalCodeInput = By.Id("input-address.postal");

        private By propertyDetailsMotiRegionSelect = By.Id("input-regionTypeCode");
        private By propertyDetailsHighwayDistrictSelect = By.Id("input-districtTypeCode");
        private By propertyDetailsRailwaySelect = By.Id("input-isRwyBeltDomPatent");
        private By propertyDetailsLandTypeSelect = By.Id("input-propertyTypeCode");
        private By propertyDetailsMunicipalZoneInput = By.Id("input-municipalZoning");
        private By propertyDetailsAnomaliesInput = By.Id("multiselect-anomalies_input");
        private By propertyDetailsAnomaliesOptions = By.XPath("//input[@id='multiselect-anomalies_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsAttrAnomaliesDeleteBttns = By.CssSelector("div[id='multiselect-anomalies'] i[class='custom-close']");
        private By propertyDetailsTenureStatusInput = By.Id("multiselect-tenures_input");
        private By propertyDetailsTenureOptions = By.XPath("//input[@id='multiselect-tenures_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsTenureDeleteBttns = By.CssSelector("div[id='multiselect-tenures'] i[class='custom-close']");
        private By propertyDetailsProvPublicHwy = By.Id("input-pphStatusTypeCode");
        private By propertyDetailsRoadEstablishInput = By.Id("multiselect-roadTypes_input");
        private By propertyDetailsRoadEstablishOptions = By.XPath("//input[@id='multiselect-roadTypes_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsAdjacentLandInput = By.Id("multiselect-adjacentLands_input");
        private By propertyDetailsAdjacentLandOptions = By.XPath("//input[@id='multiselect-adjacentLands_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsAdjacentLandDeleteBttns = By.CssSelector("div[id='multiselect-adjacentLands'] i[class='custom-close']");
        private By propertyDetailsAreaSqMtsInput = By.Name("area-sq-meters");
        private By propertyDetailsAreaHctInput = By.Name("area-hectares");
        private By propertyDetailsAreaSqFtInput = By.Name("area-sq-feet");
        private By propertyDetailsAreaAcrInput = By.Name("area-acres");
        private By propertyDetailsIsVolumeRadioYes = By.Id("input-true");
        private By propertyDetailsIsVolumeRadioNo = By.Id("input-false");
        private By propertyDetailsVolCubeMtsInput = By.Name("volume-cubic-meters");
        private By propertyDetailsVolCubeFeetInput = By.Name("volume-cubic-feet");
        private By propertyDetailsVolTypeSelect = By.Id("input-volumetricParcelTypeCode");
        private By propertyDetailsNotesTextarea = By.Id("input-notes");

        //Property Information Confirmation Modal
        private By propertyInformationConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public PropertyInformation(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void ClosePropertyInfoModal()
        {
            WaitUntilSpinnerDisappear();

            WaitUntilVisible(propertyCloseWindowBttn);
            webDriver.FindElement(propertyCloseWindowBttn).Click();
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
                case "View more property info":
                    ButtonElement(propertyViewInfoBttn);
                    break;
                case "Research File - Create new":
                    ButtonElement(propertyNewResearchFileBttn);
                    break;
                case "Acquisition File - Create new":
                    ButtonElement(propertyNewAcquisitionFileBttn);
                    break;
                case "Lease/License - Create new":
                    ButtonElement(propertyNewLeaseFileBttn);
                    break;
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
            {
                //Assert.Equal("Confirm changes", sharedModals.ModalHeader());
                //Assert.Equal("If you choose to cancel now, your changes will not be saved.", sharedModals.ConfirmationModalText1());
                //Assert.Equal("Do you want to proceed?", sharedModals.ConfirmationModalText2());

                sharedModals.ModalClickOKBttn();
            }
        }

        public void UpdatePropertyDetails(Property property)
        {
            Wait(3000);

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
            if (property.MOTIRegion != "")
            {
                ChooseSpecificSelectOption(propertyDetailsMotiRegionSelect, property.MOTIRegion);
            }
            if (property.HighwaysDistrict != "")
            {
                ChooseSpecificSelectOption(propertyDetailsHighwayDistrictSelect, property.HighwaysDistrict);
            }
            if (property.RailwayBelt != "")
            {
                ChooseSpecificSelectOption(propertyDetailsRailwaySelect, property.RailwayBelt);
            }
            if (property.LandParcelType != "")
            {
                ChooseSpecificSelectOption(propertyDetailsLandTypeSelect, property.LandParcelType);
            }
            if (property.MunicipalZoning != "")
            {
                ClearInput(propertyDetailsMunicipalZoneInput);
                webDriver.FindElement(propertyDetailsMunicipalZoneInput).SendKeys(property.MunicipalZoning);
            }

            //Delete Annomalies previously selected if any
            if (webDriver.FindElements(propertyDetailsAttrAnomaliesDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(propertyDetailsAttrAnomaliesDeleteBttns).Count > 0)
                {
                    webDriver.FindElements(propertyDetailsAttrAnomaliesDeleteBttns)[0].Click();
                }
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
                    webDriver.FindElements(propertyDetailsTenureDeleteBttns)[0].Click();
                }
            }
            if (property.TenureStatus.First() != "")
            {
                foreach (string status in property.TenureStatus)
                {
                    FocusAndClick(propertyDetailsTenureStatusInput);

                    WaitUntilClickable(propertyDetailsTenureOptions);
                    ChooseMultiSelectSpecificOption(propertyDetailsTenureOptions, status);
                }
            }

            if (property.ProvincialPublicHwy != "")
            {
                ChooseSpecificSelectOption(propertyDetailsProvPublicHwy, property.ProvincialPublicHwy);
            }

            if (property.HighwayEstablishedBy.First() != "")
            {
                ClearMultiSelectInput(propertyDetailsRoadEstablishInput);
                foreach (string status in property.HighwayEstablishedBy)
                {
                    FocusAndClick(propertyDetailsRoadEstablishInput);
                    ChooseMultiSelectSpecificOption(propertyDetailsRoadEstablishOptions, status);
                }
            }

            //Delete Adjacent Land previously selected if any
            if (webDriver.FindElements(propertyDetailsAdjacentLandDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(propertyDetailsAdjacentLandDeleteBttns).Count > 0)
                {
                    webDriver.FindElements(propertyDetailsAdjacentLandDeleteBttns)[0].Click();
                }
            }
            //if (property.AdjacentLandType.First() != "")
            //{
            //    ClearMultiSelectInput(propertyDetailsAdjacentLandInput);
            //    foreach (string type in property.AdjacentLandType)
            //    {
            //        FocusAndClick(propertyDetailsAdjacentLandInput);
            //        ChooseMultiSelectSpecificOption(propertyDetailsAdjacentLandOptions, type);
            //    }               
            //}
            if (property.SqrMeters != "")
            {
                ClearDigitsInput(propertyDetailsAreaSqMtsInput);
                webDriver.FindElement(propertyDetailsAreaSqMtsInput).SendKeys(property.SqrMeters);
            }
            if (property.IsVolumetric)
            {
                FocusAndClick(propertyDetailsIsVolumeRadioYes);
            }
            else
            {
                FocusAndClick(propertyDetailsIsVolumeRadioNo);
            }
            if (property.Volume != "")
            {
                ClearInput(propertyDetailsVolCubeMtsInput);
                webDriver.FindElement(propertyDetailsVolCubeMtsInput).SendKeys(property.Volume);
            }
            if (property.VolumeType != "")
            {
                ChooseSpecificSelectOption(propertyDetailsVolTypeSelect, property.VolumeType);
            }
            if (property.PropertyNotes != "")
            {
                ClearInput(propertyDetailsNotesTextarea);
                webDriver.FindElement(propertyDetailsNotesTextarea).SendKeys(property.PropertyNotes);
            }
        }

        public void VerifyPropertyMapPopUpView()
        {
            WaitUntilVisible(propertyLeafletTitle);
            Assert.True(webDriver.FindElement(propertyLeafletCloseLink).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletPIDLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletPINLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletPlanNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletOwnerTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletMunicipalityLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletAreaLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletZoomMapZoomBttn).Displayed);
            Assert.True(webDriver.FindElement(propertyLeafletEllipsisBttn).Displayed);
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
            Wait(2000);

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

        public void VerifyPropertyInformationHeader()
        {
            Wait(2000);

            Assert.True(webDriver.FindElement(propertyInformationHeaderTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyInformationHeaderAddressLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyInformationHeaderAddressContent).Text != null);
            Assert.True(webDriver.FindElement(propertyInformationHeaderPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyInformationHeaderPlanContent).Text != null);
            Assert.True(webDriver.FindElement(propertyInformationHeaderPIDLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyInformationHeaderPIDContent).Text != null);
            Assert.True(webDriver.FindElement(propertyInformationHeaderLandTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyInformationHeaderLandTypeContent).Text != null);
            Assert.True(webDriver.FindElement(propertyInformationHeaderZoomBttn).Displayed);
        }

        public void VerifyPropertyDetailsView()
        {
            //WaitUntilDisappear(propertyDetailsWaitSpinner);
            Wait(3000);

            AssertTrueIsDisplayed(propertyDetailsAddressTitle);

            if (webDriver.FindElements(propertyDetailsAddressNotAvailable).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsAddressNotAvailable);
            }
            else
            {
                AssertTrueIsDisplayed(propertyDetailsAddressLabel);
                AssertTrueIsDisplayed(propertyDetailsAddressContent);
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
                AssertTrueIsDisplayed(propertyDetailsHighwayRoadEstablisDiv);
            }

            if (webDriver.FindElements(propertyDetailsAdjacentLandTypeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandTypeLabel);
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandTypeDiv);
            }

            if (webDriver.FindElements(propertyDetailsFirstNationTitle).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsFirstNationTitle).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsFirstNationBandNameLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsFirstNationBandNameDiv).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsFirstNationReserveLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsFirstNationReserveDiv).Displayed);
            }

            Assert.True(webDriver.FindElement(propertyDetailsMeasurementsTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsMeasurementsAreaLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsMeasurementVolumeParcelLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqMtsLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaHtsLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqFeetLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaAcresLabel).Displayed);

            if (webDriver.FindElements(propertyDetailsMeasurementVolumeLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsMeasurementVolumeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsMeasurementTypeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAreaMtsCubeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAreaFeetCubeLabel).Displayed);
            }
            Assert.True(webDriver.FindElement(propertyDetailsViewNotesTitle).Displayed);
        }

        public void VerifyUpdatePropertyDetailsView(Property property)
        {

            WaitUntilVisible(propertyDetailsEditBttn);
            AssertTrueIsDisplayed(propertyDetailsAddressTitle);

            if (webDriver.FindElements(propertyDetailsAddressNotAvailable).Count() > 0)
                AssertTrueIsDisplayed(propertyDetailsAddressNotAvailable);
            else
            {
                AssertTrueIsDisplayed(propertyDetailsAddressLabel);
                //To-Do: Address
                AssertTrueIsDisplayed(propertyDetailsCityLabel);
                AssertTrueContentEquals(propertyDetailsCityContent, property.Address.City);
                AssertTrueIsDisplayed(propertyDetailsProvinceLabel);
                AssertTrueContentEquals(propertyDetailsProvinceContent, property.Address.Province);
                AssertTrueIsDisplayed(propertyDetailsPostalCodeLabel);
                AssertTrueContentEquals(propertyDetailsPostalCodeContent, property.Address.PostalCode);
            }

            AssertTrueIsDisplayed(propertyDetailsAttributesTitle);
            AssertTrueIsDisplayed(propertyDetailsAttrRegionLabel);
            AssertTrueContentEquals(propertyDetailsAttrRegionDiv, property.MOTIRegion);
            AssertTrueIsDisplayed(propertyDetailsAttrHighwayLabel);

            var highWayDistrictText = webDriver.FindElement(propertyDetailsAttrHighwayDiv).Text;
            Assert.Equal(property.HighwaysDistrict, GetSubstring(highWayDistrictText, 2, highWayDistrictText.Length));

            AssertTrueIsDisplayed(propertyDetailsAttrElectoralLabel);
            AssertTrueContentEquals(propertyDetailsAttrElectoralDiv,property.ElectoralDistrict);
            AssertTrueIsDisplayed(propertyDetailsAttrAgriLandLabel);
            AssertTrueContentEquals(propertyDetailsAttrAgriLandDiv, property.AgriculturalLandReserve);
            AssertTrueIsDisplayed(propertyDetailsAttrRailwayLabel);
            AssertTrueContentEquals(propertyDetailsAttrRailwayDiv, property.RailwayBelt);
            AssertTrueIsDisplayed(propertyDetailsAttrLandParcelLabel);
            AssertTrueContentEquals(propertyDetailsAttrLandParcelDiv,property.LandParcelType);
            AssertTrueIsDisplayed(propertyDetailsAttrMunicipalLabel);
            AssertTrueContentEquals(propertyDetailsAttrMunicipalDiv,property.MunicipalZoning);
            AssertTrueIsDisplayed(propertyDetailsAttrAnomaliesLabel);

            if (property.Anomalies.First() != "")
            {
                var anomaliesUI = GetViewFieldListContent(propertyDetailsAttrAnomaliesDiv);
                Assert.True(Enumerable.SequenceEqual(anomaliesUI, property.Anomalies));
            }

            AssertTrueIsDisplayed(propertyDetailsAttrCoordinatesLabel);
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

                var highwayEstablishedUI = GetViewFieldListContent(propertyDetailsHighwayRoadEstablisDiv);
                Assert.True(Enumerable.SequenceEqual(highwayEstablishedUI, property.HighwayEstablishedBy));
            }

            if (webDriver.FindElements(propertyDetailsAdjacentLandTypeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandTypeLabel);

                var adjacentLandTypeUI = GetViewFieldListContent(propertyDetailsAdjacentLandTypeDiv);
                Assert.True(Enumerable.SequenceEqual(adjacentLandTypeUI, property.AdjacentLandType));
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

            if (webDriver.FindElements(propertyDetailsAdjacentLandTypeLabel).Count() > 0)
            {
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandTypeLabel);
                AssertTrueIsDisplayed(propertyDetailsAdjacentLandInput);
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
            Wait(2000);
            return webDriver.FindElements(propertyInformationTabsTotal).Count();
        }
    }
}
