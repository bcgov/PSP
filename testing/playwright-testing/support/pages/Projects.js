const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class Projects {
  constructor(page) {
    this.page = page;
  }

  async navigateCreateProject() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Create Project']").click();
  }

  async navigateProjectListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Projects']").click();
  }

  async verifyProjectsListView() {
    await expect(this.page.locator("//span[text()='Projects']")).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(
      this.page.locator("//strong[text()='Search by:']")
    ).toBeVisible();
    await expect(this.page.locator("#input-projectNumber")).toBeVisible();
    await expect(this.page.locator("#input-projectName")).toBeVisible();

    const regionSelect = await this.page.locator("#input-projectRegionCode");
    expect(regionSelect).toBeVisible();
    const regionOptions = await regionSelect.locator("option");
    expect(regionOptions).toHaveCountGreaterThan(0);

    const statusSelect = await this.page.locator("#input-projectStatusCode");
    expect(statusSelect).toBeVisible();
    const statusOptions = await statusSelect.locator("option");
    expect(statusOptions).toHaveCountGreaterThan(0);

    await expect(this.page.getByTestId("search")).toBeVisible();
    await expect(this.page.getByTestId("reset-button")).toBeVisible();

    await expect(this.page.getByTestId("projectsTable")).toBeVisible();

    const projectNbrColumn = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1)"
    );
    await expect(projectNbrColumn).toBeVisible();
    await expect(projectNbrColumn).toHaveText("Project #");

    const projectNameColumn = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2)"
    );
    await expect(projectNameColumn).toBeVisible();
    await expect(projectNameColumn).toHaveText("Project name");

    const projectRegionColumn = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3)"
    );
    await expect(projectRegionColumn).toBeVisible();
    await expect(projectRegionColumn).toHaveText("Region");

    const projectStatusColumn = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4)"
    );
    await expect(projectStatusColumn).toBeVisible();
    await expect(projectStatusColumn).toHaveText("Status");

    const projectLastUpdateColumn = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5)"
    );
    await expect(projectLastUpdateColumn).toBeVisible();
    await expect(projectLastUpdateColumn).toHaveText("Last updadted by");

    const projectUpdateDateColumn = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(6)"
    );
    await expect(projectUpdateDateColumn).toBeVisible();
    await expect(projectUpdateDateColumn).toHaveText("Updated date");

    const projectsCountTable = await this.page.locator(
      "div[data-testid='projectsTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    await expect(projectsCountTable).toHaveCountGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async verifyCreateProjectForm() {
    const projectTitle = await this.page.getByTestId("form-title");
    expect(projectTitle).toHaveText("Create Project");

    await expect(
      this.page.locator("//div[text()='Project Details']")
    ).toBeVisible();
    await expect(
      this.page.locator("//p[contains(text(),'Before creating a project')]")
    ).toBeVisible();

    await expect(
      this.page.locator("//label[contains(text(),'Project name')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-projectName")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Project number')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-projectNumber")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Status')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-projectStatusType")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'MOTT region')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-region")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Project summary')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-summary")).toBeVisible();

    await expect(
      this.page.locator("//div[text()='Associated Codes']")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Cost type')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#typeahead-select-costTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Work activity')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#typeahead-select-workActivityCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Business function')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#typeahead-select-businessFunctionCode")
    ).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Associated products')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'+ Add another product')]/parent::button"
      )
    ).toBeVisible();

    await expect(
      this.page.locator(
        "//h2/div/div[contains(text(), 'Project Management Team')]"
      )
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Cancel')]/parent::button")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Save')]/parent::button")
    ).toBeVisible();
  }

  async cancelCreateProject() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }
}

module.exports = Projects;
