import { Locator, Page } from "@playwright/test";
import { LayoutPage } from "../layout/layout.page";

export class ResearchCreatePage extends LayoutPage {
  readonly page: Page;

  readonly researchTitle: Locator;
  readonly researchLabelName: Locator;
  readonly researchNameInput: Locator;
  readonly researchNameDescription: Locator;
  readonly researchNameHelpLink: Locator;

  readonly researchProjectSubtitle: Locator;
  readonly researchProjectAddLink: Locator;

  readonly researchPropertiesSubtitle: Locator;
  readonly researchPropertiesWorkflowLink: Locator;
  readonly researchSelectedPropertiesSubtitle: Locator;
  readonly researchPropertiesIdentifier: Locator;
  readonly researchPropertiesDescriptiveName: Locator;
  readonly researchPropertiesTooltip: Locator;

  readonly cancelButton: Locator;
  readonly confirmButton: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.researchTitle = page.locator("div", {
      hasText: "Create Research File",
    });
    this.researchLabelName = page.locator(
      "//label[normalize-space()='Name this research file:']"
    );
    this.researchNameInput = page.locator("#input-name");
    this.researchNameDescription = page.getByText(
      "A unique file number will be generated for this research file on save.",
      { exact: true }
    );
    this.researchNameHelpLink = page.getByText("Help with choosing a name", {
      exact: true,
    });

    this.researchProjectSubtitle = page
      .locator("div")
      .filter({ hasText: "Project" })
      .first();
    this.researchProjectAddLink = page.locator(
      ':text("+ Add another project")'
    );

    this.researchPropertiesSubtitle = page.locator(
      ':text("Properties to include in this file:")'
    );
    this.researchPropertiesWorkflowLink = page.locator(':text("New workflow")');
    this.researchSelectedPropertiesSubtitle = page.locator(
      ':text("Selected Properties")'
    );
    this.researchPropertiesIdentifier = page.locator(':text("Identifier")');
    this.researchPropertiesDescriptiveName = page.locator(
      ':text("Provide a descriptive name for this land")'
    );
    this.researchPropertiesTooltip = page.locator(
      "//span[@id='property-selector-tooltip']"
    );

    this.cancelButton = page.locator(
      "button[data-testid='cancel-modal-button']"
    );
    this.confirmButton = page.locator(
      "div[class='modal-footer'] a[data-testid='ok-modal-button']"
    );
  }

  async goto() {
    await this.page.goto("/mapview/sidebar/research/new");
  }

  async cancelButtonClick() {
    await this.cancelButton.click();
  }

  async confirmButtonClick() {
    await this.confirmButton.click();
  }
}
