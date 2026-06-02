import test, { BrowserContext, expect, Page } from "@playwright/test";
import { ResearchCreatePage } from "../../pages/research/research-create.page";
import { ResearchListPage } from "../../pages/research/research-list.page";

let context: BrowserContext;
let page: Page;
let researchCreatePage: ResearchCreatePage;
let researchListPage: ResearchListPage;

test.describe.serial("Research File modal", () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    researchCreatePage = new ResearchCreatePage(page);
    researchListPage = new ResearchListPage(page);
  });

  test.afterAll(async () => {
    await context.close();
  });

  test("verify initial Research File Create Form", async () => {
    await researchCreatePage.goto();

    await expect(researchCreatePage.researchTitle).toBeVisible();
    await expect(researchCreatePage.researchLabelName).toBeVisible();
    await expect(researchCreatePage.researchNameInput).toBeVisible();
    await expect(researchCreatePage.researchNameDescription).toBeVisible();
    await expect(researchCreatePage.researchNameHelpLink).toBeVisible();

    await expect(researchCreatePage.researchProjectSubtitle).toBeVisible();
    await expect(researchCreatePage.researchProjectAddLink).toBeVisible();

    await expect(researchCreatePage.researchPropertiesSubtitle).toBeVisible();
    await expect(
      researchCreatePage.researchPropertiesWorkflowLink
    ).toBeVisible();
    await expect(
      researchCreatePage.researchSelectedPropertiesSubtitle
    ).toBeVisible();
    await expect(researchCreatePage.researchPropertiesIdentifier).toBeVisible();
    await expect(
      researchCreatePage.researchPropertiesDescriptiveName
    ).toBeVisible();
    await expect(researchCreatePage.researchPropertiesTooltip).toBeVisible();

    await expect(researchCreatePage.cancelButton).toBeVisible();
    await expect(researchCreatePage.confirmButton).toBeVisible();

    await researchCreatePage.cancelButtonClick();
  });

  test("verify Research File Manage List View", async () => {
    await expect(researchListPage.researchListTitle).toBeVisible();
    await expect(researchListPage.researchNewButton).toBeVisible();

    await expect(researchListPage.researcSearchByLabel).toBeVisible();
    await expect(researchListPage.researchByRegionSelect).toBeVisible();
    await expect(researchListPage.researchByStatusSelect).toBeVisible();
    await expect(researchListPage.researchSearchBySelect).toBeVisible();
    await expect(researchListPage.researchSearchPidInput).toBeVisible();
    await expect(researchListPage.researchSearchRoadInput).toBeVisible();
    await expect(researchListPage.researchSearchDateSelect).toBeVisible();
    await expect(researchListPage.researchSearchDateToInput).toBeVisible();
    await expect(researchListPage.researchSearchDateFromInput).toBeVisible();

    await expect(researchListPage.researchSearchButton).toBeVisible();
    await expect(researchListPage.researchSearchResetButton).toBeVisible();

    await expect(researchListPage.researchListTitle).toBeVisible();

    await expect(researchListPage.researchTable).toBeVisible();
    await expect(researchListPage.researchTableFileNbrHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderByFileNbr).toBeVisible();
    await expect(researchListPage.researchTableFileNameHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderByName).toBeVisible();
    await expect(researchListPage.researchTableMotiRegionHeader).toBeVisible();
    await expect(researchListPage.researchTableCreatedByHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderByCreatedBy).toBeVisible();
    await expect(researchListPage.researchTableCreatedDateHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderCreatedDate).toBeVisible();
    await expect(
      researchListPage.researchTableLastUpdatedByHeader
    ).toBeVisible();
    await expect(researchListPage.researchTableOrderLastUpdated).toBeVisible();
    await expect(
      researchListPage.researchTableLastUpdatedDateHeader
    ).toBeVisible();
    await expect(researchListPage.researchTableOrderUpdatedDate).toBeVisible();
    await expect(researchListPage.researchTableStatusHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderStatus).toBeVisible();

    await expect(researchListPage.researchTableEntriesSpan).toBeVisible();
    await expect(researchListPage.researchTablePagination10).toBeVisible();
    await expect(researchListPage.researchTableNextPageButton).toBeVisible();
    await expect(researchListPage.researchTable1stPageButton).toBeVisible();

    await expect(researchListPage.getResearchListTotal).toBeGreaterThan(0);
  });
});
