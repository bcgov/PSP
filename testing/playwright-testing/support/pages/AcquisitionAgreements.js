const { expect } = require("@playwright/test");

class AcquisitionAgreements {
  constructor(page) {
    this.page = page;
  }

  async navigateAgreementsTab() {
    const agreementsLinkTab = await this.page.locator(
      "//a[contains(text(),'Agreements')]"
    );
    await agreementsLinkTab.waitFor({ state: "visible" });
    await agreementsLinkTab.click();
  }

  async editAgreementButton(index) {
    const elementNumber = index + 1;
    await this.page
      .locator(`button[data-testid='agreements[${elementNumber}].edit-btn']`)
      .click();
  }

  async createNewAgreement() {
    const agreementsCreateNewAgreementBtn = await this.page.locator(
      "//h2/div/div/div/div[contains(text(),'Agreements')]/following-sibling::div/button"
    );
    await agreementsCreateNewAgreementBtn.waitFor({ state: "visible" });
    await agreementsCreateNewAgreementBtn.click();
  }

  async saveAcquisitionFileAgreement() {
    await this.page.locator('button:has-text("Save")').click();
    const agreementsCreateNewAgreementBtn = await this.page.locator(
      "//h2/div/div/div/div[contains(text(),'Agreements')]/following-sibling::div/button"
    );
    expect(agreementsCreateNewAgreementBtn).toBeVisible();
  }

  async cancelAcquisitionFileAgreement() {
    await this.page.locator('button:has-text("Cancel")').click();
    const acquisitionFileConfirmationModal = await this.page.locator(
      "div[class='modal-content']"
    );

    if (await acquisitionFileConfirmationModal.isVisible()) {
      await this.page.locator('button:has-text("OK")').click();
    }

    const agreementsCreateNewAgreementBtn = await this.page.locator(
      "//h2/div/div/div/div[contains(text(),'Agreements')]/following-sibling::div/button"
    );
    await expect(agreementsCreateNewAgreementBtn).toBeVisible();
  }

  async createUpdateAgreement(agreement) {
    const agreementsStatusInput = await this.page.locator(
      "#input-agreementStatusTypeCode"
    );
    await agreementsStatusInput.selectOption({
      label: agreement.AgreementStatus,
    });

    if (agreement.AgreementCancellationReason) {
      const agreementCancellationReasonInput = await this.page.locator(
        "#input-cancellationNote"
      );
      await agreementCancellationReasonInput.fill(
        agreement.AgreementCancellationReason
      );
    }
    if (agreement.AgreementLegalSurveyPlan) {
      const agreementsLegalSurveyPlanInput = await this.page.locator(
        "#input-legalSurveyPlanNum"
      );
      await agreementsLegalSurveyPlanInput.fill(
        agreement.AgreementLegalSurveyPlan
      );
    }
    if (agreement.AgreementType) {
      const agreementsTypeSelect = await this.page.locator(
        "#input-agreementTypeCode"
      );
      await agreementsTypeSelect.selectOption({
        label: agreement.AgreementType,
      });
    }
    if (agreement.AgreementDate) {
      const agreementsDateInput = this.page.locator(
        "#datepicker-agreementDate"
      );
      await agreementsDateInput.fill(agreement.AgreementDate);
      await agreementsDateInput.press("Enter");
    }
    if (agreement.AgreementCommencementDate) {
      const agreementCommencementDateInput = await this.page.locator(
        "#datepicker-commencementDate"
      );
      await agreementCommencementDateInput.fill(
        agreement.AgreementCommencementDate
      );
      await agreementCommencementDateInput.press("Enter");
    }
    if (agreement.AgreementCompletionDate) {
      const agreementsCompletionDateInput = await this.page.locator(
        "#datepicker-completionDate"
      );
      await agreementsCompletionDateInput.fill(
        agreement.AgreementCompletionDate
      );
      await agreementsCompletionDateInput.press("Enter");
    }
    if (agreement.AgreementTerminationDate) {
      const agreementsTerminationDateInput = await this.page.locator(
        "#datepicker-terminationDate"
      );
      await agreementsTerminationDateInput.fill(
        agreement.AgreementTerminationDate
      );
      await agreementsTerminationDateInput.press("Enter");
    }
    if (agreement.AgreementPossessionDate) {
      const agreementsPossessionDateInput = await this.page.locator(
        "#datepicker-possessionDate"
      );
      await agreementsPossessionDateInput.fill(
        agreement.AgreementPossessionDate
      );
      await agreementsPossessionDateInput.press("Enter");
    }
    if (agreement.AgreementPurchasePrice) {
      const agreementsPurchasePriceInput = await this.page.locator(
        "#input-purchasePrice"
      );
      await agreementsPurchasePriceInput.fill(agreement.AgreementPurchasePrice);
    }
    if (agreement.AgreementDepositDue) {
      const agreementsDepositNotLaterInput = await this.page.locator(
        "#input-noLaterThanDays"
      );
      await agreementsDepositNotLaterInput.fill(agreement.AgreementDepositDue);
    }
    if (agreement.AgreementDepositAmount) {
      const agreementsDepositAmountInput = await this.page.locator(
        "#input-depositAmount"
      );
      await agreementsDepositAmountInput.fill(agreement.AgreementDepositAmount);
    }
  }

