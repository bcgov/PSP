const { expect } = require('@playwright/test');

class DigitalDocuments {

    constructor(page) {
        this.page = page;
    }

    async navigateDocumentsTab() {
        const documentsTab = this.page.locator("a[data-rb-event-key='documents']");
        await documentsTab.waitFor({ state: 'visible' });
        await documentsTab.click();
    }

    async navigatePropertyDocumentsTab() {
        const propsDocumentsTab = this.page.locator("a[data-rb-event-key='document']");
        await propsDocumentsTab.waitFor({ state: 'visible' });
        await propsDocumentsTab.click();
    }

    async navigateToFirstPageDocumentsTable() {
        const documentPaginationPrevPageLink = this.page.locator("ul.pagination a[aria-label='Previous page']");
        await documentPaginationPrevPageLink.waitFor({ state: 'visible' });
        await documentPaginationPrevPageLink.click();
    }

    async addNewDocumentButton() {
        const addDocumentBttn = this.page.locator("//button[@data-testid='refresh-button']/preceding-sibling::button");
        await addDocumentBttn.waitFor({ state: 'visible' });
        await addDocumentBttn.click();
    }

    async verifyDocumentFields(documentType) {
    verifyGeneralUpdateDocumentForm();

    switch (documentType) {
        case "Agricultural Land Commission (ALC)":
            verifyALCFields();
            break;
        case "BC assessment search":
            verifyBCAssessmentFields();
            break;
        case "Canada lands survey":
            verifyCanadaLandsSurveyFields();
            break;
        case "Correspondence":
            verifyPhotosCorrespondenceFields();
            break;
        case "Crown grant":
            verifyCrownGrantFields();
            break;
        case "District road register":
            verifyDistrictRoadRegisterFields();
            break;
        case "Field notes":
            verifyFieldNotesFields();
            break;
        case "Form 12":
            verifyForm12Fields();
            break;
        case "Gazette":
            verifyGazetteFields();
            break;
        case "Historical file":
            verifyHistoricalFileFields();
            break;
        case "Land Act Tenure/Reserves":
            verifyLandActTenureFields();
            break;
        case "Legal survey plan":
            verifyLegalSurveyFields();
            break;
        case "Ministerial order":
            verifyMinisterialOrderFields();
            break;
        case "Miscellaneous notes (LTSA)":
            verifyMiscellaneousNotesFields();
            break;
        case "MoTI plan":
            verifyMOTIPlanFields();
            break;
        case "Order in Council (OIC)":
            verifyOICFields();
            break;
        case "Other":
            verifyOtherTypeFields();
            break;
        case "PA plans / Design drawings":
            verifyPAPlansFields();
            break;
        case "Photos / Images/ Video":
            verifyPhotosCorrespondenceFields();
            break;
        case "Privy council":
            verifyPrivyCouncilFields();
            break;
        case "Title search / Historical title":
            verifyTitleSearchFields();
            break;
        case "Transfer of administration":
            verifyTransferAdministrationFields();
            break;
        default:
            verifyShortDescriptorField();
            break;
        }
    }

    async verifyDocumentsListView() {
        const documentsTitle = await this.page.locator("//span[contains(text(),'Documents')]");
        expect(documentsTitle).toBeVisible();

        const addDocumentBttn = await this.page.locator("//button[@data-testid='refresh-button']/preceding-sibling::button");
        expect(addDocumentBttn).toBeVisible();

        const documentFilterTypeSelect = await this.page.locator("//select[@data-testid='document-type']");
        expect(documentFilterTypeSelect).toBeVisible();

        const documentFilterStatusSelect = await this.page.locator("//select[@data-testid='document-status']");
        expect(documentFilterStatusSelect).toBeVisible();

        const documentFilterNameInput = await this.page.locator("//input[@data-testid='document-filename']");
        expect(documentFilterNameInput).toBeVisible();

        const documentFilterSearchBttn = await this.page.locator("//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='search']");
        expect(documentFilterSearchBttn).toBeVisible();

        const documentFilterResetBttn = await this.page.locator("//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='reset-button']");
        expect(documentFilterResetBttn).toBeVisible();

        const documentTableListView = await this.page.locator("//div[@data-testid='documentsTable']");
        expect(documentTableListView).toBeVisible();

        const documentTableTypeColumn = await this.page.locator("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document type')]");
        expect(documentTableTypeColumn).toBeVisible();

        const documentTableNameColumn = await this.page.locator("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document name')]");
        expect(documentTableNameColumn).toBeVisible();

        const documentTableDateColumn = await this.page.locator("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Uploaded')]");
        expect(documentTableDateColumn).toBeVisible();

        const documentTableStatusColumn = await this.page.locator("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Status')]");
        expect(documentTableStatusColumn).toBeVisible();

        const documentTableActionsColumn = await this.page.locator("//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Actions')]");
        expect(documentTableActionsColumn).toBeVisible();
    }

    async digitalDocumentsTableResultNumber() {
        await this.page.locator("div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']");
        const totalDigitalDocumentsCount = await this.page.locator("div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']").count();
        return totalDigitalDocumentsCount;
    }

    async orderByDocumentFileType() {
        await this.page.getByTestId("sort-column-documentType").click();
    }

    async orderByDocumentFileName() {
        await this.page.getByTestId("sort-column-fileName").click();
    }

    async orderByDocumentFileStatus() {
        await this.page.getByTestId("sort-column-statusTypeCode").click();
    }

    async firstDocumentFileType() {
        const fileType = await this.page.locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[1]");
        fileType.waitFor({status: 'visible'});
        return fileType.textContent();
    }

    async firstDocumentFileName() {
        const documentName = await this.page.locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[2]/div/button/div");
        documentName.waitFor({status: 'visible'});
        return documentName.textContent();
    }

    async firstDocumentFileStatus() {
        const fileStatus = await this.page.locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[4]");
        fileStatus.waitFor({status: 'visible'});
        return fileStatus.textContent();
    }

