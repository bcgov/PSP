import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class ResearchListPage extends LayoutPage {
  readonly page: Page;

  readonly projectMainOptionMenu: Locator;
  readonly reseachMainMenuOption: Locator;
  readonly researchListViewOption: Locator;

  readonly researchListTitle: Locator;
  readonly researchNewButton: Locator;

  readonly researcSearchByLabel: Locator;
  readonly researchByRegionSelect: Locator;
  readonly researchSearchNameInput: Locator;
  readonly researchByStatusSelect: Locator;
  readonly researchSearchBySelect: Locator;
  readonly researchSearchPidInput: Locator;
  readonly researchSearchRoadInput: Locator;
  readonly researchSearchDateSelect: Locator;
  readonly researchSearchDateToInput: Locator;
  readonly researchSearchDateFromInput: Locator;

  readonly researchSearchButton: Locator;
  readonly researchSearchResetButton: Locator;

  readonly researchTable: Locator;
  readonly researchTableFileNbrHeader: Locator;
  readonly researchTableOrderByFileNbr: Locator;
  readonly researchTableFileNameHeader: Locator;
  readonly researchTableOrderByName: Locator;
  readonly researchTableMotiRegionHeader: Locator;
  readonly researchTableCreatedByHeader: Locator;
  readonly researchTableOrderByCreatedBy: Locator;
  readonly researchTableCreatedDateHeader: Locator;
  readonly researchTableOrderCreatedDate: Locator;
  readonly researchTableLastUpdatedByHeader: Locator;
  readonly researchTableOrderLastUpdated: Locator;
  readonly researchTableLastUpdatedDateHeader: Locator;
  readonly researchTableOrderUpdatedDate: Locator;
  readonly researchTableStatusHeader: Locator;
  readonly researchTableOrderStatus: Locator;

  readonly researchTableEntriesSpan: Locator;
  readonly researchTablePagination5: Locator;
  readonly researchTablePagination10: Locator;
  readonly researchTablePagination20: Locator;
  readonly researchTablePagination50: Locator;
  readonly researchTablePagination100: Locator;

  readonly researchTableContent: Locator;

  readonly researchTableNextPageButton: Locator;
  readonly researchTable1stPageButton: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.projectMainOptionMenu = page.getByTestId('nav-tooltip-project');
    this.reseachMainMenuOption = page.getByTestId('nav-tooltip-research');
    this.researchListViewOption = page.getByRole('link', {
      name: 'Manage Research Files',
    });

    this.researchListTitle = page.getByText('Research Files', { exact: true });
    this.researchNewButton = page.getByRole('button', {
      name: /Create a Research File/i,
    });

    this.researcSearchByLabel = page.getByText('Search by:', { exact: true });
    this.researchByRegionSelect = page.locator('#input-regionCode');
    this.researchByStatusSelect = page.locator('#input-researchFileStatusTypeCode');
    this.researchSearchBySelect = page.locator('#input-researchSearchBy');
    this.researchSearchPidInput = page.locator('#input-pid');
    this.researchSearchNameInput = page.locator('#input-name');
    this.researchSearchRoadInput = page.locator('#input-roadOrAlias');
    this.researchSearchDateSelect = page.locator('#input-createOrUpdateRange');
    this.researchSearchDateToInput = page.locator('#datepicker-updatedOnStartDate');
    this.researchSearchDateFromInput = page.locator('#datepicker-updatedOnEndDate');

    this.researchSearchButton = page.locator('#search-button');
    this.researchSearchResetButton = page.getByTestId('reset-button');

    this.researchTable = page.getByTestId('researchFilesTable');
    this.researchTableFileNbrHeader = page.locator(':text-is("File #")');
    this.researchTableOrderByFileNbr = page.getByTestId('sort-column-rfileNumber');
    this.researchTableFileNameHeader = page.locator(':text-is("File name")');
    this.researchTableOrderByName = page.getByTestId('sort-column-name');
    this.researchTableMotiRegionHeader = page.locator(':text-is("MOTT region")');
    this.researchTableCreatedByHeader = page
      .getByTestId('researchFilesTable')
      .getByText('Created by');
    this.researchTableOrderByCreatedBy = page.getByTestId('sort-column-appCreateUserid');
    this.researchTableCreatedDateHeader = page
      .getByTestId('researchFilesTable')
      .getByText('Created date');
    this.researchTableOrderCreatedDate = page.getByTestId('sort-column-appCreateTimestamp');
    this.researchTableLastUpdatedByHeader = page
      .getByTestId('researchFilesTable')
      .getByText('Last updated by');
    this.researchTableOrderLastUpdated = page.getByTestId('sort-column-appLastUpdateUserid');
    this.researchTableLastUpdatedDateHeader = page.locator(':text-is("Last updated date")');
    this.researchTableOrderUpdatedDate = page.getByTestId('sort-column-appLastUpdateTimestamp');
    this.researchTableStatusHeader = page.locator(':text-is("Status")');
    this.researchTableOrderStatus = page.getByTestId('sort-column-researchFileStatusTypeCode');

    this.researchTableContent = page.locator(
      "div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
    );

    this.researchTableEntriesSpan = page.locator("input[data-testid='input-page-size']");
    this.researchTablePagination5 = page.locator(
      "div[class='Menu-options scrollable list-group'] div[title='menu-item-5']"
    );
    this.researchTablePagination10 = page.locator("div[title='menu-item-10']");
    this.researchTablePagination20 = page.locator("div[title='menu-item-20']");
    this.researchTablePagination50 = page.locator("div[title='menu-item-50']");
    this.researchTablePagination100 = page.locator("div[title='menu-item-100']");

    this.researchTableNextPageButton = page.locator("ul[class='pagination'] li:last-child");
    this.researchTable1stPageButton = page.locator("ul[class='pagination'] li:nth-child(2)");
  }

  async goto() {
    await this.page.goto('/research/list');
  }

  async getResearchListTotal(): Promise<number> {
    await this.researchTableContent.first().waitFor({ state: 'visible' });
    return await this.researchTableContent.count();
  }

  async createNewResearchClick() {
    await this.researchNewButton.click();
  }

  async openResearchFileInNewTab(index: number): Promise<Page> {
    const selectedFile = this.page
      .getByTestId('researchFilesTable')
      .locator('.tbody .tr-wrapper')
      .nth(index - 1)
      .locator('a');
    const [newPage] = await Promise.all([
      this.page.context().waitForEvent('page'),
      selectedFile.click(),
    ]);

    await newPage.waitForLoadState('domcontentloaded');
    await newPage.waitForURL(/\/mapview\/sidebar\/research\/\d+/, {
      timeout: 30000,
    });
    // eslint-disable-next-line playwright/no-networkidle
    await newPage.waitForLoadState('networkidle');
    await newPage.bringToFront();

    return newPage;
  }

  async searchByName(name: string) {
    await this.researchSearchBySelect.selectOption('Research file name');
    await this.researchSearchNameInput.fill(name);
    await this.researchSearchButton.click();
  }

  async clickResearchFileResult(index: number): Promise<void> {
    const link = this.page
      .getByTestId('researchFilesTable')
      .locator('.tbody .tr-wrapper')
      .nth(index - 1)
      .locator('a');

    await link.click();
  }
}
