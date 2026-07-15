import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class ResearchImprovementsPage extends LayoutPage {
  readonly page: Page;

  readonly researchImprovementTab: Locator;
  readonly reseachImprovementsTooltip: Locator;
  readonly reseachImprovementsInstructions: Locator;
  readonly researchImprovementsProperties: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.researchImprovementTab = page.getByRole('tab', { name: 'Improvements' });
    this.reseachImprovementsTooltip = page
      .locator('div')
      .filter({ hasText: 'Improvements' })
      .first();
    this.reseachImprovementsInstructions = page.getByText(
      'Click on a property to edit that property improvements in a new tab',
      { exact: true }
    );
    this.researchImprovementsProperties = page.locator('[data-testid^="property-improvements-"]');
  }

  async improvementTabClick() {
    await this.researchImprovementTab.click();
  }
}
