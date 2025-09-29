const { expect } = require("@playwright/test");

class DigitalDocuments {
  constructor(page) {
    this.page = page;
  }

  async navigateDocumentsTab() {
    const documentsTab = this.page.locator("a[data-rb-event-key='documents']");
    await documentsTab.waitFor({ state: "visible" });
    await documentsTab.click();
  }

  async navigatePropertyDocumentsTab() {
    const propsDocumentsTab = this.page.locator(
      "a[data-rb-event-key='document']"
    );
    await propsDocumentsTab.waitFor({ state: "visible" });
    await propsDocumentsTab.click();
  }

  async navigateToFirstPageDocumentsTable() {
    const documentPaginationPrevPageLink = this.page.locator(
      "ul.pagination a[aria-label='Previous page']"
    );
    await documentPaginationPrevPageLink.waitFor({ state: "visible" });
    await documentPaginationPrevPageLink.click();
  }

  async addNewDocumentButton() {
    const addDocumentBttn = this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button"
    );
    await addDocumentBttn.waitFor({ state: "visible" });
    await addDocumentBttn.click();
  }

  async verifyDocumentsListView() {
    const documentsTitle = await this.page.locator(
      "//span[contains(text(),'Documents')]"
    );
    expect(documentsTitle).toBeVisible();

    const addDocumentBttn = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button"
    );
    expect(addDocumentBttn).toBeVisible();

    const documentFilterTypeSelect = await this.page.locator(
      "//select[@data-testid='document-type']"
    );
    expect(documentFilterTypeSelect).toBeVisible();

    const documentFilterStatusSelect = await this.page.locator(
      "//select[@data-testid='document-status']"
    );
    expect(documentFilterStatusSelect).toBeVisible();

    const documentFilterNameInput = await this.page.locator(
      "//input[@data-testid='document-filename']"
    );
    expect(documentFilterNameInput).toBeVisible();

    const documentFilterSearchBttn = await this.page.locator(
      "//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='search']"
    );
    expect(documentFilterSearchBttn).toBeVisible();

    const documentFilterResetBttn = await this.page.locator(
      "//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='reset-button']"
    );
    expect(documentFilterResetBttn).toBeVisible();

    const documentTableListView = await this.page.locator(
      "//div[@data-testid='documentsTable']"
    );
    expect(documentTableListView).toBeVisible();

    const documentTableTypeColumn = await this.page.locator(
      "//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document type')]"
    );
    expect(documentTableTypeColumn).toBeVisible();

    const documentTableNameColumn = await this.page.locator(
      "//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document name')]"
    );
    expect(documentTableNameColumn).toBeVisible();

    const documentTableDateColumn = await this.page.locator(
      "//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Uploaded')]"
    );
    expect(documentTableDateColumn).toBeVisible();

    const documentTableStatusColumn = await this.page.locator(
      "//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Status')]"
    );
    expect(documentTableStatusColumn).toBeVisible();

    const documentTableActionsColumn = await this.page.locator(
      "//div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Actions')]"
    );
    expect(documentTableActionsColumn).toBeVisible();
  }

  async digitalDocumentsTableResultNumber() {
    await this.page.locator(
      "div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    const totalDigitalDocumentsCount = await this.page
      .locator(
        "div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
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
    const fileType = await this.page.locator(
      "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[1]"
    );
    fileType.waitFor({ status: "visible" });
    return fileType.textContent();
  }

  async firstDocumentFileName() {
    const documentName = await this.page.locator(
      "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[2]/div/button/div"
    );
    documentName.waitFor({ status: "visible" });
    return documentName.textContent();
  }

  async firstDocumentFileStatus() {
    const fileStatus = await this.page.locator(
      "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[4]"
    );
    fileStatus.waitFor({ status: "visible" });
    return fileStatus.textContent();
  }

  async filterDocuments({
    documentType = "",
    documentStatus = "",
    documentName = "",
  }) {
    await this.page
      .locator(
        "//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='reset-button']"
      )
      .click();

    if (documentType != "") {
      const documentTypeElement = await this.page.getByTestId("document-type");
      documentTypeElement.waitFor({ status: "visible" });
      documentTypeElement.selectOption({ label: documentType });
    }

    if (documentStatus != "") {
      const documentStatusElement = await this.page.getByTestId(
        "document-status"
      );
      documentStatusElement.waitFor({ status: "visible" });
      documentStatusElement.selectOption({ label: documentStatus });
    }

    if (documentName != "") {
      const documentNameElement = await this.page.getByTestId(
        "document-filename"
      );
      documentNameElement.waitFor({ status: "visible" });
      documentNameElement.fill(documentName);
    }

    const searchButton = await this.page.locator(
      "//input[@id='input-filename']/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div/div/button[@data-testid='search']"
    );
    searchButton.waitFor({ status: "visible" });
    searchButton.click();
  }

  async totalSearchDocuments() {
    const totalSearchDocuments = await this.page.locator(
      "div[data-testid='documentsTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    totalSearchDocuments.waitFor({ status: "visible" });
    return totalSearchDocuments.count();
  }

  async uploadDocument(documentFile) {
    await this.page.getByTestId("upload-input").fill(documentFile);
  }

  async saveDigitalDocumentUpload() {
    this.sharedModals.mainModalClickOKBttn();

    const confirmationMessage = await this.page.locator(
      "//span[contains(text(),'files successfully uploaded')]"
    );
    confirmationMessage.waitFor({ status: "visible" });
    this.sharedModals.mainModalClickOKBttn();
  }

  async saveUpdateDigitalDocument() {
    const saveButton = await this.page.locator(
      "//div[@class='modal-body']/div/div[2]/div/div/div/div/button/div[contains(text(),'Yes')]/parent::button"
    );
    saveButton.waitFor({ status: "clickable" });
    saveButton.click();
  }

  async cancelDigitalDocument() {
    await this.page.locator("button[title='cancel-modal']").click();

    const confirmationModal = await this.page.locator(
      "//div[contains(text(),'Confirm Changes')]/parent::div/parent::div"
    );
    if (confirmationModal.count() > 0) {
      const modalBody = await this.page.locator(
        "//div[contains(text(),'Confirm Changes')]/parent::div/following-sibling::div[@class='modal-body']"
      );
      expect(modalBody).toContain(
        "If you choose to cancel now, your changes will not be saved."
      );
      expect(modalBody).toContain("Do you want to proceed?");

      await this.page
        .locator("div[class='modal-content'] button[title='ok-modal']")
        .click();
    } else {
      const cancelWarning = await this.page.locator(
        "//div[@class='modal-footer']/div[@class='button-wrap']/p"
      );
      cancelWarning.waitFor({ status: "visible" });

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
    await this.page
      .locator(
        "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']"
      )
      .click();
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
        .locator(
          "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']"
        )
        .isVisible();

      if (isDetailsBttnVisible) {
        console.log("Details button is now visible!");
        break;
      }

      console.log("Details button not yet visible, retrying...");
      await this.page.waitForTimeout(refreshInterval);
    }

    // Final check after loop
    await expect(
      this.page.locator(
        "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']"
      )
    ).toBeVisible({ timeout: 5000 });
  }

  async viewUploadedDocument(index) {
    const firstViewButton = await this.page.locator(
      "//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@data-testid='document-view-button']"
    );
    firstViewButton.waitFor({ status: "visible" });

    if (index > 9) {
      await this.page
        .locator("ul[class='pagination'] a[aria-label='Next page']")
        .click();
    }

    const elementChild = (index % 10) + 1;
    await this.page
      .locator(
        `//div[@data-testid='documentsTable']/div[@class='tbody']/div[${elementChild}]/div/div[5]/div/button[@data-testid='document-view-button']`
      )
      .click();
  }

  async delete1stDocument() {
    WaitUntilVisible(documentTableResults1stDeleteBttn);
    webDriver.FindElement(documentTableResults1stDeleteBttn).Click();

    WaitUntilVisible(documentDeleteHeader);
    AssertTrueContentEquals(documentDeleteHeader, "Delete a document");
    AssertTrueContentEquals(
      documentDeleteContent1,
      "You have chosen to delete this document."
    );
    AssertTrueContentEquals(
      documentDeteleContent2,
      "If the document is linked to other files or entities in PIMS it will still be accessible from there, however if this the only instance then the file will be removed from the document store completely."
    );
    AssertTrueContentEquals(
      documentDeleteContent3,
      "Do you wish to continue deleting this document?"
    );

    webDriver.FindElement(documentDeleteOkBttn).Click();

    WaitUntilDisappear(documentTableWaitSpinner);
  }

  async editDocumentButton() {
    Wait(2000);
    FocusAndClick(documentEditBttn);
  }

  async insertDocumentTypeStatus(document, docIdx) {
    const docTypeSelectElement = await this.page.locator(
      `#input-documents.${docIdx}.documentTypeId`
    );
    docTypeSelectElement.waitFor({ status: "visible" });
    docTypeSelectElement.selectOption({ label: document.DocumentType });

    const statusSelectElement = await this.page.locator(
      `#input-documents." ${docIdx}.documentStatusCode`
    );
    statusSelectElement.selectOption({ label: document.DocumentStatus });

    await this.page
      .locator(
        `//select[@data-testid='documents.${docIdx}.document-type']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/*[1]`
      )
      .click();
  }

  async insertDocumentTypeDetails(document) {
    if (document.ApplicationNumber) {
      const documentALCTypeAppNumberInput = await this.page.getByTestId(
        "metadata-input-APPLICATION_NUMBER"
      );
      documentALCTypeAppNumberInput.awaitFor({ status: "visible" });
      documentALCTypeAppNumberInput.fill(document.ApplicationNumber);
    }

    if (document.CanadaLandSurvey) {
      const documentCanLandSurveyTypeCanLandSurveyInput =
        await this.page.getByTestId("metadata-input-CANADA_LAND_SURVEY_NUMBER");
      documentCanLandSurveyTypeCanLandSurveyInput.awaitFor({
        status: "visible",
      });
      documentCanLandSurveyTypeCanLandSurveyInput.fill(
        document.CanadaLandSurvey
      );
    }

    if (document.CivicAddress) {
      const documentCivicAddressInput = await this.page.getByTestId(
        "metadata-input-CIVIC_ADDRESS"
      );
      documentCivicAddressInput.awaitFor({ status: "visible" });
      documentCivicAddressInput.fill(document.CivicAddress);
    }

    if (document.CrownGrant) {
      const documentCrownGrantTypeCrownInput = await this.page.getByTestId(
        "metadata-input-CROWN_GRANT_NUMBER"
      );
      documentCrownGrantTypeCrownInput.awaitFor({ status: "visible" });
      documentCrownGrantTypeCrownInput.fill(document.CrownGrant);
    }

    if (document.Date) {
      const documentPhotosCorrespondenceTypeDateInput =
        await this.page.getByTestId("metadata-input-DATE");
      documentPhotosCorrespondenceTypeDateInput.awaitFor({ status: "visible" });
      documentPhotosCorrespondenceTypeDateInput.fill(document.Date);
    }

    if (document.DateSigned) {
      const documentDateSignedInput = await this.page.getByTestId(
        "metadata-input-DATE_SIGNED"
      );
      documentDateSignedInput.awaitFor({ status: "visible" });
      documentDateSignedInput.fill(document.DateSigned);
    }

    if (document.DistrictLot) {
      const documentFieldNotesTypeDistrictLotInput =
        await this.page.getByTestId("metadata-input-DISTRICT_LOT_NUMBER");
      documentFieldNotesTypeDistrictLotInput.awaitFor({ status: "visible" });
      documentFieldNotesTypeDistrictLotInput.fill(document.DistrictLot);
    }

    if (document.ElectoralDistrict) {
      const documentDistrictRoadRegisterTypeElectoralDistrictInput =
        await this.page.getByTestId("metadata-input-ELECTORAL_DISTRICT");
      documentDistrictRoadRegisterTypeElectoralDistrictInput.awaitFor({
        status: "visible",
      });
      documentDistrictRoadRegisterTypeElectoralDistrictInput.fill(
        document.ElectoralDistrict
      );
    }

    if (document.EndDate) {
      const documentHistoricFileTypeEndDateInput = await this.page.getByTestId(
        "metadata-input-END_DATE"
      );
      documentHistoricFileTypeEndDateInput.awaitFor({ status: "visible" });
      documentHistoricFileTypeEndDateInput.fill(document.EndDate);
    }

    if (document.FieldBook) {
      const documentFieldNotesTypeYearInput = await this.page.getByTestId(
        "metadata-input-FIELD_BOOK_NUMBER_YEAR"
      );
      documentFieldNotesTypeYearInput.awaitFor({ status: "visible" });
      documentFieldNotesTypeYearInput.fill(document.FieldBook);
    }

    if (document.File) {
      const documentHistoricFileTypeFileInput = await this.page.getByTestId(
        "metadata-input-FILE_NUMBER"
      );
      documentHistoricFileTypeFileInput.awaitFor({ status: "visible" });
      documentHistoricFileTypeFileInput.fill(document.File);
    }

    if (document.GazetteDate) {
      const documentGazetteDateInput = await this.page.getByTestId(
        "metadata-input-GAZETTE_DATE"
      );
      documentGazetteDateInput.awaitFor({ status: "visible" });
      documentGazetteDateInput.fill(document.GazetteDate);
    }

    if (document.GazettePage) {
      const documentGazettePageInput = await this.page.getByTestId(
        "metadata-input-GAZETTE_PAGE_NUMBER"
      );
      documentGazettePageInput.awaitFor({ status: "visible" });
      documentGazettePageInput.fill(document.GazettePage);
    }

    if (document.GazettePublishedDate) {
      const documentGazettePublishedDateInput = await this.page.getByTestId(
        "metadata-input-GAZETTE_PUBLISHED_DATE"
      );
      documentGazettePublishedDateInput.awaitFor({ status: "visible" });
      documentGazettePublishedDateInput.fill(document.GazettePublishedDate);
    }

    if (document.GazetteType) {
      const documentGazettePublishedTypeInput = await this.page.getByTestId(
        "metadata-input-GAZETTE_TYPE"
      );
      documentGazettePublishedTypeInput.awaitFor({ status: "visible" });
      documentGazettePublishedTypeInput.fill(document.GazetteType);
    }

    if (document.HighwayDistrict) {
      const documentDistrictRoadRegisterTypeHighwayDistrictInput =
        await this.page.getByTestId("metadata-input-HIGHWAY_DISTRICT");
      documentDistrictRoadRegisterTypeHighwayDistrictInput.awaitFor({
        status: "visible",
      });
      documentDistrictRoadRegisterTypeHighwayDistrictInput.fill(
        document.HighwayDistrict
      );
    }

    if (document.IndianReserveOrNationalPark) {
      const documentCanLandSurveyTypeIndianReserveInput =
        await this.page.getByTestId(
          "metadata-input-INDIAN_RESERVE_OR_NATIONAL_PARK"
        );
      documentCanLandSurveyTypeIndianReserveInput.awaitFor({
        status: "visible",
      });
      documentCanLandSurveyTypeIndianReserveInput.fill(
        document.IndianReserveOrNationalPark
      );
    }

    if (document.Jurisdiction) {
      const documentBCAssessmentTypeJurisdictionInput =
        await this.page.getByTestId("metadata-input-JURISDICTION");
      documentBCAssessmentTypeJurisdictionInput.awaitFor({ status: "visible" });
      documentBCAssessmentTypeJurisdictionInput.fill(document.Jurisdiction);
    }

    if (document.LandDistrict) {
      const documentFieldNotesTypeLandDistrictInput =
        await this.page.getByTestId("metadata-input-LAND_DISTRICT");
      documentFieldNotesTypeLandDistrictInput.awaitFor({ status: "visible" });
      documentFieldNotesTypeLandDistrictInput.fill(document.LandDistrict);
    }

    if (document.LegalSurveyPlan) {
      const documentLegalSurveyInput = await this.page.getByTestId(
        "metadata-input-LEGAL_SURVEY_PLAN_NUMBER"
      );
      documentLegalSurveyInput.awaitFor({ status: "visible" });
      documentLegalSurveyInput.fill(document.LegalSurveyPlan);
    }

    if (document.LTSAScheduleFiling) {
      const documentGazetteLTSAInput = await this.page.getByTestId(
        "metadata-input-LTSA_SCHEDULE_FILING_NUMBER"
      );
      documentGazetteLTSAInput.awaitFor({ status: "visible" });
      documentGazetteLTSAInput.fill(document.LTSAScheduleFiling);
    }

    if (document.MO) {
      const documentMinisterialOrderTypeMOInput = await this.page.getByTestId(
        "metadata-input-MO_NUMBER"
      );
      documentMinisterialOrderTypeMOInput.awaitFor({ status: "visible" });
      documentMinisterialOrderTypeMOInput.fill(document.MO);
    }

    if (document.MoTIFile) {
      const documentTypeMotiFileInput = await this.page.getByTestId(
        "metadata-input-MOTI_FILE_NUMBER"
      );
      documentTypeMotiFileInput.awaitFor({ status: "visible" });
      documentTypeMotiFileInput.fill(document.MoTIFile);
    }

    if (document.MoTIPlan) {
      const documentMOTIPlanInput = await this.page.getByTestId(
        "metadata-input-MOTI_PLAN_NUMBER"
      );
      documentMOTIPlanInput.awaitFor({ status: "visible" });
      documentMOTIPlanInput.fill(document.MoTIPlan);
    }

    if (document.OIC) {
      const documentOICTypeInput = await this.page.getByTestId(
        "metadata-input-OIC_NUMBER"
      );
      documentOICTypeInput.awaitFor({ status: "visible" });
      documentOICTypeInput.fill(document.OIC);
    }

    if (document.OICRoute) {
      const documentOICTypeOICRouteInput = await this.page.getByTestId(
        "metadata-input-OIC_ROUTE_NUMBER"
      );
      documentOICTypeOICRouteInput.awaitFor({ status: "visible" });
      documentOICTypeOICRouteInput.fill(document.OICRoute);
    }

    if (document.OICType) {
      const documentOICTypeOICTypeInput = await this.page.getByTestId(
        "metadata-input-OIC_TYPE"
      );
      documentOICTypeOICTypeInput.awaitFor({ status: "visible" });
      documentOICTypeOICTypeInput.fill(document.OICType);
    }

    if (document.Owner) {
      const documentTypeOwnerInput = await this.page.getByTestId(
        "metadata-input-OWNER"
      );
      documentTypeOwnerInput.awaitFor({ status: "visible" });
      documentTypeOwnerInput.fill(document.Owner);
    }

    if (document.PhysicalLocation) {
      const documentHistoricFileTypePhyLocationInput =
        await this.page.getByTestId("metadata-input-PHYSICAL_LOCATION");
      documentHistoricFileTypePhyLocationInput.awaitFor({ status: "visible" });
      documentHistoricFileTypePhyLocationInput.fill(document.PhysicalLocation);
    }

    if (document.PIDNumber) {
      const documentTypePropIdInput = await this.page.getByTestId(
        "metadata-input-PROPERTY_IDENTIFIER"
      );
      documentTypePropIdInput.awaitFor({ status: "visible" });
      documentTypePropIdInput.fill(document.PIDNumber);
    }

    if (document.PINNumber) {
      const documentOtherTypePINInput = await this.page.getByTestId(
        "metadata-input-PIN"
      );
      documentOtherTypePINInput.awaitFor({ status: "visible" });
      documentOtherTypePINInput.fill(document.PINNumber);
    }

    if (document.Plan) {
      const documentPAPlanNbrInput = await this.page.getByTestId(
        "metadata-input-PLAN_NUMBER"
      );
      documentPAPlanNbrInput.awaitFor({ status: "visible" });
      documentPAPlanNbrInput.fill(document.Plan);
    }

    if (document.PlanRevision) {
      const documentPAPlanRevisionInput = await this.page.getByTestId(
        "metadata-input-PLAN_REVISION"
      );
      documentPAPlanRevisionInput.awaitFor({ status: "visible" });
      documentPAPlanRevisionInput.fill(document.PlanRevision);
    }

    if (document.PlanType) {
      const documentLegalSurveyPlanTypeInput = await this.page.getByTestId(
        "metadata-input-PLAN_TYPE"
      );
      documentLegalSurveyPlanTypeInput.awaitFor({ status: "visible" });
      documentLegalSurveyPlanTypeInput.fill(document.PlanType);
    }

    if (document.Project) {
      const documentPAPlanProjectInput = await this.page.getByTestId(
        "metadata-input-PROJECT_NUMBER"
      );
      documentPAPlanProjectInput.awaitFor({ status: "visible" });
      documentPAPlanProjectInput.fill(document.Project);
    }

    if (document.ProjectName) {
      const documentPAPlanProjectNameInput = await this.page.getByTestId(
        "metadata-input-PROJECT_NAME"
      );
      documentPAPlanProjectNameInput.awaitFor({ status: "visible" });
      documentPAPlanProjectNameInput.fill(document.ProjectName);
    }

    if (document.PropertyIdentifier) {
      const documentTypePropertyIdentifierInput = await this.page.getByTestId(
        "metadata-input-PROPERTY_IDENTIFIER"
      );
      documentTypePropertyIdentifierInput.awaitFor({ status: "visible" });
      documentTypePropertyIdentifierInput.fill(document.PropertyIdentifier);
    }

    if (document.PublishedDate) {
      const documentMoTIPlanLegalSurveyPublishDateInput =
        await this.page.getByTestId("metadata-input-PUBLISHED_DATE");
      documentMoTIPlanLegalSurveyPublishDateInput.awaitFor({
        status: "visible",
      });
      documentMoTIPlanLegalSurveyPublishDateInput.fill(document.PublishedDate);
    }

    if (document.ReferenceAgencyDocumentNbr) {
      const documentLandActTypeReferenceAgencyInput =
        await this.page.getByTestId("metadata-input-REFAG_DOC_NUMBER");
      documentLandActTypeReferenceAgencyInput.awaitFor({ status: "visible" });
      documentLandActTypeReferenceAgencyInput.fill(
        document.ReferenceAgencyDocumentNbr
      );
    }

    if (document.ReferenceAgencyLandsFileNbr) {
      const documentLandActTypeReferenceLandsInput =
        await this.page.getByTestId("metadata-input-REFAG_LANDFILE_NUMBER");
      documentLandActTypeReferenceLandsInput.awaitFor({ status: "visible" });
      documentLandActTypeReferenceLandsInput.fill(
        document.ReferenceAgencyLandsFileNbr
      );
    }

    if (document.RelatedGazette) {
      const documentMoTIPlanLegalSurveyRelatedGazetteInput =
        await this.page.getByTestId("metadata-input-RELATED_GAZETTE");
      documentMoTIPlanLegalSurveyRelatedGazetteInput.awaitFor({
        status: "visible",
      });
      documentMoTIPlanLegalSurveyRelatedGazetteInput.fill(
        document.RelatedGazette
      );
    }

    if (document.RoadName) {
      const documentRoadNameInput = await this.page.getByTestId(
        "metadata-input-ROAD_NAME"
      );
      documentRoadNameInput.awaitFor({ status: "visible" });
      documentRoadNameInput.fill(document.RoadName);
    }

    if (document.Roll) {
      const documentBCAssessmentTypeRollInput = await this.page.getByTestId(
        "metadata-input-ROLL_NUMBER"
      );
      documentBCAssessmentTypeRollInput.awaitFor({ status: "visible" });
      documentBCAssessmentTypeRollInput.fill(document.Roll);
    }

    if (document.Section) {
      const documentHistoricFileTypeSectionInput = await this.page.getByTestId(
        "metadata-input-SECTION_NUMBER"
      );
      documentHistoricFileTypeSectionInput.awaitFor({ status: "visible" });
      documentHistoricFileTypeSectionInput.fill(document.Section);
    }

    if (document.ShortDescriptor) {
      const documentShortDescriptorInput = await this.page.getByTestId(
        "metadata-input-SHORT_DESCRIPTOR"
      );
      documentShortDescriptorInput.awaitFor({ status: "visible" });
      documentShortDescriptorInput.fill(document.ShortDescriptor);
    }

    if (document.StartDate) {
      const documentHistoricFileTypeStartDateInput =
        await this.page.getByTestId("metadata-input-START_DATE");
      documentHistoricFileTypeStartDateInput.awaitFor({ status: "visible" });
      documentHistoricFileTypeStartDateInput.fill(document.StartDate);
    }

    if (document.Title) {
      const documentTitleSearchTypeTitleInput = await this.page.getByTestId(
        "metadata-input-TITLE_NUMBER"
      );
      documentTitleSearchTypeTitleInput.awaitFor({ status: "visible" });
      documentTitleSearchTypeTitleInput.fill(document.Title);
    }

    if (document.Transfer) {
      const documentTransferAdmTypeTransferInput = await this.page.getByTestId(
        "metadata-input-TRANSFER_NUMBER"
      );
      documentTransferAdmTypeTransferInput.awaitFor({ status: "visible" });
      documentTransferAdmTypeTransferInput.fill(document.Transfer);
    }

    if (document.Year) {
      const documentYearInput = await this.page.getByTestId(
        "metadata-input-YEAR"
      );
      documentYearInput.awaitFor({ status: "visible" });
      documentYearInput.fill(document.Year);
    }

    if (document.YearPrivyCouncil) {
      const documentPrivyCouncilTypePrivyInput = await this.page.getByTestId(
        "metadata-input-YEAR_PRIVY_COUNCIL_NUMBER"
      );
      documentPrivyCouncilTypePrivyInput.awaitFor({ status: "visible" });
      documentPrivyCouncilTypePrivyInput.fill(document.YearPrivyCouncil);
    }
  }

  async verifyDocumentDetailsViewForm(document) {
    //Header
    const documentViewDocumentTypeLabel = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Document type')]"
    );
    documentViewDocumentTypeLabel.awaitFor({ status: "visible" });

    const documentViewDocumentTypeContent = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/div/label[contains(text(),'Document type')]/parent::div/following-sibling::div"
    );
    expect(documentViewDocumentTypeContent).toHaveText(document.DocumentType);

    const documenyViewDocumentNameLabel = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/label[contains(text(),'File name')]"
    );
    documenyViewDocumentNameLabel.awaitFor({ status: "visible" });

    const documentViewFileNameContent = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/label[contains(text(),'File name')]/parent::div/following-sibling::div"
    );
    expect(documentViewFileNameContent).not.toHaveText("");

    //Document Information
    const documentViewInfoSubtitle = await this.page.locator(
      "//div[contains(text(),'Document Information')]"
    );
    documentViewInfoSubtitle.awaitFor({ status: "visible" });

    const documentViewDocumentInfoTooltip = await this.page.getByTestId(
      "tooltip-icon-documentInfoToolTip"
    );
    documentViewDocumentInfoTooltip.awaitFor({ status: "visible" });

    const documentEditBttn = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/button"
    );
    documentEditBttn.awaitFor({ status: "visible" });

    const documentViewStatusLabel = await this.page.locator(
      "//div[contains(text(),'Document Information')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]"
    );
    documentViewStatusLabel.awaitFor({ status: "visible" });

    const documentViewStatusContent = await this.page.locator(
      "//div[contains(text(),'Document Information')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]/parent::div/following-sibling::div"
    );
    expect(documentViewStatusContent).toHaveText(document.DocumentStatus);

    //Document Details
    const documentViewDetailsSubtitle = await this.page.locator(
      "//h3[contains(text(),'Details')]"
    );
    documentViewDetailsSubtitle.awaitFor({ status: "visible" });

    if (document.ApplicationNumber) {
      const documentALCTypeAppNumberLabel = await this.page.locator(
        "//label[contains(text(),'Application #')]"
      );
      documentALCTypeAppNumberLabel.awaitFor({ status: "visible" });

      const documentViewApplicationNbrContent = await this.page.locator(
        "//label[contains(text(),'Application #')]/parent::div/following-sibling::div"
      );
      expect(documentViewApplicationNbrContent).toHaveText(
        document.ApplicationNumber
      );
    }

    if (document.CanadaLandSurvey) {
      const documentCanLandSurveyTypeCanLandSurveyLabel =
        await this.page.locator(
          "//label[contains(text(),'Canada land survey')]"
        );
      documentCanLandSurveyTypeCanLandSurveyLabel.awaitFor({
        status: "visible",
      });

      const documentViewCanadaLandSurveyContent = await this.page.locator(
        "//label[contains(text(),'Canada land survey')]/parent::div/following-sibling::div"
      );
      expect(documentViewCanadaLandSurveyContent).toHaveText(
        document.CanadaLandSurvey
      );
    }

    if (document.CivicAddress) {
      const documentCivicAddressLabel = await this.page.locator(
        "//label[contains(text(),'Civic address')]"
      );
      documentCivicAddressLabel.awaitFor({ status: "visible" });

      const documentViewCivicAddressContent = await this.page.locator(
        "//label[contains(text(),'Civic address')]/parent::div/following-sibling::div"
      );
      expect(documentViewCivicAddressContent).toHaveText(document.CivicAddress);
    }

    if (document.CrownGrant) {
      const documentCrownGrantTypeCrownLabel = await this.page.locator(
        "//label[contains(text(),'Crown grant #')]"
      );
      documentCrownGrantTypeCrownLabel.awaitFor({ status: "visible" });

      const documentViewCrownGrantContent = await this.page.locator(
        "//label[contains(text(),'Crown grant #')]/parent::div/following-sibling::div"
      );
      expect(documentViewCrownGrantContent).toHaveText(document.CrownGrant);
    }

    if (document.Date) {
      const documentPhotosCorrespondenceTypeDateLabel = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Date')]"
      );
      documentPhotosCorrespondenceTypeDateLabel.awaitFor({ status: "visible" });

      const documentViewDateContent = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Date')]/parent::div/following-sibling::div"
      );
      expect(documentViewDateContent).toHaveText(document.Date);
    }

    if (document.DateSigned) {
      const documentDateSignedLabel = await this.page.locator(
        "//label[contains(text(),'Date signed')]"
      );
      documentDateSignedLabel.awaitFor({ status: "visible" });

      const documentViewDateSignedContent = await this.page.locator(
        "//label[contains(text(),'Date signed')]/parent::div/following-sibling::div"
      );
      expect(documentViewDateSignedContent).toHaveText(document.DateSigned);
    }

    if (document.DistrictLot) {
      const documentFieldNotesTypeDistrictLotLabel = await this.page.locator(
        "//input[@data-testid='metadata-input-DISTRICT_LOT_NUMBER']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'District lot')]"
      );
      documentFieldNotesTypeDistrictLotLabel.awaitFor({ status: "visible" });

      const documentViewDistrictLotContent = await this.page.locator(
        "//label[contains(text(),'District lot')]/parent::div/following-sibling::div"
      );
      expect(documentViewDistrictLotContent).toHaveText(document.DistrictLot);
    }

    if (document.ElectoralDistrict) {
      const documentDistrictRoadRegisterTypeElectoralDistrictLabel =
        await this.page.locator(
          "//label[contains(text(),'Electoral district')]"
        );
      documentDistrictRoadRegisterTypeElectoralDistrictLabel.awaitFor({
        status: "visible",
      });

      const documentViewElectoralDistrictContent = await this.page.locator(
        "//label[contains(text(),'Electoral district')]/parent::div/following-sibling::div"
      );
      expect(documentViewElectoralDistrictContent).toHaveText(
        document.ElectoralDistrict
      );
    }

    if (document.EndDate) {
      const documentHistoricFileTypeEndDateLabel = await this.page.locator(
        "//label[contains(text(),'End date')]"
      );
      documentHistoricFileTypeEndDateLabel.awaitFor({ status: "visible" });

      const documentViewEndDateContent = await this.page.locator(
        "//label[contains(text(),'End date')]/parent::div/following-sibling::div"
      );
      expect(documentViewEndDateContent).toHaveText(document.EndDate);
    }

    if (document.FieldBook) {
      const documentFieldNotesTypeYearLabel = await this.page.locator(
        "//label[contains(text(),'Field book #/Year')]"
      );
      documentFieldNotesTypeYearLabel.awaitFor({ status: "visible" });

      const documentViewFieldBookContent = await this.page.locator(
        "//label[contains(text(),'Field book #/Year')]/parent::div/following-sibling::div"
      );
      expect(documentViewFieldBookContent).toHaveText(document.FieldBook);
    }

    if (document.File) {
      const documentHistoricFileTypeFileLabel = await this.page.locator(
        "//div[@class='pr-0 text-left col-4']/label[contains(text(),'File #')]"
      );
      documentHistoricFileTypeFileLabel.awaitFor({ status: "visible" });

      const documentViewFileNumberContent = await this.page.locator(
        "//div[@class='pr-0 text-left col-4']/label[contains(text(),'File #')]/parent::div/following-sibling::div"
      );
      expect(documentViewFileNumberContent).toHaveText(document.File);
    }

    if (document.GazetteDate) {
      const documentGazetteDateLabel = await this.page.locator(
        "//label[contains(text(),'Gazette date')]"
      );
      documentGazetteDateLabel.awaitFor({ status: "visible" });

      const documentViewGazetteDateContent = await this.page.locator(
        "//label[contains(text(),'Gazette date')]/parent::div/following-sibling::div"
      );
      expect(documentViewGazetteDateContent).toHaveText(document.GazetteDate);
    }

    if (document.GazettePage) {
      const documentGazettePageLabel = await this.page.locator(
        "//label[contains(text(),'Gazette page #')]"
      );
      documentGazettePageLabel.awaitFor({ status: "visible" });

      const documentViewGazettePageContent = await this.page.locator(
        "//label[contains(text(),'Gazette page #')]/parent::div/following-sibling::div"
      );
      expect(documentViewGazettePageContent).toHaveText(document.GazettePage);
    }

    if (document.GazettePublishedDate) {
      const documentGazettePublishedDateLabel = await this.page.locator(
        "//label[contains(text(),'Gazette published date')]"
      );
      documentGazettePublishedDateLabel.awaitFor({ status: "visible" });

      const documentViewGazettePublishedDateContent = await this.page.locator(
        "//label[contains(text(),'Gazette published date')]/parent::div/following-sibling::div"
      );
      expect(documentViewGazettePublishedDateContent).toHaveText(
        document.GazettePublishedDate
      );
    }

    if (document.GazetteType) {
      const documentGazettePublishedTypeLabel = await this.page.locator(
        "//label[contains(text(),'Gazette type')]"
      );
      documentGazettePublishedTypeLabel.awaitFor({ status: "visible" });

      const documentViewGazettePublishedTypeContent = await this.page.locator(
        "//label[contains(text(),'Gazette type')]/parent::div/following-sibling::div"
      );
      expect(documentViewGazettePublishedTypeContent).toHaveText(
        document.GazetteType
      );
    }

    if (document.HighwayDistrict) {
      const documentDistrictRoadRegisterTypeHighwayDistrictLabel =
        await this.page.locator("//label[contains(text(),'Highway district')]");
      documentDistrictRoadRegisterTypeHighwayDistrictLabel.awaitFor({
        status: "visible",
      });

      const documentViewGazetteHighwayDistrictContent = await this.page.locator(
        "//label[contains(text(),'Highway district')]/parent::div/following-sibling::div"
      );
      expect(documentViewGazetteHighwayDistrictContent).toHaveText(
        document.HighwayDistrict
      );
    }

    if (document.IndianReserveOrNationalPark) {
      const documentCanLandSurveyTypeIndianReserveLabel =
        await this.page.locator("//label[contains(text(),'Indian reserve')]");
      documentCanLandSurveyTypeIndianReserveLabel.awaitFor({
        status: "visible",
      });

      const documentViewIndianReserveContent = await this.page.locator(
        "//label[contains(text(),'Indian reserve')]/parent::div/following-sibling::div"
      );
      expect(documentViewIndianReserveContent).toHaveText(
        document.IndianReserveOrNationalPark
      );
    }

    if (document.Jurisdiction) {
      const documentBCAssessmentTypeJurisdictionLabel = await this.page.locator(
        "//label[contains(text(),'Jurisdiction')]"
      );
      documentBCAssessmentTypeJurisdictionLabel.awaitFor({ status: "visible" });

      const documentViewJurisdictionContent = await this.page.locator(
        "//label[contains(text(),'Jurisdiction')]/parent::div/following-sibling::div"
      );
      expect(documentViewJurisdictionContent).toHaveText(document.Jurisdiction);
    }

    if (document.LandDistrict) {
      const documentFieldNotesTypeLandDistrictLabel = await this.page.locator(
        "//label[contains(text(),'Land district')]"
      );
      documentFieldNotesTypeLandDistrictLabel.awaitFor({ status: "visible" });

      const documentViewLandDistrictContent = await this.page.locator(
        "//label[contains(text(),'Land district')]/parent::div/following-sibling::div"
      );
      expect(documentViewLandDistrictContent).toHaveText(document.LandDistrict);
    }

    if (document.LegalSurveyPlan) {
      const documentLegalSurveyNbrLabel = await this.page.locator(
        "//label[contains(text(),'Legal survey plan #')]"
      );
      documentLegalSurveyNbrLabel.awaitFor({ status: "visible" });

      const documentViewLegalSurveyPlanContent = await this.page.locator(
        "//label[contains(text(),'Legal survey plan #')]/parent::div/following-sibling::div"
      );
      expect(documentViewLegalSurveyPlanContent).toHaveText(
        document.LegalSurveyPlan
      );
    }

    if (document.LTSAScheduleFiling) {
      const documentGazetteLTSALabel = await this.page.locator(
        "//label[contains(text(),'LTSA schedule filing')]"
      );
      documentGazetteLTSALabel.awaitFor({ status: "visible" });

      const documentViewLTSAScheduleFilingContent = await this.page.locator(
        "//label[contains(text(),'LTSA schedule filing')]/parent::div/following-sibling::div"
      );
      expect(documentViewLTSAScheduleFilingContent).toHaveText(
        document.LTSAScheduleFiling
      );
    }

    if (document.MO) {
      const documentMinisterialOrderTypeMOLabel = await this.page.locator(
        "//label[contains(text(),'MO #')]"
      );
      documentMinisterialOrderTypeMOLabel.awaitFor({ status: "visible" });

      const documentViewMOContent = await this.page.locator(
        "//label[contains(text(),'MO #')]/parent::div/following-sibling::div"
      );
      expect(documentViewMOContent).toHaveText(document.MO);
    }

    if (document.MoTIFile) {
      const documentMOTIFileLabel = await this.page.locator(
        "//label[contains(text(),'MoTI file #')]"
      );
      documentMOTIFileLabel.awaitFor({ status: "visible" });

      const documentViewMotiFileContent = await this.page.locator(
        "//label[contains(text(),'MoTI file #')]/parent::div/following-sibling::div"
      );
      expect(documentViewMotiFileContent).toHaveText(document.MoTIFile);
    }

    if (document.MoTIPlan) {
      const documentMOTIPlanLabel = await this.page.locator(
        "//label[contains(text(),'MoTI plan #')]"
      );
      documentMOTIPlanLabel.awaitFor({ status: "visible" });

      const documentViewMotiPlanContent = await this.page.locator(
        "//label[contains(text(),'MoTI plan #')]/parent::div/following-sibling::div"
      );
      expect(documentViewMotiPlanContent).toHaveText(document.MoTIPlan);
    }

    if (document.OIC) {
      const documentOICTypeOICLabel = await this.page.locator(
        "//label[contains(text(),'OIC #')]"
      );
      documentOICTypeOICLabel.awaitFor({ status: "visible" });

      const documentViewOICNumberContent = await this.page.locator(
        "//label[contains(text(),'OIC #')]/parent::div/following-sibling::div"
      );
      expect(documentViewOICNumberContent).toHaveText(document.OIC);
    }

    if (document.OICRoute) {
      const documentOICTypeOICRouteLabel = await this.page.locator(
        "//label[contains(text(),'OIC route #')]"
      );
      documentOICTypeOICRouteLabel.awaitFor({ status: "visible" });

      const documentViewOICRouteContent = await this.page.locator(
        "//label[contains(text(),'OIC route #')]/parent::div/following-sibling::div"
      );
      expect(documentViewOICRouteContent).toHaveText(document.OICRoute);
    }

    if (document.OICType) {
      const documentOICTypeOICTypeLabel = await this.page.locator(
        "//label[contains(text(),'OIC type')]"
      );
      documentOICTypeOICTypeLabel.awaitFor({ status: "visible" });

      const documentViewOICTypeContent = await this.page.locator(
        "//label[contains(text(),'OIC type')]/parent::div/following-sibling::div"
      );
      expect(documentViewOICTypeContent).toHaveText(document.OICType);
    }

    if (document.Owner) {
      const documentOwnerLabel = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Owner')]"
      );
      documentOwnerLabel.awaitFor({ status: "visible" });

      const documentViewOwnerContent = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Owner')]/parent::div/following-sibling::div"
      );
      expect(documentViewOwnerContent).toHaveText(document.Owner);
    }

    if (document.PhysicalLocation) {
      const documentHistoricFileTypePhyLocationLabel = await this.page.locator(
        "//label[contains(text(),'Physical location')]"
      );
      documentHistoricFileTypePhyLocationLabel.awaitFor({ status: "visible" });

      const documentViewPhysicalLocationContent = await this.page.locator(
        "//label[contains(text(),'Physical location')]/parent::div/following-sibling::div"
      );
      expect(documentViewPhysicalLocationContent).toHaveText(
        document.PhysicalLocation
      );
    }

    if (document.PIDNumber) {
      const documentViewPIDLabel = await this.page.locator(
        "//label[contains(text(),'PID')]"
      );
      documentViewPIDLabel.awaitFor({ status: "visible" });

      const documentViewPIDContent = await this.page.locator(
        "//label[contains(text(),'PID')]/parent::div/following-sibling::div"
      );
      expect(documentViewPIDContent).toHaveText(document.PIDNumber);
    }

    if (document.PINNumber) {
      const documentOtherTypePINLabel = await this.page.locator(
        "//input[@data-testid='metadata-input-PIN']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'PIN')]"
      );
      documentOtherTypePINLabel.awaitFor({ status: "visible" });

      const documentViewPINContent = await this.page.locator(
        "//div[@class='pb-2 row'][1]/div/label[contains(text(),'PIN')]/parent::div/following-sibling::div"
      );
      expect(documentViewPINContent).toHaveText(document.PINNumber);
    }

    if (document.Plan) {
      const documentPAPlanNbrLabel = await this.page.locator(
        "//label[contains(text(),'Plan #')]"
      );
      documentPAPlanNbrLabel.awaitFor({ status: "visible" });

      const documentViewPlanNumberContent = await this.page.locator(
        "//label[contains(text(),'Plan #')]/parent::div/following-sibling::div"
      );
      expect(documentViewPlanNumberContent).toHaveText(document.Plan);
    }

    if (document.PlanRevision) {
      const documentPAPlanRevisionLabel = await this.page.locator(
        "//label[contains(text(),'Plan revision')]"
      );
      documentPAPlanRevisionLabel.awaitFor({ status: "visible" });

      const documentViewPlanRevisionContent = await this.page.locator(
        "//label[contains(text(),'Plan revision')]/parent::div/following-sibling::div"
      );
      expect(documentViewPlanRevisionContent).toHaveText(document.PlanRevision);
    }

    if (document.PlanType) {
      const documentLegalSurveyPlanTypeLabel = await this.page.locator(
        "//label[contains(text(),'Plan type')]"
      );
      documentLegalSurveyPlanTypeLabel.awaitFor({ status: "visible" });

      const documentViewPlanTypeContent = await this.page.locator(
        "//label[contains(text(),'Plan type')]/parent::div/following-sibling::div"
      );
      expect(documentViewPlanTypeContent).toHaveText(document.PlanType);
    }

    if (document.Project) {
      const documentPAPlanProjectLabel = await this.page.locator(
        "//label[contains(text(),'Project #')]"
      );
      documentPAPlanProjectLabel.awaitFor({ status: "visible" });

      const documentViewProjectNumberContent = await this.page.locator(
        "//label[contains(text(),'Project #')]/parent::div/following-sibling::div"
      );
      expect(documentViewProjectNumberContent).toHaveText(document.Project);
    }

    if (document.ProjectName) {
      const documentViewProjectLabel = await this.page.locator(
        "//label[contains(text(),'Project name')]"
      );
      documentViewProjectLabel.awaitFor({ status: "visible" });

      const documentViewProjectContent = await this.page.locator(
        "//label[contains(text(),'Project name')]/parent::div/following-sibling::div"
      );
      expect(documentViewProjectContent).toHaveText(document.ProjectName);
    }

    if (document.PropertyIdentifier) {
      const documentViewPropertyIdentifierLabel = await this.page.locator(
        "//label[contains(text(),'Property identifier')]"
      );
      documentViewPropertyIdentifierLabel.awaitFor({ status: "visible" });

      const documentViewPropertyIdentifierContent = await this.page.locator(
        "//label[contains(text(),'Property identifier')]/parent::div/following-sibling::div"
      );
      expect(documentViewPropertyIdentifierContent).toHaveText(
        document.PropertyIdentifier
      );
    }

    if (document.PublishedDate) {
      const documentMoTIPlanLegalSurveyPublishDateLabel =
        await this.page.locator("//label[contains(text(),'Published date')]");
      documentMoTIPlanLegalSurveyPublishDateLabel.awaitFor({
        status: "visible",
      });

      const documentViewPublishedDateContent = await this.page.locator(
        "//label[contains(text(),'Published date')]/parent::div/following-sibling::div"
      );
      expect(documentViewPublishedDateContent).toHaveText(
        document.PublishedDate
      );
    }

    if (document.ReferenceAgencyDocumentNbr) {
      const documentLandActTypeReferenceAgencyLabel = await this.page.locator(
        "//label[contains(text(),'Reference/Agency Document #')]"
      );
      documentLandActTypeReferenceAgencyLabel.awaitFor({ status: "visible" });

      const documentViewReferenceAgencyContent = await this.page.locator(
        "//label[contains(text(),'Reference/Agency Document #')]/parent::div/following-sibling::div"
      );
      expect(documentViewReferenceAgencyContent).toHaveText(
        document.ReferenceAgencyDocumentNbr
      );
    }

    if (document.ReferenceAgencyLandsFileNbr) {
      const documentLandActTypeReferenceLandsLabel = await this.page.locator(
        "//label[contains(text(),'Reference/Agency Lands file #')]"
      );
      documentLandActTypeReferenceLandsLabel.awaitFor({ status: "visible" });

      const documentViewReferenceLandsContent = await this.page.locator(
        "//label[contains(text(),'Reference/Agency Lands file #')]/parent::div/following-sibling::div"
      );
      expect(documentViewReferenceLandsContent).toHaveText(
        document.ReferenceAgencyLandsFileNbr
      );
    }

    if (document.RelatedGazette) {
      const documentMoTIPlanLegalSurveyRelatedGazetteLabel =
        await this.page.locator("//label[contains(text(),'Related gazette')]");
      documentMoTIPlanLegalSurveyRelatedGazetteLabel.awaitFor({
        status: "visible",
      });

      const documentViewRelatedGazetteContent = await this.page.locator(
        "//label[contains(text(),'Related gazette')]/parent::div/following-sibling::div"
      );
      expect(documentViewRelatedGazetteContent).toHaveText(
        document.RelatedGazette
      );
    }

    if (document.RoadName) {
      const documentRoadNameLabel = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Road name')]"
      );
      documentRoadNameLabel.awaitFor({ status: "visible" });

      const documentViewRoadNameContent = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Road name')]/parent::div/following-sibling::div"
      );
      expect(documentViewRoadNameContent).toHaveText(document.RoadName);
    }

    if (document.Roll) {
      const documentBCAssessmentTypeRollLabel = await this.page.locator(
        "//label[contains(text(),'Roll')]"
      );
      documentBCAssessmentTypeRollLabel.awaitFor({ status: "visible" });

      const documentViewRollContent = await this.page.locator(
        "//label[contains(text(),'Roll')]/parent::div/following-sibling::div"
      );
      expect(documentViewRollContent).toHaveText(document.Roll);
    }

    if (document.Section) {
      const documentHistoricFileTypeSectionLabel = await this.page.locator(
        "//label[contains(text(),'Section')]"
      );
      documentHistoricFileTypeSectionLabel.awaitFor({ status: "visible" });

      const documentViewSectionContent = await this.page.locator(
        "//label[contains(text(),'Section')]/parent::div/following-sibling::div"
      );
      expect(documentViewSectionContent).toHaveText(document.Section);
    }

    if (document.ShortDescriptor) {
      const documentShortDescriptorLabel = await this.page.locator(
        "//input[@data-testid='metadata-input-SHORT_DESCRIPTOR']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Short descriptor')]"
      );
      documentShortDescriptorLabel.awaitFor({ status: "visible" });

      const documentViewShortDescriptorContent = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Short descriptor')]/parent::div/following-sibling::div"
      );
      expect(documentViewShortDescriptorContent).toHaveText(
        document.ShortDescriptor
      );
    }

    if (document.StartDate) {
      const documentHistoricFileTypeStartDateLabel = await this.page.locator(
        "//label[contains(text(),'Start date')]"
      );
      documentHistoricFileTypeStartDateLabel.awaitFor({ status: "visible" });

      const documentViewStartDateContent = await this.page.locator(
        "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Start date')]/parent::div/following-sibling::div"
      );
      expect(documentViewStartDateContent).toHaveText(document.StartDate);
    }

    if (document.Title) {
      const documentTitleSearchTypeTitleLabel = await this.page.locator(
        "//input[@data-testid='metadata-input-TITLE_NUMBER']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Title')]"
      );
      documentTitleSearchTypeTitleLabel.awaitFor({ status: "visible" });

      const documentViewTitleContent = await this.page.locator(
        "//label[contains(text(),'Title')]/parent::div/following-sibling::div"
      );
      expect(documentViewTitleContent).toHaveText(document.Title);
    }

    if (document.Transfer) {
      const documentTransferAdmTypeTransferLabel = await this.page.locator(
        "//label[contains(text(),'Transfer')]"
      );
      documentTransferAdmTypeTransferLabel.awaitFor({ status: "visible" });

      const documentViewTransferContent = await this.page.locator(
        "//label[contains(text(),'Transfer')]/parent::div/following-sibling::div"
      );
      expect(documentViewTransferContent).toHaveText(document.Transfer);
    }

    if (document.Year) {
      const documentYearLabel = await this.page.locator(
        "//label[contains(text(),'Year')]"
      );
      documentYearLabel.awaitFor({ status: "visible" });

      const documentViewYearContent = await this.page.locator(
        "//label[contains(text(),'Year')]/parent::div/following-sibling::div"
      );
      expect(documentViewYearContent).toHaveText(document.Year);
    }

    if (document.YearPrivyCouncil) {
      const documentPrivyCouncilTypePrivyLabel = await this.page.locator(
        "//label[contains(text(),'Year - privy council #')]"
      );
      documentPrivyCouncilTypePrivyLabel.awaitFor({ status: "visible" });

      const documentViewYearPrivyCouncilContent = await this.page.locator(
        "//label[contains(text(),'Year - privy council #')]/parent::div/following-sibling::div"
      );
      expect(documentViewYearPrivyCouncilContent).toHaveText(
        document.YearPrivyCouncil
      );
    }
  }

  async verifyPropertyMgmtFileDocumentsInitMainTables(
    title1stTable,
    title2ndTable
  ) {
    const digitalDocsTitle1 = await this.page.locator(
      `//span[contains(text(),'${title1stTable}')]/parent::div/parent::div/parent::div/parent::div/parent::h2`
    );
    digitalDocsTitle1.awaitFor({ status: "visible" });

    const addDocumentBttn = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button"
    );
    addDocumentBttn.awaitFor({ status: "visible" });

    const digitalTitlebutton = await this.page.locator(
      `//span[contains(text(),'${title1stTable}')]/parent::div/following-sibling::div/div/button[2]`
    );
    digitalTitlebutton.awaitFor({ status: "visible" });

    const documentsTableColumnType = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Document type')]"
    );
    documentsTableColumnType.awaitFor({ status: "visible" });

    const documentsTableColumnName = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Document name')]"
    );
    documentsTableColumnName.awaitFor({ status: "visible" });

    const documentsTableColumnUploaded = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Uploaded')]"
    );
    documentsTableColumnUploaded.awaitFor({ status: "visible" });

    const documentsTableColumnStatus = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]"
    );
    documentsTableColumnStatus.awaitFor({ status: "visible" });

    const documentsTableColumnActions = await this.page.locator(
      "//button[@data-testid='refresh-button']/preceding-sibling::button/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]"
    );
    documentsTableColumnActions.awaitFor({ status: "visible" });

    const digitalDocsTitle2 = await this.page.locator(
      `//span[contains(text(),'${title2ndTable}')]/parent::div/parent::div/parent::div/parent::div/parent::h2`
    );
    digitalDocsTitle2.awaitFor({ status: "visible" });

    const digitalDocsTitle2Button = await this.page.locator(
      `//span[contains(text(),'${title2ndTable}')]/parent::div/following-sibling::div/div/button`
    );
    digitalDocsTitle2Button.awaitFor({ status: "visible" });
  }

  async verifyInitUploadDocumentForm() {
    const documentsUploadHeader = await this.page.locator(
      "div[class='modal-header'] div[class='modal-title h4']"
    );
    documentsUploadHeader.awaitFor({ status: "visible" });

    const documentUploadInstructionsLabel = await this.page.locator(
      "//label[contains(text(),'Choose a max of 10 files to attach at the time')]"
    );
    documentUploadInstructionsLabel.awaitFor({ status: "visible" });

    const documentUploadDragDropArea = await this.page.locator(
      "//div[contains(text(),'Drag files here to attach or')]"
    );
    documentUploadDragDropArea.awaitFor({ status: "visible" });
  }

  async verifyGeneralUpdateDocumentForm() {
    const documentsUploadHeader = await this.page.locator(
      "div[class='modal-header'] div[class='modal-title h4']"
    );
    documentsUploadHeader.awaitFor({ status: "visible" });

    const documenyViewDocumentNameLabel = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/label[contains(text(),'File name')]"
    );
    documenyViewDocumentNameLabel.awaitFor({ status: "visible" });

    const documentViewFileNameContent = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/label[contains(text(),'File name')]/parent::div/following-sibling::div"
    );
    documentViewFileNameContent.awaitFor({ status: "visible" });

    const documentViewInfoSubtitle = await this.page.locator(
      "//div[contains(text(),'Document Information')]"
    );
    documentViewInfoSubtitle.awaitFor({ status: "visible" });

    const documentsGeneralUpdateDocumentTypeLabel = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Document type')]"
    );
    documentsGeneralUpdateDocumentTypeLabel.awaitFor({ status: "visible" });

    const documentGeneralUpdateDocumentSelect = await this.page.locator(
      "div[class='modal-content'] select[id='input-documentTypeId']"
    );
    documentGeneralUpdateDocumentSelect.awaitFor({ status: "visible" });

    const documentGeneralUpdateStatusLabel = await this.page.locator(
      "//div[@class='modal-body']/div/div/div/div/div/div/label[contains(text(),'Status')]"
    );
    documentGeneralUpdateStatusLabel.awaitFor({ status: "visible" });

    const documentUploadStatusSelect = await this.page.locator(
      "#input-documentStatusCode"
    );
    documentUploadStatusSelect.awaitFor({ status: "visible" });

    const documentUpdateDetailsSubtitle = await this.page.locator(
      "//h3[contains(text(),'Details')]"
    );
    documentUpdateDetailsSubtitle.awaitFor({ status: "visible" });
  }
}

module.exports = { DigitalDocuments };