  async deleteLastAgreement() {
    const agreementsTotalCount = await this.page.locator(
      "//div[contains(text(),'Agreements')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div[@class='collapse show']/div"
    );
    const lastAgreement = await agreementsTotalCount.count();

    await this.page
      .locator(`button[data-testid='agreements[${lastAgreement}].delete-btn']`)
      .click();

    const acquisitionFileConfirmationModal = await this.page.locator(
      "div[class='modal-content']"
    );
    if (await acquisitionFileConfirmationModal.isVisible()) {
      await expect(this.page.locator("text=Delete Agreement")).toBeVisible();
      await expect(
        this.page.locator("text=You have selected to delete this Agreement.")
      ).toBeVisible();
      await this.page.click('button:has-text("OK")');
    }
  }

  async totalAgreementsCount() {
    const agreementsTotalCount = await this.page.locator(
      "//div[contains(text(),'Agreements')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div[@class='collapse show']/div"
    );
    return await agreementsTotalCount.count();
  }

  async verifyInitAgreementTab() {
    const agreementsSubtitle = await this.page.locator(
      "//h2/div/div/div/div[contains(text(),'Agreements')]"
    );
    await expect(agreementsSubtitle).toBeVisible();

    const agreementsCreateNewAgreementBtn = await this.page.locator(
      "//h2/div/div/div/div[contains(text(),'Agreements')]/following-sibling::div/button"
    );
    await expect(agreementsCreateNewAgreementBtn).toBeVisible();

    const agreementsInitMessage = await this.page.locator(
      "//p[contains(text(),'There are no agreements indicated in this acquisition file.')]"
    );
    await expect(agreementsInitMessage).toBeVisible();
  }

  async verifyCreateAgreementForm() {
    const AgreementsDetailsCreateSubtitle = await this.page.locator(
      "//h2/div/div[contains(text(),'Agreement Details')]"
    );
    await expect(AgreementsDetailsCreateSubtitle).toBeVisible();

    const agreementsStatusLabel = this.page.locator(
      "//label[contains(text(),'Agreement status')]"
    );
    await expect(agreementsStatusLabel).toBeVisible();

    const agreementsStatusInput = await this.page.locator(
      "#input-agreementStatusTypeCode"
    );
    await expect(agreementsStatusInput).toBeVisible();

    const agreementsLegalSurveyPlanLabel = await this.page.locator(
      "//label[contains(text(),'Legal survey plan')]"
    );
    await expect(agreementsLegalSurveyPlanLabel).toBeVisible();

    const agreementsLegalSurveyPlanInput = await this.page.locator(
      "#input-legalSurveyPlanNum"
    );
    await expect(agreementsLegalSurveyPlanInput).toBeVisible();

    const agreementsTypeLabel = await this.page.locator(
      "//label[contains(text(),'Agreement type')]"
    );
    await expect(agreementsTypeLabel).toBeVisible();

    const agreementsTypeSelect = await this.page.locator(
      "#input-agreementTypeCode"
    );
    await expect(agreementsTypeSelect).toBeVisible();

    const agreementsDateLabel = await this.page.locator(
      "//label[contains(text(),'Agreement date')]"
    );
    await expect(agreementsDateLabel).toBeVisible();

    const agreementsDateInput = await this.page.locator(
      "#datepicker-agreementDate"
    );
    await expect(agreementsDateInput).toBeVisible();

    const agreementsCompletionDateLabel = await this.page.locator(
      "//label[contains(text(),'Completion date')]"
    );
    await expect(agreementsCompletionDateLabel).toBeVisible();

    const agreementsCompletionDateInput = await this.page.locator(
      "#datepicker-completionDate"
    );
    await expect(agreementsCompletionDateInput).toBeVisible();

    const agreementsTerminationDateLabel = await this.page.locator(
      "//label[contains(text(),'Termination date')]"
    );
    await expect(agreementsTerminationDateLabel).toBeVisible();

    const agreementsTerminationDateInput = await this.page.locator(
      "#datepicker-terminationDate"
    );
    await expect(agreementsTerminationDateInput).toBeVisible();

    const agreementsPossessionDateLabel = await this.page.locator(
      "//label[contains(text(),'Possession date')]"
    );
    await expect(agreementsPossessionDateLabel).toBeVisible();

    const agreementsPossessionDateInput = await this.page.locator(
      "#datepicker-possessionDate"
    );
    await expect(agreementsPossessionDateInput).toBeVisible();

    const agreementFinancialSubtitle = await this.page.locator(
      "//div[contains(text(),'Financial')]"
    );
    await expect(agreementFinancialSubtitle).toBeVisible();

    const agreementsPurchasePriceLabel = await this.page.locator(
      "//label[contains(text(),'Purchase price')]"
    );
    await expect(agreementsPurchasePriceLabel).toBeVisible();

    const agreementsPurchasePriceInput = await this.page.locator(
      "#input-purchasePrice"
    );
    await expect(agreementsPurchasePriceInput).toBeVisible();

    const agreementsDepositNotLaterLabel = await this.page.locator(
      "//label[contains(text(),'Deposit due no later than')]"
    );
    await expect(agreementsDepositNotLaterLabel).toBeVisible();

    const agreementsDepositNotLaterInput = await this.page.locator(
      "#input-noLaterThanDays"
    );
    await expect(agreementsDepositNotLaterInput).toBeVisible();

    const agreementsDepositAmountLabel = await this.page.locator(
      "//label[contains(text(),'Deposit amount')]"
    );
    await expect(agreementsDepositAmountLabel).toBeVisible();

    const agreementsDepositAmountInput = await this.page.locator(
      "#input-depositAmount"
    );
    await expect(agreementsDepositAmountInput).toBeVisible();
  }

