import { expect, Locator, Page } from '@playwright/test';
import { ConSubHistory } from './consolidation-subdivision-history.page';

export class ConsolidationPage {
  readonly page: Page;

  readonly consolidationCreateTitle: Locator;
  readonly consolidationCreateSubtitle: Locator;
  readonly consolidationParentsInstructionsParagraph: Locator;

  readonly consolidationSelectedParentsSubtitle: Locator;
  readonly consolidationParentPIDInput: Locator;
  readonly subconSearchParentResetButton: Locator;
  readonly subconSearchParentButton: Locator;

  readonly consolidationParentsResultIdentifierColumn: Locator;
  readonly consolidationParentsResultPlanColumn: Locator;
  readonly consolidationParentsResultAreaColumn: Locator;
  readonly consolidationParentsResultAddressColumn: Locator;

  readonly consolidationChildInstructionsParagraph: Locator;
  readonly subconChildrenSearchTab: Locator;
  readonly subconChildrenSearchByPIDInput: Locator;
  readonly subconChildrenSearchButton: Locator;
  readonly subconChildrenResetButton: Locator;
  readonly subconChildren1stResultCheckbox: Locator;
  readonly subconChildernAddToSelectionBttn: Locator;

  readonly consolidationSelectedChildSubtitle: Locator;
  readonly consolidationChildResultIdentifierColumn: Locator;
  readonly consolidationChildResultPlanColumn: Locator;
  readonly consolidationChildResultAreaColumn: Locator;
  readonly consolidationChildResultAddressColumn: Locator;

  readonly consolidationPropertiesCreateButton: Locator;

  readonly subconChildrenFirstResultCheckbox: Locator;
  readonly subconChildrenAddToSelectionButton: Locator;

  readonly consolidationChooseParentsErrorMsg: Locator;
  readonly subdivisionChooseChildrenErrorMsg: Locator;

  //Modal Elements
  readonly subconModalWindow: Locator;
  readonly generalModalHeader: Locator;
  readonly subconGeneralModalContent: Locator;
  readonly subconWarningHeader: Locator;
  readonly subconErrorHeader: Locator;
  readonly subconModalSaveWarningP1: Locator;
  readonly subconModalSaveWarningP2: Locator;
  readonly subconModalOkBttn: Locator;
  readonly subconModalCancelBttn: Locator;

  //Toast Elements
  readonly generalToastBody: Locator;

  //Property Element
  readonly propertyDetailsTab: Locator;