    async filterDocuments({documentType = "", documentStatus = "", documentName = ""}) {
        await this.page.locator("//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='reset-button']").click();

        if(documentType != "") {
            const documentTypeElement = await this.page.getByTestId("document-type");
            documentTypeElement.waitFor({status: 'visible'});
            documentTypeElement.selectOption({label: documentType});
        }

        if(documentStatus != "") {
            const documentStatusElement = await this.page.getByTestId("document-status");
            documentStatusElement.waitFor({status: 'visible'});
            documentStatusElement.selectOption({label: documentStatus});
        }

        if(documentName != "") {
            const documentNameElement = await this.page.getByTestId("document-filename");
            documentNameElement.waitFor({status: 'visible'});
            documentNameElement.fill(documentName);
        }

        const searchButton = await this.page.locator("//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='search']");
        searchButton.waitFor({status: 'visible'});
        searchButton.click();
    }

    async totalSearchDocuments() {
        const totalSearchDocuments = await this.page.locator("div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']");
        totalSearchDocuments.waitFor({status: 'visible'});
        return totalSearchDocuments.count();
    }

    async uploadDocument(documentFile) {
        await this.page.getByTestId("upload-input").fill(documentFile);
    }

    async saveDigitalDocumentUpload()
    {
        this.sharedModals.mainModalClickOKBttn();

        const confirmationMessage = await this.page.locator("//span[contains(text(),'files successfully uploaded')]");
        confirmationMessage.waitFor({status: 'visible'});
        this.sharedModals.mainModalClickOKBttn();
    }

    async saveUpdateDigitalDocument() {
        const saveButton = await this.page.locator("//div[@class='modal-body']/div/div[2]/div/div/div/div/button/div[contains(text(),'Yes')]/parent::button");
        saveButton.waitFor({status: 'clickable'});
        saveButton.click();
    }

    async cancelDigitalDocument() {
        await this.page.locator("button[title='cancel-modal']").click();

        const confirmationModal = await this.page.locator("//div[contains(text(),'Confirm Changes')]/parent::div/parent::div");
        if (confirmationModal.count() > 0) {
            const modalBody = await this.page.locator("//div[contains(text(),'Confirm Changes')]/parent::div/following-sibling::div[@class='modal-body']");
            expect(modalBody).toContain("If you choose to cancel now, your changes will not be saved.");
            expect(modalBody).toContain("Do you want to proceed?");

            await this.page.locator("div[class='modal-content'] button[title='ok-modal']").click();
        }
        else {
            const cancelWarning = await this.page.locator("//div[@class='modal-footer']/div[@class='button-wrap']/p");
            cancelWarning.waitFor({status: 'visible'});

            expect(cancelWarning).toContain("Unsaved updates will be lost. Click");
            expect(cancelWarning).toContain("again to proceed without saving, or");
            expect(cancelWarning).toContain("to save the changes.");

            await this.page.locator("button[title='cancel-modal']").click();
        }
    }

    async closeDigitalDocumentViewDetails() {
        await this.page.locator("div[class='modal-close-btn']").click();
    }

    async view1stDocument() {
        await this.page.locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']").click();
    }