  async verifyViewAgreementForm(agreement, index) {
    const agreementNbr = index + 1;
    // Helper to get dynamic XPath with agreementNbr
    const x = (label) =>
      `//button[@data-testid='agreements[${agreementNbr}].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'${label}')]/parent::div/following-sibling::div`;
    const xLabel = (label) =>
      `//button[@data-testid='agreements[${agreementNbr}].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'${label}')]`;

    // Agreement header
    await expect(
      this.page.locator(
        `//button[@data-testid='agreements[${agreementNbr}].edit-btn']/parent::div/parent::div/preceding-sibling::div`
      )
    ).toHaveText(`Agreement ${agreementNbr}`);

    // Agreement status
    await expect(this.page.locator(xLabel("Agreement status"))).toBeVisible();
    await expect(this.page.locator(x("Agreement status"))).toHaveText(
      agreement.AgreementStatus
    );

    // Cancellation reason (optional)
    if (agreement.AgreementCancellationReason) {
      await expect(
        this.page.locator(xLabel("Cancellation reason"))
      ).toBeVisible();
      await expect(this.page.locator(x("Cancellation reason"))).toHaveText(
        agreement.AgreementCancellationReason
      );
    }

    // Legal survey plan
    await expect(this.page.locator(xLabel("Legal survey plan"))).toBeVisible();
    await expect(this.page.locator(x("Legal survey plan"))).toHaveText(
      agreement.AgreementLegalSurveyPlan
    );

    // Agreement type
    await expect(this.page.locator(xLabel("Agreement type"))).toBeVisible();
    await expect(this.page.locator(x("Agreement type"))).toHaveText(
      agreement.AgreementType
    );

    // Agreement date
    await expect(this.page.locator(xLabel("Agreement date"))).toBeVisible();
    await expect(this.page.locator(x("Agreement date"))).toHaveText(
      this.transformDateFormat(agreement.AgreementDate)
    );

    // Commencement date (optional)
    if (agreement.AgreementCommencementDate) {
      await expect(
        this.page.locator(xLabel("Commencement date"))
      ).toBeVisible();
      await expect(this.page.locator(x("Commencement date"))).toHaveText(
        this.transformDateFormat(agreement.AgreementCommencementDate)
      );
    }

    // Completion date
    await expect(this.page.locator(xLabel("Completion date"))).toBeVisible();
    await expect(this.page.locator(x("Completion date"))).toHaveText(
      this.transformDateFormat(agreement.AgreementCompletionDate)
    );

    // Termination date
    await expect(this.page.locator(xLabel("Termination date"))).toBeVisible();
    await expect(this.page.locator(x("Termination date"))).toHaveText(
      this.transformDateFormat(agreement.AgreementTerminationDate)
    );

    // Possession date
    await expect(this.page.locator(xLabel("Possession date"))).toBeVisible();
    await expect(this.page.locator(x("Possession date"))).toHaveText(
      this.transformDateFormat(agreement.AgreementPossessionDate)
    );

    // Financial subtitle
    await expect(
      this.page.locator(
        `//button[@data-testid='agreements[${agreementNbr}].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[contains(text(),'Financial')]`
      )
    ).toBeVisible();

    // Purchase price
    await expect(this.page.locator(xLabel("Purchase price"))).toBeVisible();
    await expect(this.page.locator(x("Purchase price"))).toHaveText(
      this.transformCurrencyFormat(agreement.AgreementPurchasePrice)
    );

    // Deposit due no later than
    await expect(
      this.page.locator(xLabel("Deposit due no later than"))
    ).toBeVisible();
    await expect(this.page.locator(x("Deposit due no later than"))).toHaveText(
      `${agreement.AgreementDepositDue} days`
    );

    // Deposit amount
    await expect(this.page.locator(xLabel("Deposit amount"))).toBeVisible();
    await expect(this.page.locator(x("Deposit amount"))).toHaveText(
      this.transformCurrencyFormat(agreement.AgreementDepositAmount)
    );
  }
}

module.exports = { AcquisitionAgreements };
