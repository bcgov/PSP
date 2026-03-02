const { expect } = require("@playwright/test");
const AcquisitionsDetails = require("./AcquisitionDetails");
const ResearchDetails = require("./ResearchFiles");
const LeaseDetails = require("./LeaseLicences");
const DispositionDetails = require("./DispositionFiles");
const ManagementDetails = require("./ManagementFile");
const acquisitionData = require("../../data/AcquisitionFiles.json");
const researchData = require("../../data/ResearchFiles.json");
const leaseData = require("../../data/Leases.json");
const dispositionData = require("../../data/DispositionFiles.json");
const managementData = require("../../data/ManagementFiles.json");

class WorkLists {
  constructor(page) {
    this.page = page;
    this.acquisitionDetails = new AcquisitionsDetails(page);
    this.researchDetails = new ResearchDetails(page);
    this.leaseDetails = new LeaseDetails(page);
    this.dispositionDetails = new DispositionDetails(page);
    this.managementDetails = new ManagementDetails(page);
  }

  async navigateWorkLists() {
    await this.page.locator("#worklistControlButton").click();
  }

  async verifyWorkListForm(propertyCount) {
    await expect(
      this.page.locator("//p[contains(text(),'Working list')]")
    ).toBeVisible();

    if (propertyCount > 0) {
      await expect(
        this.page.locator("//div[@data-testid='worklist-scroll-area']/div")
      ).toHaveCount(propertyCount);
    } else {
      await expect(
        this.page.locator(
          "//div[contains(text(),'CTRL + Click to add a property')]"
        )
      ).toBeVisible();
    }
  }

  async verifyWorklistMenuUptions() {
    const worklistMoreOptionsBttn = await this.page
      .locator(
        "div[data-testid='worklist-sidebar'] button[id='dropdown-ellipsis']"
      )
      .first();
    expect(worklistMoreOptionsBttn).toBeVisible();
    await worklistMoreOptionsBttn.click();

    await this.page
      .locator("div[aria-labelledby='dropdown-ellipsis']")
      .waitFor({ status: "visible" });

    const clearListOption = await this.page.locator(
      "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Clear list']"
    );
    expect(clearListOption).toBeVisible();

    const createResearchOption = await this.page.locator(
      "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Research File']"
    );
    expect(createResearchOption).toBeVisible();

    const createAcquisitionOption = await this.page.locator(
      "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Acquisition File']"
    );
    expect(createAcquisitionOption).toBeVisible();

    const createManagementOption = await this.page.locator(
      "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Management File']"
    );
    expect(createManagementOption).toBeVisible();

    const createLeaseOption = await this.page.locator(
      "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Lease File']"
    );
    expect(createLeaseOption).toBeVisible();

    const createDispositionOption = await this.page.locator(
      "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Disposition File']"
    );
    expect(createDispositionOption).toBeVisible();

    const addToOpenFileOption = await this.page.locator(
      "//span[text()='Add to Open File']"
    );
    expect(addToOpenFileOption).toBeVisible();

    const addToOpenFileTooltip = await this.page.locator(
      "span[data-testid='tooltip-icon-tooltip-6']"
    );
    expect(addToOpenFileTooltip).toBeVisible();
  }

  async verifyWorklistWithProps() {
    this.page.locator("#worklistControlButton").click();

    await expect(
      this.page.locator("//p[contains(text(),'Working list')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("search-property-0")).toBeVisible();
  }

  async verifyWorklistStrata() {
    const planElement = await this.page.getByTestId("worklist-item[0].parcel.identifier");
    await expect(planElement).toBeVisible();

    const collapseBttn = await this.page.getByTestId("worklist-item[0].collapse-btn");
    await expect(collapseBttn).toBeVisible();

    const planChild1 = await this.page.getByTestId("worklist-item[0].child[0]");
    await expect(planChild1).toBeVisible();

    const planChild2 = await this.page.getByTestId("worklist-item[0].child[1]");
    await expect(planChild2).toBeVisible();
  }

  async countItemsOnWorklist(propertyCountData) {
    const countWorklistItems = await this.page.locator(
      "//button[@id='worklistControlButton']/following-sibling::div"
    );
    await countWorklistItems.waitFor({ state: "visible" });
    const worklistItemNumber = await countWorklistItems.textContent();
    return worklistItemNumber == propertyCountData;
  }

  async deleteNthElementWorklist(index) {
    const prevItemsCount = await this.page.locator(
      "//button[@id='worklistControlButton']/following-sibling::div"
    );
    const intPrevItemsCount = parseInt(await prevItemsCount.textContent());
    const propertyItem = await this.page
      .locator(`div[data-testid='worklist-item[${index}].parcel']`)
      .first();
    await propertyItem.hover();

    const deleteBttn = await this.page.locator(
      `div[data-testid='worklist-item[${index}].parcel'] button[title='Delete parcel from list']`
    );
    await expect(deleteBttn).toBeVisible();
    await deleteBttn.click();
    const afterItemsCount = await this.page.locator(
      "//button[@id='worklistControlButton']/following-sibling::div"
    );
    const intAfterItemsCount = parseInt(await afterItemsCount.textContent());
    expect(intPrevItemsCount).toBeGreaterThan(intAfterItemsCount);
  }

  async createFileWithProps(fileType) {
    const worklistMoreOptionsBttn = await this.page
      .locator(
        "div[data-testid='worklist-sidebar'] button[id='dropdown-ellipsis']"
      )
      .first();
    expect(worklistMoreOptionsBttn).toBeVisible();
    await worklistMoreOptionsBttn.click();

    await this.page
      .locator("div[aria-labelledby='dropdown-ellipsis']")
      .waitFor({ status: "visible" });

    switch (fileType) {
      case "acquisition":
        const createAcquisitionOption = await this.page.locator(
          "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Acquisition File']"
        );
        expect(createAcquisitionOption).toBeVisible();
        await createAcquisitionOption.click();
        await this.acquisitionDetails.createMinimumAcquisitionFile(
          acquisitionData[0]
        );
        break;
      case "management":
        const createManagementOption = await this.page.locator(
          "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Management File']"
        );
        expect(createManagementOption).toBeVisible();
        await createManagementOption.click();
        this.managementDetails.createMinimumManagementDetails(
          managementData[0]
        );
        break;
      case "lease":
        const createLeaseOption = await this.page.locator(
          "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Lease File']"
        );
        expect(createLeaseOption).toBeVisible();
        await createLeaseOption.click();
        await this.leaseDetails.createMinimumLease(leaseData[0]);
        break;
      case "disposition":
        const createDispositionOption = await this.page.locator(
          "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Disposition File']"
        );
        expect(createDispositionOption).toBeVisible();
        await createDispositionOption.click();
        this.dispositionDetails.createMinimumDispositionFile(
          dispositionData[0]
        );
        break;
      default:
        const createResearchOption = await this.page.locator(
          "div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Research File']"
        );
        expect(createResearchOption).toBeVisible();
        await createResearchOption.click();
        this.researchDetails.createMinimumResearchFile(researchData[0]);
    }
  }
}

module.exports = WorkLists;
