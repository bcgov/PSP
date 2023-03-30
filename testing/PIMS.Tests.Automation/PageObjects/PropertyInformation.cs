using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System.Diagnostics;

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

        //Property Details Elements
        private By propertyDetailsTab = By.XPath("//a[contains(text(),'Property Details')]");
        private By propertyDetailsEditBttn = By.CssSelector("div[role='tabpanel']:nth-child(3) div div button[title='Edit research file']");
        private By propertyDetailsResearchEditBttn = By.CssSelector("div[role='tabpanel']:nth-child(4) div div button[title='Edit research file']");

        private By propertyDetailsAddressTitle = By.XPath("//div[@class='tab-content']/div[@role='tabpanel'][3]/div/div[2]/h2/div/div[contains(text(),'Property Address')]");
        private By propertyDetailsResearchTitle = By.XPath("//div[@class='tab-content']/div[@role='tabpanel'][4]/div/div[2]/h2/div/div[contains(text(),'Property Address')]");
        private By propertyDetailsEditAddressTitle = By.XPath("//div[contains(text(),'Property Address')]");
        private By propertyDetailsAddressNotAvailable = By.XPath("//b[contains(text(),'Property address not available')]");
        private By propertyDetailsAddressLabel = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div/label");
        private By propertyDetailsAddressContent = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div[2]/h2/div/div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div[1]/div[2]");
        private By propertyDetailsAddressLine1Label = By.XPath("//label[contains(text(),'Address (line 1)')]");
        private By propertyDetailsAddressLine1Content = By.XPath("//label[contains(text(),'Address')]/parent::div/following-sibling::div");
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
        private By propertyDetailsAttrLandParcelLabel = By.XPath("//label[contains(text(),'Land parcel type')]");
        private By propertyDetailsAttrLandParcelDiv = By.XPath("//label[contains(text(),'Land parcel type')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrMunicipalLabel = By.XPath("//label[contains(text(),'Municipal zoning')]");
        private By propertyDetailsAttrMunicipalDiv = By.XPath("//label[contains(text(),'Municipal zoning')]/parent::div/following-sibling::div");
        private By propertyDetailsAttrAnomaliesLabel = By.XPath("//label[contains(text(),'Anomalies')]");
        private By propertyDetailsAttrAnomaliesDiv = By.XPath("//label[contains(text(),'Anomalies')]/parent::div/following-sibling::div");
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

        private By propertyDetailsViewNotesTitle = By.XPath("//div[@role='tabpanel'][3]/div/div/h2/div/div[contains(text(),'Notes')]");
        private By propertyDetailsEditNotesTitle = By.XPath("//h2/div/div[contains(text(),'Notes')]");
        private By propertyDetailsResearchNotesTitle = By.XPath("//div[@role='tabpanel'][4]/div/div[6]/h2/div/div[contains(text(),'Notes')]");
        
        private By propertyDetailsAddressAddLineBttn = By.XPath("//div[contains(text(),'Add an address line')]/parent::button");
        private By propertyDetailsAddressLine1Input = By.Id("input-address.streetAddress1");
        private By propertyDetailsAddressLine2Input = By.Id("input-address.streetAddress2");
        private By propertyDetailsAddressCityInput = By.Id("input-address.municipality");
        private By propertyDetailsPostalCodeInput = By.Id("input-address.postal");

        private By propertyDetailsMotiRegionSelect = By.Id("input-regionTypeCode");
        private By propertyDetailsHighwayDistrictSelect = By.Id("input-districtTypeCode");
        private By propertyDetailsRailwaySelect = By.Id("input-isRwyBeltDomPatent");
        private By propertyDetailsLandTypeSelect = By.Id("input-propertyTypeCode");
        private By propertyDetailsMunicipalZoneInput = By.Id("input-municipalZoning");
        private By propertyDetailsAnomaliesInput = By.Id("multiselect-anomalies_input");
        private By propertyDetailsAnomaliesOptions = By.XPath("//input[@id='multiselect-anomalies_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsTenureStatusInput = By.Id("multiselect-tenures_input");
        private By propertyDetailsTenureOptions = By.XPath("//input[@id='multiselect-tenures_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsProvPublicHwy = By.Id("input-pphStatusTypeCode");
        private By propertyDetailsRoadEstablishInput = By.Id("multiselect-roadTypes_input");
        private By propertyDetailsRoadEstablishOptions = By.XPath("//input[@id='multiselect-roadTypes_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By propertyDetailsAdjacentLandInput = By.Id("multiselect-adjacentLands_input");
        private By propertyDetailsAdjacentLandOptions = By.XPath("//input[@id='multiselect-adjacentLands_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
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

        private By propertyDetailsCancelContentModal1 = By.CssSelector("div[class='modal-body'] div");
        private By propertyDetailsCancelContentModal2 = By.CssSelector("div[class='modal-body'] strong");

        //PIMS Files Elements
        private By propertyPimsFilesLinkTab = By.XPath("//a[contains(text(),'PIMS Files')]");
        private By propertyPimsFiles = By.XPath("//div[contains(text(),'This property is associated with the following files.')]");

        private By propertyResearchFileSubtitle = By.XPath("//div[contains(text(),'Research Files')]");
        private By propertyResearchCountLabel = By.XPath("//div[contains(text(),'Research Files')]/following-sibling::div[@class='my-1 col-auto']");
        private By propertyResearchTable = By.XPath("//div[contains(text(),'Research Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyResearchExpandTableBttn = By.XPath("//div[contains(text(),'Research Files')]/parent::div/parent::div/following-sibling::div/*[1]");

        private By propertyAcquisitionFileSubtitle = By.XPath("//div[contains(text(),'Acquisition Files')]");
        private By propertyAcquisitionCountLabel = By.XPath("//div[contains(text(),'Acquisition Files')]/following-sibling::div[@class='my-1 col-auto']");
        private By propertyAcquisitionTable = By.XPath("//div[contains(text(),'Acquisition Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyAcquisitionExpandTableBttn = By.XPath("//div[contains(text(),'Acquisition Files')]/parent::div/parent::div/following-sibling::div/*[1]");

        private By propertyLeasesSubtitle = By.XPath("//div[contains(text(),'Leases/Licenses')]");
        private By propertyLeaseCountLabel = By.XPath("//div[contains(text(),'Leases/Licenses')]/following-sibling::div[@class='my-1 col-auto']");
        private By propertyLeaseTable = By.XPath("//div[contains(text(),'Leases/Licenses')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyLeaseExpandTableBttn = By.XPath("//div[contains(text(),'Leases/Licenses')]/parent::div/parent::div/following-sibling::div/*[1]");

        private By propertyDispositionFileSubtitle = By.XPath("//div[contains(text(),'Disposition Files')]");
        private By propertyDispositionCountLabel = By.XPath("//div[contains(text(),'Disposition Files')]/following-sibling::div[@class='my-1 col-auto']");

        private SharedModals sharedModals;


        public PropertyInformation(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void ClosePropertyInfoModal()
        {
            Wait();
            webDriver.FindElement(propertyCloseWindowBttn).Click();
        }

        public void CloseLTSAPopUp()
        {
            Wait(3000);
            webDriver.FindElement(propertyLeafletCloseLink).Click();
        }

        public void OpenMoreOptionsPopUp()
        {
            Wait();
            webDriver.FindElement(propertyLeafletEllipsisBttn).Click();
        }

        public void ChooseCreationOptionFromPin(string option)
        {
            Wait(5000);
            ButtonElement(option);
        }

        public void NavigatePropertyDetailsTab()
        {
            Wait();
            webDriver.FindElement(propertyDetailsTab).Click();
        }

        public void EditPropertyInfoBttn()
        {
            Wait();
            webDriver.FindElement(propertyDetailsEditBttn).Click();
        }

        public void EditPropertyInfoResearchBttn()
        {
            Wait();
            webDriver.FindElement(propertyDetailsResearchEditBttn).Click();
        }

        public void SavePropertyDetails()
        {
            Wait();
            ButtonElement("Save");

            sharedModals.SiteMinderModal();
        }

        public void CancelPropertyDetails()
        {
            Wait();
            ButtonElement("Cancel");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(webDriver.FindElement(propertyDetailsCancelContentModal1).Text.Equals("If you cancel now, this property information will not be saved."));
            Assert.True(webDriver.FindElement(propertyDetailsCancelContentModal2).Text.Equals("Are you sure you want to Cancel?"));

            sharedModals.ModalClickOKBttn();
        }

        public void UpdateMinPropertyDetails(string notes)
        {
            Wait();
            ClearInput(propertyDetailsNotesTextarea);
            webDriver.FindElement(propertyDetailsNotesTextarea).SendKeys(notes);
        }

        public void UpdateMaxPropertyDetails(string addressLine1, string addressLine2, string city, string postalCode, string municipalZoning, string sqMts, string cubeMts, string notes)
        {

            Wait(5000);
            ClearInput(propertyDetailsAddressLine1Input);
            webDriver.FindElement(propertyDetailsAddressLine1Input).SendKeys(addressLine1);

            Wait();
            if (webDriver.FindElements(propertyDetailsAddressLine2Input).Count > 1)
            {
                ClearInput(propertyDetailsAddressLine2Input);
            }
            webDriver.FindElement(propertyDetailsAddressAddLineBttn).Click();
            webDriver.FindElement(propertyDetailsAddressLine2Input).SendKeys(addressLine2);

            ClearInput(propertyDetailsAddressCityInput);
            webDriver.FindElement(propertyDetailsAddressCityInput).SendKeys(city);
            ClearInput(propertyDetailsPostalCodeInput);
            webDriver.FindElement(propertyDetailsPostalCodeInput).SendKeys(postalCode);

            ChooseRandomSelectOption(propertyDetailsMotiRegionSelect, 1);
            ChooseRandomSelectOption(propertyDetailsHighwayDistrictSelect, 1);
            ChooseRandomSelectOption(propertyDetailsRailwaySelect, 1);
            ChooseRandomSelectOption(propertyDetailsLandTypeSelect, 1);

            ClearInput(propertyDetailsMunicipalZoneInput);
            webDriver.FindElement(propertyDetailsMunicipalZoneInput).SendKeys(municipalZoning);

            webDriver.FindElement(propertyDetailsAnomaliesInput).Click();
            ChooseMultiSelectRandomOptions(propertyDetailsAnomaliesOptions, 1);
            webDriver.FindElement(propertyDetailsAttrAnomaliesLabel).Click();

            FocusAndClick(propertyDetailsTenureStatusInput);
            ChooseMultiSelectRandomOptions(propertyDetailsTenureOptions, 1);
            webDriver.FindElement(propertyDetailsTenureStatusLabel).Click();
            ChooseRandomSelectOption(propertyDetailsProvPublicHwy, 1);

            if (webDriver.FindElements(propertyDetailsRoadEstablishInput).Count() > 0)
            {
                webDriver.FindElement(propertyDetailsRoadEstablishInput).Click();
                ChooseMultiSelectRandomOptions(propertyDetailsRoadEstablishOptions, 3);
                webDriver.FindElement(propertyDetailsHighwayRoadEstablishLabel).Click();
            }

            if (webDriver.FindElements(propertyDetailsAdjacentLandInput).Count() > 0)
            {
                webDriver.FindElement(propertyDetailsAdjacentLandInput).Click();
                ChooseMultiSelectRandomOptions(propertyDetailsAdjacentLandOptions, 1);
                webDriver.FindElement(propertyDetailsAdjacentLandTypeLabel).Click();
            }

            ClearInput(propertyDetailsAreaSqMtsInput);
            webDriver.FindElement(propertyDetailsAreaSqMtsInput).SendKeys(sqMts);
            FocusAndClick(propertyDetailsIsVolumeRadioYes);

            Wait();
            ClearInput(propertyDetailsVolCubeMtsInput);
            webDriver.FindElement(propertyDetailsVolCubeMtsInput).SendKeys(cubeMts);
            ChooseRandomSelectOption(propertyDetailsVolTypeSelect, 1);
            ClearInput(propertyDetailsNotesTextarea);
            webDriver.FindElement(propertyDetailsNotesTextarea).SendKeys(notes);
        }

        public void VerifyPropertyMapPopUpView()
        {
            Wait(10000);
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

        public void VerifyPropertyInformationHeader()
        {
            Wait(5000);
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

        public void VerifyPropertyDetailsView(string feature)
        {
            Wait();

            if (feature == "Research File")
            {
                WaitUntil(propertyDetailsResearchEditBttn);
                Assert.True(webDriver.FindElement(propertyDetailsResearchTitle).Displayed);
            } else
            {
                WaitUntil(propertyDetailsEditBttn);
                Assert.True(webDriver.FindElement(propertyDetailsAddressTitle).Displayed);
            }

            
            if (webDriver.FindElements(propertyDetailsAddressNotAvailable).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsAddressNotAvailable).Displayed);
            }
            else
            {
                Assert.True(webDriver.FindElement(propertyDetailsAddressLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAddressContent).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsCityLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsCityContent).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsProvinceLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsProvinceContent).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsPostalCodeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsPostalCodeContent).Displayed);
            }

            Assert.True(webDriver.FindElement(propertyDetailsAttributesTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrRegionDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrHighwayLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrHighwayDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrElectoralLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrElectoralDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAgriLandLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAgriLandDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrRailwayLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrRailwayDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrLandParcelLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrLandParcelDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrMunicipalLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrMunicipalDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAnomaliesLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAnomaliesDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrCoordinatesLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrCoordinatesDiv).Displayed);

            Assert.True(webDriver.FindElement(propertyDetailsTenureTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsTenureStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsTenureStatusDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsPublicHwyLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsPublicHwyDiv).Displayed);

            if (webDriver.FindElements(propertyDetailsHighwayRoadEstablishLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsHighwayRoadEstablishLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsHighwayRoadEstablisDiv).Displayed);
            }

            if (webDriver.FindElements(propertyDetailsAdjacentLandTypeLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsAdjacentLandTypeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAdjacentLandTypeDiv).Displayed);
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

            if(webDriver.FindElements(propertyDetailsMeasurementVolumeLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsMeasurementVolumeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsMeasurementTypeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAreaMtsCubeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAreaFeetCubeLabel).Displayed);
            }

            if (feature == "Research File")
            {
                Assert.True(webDriver.FindElement(propertyDetailsResearchNotesTitle).Displayed);
            }
            else
            {
                Assert.True(webDriver.FindElement(propertyDetailsViewNotesTitle).Displayed);
            }
        }

        public void VerifyPropertyDetailsEditForm(string feature)
        {
            Wait();

            Assert.True(webDriver.FindElement(propertyDetailsEditAddressTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAddressLine1Label).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAddressLine1Input).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAddressAddLineBttn).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsEditCityLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAddressCityInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsEditPostalCodeLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsPostalCodeInput).Displayed);

            Assert.True(webDriver.FindElement(propertyDetailsAttributesTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsMotiRegionSelect).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrHighwayLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsHighwayDistrictSelect).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrElectoralLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrElectoralDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAgriLandLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAgriLandDiv).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrRailwayLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsRailwaySelect).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrLandParcelLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsLandTypeSelect).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrMunicipalLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsMunicipalZoneInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAttrAnomaliesLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAnomaliesInput).Displayed);

            Assert.True(webDriver.FindElement(propertyDetailsTenureTitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsTenureStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsTenureStatusInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsPublicHwyLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsProvPublicHwy).Displayed);
       
            if (webDriver.FindElements(propertyDetailsHighwayRoadEstablishLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsHighwayRoadEstablishLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsRoadEstablishInput).Displayed);
            }

            if (webDriver.FindElements(propertyDetailsAdjacentLandTypeLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsAdjacentLandTypeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAdjacentLandInput).Displayed);
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
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqMtsLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqMtsInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaHtsLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaHctInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqFeetLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqFtInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaSqFtInput).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaAcresLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsAreaAcrInput).Displayed);

            Assert.True(webDriver.FindElement(propertyDetailsMeasurementVolumeParcelLabel).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsIsVolumeRadioYes).Displayed);
            Assert.True(webDriver.FindElement(propertyDetailsIsVolumeRadioNo).Displayed); 

            if (webDriver.FindElements(propertyDetailsMeasurementVolumeLabel).Count() > 0)
            {
                Assert.True(webDriver.FindElement(propertyDetailsMeasurementVolumeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsMeasurementTypeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsVolTypeSelect).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAreaMtsCubeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsVolCubeMtsInput).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsAreaFeetCubeLabel).Displayed);
                Assert.True(webDriver.FindElement(propertyDetailsVolCubeFeetInput).Displayed);
                
            }

            if (feature == "Property Information")
            {
                Assert.True(webDriver.FindElement(propertyDetailsEditNotesTitle).Displayed);
            }
            else
            {
                Assert.True(webDriver.FindElement(propertyDetailsResearchNotesTitle).Displayed);
            }
            
            Assert.True(webDriver.FindElement(propertyDetailsNotesTextarea).Displayed);
        }

        public void VerifyPimsFiles()
        {
            Wait();

            webDriver.FindElement(propertyPimsFilesLinkTab).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyPimsFiles).Displayed);
                
            Assert.True(webDriver.FindElement(propertyResearchFileSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyResearchCountLabel).Displayed);
            webDriver.FindElement(propertyResearchExpandTableBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyResearchTable).Displayed);

            Assert.True(webDriver.FindElement(propertyAcquisitionFileSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyAcquisitionCountLabel).Displayed);
            webDriver.FindElement(propertyAcquisitionExpandTableBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyAcquisitionTable).Displayed);

            Assert.True(webDriver.FindElement(propertyLeasesSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyLeaseCountLabel).Displayed);
            webDriver.FindElement(propertyLeaseExpandTableBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyLeaseTable).Displayed);

            Assert.True(webDriver.FindElement(propertyDispositionFileSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDispositionCountLabel).Displayed); 
        }

        public void VerifyNonInventoryPropertyTabs()
        {
            Wait();
            Assert.True(webDriver.FindElement(propertyInformationTitleTab).Displayed);
            Assert.True(webDriver.FindElement(propertyInformationValueTab).Displayed);

        }

        public int PropertyTabs()
        {
            Wait();
            return webDriver.FindElements(propertyInformationTabsTotal).Count();
        }

        private void FiringDriver_FindingElement(object? sender, FindElementEventArgs e)
        {
            Debug.WriteLine("TESTING LISTENERS");
        }
    }
}
