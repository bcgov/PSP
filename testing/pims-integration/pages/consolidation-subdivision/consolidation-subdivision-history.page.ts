import { expect, Locator, Page } from '@playwright/test';
import { formatSqToMts } from '../../utils/utils';

type conSubHistory = {
  pid: string;
  plan: string;
  status: string;
  area: string;
};

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

  async verifySubdivisionHistory(PID: string, plan: string, status: string, area: string, subdivisions: conSubHistory[]) {
    await expect(this.subdivisionHistorySubtitle).toBeVisible();

    await expect(this.subconHistoryCreatedOnLabel).toBeVisible();
    await expect(this.subdivisionHistoryTableParentColumn).toBeVisible();
    await expect(this.subconHistoryTableIDColumn).toBeVisible();
    await expect(this.subconHistoryTablePlanColumn).toBeVisible();
    await expect(this.subconHistoryTableStatusColumn).toBeVisible();
    await expect(this.subconHistoryTableAreaColumn).toBeVisible();
    await expect(this.subdivisionParentIdentifier).toHaveText(
      `PID: ${PID}`
    );
    await expect(this.subdivisionParentPlan).toHaveText(plan);
    await expect(this.subdivisionParentStatus).toHaveText(status);
    await expect(this.subdivisionParentArea).toHaveText(area);

    for (let i = 0; i < subdivisions.length; i++) {
      const child = subdivisions[i];
      const childRow = this.subconTableContent.nth(i + 1);
      const cells = childRow.locator("[role='cell']");

      await expect(cells.nth(2).locator('a')).toHaveText(`PID: ${child.pid}`);
      await expect(cells.nth(3)).toHaveText(child.plan);
      await expect(cells.nth(4)).toHaveText(child.status);
      await expect(cells.nth(5)).toHaveText(formatSqToMts(child.area));
    }
  }

  async verifyConsolidationHistory(consolidations: conSubHistory[], PID: string, plan: string, status: string, area: string,) {
    await expect(this.propertyInformationTitle).toBeVisible();
    await expect(this.consolidationHistorySubtitle).toBeVisible();

    await expect(this.subconHistoryCreatedOnLabel).toBeVisible();
    await expect(this.consolidationHistoryTableChildColumn).toBeVisible();
    await expect(this.subconHistoryTableIDColumn).toBeVisible();
    await expect(this.subconHistoryTablePlanColumn).toBeVisible();
    await expect(this.subconHistoryTableStatusColumn).toBeVisible();
    await expect(this.subconHistoryTableAreaColumn).toBeVisible();

    for (let i = 0; i < consolidations.length; i++) {
      const parent = consolidations[i];
      const parentRow = this.subconTableContent.nth(i);
      const cells = parentRow.locator("[role='cell']");

      await expect(cells.nth(2).locator('a')).toHaveText(
        `PID: ${parent.pid}`
      );
      await expect(cells.nth(3)).toHaveText(parent.plan);
      await expect(cells.nth(4)).toHaveText(parent.status);
      await expect(cells.nth(5)).toHaveText(formatSqToMts(parent.area));
    }

    const numberOfRows = await this.subconTableContent.count();

    // Last row contains the consolidation child.
    const childRow = this.subconTableContent.nth(numberOfRows - 1);
    const cells = childRow.locator("[role='cell']");

    await expect(cells.nth(2).locator('a')).toHaveText(`PID: ${PID}`);
    await expect(cells.nth(3)).toHaveText(plan);
    await expect(cells.nth(4)).toHaveText(status);
    await expect(cells.nth(5)).toHaveText(formatSqToMts(area));
  }
}
