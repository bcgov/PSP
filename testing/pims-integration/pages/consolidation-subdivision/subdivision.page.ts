import { expect, Locator, Page } from '@playwright/test';
import { ConSubHistory } from './consolidation-subdivision-history.page';

export class SubdivisionPage {
  private readonly page: Page;

  readonly menuSubdivisionConsolidationButton: Locator;
  readonly createSubdivisionButton: Locator;

  readonly subdivisionCreateTitle: Locator;
  readonly subdivisionCreateSubtitle: Locator;
  readonly subdivisionParentInstructionsParagraph: Locator;

  readonly subconParentSearchAnchor: Locator;
  readonly subconSearchParentByPIDSelect: Locator;
  readonly subconSearchParentByPIDInput: Locator;
  readonly subconSearchParentButton: Locator;
  readonly subconSearchParentResetButton: Locator;

  readonly subdivisionSelectedParentSubtitle: Locator;
  readonly subconParentResultIdentifierColumn: Locator;
  readonly subconParentResultPlanColumn: Locator;
  readonly subconParentResultAreaColumn: Locator;
  readonly subconParentResultAddressColumn: Locator;

  readonly subdivisionChildrenInstructionsParagraph: Locator;
  readonly subconChildrenLocateOnMapTab: Locator;
  readonly subconChildrenLocateOnMapSubtitle: Locator;
  readonly subconChildrenLocateOnMapBlueIcon: Locator;
  readonly subconChildrenLocateOnMapInstruction1: Locator;
  readonly subconChildrenLocateOnMapInstruction2: Locator;
  readonly subconChildrenLocateOnMapInstruction3: Locator;
  readonly subconChildrenLocateOnMapSelectedLabel: Locator;
  readonly subconChildrenLocateOnMapPIDLabel: Locator;
  readonly subconChildrenLocateOnMapPlanLabel: Locator;
  readonly subconChildrenLocateOnMapAddressLabel: Locator;
  readonly subconChildrenLocateOnMapRegionLabel: Locator;
  readonly subconChildrenLocateOnMapDistrictLabel: Locator;

  readonly subconChildrenSearchTab: Locator;
  readonly subconChildrenSearchByPIDSelect: Locator;
  readonly subconChildrenSearchByPIDInput: Locator;
  readonly subconChildrenSearchButton: Locator;
  readonly subconChildrenResetButton: Locator;
  readonly subconChildrenFirstResultCheckbox: Locator;
  readonly subconChildrenAddToSelectionButton: Locator;

  readonly subdivisionSelectedChildrenSubtitle: Locator;
  readonly subdivisionChildrenResultIdentifierColumn: Locator;
  readonly subdivisionChildrenResultPlanColumn: Locator;
  readonly subdivisionChildrenResultAreaColumn: Locator;
  readonly subdivisionChildrenResultAddressColumn: Locator;

  readonly subdivisionPropertiesCreateButton: Locator;
  readonly subconPropertiesCancelButton: Locator;

  readonly subconModalWindow: Locator;
  readonly subconWarningHeader: Locator;
  readonly subconErrorHeader: Locator;
  readonly subconModalSaveWarningP1: Locator;
  readonly subconModalSaveWarningP2: Locator;
  readonly subconModalOkBttn: Locator;

  //Properties page element:
  readonly propertyDetailsTab: Locator;

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

    //Properties page element:
    this.propertyDetailsTab = page.locator("a[data-rb-event-key='details']");
  }

  async goto() {
    await this.page.goto('/mapview/sidebar/subdivision/new', { waitUntil: 'domcontentloaded' });
  }

  async waitForPropertyPanel() {
    await expect(this.propertyDetailsTab).toBeVisible();
  }

  async createSubdivision(parentProperty: ConSubHistory, childrenProperties: ConSubHistory[]) {
    await this.subconSearchParentByPIDInput.fill(parentProperty.pid);
    await this.subconSearchParentButton.click();
    await this.subconChildrenSearchTab.click();

    for (const child of childrenProperties) {
      await this.subconChildrenResetButton.click();
      await this.subconChildrenSearchByPIDInput.fill(child.pid);

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
