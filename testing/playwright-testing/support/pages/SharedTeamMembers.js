const SharedSelectContact = require("./SharedSelectContacts");
const SharedModal = require("./SharedModal");
const { expect } = require("@playwright/test");

class SharedTeamMembers {
  constructor(page) {
    this.page = page;
    this.sharedSelectContact = new SharedSelectContact(this.page);
    this.sharedModal = new SharedModal(page);
  }

  async addTeamMembers(teamMember) {
    await this.page.getByTestId("add-team-member").click();

    const countMembersIdx =
      (await this.page.locator("div[data-testid^='teamMemberRow']").count()) -
      1;

    const contactTypeselects = this.page.locator(
      "select[id^='input-team.'][id$='.contactTypeCode']"
    );
    await contactTypeselects
      .nth(countMembersIdx)
      .selectOption({ label: teamMember.TeamMemberRole });

    await this.page.locator("button[title='Select Contact']").last().click();
    await this.sharedSelectContact.selectContact(
      teamMember.TeamMemberContactName,
      teamMember.TeamMemberContactType
    );

    if (
      await this.page
        .locator(`#input-team.${countMembersIdx}.primaryContactId`)
        .isVisible()
    ) {
      await this.page
        .locator(`#input-team.${countMembersIdx}.primaryContactId`)
        .selectOption({ label: teamMember.TeamMemberPrimaryContact });
    }
  }

  async addMgmtTeamMembers(teamMember) {
    await this.page.getByTestId("add-team-member").click();

    const countMembersIdx =
      (await this.page.locator("div[data-testid^='teamMemberRow']").count()) -
      1;
    const escaped = await this.page.evaluate(
      (str) => CSS.escape(str),
      `${countMembersIdx}`
    );
    const contactTypeselects = this.page.locator(
      "select[id^='input-team.'][id$='.teamProfileTypeCode']"
    );
    await contactTypeselects
      .nth(countMembersIdx)
      .selectOption({ label: teamMember.TeamMemberRole });

    await this.page.locator("button[title='Select Contact']").last().click();
    await this.sharedSelectContact.selectContact(
      teamMember.TeamMemberContactName,
      teamMember.TeamMemberContactType
    );

    if (
      await this.page
        .locator(`#input-team.${escaped}.primaryContactId`)
        .isVisible()
    ) {
      await this.page
        .locator(`#input-team.${escaped}.primaryContactId`)
        .selectOption({ label: teamMember.TeamMemberPrimaryContact });
    }
  }

  async verifyTeamMembersViewForm(teamMembers) {
    let index = 1;

    for (let i = 0; i < teamMembers.length; i++) {
      await expect(
        this.page
          .locator(
            `//div[@data-testid="teamMember[${i}]"]/preceding-sibling::div/label`
          )
          .textContent()
      ).toBe(teamMembers[i].TeamMemberRole + ":");
      await expect(
        this.page.locator(`div[data-testid='teamMember[${i}]'] span`)
      ).toBe(teamMembers[i].TeamMemberContactName);

      if (
        await this.page
          .locator(`div[data-testid='primaryContact[${i}]'] span`)
          .isVisible()
      ) {
        index++;
        await expect(
          this.page.locator(`div[data-testid='primaryContact[${i}]'] span`)
        ).toBe(teamMembers[i].TeamMemberPrimaryContact);
      }

      index++;
    }
  }

  async verifyRequiredTeamMemberMessages() {
    //Add a new Team member form
    await this.page.getByTestId("add-team-member").click();

    //Verify that invalid team member message is displayed
    await this.page
      .locator("#input-team.0.contactTypeCode")
      .selectOption({ label: "Expropriation agent" });
    await this.page.locator("//h2/div/div[contains(text(),'Team')]").click();
    await expect(this.page.locator("invalid-feedback").textContent()).toBe(
      "Select a team member"
    );

    //verify that invalid profile message is displayed
    await this.page
      .locator(
        "div[data-testid='contact-input'] button[title='Select Contact']"
      )
      .click();
    await this.sharedSelectContact.selectContact("Test", "");
    await this.page
      .locator("#input-team.0.contactTypeCode")
      .selectOption({ label: "Select profile..." });
    await this.page.locator("//h2/div/div[contains(text(),'Team')]").click();
    await expect(this.page.locator("invalid-feedback").textContent()).toBe(
      "Select a profile"
    );
  }

  async deleteFirstStaffMember() {
    await this.page.getByTestId("team.0.remove-button").click();

    await this.page
      .locator('div[class="modal-content"]')
      .waitFor({ state: "visible" });

    const actualHeaderTitle = await this.sharedModal.mainModalHeader();
    expect(actualHeaderTitle).toEqual("Remove Team Member");

    const actualModalContent = await this.sharedModal.mainModalContent();
    expect(actualModalContent).toEqual(
      "Do you wish to remove this team member?"
    );
    await this.sharedModal.mainModalClickOKBttn();
  }
}

module.exports = SharedTeamMembers;