  constructor(page: Page) {
    this.page = page;

    this.consolidationCreateTitle = page.getByRole('heading', {
      level: 1,
      name: /Create a Consolidation/i,
    });

    this.consolidationCreateSubtitle = page.getByRole('heading', {
      level: 2,
      name: /Properties in Consolidation/i,
    });

    this.consolidationParentsInstructionsParagraph = page.getByText(
      'Select two or more parent properties that were consolidated:',
      { exact: false }
    );

    this.consolidationSelectedParentsSubtitle = page.getByText('Selected Parents', { exact: true });

    this.consolidationParentPIDInput = page.locator('#input-pid');
    this.subconSearchParentResetButton = page.locator('#search-button');
    this.subconSearchParentButton = page.locator('#reset-button');

    this.consolidationParentsResultIdentifierColumn = page.locator(
      "xpath=//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]"
    );

    this.consolidationParentsResultPlanColumn = page.locator(
      "xpath=//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]"
    );

    this.consolidationParentsResultAreaColumn = page.locator(
      "xpath=//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]"
    );

    this.consolidationParentsResultAddressColumn = page.locator(
      "xpath=//p[contains(text(),'Select two or more parent properties that were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]"
    );

    this.consolidationChildInstructionsParagraph = page.getByText(
      'Select the child property to which parent properties were consolidated:',
      { exact: false }
    );

    this.consolidationSelectedChildSubtitle = page.getByText('Selected Child', { exact: true });
    this.subconChildrenSearchTab = page.locator(
      "xpath=//a[contains(text(),'Locate on Map')]/following-sibling::a"
    );
    this.subconChildrenSearchByPIDInput = page.locator(
      "xpath=//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/div/input"
    );
    this.subconChildrenSearchButton = page.locator(
      "xpath=//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/button[@data-testid='search']"
    );
    this.subconChildrenResetButton = page.locator(
      "xpath=//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/button[@data-testid='reset-button']"
    );
    this.subconChildren1stResultCheckbox = page.locator(
      "div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td']:first-child input"
    );
    this.subconChildernAddToSelectionBttn = page.locator(
      "xpath=//div[contains(text(),'Add to selection')]/parent::button"
    );
    this.consolidationChildResultIdentifierColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]"
    );

    this.consolidationChildResultPlanColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]"
    );

    this.consolidationChildResultAreaColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]"
    );

    this.consolidationChildResultAddressColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child property to which parent properties were consolidated:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]"
    );

    this.consolidationPropertiesCreateButton = page.getByRole('button', {
      name: /Create Consolidation/i,
    });

    this.subconChildrenFirstResultCheckbox = page.locator(
      "div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td']:first-child input"
    );
    this.subconChildrenAddToSelectionButton = page.locator(
      "xpath=//div[contains(text(),'Add to selection')]/parent::button"
    );

    this.consolidationChooseParentsErrorMsg = page.getByText(
      'You must select at least two parent properties'
    );

    this.subdivisionChooseChildrenErrorMsg = page.getByText(
      'You must select at least two child properties'
    );

    this.subconModalWindow = page.locator("div[class='modal-content']");
    this.generalModalHeader = page.locator("div[class='modal-header'] div[class='modal-title h4']");
    this.subconErrorHeader = page.locator(
      "xpath=//div[@class='modal-header']/div[contains(text(),'Error')]"
    );
    this.subconWarningHeader = page.locator(
      "xpath=//div[@class='modal-header']/div[contains(text(),'Are you sure?')]"
    );
    this.subconGeneralModalContent = page.locator("div[class='modal-body']");
    this.subconModalSaveWarningP1 = page.locator("div[class='modal-body'] p:first-child");
    this.subconModalSaveWarningP2 = page.locator("div[class='modal-body'] p:nth-child(2)");
    this.subconModalOkBttn = page.locator("button[title='ok-modal']");
    this.subconModalCancelBttn = page.locator("button[title='cancel-modal']");

    this.generalToastBody = page.locator("div[class='Toastify__toast-body']");

    //Properties page element:
    this.propertyDetailsTab = page.locator("a[data-rb-event-key='details']");
  }

  async goto() {
    await this.page.goto('/mapview/sidebar/consolidation/new', { waitUntil: 'domcontentloaded' });
  }

  async saveConsolidation() {
    await this.consolidationPropertiesCreateButton.click();

    await expect(this.subconModalWindow).toBeVisible();
    await expect(this.subconWarningHeader).toBeVisible();

    await expect(this.subconModalSaveWarningP1).toHaveText(
      'You are consolidating two or more properties into one. ' +
        'The old parent properties records will be retired, and a new child property will be created.'
    );

    await expect(this.subconModalSaveWarningP2).toHaveText(
      'If you proceed, you will be redirected to the new child property record, ' +
        'where you can view changes and make updates. Do you want to proceed?'
    );

    await this.subconModalOkBttn.click();
  }

  async waitForPropertyPanel() {
    await expect(this.propertyDetailsTab).toBeVisible();
  }

  async cancelSubdivisionConsolidation() {
    await this.subconModalCancelBttn.click();
    await this.subconModalCancelBttn.click();
  }

  async createConsolidation(parentProperties: ConSubHistory[], childroperty: ConSubHistory) {
    for (const parent of parentProperties) {
      await this.subconSearchParentResetButton.click();
      await this.consolidationParentPIDInput.fill(parent.pid);
      await this.subconSearchParentButton.click();
    }

    await this.subconChildrenSearchTab.click();
    await this.subconChildrenSearchByPIDInput.fill(childroperty.pid);

    await this.subconChildrenSearchButton.click();

    await this.subconChildrenFirstResultCheckbox.check();
    await this.subconChildrenAddToSelectionButton.click();
  }

  async verifyInvalidConsolidationChildMessage() {
    await expect(this.subconErrorHeader).toBeVisible();

    await expect(this.subconGeneralModalContent).toHaveText(
      'Consolidated child property may not be in the PIMS inventory unless also in the parent property list.'
    );

    await this.subconModalOkBttn.click();
  }

  async verifyInvalidConsolidationRepeatedParentMessage() {
    await expect(this.subconErrorHeader).toBeVisible();
    await expect(this.subconGeneralModalContent).toHaveText(
      'Consolidations must contain at least two different parent properties.'
    );

    await this.subconModalOkBttn.click();
  }

  async verifyInvalidSubdivisionChildMessage() {
    await expect(this.generalToastBody).toHaveText(
      'A property that the user is trying to select has already been added to the selected properties list'
    );
  }

  async verifyMissingParentMessageModal() {
    await expect(this.subconModalWindow).toBeVisible();

    await expect(this.generalModalHeader).toHaveText('Error');

    await expect(this.subconGeneralModalContent).toHaveText(
      'Only properties that are part of the Core Inventory (owned) can be subdivided/consolidated. ' +
        'This property is not in core inventory within PIMS.'
    );
  }

  async verifyMissingParentErrorMessage() {
    await expect(this.consolidationChooseParentsErrorMsg).toBeVisible();
  }

  async verifyMissingChildMessage() {
    await expect(this.subdivisionChooseChildrenErrorMsg).toBeVisible();
  }
}
