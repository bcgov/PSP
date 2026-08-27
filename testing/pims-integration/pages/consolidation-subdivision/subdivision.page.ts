import { expect, Locator, Page } from '@playwright/test';

export class subdivisionPage {
  private readonly page: Page;

  private readonly menuSubdivisionConsolidationButton: Locator;
  private readonly createSubdivisionButton: Locator;

  private readonly subdivisionCreateTitle: Locator;
  private readonly subdivisionCreateSubtitle: Locator;
  private readonly subdivisionParentInstructionsParagraph: Locator;

  private readonly subconParentSearchAnchor: Locator;
  private readonly subconSearchParentByPIDSelect: Locator;
  private readonly subconSearchParentByPIDInput: Locator;
  private readonly subconSearchParentButton: Locator;
  private readonly subconSearchParentResetButton: Locator;

  private readonly subdivisionSelectedParentSubtitle: Locator;
  private readonly subconParentResultIdentifierColumn: Locator;
  private readonly subconParentResultPlanColumn: Locator;
  private readonly subconParentResultAreaColumn: Locator;
  private readonly subconParentResultAddressColumn: Locator;

  private readonly subdivisionChildrenInstructionsParagraph: Locator;
  private readonly subconChildrenLocateOnMapTab: Locator;
  private readonly subconChildrenLocateOnMapSubtitle: Locator;
  private readonly subconChildrenLocateOnMapBlueIcon: Locator;
  private readonly subconChildrenLocateOnMapInstruction1: Locator;
  private readonly subconChildrenLocateOnMapInstruction2: Locator;
  private readonly subconChildrenLocateOnMapInstruction3: Locator;
  private readonly subconChildrenLocateOnMapSelectedLabel: Locator;
  private readonly subconChildrenLocateOnMapPIDLabel: Locator;
  private readonly subconChildrenLocateOnMapPlanLabel: Locator;
  private readonly subconChildrenLocateOnMapAddressLabel: Locator;
  private readonly subconChildrenLocateOnMapRegionLabel: Locator;
  private readonly subconChildrenLocateOnMapDistrictLabel: Locator;

  private readonly subconChildrenSearchTab: Locator;
  private readonly subconChildrenSearchByPIDSelect: Locator;
  private readonly subconChildrenSearchByPIDInput: Locator;
  private readonly subconChildrenSearchButton: Locator;
  private readonly subconChildrenResetButton: Locator;
  private readonly subconChildrenFirstResultCheckbox: Locator;
  private readonly subconChildrenAddToSelectionButton: Locator;

  private readonly subdivisionSelectedChildrenSubtitle: Locator;
  private readonly subdivisionChildrenResultIdentifierColumn: Locator;
  private readonly subdivisionChildrenResultPlanColumn: Locator;
  private readonly subdivisionChildrenResultAreaColumn: Locator;
  private readonly subdivisionChildrenResultAddressColumn: Locator;

  private readonly subdivisionPropertiesCreateButton: Locator;
  private readonly subconPropertiesCancelButton: Locator;

  private readonly subconModalWindow: Locator;
  private readonly subconWarningHeader: Locator;
  private readonly subconErrorHeader: Locator;
  private readonly subconModalSaveWarningP1: Locator;
  private readonly subconModalSaveWarningP2: Locator;
  private readonly subconModalOkBttn: Locator;

