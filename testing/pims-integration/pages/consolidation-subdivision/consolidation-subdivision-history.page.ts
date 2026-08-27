import { expect, Locator, Page } from '@playwright/test';
import { formatSqToMts } from '../../utils/utils';

export class ConsolidationSubdivisionHistoryPage {
  private readonly page: Page;

  private readonly propertyInformationTitle: Locator;

  private readonly subdivisionHistorySubtitle: Locator;
  private readonly consolidationHistorySubtitle: Locator;
  private readonly subconHistoryCreatedOnLabel: Locator;

  private readonly subdivisionHistoryTableParentColumn: Locator;
  private readonly consolidationHistoryTableChildColumn: Locator;
  private readonly subconHistoryTableIDColumn: Locator;
  private readonly subconHistoryTablePlanColumn: Locator;
  private readonly subconHistoryTableStatusColumn: Locator;
  private readonly subconHistoryTableAreaColumn: Locator;

  private readonly subdivisionParentIdentifier: Locator;
  private readonly subdivisionParentPlan: Locator;
  private readonly subdivisionParentStatus: Locator;
  private readonly subdivisionParentArea: Locator;

  private readonly subconTableContent: Locator;

  constructor(page: Page) {
    this.page = page;

    this.propertyInformationTitle = page.getByRole('heading', {
      level: 1,
      name: /Property Information/i,
    });

    this.subdivisionHistorySubtitle = page.getByText('Subdivision History', {
      exact: true,
    });

    this.consolidationHistorySubtitle = page.getByText('Consolidation History', { exact: true });

    this.subconHistoryCreatedOnLabel = page.getByText('Created on', { exact: true });

    const operationTable = page.locator("div[data-testid='propertyOperationTable']");

    this.subdivisionHistoryTableParentColumn = operationTable
      .locator('.thead')
      .getByText('Parent', { exact: true });
    this.consolidationHistoryTableChildColumn = operationTable
      .locator('.thead')
      .getByText('Child', { exact: true });

    this.subconHistoryTableIDColumn = operationTable
      .locator('.thead')
      .getByText('Identifier', { exact: true });

    this.subconHistoryTablePlanColumn = operationTable
      .locator('.thead')
      .getByText('Plan #', { exact: true });

    this.subconHistoryTableStatusColumn = operationTable
      .locator('.thead')
      .getByText('Status', { exact: true });

    this.subconHistoryTableAreaColumn = operationTable
      .locator('.thead')
      .getByText('Area', { exact: true });

    const subdivisionParentRow = operationTable.locator('.tbody .tr-wrapper').first();

    this.subdivisionParentIdentifier = subdivisionParentRow
      .locator("[role='cell']")
      .nth(2)
      .locator('a');

    this.subdivisionParentPlan = subdivisionParentRow.locator("[role='cell']").nth(3);

    this.subdivisionParentStatus = subdivisionParentRow.locator("[role='cell']").nth(4);

    this.subdivisionParentArea = subdivisionParentRow.locator("[role='cell']").nth(5);

    this.subconTableContent = operationTable.locator('.tbody .tr-wrapper');
  }

  async verifySubdivisionHistory() {
    await expect(this.subdivisionHistorySubtitle).toBeVisible();

    await expect(this.subconHistoryCreatedOnLabel).toBeVisible();
    await expect(this.subdivisionHistoryTableParentColumn).toBeVisible();
    await expect(this.subconHistoryTableIDColumn).toBeVisible();
    await expect(this.subconHistoryTablePlanColumn).toBeVisible();
    await expect(this.subconHistoryTableStatusColumn).toBeVisible();
    await expect(this.subconHistoryTableAreaColumn).toBeVisible();
    await expect(this.subdivisionParentIdentifier).toHaveText(
      `PID: ${subdivision.subdivisionSource.propertyHistoryIdentifier}`
    );
    await expect(this.subdivisionParentPlan).toHaveText(
      subdivision.subdivisionSource.propertyHistoryPlan
    );
    await expect(this.subdivisionParentStatus).toHaveText(
      subdivision.subdivisionSource.propertyHistoryStatus
    );
    await expect(this.subdivisionParentArea).toHaveText(
      this.transformSqMtsFormat(subdivision.subdivisionSource.propertyHistoryArea)
    );

    for (let i = 0; i < subdivision.subdivisionDestination.length; i++) {
      const child = subdivision.subdivisionDestination[i];

      const childRow = this.subconTableContent.nth(i + 1);
      const cells = childRow.locator("[role='cell']");

      await expect(cells.nth(2).locator('a')).toHaveText(`PID: ${child.propertyHistoryIdentifier}`);

      await expect(cells.nth(3)).toHaveText(child.propertyHistoryPlan);

      await expect(cells.nth(4)).toHaveText(child.propertyHistoryStatus);

      await expect(cells.nth(5)).toHaveText(formatSqToMts(child.propertyHistoryArea));
    }
  }

  async verifyConsolidationHistory() {
    await expect(this.propertyInformationTitle).toBeVisible();
    await expect(this.consolidationHistorySubtitle).toBeVisible();

    await expect(this.subconHistoryCreatedOnLabel).toBeVisible();
    await expect(this.consolidationHistoryTableChildColumn).toBeVisible();
    await expect(this.subconHistoryTableIDColumn).toBeVisible();
    await expect(this.subconHistoryTablePlanColumn).toBeVisible();
    await expect(this.subconHistoryTableStatusColumn).toBeVisible();
    await expect(this.subconHistoryTableAreaColumn).toBeVisible();

    for (let i = 0; i < consolidation.consolidationSource.length; i++) {
      const parent = consolidation.consolidationSource[i];
      const parentRow = this.subconTableContent.nth(i);
      const cells = parentRow.locator("[role='cell']");

      await expect(cells.nth(2).locator('a')).toHaveText(
        `PID: ${parent.propertyHistoryIdentifier}`
      );
      await expect(cells.nth(3)).toHaveText(parent.propertyHistoryPlan);
      await expect(cells.nth(4)).toHaveText(parent.propertyHistoryStatus);
      await expect(cells.nth(5)).toHaveText(formatSqToMts(parent.propertyHistoryArea));
    }

    const numberOfRows = await this.subconTableContent.count();

    // Last row contains the consolidation child.
    const childRow = this.subconTableContent.nth(numberOfRows - 1);
    const cells = childRow.locator("[role='cell']");
    const child = consolidation.consolidationDestination;

    await expect(cells.nth(2).locator('a')).toHaveText(`PID: ${child.propertyHistoryIdentifier}`);
    await expect(cells.nth(3)).toHaveText(child.propertyHistoryPlan);
    await expect(cells.nth(4)).toHaveText(child.propertyHistoryStatus);
    await expect(cells.nth(5)).toHaveText(formatSqToMts(child.propertyHistoryArea));
  }
}
