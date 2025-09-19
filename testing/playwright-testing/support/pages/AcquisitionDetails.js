const { expect } = require("@playwright/test");
const {
  clickAndWaitFor,
  fillTypeahead,
  textEqualsIfNotEmpty,
  textNotEmpty,
  listEquals,
} = require("../../support/common.js");

class AcquisitionDetailsPage {
  constructor(page) {
    this.page = page;
  }

  async navigateToCreateNewAcquisitionFile() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-acquisition'] a",
      "div[data-testid='side-tray']"
    );
  }

  async navigateToFileSummary() {
    const acquisitionFileSummaryBttn = await this.page.locator(
      "div[data-testid='menu-item-row-0'] div button[title='File Details']"
    );
    if ((await acquisitionFileSummaryBttn.count()) > 0) {
      await acquisitionFileSummaryBttn.click();
    }
  }

  async navigateToFileDetailsTab() {
    const acquisitionFileDetailsTab = await this.page.locator(
      "//a[contains(text(),'File Details')]"
    );
    await acquisitionFileDetailsTab.waitFor({ state: "visible" });
    await acquisitionFileDetailsTab.click();
  }

  async navigateToSubfilesTab() {
    const acquisitionSubfilesTab = await this.page.locator(
      "//a[contains(text(),'Sub-Files')]"
    );
    await acquisitionSubfilesTab.waitFor({ state: "visible" });
    await acquisitionSubfilesTab.click();
  }

  async editAcquisitionFileBttn() {
    const acquisitionFileEditButton = await this.page.locator(
      "button[title='Edit acquisition file']"
    );
    await acquisitionFileEditButton.waitFor({ state: "visible" });
    await acquisitionFileEditButton.click();
  }

  async createMinimumAcquisitionFile(acquisition) {
    await this.page
      .locator("#input-fileName")
      .fill(acquisition.AcquisitionFileName);
    await this.page
      .locator("#input-acquisitionType")
      .selectOption({ label: acquisition.AcquisitionType });
    await this.page
      .locator("#input-region")
      .selectOption({ label: acquisition.AcquisitionMOTIRegion });
  }

  async updateAcquisitionFile(acquisition, acquisitionType) {
    // Status
    if (acquisition.AcquisitionStatus && acquisitionType === "Main") {
      const statusSelect = this.page.locator("#input-fileStatusTypeCode");
      await statusSelect.selectOption({ label: acquisition.AcquisitionStatus });
    }

    // Project
    if (acquisition.AcquisitionProject && acquisitionType === "Main") {
      await fillTypeahead(
        this.page,
        "input[id='typeahead-project']",
        acquisition.AcquisitionProject,
        "div[id='typeahead-project'] a"
      );
    }

    if (acquisition.AcquisitionProjProduct && acquisitionType === "Main") {
      const productSelect = this.page.locator("#input-product");
      await productSelect.selectOption({
        label: `${acquisition.AcquisitionProjProductCode} ${acquisition.AcquisitionProjProduct}`,
      });
    }

    if (acquisition.AcquisitionProjFunding) {
      const fundingSelect = await this.page.locator("#input-fundingTypeCode");
      await fundingSelect.selectOption({
        label: acquisition.AcquisitionProjFunding,
      });
    }

    if (acquisition.AcquisitionFundingOther) {
      const otherFundingInput = this.page.locator(
        "#input-fundingTypeOtherDescription"
      );
      await otherFundingInput.fill("");
      await otherFundingInput.fill(acquisition.AcquisitionFundingOther);
    }

    // Progress Statuses
    const progressDeleteBtns = this.page.locator(
      "div[id='multiselect-progressStatuses'] i[class='custom-close']"
    );
    while ((await progressDeleteBtns.count()) > 0) {
      await progressDeleteBtns.first().click();
    }

    if (acquisition.AcquisitionFileProgressStatuses.length > 0) {
      for (const status of acquisition.AcquisitionFileProgressStatuses) {
        const progressSelect = await this.page.locator(
          "#multiselect-progressStatuses_input"
        );
        await progressSelect.click();
        const option = this.page.locator(
          "//input[@id='multiselect-progressStatuses_input']/parent::div/following-sibling::div/ul[@class='optionContainer']"
        );
        await option.locator(`text=${status}`).click();
      }
    }

    if (acquisition.AcquisitionAppraisalStatus) {
      const appraisalSelect = await this.page.locator(
        "#input-appraisalStatusType"
      );
      await appraisalSelect.selectOption({
        label: acquisition.AcquisitionAppraisalStatus,
      });
    }

    if (acquisition.AcquisitionLegalSurveyStatus) {
      const legalSurveySelect = await this.page.locator(
        "#input-legalSurveyStatusType"
      );
      await legalSurveySelect.selectOption({
        label: acquisition.AcquisitionLegalSurveyStatus,
      });
    }

    // Type of Taking
    const takingDeleteBtns = await this.page.locator(
      "div[id='multiselect-takingStatuses'] i[class='custom-close']"
    );
    while ((await takingDeleteBtns.count()) > 0) {
      await takingDeleteBtns.first().click();
    }

    if (acquisition.AcquisitionTypeTakingStatuses.length > 0) {
      for (const status of acquisition.AcquisitionTypeTakingStatuses) {
        const takingSelect = await this.page.locator(
          "#multiselect-takingStatuses_input"
        );
        await takingSelect.click();
        const option = this.page.locator(
          "//input[@id='multiselect-takingStatuses_input']/parent::div/following-sibling::div/ul[@class='optionContainer']"
        );
        await option.locator(`text=${status}`).click();
      }
    }

    if (acquisition.AcquisitionExpropriationRiskStatus) {
      const expropriationSelect = await this.page.locator(
        "#input-expropiationRiskStatusType"
      );
      await expropriationSelect.selectOption({
        label: acquisition.AcquisitionExpropriationRiskStatus,
      });
    }

    // Schedule
    if (acquisition.AcquisitionAssignedDate) {
      const assignedDateInput = await this.page.locator(
        "#datepicker-assignedDate"
      );
      await assignedDateInput.fill(acquisition.AcquisitionAssignedDate);
      await assignedDateInput.press("Enter");
    }
    if (acquisition.AcquisitionDeliveryDate) {
      const deliveryDateInput = await this.page.locator(
        "#datepicker-deliveryDate"
      );
      await deliveryDateInput.fill(acquisition.AcquisitionDeliveryDate);
      await deliveryDateInput.press("Enter");
    }
    if (acquisition.AcquisitionEstimatedDate) {
      const estimatedDateInput = await this.page.locator(
        "#datepicker-estimatedCompletionDate"
      );
      await estimatedDateInput.fill(acquisition.AcquisitionEstimatedDate);
      await estimatedDateInput.press("Enter");
    }
    if (acquisition.AcquisitionPossesionDate) {
      const possesionDateInput = await this.page.locator(
        "#datepicker-possessionDate"
      );
      await possesionDateInput.fill(acquisition.AcquisitionPossesionDate);
      await possesionDateInput.press("Enter");
    }

    // Details
    if (acquisition.AcquisitionFileName) {
      const fileNameInput = await this.page.locator("#input-fileName");
      await fileNameInput.fill(acquisition.AcquisitionFileName);
    }
    if (acquisition.HistoricalFileNumber) {
      const historicalNumberInput = await this.page.locator(
        "#input-legacyFileNumber"
      );
      await historicalNumberInput.fill(acquisition.HistoricalFileNumber);
    }
    if (acquisition.PhysicalFileStatus) {
      const physicalStatusSelect = await this.page.locator(
        "#input-acquisitionPhysFileStatusType"
      );
      await physicalStatusSelect.selectOption({
        label: acquisition.PhysicalFileStatus,
      });
    }
    if (acquisition.PhysicalFileDetails) {
      const physicalDetailsInput = await this.page.locator(
        "#input-physicalFileDetails"
      );
      await physicalDetailsInput.fill(acquisition.PhysicalFileDetails);
    }
    if (acquisition.AcquisitionType) {
      const typeSelect = await this.page.locator("#input-acquisitionType");
      await typeSelect.selectOption({ label: acquisition.AcquisitionType });
    }
    if (
      acquisition.AcquisitionSubfileInterest &&
      acquisitionType === "Subfile"
    ) {
      const subfileInterestSelect = await this.page.locator(
        "#input-subfileInterestTypeCode"
      );
      await subfileInterestSelect.selectOption({
        label: acquisition.AcquisitionSubfileInterest,
      });
    }
    if (
      acquisition.AcquisitionSubfileInterestOther &&
      acquisitionType === "Subfile"
    ) {
      const subfileInterestOtherInput = await this.page.locator(
        "#input-otherSubfileInterestType"
      );
      await subfileInterestOtherInput.fill(
        acquisition.AcquisitionSubfileInterestOther
      );
    }
    if (acquisition.AcquisitionMOTIRegion) {
      const regionSelect = await this.page.locator("#input-region");
      await regionSelect.selectOption({
        label: acquisition.AcquisitionMOTIRegion,
      });
    }

    // Team
    const acquisitionFileTeamMembersGroup = await this.page.locator(
      "//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']"
    );
    while (acquisitionFileTeamMembersGroup.count() > 0) {
      await this.sharedTeams.deleteFirstStaffMember();
    }
    if (acquisition.AcquisitionTeam.length > 0) {
      for (const member of acquisition.AcquisitionTeam) {
        await this.sharedTeams.addTeamMembers(member);
      }
    }

    // Owners
    const acquisitionFileOwnersGroup = await this.page.locator(
      "//div[contains(text(),'Owners')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']"
    );
    while (acquisitionFileOwnersGroup.count() > 0) {
      await deleteOwner();
    }
    if (acquisition.AcquisitionOwners.length > 0) {
      for (let i = 0; i < acquisition.AcquisitionOwners.length; i++) {
        await addOwners(acquisition.AcquisitionOwners[i], i);
      }
    }

    // Owner Solicitor (Main)
    if (acquisition.OwnerSolicitor && acquisitionType === "Main") {
      const acquisitionFileOwnerSolicitorButton = this.page.locator(
        "//label[contains(text(),'Owner solicitor')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']"
      );
      await acquisitionFileOwnerSolicitorButton.click();
      await this.sharedSelectContact.selectContact(
        acquisition.OwnerSolicitor,
        ""
      );
    }

    // Owner Representative (Main)
    if (acquisition.OwnerRepresentative && acquisitionType === "Main") {
      const acquisitionFileOwnerRepresentativeButton = await this.page.locator(
        "//label[contains(text(),'Owner representative')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']"
      );
      await acquisitionFileOwnerRepresentativeButton.click();
      await this.sharedSelectContact.selectContact(
        acquisition.OwnerRepresentative,
        ""
      );
    }

    // Owner Solicitor (Subfile)
    if (acquisition.OwnerSolicitor && acquisitionType === "Subfile") {
      const acquisitionSubfileOwnerSolicitorButton = await this.page.locator(
        "//label[contains(text(),'Sub-interest solicitor')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']"
      );
      await acquisitionSubfileOwnerSolicitorButton.click();
      await this.sharedSelectContact.selectContact(
        acquisition.OwnerSolicitor,
        ""
      );
    }

    // Owner Representative (Subfile)
    if (acquisition.OwnerRepresentative && acquisitionType === "Subfile") {
      const acquisitionSubfileOwnerRepresentativeButton =
        await this.page.locator(
          "//label[contains(text(),'Sub-interest representative')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']"
        );
      await acquisitionSubfileOwnerRepresentativeButton.click();
      await this.sharedSelectContact.selectContact(
        acquisition.OwnerRepresentative,
        ""
      );
    }

    // Owner Comment
    if (acquisition.OwnerComment) {
      const acquisitionFileOwnerCommentEditTextArea = await this.page.locator(
        "#input-ownerRepresentatives.0.comment"
      );
      await acquisitionFileOwnerCommentEditTextArea.fill("");
      await acquisitionFileOwnerCommentEditTextArea.fill(
        acquisition.OwnerComment
      );
    }
  }

  async verifyAcquisitionFileView(acquisition, acquisitionType) {
    // Header
    const acquisitionFileViewTitle = await this.page.locator(
      "//h1[contains(text(),'Acquisition File')]"
    );
    expect(acquisitionFileViewTitle).toBeVisible();

    const acquisitionFileHeaderCodeLabel = await this.page.locator(
      "//label[contains(text(), 'File:')]"
    );
    expect(acquisitionFileHeaderCodeLabel).toBeVisible();

    const acquisitionFileHeaderCodeContent = await this.page.locator(
      "//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]"
    );
    await textNotEmpty(acquisitionFileHeaderCodeContent);

    const acquisitionFileHeaderProjectContent = await this.page.locator(
      "//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileHeaderProjectContent,
      `${acquisition.AcquisitionProjCode} - ${acquisition.AcquisitionProject}`
    );

    const acquisitionFileHeaderProductContent = await this.page.locator();
    await textEqualsIfNotEmpty(
      acquisitionFileHeaderProductContent,
      `${acquisition.AcquisitionProjProductCode} - ${acquisition.AcquisitionProjProduct}`
    );

    const acquisitionFileHeaderCreatedDateContent = await this.page.locator(
      "//strong[contains(text(), 'Created')]/parent::span"
    );
    await textNotEmpty(acquisitionFileHeaderCreatedDateContent);

    const acquisitionFileHeaderCreatedByContent = await this.page.locator(
      "//strong[contains(text(),'Created')]/parent::span/span[@id='userNameTooltip']/strong"
    );
    await textNotEmpty(acquisitionFileHeaderCreatedByContent);

    const acquisitionFileHeaderLastUpdateContent = await this.page.locator(
      "//strong[contains(text(), 'Updated')]/parent::span"
    );
    await textNotEmpty(acquisitionFileHeaderLastUpdateContent);

    const acquisitionFileHeaderLastUpdateByContent = await this.page.locator(
      "//strong[contains(text(), 'Updated')]/parent::span/span[@id='userNameTooltip']/strong"
    );
    await textNotEmpty(acquisitionFileHeaderLastUpdateByContent);

    const acquisitionHeaderStatusContent = await this.page.locator(
      "//b[contains(text(),'File')]/parent::span/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionHeaderStatusContent,
      acquisition.AcquisitionStatus.toUpperCase()
    );

    // Project
    const acquisitionFileProjectContent = await this.page.locator(
      "//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileProjectContent,
      `${acquisition.AcquisitionProjCode} - ${acquisition.AcquisitionProject}`
    );

    const acquisitionFileProjectProductContent = await this.page.locator(
      "//label[contains(text(),'Product')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileProjectProductContent,
      `${acquisition.AcquisitionProjProductCode} ${acquisition.AcquisitionProjProduct}`
    );

    const acquisitionFileProjectFundingContent = await this.page.locator(
      "//label[contains(text(),'Funding')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileProjectFundingContent,
      acquisition.AcquisitionProjFunding
    );

    if (acquisition.AcquisitionFundingOther) {
      const acquisitionFileProjectOtherFundingContent = await this.page.locator(
        "//label[contains(text(),'Other funding')]/parent::div/following-sibling::div"
      );
      await textEqualsIfNotEmpty(
        acquisitionFileProjectOtherFundingContent,
        acquisition.AcquisitionFundingOther
      );
    }

    // File Progress Statuses
    if (acquisition.AcquisitionFileProgressStatuses.length > 0) {
      await listEquals(
        "//div[@data-testid='prg-file-progress-status']",
        acquisition.AcquisitionFileProgressStatuses
      );
    }

    // Type Taking Statuses
    if (acquisition.AcquisitionTypeTakingStatuses.length > 0) {
      await listEquals(
        "//div[@data-testid='prg-taking-type-status']",
        acquisition.AcquisitionTypeTakingStatuses
      );
    }

    // Schedule dates
    const acquisitionFileScheduleAssignedDateContent = await this.page.locator(
      "//label[contains(text(),'Assigned date')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileScheduleAssignedDateContent,
      acquisition.AcquisitionAssignedDate
        ? transformDateFormat(acquisition.AcquisitionAssignedDate)
        : new Date().toLocaleDateString("en-US", {
            month: "short",
            day: "numeric",
            year: "numeric",
          })
    );

    const acquisitionFileScheduleDeliveryDateContent = await this.page.locator(
      "//label[contains(text(),'Delivery date')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileScheduleDeliveryDateContent,
      acquisition.AcquisitionDeliveryDate &&
        transformDateFormat(acquisition.AcquisitionDeliveryDate)
    );

    const acquisitionFileScheduleEstimatedDateContent = await this.page.locator(
      "//label[contains(text(),'Estimated date')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileScheduleEstimatedDateContent,
      transformDateFormat(acquisition.AcquisitionEstimatedDate)
    );

    const acquisitionFileSchedulePossesionDateContent = await this.page.locator(
      "//label[contains(text(),'Possession date')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileSchedulePossesionDateContent,
      transformDateFormat(acquisition.AcquisitionPossesionDate)
    );

    // Details
    const acquisitionFileDetailsNameContent = await this.page.locator(
      "//label[contains(text(),'Acquisition file name')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileDetailsNameContent,
      acquisition.AcquisitionFileName
    );

    const acquisitionFileDetailsPhysicalFileContent = await this.page.locator(
      "//label[contains(text(),'Physical file status')]/parent::div/following-sibling::div"
    );
    await textEqualsIfNotEmpty(
      acquisitionFileDetailsPhysicalFileContent,
      acquisition.PhysicalFileStatus
    );

    const acquisitionFileDetailsPhysicalFileDetailsContent =
      await this.page.locator(
        "//label[contains(text(),'Physical file details')]/parent::div/following-sibling::div"
      );
    await textEqualsIfNotEmpty(
      acquisitionFileDetailsPhysicalFileDetailsContent,
      acquisition.PhysicalFileDetails
    );
    await textEqualsIfNotEmpty(
      acquisitionFileDetailsTypeContent,
      acquisition.AcquisitionType
    );

    const acquisitionFileDetailsSubfileInterestContent =
      await this.page.locator(
        "//label[contains(text(),'Sub-file interest')]/parent::div/following-sibling::div"
      );
    if (acquisitionType === "Subfile") {
      await textEqualsIfNotEmpty(
        acquisitionFileDetailsSubfileInterestContent,
        acquisition.AcquisitionSubfileInterest
      );
    }
    const acquisitionFileDetailsMOTIRegionContent = await this.page.locator(
      "//label[contains(text(),'Ministry region')]/parent::div/following-sibling::div"
    );
    await textNotEmpty(acquisitionFileDetailsMOTIRegionContent);

    // Team Members
    if (acquisition.AcquisitionTeam?.length > 0) {
      await this.sharedTeams.verifyTeamMembersViewForm(
        acquisition.AcquisitionTeam
      );
    }

    // Owners
    if (acquisition.AcquisitionOwners?.length > 0) {
      for (let i = 0; i < acquisition.AcquisitionOwners.length; i++) {
        const owner = acquisition.AcquisitionOwners[i];
        const base = `//span[@data-testid='owner[${i}]']`;

        if (owner.OwnerContactType === "Individual") {
          await expect(page.locator(`${base}/div[2]/div[2]`)).toContainText(
            owner.OwnerGivenNames
          );
          await expect(page.locator(`${base}/div[2]/div[2]`)).toContainText(
            owner.OwnerLastName
          );
          await expect(page.locator(`${base}/div[3]/div[2]`)).toHaveText(
            owner.OwnerOtherName
          );
        } else {
          await expect(page.locator(`${base}/div[2]/div[2]`)).toContainText(
            owner.OwnerCorporationName
          );
          if (owner.OwnerRegistrationNumber) {
            await expect(page.locator(`${base}/div[2]/div[2]`)).toContainText(
              owner.OwnerRegistrationNumber
            );
          }
          await expect(page.locator(`${base}/div[3]/div[2]`)).toHaveText(
            owner.OwnerOtherName
          );
        }

        // Mail address assertions
        const address = owner.OwnerMailAddress || {};
        for (const field of [
          "AddressLine1",
          "AddressLine2",
          "AddressLine3",
          "Country",
          "City",
          "Province",
          "PostalCode",
        ]) {
          if (address[field]) {
            await expect(page.locator(`${base}/div[4]/div[2]`)).toContainText(
              address[field]
            );
          }
        }
        if (address.OtherCountry) {
          await expect(page.locator(`${base}/div[4]/div[2]`)).toContainText(
            `Other - ${address.OtherCountry}`
          );
        }
      }
    }

    // Owner contacts
    if (acquisition.OwnerSolicitor && acquisitionType === "Main") {
      const acquisitionFileOwnerSolicitorContent = await this.page.locator(
        "//label[contains(text(),'Owner solicitor')]/parent::div/following-sibling::div/a/span"
      );
      await textEqualsIfNotEmpty(
        acquisitionFileOwnerSolicitorContent,
        acquisition.OwnerSolicitor
      );
    }
    if (acquisition.OwnerRepresentative && acquisitionType === "Main") {
      const acquisitionFileOwnerRepresentativeContent = await this.page.locator(
        "//label[contains(text(),'Owner representative')]/parent::div/following-sibling::div/a/span"
      );
      await textEqualsIfNotEmpty(
        acquisitionFileOwnerRepresentativeContent,
        acquisition.OwnerRepresentative
      );
    }
    if (acquisition.OwnerSolicitor && acquisitionType === "Subfile") {
      const acquisitionSubfileOwnerSolicitorContent = await this.page.locator(
        "//label[contains(text(),'Sub-interest solicitor')]/parent::div/following-sibling::div/a/span"
      );
      await textEqualsIfNotEmpty(
        acquisitionSubfileOwnerSolicitorContent,
        acquisition.OwnerSolicitor
      );
    }
    if (acquisition.OwnerRepresentative && acquisitionType === "Subfile") {
      const acquisitionSubfileOwnerRepresentativeContent =
        await this.page.locator(
          "//label[contains(text(),'Sub-interest representative')]/parent::div/following-sibling::div/a/span"
        );
      await textEqualsIfNotEmpty(
        acquisitionSubfileOwnerRepresentativeContent,
        acquisition.OwnerRepresentative
      );
    }
    if (acquisition.OwnerComment) {
      const acquisitionFileOwnerCommentContent = await this.page.locator(
        "//label[contains(text(),'Comment')]/parent::div/following-sibling::div"
      );
      await textEqualsIfNotEmpty(
        acquisitionFileOwnerCommentContent,
        acquisition.OwnerComment
      );
    }
  }
}

module.exports = { AcquisitionDetailsPage };