  constructor(page: Page) {
    this.page = page;
    this.menuSubdivisionConsolidationButton = page.locator(
      "div[data-testid='nav-tooltip-subdivision&consolidation'] a"
    );
    this.createSubdivisionButton = page.getByRole('link', {
      name: /Create a Subdivision/i,
    });

    this.subdivisionCreateTitle = page.getByRole('heading', {
      level: 1,
      name: /Create a Subdivision/i,
    });

    this.subdivisionCreateSubtitle = page.getByRole('heading', {
      level: 2,
      name: /Properties in Subdivision/i,
    });

    this.subdivisionParentInstructionsParagraph = page.getByText(
      'Select the parent property that was subdivided:',
      { exact: false }
    );

    this.subconParentSearchAnchor = page.locator("a[data-rb-event-key='parent-property']");

    this.subconSearchParentByPIDSelect = page.locator(
      "xpath=//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/div/div/select"
    );

    this.subconSearchParentByPIDInput = page.locator('#input-pid');

    this.subconSearchParentButton = page.locator(
      "xpath=//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/button[@data-testid='search']"
    );

    this.subconSearchParentResetButton = page.locator(
      "xpath=//a[contains(text(),'Parent Property Search')]/parent::nav/following-sibling::div/div/div/div/div/div/div/div/button[@data-testid='reset-button']"
    );

    this.subdivisionSelectedParentSubtitle = page.getByText('Selected Parent', {
      exact: true,
    });

    this.subconParentResultIdentifierColumn = page.locator(
      "xpath=//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]"
    );

    this.subconParentResultPlanColumn = page.locator(
      "xpath=//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]"
    );

    this.subconParentResultAreaColumn = page.locator(
      "xpath=//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]"
    );

    this.subconParentResultAddressColumn = page.locator(
      "xpath=//p[contains(text(),'Select the parent property that was subdivided')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]"
    );

    this.subdivisionChildrenInstructionsParagraph = page.getByText(
      'Select the child properties to which parent property was subdivided:',
      { exact: false }
    );

    this.subconChildrenLocateOnMapTab = page.getByRole('link', {
      name: 'Locate on Map',
    });

    this.subconChildrenLocateOnMapSubtitle = page.getByRole('heading', {
      level: 3,
      name: 'Select a property',
    });

    this.subconChildrenLocateOnMapBlueIcon = page.locator('#Layer_2');

    this.subconChildrenLocateOnMapInstruction1 = page.getByText('Single-click blue marker above', {
      exact: false,
    });

    this.subconChildrenLocateOnMapInstruction2 = page.getByText('Mouse to a parcel on the map', {
      exact: false,
    });

    this.subconChildrenLocateOnMapInstruction3 = page.getByText(
      'Single-click on parcel to select it',
      { exact: false }
    );

    this.subconChildrenLocateOnMapSelectedLabel = page.getByText('Selected property attributes', {
      exact: true,
    });

    this.subconChildrenLocateOnMapPIDLabel = page.getByText('PID', {
      exact: true,
    });

    this.subconChildrenLocateOnMapPlanLabel = page.getByText('Plan #', {
      exact: true,
    });

    this.subconChildrenLocateOnMapAddressLabel = page.getByText('Address', {
      exact: true,
    });

    this.subconChildrenLocateOnMapRegionLabel = page.getByText('Region', {
      exact: true,
    });

    this.subconChildrenLocateOnMapDistrictLabel = page.getByText('District', {
      exact: true,
    });

    this.subconChildrenSearchTab = page.locator(
      "xpath=//a[contains(text(),'Locate on Map')]/following-sibling::a"
    );

    this.subconChildrenSearchByPIDSelect = page.locator(
      "xpath=//h3[contains(text(),'Search for a property')]/following-sibling::form/div/div/div/div/div/div/select"
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

    this.subconChildrenFirstResultCheckbox = page
      .locator(
        "div[data-testid='map-properties'] div.tbody div.tr-wrapper div.td:first-child input"
      )
      .first();

    this.subconChildrenAddToSelectionButton = page.getByRole('button', {
      name: /Add to selection/i,
    });

    this.subdivisionSelectedChildrenSubtitle = page.getByText('Selected Children', { exact: true });

    this.subdivisionChildrenResultIdentifierColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Identifier')]"
    );

    this.subdivisionChildrenResultPlanColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Plan')]"
    );

    this.subdivisionChildrenResultAreaColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Area m')]"
    );

    this.subdivisionChildrenResultAddressColumn = page.locator(
      "xpath=//p[contains(text(),'Select the child properties to which parent property was subdivided:')]/following-sibling::div[2]//div[@class='collapse show']/div/div[contains(text(),'Address')]"
    );

    this.subdivisionPropertiesCreateButton = page.getByRole('button', {
      name: /Create Subdivision/i,
    });

    this.subconPropertiesCancelButton = page.getByRole('button', {
      name: /Cancel/i,
    });

    this.subconModalWindow = page.locator('.modal-content');

    this.subconWarningHeader = page.locator(
      "xpath=//div[@class='modal-header']/div[contains(text(),'Are you sure?')]"
    );

    this.subconErrorHeader = page.locator(
      "xpath=//div[@class='modal-header']/div[contains(text(),'Error')]"
    );

    this.subconModalSaveWarningP1 = page.locator('.modal-body p').nth(0);
    this.subconModalSaveWarningP2 = page.locator('.modal-body p').nth(1);
    this.subconModalOkBttn = page.locator("button[title='ok-modal']");
  }

  async navigateToCreateNewSubdivision() {
    await this.menuSubdivisionConsolidationButton.click();
    await this.createSubdivisionButton.click();
  }

  async createSubdivision(parentProperty: string, childrenProperties: string[]) {
    await this.subconSearchParentByPIDInput.fill(parentProperty);
    await this.subconSearchParentButton.click();
    await this.subconChildrenSearchTab.click();

    for (const child of childrenProperties) {
      await this.subconChildrenResetButton.click();
      await this.subconChildrenSearchByPIDInput.fill(child);

      await this.subconChildrenSearchButton.click();
      await this.subconChildrenFirstResultCheckbox.check();
      await this.subconChildrenAddToSelectionButton.click();
    }
  }

  async saveSubdivision() {
    await this.subdivisionPropertiesCreateButton.click();

    await expect(this.subconModalWindow).toBeVisible();
    await expect(this.subconWarningHeader).toBeVisible();

    await expect(this.subconModalSaveWarningP1).toHaveText(
      'You are subdividing a property into two or more properties. ' +
        'The old parent property record will be retired, and the new child properties will be created'
    );

    await expect(this.subconModalSaveWarningP2).toHaveText(
      'If you proceed, you will be redirected to the old parent property record, ' +
        'where you can view changes and make updates to the new properties. Do you want to proceed?'
    );

    await this.subconModalOkBttn.click();
  }
}
