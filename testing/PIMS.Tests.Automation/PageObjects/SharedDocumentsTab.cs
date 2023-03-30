using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedDocumentsTab : PageObjectBase
    {
        //Documents Tab Element
        private By documentsTab = By.CssSelector("a[data-rb-event-key='documents']");

        //Activity Documents List Header
        //private By documentsTitle = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/h2/div/div/div/div[contains(text(),'Documents')]");
        //private By addDocumentBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/h2/div/div/div/div[contains(text(),'Documents')]/following-sibling::div/button");

        //Documents Tab List Header
        private By documentsTitle = By.XPath("//div[contains(text(),'File Documents')]");
        private By addDocumentBttn = By.XPath("//div[contains(text(),'File Documents')]/following-sibling::div/button");

        //Upload Documents Dialog General Elements
        private By documentsUploadHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4'] span");
        private By documentUploadInstructions = By.CssSelector("div[class='modal-body'] div div[class='pb-4']");
        private By documentUploadDocTypeLabel = By.XPath("//label[contains(text(),'Document type')]");
        private By documentUploadDocTypeModalSelect = By.XPath("//div[@class='modal-body']/div/div[2]/div[2]/div/select[@id='input-documentTypeId']");
        private By documentUploadDocInput = By.Id("uploadInput");
        private By documentUploadDocInfoSubtitle = By.XPath("//h2[contains(text(),'Document information')]");
        private By documentUploadStatusLabel = By.XPath("//div[@class='pb-2 row']/div/label[contains(text(),'Status')]");
        private By documentUploadStatusSelect = By.Id("input-documentStatusCode");
        private By documentUploadDetailsSubtitle = By.XPath("//h3[contains(text(),'Details')]");

        //Upload Documents Other Type Fields
        private By documentOtherTypePINLabel = By.XPath("//div[@class='pb-2 row'][1]/div/label[contains(text(),'PIN')]");
        private By documentOtherTypePINInput = By.Id("input-documentMetadata.71");
        private By documentOtherTypePropIdLabel = By.XPath("//div[@class='pb-2 row'][2]/div/label[contains(text(),'Property identifier')]");
        private By documentOtherTypePropIdInput = By.Id("input-documentMetadata.94");
        private By documentOtherTypeRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private By documentOtherTypeRoadNameInput = By.Id("input-documentMetadata.75");
        private By documentOtherTypeDescriptionLabel = By.XPath("//label[contains(text(),'Short descriptor')]");
        private By documentOtherTypeDescriptionInput = By.Id("input-documentMetadata.55");

        //Upload Documents Field Notes Type Fields
        private By documentFieldNotesTypeDistrictLotLabel = By.XPath("//label[contains(text(),'District lot')]");
        private By documentFieldNotesTypeDistrictLotInput = By.Id("input-documentMetadata.101");
        private By documentFieldNotesTypeYearLabel = By.XPath("//label[contains(text(),'Field book #/Year')]");
        private By documentFieldNotesTypeYearInput = By.Id("input-documentMetadata.104");
        private By documentFieldNotesTypeLandDistrictLabel = By.XPath("//label[contains(text(),'Land district')]");
        private By documentFieldNotesTypeLandDistrictInput = By.Id("input-documentMetadata.99");

        //Upload Documents District Road Register Fields
        private By documentDistrictRoadRegisterTypeElectoralDistrictLabel = By.XPath("//label[contains(text(),'Electoral district')]");
        private By documentDistrictRoadRegisterTypeElectoralDistrictInput = By.Id("input-documentMetadata.102");
        private By documentDistrictRoadRegisterTypeHighwayDistrictLabel = By.XPath("//label[contains(text(),'Highway district')]");
        private By documentDistrictRoadRegisterTypeHighwayDistrictInput = By.Id("input-documentMetadata.103");
        private By documentDistrictRoadRegisterTypeRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private By documentDistrictRoadRegisterTypeRoadNameInput = By.Id("input-documentMetadata.75");

        //Upload BC Assessment Search Type Fields
        private By documentBCAssessmentTypeAddressLabel = By.XPath("//label[contains(text(),'Civic address')]");
        private By documentBCAssessmentTypeAddressInput = By.Id("input-documentMetadata.96");
        private By documentBCAssessmentTypeAddressMandatory = By.XPath("//div[contains(text(),'Civic address is required')]");
        private By documentBCAssessmentTypeJurisdictionLabel = By.XPath("//label[contains(text(),'Jurisdiction')]");
        private By documentBCAssessmentTypeJurisdictionInput = By.Id("input-documentMetadata.67");
        private By documentBCAssessmentTypeJurisdictionMandatory = By.XPath("//div[contains(text(),'Jurisdiction is required')]");
        private By documentBCAssessmentTypeRollLabel = By.XPath("//label[contains(text(),'Roll')]");
        private By documentBCAssessmentTypeRollInput = By.Id("input-documentMetadata.63");
        private By documentBCAssessmentTypeYearLabel = By.XPath("//label[contains(text(),'Year')]");
        private By documentBCAssessmentTypeYearInput = By.Id("input-documentMetadata.48");
        private By documentBCAssessmentTypeYearMandatory = By.XPath("//div[contains(text(),'Year is required')]");

        //Upload Transfer of Administration Type Fields
        private By documentTransferAdmTypeDateLabel = By.XPath("//label[contains(text(),'Date signed')]");
        private By documentTransferAdmTypeDateInput = By.Id("input-documentMetadata.73");
        private By documentTransferAdmTypeMOTIFileLabel = By.XPath("//label[contains(text(),'MoTI file')]");
        private By documentTransferAdmTypeMOTIFileInput = By.Id("input-documentMetadata.87");
        private By documentTransferAdmTypeMOTIFileMandatory = By.XPath("//div[contains(text(),'MoTI file # is required')]");
        private By documentTransferAdmTypeProIdLabel = By.XPath("//label[contains(text(),'Property identifier')]");
        private By documentTransferAdmTypePropIdInput = By.Id("input-documentMetadata.100");
        private By documentTransferAdmTypeRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private By documentTransferAdmTypeRoadNameInput = By.Id("input-documentMetadata.75");
        private By documentTransferAdmTypeRoadNameMandatory = By.XPath("//div[contains(text(),'Road name is required')]");
        private By documentTransferAdmTypeTransferLabel = By.XPath("//label[contains(text(),'Transfer')]");
        private By documentTransferAdmTypeTransferInput = By.Id("input-documentMetadata.66");
        private By documentTransferAdmTypeTransferMandatory = By.XPath("//div[contains(text(),'Transfer # is required')]");

        //Upload Ministerial Order Type Fields
        private By documentMinisterialOrderTypeDateSignedLabel = By.XPath("//label[contains(text(),'Date signed')]");
        private By documentMinisterialOrderTypeDateSignedInput = By.Id("input-documentMetadata.73");
        private By documentMinisterialOrderTypeMOLabel = By.XPath("//label[contains(text(),'MO #')]");
        private By documentMinisterialOrderTypeMOInput = By.Id("input-documentMetadata.70");
        private By documentMinisterialOrderTypeMotiFileLabel = By.XPath("//label[contains(text(),'MoTI file')]");
        private By documentMinisterialOrderTypeMotiFileInput = By.Id("input-documentMetadata.87");
        private By documentMinisterialOrderTypePropIdtLabel = By.XPath("//label[contains(text(),'Property identifier')]");
        private By documentMinisterialOrderTypePropIdInput = By.Id("input-documentMetadata.100");
        private By documentMinisterialOrderTypeRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private By documentMinisterialOrderTypeRoadNameInput = By.Id("input-documentMetadata.75");

        //Upload Canada Lands Survey Fields
        private By documentCanLandSurveyTypeCanLandSurveyLabel = By.XPath("//label[contains(text(),'Canada land survey')]");
        private By documentCanLandSurveyTypeCanLandSurveyInput = By.Id("input-documentMetadata.97");
        private By documentCanLandSurveyTypeCanLandSurveyMandatory = By.XPath("//div[contains(text(),'Canada land survey # is required')]");
        private By documentCanLandSurveyTypeIndianReserveLabel = By.XPath("//label[contains(text(),'Indian reserve')]");
        private By documentCanLandSurveyTypeIndianReserveInput = By.Id("input-documentMetadata.98");
        private By documentCanLandSurveyTypeIndianReserveMandatory = By.XPath("//div[contains(text(),'Indian reserve or national park is required')]");

        //Upload Photos/Images/Video and Correspondence Fields
        private By documentPhotosCorrespondenceTypeAddressLabel = By.XPath("//label[contains(text(),'Civic address')]");
        private By documentPhotosCorrespondenceTypeAddressInput = By.Id("input-documentMetadata.96");
        private By documentPhotosCorrespondenceTypeDateLabel = By.XPath("//label[contains(text(),'Date')]");
        private By documentPhotosCorrespondenceTypeDateInput = By.Id("input-documentMetadata.57");
        private By documentPhotosCorrespondenceTypeOwnerLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Owner')]");
        private By documentPhotosCorrespondenceTypeOwnerInput = By.Id("input-documentMetadata.51");
        private By documentPhotosCorrespondenceTypePropIdLabel = By.XPath("//label[contains(text(),'Property identifier')]");
        private By documentPhotosCorrespondenceTypePropIdInput = By.Id("input-documentMetadata.94");
        private By documentPhotosCorrespondenceTypeDescriptionLabel = By.XPath("//label[contains(text(),'Short descriptor')]");
        private By documentPhotosCorrespondenceTypeDescriptionInput = By.Id("input-documentMetadata.55");

        //Upload Miscellaneous notes (LTSA) Fields
        private By documentMiscNotesTypePIDLabel = By.XPath("//input[@id='input-documentMetadata.62']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'PID')]");
        private By documentMiscNotesTypePIDInput = By.Id("input-documentMetadata.62");

        //Upload Title search/ Historical title Fields
        private By documentTitleSearchTypeOwnerLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Owner')]");
        private By documentTitleSearchTypeOwnerInput = By.Id("input-documentMetadata.51");
        private By documentTitleSearchTypePIDLabel = By.XPath("//input[@id='input-documentMetadata.62']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'PID')]");
        private By documentTitleSearchTypePIDInput = By.Id("input-documentMetadata.62");
        private By documentTitleSearchTypeTitleLabel = By.XPath("//label[contains(text(),'Title')]");
        private By documentTitleSearchTypeTitleInput = By.Id("input-documentMetadata.58");

        //Upload Historical File Fields
        private By documentHistoricFileTypeEndDateLabel = By.XPath("//label[contains(text(),'End date')]");
        private By documentHistoricFileTypeEndDateInput = By.Id("input-documentMetadata.93");
        private By documentHistoricFileTypeFileLabel = By.XPath("//div[@class='pr-0 text-left col-4']/label[contains(text(),'File #')]");
        private By documentHistoricFileTypeFileInput = By.Id("input-documentMetadata.42");
        private By documentHistoricFileTypeFileMandatory = By.XPath("//div[contains(text(),'File # is required')]");
        private By documentHistoricFileTypePhyLocationLabel = By.XPath("//label[contains(text(),'Physical location')]");
        private By documentHistoricFileTypePhyLocationInput = By.Id("input-documentMetadata.95");
        private By documentHistoricFileTypeSectionLabel = By.XPath("//label[contains(text(),'Section')]");
        private By documentHistoricFileTypeSectionInput = By.Id("input-documentMetadata.47");
        private By documentHistoricFileTypeStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private By documentHistoricFileTypeStartDateInput = By.Id("input-documentMetadata.91");

        //Upload Crown Grant Fields
        private By documentCrownGrantTypeCrownLabel = By.XPath("//label[contains(text(),'Crown grant #')]");
        private By documentCrownGrantTypeCrownInput = By.Id("input-documentMetadata.56");
        private By documentCrownGrantTypeCrownMandatory = By.XPath("//div[contains(text(),'Crown grant # is required')]");

        //Upload Privy Council Fields
        private By documentPrivyCouncilTypePrivyLabel = By.XPath("//label[contains(text(),'Year - privy council #')]");
        private By documentPrivyCouncilTypePrivyInput = By.Id("input-documentMetadata.92");
        private By documentPrivyCounciltTypePrivyMandatory = By.XPath("//div[contains(text(),'Year - privy council # is required')]");

        //Upload OIC Fields
        private By documentOICTypeOICLabel = By.XPath("//label[contains(text(),'OIC #')]");
        private By documentOICTypeOICInput = By.Id("input-documentMetadata.43");
        private By documentOICTypeOICRouteLabel = By.XPath("//label[contains(text(),'OIC route #')]");
        private By documentOICTypeOICRouteInput = By.Id("input-documentMetadata.90");
        private By documentOICTypeOICTypeLabel = By.XPath("//label[contains(text(),'OIC type')]");
        private By documentOICTypeOICTypeInput = By.Id("input-documentMetadata.89");
        private By documentOICTypeRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private By documentOICTypeRoadNameInput = By.Id("input-documentMetadata.75");
        private By documentOICTypeYearLabel = By.XPath("//label[contains(text(),'Year')]");
        private By documentOICTypeYearInput = By.Id("input-documentMetadata.48");

        //Upload Legal Survey Plans Fields
        private By documentLegalSurveyNbrLabel = By.XPath("//label[contains(text(),'Legal survey plan #')]");
        private By documentLegalSurveyNbrInput = By.Id("input-documentMetadata.84");
        private By documentLegalSurveyMOTIPlanLabel = By.XPath("//label[contains(text(),'MoTI plan #')]");
        private By documentLegalSurveyMOTIPlanInput = By.Id("input-documentMetadata.83");
        private By documentLegalSurveyPlanTypeLabel = By.XPath("//label[contains(text(),'Plan type')]");
        private By documentLegalSurveyPlanTyoeInput = By.Id("input-documentMetadata.88");

        //Upload MoTI Plan Fields
        private By documentMoTIPlanLegalSurveyPlanLabel = By.XPath("//label[contains(text(),'Legal survey plan #')]");
        private By documentMoTIPlanLegalSurveyPlanInput = By.Id("input-documentMetadata.84");
        private By documentMoTIPlanLegalSurveyMotiFileLabel = By.XPath("//label[contains(text(),'MoTI file #')]");
        private By documentMoTIPlanLegalSurveyMotiFileInput = By.Id("input-documentMetadata.87");
        private By documentMoTIPlanLegalSurveyMotiPlanLabel = By.XPath("//label[contains(text(),'MoTI plan #')]");
        private By documentMoTIPlanLegalSurveyMotiPlanInput = By.Id("input-documentMetadata.83");
        private By documentMoTIPlanLegalSurveyPublishDateLabel = By.XPath("//label[contains(text(),'Published date')]");
        private By documentMoTIPlanLegalSurveyPublishDateInput = By.Id("input-documentMetadata.86");
        private By documentMoTIPlanLegalSurveyRelatedGazetteLabel = By.XPath("//label[contains(text(),'Related gazette')]");
        private By documentMoTIPlanLegalSurveyRelatedGazetteInput = By.Id("input-documentMetadata.85");

        //Upload Gazette Fields
        private By documentGazetteDateLabel = By.XPath("//label[contains(text(),'Gazette date')]");
        private By documentGazetteDateInput = By.Id("input-documentMetadata.81");
        private By documentGazettePageLabel = By.XPath("//label[contains(text(),'Gazette page #')]");
        private By documentGazettePageInput = By.Id("input-documentMetadata.80");
        private By documentGazettePublishedDateLabel = By.XPath("//label[contains(text(),'Gazette published date')]");
        private By documentGazettePublishedDateInput = By.Id("input-documentMetadata.78");
        private By documentGazettePublishedDateMandatory = By.XPath("//div[contains(text(),'Gazette published date is required')]");
        private By documentGazettePublishedTypeLabel = By.XPath("//label[contains(text(),'Gazette type')]");
        private By documentGazettePublishedTypeInput = By.Id("input-documentMetadata.82");
        private By documentGazettePublishedTypeMandatory = By.XPath("//div[contains(text(),'Gazette type is required')]");
        private By documentGazetteLegalSurveyPlanLabel = By.XPath("//label[contains(text(),'Legal survey plan #')]");
        private By documentGazetteLegalSurveyPlanInput = By.Id("input-documentMetadata.84");
        private By documentGazetteLTSALabel = By.XPath("//label[contains(text(),'LTSA schedule filing')]");
        private By documentGazetteLTSAInput = By.Id("input-documentMetadata.39");
        private By documentGazetteLegalSurveyMotiPlanLabel = By.XPath("//label[contains(text(),'MoTI plan #')]");
        private By documentGazetteLegalSurveyMotiPlanInput = By.Id("input-documentMetadata.83");
        private By documentGazetteRoadNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Road name')]");
        private By documentGazetteRoadNameInput = By.Id("input-documentMetadata.75");
        private By documentGazetteRoadNameMandatory = By.XPath("//div[contains(text(),'Road name is required')]");

        //Upload PA plans Fields
        private By documentPAPlanNbrLabel = By.XPath("//label[contains(text(),'Plan #')]");
        private By documentPAPlanNbrInput = By.Id("input-documentMetadata.28");
        private By documentPAPlanNbrMandatory = By.XPath("//div[contains(text(),'Plan # is required')]");
        private By documentPAPlanRevisionLabel = By.XPath("//label[contains(text(),'Plan revision')]");
        private By documentPAPlanRevisionInput = By.Id("input-documentMetadata.79");
        private By documentPAPlanProjectLabel = By.XPath("//label[contains(text(),'Project #')]");
        private By documentPAPlanProjectInput = By.Id("input-documentMetadata.31");
        private By documentPAPlanProjectNameLabel = By.XPath("//input[@id='input-documentMetadata.77']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Project name')]");
        private By documentPAPlanProjectNameInput = By.Id("input-documentMetadata.77");
        private By documentPAPlanProjectNameMandatory = By.XPath("//div[contains(text(),'Project name is required')]");

        //Edit Upload Elements
        private By documentEditDocumentTypeLabel = By.XPath("//div[@class='modal-body']/div/div/div/label[contains(text(),'Document type')]");
        private By documentEditDocumentTypeContent = By.XPath("//div[@class='modal-body']/div/div/div/label[contains(text(),'Document type')]/parent::div/following-sibling::div");
        private By documenyEditDocumentNameLabel = By.XPath("//div[@class='modal-body']/div/div/div/label[contains(text(),'File name')]");
        private By documentEditFileNameContent = By.XPath("//div[@class='modal-body']/div/div/div/label[contains(text(),'File name')]/parent::div/following-sibling::div");

        //Document Modal Elements
        private By documentModalCloseIcon = By.CssSelector("button[class='close']");
        private By documentModalContentDiv = By.CssSelector("div[class='modal-content']");
        private By documentEditBttn = By.XPath("//div[@class='modal-body']/div/div[3]/div/div[2]/button");
        private By documentSaveButton = By.CssSelector("button[data-testid='save']");
        private By documentCancelButton = By.CssSelector("button[data-testid='cancel']");
        private By documentSaveEditBttn = By.XPath("//div[@class='modal-body']/div/div[3]/div[2]/div/div/button[@type='submit']");
        private By documentCancelEditBttn = By.XPath("//div[@class='modal-body']/div/div[3]/div[2]/div/div/button[@type='button']");

        //Toast Element
        private By documentGeneralToastBody = By.CssSelector("div[class='Toastify__toast-body']");

        //Document Confirmation Modal Elements
        private By documentConfirmationModal = By.XPath("//div[contains(text(),'Unsaved Changes')]/parent::div/parent::div");
        private By documentConfirmationContent = By.XPath("//div[contains(text(),'Unsaved Changes')]/parent::div/following-sibling::div[@class='modal-body']");
        private By documentConfirmModalOkBttn = By.XPath("//div[contains(text(),'Unsaved Changes')]/parent::div/following-sibling::div/button[@title='ok-modal']");

        //Document Delete Document Confirmation Modal Elements
        private By documentDeleteHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private By documentDeleteContent1 = By.CssSelector("div[class='modal-body'] div div:nth-child(1)");
        private By documentDeteleContent2 = By.CssSelector("div[class='modal-body'] div:nth-child(3)");
        private By documentDeleteContent3 = By.CssSelector("div[class='modal-body'] div strong");
        private By documentDeleteOkBttn = By.CssSelector("button[title='ok-modal']");

        //Activities Documents List Filters
        //private By documentFilterTypeSelect = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[2]/div/div[1]/div/select[@data-testid='document-type']");
        //private By documentFilterStatusSelect = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[2]/div/div[2]/div/select[@data-testid='document-status']");
        //private By documentFilterNameInput = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[2]/div/div[3]/div/input[@data-testid='document-filename']");
        //private By documentFilterSearchBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[3]/div/div[1]/button[@data-testid='search']");
        //private By documentFilterResetBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[3]/div/div[2]/button[@data-testid='reset-button']");

        //Documents Tab List Filters
        private By documentFilterTypeSelect = By.XPath("//select[@data-testid='document-type']");
        private By documentFilterStatusSelect = By.XPath("//select[@data-testid='document-status']");
        private By documentFilterNameInput = By.XPath("//input[@data-testid='document-filename']");
        private By documentFilterSearchBttn = By.XPath("//button[@data-testid='search']");
        private By documentFilterResetBttn = By.XPath("//button[@data-testid='reset-button']");

        //Activities Documents List Results
        //private By documentTableResults = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']");
        //private By documentTableTypeColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document type')]");
        //private By documentTableNameColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'File name')]");
        //private By documentTableDateColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Uploaded')]");
        //private By documentTableStatusColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Status')]");
        //private By documentTableActionsColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Actions')]");
        //private By documentTableContentTotal = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div[@class='tbody']/div");

        //Documents Tab List Results
        private By documentTableResults = By.XPath("//div[@data-testid='documentsTable']");
        private By documentTableTypeColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document type')]");
        private By documentTableNameColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'File name')]");
        private By documentTableDateColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Uploaded')]");
        private By documentTableStatusColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Status')]");
        private By documentTableActionsColumn = By.XPath("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Actions')]");
        private By documentTableContentTotal = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div");

        //Activities Documents List 1st Result Elements
        //private By documentTableResults1stDownloadBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/div/button[@data-testid='document-download-button']");
        //private By documentTableResults1stViewBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-view-button']");
        //private By documentTableResults1stDeleteBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-delete-button']");

        //Activities Documents List 1st Result Elements
        private By documentTableResults1stDownloadBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/div/button[@data-testid='document-download-button']");
        private By documentTableResults1stViewBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-view-button']");
        private By documentTableResults1stDeleteBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-delete-button']");

        //Activities Documents Pagination
        //private By documentPagination = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@class='row']/div[4]/ul[@class='pagination']");
        //private By documentMenuPagination = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@class='row']/div[3]/div[@class='Menu-root']");

        //Documents Tab Pagination
        private By documentPagination = By.XPath("//div[@class='row']/div[4]/ul[@class='pagination']");
        private By documentMenuPagination = By.XPath("//div[@class='row']/div[3]/div[@class='Menu-root']");


        public SharedDocumentsTab(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateDocumentsTab()
        {
            Wait(2000);
            webDriver.FindElement(documentsTab).Click();
        }

        public void AddNewDocument()
        {
            Wait();
            FocusAndClick(addDocumentBttn);
        }

        public void VerifyDocumentFields(string documentType)
        {
            Wait();
            ChooseSpecificSelectOption(documentUploadDocTypeModalSelect, documentType);

            Wait();
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
                case "OIC":
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
            }
        }

        public void VerifyActivityDocumentsListView()
        {
            Wait();
            Assert.True(webDriver.FindElement(documentsTitle).Displayed);
            Assert.True(webDriver.FindElement(addDocumentBttn).Displayed);

            Assert.True(webDriver.FindElement(documentFilterTypeSelect).Displayed);
            Assert.True(webDriver.FindElement(documentFilterStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(documentFilterNameInput).Displayed);
            Assert.True(webDriver.FindElement(documentFilterSearchBttn).Displayed);
            Assert.True(webDriver.FindElement(documentFilterResetBttn).Displayed);

            Assert.True(webDriver.FindElement(documentTableResults).Displayed);
            Assert.True(webDriver.FindElement(documentTableTypeColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableNameColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableDateColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableStatusColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableActionsColumn).Displayed);
        }

        public void UploadDocument(string documentFile)
        {
            Wait();
            webDriver.FindElement(documentUploadDocInput).SendKeys(documentFile);
        }

        public void UploadTransferAdminFile(string dateSigned, string motiFile, string pid, string roadName, string transfer)
        {
            Wait();
            ChooseSpecificSelectOption(documentUploadDocTypeModalSelect, "Transfer of administration");

            Wait();
            webDriver.FindElement(documentTransferAdmTypeDateInput).SendKeys(dateSigned);
            webDriver.FindElement(documentTransferAdmTypeMOTIFileInput).SendKeys(motiFile);
            webDriver.FindElement(documentTransferAdmTypePropIdInput).SendKeys(pid);
            webDriver.FindElement(documentTransferAdmTypeRoadNameInput).SendKeys(roadName);
            webDriver.FindElement(documentTransferAdmTypeTransferInput).SendKeys(transfer);
        }

        public void UploadGazetteFile(string date, string pageNbr, string publishedDate, string gazatteType, string surveyPlan, string LTSASchdule, string motiPlan, string roadName)
        {
            Wait();

            ChooseSpecificSelectOption(documentUploadDocTypeModalSelect, "Gazette");

            Wait();
            webDriver.FindElement(documentGazetteDateInput).SendKeys(date);
            webDriver.FindElement(documentGazettePageInput).SendKeys(pageNbr);
            webDriver.FindElement(documentGazettePublishedDateInput).SendKeys(publishedDate);
            webDriver.FindElement(documentGazettePublishedTypeInput).SendKeys(gazatteType);
            webDriver.FindElement(documentGazetteLegalSurveyPlanInput).SendKeys(surveyPlan);
            webDriver.FindElement(documentGazetteLTSAInput).SendKeys(LTSASchdule);
            webDriver.FindElement(documentGazetteLegalSurveyMotiPlanInput).SendKeys(motiPlan);
            webDriver.FindElement(documentGazetteRoadNameInput).SendKeys(roadName);
        }

        public void SaveDigitalDocument()
        {
            Wait();
            webDriver.FindElement(documentSaveButton).Click();

            WaitUntil(documentGeneralToastBody);
        }

        public void SaveEditDigitalDocument()
        {
            Wait();
            webDriver.FindElement(documentSaveEditBttn).Click();

            Wait(10000);
        }

        public void CancelDigitalDocument()
        {
            Wait();
            webDriver.FindElement(documentCancelButton).Click();

            Wait();
            if (webDriver.FindElements(documentConfirmationModal).Count() > 0)
            {
                Assert.True(webDriver.FindElement(documentConfirmationContent).Text.Equals("You have made changes on this form. Do you wish to leave without saving?"));
                webDriver.FindElement(documentConfirmModalOkBttn).Click();
            }
        }

        public void CancelEditDigitalDocument()
        {
            Wait();
            webDriver.FindElement(documentCancelEditBttn).Click();

            Wait();
            if (webDriver.FindElements(documentConfirmationModal).Count() > 0)
            {
                Assert.True(webDriver.FindElement(documentConfirmationContent).Text.Equals("You have made changes on this form. Do you wish to leave without saving?"));
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
            Wait(5000);
            webDriver.FindElement(documentTableResults1stViewBttn).Click();
        }

        public void Delete1stDocument()
        {
            Wait();
            webDriver.FindElement(documentTableResults1stDeleteBttn).Click();
           
            Wait();
            Assert.True(webDriver.FindElement(documentDeleteHeader).Text.Equals("Delete a document"));
            Assert.True(webDriver.FindElement(documentDeleteContent1).Text.Equals("You have chosen to delete this document."));
            Assert.True(webDriver.FindElement(documentDeteleContent2).Text.Equals("If the document is linked to other files or entities in PIMS it will still be accessible from there, however if this the only instance then the file will be removed from the document store completely."));
            Assert.True(webDriver.FindElement(documentDeleteContent3).Text.Equals("Do you wish to continue deleting this document?"));

            webDriver.FindElement(documentDeleteOkBttn).Click();
        }

        public void EditDocument()
        {
            Wait(20000);
            webDriver.FindElement(documentEditBttn).Click();
        }

        public void ChangeStatus(string status)
        {
            Wait();
            ChooseSpecificSelectOption(documentUploadStatusSelect, status);
        }

        public int GetTotalUploadedDocuments()
        {
            Wait();
            return webDriver.FindElements(documentTableContentTotal).Count();
        }

        public void VerifyGazetteEditForm()
        {
            Wait();

            Assert.True(webDriver.FindElement(documentEditDocumentTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(documentEditDocumentTypeContent).Text.Equals("Gazette"));
            Assert.True(webDriver.FindElement(documenyEditDocumentNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentEditFileNameContent).Text != "");

            Assert.True(webDriver.FindElement(documentGazetteDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteDateInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazettePageLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePageInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazettePublishedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePublishedDateInput).Displayed);
            
            Assert.True(webDriver.FindElement(documentGazettePublishedTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePublishedTypeInput).Displayed);
            
            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyPlanInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazetteLTSALabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLTSAInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyMotiPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyMotiPlanInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazetteRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteRoadNameInput).Displayed);
        }

        private void VerifyGeneralUploadDocumentForm()
        {
            Wait();
            Assert.True(webDriver.FindElement(documentsUploadHeader).Displayed);
            Assert.True(webDriver.FindElement(documentUploadInstructions).Displayed);
            Assert.True(webDriver.FindElement(documentUploadDocTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(documentUploadDocTypeModalSelect).Displayed);
            Assert.True(webDriver.FindElement(documentUploadDocInput).Displayed);
            Assert.True(webDriver.FindElement(documentUploadDocInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(documentUploadStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(documentUploadStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(documentUploadDetailsSubtitle).Displayed);
        }

        private void VerifyOtherTypeFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();
            Assert.True(webDriver.FindElement(documentOtherTypePINLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypePINInput).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypePropIdLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypePropIdInput).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypeRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypeRoadNameInput).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypeDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOtherTypeDescriptionInput).Displayed);
        }

        private void VerifyFieldNotesFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();
            Assert.True(webDriver.FindElement(documentFieldNotesTypeDistrictLotLabel).Displayed);
            Assert.True(webDriver.FindElement(documentFieldNotesTypeDistrictLotInput).Displayed);
            Assert.True(webDriver.FindElement(documentFieldNotesTypeYearLabel).Displayed);
            Assert.True(webDriver.FindElement(documentFieldNotesTypeYearInput).Displayed);
            Assert.True(webDriver.FindElement(documentFieldNotesTypeLandDistrictLabel).Displayed);
            Assert.True(webDriver.FindElement(documentFieldNotesTypeLandDistrictInput).Displayed); 
        }

        private void VerifyDistrictRoadRegisterFields()
        {
            Wait();

            VerifyGeneralUploadDocumentForm();
            Assert.True(webDriver.FindElement(documentDistrictRoadRegisterTypeElectoralDistrictLabel).Displayed);
            Assert.True(webDriver.FindElement(documentDistrictRoadRegisterTypeElectoralDistrictInput).Displayed);
            Assert.True(webDriver.FindElement(documentDistrictRoadRegisterTypeHighwayDistrictLabel).Displayed);
            Assert.True(webDriver.FindElement(documentDistrictRoadRegisterTypeHighwayDistrictInput).Displayed);
            Assert.True(webDriver.FindElement(documentDistrictRoadRegisterTypeRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentDistrictRoadRegisterTypeRoadNameInput).Displayed);
        }

        private void VerifyBCAssessmentFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentBCAssessmentTypeAddressLabel).Displayed);
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeAddressInput).Displayed);
            webDriver.FindElement(documentBCAssessmentTypeAddressInput).Click();
            webDriver.FindElement(documentBCAssessmentTypeAddressLabel).Click();
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeAddressMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentBCAssessmentTypeJurisdictionLabel).Displayed);
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeJurisdictionInput).Displayed);
            webDriver.FindElement(documentBCAssessmentTypeJurisdictionInput).Click();
            webDriver.FindElement(documentBCAssessmentTypeJurisdictionLabel).Click();
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeJurisdictionMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentBCAssessmentTypeRollLabel).Displayed);
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeRollInput).Displayed);

            Assert.True(webDriver.FindElement(documentBCAssessmentTypeYearLabel).Displayed);
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeYearInput).Displayed);
            webDriver.FindElement(documentBCAssessmentTypeYearInput).Click();
            webDriver.FindElement(documentBCAssessmentTypeYearLabel).Click();
            Assert.True(webDriver.FindElement(documentBCAssessmentTypeYearMandatory).Displayed);
        }

        private void VerifyTransferAdministrationFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentTransferAdmTypeDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTransferAdmTypeDateInput).Displayed);

            Assert.True(webDriver.FindElement(documentTransferAdmTypeMOTIFileLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTransferAdmTypeMOTIFileInput).Displayed);
            webDriver.FindElement(documentTransferAdmTypeMOTIFileInput).Click();
            webDriver.FindElement(documentTransferAdmTypeMOTIFileLabel).Click();
            Assert.True(webDriver.FindElement(documentTransferAdmTypeMOTIFileMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentTransferAdmTypeProIdLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTransferAdmTypePropIdInput).Displayed);

            Assert.True(webDriver.FindElement(documentTransferAdmTypeRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTransferAdmTypeRoadNameInput).Displayed);
            webDriver.FindElement(documentTransferAdmTypeRoadNameInput).Click();
            webDriver.FindElement(documentTransferAdmTypeRoadNameLabel).Click();
            Assert.True(webDriver.FindElement(documentTransferAdmTypeRoadNameMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentTransferAdmTypeTransferLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTransferAdmTypeTransferInput).Displayed);
            webDriver.FindElement(documentTransferAdmTypeTransferInput).Click();
            webDriver.FindElement(documentTransferAdmTypeTransferLabel).Click();
            Assert.True(webDriver.FindElement(documentTransferAdmTypeTransferMandatory).Displayed);
        }

        private void VerifyMinisterialOrderFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeDateSignedLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeDateSignedInput).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeMOLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeMOInput).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeMotiFileLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeMotiFileInput).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypePropIdtLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypePropIdInput).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMinisterialOrderTypeRoadNameInput).Displayed);
        }

        private void VerifyCanadaLandsSurveyFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyLabel).Displayed);
            Assert.True(webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyInput).Displayed);
            webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyInput).Click();
            webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyLabel).Click();
            Assert.True(webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentCanLandSurveyTypeIndianReserveLabel).Displayed);
            Assert.True(webDriver.FindElement(documentCanLandSurveyTypeIndianReserveInput).Displayed);
            webDriver.FindElement(documentCanLandSurveyTypeIndianReserveInput).Click();
            webDriver.FindElement(documentCanLandSurveyTypeIndianReserveLabel).Click();
            Assert.True(webDriver.FindElement(documentCanLandSurveyTypeIndianReserveMandatory).Displayed);
        }

        private void VerifyPhotosCorrespondenceFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeAddressLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeAddressInput).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeDateInput).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeOwnerLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeOwnerInput).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypePropIdLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypePropIdInput).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPhotosCorrespondenceTypeDescriptionInput).Displayed);
        }

        private void VerifyMiscellaneousNotesFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentMiscNotesTypePIDLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMiscNotesTypePIDInput).Displayed);
        }

        private void VerifyTitleSearchFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentTitleSearchTypeOwnerLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTitleSearchTypeOwnerInput).Displayed);
            Assert.True(webDriver.FindElement(documentTitleSearchTypePIDLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTitleSearchTypePIDInput).Displayed);
            Assert.True(webDriver.FindElement(documentTitleSearchTypeTitleLabel).Displayed);
            Assert.True(webDriver.FindElement(documentTitleSearchTypeTitleInput).Displayed);
        }

        private void VerifyHistoricalFileFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentHistoricFileTypeEndDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypeEndDateInput).Displayed);

            Assert.True(webDriver.FindElement(documentHistoricFileTypeFileLabel).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypeFileInput).Displayed);
            webDriver.FindElement(documentHistoricFileTypeFileInput).Click();
            webDriver.FindElement(documentHistoricFileTypeFileLabel).Click();
            Assert.True(webDriver.FindElement(documentHistoricFileTypeFileMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentHistoricFileTypePhyLocationLabel).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypePhyLocationInput).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypeSectionLabel).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypeSectionInput).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypeStartDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentHistoricFileTypeStartDateInput).Displayed);
        }

        private void VerifyCrownGrantFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentCrownGrantTypeCrownLabel).Displayed);
            Assert.True(webDriver.FindElement(documentCrownGrantTypeCrownInput).Displayed);
            webDriver.FindElement(documentCrownGrantTypeCrownInput).Click();
            webDriver.FindElement(documentCrownGrantTypeCrownLabel).Click();
            Assert.True(webDriver.FindElement(documentCrownGrantTypeCrownMandatory).Displayed);
        }

        private void VerifyPrivyCouncilFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentPrivyCouncilTypePrivyLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPrivyCouncilTypePrivyInput).Displayed);
            webDriver.FindElement(documentPrivyCouncilTypePrivyInput).Click();
            webDriver.FindElement(documentPrivyCouncilTypePrivyLabel).Click();
            Assert.True(webDriver.FindElement(documentPrivyCounciltTypePrivyMandatory).Displayed);
        }

        private void VerifyOICFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentOICTypeOICLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeOICInput).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeOICRouteLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeOICRouteInput).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeOICTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeOICTypeInput).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeRoadNameInput).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeYearLabel).Displayed);
            Assert.True(webDriver.FindElement(documentOICTypeYearInput).Displayed);
        }

        private void VerifyLegalSurveyFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentLegalSurveyNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(documentLegalSurveyNbrInput).Displayed);
            Assert.True(webDriver.FindElement(documentLegalSurveyMOTIPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentLegalSurveyMOTIPlanInput).Displayed);
            Assert.True(webDriver.FindElement(documentLegalSurveyPlanTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(documentLegalSurveyPlanTyoeInput).Displayed);
        }

        private void VerifyMOTIPlanFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyPlanInput).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyMotiFileLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyMotiFileInput).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyMotiPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyMotiPlanInput).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyPublishDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyPublishDateInput).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyRelatedGazetteLabel).Displayed);
            Assert.True(webDriver.FindElement(documentMoTIPlanLegalSurveyRelatedGazetteInput).Displayed);
        }

        private void VerifyGazetteFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentGazetteDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteDateInput).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePageLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePageInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazettePublishedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePublishedDateInput).Displayed);
            webDriver.FindElement(documentGazettePublishedDateInput).Click();
            webDriver.FindElement(documentGazettePublishedDateLabel).Click();
            Assert.True(webDriver.FindElement(documentGazettePublishedDateMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentGazettePublishedTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazettePublishedTypeInput).Displayed);
            webDriver.FindElement(documentGazettePublishedTypeInput).Click();
            webDriver.FindElement(documentGazettePublishedTypeLabel).Click();
            Assert.True(webDriver.FindElement(documentGazettePublishedTypeMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyPlanInput).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLTSALabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLTSAInput).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyMotiPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteLegalSurveyMotiPlanInput).Displayed);

            Assert.True(webDriver.FindElement(documentGazetteRoadNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentGazetteRoadNameInput).Displayed);
            webDriver.FindElement(documentGazetteRoadNameInput).Click();
            webDriver.FindElement(documentGazetteRoadNameLabel).Click();
            Assert.True(webDriver.FindElement(documentGazetteRoadNameMandatory).Displayed);
        }

        private void VerifyPAPlansFields()
        {
            Wait();
            VerifyGeneralUploadDocumentForm();

            Assert.True(webDriver.FindElement(documentPAPlanNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPAPlanNbrInput).Displayed);
            webDriver.FindElement(documentPAPlanNbrInput).Click();
            webDriver.FindElement(documentPAPlanNbrLabel).Click();
            Assert.True(webDriver.FindElement(documentPAPlanNbrMandatory).Displayed);

            Assert.True(webDriver.FindElement(documentPAPlanRevisionLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPAPlanRevisionInput).Displayed);
            Assert.True(webDriver.FindElement(documentPAPlanProjectLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPAPlanProjectInput).Displayed);

            Assert.True(webDriver.FindElement(documentPAPlanProjectNameLabel).Displayed);
            Assert.True(webDriver.FindElement(documentPAPlanProjectNameInput).Displayed);
            webDriver.FindElement(documentPAPlanProjectNameInput).Click();
            webDriver.FindElement(documentPAPlanProjectNameLabel).Click();
            Assert.True(webDriver.FindElement(documentPAPlanProjectNameMandatory).Displayed);
        }

    }
}
