import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class ResearchViewFileDetails extends LayoutPage {
  readonly page: Page;

  readonly researchEditButton: Locator;

  readonly researchProjectSubtitle: Locator;
  readonly researchProjectLabel: Locator;
  readonly researchProjectContent: Locator;

  readonly reseachRoadTitle: Locator;
  readonly researchRoadNameLabel: Locator;
  readonly researchRoadNameContent: Locator;
  readonly researchRoadAliasLabel: Locator;
  readonly researchRoadAliasContent: Locator;

  readonly researchRequestTitle: Locator;
  readonly researchPurposeLabel: Locator;
  readonly researchPurposeContent: Locator;
  readonly researchRequestDateLabel: Locator;
  readonly researchSourceRequestLabel: Locator;
  readonly researchRequestDateContent: Locator;
  readonly researchRequesterLabel: Locator;
  readonly researchSourceRequestContent: Locator;
  readonly researchRequesterContent: Locator;
  readonly researchDescriptionLabel: Locator;
  readonly researchDescriptionContent: Locator;

  readonly researchResultTitle: Locator;
  readonly researchCompletedOnLabel: Locator;
  readonly researchResultRequestLabel: Locator;

  readonly researchExpropiationTitle: Locator;
  readonly researchExpropriationBool: Locator;
  readonly researchExpropriationComments: Locator;

  constructor(page: Page) {
    super(page);
    this.page = page;

    this.researchEditButton = page.getByTitle('Edit research file', { exact: true });

    this.researchProjectSubtitle = page.locator('div').filter({ hasText: 'Project' }).first();
    this.researchProjectLabel = page.getByText('Ministry project:', { exact: true });
    this.researchProjectContent = page.locator(
      "//label[text()='Ministry project']/parent::div/following-sibling::div"
    );

    this.reseachRoadTitle = page.getByText('Roads', { exact: true });
    this.researchRoadNameLabel = page.locator('label:has-text("Road name:")');
    this.researchRoadNameContent = page.locator(
      "//label[text()='Road name:']/parent::div/following-sibling::div"
    );
    this.researchRoadAliasLabel = page.locator('label:has-text("Road alias:")');
    this.researchRoadAliasContent = page.locator(
      "//label[text()='Road alias:']/parent::div/following-sibling::div"
    );

    this.researchRequestTitle = page.getByText('Research Request', { exact: true });
    this.researchPurposeLabel = page.getByText('Research purpose:', { exact: true });
    this.researchPurposeContent = page.locator(
      "//label[text()='Research purpose:']/parent::div/following-sibling::div"
    );
    this.researchRequestDateLabel = page.getByText('Request date:', { exact: true });
    this.researchRequestDateContent = page.locator(
      "//label[text()='Request date:']/parent::div/following-sibling::div"
    );
    this.researchSourceRequestLabel = page.getByText('Source of request:', { exact: true });
    this.researchSourceRequestContent = page.locator(
      "//label[text()='Source of request:']/parent::div/following-sibling::div"
    );
    this.researchRequesterLabel = page.locator('label:has-text("Requester:")');
    this.researchRequesterContent = page.locator(
      "//label[text()='Source of request:']/parent::div/following-sibling::div"
    );
    this.researchDescriptionLabel = page.locator('label:has-text("Requester:")');
    this.researchDescriptionContent = page.locator(
      "//label[text()='Requester:']/parent::div/following-sibling::div"
    );

    this.researchResultTitle = page.locator(':text-is("Result")');
    this.researchCompletedOnLabel = page.locator('label:has-text("Research completed on:")');
    this.researchResultRequestLabel = page.locator('label:has-text("Result of request:")');

    this.researchExpropiationTitle = page.getByText('Expropriation', { exact: true });
    this.researchExpropriationBool = page.locator('label:has-text("Expropriation?:")');
    this.researchExpropriationComments = page.locator('label:has-text("Expropriation comments:")');
  }
}