    async waitUploadDocument() {
        const maxWaitTime = 5 * 60 * 1000; // 5 minutes
        const refreshInterval = 5000; // 5 seconds between refreshes
        const startTime = Date.now();

    while (Date.now() - startTime < maxWaitTime) {
        // Click refresh button
        await this.page.getByTestId("refresh-button").click();

        // Wait briefly for UI to update
        await this.page.waitForTimeout(1000);

        // Check if download button is visible
        const isDetailsBttnVisible = await this.page
            .locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']")
            .isVisible();

        if (isDetailsBttnVisible) {
            console.log('Details button is now visible!');
            break;
        }

        console.log('Details button not yet visible, retrying...');
        await this.page.waitForTimeout(refreshInterval);
    }

    // Final check after loop
    await expect(this.page.locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']"))
        .toBeVisible({ timeout: 5000 });
    }

    async viewUploadedDocument(index) {
        const firstViewButton = await this.page.locator("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']");
        firstViewButton.waitFor({status: 'visible'});

        if (index > 9) {
            await this.page.locator("ul[class='pagination'] a[aria-label='Next page']").click();
        }

        const elementChild = (index % 10) + 1;
        await this.page.locator(`//div[@data-testid='documentsTable']/div[@class='tbody']/div[${elementChild}]/div/div[5]/div/button[@data-testid='document-view-button']`).click();
    }

    async delete1stDocument()
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

    async editDocumentButton()
    {
        Wait(2000);
        FocusAndClick(documentEditBttn);
    }

    // public void InsertDocumentTypeStatus(DigitalDocument document, int docIdx)
    // {
    //     Wait();

    //     By docTypeSelectElement = By.Id("input-documents."+ docIdx +".documentTypeId");
    //     By statusSelectElement = By.Id("input-documents." +docIdx +".documentStatusCode");

    //     WaitUntilExist(docTypeSelectElement);
    //     ChooseSpecificSelectOption(docTypeSelectElement, document.DocumentType);
    //     ChooseSpecificSelectOption(statusSelectElement, document.DocumentStatus);

    //     Wait();
    //     webDriver.FindElement(By.XPath("//select[@data-testid='documents."+ docIdx +".document-type']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/*[1]")).Click();
    // }

    // public void InsertDocumentTypeDetails(DigitalDocument document)
    // {
    //     Wait();

    //     if (document.ApplicationNumber != "" && webDriver.FindElements(documentALCTypeAppNumberInput).Count > 0)
    //         webDriver.FindElement(documentALCTypeAppNumberInput).SendKeys(document.ApplicationNumber);

    //     if (document.CanadaLandSurvey != "" && webDriver.FindElements(documentCanLandSurveyTypeCanLandSurveyInput).Count > 0)
    //         webDriver.FindElement(documentCanLandSurveyTypeCanLandSurveyInput).SendKeys(document.CanadaLandSurvey);

    //     if (document.CivicAddress != "" && webDriver.FindElements(documentCivicAddressInput).Count > 0)
    //         webDriver.FindElement(documentCivicAddressInput).SendKeys(document.CivicAddress);

    //     if (document.CrownGrant != "" && webDriver.FindElements(documentCrownGrantTypeCrownInput).Count > 0)
    //         webDriver.FindElement(documentCrownGrantTypeCrownInput).SendKeys(document.CrownGrant);

    //     if (document.Date != "" && webDriver.FindElements(documentPhotosCorrespondenceTypeDateInput).Count > 0)
    //         webDriver.FindElement(documentPhotosCorrespondenceTypeDateInput).SendKeys(document.Date);

    //     if (document.DateSigned != "" && webDriver.FindElements(documentDateSignedInput).Count > 0)
    //         webDriver.FindElement(documentDateSignedInput).SendKeys(document.DateSigned);

    //     if (document.DistrictLot != "" && webDriver.FindElements(documentFieldNotesTypeDistrictLotInput).Count > 0)
    //         webDriver.FindElement(documentFieldNotesTypeDistrictLotInput).SendKeys(document.DistrictLot);

    //     if (document.ElectoralDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeElectoralDistrictInput).Count > 0)
    //         webDriver.FindElement(documentDistrictRoadRegisterTypeElectoralDistrictInput).SendKeys(document.ElectoralDistrict);

    //     if (document.EndDate != "" && webDriver.FindElements(documentHistoricFileTypeEndDateInput).Count > 0)
    //         webDriver.FindElement(documentHistoricFileTypeEndDateInput).SendKeys(document.EndDate);

    //     if (document.FieldBook != "" && webDriver.FindElements(documentFieldNotesTypeYearInput).Count > 0)
    //         webDriver.FindElement(documentFieldNotesTypeYearInput).SendKeys(document.FieldBook);

    //     if (document.File != "" && webDriver.FindElements(documentHistoricFileTypeFileInput).Count > 0)
    //         webDriver.FindElement(documentHistoricFileTypeFileInput).SendKeys(document.File);

    //     if (document.GazetteDate != "" && webDriver.FindElements(documentGazetteDateInput).Count > 0)
    //         webDriver.FindElement(documentGazetteDateInput).SendKeys(document.GazetteDate);

    //     if (document.GazettePage != "" && webDriver.FindElements(documentGazettePageInput).Count > 0)
    //         webDriver.FindElement(documentGazettePageInput).SendKeys(document.GazettePage);

    //     if (document.GazettePublishedDate != "" && webDriver.FindElements(documentGazettePublishedDateInput).Count > 0)
    //         webDriver.FindElement(documentGazettePublishedDateInput).SendKeys(document.GazettePublishedDate);

    //     if (document.GazetteType != "" && webDriver.FindElements(documentGazettePublishedTypeInput).Count > 0)
    //         webDriver.FindElement(documentGazettePublishedTypeInput).SendKeys(document.GazetteType);

    //     if (document.HighwayDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeHighwayDistrictInput).Count > 0)
    //         webDriver.FindElement(documentDistrictRoadRegisterTypeHighwayDistrictInput).SendKeys(document.HighwayDistrict);

    //     if (document.IndianReserveOrNationalPark != "" && webDriver.FindElements(documentCanLandSurveyTypeIndianReserveInput).Count > 0)
    //         webDriver.FindElement(documentCanLandSurveyTypeIndianReserveInput).SendKeys(document.IndianReserveOrNationalPark);

    //     if (document.Jurisdiction != "" && webDriver.FindElements(documentBCAssessmentTypeJurisdictionInput).Count > 0)
    //         webDriver.FindElement(documentBCAssessmentTypeJurisdictionInput).SendKeys(document.Jurisdiction);

    //     if (document.LandDistrict != "" && webDriver.FindElements(documentFieldNotesTypeLandDistrictInput).Count > 0)
    //         webDriver.FindElement(documentFieldNotesTypeLandDistrictInput).SendKeys(document.LandDistrict);

    //     if (document.LegalSurveyPlan != "" && webDriver.FindElements(documentLegalSurveyInput).Count > 0)
    //         webDriver.FindElement(documentLegalSurveyInput).SendKeys(document.LegalSurveyPlan);

    //     if (document.LTSAScheduleFiling != "" && webDriver.FindElements(documentGazetteLTSAInput).Count > 0)
    //         webDriver.FindElement(documentGazetteLTSAInput).SendKeys(document.LTSAScheduleFiling);

    //     if (document.MO != "" && webDriver.FindElements(documentMinisterialOrderTypeMOInput).Count > 0)
    //         webDriver.FindElement(documentMinisterialOrderTypeMOInput).SendKeys(document.MO);

    //     if (document.MoTIFile != "" && webDriver.FindElements(documentTypeMotiFileInput).Count > 0)
    //         webDriver.FindElement(documentTypeMotiFileInput).SendKeys(document.MoTIFile);

    //     if (document.MoTIPlan != "" && webDriver.FindElements(documentMOTIPlanInput).Count > 0)
    //         webDriver.FindElement(documentMOTIPlanInput).SendKeys(document.MoTIPlan);

    //     if (document.OIC != "" && webDriver.FindElements(documentOICTypeInput).Count > 0)
    //         webDriver.FindElement(documentOICTypeInput).SendKeys(document.OIC);

    //     if (document.OICRoute != "" && webDriver.FindElements(documentOICTypeOICRouteInput).Count > 0)
    //         webDriver.FindElement(documentOICTypeOICRouteInput).SendKeys(document.OICRoute);

    //     if (document.OICType != "" && webDriver.FindElements(documentOICTypeOICTypeInput).Count > 0)
    //         webDriver.FindElement(documentOICTypeOICTypeInput).SendKeys(document.OICType);

    //     if (document.Owner != "" && webDriver.FindElements(documentTypeOwnerInput).Count > 0)
    //         webDriver.FindElement(documentTypeOwnerInput).SendKeys(document.Owner);

    //     if (document.PhysicalLocation != "" && webDriver.FindElements(documentHistoricFileTypePhyLocationInput).Count > 0)
    //         webDriver.FindElement(documentHistoricFileTypePhyLocationInput).SendKeys(document.PhysicalLocation);

    //     if (document.PIDNumber != "" && webDriver.FindElements(documentTypePropIdInput).Count > 0)
    //         webDriver.FindElement(documentTypePropIdInput).SendKeys(document.PIDNumber);

    //     if (document.PINNumber != "" && webDriver.FindElements(documentOtherTypePINInput).Count > 0)
    //         webDriver.FindElement(documentOtherTypePINInput).SendKeys(document.PINNumber);

    //     if (document.Plan != "" && webDriver.FindElements(documentPAPlanNbrInput).Count > 0)
    //         webDriver.FindElement(documentPAPlanNbrInput).SendKeys(document.Plan);

    //     if (document.PlanRevision != "" && webDriver.FindElements(documentPAPlanRevisionInput).Count > 0)
    //         webDriver.FindElement(documentPAPlanRevisionInput).SendKeys(document.PlanRevision);

    //     if (document.PlanType != "" && webDriver.FindElements(documentLegalSurveyPlanTypeInput).Count > 0)
    //         webDriver.FindElement(documentLegalSurveyPlanTypeInput).SendKeys(document.PlanType);

    //     if (document.Project != "" && webDriver.FindElements(documentPAPlanProjectInput).Count > 0)
    //         webDriver.FindElement(documentPAPlanProjectInput).SendKeys(document.Project);

    //     if (document.ProjectName != "" && webDriver.FindElements(documentPAPlanProjectNameInput).Count > 0)
    //         webDriver.FindElement(documentPAPlanProjectNameInput).SendKeys(document.ProjectName);

    //     if (document.PropertyIdentifier != "" && webDriver.FindElements(documentTypePropertyIdentifierInput).Count > 0)
    //         webDriver.FindElement(documentTypePropertyIdentifierInput).SendKeys(document.PropertyIdentifier);

    //     if (document.PublishedDate != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyPublishDateInput).Count > 0)
    //         webDriver.FindElement(documentMoTIPlanLegalSurveyPublishDateInput).SendKeys(document.PublishedDate);

    //     if (document.ReferenceAgencyDocumentNbr!= "" && webDriver.FindElements(documentLandActTypeReferenceAgencyInput).Count > 0)
    //         webDriver.FindElement(documentLandActTypeReferenceAgencyInput).SendKeys(document.ReferenceAgencyDocumentNbr);

    //     if (document.ReferenceAgencyLandsFileNbr!= "" && webDriver.FindElements(documentLandActTypeReferenceLandsInput).Count > 0)
    //         webDriver.FindElement(documentLandActTypeReferenceLandsInput).SendKeys(document.ReferenceAgencyLandsFileNbr);

    //     if (document.RelatedGazette != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyRelatedGazetteInput).Count > 0)
    //         webDriver.FindElement(documentMoTIPlanLegalSurveyRelatedGazetteInput).SendKeys(document.RelatedGazette);

    //     if (document.RoadName != "" && webDriver.FindElements(documentRoadNameInput).Count > 0)
    //         webDriver.FindElement(documentRoadNameInput).SendKeys(document.RoadName);

    //     if (document.Roll != "" && webDriver.FindElements(documentBCAssessmentTypeRollInput).Count > 0)
    //         webDriver.FindElement(documentBCAssessmentTypeRollInput).SendKeys(document.Roll);

    //     if (document.Section != "" && webDriver.FindElements(documentHistoricFileTypeSectionInput).Count > 0)
    //         webDriver.FindElement(documentHistoricFileTypeSectionInput).SendKeys(document.Section);

    //     if (document.ShortDescriptor != "" && webDriver.FindElements(documentShortDescriptorInput).Count > 0)
    //         webDriver.FindElement(documentShortDescriptorInput).SendKeys(document.ShortDescriptor);

    //     if (document.StartDate != "" && webDriver.FindElements(documentHistoricFileTypeStartDateInput).Count > 0)
    //         webDriver.FindElement(documentHistoricFileTypeStartDateInput).SendKeys(document.StartDate);

    //     if (document.Title != "" && webDriver.FindElements(documentTitleSearchTypeTitleInput).Count > 0)
    //         webDriver.FindElement(documentTitleSearchTypeTitleInput).SendKeys(document.Title);

    //     if (document.Transfer != "" && webDriver.FindElements(documentTransferAdmTypeTransferInput).Count > 0)
    //         webDriver.FindElement(documentTransferAdmTypeTransferInput).SendKeys(document.Transfer);

    //     if (document.Year != "" && webDriver.FindElements(documentYearInput).Count > 0)
    //         webDriver.FindElement(documentYearInput).SendKeys(document.Year);

    //     if (document.YearPrivyCouncil != "" && webDriver.FindElements(documentPrivyCouncilTypePrivyInput).Count > 0)
    //         webDriver.FindElement(documentPrivyCouncilTypePrivyInput).SendKeys(document.YearPrivyCouncil);
    // }

    // public void VerifyDocumentDetailsViewForm(DigitalDocument document)
    // {
    //     Wait();
    //     WaitUntilSpinnerDisappear();

    //     //Header
    //     AssertTrueIsDisplayed(documentViewDocumentTypeLabel);

    //     AssertTrueContentEquals(documentViewDocumentTypeContent, document.DocumentType);
    //     AssertTrueIsDisplayed(documenyViewDocumentNameLabel);
    //     AssertTrueContentNotEquals(documentViewFileNameContent, "");

    //     //Document Information
    //     AssertTrueIsDisplayed(documentViewInfoSubtitle);
    //     AssertTrueIsDisplayed(documentViewDocumentInfoTooltip);
    //     AssertTrueIsDisplayed(documentEditBttn);
    //     AssertTrueIsDisplayed(documentViewStatusLabel);
    //     AssertTrueContentEquals(documentViewStatusContent, document.DocumentStatus);

    //     //Document Details
    //     AssertTrueIsDisplayed(documentViewDetailsSubtitle);

    //     if (document.ApplicationNumber != "" && webDriver.FindElements(documentALCTypeAppNumberLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewApplicationNbrContent, document.ApplicationNumber);

    //     if (document.CanadaLandSurvey != "" && webDriver.FindElements(documentCanLandSurveyTypeCanLandSurveyLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewCanadaLandSurveyContent, document.CanadaLandSurvey);
    
    //     if (document.CivicAddress != "" && webDriver.FindElements(documentCivicAddressLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewCivicAddressContent, document.CivicAddress);
        
    //     if (document.CrownGrant != "" && webDriver.FindElements(documentCrownGrantTypeCrownLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewCrownGrantContent, document.CrownGrant);
        
    //     if (document.Date != "" && webDriver.FindElements(documentPhotosCorrespondenceTypeDateLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewDateContent, TranformFormatDateDocument(document.Date));
        
    //     if (document.DateSigned != "" && webDriver.FindElements(documentDateSignedLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewDateSignedContent, TranformFormatDateDocument(document.DateSigned));
        
    //     if (document.DistrictLot != "" && webDriver.FindElements(documentFieldNotesTypeDistrictLotLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewDistrictLotContent, document.DistrictLot);
        
    //     if (document.ElectoralDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeElectoralDistrictLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewElectoralDistrictContent, document.ElectoralDistrict);
        
    //     if (document.EndDate != "" && webDriver.FindElements(documentHistoricFileTypeEndDateLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewEndDateContent, TranformFormatDateDocument(document.EndDate));

    //     if (document.FieldBook != "" && webDriver.FindElements(documentFieldNotesTypeYearLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewFieldBookContent, document.FieldBook);

    //     if (document.File != "" && webDriver.FindElements(documentHistoricFileTypeFileLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewFileNumberContent, document.File);

    //     if (document.GazetteDate != "" && webDriver.FindElements(documentGazetteDateLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewGazetteDateContent, TranformFormatDateDocument(document.GazetteDate));

    //     if (document.GazettePage != "" && webDriver.FindElements(documentGazettePageLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewGazettePageContent, document.GazettePage);

    //     if (document.GazettePublishedDate != "" && webDriver.FindElements(documentGazettePublishedDateLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewGazettePublishedDateContent, TranformFormatDateDocument(document.GazettePublishedDate));
        
    //     if (document.GazetteType != "" && webDriver.FindElements(documentGazettePublishedTypeLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewGazettePublishedTypeContent, document.GazetteType);
        
    //     if (document.HighwayDistrict != "" && webDriver.FindElements(documentDistrictRoadRegisterTypeHighwayDistrictLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewGazetteHighwayDistrictContent, document.HighwayDistrict);
        
    //     if (document.IndianReserveOrNationalPark != "" && webDriver.FindElements(documentCanLandSurveyTypeIndianReserveLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewIndianReserveContent, document.IndianReserveOrNationalPark);
        
    //     if (document.Jurisdiction != "" && webDriver.FindElements(documentBCAssessmentTypeJurisdictionLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewJurisdictionContent, document.Jurisdiction);
        
    //     if (document.LandDistrict != "" && webDriver.FindElements(documentFieldNotesTypeLandDistrictLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewLandDistrictContent, document.LandDistrict);
        
    //     if (document.LegalSurveyPlan != "" && webDriver.FindElements(documentLegalSurveyNbrLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewLegalSurveyPlanContent, document.LegalSurveyPlan);
        
    //     if (document.LTSAScheduleFiling != "" && webDriver.FindElements(documentGazetteLTSALabel).Count > 0)
    //         AssertTrueContentEquals(documentViewLTSAScheduleFilingContent, document.LTSAScheduleFiling);
        
    //     if (document.MO != "" && webDriver.FindElements(documentMinisterialOrderTypeMOLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewMOContent, document.MO);
        
    //     if (document.MoTIFile != "" && webDriver.FindElements(documentMOTIFileLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewMotiFileContent, document.MoTIFile);
        
    //     if (document.MoTIPlan != "" && webDriver.FindElements(documentMOTIPlanLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewMotiPlanContent, document.MoTIPlan);
        
    //     if (document.OIC != "" && webDriver.FindElements(documentOICTypeOICLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewOICNumberContent, document.OIC);
        
    //     if (document.OICRoute != "" && webDriver.FindElements(documentOICTypeOICRouteLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewOICRouteContent, document.OICRoute);
        
    //     if (document.OICType != "" && webDriver.FindElements(documentOICTypeOICTypeLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewOICTypeContent, document.OICType);
        
    //     if (document.Owner != "" && webDriver.FindElements(documentOwnerLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewOwnerContent, document.Owner);
        
    //     if (document.PhysicalLocation != "" && webDriver.FindElements(documentHistoricFileTypePhyLocationLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPhysicalLocationContent, document.PhysicalLocation);
        
    //     if (document.PIDNumber != "" && webDriver.FindElements(documentViewPIDLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPIDContent,document.PIDNumber);
        
    //     if (document.PINNumber != "" && webDriver.FindElements(documentOtherTypePINLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPINContent, document.PINNumber);
        
    //     if (document.Plan != "" && webDriver.FindElements(documentPAPlanNbrLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPlanNumberContent, document.Plan);
        
    //     if (document.PlanRevision != "" && webDriver.FindElements(documentPAPlanRevisionLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPlanRevisionContent, document.PlanRevision);
        
    //     if (document.PlanType != "" && webDriver.FindElements(documentLegalSurveyPlanTypeLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPlanTypeContent, document.PlanType);
        
    //     if (document.Project != "" && webDriver.FindElements(documentPAPlanProjectLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewProjectNumberContent, document.Project);
        
    //     if (document.ProjectName != "" && webDriver.FindElements(documentViewProjectLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewProjectContent, document.ProjectName);
        
    //     if (document.PropertyIdentifier != "" && webDriver.FindElements(documentViewPropertyIdentifierLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPropertyIdentifierContent, document.PropertyIdentifier);
        
    //     if (document.PublishedDate != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyPublishDateLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewPublishedDateContent, TranformFormatDateDocument(document.PublishedDate));

    //     if (document.ReferenceAgencyDocumentNbr != "" && webDriver.FindElements(documentLandActTypeReferenceAgencyLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewReferenceAgencyContent, document.ReferenceAgencyDocumentNbr);

    //     if (document.ReferenceAgencyLandsFileNbr != "" && webDriver.FindElements(documentLandActTypeReferenceLandsLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewReferenceLandsContent, document.ReferenceAgencyLandsFileNbr);

    //     if (document.RelatedGazette != "" && webDriver.FindElements(documentMoTIPlanLegalSurveyRelatedGazetteLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewRelatedGazetteContent, document.RelatedGazette);
        
    //     if (document.RoadName != "" && webDriver.FindElements(documentRoadNameLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewRoadNameContent, document.RoadName);
        
    //     if (document.Roll != "" && webDriver.FindElements(documentBCAssessmentTypeRollLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewRollContent, document.Roll);
        
    //     if (document.Section != "" && webDriver.FindElements(documentHistoricFileTypeSectionLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewSectionContent, document.Section);
        
    //     if (document.ShortDescriptor != "" && webDriver.FindElements(documentShortDescriptorLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewShortDescriptorContent, document.ShortDescriptor);
        
    //     if (document.StartDate != "" && webDriver.FindElements(documentHistoricFileTypeStartDateLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewStartDateContent, TranformFormatDateDocument(document.StartDate));
        
    //     if (document.Title != "" && webDriver.FindElements(documentTitleSearchTypeTitleLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewTitleContent, document.Title);
        
    //     if (document.Transfer != "" && webDriver.FindElements(documentTransferAdmTypeTransferLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewTransferContent, document.Transfer);
        
    //     if (document.Year != "" && webDriver.FindElements(documentYearLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewYearContent, document.Year);
        
    //     if (document.YearPrivyCouncil != "" && webDriver.FindElements(documentPrivyCouncilTypePrivyLabel).Count > 0)
    //         AssertTrueContentEquals(documentViewYearPrivyCouncilContent, document.YearPrivyCouncil);
    // }

    // public void VerifyPropertyMgmtFileDocumentsInitMainTables(string title1stTable, string title2ndTable)
    // {
    //     AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'"+ title1stTable +"')]/parent::div/parent::div/parent::div/parent::div/parent::h2"));
    //     AssertTrueIsDisplayed(addDocumentBttn);
    //     AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'"+ title1stTable +"')]/parent::div/following-sibling::div/div/button[2]"));
    //     AssertTrueIsDisplayed(documentsTableColumnType);
    //     AssertTrueIsDisplayed(documentsTableColumnName);
    //     AssertTrueIsDisplayed(documentsTableColumnUploaded);
    //     AssertTrueIsDisplayed(documentsTableColumnStatus);
    //     AssertTrueIsDisplayed(documentsTableColumnActions);


    //     AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'"+ title2ndTable +"')]/parent::div/parent::div/parent::div/parent::div/parent::h2"));
    //     AssertTrueIsDisplayed(By.XPath("//span[contains(text(),'"+ title2ndTable +"')]/parent::div/following-sibling::div/div/button"));
    // }

    // public void VerifyInitUploadDocumentForm()
    // {
    //     WaitUntilVisible(documentsUploadHeader);

    //     AssertTrueIsDisplayed(documentsUploadHeader);
    //     AssertTrueIsDisplayed(documentUploadInstructionsLabel);
    //     AssertTrueIsDisplayed(documentUploadDragDropArea);
    //     WaitUntilExist(documentUploadDocInput);
    // }

    // private void VerifyGeneralUpdateDocumentForm()
    // {
    //     WaitUntilVisible(documentsUploadHeader);

    //     AssertTrueIsDisplayed(documentsUploadHeader);
    //     AssertTrueIsDisplayed(documenyViewDocumentNameLabel);
    //     AssertTrueIsDisplayed(documentViewFileNameContent);

    //     AssertTrueIsDisplayed(documentViewInfoSubtitle);
    //     AssertTrueIsDisplayed(documentsGeneralUpdateDocumentTypeLabel);
    //     AssertTrueIsDisplayed(documentGeneralUpdateDocumentSelect);
    //     AssertTrueIsDisplayed(documentGeneralUpdateStatusLabel);
    //     AssertTrueIsDisplayed(documentUploadStatusSelect);

    //     AssertTrueIsDisplayed(documentUpdateDetailsSubtitle);
    // }

    // async verifyALCFields() {
    //         const documentALCTypeAppNumberLabel = this.page.locator("//label[contains(text(),'Application #')]");
    //         const documentALCTypeAppNumberInput = this.page.locator("input[data-testid='metadata-input-APPLICATION_NUMBER']");

    //         await expect(documentALCTypeAppNumberLabel).toBeVisible();
    //         await expect(documentALCTypeAppNumberInput).toBeVisible();
    // }

    // private void VerifyBCAssessmentFields()
    // {
    //     WaitUntilVisible(documentCivicAddressLabel);

    //     AssertTrueIsDisplayed(documentCivicAddressLabel);
    //     AssertTrueIsDisplayed(documentCivicAddressInput);

    //     AssertTrueIsDisplayed(documentBCAssessmentTypeJurisdictionLabel);
    //     AssertTrueIsDisplayed(documentBCAssessmentTypeJurisdictionInput);

    //     AssertTrueIsDisplayed(documentBCAssessmentTypeRollLabel);
    //     AssertTrueIsDisplayed(documentBCAssessmentTypeRollInput);

    //     AssertTrueIsDisplayed(documentYearLabel);
    //     AssertTrueIsDisplayed(documentYearInput);
    // }

    // private void VerifyCanadaLandsSurveyFields()
    // {
    //     WaitUntilVisible(documentCanLandSurveyTypeCanLandSurveyLabel);

    //     AssertTrueIsDisplayed(documentCanLandSurveyTypeCanLandSurveyLabel);
    //     AssertTrueIsDisplayed(documentCanLandSurveyTypeCanLandSurveyInput);

    //     AssertTrueIsDisplayed(documentCanLandSurveyTypeIndianReserveLabel);
    //     AssertTrueIsDisplayed(documentCanLandSurveyTypeIndianReserveInput);
    // }

    // private void VerifyCrownGrantFields()
    // {
    //     WaitUntilVisible(documentCrownGrantTypeCrownLabel);

    //     AssertTrueIsDisplayed(documentCrownGrantTypeCrownLabel);
    //     AssertTrueIsDisplayed(documentCrownGrantTypeCrownInput);
    // }

    // private void VerifyDistrictRoadRegisterFields()
    // {
    //     WaitUntilVisible(documentDistrictRoadRegisterTypeElectoralDistrictLabel);

    //     AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeElectoralDistrictLabel);
    //     AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeElectoralDistrictInput);

    //     AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeHighwayDistrictLabel);
    //     AssertTrueIsDisplayed(documentDistrictRoadRegisterTypeHighwayDistrictInput);

    //     AssertTrueIsDisplayed(documentRoadNameLabel);
    //     AssertTrueIsDisplayed(documentRoadNameInput);
    // }

    // private void VerifyFieldNotesFields()
    // {
    //     WaitUntilVisible(documentFieldNotesTypeDistrictLotLabel);

    //     AssertTrueIsDisplayed(documentFieldNotesTypeDistrictLotLabel);
    //     AssertTrueIsDisplayed(documentFieldNotesTypeDistrictLotInput);

    //     AssertTrueIsDisplayed(documentFieldNotesTypeYearLabel);
    //     AssertTrueIsDisplayed(documentFieldNotesTypeYearInput);

    //     AssertTrueIsDisplayed(documentFieldNotesTypeLandDistrictLabel);
    //     AssertTrueIsDisplayed(documentFieldNotesTypeLandDistrictInput); 
    // }

    // private void VerifyForm12Fields()
    // {
    //     WaitUntilVisible(documentGazetteDateLabel);

    //     AssertTrueIsDisplayed(documentGazetteDateLabel);
    //     AssertTrueIsDisplayed(documentGazetteDateInput);

    //     AssertTrueIsDisplayed(documentGazetteLegalSurveyPlanLabel);
    //     AssertTrueIsDisplayed(documentLegalSurveyInput);

    //     AssertTrueIsDisplayed(documentMOTIPlanLabel);
    //     AssertTrueIsDisplayed(documentMOTIPlanInput);

    //     AssertTrueIsDisplayed(documentShortDescriptorLabel);
    //     AssertTrueIsDisplayed(documentShortDescriptorInput);
    // }

    // private void VerifyGazetteFields()
    // {
    //     WaitUntilVisible(documentGazetteDateLabel);

    //     AssertTrueIsDisplayed(documentGazetteDateLabel);
    //     AssertTrueIsDisplayed(documentGazetteDateInput);

    //     AssertTrueIsDisplayed(documentGazettePageLabel);
    //     AssertTrueIsDisplayed(documentGazettePageInput);

    //         AssertTrueIsDisplayed(documentGazettePublishedDateLabel);
    //         AssertTrueIsDisplayed(documentGazettePublishedDateInput);

    //         AssertTrueIsDisplayed(documentGazettePublishedTypeLabel);
    //         AssertTrueIsDisplayed(documentGazettePublishedTypeInput);

    //         AssertTrueIsDisplayed(documentGazetteLegalSurveyPlanLabel);
    //         AssertTrueIsDisplayed(documentLegalSurveyInput);

    //         AssertTrueIsDisplayed(documentGazetteLTSALabel);
    //         AssertTrueIsDisplayed(documentGazetteLTSAInput);

    //         AssertTrueIsDisplayed(documentGazetteLegalSurveyMotiPlanLabel);
    //         AssertTrueIsDisplayed(documentMOTIPlanInput);

    //         AssertTrueIsDisplayed(documentRoadNameLabel);
    //         AssertTrueIsDisplayed(documentRoadNameInput);
    // }

    // private void VerifyHistoricalFileFields()
    //     {
    //         WaitUntilVisible(documentHistoricFileTypeEndDateLabel);

    //         AssertTrueIsDisplayed(documentHistoricFileTypeEndDateLabel);
    //         AssertTrueIsDisplayed(documentHistoricFileTypeEndDateInput);

    //         AssertTrueIsDisplayed(documentHistoricFileTypeFileLabel);
    //         AssertTrueIsDisplayed(documentHistoricFileTypeFileInput);

    //         AssertTrueIsDisplayed(documentHistoricFileTypePhyLocationLabel);
    //         AssertTrueIsDisplayed(documentHistoricFileTypePhyLocationInput);

    //         AssertTrueIsDisplayed(documentHistoricFileTypeSectionLabel);
    //         AssertTrueIsDisplayed(documentHistoricFileTypeSectionInput);

    //         AssertTrueIsDisplayed(documentHistoricFileTypeStartDateLabel);
    //         AssertTrueIsDisplayed(documentHistoricFileTypeStartDateInput);
    // }

    // private void VerifyLandActTenureFields()
    //     {
    //         WaitUntilVisible(documentLandActTypeReferenceAgencyLabel);

    //         AssertTrueIsDisplayed(documentLandActTypeReferenceAgencyLabel);
    //         AssertTrueIsDisplayed(documentLandActTypeReferenceAgencyInput);

    //         AssertTrueIsDisplayed(documentLandActTypeReferenceLandsLabel);
    //         AssertTrueIsDisplayed(documentLandActTypeReferenceLandsInput);

    //         AssertTrueIsDisplayed(documentShortDescriptorLabel);
    //         AssertTrueIsDisplayed(documentShortDescriptorInput);
    // }

    // private void VerifyLegalSurveyFields()
    //     {
    //         WaitUntilVisible(documentLegalSurveyNbrLabel);

    //         AssertTrueIsDisplayed(documentLegalSurveyNbrLabel);
    //         AssertTrueIsDisplayed(documentLegalSurveyInput);
    //         AssertTrueIsDisplayed(documentMOTIPlanLabel);
    //         AssertTrueIsDisplayed(documentMOTIPlanInput);
    //         AssertTrueIsDisplayed(documentLegalSurveyPlanTypeLabel);
    //         AssertTrueIsDisplayed(documentLegalSurveyPlanTypeInput);
    // }

    // private void VerifyMinisterialOrderFields()
    //     {
    //         WaitUntilVisible(documentDateSignedLabel);

    //         AssertTrueIsDisplayed(documentDateSignedLabel);
    //         AssertTrueIsDisplayed(documentDateSignedInput);

    //         AssertTrueIsDisplayed(documentMinisterialOrderTypeMOLabel);
    //         AssertTrueIsDisplayed(documentMinisterialOrderTypeMOInput);

    //         AssertTrueIsDisplayed(documentMOTIFileLabel);
    //         AssertTrueIsDisplayed(documentTypeMotiFileInput);

    //         AssertTrueIsDisplayed(documentPropertyIdentifierLabel);
    //         AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);

    //         AssertTrueIsDisplayed(documentRoadNameLabel);
    //         AssertTrueIsDisplayed(documentRoadNameInput);
    // }

    // private void VerifyMiscellaneousNotesFields()
    //     {
    //         WaitUntilVisible(documentMiscNotesTypePIDLabel);

    //         AssertTrueIsDisplayed(documentMiscNotesTypePIDLabel);
    //         AssertTrueIsDisplayed(documentMiscNotesTypePIDInput);
    // }

    // private void VerifyMOTIPlanFields()
    //     {
    //         WaitUntilVisible(documentLegalSurveyNbrLabel);

    //         AssertTrueIsDisplayed(documentLegalSurveyNbrLabel);
    //         AssertTrueIsDisplayed(documentLegalSurveyInput);

    //         AssertTrueIsDisplayed(documentMOTIFileLabel);
    //         AssertTrueIsDisplayed(documentTypeMotiFileInput);

    //         AssertTrueIsDisplayed(documentMOTIPlanLabel);
    //         AssertTrueIsDisplayed(documentMOTIPlanInput);

    //         AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyPublishDateLabel);
    //         AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyPublishDateInput);

    //         AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyRelatedGazetteLabel);
    //         AssertTrueIsDisplayed(documentMoTIPlanLegalSurveyRelatedGazetteInput);
    // }

    // private void VerifyOICFields()
    //     {
    //         WaitUntilVisible(documentOICTypeOICLabel);

    //         AssertTrueIsDisplayed(documentOICTypeOICLabel);
    //         AssertTrueIsDisplayed(documentOICTypeInput);
    //         AssertTrueIsDisplayed(documentOICTypeOICRouteLabel);
    //         AssertTrueIsDisplayed(documentOICTypeOICRouteInput);
    //         AssertTrueIsDisplayed(documentOICTypeOICTypeLabel);
    //         AssertTrueIsDisplayed(documentOICTypeOICTypeInput);
    //         AssertTrueIsDisplayed(documentRoadNameLabel);
    //         AssertTrueIsDisplayed(documentRoadNameInput);
    //         AssertTrueIsDisplayed(documentYearLabel);
    //         AssertTrueIsDisplayed(documentYearInput);
    // }

    // private void VerifyOtherTypeFields()
    //     {
    //         WaitUntilVisible(documentOtherTypePINLabel);

    //         AssertTrueIsDisplayed(documentOtherTypePINLabel);
    //         AssertTrueIsDisplayed(documentOtherTypePINInput);
    //         AssertTrueIsDisplayed(documentOtherTypePropIdLabel);
    //         AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);
    //         AssertTrueIsDisplayed(documentRoadNameLabel);
    //         AssertTrueIsDisplayed(documentRoadNameInput);
    //         AssertTrueIsDisplayed(documentShortDescriptorLabel);
    //         AssertTrueIsDisplayed(documentShortDescriptorInput);
    // }

    // private void VerifyPAPlansFields()
    //     {
    //         WaitUntilVisible(documentPAPlanNbrLabel);

    //         AssertTrueIsDisplayed(documentPAPlanNbrLabel);
    //         AssertTrueIsDisplayed(documentPAPlanNbrInput);

    //         AssertTrueIsDisplayed(documentPAPlanRevisionLabel);
    //         AssertTrueIsDisplayed(documentPAPlanRevisionInput);
    //         AssertTrueIsDisplayed(documentPAPlanProjectLabel);
    //         AssertTrueIsDisplayed(documentPAPlanProjectInput);

    //         AssertTrueIsDisplayed(documentPAPlanProjectNameLabel);
    //         AssertTrueIsDisplayed(documentPAPlanProjectNameInput);
    // }

    // private void VerifyPhotosCorrespondenceFields()
    //     {
    //         WaitUntilVisible(documentCivicAddressLabel);

    //         AssertTrueIsDisplayed(documentCivicAddressLabel);
    //         AssertTrueIsDisplayed(documentCivicAddressInput);

    //         AssertTrueIsDisplayed(documentPhotosCorrespondenceTypeDateLabel);
    //         AssertTrueIsDisplayed(documentPhotosCorrespondenceTypeDateInput);

    //         AssertTrueIsDisplayed(documentOwnerLabel);
    //         AssertTrueIsDisplayed(documentTypeOwnerInput);

    //         AssertTrueIsDisplayed(documentPhotosCorrespondenceTypePropIdLabel);
    //         AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);

    //         AssertTrueIsDisplayed(documentShortDescriptorLabel);
    //         AssertTrueIsDisplayed(documentShortDescriptorInput);
    // }

    // private void VerifyPrivyCouncilFields()
    //     {
    //         WaitUntilVisible(documentPrivyCouncilTypePrivyLabel);

    //         AssertTrueIsDisplayed(documentPrivyCouncilTypePrivyLabel);
    //         AssertTrueIsDisplayed(documentPrivyCouncilTypePrivyInput);
    // }

    // private void VerifyShortDescriptorField()
    //     {
    //         WaitUntilVisible(documentShortDescriptorLabel);

    //         AssertTrueIsDisplayed(documentShortDescriptorLabel);
    //         AssertTrueIsDisplayed(documentShortDescriptorInput);
    // }

    // private void VerifyTitleSearchFields()
    //     {
    //         WaitUntilVisible(documentOwnerLabel);

    //         AssertTrueIsDisplayed(documentOwnerLabel);
    //         AssertTrueIsDisplayed(documentTypeOwnerInput);
    //         AssertTrueIsDisplayed(documentTitleSearchTypePIDLabel);
    //         AssertTrueIsDisplayed(documentMiscNotesTypePIDInput);
    //         AssertTrueIsDisplayed(documentTitleSearchTypeTitleLabel);
    //         AssertTrueIsDisplayed(documentTitleSearchTypeTitleInput);
    // }

    // private void VerifyTransferAdministrationFields()
    //     {
    //         WaitUntilVisible(documentDateSignedLabel);

    //         AssertTrueIsDisplayed(documentDateSignedLabel);
    //         AssertTrueIsDisplayed(documentDateSignedInput);

    //         AssertTrueIsDisplayed(documentMOTIFileLabel);
    //         AssertTrueIsDisplayed(documentTypeMotiFileInput);

    //         AssertTrueIsDisplayed(documentTransferAdmTypeProIdLabel);
    //         AssertTrueIsDisplayed(documentTypePropertyIdentifierInput);

    //         AssertTrueIsDisplayed(documentRoadNameLabel);
    //         AssertTrueIsDisplayed(documentRoadNameInput);

    //         AssertTrueIsDisplayed(documentTransferAdmTypeTransferLabel);
    //         AssertTrueIsDisplayed(documentTransferAdmTypeTransferInput);
    // }
}

module.exports = { DigitalDocuments };
