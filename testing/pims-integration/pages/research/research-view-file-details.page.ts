import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class ResearchViewFileDetails extends LayoutPage {
  readonly page: Page;

  readonly researchDocumentTab: Locator;

  readonly researchEditButton: Locator;

  readonly researchProjectSubtitle: Locator;
  readonly researchProjectLabel: Locator;

  readonly reseachRoadTitle: Locator;
  readonly researchRoadNameLabel: Locator;
  readonly researchRoadAliasLabel: Locator;

  readonly researchRequestTitle: Locator;
  readonly researchPurposeLabel: Locator;
  readonly researchRequestDateLabel: Locator;
  readonly researchSourceRequestLabel: Locator;

  readonly researchRequesterLabel: Locator;
  readonly researchSourceRequestContent: Locator;
  readonly researchRequesterContent: Locator;
  readonly researchDescriptionLabel: Locator;

  readonly researchResultTitle: Locator;
  readonly researchCompletedOnLabel: Locator;
  readonly researchResultRequestLabel: Locator;

  readonly researchExpropiationTitle: Locator;
  readonly researchExpropriationBool: Locator;
  readonly researchExpropriationComments: Locator;

  constructor(page: Page) {
    super(page);
    this.page = page;

    this.researchDocumentTab = page.getByRole('tab', { name: 'Documents' });

    this.researchEditButton = page.getByTitle('Edit research file', { exact: true });

    this.researchProjectSubtitle = page.locator('div').filter({ hasText: 'Project' }).first();
    this.researchProjectLabel = page.getByText('Ministry project:', { exact: true });

    this.reseachRoadTitle = page.getByText('Roads', { exact: true });
    this.researchRoadNameLabel = page.locator('label:has-text("Road name:")');

    this.researchRoadAliasLabel = page.locator('label:has-text("Road alias:")');

    this.researchRequestTitle = page.getByText('Research Request', { exact: true });
    this.researchPurposeLabel = page.getByText('Research purpose:', { exact: true });

    this.researchRequestDateLabel = page.getByText('Request date:', { exact: true });

    this.researchSourceRequestLabel = page.getByText('Source of request:', { exact: true });
    this.researchSourceRequestContent = page.locator(
      "//label[text()='Source of request:']/parent::div/following-sibling::div"
    );
    this.researchRequesterLabel = page.locator('label:has-text("Requester:")');
    this.researchRequesterContent = page.locator(
      "//label[text()='Source of request:']/parent::div/following-sibling::div"
    );
    this.researchDescriptionLabel = page.locator('label:has-text("Requester:")');

    this.researchResultTitle = page.locator(':text-is("Result")');
    this.researchCompletedOnLabel = page.locator('label:has-text("Research completed on:")');
    this.researchResultRequestLabel = page.locator('label:has-text("Result of request:")');

    this.researchExpropiationTitle = page.getByText('Expropriation', { exact: true });
    this.researchExpropriationBool = page.locator('label:has-text("Expropriation?:")');
    this.researchExpropriationComments = page.locator('label:has-text("Expropriation comments:")');
  }

  async navigateDocumentsTab() {
    await this.researchDocumentTab.click();
  }

  async getFieldValueByLabel(label: string): Promise<string> {
    return await this.page
      .locator(`//label[contains(normalize-space(), '${label}')]/parent::div/following-sibling::div`)
      .innerText();
  }

  async getResearchDescription(): Promise<string> {
    return (
      await this.page
        .getByTestId('request-description')
        .locator('label')
        .innerText()
    ).trim();
  }

  async getResearchResult(): Promise<string> {
    return (
      await this.page
        .getByTestId('research-result')
        .locator('label')
        .innerText()
    ).trim();
  }

  async getResearchExpropriationNotes(): Promise<string> {
    return (
      await this.page
        .getByTestId('expropriation-notes')
        .locator('label')
        .innerText()
    ).trim();
  }

}
