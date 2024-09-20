using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class DigitalDocuments: PageObjectBase
    {
        //Documents Tab Element
        private readonly By documentsTab = By.CssSelector("a[data-rb-event-key='documents']");

        //Documents Tab List Header
        private readonly By documentsFileTitle = By.XPath("//div[contains(text(),'File Documents')]");
        private readonly By documentsTitle = By.XPath("//div[contains(text(),'Documents')]");
        private readonly By addFileDocumentBttn = By.XPath("//div[contains(text(),'File Documents')]/following-sibling::div/button");
        private readonly By addDocumentBttn = By.XPath("//div[contains(text(),'Documents')]/following-sibling::div/button");

        //Upload Documents Dialog General Elements
        private readonly By documentsUploadHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private readonly By documentUploadInstructionsLabel = By.XPath("//label[contains(text(),'Choose a max of 10 files to attach at the time')]");
        private readonly By documentUploadDragDropArea = By.XPath("//div[contains(text(),'Drag files here to attach or')]");
        private readonly By documentUploadDocInput = By.CssSelector("input[data-testid='upload-input']");
        private readonly By documentUploadConfirmationMessage = By.XPath("//span[contains(text(),'files successfully uploaded')]");

        //Update Documents General Elements
        private readonly By documentUpdateDocStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private readonly By documentUploadStatusSelect = By.Id("input-documentStatusCode");
        private readonly By documentUpdateDetailsSubtitle = By.XPath("//h3[contains(text(),'Details')]");
        

        //Upload Documents Other Type Fields
        private readonly By documentOtherTypePINLabel = By.XPath("//div[@class='pb-2 row'][1]/div/label[contains(text(),'PIN')]");
        private readonly By documentOtherTypePINInput = By.Id("input-documentMetadata.71");
        private readonly By documentOtherTypePropIdLabel = By.XPath("//div[@class='pb-2 row'][2]/div/label[contains(text(),'Property identifier')]");
        private readonly By documentRoadNameInput = By.Id("input-documentMetadata.75");
        private readonly By documentShortDescriptorLabel = By.XPath("//label[contains(text(),'Short descriptor')]");

        //Upload Documents Field Notes Type Fields
        private readonly By documentFieldNotesTypeDistrictLotLabel = By.XPath("//label[contains(text(),'District lot')]");
        private readonly By documentFieldNotesTypeDistrictLotInput = By.Id("input-documentMetadata.101");
        private readonly By documentFieldNotesTypeYearLabel = By.XPath("//label[contains(text(),'Field book #/Year')]");
        private readonly By documentFieldNotesTypeYearInput = By.Id("input-documentMetadata.104");
        private readonly By documentFieldNotesTypeLandDistrictLabel = By.XPath("//label[contains(text(),'Land district')]");
        private readonly By documentFieldNotesTypeLandDistrictInput = By.Id("input-documentMetadata.99");

        //Upload Documents District Road Register Fields
        private readonly By documentDistrictRoadRegisterTypeElectoralDistrictLabel = By.XPath("//label[contains(text(),'Electoral district')]");
        private readonly By documentDistrictRoadRegisterTypeElectoralDistrictInput = By.Id("input-documentMetadata.102");
        private readonly By documentDistrictRoadRegisterTypeHighwayDistrictLabel = By.XPath("//label[contains(text(),'Highway district')]");
        private readonly By documentDistrictRoadRegisterTypeHighwayDistrictInput = By.Id("input-documentMetadata.103");

        //Upload BC Assessment Search Type Fields
        private readonly By documentCivicAddressLabel = By.XPath("//label[contains(text(),'Civic address')]");
        private readonly By documentBCAssessmentTypeAddressMandatory = By.XPath("//div[contains(text(),'Civic address is required')]");
        private readonly By documentBCAssessmentTypeJurisdictionLabel = By.XPath("//label[contains(text(),'Jurisdiction')]");
        private readonly By documentBCAssessmentTypeJurisdictionInput = By.Id("input-documentMetadata.67");
        private readonly By documentBCAssessmentTypeJurisdictionMandatory = By.XPath("//div[contains(text(),'Jurisdiction is required')]");
        private readonly By documentBCAssessmentTypeRollLabel = By.XPath("//label[contains(text(),'Roll')]");
        private readonly By documentBCAssessmentTypeRollInput = By.Id("input-documentMetadata.63");
        private readonly By documentBCAssessmentTypeYearMandatory = By.XPath("//div[contains(text(),'Year is required')]");

        //Upload Transfer of Administration Type Fields
        private readonly By documentDateSignedLabel = By.XPath("//label[contains(text(),'Date signed')]");
        private readonly By documentDateSignedInput = By.Id("input-documentMetadata.73");
        private readonly By documentMOTIFileLabel = By.XPath("//label[contains(text(),'MoTI file #')]");
        private readonly By documentTransferAdmTypeMOTIFileMandatory = By.XPath("//div[contains(text(),'MoTI file # is required')]");
        private readonly By documentTransferAdmTypeProIdLabel = By.XPath("//label[contains(text(),'Property identifier')]");
        private readonly By documentTransferAdmTypeRoadNameMandatory = By.XPath("//div[contains(text(),'Road name is required')]");
        private readonly By documentTransferAdmTypeTransferLabel = By.XPath("//label[contains(text(),'Transfer')]");
        private readonly By documentTransferAdmTypeTransferInput = By.Id("input-documentMetadata.66");
        private readonly By documentTransferAdmTypeTransferMandatory = By.XPath("//div[contains(text(),'Transfer # is required')]");

        //Upload Ministerial Order Type Fields
        private readonly By documentMinisterialOrderTypeMOLabel = By.XPath("//label[contains(text(),'MO #')]");
        private readonly By documentMinisterialOrderTypeMOInput = By.Id("input-documentMetadata.70");
        private readonly By documentTypeMotiFileInput = By.Id("input-documentMetadata.87");
        private readonly By documentPropertyIdentifierLabel = By.XPath("//label[contains(text(),'Property identifier')]");
        private readonly By documentTypePropIdInput = By.Id("input-documentMetadata.100");

        //Upload Canada Lands Survey Fields
        private readonly By documentCanLandSurveyTypeCanLandSurveyLabel = By.XPath("//label[contains(text(),'Canada land survey')]");
        private readonly By documentCanLandSurveyTypeCanLandSurveyInput = By.Id("input-documentMetadata.97");
        private readonly By documentCanLandSurveyTypeCanLandSurveyMandatory = By.XPath("//div[contains(text(),'Canada land survey # is required')]");
        private readonly By documentCanLandSurveyTypeIndianReserveLabel = By.XPath("//label[contains(text(),'Indian reserve')]");
        private readonly By documentCanLandSurveyTypeIndianReserveInput = By.Id("input-documentMetadata.98");
        private readonly By documentCanLandSurveyTypeIndianReserveMandatory = By.XPath("//div[contains(text(),'Indian reserve or national park is required')]");

        //Upload Photos/Images/Video and Correspondence Fields
        private readonly By documentCivicAddressInput = By.CssSelector("input[data-testid='metadata-input-CIVIC_ADDRESS']");
        private readonly By documentPhotosCorrespondenceTypeDateLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Date')]");
        private readonly By documentPhotosCorrespondenceTypeDateInput = By.Id("input-documentMetadata.57");
        private readonly By documentOwnerLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Owner')]");
        private readonly By documentTypeOwnerInput = By.Id("input-documentMetadata.51");
        private readonly By documentPhotosCorrespondenceTypePropIdLabel = By.XPath("//label[contains(text(),'Property identifier')]");
        private readonly By documentTypePropertyIdentifierInput = By.Id("input-documentMetadata.94");

        private By documentShortDescriptorInput = By.Id("input-documentMetadata.55");

        //Upload Miscellaneous notes (LTSA) Fields
        private readonly By documentMiscNotesTypePIDLabel = By.XPath("//input[@id='input-documentMetadata.62']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'PID')]");
        private readonly By documentMiscNotesTypePIDInput = By.Id("input-documentMetadata.62");

        //Upload Title search/ Historical title Fields
        private readonly By documentTitleSearchTypePIDLabel = By.XPath("//input[@id='input-documentMetadata.62']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'PID')]");
        private readonly By documentTitleSearchTypePIDInput = By.Id("input-documentMetadata.62");
        private readonly By documentTitleSearchTypeTitleLabel = By.XPath("//input[@id='input-documentMetadata.58']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Title')]");
        private readonly By documentTitleSearchTypeTitleInput = By.Id("input-documentMetadata.58");

        //Upload Historical File Fields
        private readonly By documentHistoricFileTypeEndDateLabel = By.XPath("//label[contains(text(),'End date')]");
        private readonly By documentHistoricFileTypeEndDateInput = By.Id("input-documentMetadata.93");
        private readonly By documentHistoricFileTypeFileLabel = By.XPath("//div[@class='pr-0 text-left col-4']/label[contains(text(),'File #')]");
        private readonly By documentHistoricFileTypeFileInput = By.Id("input-documentMetadata.42");
        private readonly By documentHistoricFileTypeFileMandatory = By.XPath("//div[contains(text(),'File # is required')]");
        private readonly By documentHistoricFileTypePhyLocationLabel = By.XPath("//label[contains(text(),'Physical location')]");
        private readonly By documentHistoricFileTypePhyLocationInput = By.Id("input-documentMetadata.95");
        private readonly By documentHistoricFileTypeSectionLabel = By.XPath("//label[contains(text(),'Section')]");
        private readonly By documentHistoricFileTypeSectionInput = By.Id("input-documentMetadata.47");
        private readonly By documentHistoricFileTypeStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private readonly By documentHistoricFileTypeStartDateInput = By.Id("input-documentMetadata.91");

        //Upload Crown Grant Fields
        private readonly By documentCrownGrantTypeCrownLabel = By.XPath("//label[contains(text(),'Crown grant #')]");
        private readonly By documentCrownGrantTypeCrownInput = By.Id("input-documentMetadata.56");
        private readonly By documentCrownGrantTypeCrownMandatory = By.XPath("//div[contains(text(),'Crown grant # is required')]");

        //Upload Privy Council Fields
        private readonly By documentPrivyCouncilTypePrivyLabel = By.XPath("//label[contains(text(),'Year - privy council #')]");
        private readonly By documentPrivyCouncilTypePrivyInput = By.Id("input-documentMetadata.92");
        private readonly By documentPrivyCounciltTypePrivyMandatory = By.XPath("//div[contains(text(),'Year - privy council # is required')]");

        //Upload OIC Fields
        private readonly By documentOICTypeOICLabel = By.XPath("//label[contains(text(),'OIC #')]");
        private readonly By documentOICTypeInput = By.Id("input-documentMetadata.43");
        private readonly By documentOICTypeOICRouteLabel = By.XPath("//label[contains(text(),'OIC route #')]");
        private readonly By documentOICTypeOICRouteInput = By.Id("input-documentMetadata.90");
        private readonly By documentOICTypeOICTypeLabel = By.XPath("//label[contains(text(),'OIC type')]");
        private readonly By documentOICTypeOICTypeInput = By.Id("input-documentMetadata.89");
        private readonly By documentYearLabel = By.XPath("//label[contains(text(),'Year')]");
        private readonly By documentYearInput = By.Id("input-documentMetadata.48");

        //Upload Legal Survey Plans Fields
        private readonly By documentLegalSurveyNbrLabel = By.XPath("//label[contains(text(),'Legal survey plan #')]");
        private readonly By documentLegalSurveyInput = By.Id("input-documentMetadata.84");
        private readonly By documentMOTIPlanLabel = By.XPath("//label[contains(text(),'MoTI plan #')]");
        private readonly By documentMOTIPlanInput = By.Id("input-documentMetadata.83");
        private readonly By documentLegalSurveyPlanTypeLabel = By.XPath("//label[contains(text(),'Plan type')]");
        private readonly By documentLegalSurveyPlanTypeInput = By.Id("input-documentMetadata.88");

        //Upload MoTI Plan Fields
        private readonly By documentMoTIPlanLegalSurveyPublishDateLabel = By.XPath("//label[contains(text(),'Published date')]");
        private readonly By documentMoTIPlanLegalSurveyPublishDateInput = By.Id("input-documentMetadata.86");
        private readonly By documentMoTIPlanLegalSurveyRelatedGazetteLabel = By.XPath("//label[contains(text(),'Related gazette')]");
        private readonly By documentMoTIPlanLegalSurveyRelatedGazetteInput = By.Id("input-documentMetadata.85");

        //Upload Gazette Fields
        private readonly By documentGazetteDateLabel = By.XPath("//label[contains(text(),'Gazette date')]");
        private readonly By documentGazetteDateInput = By.Id("input-documentMetadata.81");
        private readonly By documentGazettePageLabel = By.XPath("//label[contains(text(),'Gazette page #')]");
        private readonly By documentGazettePageInput = By.Id("input-documentMetadata.80");
        private readonly By documentGazettePublishedDateLabel = By.XPath("//label[contains(text(),'Gazette published date')]");
        private readonly By documentGazettePublishedDateInput = By.Id("input-documentMetadata.78");
        private readonly By documentGazettePublishedDateMandatory = By.XPath("//div[contains(text(),'Gazette published date is required')]");
        private readonly By documentGazettePublishedTypeLabel = By.XPath("//label[contains(text(),'Gazette type')]");
        private readonly By documentGazettePublishedTypeInput = By.Id("input-documentMetadata.82");
        private readonly By documentGazettePublishedTypeMandatory = By.XPath("//div[contains(text(),'Gazette type is required')]");
        private readonly By documentGazetteLegalSurveyPlanLabel = By.XPath("//label[contains(text(),'Legal survey plan #')]");
        private readonly By documentGazetteLTSALabel = By.XPath("//label[contains(text(),'LTSA schedule filing')]");
        private readonly By documentGazetteLTSAInput = By.Id("input-documentMetadata.39");
        private readonly By documentGazetteLegalSurveyMotiPlanLabel = By.XPath("//label[contains(text(),'MoTI plan #')]");
        private readonly By documentRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private readonly By documentGazetteRoadNameMandatory = By.XPath("//div[contains(text(),'Road name is required')]");

        //Upload PA plans Fields
        private readonly By documentPAPlanNbrLabel = By.XPath("//label[contains(text(),'Plan #')]");
        private readonly By documentPAPlanNbrInput = By.Id("input-documentMetadata.28");
        private readonly By documentPAPlanNbrMandatory = By.XPath("//div[contains(text(),'Plan # is required')]");
        private readonly By documentPAPlanRevisionLabel = By.XPath("//label[contains(text(),'Plan revision')]");
        private readonly By documentPAPlanRevisionInput = By.Id("input-documentMetadata.79");
        private readonly By documentPAPlanProjectLabel = By.XPath("//label[contains(text(),'Project #')]");
        private readonly By documentPAPlanProjectInput = By.Id("input-documentMetadata.31");
        private readonly By documentPAPlanProjectNameLabel = By.XPath("//input[@id='input-documentMetadata.77']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Project name')]");
        private readonly By documentPAPlanProjectNameInput = By.Id("input-documentMetadata.77");
        private readonly By documentPAPlanProjectNameMandatory = By.XPath("//div[contains(text(),'Project name is required')]");

        //View Document Details Elements
        private readonly By documentViewDocumentTypeLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/label[contains(text(),'Document type')]");
        private readonly By documentViewDocumentTypeContent = By.XPath("//div[@class='modal-body']/div/div/div/div/label[contains(text(),'Document type')]/parent::div/following-sibling::div");
        private readonly By documenyViewDocumentNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/label[contains(text(),'File name')]");
        private readonly By documentViewFileNameContent = By.XPath("//div[@class='modal-body']/div/div/div/div/label[contains(text(),'File name')]/parent::div/following-sibling::div");
        private readonly By documentViewDownloadButton = By.CssSelector("button[data-testid='document-download-button']");
        private readonly By documentViewInfoSubtitle = By.XPath("//div[contains(text(),'Document Information')]");
        private readonly By documentViewDocumentInfoTooltip = By.CssSelector("span[data-testid='tooltip-icon-documentInfoToolTip']");
        private readonly By documentViewStatusLabel = By.XPath("//div[contains(text(),'Document Information')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]");
        private readonly By documentViewStatusContent = By.XPath("//div[contains(text(),'Document Information')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]/parent::div/following-sibling::div");

        private readonly By documentViewDetailsSubtitle = By.XPath("//h3[contains(text(),'Details')]");

        private readonly By documentViewCanadaLandSurveyContent = By.XPath("//label[contains(text(),'Canada land survey')]/parent::div/following-sibling::div");
        private readonly By documentViewCivicAddressContent = By.XPath("//label[contains(text(),'Civic address')]/parent::div/following-sibling::div");
        private readonly By documentViewCrownGrantContent = By.XPath("//label[contains(text(),'Crown grant #')]/parent::div/following-sibling::div");
        private readonly By documentViewDateContent = By.XPath("//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Date')]/parent::div/following-sibling::div");
        private readonly By documentViewDateSignedContent = By.XPath("//label[contains(text(),'Date signed')]/parent::div/following-sibling::div");
        private readonly By documentViewDistrictLotContent = By.XPath("//label[contains(text(),'District lot')]/parent::div/following-sibling::div");
        private readonly By documentViewElectoralDistrictContent = By.XPath("//label[contains(text(),'Electoral district')]/parent::div/following-sibling::div");
        private readonly By documentViewEndDateContent = By.XPath("//label[contains(text(),'End date')]/parent::div/following-sibling::div");
        private readonly By documentViewFieldBookContent = By.XPath("//label[contains(text(),'Field book #/Year')]/parent::div/following-sibling::div");
        private readonly By documentViewFileNumberContent = By.XPath("//div[@class='pr-0 text-left col-4']/label[contains(text(),'File #')]/parent::div/following-sibling::div");
        private readonly By documentViewGazetteDateContent = By.XPath("//label[contains(text(),'Gazette date')]/parent::div/following-sibling::div");
        private readonly By documentViewGazettePageContent = By.XPath("//label[contains(text(),'Gazette page #')]/parent::div/following-sibling::div");
        private readonly By documentViewGazettePublishedDateContent = By.XPath("//label[contains(text(),'Gazette published date')]/parent::div/following-sibling::div");
        private readonly By documentViewGazettePublishedTypeContent = By.XPath("//label[contains(text(),'Gazette type')]/parent::div/following-sibling::div");
        private readonly By documentViewGazetteHighwayDistrictContent = By.XPath("//label[contains(text(),'Highway district')]/parent::div/following-sibling::div");
        private readonly By documentViewIndianReserveContent = By.XPath("//label[contains(text(),'Indian reserve')]/parent::div/following-sibling::div");
        private readonly By documentViewJurisdictionContent = By.XPath("//label[contains(text(),'Jurisdiction')]/parent::div/following-sibling::div");
        private readonly By documentViewLandDistrictContent = By.XPath("//label[contains(text(),'Land district')]/parent::div/following-sibling::div");
        private readonly By documentViewLegalSurveyPlanContent = By.XPath("//label[contains(text(),'Legal survey plan #')]/parent::div/following-sibling::div");
        private readonly By documentViewLTSAScheduleFilingContent = By.XPath("//label[contains(text(),'LTSA schedule filing')]/parent::div/following-sibling::div");
        private readonly By documentViewMOContent = By.XPath("//label[contains(text(),'MO #')]/parent::div/following-sibling::div");
        private readonly By documentViewMotiFileContent = By.XPath("//label[contains(text(),'MoTI file #')]/parent::div/following-sibling::div");
        private readonly By documentViewMotiPlanContent = By.XPath("//label[contains(text(),'MoTI plan #')]/parent::div/following-sibling::div");
        private readonly By documentViewOICNumberContent = By.XPath("//label[contains(text(),'OIC #')]/parent::div/following-sibling::div");
        private readonly By documentViewOICRouteContent = By.XPath("//label[contains(text(),'OIC route #')]/parent::div/following-sibling::div");
        private readonly By documentViewOICTypeContent = By.XPath("//label[contains(text(),'OIC type')]/parent::div/following-sibling::div");
        private readonly By documentViewOwnerContent = By.XPath("//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Owner')]/parent::div/following-sibling::div");
        private readonly By documentViewPhysicalLocationContent = By.XPath("//label[contains(text(),'Physical location')]/parent::div/following-sibling::div");
        private readonly By documentViewPIDLabel = By.XPath("//label[contains(text(),'PID')]");
        private readonly By documentViewPIDContent = By.XPath("//label[contains(text(),'PID')]/parent::div/following-sibling::div");
        private readonly By documentViewPINContent = By.XPath("//div[@class='pb-2 row'][1]/div/label[contains(text(),'PIN')]/parent::div/following-sibling::div");
        private readonly By documentViewPlanNumberContent = By.XPath("//label[contains(text(),'Plan #')]/parent::div/following-sibling::div");
        private readonly By documentViewPlanRevisionContent = By.XPath("//label[contains(text(),'Plan revision')]/parent::div/following-sibling::div");
        private readonly By documentViewPlanTypeContent = By.XPath("//label[contains(text(),'Plan type')]/parent::div/following-sibling::div");
        private readonly By documentViewProjectNumberContent = By.XPath("//label[contains(text(),'Project #')]/parent::div/following-sibling::div");
        private readonly By documentViewProjectLabel = By.XPath("/label[contains(text(),'Project name')]");
        private readonly By documentViewProjectContent = By.XPath("/label[contains(text(),'Project name')]/parent::div/following-sibling::div");
        private readonly By documentViewPropertyIdentifierLabel = By.XPath("/label[contains(text(),'Property identifier')]");
        private readonly By documentViewPropertyIdentifierContent = By.XPath("/label[contains(text(),'Property identifier')]/parent::div/following-sibling::div");
        private readonly By documentViewPublishedDateContent = By.XPath("//label[contains(text(),'Published date')]/parent::div/following-sibling::div");
        private readonly By documentViewRelatedGazetteContent = By.XPath("//label[contains(text(),'Related gazette')]/parent::div/following-sibling::div");
        private readonly By documentViewRoadNameContent = By.XPath("//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Road name')]/parent::div/following-sibling::div");
        private readonly By documentViewRollContent = By.XPath("//label[contains(text(),'Roll')]/parent::div/following-sibling::div");
        private readonly By documentViewSectionContent = By.XPath("//label[contains(text(),'Section')]/parent::div/following-sibling::div");
        private readonly By documentViewShortDescriptorContent = By.XPath("//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Short descriptor')]/parent::div/following-sibling::div");
        private readonly By documentViewStartDateContent = By.XPath("//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Start date')]/parent::div/following-sibling::div");
        private readonly By documentViewTitleContent = By.XPath("//label[contains(text(),'Title')]/parent::div/following-sibling::div");
        private readonly By documentViewTransferContent = By.XPath("//label[contains(text(),'Transfer')]/parent::div/following-sibling::div");
        private readonly By documentViewYearContent = By.XPath("//label[contains(text(),'Year')]/parent::div/following-sibling::div");
        private readonly By documentViewYearPrivyCouncilContent = By.XPath("//label[contains(text(),'Year - privy council #')]/parent::div/following-sibling::div");

        //Document Modal Elements
        private readonly By documentModalCloseIcon = By.CssSelector("div[class='modal-close-btn']");
        private readonly By documentEditBttn = By.XPath("//div[@class='modal-body']/div/div/div/div/button");
        private readonly By documentCancelEditButton = By.XPath("//div[@class='modal-body']/div/div[2]/div/div/div/div/button/div[contains(text(),'No')]/parent::button");
        private readonly By documentSaveEditButton = By.XPath("//div[@class='modal-body']/div/div[2]/div/div/div/div/button/div[contains(text(),'Yes')]/parent::button");

        //Toast Element
        private readonly By documentGeneralToastBody = By.CssSelector("div[class='Toastify__toast-body']");

        //Document Confirmation Modal Elements
        private readonly By documentConfirmationModal = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/parent::div");
        private readonly By documentConfirmationContent = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/following-sibling::div[@class='modal-body']");
        private readonly By documentConfirmModalOkBttn = By.CssSelector("div[class='modal-content'] button[title='ok-modal']");

        //Document Delete Document Confirmation Modal Elements
        private readonly By documentDeleteHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private readonly By documentDeleteContent1 = By.CssSelector("div[class='modal-body'] div div:nth-child(1)");
        private readonly By documentDeteleContent2 = By.CssSelector("div[class='modal-body'] div:nth-child(3)");
        private readonly By documentDeleteContent3 = By.CssSelector("div[class='modal-body'] div strong");
        private readonly By documentDeleteOkBttn = By.CssSelector("button[title='ok-modal']");

        //Documents Tab List Filters
        private readonly By documentFilterTypeSelect = By.XPath("//select[@data-testid='document-type']");
        private readonly By documentFilterStatusSelect = By.XPath("//select[@data-testid='document-status']");
        private readonly By documentFilterNameInput = By.XPath("//input[@data-testid='document-filename']");
        private readonly By documentFilterSearchBttn = By.XPath("//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='search']");
        private readonly By documentFilterResetBttn = By.XPath("//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='reset-button']");

        //Document List Sortable Columns Elements
        private readonly By documentDocumentTypeSortBttn = By.CssSelector("div[data-testid='sort-column-documentType']");
        private readonly By documentDocumentNameSortBttn = By.CssSelector("div[data-testid='sort-column-fileName']");
        private readonly By documentDocumentUploadedDateSortBttn = By.CssSelector("div[data-testid='sort-column-appCreateTimestamp']");
        private readonly By documentDocumentStatusSortBttn = By.CssSelector("div[data-testid='sort-column-statusTypeCode']");

        //Documents Tab List Results
        private readonly By documentTableListView = By.XPath("//div[@data-testid='documentsTable']");
        private readonly By documentTableTypeColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document type')]");
        private readonly By documentTableNameColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'File name')]");
        private readonly By documentTableDateColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Uploaded')]");
        private readonly By documentTableStatusColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Status')]");
        private readonly By documentTableActionsColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Actions')]");
        private readonly By documentTableContentTotal = By.CssSelector("div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By documentTableWaitSpinner = By.CssSelector("div[class='table-loading']");
       
        //Activities Documents List 1st Result Elements
        private readonly By documentTableResults1stDocumentTypeContent = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[1]");
        private readonly By documentTableResults1stDocumentNameContent = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[2]/div/button/div");
        private readonly By documentTableResults1stDocumentStatusContent = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[4]");
        private readonly By documentTableResults1stViewBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-view-button']");
        private readonly By documentTableResults1stDeleteBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-delete-button']");

        //Documents Tab Pagination
        private readonly By documentPagination = By.XPath("//div[@class='row']/div[4]/ul[@class='pagination']");
        private readonly By documentMenuPagination = By.XPath("//div[@class='row']/div[3]/div[@class='Menu-root']");
        private readonly By documentPaginationPrevPageLink = By.CssSelector("ul[class='pagination'] a[aria-label='Previous page']");
        private readonly By documentPaginationNextPageLink = By.CssSelector("ul[class='pagination'] a[aria-label='Next page']");

        //Document Preview Elements
        private readonly By documentPreiewWindow = By.CssSelector("iframe");
        private readonly By documentPreiewDownloadBttn = By.CssSelector("button[aria-label='Download']");
        private readonly By documentPreiewZoomInBttn = By.CssSelector("button[aria-label='Zoom in']");
        private readonly By documentPreiewZoomOutBttn = By.CssSelector("button[aria-label='Zoom out']");
        private readonly By documentPreiewEnterScreenBttn = By.CssSelector("button[aria-label='Enter Fullscreen']");
        private readonly By documentPreiewCloseBttn = By.CssSelector("button[aria-label='Close']");


        private SharedModals sharedModals;

        public DigitalDocuments(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateDocumentsTab()
        {
            WaitUntilVisible(documentsTab);
            webDriver.FindElement(documentsTab).Click();
        }

        public void NavigateToFirstPageDocumentsTable()
        {
            WaitUntilVisible(documentPaginationPrevPageLink);
            FocusAndClick(documentPaginationPrevPageLink);
        }

        public void AddNewDocumentButton(string fileType)
        {
            if (fileType.Equals("Lease") || fileType.Equals("CDOGS Templates") || fileType.Equals("Project") || fileType.Equals("Property Management"))
            {
                WaitUntilClickable(addDocumentBttn);
                FocusAndClick(addDocumentBttn);
            }
            else
            {
                WaitUntilClickable(addFileDocumentBttn);
                FocusAndClick(addFileDocumentBttn);
            } 
        }

        public void VerifyDocumentFields(string documentType)
        {
            VerifyGeneralUpdateDocumentForm();
            System.Diagnostics.Debug.WriteLine(documentType);

            switch (documentType)
            {
                case "BC assessment search":
                    VerifyBCAssessmentFields();
                    break;
                case "Other":
                    VerifyOtherTypeFields();
                    break;
                case "Field notes":
                    VerifyFieldNotesFields();
                    break;
                case "District road register":
                    VerifyDistrictRoadRegisterFields();
                    break;
                case "Transfer of administration":
                    VerifyTransferAdministrationFields();
                    break;
                case "Ministerial order":
                    VerifyMinisterialOrderFields();
                    break;
                case "Canada lands survey":
                    VerifyCanadaLandsSurveyFields();
                    break;
                case "Correspondence":
                    VerifyPhotosCorrespondenceFields();
                    break;
                case "Photos / Images/ Video":
                    VerifyPhotosCorrespondenceFields();
                    break;
                case "Miscellaneous notes (LTSA)":
                    VerifyMiscellaneousNotesFields();
                    break;
                case "Title search / Historical title":
                    VerifyTitleSearchFields();
                    break;
                case "Historical file":
                    VerifyHistoricalFileFields();
                    break;
                case "Crown grant":
                    VerifyCrownGrantFields();
                    break;
                case "Privy council":
                    VerifyPrivyCouncilFields();
                    break;
                case "Order in Council (OIC)":
                    VerifyOICFields();
                    break;
                case "Legal survey plan":
                    VerifyLegalSurveyFields();
                    break;
                case "MoTI plan":
                    VerifyMOTIPlanFields();
                    break;
                case "Gazette":
                    VerifyGazetteFields();
                    break;
                case "PA plans / Design drawings":
                    VerifyPAPlansFields();
                    break;
                default:
                    VerifyShortDescriptorField();
                    break;
            }
        }

        public void VerifyDocumentsListView(string fileType)
        {
            WaitUntilVisible(documentFilterTypeSelect);
            if (fileType.Equals("CDOGS Templates") || fileType.Equals("Project") || fileType.Equals("Property Management"))
            {
                AssertTrueIsDisplayed(documentsTitle);
                AssertTrueIsDisplayed(addDocumentBttn);
            }
            else
            {
                AssertTrueIsDisplayed(documentsFileTitle);
                AssertTrueIsDisplayed(addFileDocumentBttn);
            }
            AssertTrueIsDisplayed(documentFilterTypeSelect);
            AssertTrueIsDisplayed(documentFilterStatusSelect);
            AssertTrueIsDisplayed(documentFilterNameInput);
            AssertTrueIsDisplayed(documentFilterSearchBttn);
            AssertTrueIsDisplayed(documentFilterResetBttn);

            AssertTrueIsDisplayed(documentTableListView);
            AssertTrueIsDisplayed(documentTableTypeColumn);
            AssertTrueIsDisplayed(documentTableNameColumn);
            AssertTrueIsDisplayed(documentTableDateColumn);
            AssertTrueIsDisplayed(documentTableStatusColumn);
            AssertTrueIsDisplayed(documentTableActionsColumn);
        }

        public void VerifyPaginationElements()
        {
            AssertTrueIsDisplayed(documentPagination);
            AssertTrueIsDisplayed(documentMenuPagination);
            AssertTrueIsDisplayed(documentPaginationPrevPageLink);
            AssertTrueIsDisplayed(documentPaginationNextPageLink);
        }

        public int DigitalDocumentsTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(documentTableContentTotal).Count;
        }

        public void OrderByDocumentFileType()
        {
            WaitUntilClickable(documentDocumentTypeSortBttn);
            webDriver.FindElement(documentDocumentTypeSortBttn).Click();
        }

        public void OrderByDocumentFileName()
        {
            WaitUntilClickable(documentDocumentNameSortBttn);
            webDriver.FindElement(documentDocumentNameSortBttn).Click();
        }

        public void OrderByDocumentFileStatus()
        {
            WaitUntilClickable(documentDocumentStatusSortBttn);
            webDriver.FindElement(documentDocumentStatusSortBttn).Click();
        }

        public string FirstDocumentFileType()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(documentTableResults1stDocumentTypeContent).Text;
        }

        public string FirstDocumentFileName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(documentTableResults1stDocumentNameContent).Text;
        }

        public string FirstDocumentFileStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(documentTableResults1stDocumentStatusContent).Text;
        }

        public void FilterByType(string documentType)
        {
            Wait();
            webDriver.FindElement(documentFilterResetBttn).Click();

            WaitUntilClickable(documentFilterTypeSelect);
            ChooseSpecificSelectOption(documentFilterTypeSelect, documentType);
            FocusAndClick(documentFilterSearchBttn);
        }

        public void FilterByStatus(string documentStatus)
        {
            Wait();
            webDriver.FindElement(documentFilterResetBttn).Click();

            WaitUntilVisible(documentFilterStatusSelect);
            ChooseSpecificSelectOption(documentFilterStatusSelect, documentStatus);
            FocusAndClick(documentFilterSearchBttn);
        }

        public void FilterByName(string documentName)
        {
            Wait();
            webDriver.FindElement(documentFilterResetBttn).Click();

            WaitUntilVisible(documentFilterNameInput);
            webDriver.FindElement(documentFilterNameInput).SendKeys(documentName);
            FocusAndClick(documentFilterSearchBttn);
        }

        public int TotalSearchDocuments()
        {
            return webDriver.FindElements(documentTableContentTotal).Count();
        } 

        public void UploadDocument(string documentFile)
        {
            Wait();
            webDriver.FindElement(documentUploadDocInput).SendKeys(documentFile);
        }

        public void SaveDigitalDocumentUpload()
        {
            sharedModals.ModalClickOKBttn();

            WaitUntilVisible(documentUploadConfirmationMessage);
            sharedModals.ModalClickOKBttn();
        }

        public void SaveDigitalDocumentUpdate()
        {
            webDriver.FindElement(documentSaveEditButton).Click();
            WaitUntilSpinnerDisappear();
        }

        public void SaveEditDigitalDocument()
        {
            WaitUntilClickable(documentSaveEditButton);
            webDriver.FindElement(documentSaveEditButton).Click();

            WaitUntilSpinnerDisappear();
        }

        public void CancelDigitalDocument()
        {
            sharedModals.ModalClickCancelBttn();

            Wait();
            if (webDriver.FindElements(documentConfirmationModal).Count() > 0)
            {
                AssertTrueElementContains(documentConfirmationContent, "If you choose to cancel now, your changes will not be saved.");
                AssertTrueElementContains(documentConfirmationContent, "Do you want to proceed?");

                webDriver.FindElement(documentConfirmModalOkBttn).Click();
            }
        }

        public void CancelEditDigitalDocument()
        {
            Wait();
            FocusAndClick(documentCancelEditButton);

            WaitUntilVisible(documentConfirmationModal);
            if (webDriver.FindElements(documentConfirmationModal).Count() > 0)
            {
                AssertTrueElementContains(documentConfirmationContent, "If you choose to cancel now, your changes will not be saved.");
                AssertTrueElementContains(documentConfirmationContent, "Do you want to proceed?");

                webDriver.FindElement(documentConfirmModalOkBttn).Click();
            }
        }

        public void CloseDigitalDocumentViewDetails()
        {
            Wait();
            webDriver.FindElement(documentModalCloseIcon).Click();
        }

        public void View1stDocument()
        {
            Wait();
            webDriver.FindElement(documentTableResults1stViewBttn).Click();
        }

        public void ViewUploadedDocument(int index)
        {
            Wait();
            WaitUntilClickable(documentTableResults1stViewBttn);

            if (index > 9)
                FocusAndClick(documentPaginationNextPageLink);

            var elementChild = (index % 10) + 1;
            FocusAndClick(By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[" + elementChild + "]/div/div[5]/div/div/button[@data-testid='document-view-button']"));
        }

        public void Delete1stDocument()
        {
            WaitUntilVisible(documentTableResults1stDeleteBttn);
            webDriver.FindElement(documentTableResults1stDeleteBttn).Click();
           
            WaitUntilVisible(documentDeleteHeader);
            AssertTrueContentEquals(documentDeleteHeader, "Delete a document");
            AssertTrueContentEquals(documentDeleteContent1, "You have chosen to delete this document.");
            AssertTrueContentEquals(documentDeteleContent2, "If the document is linked to other files or entities in PIMS it will still be accessible from there, however if this the only instance then the file will be removed from the document store completely.");
            AssertTrueContentEquals(documentDeleteContent3, "Do you wish to continue deleting this document?");

            webDriver.FindElement(documentDeleteOkBttn).Click();

            WaitUntilDisappear(documentTableWaitSpinner);
        }

        public void EditDocument()
        {
            Wait(2000);
            FocusAndClick(documentEditBttn);
        }

        public void InsertDocumentTypeStatus(DigitalDocument document, int docIdx)
        {
            Wait();

            By docTypeSelectElement = By.Id("input-documents."+ docIdx +".documentTypeId");
            By statusSelectElement = By.Id("input-documents." +docIdx +".documentStatusCode");

            WaitUntilExist(docTypeSelectElement);
            ChooseSpecificSelectOption(docTypeSelectElement, document.DocumentType);
            ChooseSpecificSelectOption(statusSelectElement, document.DocumentStatus);
        }

        public void InsertDocumentTypeDetails(DigitalDocument document)
        {
            Wait();

            if (document.CanadaLandSurvey != "" && webDriver.FindElements(documentCanLandSurveyTypeCanLandSurveyInput).Count > 0)
                webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyInput).SendKeys(document.CanadaLandSurvey);

            if (document.CivicAddress != "" && webDriver.FindElements(documentCivicAddressInput).Count > 0)
                webDriver.FindElement(documentCivicAddressInput).SendKeys(document.CivicAddress);

            if (document.CrownGrant != "" && webDriver.FindElements(documentCrownGrantTypeCrownInput).Count > 0)
                webDriver.FindElement(documentCrownGrantTypeCrownInput).SendKeys(document.CrownGrant);

            if (document.Date != "" && webDriver.FindElements(documentPhotosCorrespondenceTypeDateInput).Count > 0)
                webDriver.FindElement(documentPhotosCorrespondenceTypeDateInput).SendKeys(document.Date);

            if (document.DateSigned != "" && webDriver.FindElements(documentDateSignedInput).Count > 0)
                webDriver.FindElement(documentDateSignedInput).SendKeys(document.DateSigned);

            if (document.DistrictLot != "" && webDriver.FindElements(documentFieldNotesTypeDistrictLotInput).Count > 0)
                webDriver.FindElement(documentFieldNotesTypeDistrictLotInput).SendKeys(document.DistrictLot);

            if (document.ElectoralDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeElectoralDistrictInput).Count > 0)
                webDriver.FindElement(documentDistrictRoadRegisterTypeElectoralDistrictInput).SendKeys(document.ElectoralDistrict);

            if (document.EndDate != "" && webDriver.FindElements(documentHistoricFileTypeEndDateInput).Count > 0)
                webDriver.FindElement(documentHistoricFileTypeEndDateInput).SendKeys(document.EndDate);

            if (document.FieldBook != "" && webDriver.FindElements(documentFieldNotesTypeYearInput).Count > 0)
                webDriver.FindElement(documentFieldNotesTypeYearInput).SendKeys(document.FieldBook);

            if (document.File != "" && webDriver.FindElements(documentHistoricFileTypeFileInput).Count > 0)
                webDriver.FindElement(documentHistoricFileTypeFileInput).SendKeys(document.File);

            if (document.GazetteDate != "" && webDriver.FindElements(documentGazetteDateInput).Count > 0)
                webDriver.FindElement(documentGazetteDateInput).SendKeys(document.GazetteDate);

            if (document.GazettePage != "" && webDriver.FindElements(documentGazettePageInput).Count > 0)
                webDriver.FindElement(documentGazettePageInput).SendKeys(document.GazettePage);

            if (document.GazettePublishedDate != "" && webDriver.FindElements(documentGazettePublishedDateInput).Count > 0)
                webDriver.FindElement(documentGazettePublishedDateInput).SendKeys(document.GazettePublishedDate);

            if (document.GazetteType != "" && webDriver.FindElements(documentGazettePublishedTypeInput).Count > 0)
                webDriver.FindElement(documentGazettePublishedTypeInput).SendKeys(document.GazetteType);

            if (document.HighwayDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeHighwayDistrictInput).Count > 0)
                webDriver.FindElement(documentDistrictRoadRegisterTypeHighwayDistrictInput).SendKeys(document.HighwayDistrict);

            if (document.IndianReserveOrNationalPark != "" && webDriver.FindElements(documentCanLandSurveyTypeIndianReserveInput).Count > 0)
                webDriver.FindElement(documentCanLandSurveyTypeIndianReserveInput).SendKeys(document.IndianReserveOrNationalPark);

            if (document.Jurisdiction != "" && webDriver.FindElements(documentBCAssessmentTypeJurisdictionInput).Count > 0)
                webDriver.FindElement(documentBCAssessmentTypeJurisdictionInput).SendKeys(document.Jurisdiction);

            if (document.LandDistrict != "" && webDriver.FindElements(documentFieldNotesTypeLandDistrictInput).Count > 0)
                webDriver.FindElement(documentFieldNotesTypeLandDistrictInput).SendKeys(document.LandDistrict);

            if (document.LegalSurveyPlan != "" && webDriver.FindElements(documentLegalSurveyInput).Count > 0)
                webDriver.FindElement(documentLegalSurveyInput).SendKeys(document.LegalSurveyPlan);

            if (document.LTSAScheduleFiling != "" && webDriver.FindElements(documentGazetteLTSAInput).Count > 0)
                webDriver.FindElement(documentGazetteLTSAInput).SendKeys(document.LTSAScheduleFiling);

            if (document.MO != "" && webDriver.FindElements(documentMinisterialOrderTypeMOInput).Count > 0)
                webDriver.FindElement(documentMinisterialOrderTypeMOInput).SendKeys(document.MO);

            if (document.MoTIFile != "" && webDriver.FindElements(documentTypeMotiFileInput).Count > 0)
                webDriver.FindElement(documentTypeMotiFileInput).SendKeys(document.MoTIFile);

            if (document.MoTIPlan != "" && webDriver.FindElements(documentMOTIPlanInput).Count > 0)
                webDriver.FindElement(documentMOTIPlanInput).SendKeys(document.MoTIPlan);

            if (document.OIC != "" && webDriver.FindElements(documentOICTypeInput).Count > 0)
                webDriver.FindElement(documentOICTypeInput).SendKeys(document.OIC);

            if (document.OICRoute != "" && webDriver.FindElements(documentOICTypeOICRouteInput).Count > 0)
                webDriver.FindElement(documentOICTypeOICRouteInput).SendKeys(document.OICRoute);

            if (document.OICType != "" && webDriver.FindElements(documentOICTypeOICTypeInput).Count > 0)
                webDriver.FindElement(documentOICTypeOICTypeInput).SendKeys(document.OICType);

            if (document.Owner != "" && webDriver.FindElements(documentTypeOwnerInput).Count > 0)
                webDriver.FindElement(documentTypeOwnerInput).SendKeys(document.Owner);

            if (document.PhysicalLocation != "" && webDriver.FindElements(documentTypeOwnerInput).Count > 0)
                webDriver.FindElement(documentTypeOwnerInput).SendKeys(document.PhysicalLocation);

            if (document.PIDNumber != "" && webDriver.FindElements(documentTypePropIdInput).Count > 0)
                webDriver.FindElement(documentTypePropIdInput).SendKeys(document.PIDNumber);

            if (document.PINNumber != "" && webDriver.FindElements(documentOtherTypePINInput).Count > 0)
                webDriver.FindElement(documentOtherTypePINInput).SendKeys(document.PINNumber);

            if (document.Plan != "" && webDriver.FindElements(documentPAPlanNbrInput).Count > 0)
                webDriver.FindElement(documentPAPlanNbrInput).SendKeys(document.Plan);

            if (document.PlanRevision != "" && webDriver.FindElements(documentPAPlanRevisionInput).Count > 0)
                webDriver.FindElement(documentPAPlanRevisionInput).SendKeys(document.PlanRevision);

            if (document.PlanType != "" && webDriver.FindElements(documentLegalSurveyPlanTypeInput).Count > 0)
                webDriver.FindElement(documentLegalSurveyPlanTypeInput).SendKeys(document.PlanType);

            if (document.Project != "" && webDriver.FindElements(documentPAPlanProjectInput).Count > 0)
                webDriver.FindElement(documentPAPlanProjectInput).SendKeys(document.Project);

            if (document.ProjectName != "" && webDriver.FindElements(documentPAPlanProjectNameInput).Count > 0)
                webDriver.FindElement(documentPAPlanProjectNameInput).SendKeys(document.ProjectName);

            if (document.PropertyIdentifier != "" && webDriver.FindElements(documentTypePropertyIdentifierInput).Count > 0)
                webDriver.FindElement(documentTypePropertyIdentifierInput).SendKeys(document.PropertyIdentifier);

            if (document.PublishedDate != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyPublishDateInput).Count > 0)
                webDriver.FindElement(documentMoTIPlanLegalSurveyPublishDateInput).SendKeys(document.PublishedDate);

            if (document.RelatedGazette != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyRelatedGazetteInput).Count > 0)
                webDriver.FindElement(documentMoTIPlanLegalSurveyRelatedGazetteInput).SendKeys(document.RelatedGazette);

            if (document.RoadName != "" && webDriver.FindElements(documentRoadNameInput).Count > 0)
                webDriver.FindElement(documentRoadNameInput).SendKeys(document.RoadName);

            if (document.Roll != "" && webDriver.FindElements(documentBCAssessmentTypeRollInput).Count > 0)
                webDriver.FindElement(documentBCAssessmentTypeRollInput).SendKeys(document.Roll);

            if (document.Section != "" && webDriver.FindElements(documentHistoricFileTypeSectionInput).Count > 0)
                webDriver.FindElement(documentHistoricFileTypeSectionInput).SendKeys(document.Section);

            if (document.ShortDescriptor != "" && webDriver.FindElements(documentShortDescriptorInput).Count > 0)
                webDriver.FindElement(documentShortDescriptorInput).SendKeys(document.ShortDescriptor);

            if (document.StartDate != "" && webDriver.FindElements(documentHistoricFileTypeStartDateInput).Count > 0)
                webDriver.FindElement(documentHistoricFileTypeStartDateInput).SendKeys(document.StartDate);

            if (document.Title != "" && webDriver.FindElements(documentTitleSearchTypeTitleInput).Count > 0)
                webDriver.FindElement(documentTitleSearchTypeTitleInput).SendKeys(document.Title);

            if (document.Transfer != "" && webDriver.FindElements(documentTransferAdmTypeTransferInput).Count > 0)
                webDriver.FindElement(documentTransferAdmTypeTransferInput).SendKeys(document.Transfer);

            if (document.Year != "" && webDriver.FindElements(documentYearInput).Count > 0)
                webDriver.FindElement(documentYearInput).SendKeys(document.Year);

            if (document.YearPrivyCouncil != "" && webDriver.FindElements(documentPrivyCouncilTypePrivyInput).Count > 0)
                webDriver.FindElement(documentPrivyCouncilTypePrivyInput).SendKeys(document.YearPrivyCouncil);
        }

        public void UpdateNewDocumentType(DigitalDocument document)
        {
            ChooseSpecificSelectOption(documentUploadStatusSelect, document.DocumentStatus);

            if (document.CanadaLandSurvey != "" && webDriver.FindElements(documentCanLandSurveyTypeCanLandSurveyInput).Count > 0)
            {
                ClearInput(documentCanLandSurveyTypeCanLandSurveyInput);
                webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyInput).SendKeys(document.CanadaLandSurvey);
            }
            if (document.CivicAddress != "" && webDriver.FindElements(documentCivicAddressInput).Count > 0)
            {
                ClearInput(documentCivicAddressInput);
                webDriver.FindElement(documentCivicAddressInput).SendKeys(document.CivicAddress);
            }
            if (document.CrownGrant != "" && webDriver.FindElements(documentCrownGrantTypeCrownInput).Count > 0)
            {
                ClearInput(documentCrownGrantTypeCrownInput);
                webDriver.FindElement(documentCrownGrantTypeCrownInput).SendKeys(document.CrownGrant);
            }
            if (document.Date != "" && webDriver.FindElements(documentPhotosCorrespondenceTypeDateInput).Count > 0)
            {
                ClearInput(documentPhotosCorrespondenceTypeDateInput);
                webDriver.FindElement(documentPhotosCorrespondenceTypeDateInput).SendKeys(document.Date);
            }
            if (document.DateSigned != "" && webDriver.FindElements(documentDateSignedInput).Count > 0)
            {
                ClearInput(documentDateSignedInput);
                webDriver.FindElement(documentDateSignedInput).SendKeys(document.DateSigned);
            }
            if (document.DistrictLot != "" && webDriver.FindElements(documentFieldNotesTypeDistrictLotInput).Count > 0)
            {
                ClearInput(documentFieldNotesTypeDistrictLotInput);
                webDriver.FindElement(documentFieldNotesTypeDistrictLotInput).SendKeys(document.DistrictLot);
            }
            if (document.ElectoralDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeElectoralDistrictInput).Count > 0)
            {
                ClearInput(documentDistrictRoadRegisterTypeElectoralDistrictInput);
                webDriver.FindElement(documentDistrictRoadRegisterTypeElectoralDistrictInput).SendKeys(document.ElectoralDistrict);
            }
            if (document.EndDate != "" && webDriver.FindElements(documentHistoricFileTypeEndDateInput).Count > 0)
            {
                ClearInput(documentHistoricFileTypeEndDateInput);
                webDriver.FindElement(documentHistoricFileTypeEndDateInput).SendKeys(document.EndDate);
            }
            if (document.FieldBook != "" && webDriver.FindElements(documentFieldNotesTypeYearInput).Count > 0)
            {
                ClearInput(documentFieldNotesTypeYearInput);
                webDriver.FindElement(documentFieldNotesTypeYearInput).SendKeys(document.FieldBook);
            }
            if (document.File != "" && webDriver.FindElements(documentHistoricFileTypeFileInput).Count > 0)
            {
                ClearInput(documentHistoricFileTypeFileInput);
                webDriver.FindElement(documentHistoricFileTypeFileInput).SendKeys(document.File);
            }
            if (document.GazetteDate != "" && webDriver.FindElements(documentGazetteDateInput).Count > 0)
            {
                ClearInput(documentGazetteDateInput);
                webDriver.FindElement(documentGazetteDateInput).SendKeys(document.GazetteDate);
            }
            if (document.GazettePage != "" && webDriver.FindElements(documentGazettePageInput).Count > 0)
            {
                ClearInput(documentGazettePageInput);
                webDriver.FindElement(documentGazettePageInput).SendKeys(document.GazettePage);
            }
            if (document.GazettePublishedDate != "" && webDriver.FindElements(documentGazettePublishedDateInput).Count > 0)
            {
                ClearInput(documentGazettePublishedDateInput);
                webDriver.FindElement(documentGazettePublishedDateInput).SendKeys(document.GazettePublishedDate);
            }
            if (document.GazetteType != "" && webDriver.FindElements(documentGazettePublishedTypeInput).Count > 0)
            {
                ClearInput(documentGazettePublishedTypeInput);
                webDriver.FindElement(documentGazettePublishedTypeInput).SendKeys(document.GazetteType);
            }
            if (document.HighwayDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeHighwayDistrictInput).Count > 0)
            {
                ClearInput(documentDistrictRoadRegisterTypeHighwayDistrictInput);
                webDriver.FindElement(documentDistrictRoadRegisterTypeHighwayDistrictInput).SendKeys(document.HighwayDistrict);
            }
            if (document.IndianReserveOrNationalPark != "" && webDriver.FindElements(documentCanLandSurveyTypeIndianReserveInput).Count > 0)
            {
                ClearInput(documentCanLandSurveyTypeIndianReserveInput);
                webDriver.FindElement(documentCanLandSurveyTypeIndianReserveInput).SendKeys(document.IndianReserveOrNationalPark);
            }
            if (document.Jurisdiction != "" && webDriver.FindElements(documentBCAssessmentTypeJurisdictionInput).Count > 0)
            {
                ClearInput(documentBCAssessmentTypeJurisdictionInput);
                webDriver.FindElement(documentBCAssessmentTypeJurisdictionInput).SendKeys(document.Jurisdiction);
            }
            if (document.LandDistrict != "" && webDriver.FindElements(documentFieldNotesTypeLandDistrictInput).Count > 0)
            {
                ClearInput(documentFieldNotesTypeLandDistrictInput);
                webDriver.FindElement(documentFieldNotesTypeLandDistrictInput).SendKeys(document.LandDistrict);
            }
            if (document.LegalSurveyPlan != "" && webDriver.FindElements(documentLegalSurveyInput).Count > 0)
            {
                ClearInput(documentLegalSurveyInput);
                webDriver.FindElement(documentLegalSurveyInput).SendKeys(document.LegalSurveyPlan);
            }
            if (document.LTSAScheduleFiling != "" && webDriver.FindElements(documentGazetteLTSAInput).Count > 0)
            {
                ClearInput(documentGazetteLTSAInput);
                webDriver.FindElement(documentGazetteLTSAInput).SendKeys(document.LTSAScheduleFiling);
            }
            if (document.MO != "" && webDriver.FindElements(documentMinisterialOrderTypeMOInput).Count > 0)
            {
                ClearInput(documentMinisterialOrderTypeMOInput);
                webDriver.FindElement(documentMinisterialOrderTypeMOInput).SendKeys(document.MO);
            }
            if (document.MoTIFile != "" && webDriver.FindElements(documentTypeMotiFileInput).Count > 0)
            {
                ClearInput(documentTypeMotiFileInput);
                webDriver.FindElement(documentTypeMotiFileInput).SendKeys(document.MoTIFile);
            }
            if (document.MoTIPlan != "" && webDriver.FindElements(documentMOTIPlanInput).Count > 0)
            {
                ClearInput(documentMOTIPlanInput);
                webDriver.FindElement(documentMOTIPlanInput).SendKeys(document.MoTIPlan);
            }
            if (document.OIC != "" && webDriver.FindElements(documentOICTypeInput).Count > 0)
            {
                ClearInput(documentOICTypeInput);
                webDriver.FindElement(documentOICTypeInput).SendKeys(document.OIC);
            }
            if (document.OICRoute != "" && webDriver.FindElements(documentOICTypeOICRouteInput).Count > 0)
            {
                ClearInput(documentOICTypeOICRouteInput);
                webDriver.FindElement(documentOICTypeOICRouteInput).SendKeys(document.OICRoute);
            }
            if (document.OICType != "" && webDriver.FindElements(documentOICTypeOICTypeInput).Count > 0)
            {
                ClearInput(documentOICTypeOICTypeInput);
                webDriver.FindElement(documentOICTypeOICTypeInput).SendKeys(document.OICType);
            }
            if (document.Owner != "" && webDriver.FindElements(documentTypeOwnerInput).Count > 0)
            {
                ClearInput(documentTypeOwnerInput);
                webDriver.FindElement(documentTypeOwnerInput).SendKeys(document.Owner);
            }
            if (document.PhysicalLocation != "" && webDriver.FindElements(documentTypeOwnerInput).Count > 0)
            {
                ClearInput(documentTypeOwnerInput);
                webDriver.FindElement(documentTypeOwnerInput).SendKeys(document.PhysicalLocation);
            }
            if (document.PIDNumber != "" && webDriver.FindElements(documentTypePropIdInput).Count > 0)
            {
                ClearInput(documentTypePropIdInput);
                webDriver.FindElement(documentTypePropIdInput).SendKeys(document.PIDNumber);
            }
            if (document.PINNumber != "" && webDriver.FindElements(documentOtherTypePINInput).Count > 0)
            {
                ClearInput(documentOtherTypePINInput);
                webDriver.FindElement(documentOtherTypePINInput).SendKeys(document.PINNumber);
            }
            if (document.Plan != "" && webDriver.FindElements(documentPAPlanNbrInput).Count > 0)
            {
                ClearInput(documentPAPlanNbrInput);
                webDriver.FindElement(documentPAPlanNbrInput).SendKeys(document.Plan);
            }
            if (document.PlanRevision != "" && webDriver.FindElements(documentPAPlanRevisionInput).Count > 0)
            {
                ClearInput(documentPAPlanRevisionInput);
                webDriver.FindElement(documentPAPlanRevisionInput).SendKeys(document.PlanRevision);
            }
            if (document.PlanType != "" && webDriver.FindElements(documentLegalSurveyPlanTypeInput).Count > 0)
            {
                ClearInput(documentLegalSurveyPlanTypeInput);
                webDriver.FindElement(documentLegalSurveyPlanTypeInput).SendKeys(document.PlanType);
            }
            if (document.Project != "" && webDriver.FindElements(documentPAPlanProjectInput).Count > 0)
            {
                ClearInput(documentPAPlanProjectInput);
                webDriver.FindElement(documentPAPlanProjectInput).SendKeys(document.Project);
            }
            if (document.ProjectName != "" && webDriver.FindElements(documentPAPlanProjectNameInput).Count > 0)
            {
                ClearInput(documentPAPlanProjectNameInput);
                webDriver.FindElement(documentPAPlanProjectNameInput).SendKeys(document.ProjectName);
            }
            if (document.PropertyIdentifier != "" && webDriver.FindElements(documentTypePropertyIdentifierInput).Count > 0)
            {
                ClearInput(documentTypePropertyIdentifierInput);
                webDriver.FindElement(documentTypePropertyIdentifierInput).SendKeys(document.PropertyIdentifier);
            }
            if (document.PublishedDate != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyPublishDateInput).Count > 0)
            {
                ClearInput(documentMoTIPlanLegalSurveyPublishDateInput);
                webDriver.FindElement(documentMoTIPlanLegalSurveyPublishDateInput).SendKeys(document.PublishedDate);
            }
            if (document.RelatedGazette != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyRelatedGazetteInput).Count > 0)
            {
                ClearInput(documentMoTIPlanLegalSurveyRelatedGazetteInput);
                webDriver.FindElement(documentMoTIPlanLegalSurveyRelatedGazetteInput).SendKeys(document.RelatedGazette);
            }
            if (document.RoadName != "" && webDriver.FindElements(documentRoadNameInput).Count > 0)
            {
                ClearInput(documentRoadNameInput);
                webDriver.FindElement(documentRoadNameInput).SendKeys(document.RoadName);
            }
            if (document.Roll != "" && webDriver.FindElements(documentBCAssessmentTypeRollInput).Count > 0)
            {
                ClearInput(documentBCAssessmentTypeRollInput);
                webDriver.FindElement(documentBCAssessmentTypeRollInput).SendKeys(document.Roll);
            }
            if (document.Section != "" && webDriver.FindElements(documentHistoricFileTypeSectionInput).Count > 0)
            {
                ClearInput(documentHistoricFileTypeSectionInput);
                webDriver.FindElement(documentHistoricFileTypeSectionInput).SendKeys(document.Section);
            }
            if (document.ShortDescriptor != "" && webDriver.FindElements(documentShortDescriptorInput).Count > 0)
            {
                ClearInput(documentShortDescriptorInput);
                webDriver.FindElement(documentShortDescriptorInput).SendKeys(document.ShortDescriptor);
            }
            if (document.StartDate != "" && webDriver.FindElements(documentHistoricFileTypeStartDateInput).Count > 0)
            {
                ClearInput(documentHistoricFileTypeStartDateInput);
                webDriver.FindElement(documentHistoricFileTypeStartDateInput).SendKeys(document.StartDate);
            }
            if (document.Title != "" && webDriver.FindElements(documentTitleSearchTypeTitleInput).Count > 0)
            {
                ClearInput(documentTitleSearchTypeTitleInput);
                webDriver.FindElement(documentTitleSearchTypeTitleInput).SendKeys(document.Title);
            }
            if (document.Transfer != "" && webDriver.FindElements(documentTransferAdmTypeTransferInput).Count > 0)
            {
                ClearInput(documentTransferAdmTypeTransferInput);
                webDriver.FindElement(documentTransferAdmTypeTransferInput).SendKeys(document.Transfer);
            }
            if (document.Year != "" && webDriver.FindElements(documentYearInput).Count > 0)
            {
                ClearInput(documentYearInput);
                webDriver.FindElement(documentYearInput).SendKeys(document.Year);
            }
            if (document.YearPrivyCouncil != "" && webDriver.FindElements(documentPrivyCouncilTypePrivyInput).Count > 0)
            {
                ClearInput(documentPrivyCouncilTypePrivyInput);
                webDriver.FindElement(documentPrivyCouncilTypePrivyInput).SendKeys(document.YearPrivyCouncil);
            }
        }

        public void VerifyDocumentDetailsViewForm(DigitalDocument document)
        {
            Wait();
            WaitUntilSpinnerDisappear();

            //Header
            AssertTrueIsDisplayed(documentViewDocumentTypeLabel);

            AssertTrueContentEquals(documentViewDocumentTypeContent, document.DocumentType);
            AssertTrueIsDisplayed(documenyViewDocumentNameLabel);
            AssertTrueContentNotEquals(documentViewFileNameContent, "");

            //WaitUntilVisible(documentViewDownloadButton);
            //AssertTrueIsDisplayed(documentViewDownloadButton);

            //Document Information
            AssertTrueIsDisplayed(documentViewInfoSubtitle);
            AssertTrueIsDisplayed(documentViewDocumentInfoTooltip);
            AssertTrueIsDisplayed(documentEditBttn);
            AssertTrueIsDisplayed(documentViewStatusLabel);
            AssertTrueContentEquals(documentViewStatusContent, document.DocumentStatus);

            //Document Details
            AssertTrueIsDisplayed(documentViewDetailsSubtitle);

            if (document.CanadaLandSurvey != "" && webDriver.FindElements(documentCanLandSurveyTypeCanLandSurveyLabel).Count > 0)
                AssertTrueContentEquals(documentViewCanadaLandSurveyContent, document.CanadaLandSurvey);
        
            if (document.CivicAddress != "" && webDriver.FindElements(documentCivicAddressLabel).Count > 0)
                AssertTrueContentEquals(documentViewCivicAddressContent, document.CivicAddress);
            
            if (document.CrownGrant != "" && webDriver.FindElements(documentCrownGrantTypeCrownLabel).Count > 0)
                AssertTrueContentEquals(documentViewCrownGrantContent, document.CrownGrant);
            
            if (document.Date != "" && webDriver.FindElements(documentPhotosCorrespondenceTypeDateLabel).Count > 0)
                AssertTrueContentEquals(documentViewDateContent, TranformFormatDateDocument(document.Date));
            
            if (document.DateSigned != "" && webDriver.FindElements(documentDateSignedLabel).Count > 0)
                AssertTrueContentEquals(documentViewDateSignedContent, TranformFormatDateDocument(document.DateSigned));
            
            if (document.DistrictLot != "" && webDriver.FindElements(documentFieldNotesTypeDistrictLotLabel).Count > 0)
                AssertTrueContentEquals(documentViewDistrictLotContent, document.DistrictLot);
            
            if (document.ElectoralDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeElectoralDistrictLabel).Count > 0)
                AssertTrueContentEquals(documentViewElectoralDistrictContent, document.ElectoralDistrict);
            
            if (document.EndDate != "" && webDriver.FindElements(documentHistoricFileTypeEndDateLabel).Count > 0)
                AssertTrueContentEquals(documentViewEndDateContent, TranformFormatDateDocument(document.EndDate));

            if (document.FieldBook != "" && webDriver.FindElements(documentFieldNotesTypeYearLabel).Count > 0)
                AssertTrueContentEquals(documentViewFieldBookContent, document.FieldBook);

            if (document.File != "" && webDriver.FindElements(documentHistoricFileTypeFileLabel).Count > 0)
                AssertTrueContentEquals(documentViewFileNumberContent, document.File);

            if (document.GazetteDate != "" && webDriver.FindElements(documentGazetteDateLabel).Count > 0)
                AssertTrueContentEquals(documentViewGazetteDateContent, TranformFormatDateDocument(document.GazetteDate));

            if (document.GazettePage != "" && webDriver.FindElements(documentGazettePageLabel).Count > 0)
                AssertTrueContentEquals(documentViewGazettePageContent, document.GazettePage);

            if (document.GazettePublishedDate != "" && webDriver.FindElements(documentGazettePublishedDateLabel).Count > 0)
                AssertTrueContentEquals(documentViewGazettePublishedDateContent, TranformFormatDateDocument(document.GazettePublishedDate));
            
            if (document.GazetteType != "" && webDriver.FindElements(documentGazettePublishedTypeLabel).Count > 0)
                AssertTrueContentEquals(documentViewGazettePublishedTypeContent, document.GazetteType);
            
            if (document.HighwayDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeHighwayDistrictLabel).Count > 0)
                AssertTrueContentEquals(documentViewGazetteHighwayDistrictContent, document.HighwayDistrict);
            
            if (document.IndianReserveOrNationalPark != "" && webDriver.FindElements(documentCanLandSurveyTypeIndianReserveLabel).Count > 0)
                AssertTrueContentEquals(documentViewIndianReserveContent, document.IndianReserveOrNationalPark);
            
            if (document.Jurisdiction != "" && webDriver.FindElements(documentBCAssessmentTypeJurisdictionLabel).Count > 0)
                AssertTrueContentEquals(documentViewJurisdictionContent, document.Jurisdiction);
            
            if (document.LandDistrict != "" && webDriver.FindElements(documentFieldNotesTypeLandDistrictLabel).Count > 0)
                AssertTrueContentEquals(documentViewLandDistrictContent, document.LandDistrict);
            
            if (document.LegalSurveyPlan != "" && webDriver.FindElements(documentLegalSurveyNbrLabel).Count > 0)
                AssertTrueContentEquals(documentViewLegalSurveyPlanContent, document.LegalSurveyPlan);
            
            if (document.LTSAScheduleFiling != "" && webDriver.FindElements(documentGazetteLTSALabel).Count > 0)
                AssertTrueContentEquals(documentViewLTSAScheduleFilingContent, document.LTSAScheduleFiling);
            
            if (document.MO != "" && webDriver.FindElements(documentMinisterialOrderTypeMOLabel).Count > 0)
                AssertTrueContentEquals(documentViewMOContent, document.MO);
            
            if (document.MoTIFile != "" && webDriver.FindElements(documentMOTIFileLabel).Count > 0)
                AssertTrueContentEquals(documentViewMotiFileContent, document.MoTIFile);
            
            if (document.MoTIPlan != "" && webDriver.FindElements(documentMOTIPlanLabel).Count > 0)
                AssertTrueContentEquals(documentViewMotiPlanContent, document.MoTIPlan);
            
            if (document.OIC != "" && webDriver.FindElements(documentOICTypeOICLabel).Count > 0)
                AssertTrueContentEquals(documentViewOICNumberContent, document.OIC);
            
            if (document.OICRoute != "" && webDriver.FindElements(documentOICTypeOICRouteLabel).Count > 0)
                AssertTrueContentEquals(documentViewOICRouteContent, document.OICRoute);
            
            if (document.OICType != "" && webDriver.FindElements(documentOICTypeOICTypeLabel).Count > 0)
                AssertTrueContentEquals(documentViewOICTypeContent, document.OICType);
            
            if (document.Owner != "" && webDriver.FindElements(documentOwnerLabel).Count > 0)
                AssertTrueContentEquals(documentViewOwnerContent, document.Owner);
            
            if (document.PhysicalLocation != "" && webDriver.FindElements(documentHistoricFileTypePhyLocationLabel).Count > 0)
                AssertTrueContentEquals(documentViewPhysicalLocationContent, document.PhysicalLocation);
            
            if (document.PIDNumber != "" && webDriver.FindElements(documentViewPIDLabel).Count > 0)
                AssertTrueContentEquals(documentViewPIDContent,document.PIDNumber);
            
            if (document.PINNumber != "" && webDriver.FindElements(documentOtherTypePINLabel).Count > 0)
                AssertTrueContentEquals(documentViewPINContent, document.PINNumber);
            
            if (document.Plan != "" && webDriver.FindElements(documentPAPlanNbrLabel).Count > 0)
                AssertTrueContentEquals(documentViewPlanNumberContent, document.Plan);
            
            if (document.PlanRevision != "" && webDriver.FindElements(documentPAPlanRevisionLabel).Count > 0)
                AssertTrueContentEquals(documentViewPlanRevisionContent, document.PlanRevision);
            
            if (document.PlanType != "" && webDriver.FindElements(documentLegalSurveyPlanTypeLabel).Count > 0)
                AssertTrueContentEquals(documentViewPlanTypeContent, document.PlanType);
            
            if (document.Project != "" && webDriver.FindElements(documentPAPlanProjectLabel).Count > 0)
                AssertTrueContentEquals(documentViewProjectNumberContent, document.Project);
            
            if (document.ProjectName != "" && webDriver.FindElements(documentViewProjectLabel).Count > 0)
                AssertTrueContentEquals(documentViewProjectContent, document.ProjectName);
            
            if (document.PropertyIdentifier != "" && webDriver.FindElements(documentViewPropertyIdentifierLabel).Count > 0)
                AssertTrueContentEquals(documentViewPropertyIdentifierContent, document.PropertyIdentifier);
            
            if (document.PublishedDate != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyPublishDateLabel).Count > 0)
                AssertTrueContentEquals(documentViewPublishedDateContent, TranformFormatDateDocument(document.PublishedDate));
            
            if (document.RelatedGazette != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyRelatedGazetteLabel).Count > 0)
                AssertTrueContentEquals(documentViewRelatedGazetteContent, document.RelatedGazette);
            
            if (document.RoadName != "" && webDriver.FindElements(documentRoadNameLabel).Count > 0)
                AssertTrueContentEquals(documentViewRoadNameContent, document.RoadName);
            
            if (document.Roll != "" && webDriver.FindElements(documentBCAssessmentTypeRollLabel).Count > 0)
                AssertTrueContentEquals(documentViewRollContent, document.Roll);
            
            if (document.Section != "" && webDriver.FindElements(documentHistoricFileTypeSectionLabel).Count > 0)
                AssertTrueContentEquals(documentViewSectionContent, document.Section);
            
            if (document.ShortDescriptor != "" && webDriver.FindElements(documentShortDescriptorLabel).Count > 0)
                AssertTrueContentEquals(documentViewShortDescriptorContent, document.ShortDescriptor);
            
            if (document.StartDate != "" && webDriver.FindElements(documentHistoricFileTypeStartDateLabel).Count > 0)
                AssertTrueContentEquals(documentViewStartDateContent, TranformFormatDateDocument(document.StartDate));
            
            if (document.Title != "" && webDriver.FindElements(documentTitleSearchTypeTitleLabel).Count > 0)
                AssertTrueContentEquals(documentViewTitleContent, document.Title);
            
            if (document.Transfer != "" && webDriver.FindElements(documentTransferAdmTypeTransferLabel).Count > 0)
                AssertTrueContentEquals(documentViewTransferContent, document.Transfer);
            
            if (document.Year != "" && webDriver.FindElements(documentYearLabel).Count > 0)
                AssertTrueContentEquals(documentViewYearContent, document.Year);
            
            if (document.YearPrivyCouncil != "" && webDriver.FindElements(documentPrivyCouncilTypePrivyLabel).Count > 0)
                AssertTrueContentEquals(documentViewYearPrivyCouncilContent, document.YearPrivyCouncil);
        }

        //public void VerifyDocumentDetailsUpdateViewForm(DigitalDocument document)
        //{
        //    WaitUntilClickable(documentEditBttn);

        //    //Header
        //    AssertTrueIsDisplayed(documentViewDocumentTypeLabel);

        //    AssertTrueContentNotEquals(documentViewDocumentTypeContent, "");
        //    AssertTrueIsDisplayed(documenyViewDocumentNameLabel);
        //    AssertTrueContentNotEquals(documentViewFileNameContent, "");
        //    AssertTrueIsDisplayed(documentViewDownloadButton);

        //    //Document Information
        //    //AssertTrueIsDisplayed(documentUploadDocInfoSubtitle);
        //    AssertTrueIsDisplayed(documentViewDocumentInfoTooltip);
        //    AssertTrueIsDisplayed(documentEditBttn);
        //    //AssertTrueIsDisplayed(documentUploadStatusLabel);
        //    AssertTrueContentEquals(documentViewStatusContent, document.DocumentStatus);

        //    //Document Details
        //    Assert.True(webDriver.FindElement(documentViewDetailsSubtitle).Displayed);

        //    if (document.CanadaLandSurvey != "" && webDriver.FindElements(documentCanLandSurveyTypeCanLandSurveyLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewCanadaLandSurveyContent, document.CanadaLandSurvey);
            
        //    if (document.CivicAddress != "" && webDriver.FindElements(documentCivicAddressLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewCivicAddressContent, document.CivicAddress);
            
        //    if (document.CrownGrant != "" && webDriver.FindElements(documentCrownGrantTypeCrownLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewCrownGrantContent,document.CrownGrant);
            
        //    if (document.Date != "" && webDriver.FindElements(documentPhotosCorrespondenceTypeDateLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewDateContent, TranformFormatDateDocument(document.Date));
            
        //    if (document.DateSigned != "" && webDriver.FindElements(documentDateSignedLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewDateSignedContent, TranformFormatDateDocument(document.DateSigned));
            
        //    if (document.DistrictLot != "" && webDriver.FindElements(documentFieldNotesTypeDistrictLotLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewDistrictLotContent, document.DistrictLot);
            
        //    if (document.ElectoralDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeElectoralDistrictLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewElectoralDistrictContent, document.ElectoralDistrict);
            
        //    if (document.EndDate != "" && webDriver.FindElements(documentHistoricFileTypeEndDateLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewEndDateContent, TranformFormatDateDocument(document.EndDate));
            
        //    if (document.FieldBook != "" && webDriver.FindElements(documentFieldNotesTypeYearLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewFieldBookContent, document.FieldBook);
            
        //    if (document.File != "" && webDriver.FindElements(documentHistoricFileTypeFileLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewFileNumberContent, document.File);
            
        //    if (document.GazetteDate != "" && webDriver.FindElements(documentGazetteDateLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewGazetteDateContent, TranformFormatDateDocument(document.GazetteDate));
            
        //    if (document.GazettePage != "" && webDriver.FindElements(documentGazettePageLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewGazettePageContent, document.GazettePage);
            
        //    if (document.GazettePublishedDate != "" && webDriver.FindElements(documentGazettePublishedDateLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewGazettePublishedDateContent, TranformFormatDateDocument(document.GazettePublishedDate));
            
        //    if (document.GazetteType != "" && webDriver.FindElements(documentGazettePublishedTypeLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewGazettePublishedTypeContent, document.GazetteType);
            
        //    if (document.HighwayDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeHighwayDistrictLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewGazetteHighwayDistrictContent, document.HighwayDistrict);
            
        //    if (document.IndianReserveOrNationalPark != "" && webDriver.FindElements(documentCanLandSurveyTypeIndianReserveLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewIndianReserveContent, document.IndianReserveOrNationalPark);
            
        //    if (document.Jurisdiction != "" && webDriver.FindElements(documentBCAssessmentTypeJurisdictionLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewJurisdictionContent, document.Jurisdiction);
            
        //    if (document.LandDistrict != "" && webDriver.FindElements(documentFieldNotesTypeLandDistrictLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewLandDistrictContent, document.LandDistrict);
            
        //    if (document.LegalSurveyPlan != "" && webDriver.FindElements(documentLegalSurveyNbrLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewLegalSurveyPlanContent, document.LegalSurveyPlan);
            
        //    if (document.LTSAScheduleFiling != "" && webDriver.FindElements(documentGazetteLTSALabel).Count > 0)
        //        AssertTrueContentEquals(documentViewLTSAScheduleFilingContent, document.LTSAScheduleFiling);
            
        //    if (document.MO != "" && webDriver.FindElements(documentMinisterialOrderTypeMOLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewMOContent, document.MO);
            
        //    if (document.MoTIFile != "" && webDriver.FindElements(documentMOTIFileLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewMotiFileContent, document.MoTIFile);
            
        //    if (document.MoTIPlan != "" && webDriver.FindElements(documentMOTIPlanLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewMotiPlanContent, document.MoTIPlan);
            
        //    if (document.OIC != "" && webDriver.FindElements(documentOICTypeOICLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewOICNumberContent, document.OIC);
            
        //    if (document.OICRoute != "" && webDriver.FindElements(documentOICTypeOICRouteLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewOICRouteContent, document.OICRoute);
            
        //    if (document.OICType != "" && webDriver.FindElements(documentOICTypeOICTypeLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewOICTypeContent, document.OICType);
            
        //    if (document.Owner != "" && webDriver.FindElements(documentOwnerLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewOwnerContent, document.Owner);
            
        //    if (document.PhysicalLocation != "" && webDriver.FindElements(documentHistoricFileTypePhyLocationLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPhysicalLocationContent, document.PhysicalLocation);
            
        //    if (document.PIDNumber != "" && webDriver.FindElements(documentViewPIDLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPIDContent, document.PIDNumber);
            
        //    if (document.PINNumber != "" && webDriver.FindElements(documentOtherTypePINLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPINContent, document.PINNumber);
            
        //    if (document.Plan != "" && webDriver.FindElements(documentPAPlanNbrLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPlanNumberContent, document.Plan);
            
        //    if (document.PlanRevision != "" && webDriver.FindElements(documentPAPlanRevisionLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPlanRevisionContent, document.PlanRevision);
            
        //    if (document.PlanType != "" && webDriver.FindElements(documentLegalSurveyPlanTypeLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPlanTypeContent, document.PlanType);
            
        //    if (document.Project != "" && webDriver.FindElements(documentPAPlanProjectLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewProjectNumberContent, document.Project);
            
        //    if (document.ProjectName != "" && webDriver.FindElements(documentViewProjectLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewProjectContent, document.ProjectName);
            
        //    if (document.PropertyIdentifier != "" && webDriver.FindElements(documentViewPropertyIdentifierLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPropertyIdentifierContent, document.PropertyIdentifier);
            
        //    if (document.PublishedDate != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyPublishDateLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewPublishedDateContent,TranformFormatDateDocument(document.PublishedDate));
            
        //    if (document.RelatedGazette != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyRelatedGazetteLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewRelatedGazetteContent, document.RelatedGazette);
            
        //    if (document.RoadName != "" && webDriver.FindElements(documentRoadNameLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewRoadNameContent, document.RoadName);
            
        //    if (document.Roll != "" && webDriver.FindElements(documentBCAssessmentTypeRollLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewRollContent, document.Roll);
            
        //    if (document.Section != "" && webDriver.FindElements(documentHistoricFileTypeSectionLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewSectionContent, document.Section);
            
        //    if (document.ShortDescriptor != "" && webDriver.FindElements(documentShortDescriptorLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewShortDescriptorContent, document.ShortDescriptor);
            
        //    if (document.StartDate != "" && webDriver.FindElements(documentHistoricFileTypeStartDateLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewStartDateContent, TranformFormatDateDocument(document.StartDate));
            
        //    if (document.Title != "" && webDriver.FindElements(documentTitleSearchTypeTitleLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewTitleContent, document.Title);
            
        //    if (document.Transfer != "" && webDriver.FindElements(documentTransferAdmTypeTransferLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewTransferContent, document.Transfer);
            
        //    if (document.Year != "" && webDriver.FindElements(documentYearLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewYearContent, document.Year);
            
        //    if (document.YearPrivyCouncil != "" && webDriver.FindElements(documentPrivyCouncilTypePrivyLabel).Count > 0)
        //        AssertTrueContentEquals(documentViewYearPrivyCouncilContent, document.YearPrivyCouncil);
        //}

        public void VerifyInitUploadDocumentForm()
        {
            WaitUntilVisible(documentsUploadHeader);

            AssertTrueIsDisplayed(documentsUploadHeader);
            AssertTrueIsDisplayed(documentUploadInstructionsLabel);
            AssertTrueIsDisplayed(documentUploadDragDropArea);
            WaitUntilExist(documentUploadDocInput);
        }

        private void VerifyGeneralUpdateDocumentForm()
        {
            WaitUntilVisible(documentsUploadHeader);

            AssertTrueIsDisplayed(documentsUploadHeader);
            AssertTrueIsDisplayed(documentViewDocumentTypeLabel);
            AssertTrueIsDisplayed(documentViewDocumentTypeContent);
            AssertTrueIsDisplayed(documenyViewDocumentNameLabel);
            AssertTrueIsDisplayed(documentViewFileNameContent);

            AssertTrueIsDisplayed(documentViewInfoSubtitle);
            AssertTrueIsDisplayed(documentUploadStatusSelect);
            AssertTrueIsDisplayed(documentUploadStatusSelect);

            AssertTrueIsDisplayed(documentUpdateDetailsSubtitle);
        }

        private void VerifyOtherTypeFields()
        {
            WaitUntilVisible(documentOtherTypePINLabel);

            AssertTrueIsDisplayed(documentOtherTypePINLabel);
            AssertTrueIsDisplayed(documentOtherTypePINInput);
            AssertTrueIsDisplayed(documentOtherTypePropIdLabel);
            AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);
            AssertTrueIsDisplayed(documentRoadNameLabel);
            AssertTrueIsDisplayed(documentRoadNameInput);
            AssertTrueIsDisplayed(documentShortDescriptorLabel);
            AssertTrueIsDisplayed(documentShortDescriptorInput);
        }

        private void VerifyFieldNotesFields()
        {
            WaitUntilVisible(documentFieldNotesTypeDistrictLotLabel);

            AssertTrueIsDisplayed(documentFieldNotesTypeDistrictLotLabel);
            AssertTrueIsDisplayed(documentFieldNotesTypeDistrictLotInput);
            AssertTrueIsDisplayed(documentFieldNotesTypeYearLabel);
            AssertTrueIsDisplayed(documentFieldNotesTypeYearInput);
            AssertTrueIsDisplayed(documentFieldNotesTypeLandDistrictLabel);
            AssertTrueIsDisplayed(documentFieldNotesTypeLandDistrictInput); 
        }

        private void VerifyDistrictRoadRegisterFields()
        {
            WaitUntilVisible(documentDistrictRoadRegisterTypeElectoralDistrictLabel);

            AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeElectoralDistrictLabel);
            AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeElectoralDistrictInput);
            AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeHighwayDistrictLabel);
            AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeHighwayDistrictInput);
            AssertTrueIsDisplayed(documentRoadNameLabel);
            AssertTrueIsDisplayed(documentRoadNameInput);
        }

        private void VerifyBCAssessmentFields()
        {
            WaitUntilVisible(documentCivicAddressLabel);

            AssertTrueIsDisplayed(documentCivicAddressLabel);
            AssertTrueIsDisplayed(documentCivicAddressInput);
            //webDriver.FindElement(documentCivicAddressInput).Click();
            //webDriver.FindElement(documentCivicAddressLabel).Click();
            //AssertTrueIsDisplayed(documentBCAssessmentTypeAddressMandatory);

            AssertTrueIsDisplayed(documentBCAssessmentTypeJurisdictionLabel);
            AssertTrueIsDisplayed(documentBCAssessmentTypeJurisdictionInput);
            //webDriver.FindElement(documentBCAssessmentTypeJurisdictionInput).Click();
            //webDriver.FindElement(documentBCAssessmentTypeJurisdictionLabel).Click();
            //AssertTrueIsDisplayed(documentBCAssessmentTypeJurisdictionMandatory);

            AssertTrueIsDisplayed(documentBCAssessmentTypeRollLabel);
            AssertTrueIsDisplayed(documentBCAssessmentTypeRollInput);

            AssertTrueIsDisplayed(documentYearLabel);
            AssertTrueIsDisplayed(documentYearInput);
            //webDriver.FindElement(documentYearInput).Click();
            //webDriver.FindElement(documentYearLabel).Click();
            //AssertTrueIsDisplayed(documentBCAssessmentTypeYearMandatory);
        }

        private void VerifyTransferAdministrationFields()
        {
            WaitUntilVisible(documentDateSignedLabel);

            AssertTrueIsDisplayed(documentDateSignedLabel);
            AssertTrueIsDisplayed(documentDateSignedInput);

            AssertTrueIsDisplayed(documentMOTIFileLabel);
            AssertTrueIsDisplayed(documentTypeMotiFileInput);
            //webDriver.FindElement(documentTypeMotiFileInput).Click();
            //webDriver.FindElement(documentMOTIFileLabel).Click();
            //AssertTrueIsDisplayed(documentTransferAdmTypeMOTIFileMandatory);

            AssertTrueIsDisplayed(documentTransferAdmTypeProIdLabel);
            AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);

            AssertTrueIsDisplayed(documentRoadNameLabel);
            AssertTrueIsDisplayed(documentRoadNameInput);
            //webDriver.FindElement(documentRoadNameInput).Click();
            //webDriver.FindElement(documentRoadNameLabel).Click();
            //AssertTrueIsDisplayed(documentTransferAdmTypeRoadNameMandatory);

            AssertTrueIsDisplayed(documentTransferAdmTypeTransferLabel);
            AssertTrueIsDisplayed(documentTransferAdmTypeTransferInput);
            //webDriver.FindElement(documentTransferAdmTypeTransferInput).Click();
            //webDriver.FindElement(documentTransferAdmTypeTransferLabel).Click();
            //AssertTrueIsDisplayed(documentTransferAdmTypeTransferMandatory);
        }

        private void VerifyMinisterialOrderFields()
        {
            WaitUntilVisible(documentDateSignedLabel);

            AssertTrueIsDisplayed(documentDateSignedLabel);
            AssertTrueIsDisplayed(documentDateSignedInput);
            AssertTrueIsDisplayed(documentMinisterialOrderTypeMOLabel);
            AssertTrueIsDisplayed(documentMinisterialOrderTypeMOInput);
            AssertTrueIsDisplayed(documentMOTIFileLabel);
            AssertTrueIsDisplayed(documentTypeMotiFileInput);
            AssertTrueIsDisplayed(documentPropertyIdentifierLabel);
            AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);
            AssertTrueIsDisplayed(documentRoadNameLabel);
            AssertTrueIsDisplayed(documentRoadNameInput);
        }

        private void VerifyCanadaLandsSurveyFields()
        {
            WaitUntilVisible(documentCanLandSurveyTypeCanLandSurveyLabel);

            AssertTrueIsDisplayed(documentCanLandSurveyTypeCanLandSurveyLabel);
            AssertTrueIsDisplayed(documentCanLandSurveyTypeCanLandSurveyInput);
            //webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyInput).Click();
            //webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyLabel).Click();
            //AssertTrueIsDisplayed(documentCanLandSurveyTypeCanLandSurveyMandatory);

            AssertTrueIsDisplayed(documentCanLandSurveyTypeIndianReserveLabel);
            AssertTrueIsDisplayed(documentCanLandSurveyTypeIndianReserveInput);
            //webDriver.FindElement(documentCanLandSurveyTypeIndianReserveInput).Click();
            //webDriver.FindElement(documentCanLandSurveyTypeIndianReserveLabel).Click();
            //AssertTrueIsDisplayed(documentCanLandSurveyTypeIndianReserveMandatory);
        }

        private void VerifyPhotosCorrespondenceFields()
        {
            WaitUntilVisible(documentCivicAddressLabel);

            AssertTrueIsDisplayed(documentCivicAddressLabel);
            AssertTrueIsDisplayed(documentCivicAddressInput);
            AssertTrueIsDisplayed(documentPhotosCorrespondenceTypeDateLabel);
            AssertTrueIsDisplayed(documentPhotosCorrespondenceTypeDateInput);
            AssertTrueIsDisplayed(documentOwnerLabel);
            AssertTrueIsDisplayed(documentTypeOwnerInput);
            AssertTrueIsDisplayed(documentPhotosCorrespondenceTypePropIdLabel);
            AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);
            AssertTrueIsDisplayed(documentShortDescriptorLabel);
            AssertTrueIsDisplayed(documentShortDescriptorInput);
        }

        private void VerifyMiscellaneousNotesFields()
        {
            WaitUntilVisible(documentMiscNotesTypePIDLabel);

            AssertTrueIsDisplayed(documentMiscNotesTypePIDLabel);
            AssertTrueIsDisplayed(documentMiscNotesTypePIDInput);
        }

        private void VerifyTitleSearchFields()
        {
            WaitUntilVisible(documentOwnerLabel);

            AssertTrueIsDisplayed(documentOwnerLabel);
            AssertTrueIsDisplayed(documentTypeOwnerInput);
            AssertTrueIsDisplayed(documentTitleSearchTypePIDLabel);
            AssertTrueIsDisplayed(documentTitleSearchTypePIDInput);
            AssertTrueIsDisplayed(documentTitleSearchTypeTitleLabel);
            AssertTrueIsDisplayed(documentTitleSearchTypeTitleInput);
        }

        private void VerifyHistoricalFileFields()
        {
            WaitUntilVisible(documentHistoricFileTypeEndDateLabel);

            AssertTrueIsDisplayed(documentHistoricFileTypeEndDateLabel);
            AssertTrueIsDisplayed(documentHistoricFileTypeEndDateInput);

            AssertTrueIsDisplayed(documentHistoricFileTypeFileLabel);
            AssertTrueIsDisplayed(documentHistoricFileTypeFileInput);
            //webDriver.FindElement(documentHistoricFileTypeFileInput).Click();
            //webDriver.FindElement(documentHistoricFileTypeFileLabel).Click();
            //AssertTrueIsDisplayed(documentHistoricFileTypeFileMandatory);

            AssertTrueIsDisplayed(documentHistoricFileTypePhyLocationLabel);
            AssertTrueIsDisplayed(documentHistoricFileTypePhyLocationInput);
            AssertTrueIsDisplayed(documentHistoricFileTypeSectionLabel);
            AssertTrueIsDisplayed(documentHistoricFileTypeSectionInput);
            AssertTrueIsDisplayed(documentHistoricFileTypeStartDateLabel);
            AssertTrueIsDisplayed(documentHistoricFileTypeStartDateInput);
        }

        private void VerifyCrownGrantFields()
        {
            WaitUntilVisible(documentCrownGrantTypeCrownLabel);

            AssertTrueIsDisplayed(documentCrownGrantTypeCrownLabel);
            AssertTrueIsDisplayed(documentCrownGrantTypeCrownInput);
            //webDriver.FindElement(documentCrownGrantTypeCrownInput).Click();
            //webDriver.FindElement(documentCrownGrantTypeCrownLabel).Click();
            //AssertTrueIsDisplayed(documentCrownGrantTypeCrownMandatory);
        }

        private void VerifyPrivyCouncilFields()
        {
            WaitUntilVisible(documentPrivyCouncilTypePrivyLabel);

            AssertTrueIsDisplayed(documentPrivyCouncilTypePrivyLabel);
            AssertTrueIsDisplayed(documentPrivyCouncilTypePrivyInput);
            //webDriver.FindElement(documentPrivyCouncilTypePrivyInput).Click();
            //webDriver.FindElement(documentPrivyCouncilTypePrivyLabel).Click();
            //AssertTrueIsDisplayed(documentPrivyCounciltTypePrivyMandatory);
        }

        private void VerifyOICFields()
        {
            WaitUntilVisible(documentOICTypeOICLabel);

            AssertTrueIsDisplayed(documentOICTypeOICLabel);
            AssertTrueIsDisplayed(documentOICTypeInput);
            AssertTrueIsDisplayed(documentOICTypeOICRouteLabel);
            AssertTrueIsDisplayed(documentOICTypeOICRouteInput);
            AssertTrueIsDisplayed(documentOICTypeOICTypeLabel);
            AssertTrueIsDisplayed(documentOICTypeOICTypeInput);
            AssertTrueIsDisplayed(documentRoadNameLabel);
            AssertTrueIsDisplayed(documentRoadNameInput);
            AssertTrueIsDisplayed(documentYearLabel);
            AssertTrueIsDisplayed(documentYearInput);
        }

        private void VerifyLegalSurveyFields()
        {
            WaitUntilVisible(documentLegalSurveyNbrLabel);

            AssertTrueIsDisplayed(documentLegalSurveyNbrLabel);
            AssertTrueIsDisplayed(documentLegalSurveyInput);
            AssertTrueIsDisplayed(documentMOTIPlanLabel);
            AssertTrueIsDisplayed(documentMOTIPlanInput);
            AssertTrueIsDisplayed(documentLegalSurveyPlanTypeLabel);
            AssertTrueIsDisplayed(documentLegalSurveyPlanTypeInput);
        }

        private void VerifyMOTIPlanFields()
        {
            WaitUntilVisible(documentLegalSurveyNbrLabel);

            AssertTrueIsDisplayed(documentLegalSurveyNbrLabel);
            AssertTrueIsDisplayed(documentLegalSurveyInput);
            AssertTrueIsDisplayed(documentMOTIFileLabel);
            AssertTrueIsDisplayed(documentTypeMotiFileInput);
            AssertTrueIsDisplayed(documentMOTIPlanLabel);
            AssertTrueIsDisplayed(documentMOTIPlanInput);
            AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyPublishDateLabel);
            AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyPublishDateInput);
            AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyRelatedGazetteLabel);
            AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyRelatedGazetteInput);
        }

        private void VerifyGazetteFields()
        {
            WaitUntilVisible(documentGazetteDateLabel);

            AssertTrueIsDisplayed(documentGazetteDateLabel);
            AssertTrueIsDisplayed(documentGazetteDateInput);
            AssertTrueIsDisplayed(documentGazettePageLabel);
            AssertTrueIsDisplayed(documentGazettePageInput);

            AssertTrueIsDisplayed(documentGazettePublishedDateLabel);
            AssertTrueIsDisplayed(documentGazettePublishedDateInput);
            //webDriver.FindElement(documentGazettePublishedDateInput).Click();
            //webDriver.FindElement(documentGazettePublishedDateLabel).Click();
            //AssertTrueIsDisplayed(documentGazettePublishedDateMandatory);

            AssertTrueIsDisplayed(documentGazettePublishedTypeLabel);
            AssertTrueIsDisplayed(documentGazettePublishedTypeInput);
            //webDriver.FindElement(documentGazettePublishedTypeInput).Click();
            //webDriver.FindElement(documentGazettePublishedTypeLabel).Click();
            //AssertTrueIsDisplayed(documentGazettePublishedTypeMandatory);

            AssertTrueIsDisplayed(documentGazetteLegalSurveyPlanLabel);
            AssertTrueIsDisplayed(documentLegalSurveyInput);
            AssertTrueIsDisplayed(documentGazetteLTSALabel);
            AssertTrueIsDisplayed(documentGazetteLTSAInput);
            AssertTrueIsDisplayed(documentGazetteLegalSurveyMotiPlanLabel);
            AssertTrueIsDisplayed(documentMOTIPlanInput);

            AssertTrueIsDisplayed(documentRoadNameLabel);
            AssertTrueIsDisplayed(documentRoadNameInput);
            //webDriver.FindElement(documentRoadNameInput).Click();
            //webDriver.FindElement(documentRoadNameLabel).Click();
            //AssertTrueIsDisplayed(documentGazetteRoadNameMandatory);
        }

        private void VerifyPAPlansFields()
        {
            WaitUntilVisible(documentPAPlanNbrLabel);

            AssertTrueIsDisplayed(documentPAPlanNbrLabel);
            AssertTrueIsDisplayed(documentPAPlanNbrInput);
            //webDriver.FindElement(documentPAPlanNbrInput).Click();
            //webDriver.FindElement(documentPAPlanNbrLabel).Click();
            //AssertTrueIsDisplayed(documentPAPlanNbrMandatory);

            AssertTrueIsDisplayed(documentPAPlanRevisionLabel);
            AssertTrueIsDisplayed(documentPAPlanRevisionInput);
            AssertTrueIsDisplayed(documentPAPlanProjectLabel);
            AssertTrueIsDisplayed(documentPAPlanProjectInput);

            AssertTrueIsDisplayed(documentPAPlanProjectNameLabel);
            AssertTrueIsDisplayed(documentPAPlanProjectNameInput);
            //webDriver.FindElement(documentPAPlanProjectNameInput).Click();
            //webDriver.FindElement(documentPAPlanProjectNameLabel).Click();
            //AssertTrueIsDisplayed(documentPAPlanProjectNameMandatory);
        }

        private void VerifyShortDescriptorField()
        {
            WaitUntilVisible(documentShortDescriptorLabel);

            AssertTrueIsDisplayed(documentShortDescriptorLabel);
            AssertTrueIsDisplayed(documentShortDescriptorInput);
        }

        private string TranformFormatDateDocument(string date)
        {
            if (date == "")
            {
                return "";
            }
            else
            {
                var dateObject = DateTime.Parse(date);
                return dateObject.ToString("G");
            }
        }
    }
}
