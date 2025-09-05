const { clickSaveButton, clickAndWaitFor } = require("../../support/common.js");
const { expect } = require("@playwright/test");

class SharedFileProperties {
  constructor(page) {
    this.page = page;
  }

  async navigateToAddPropertiesToFile() {
    clickAndWaitFor(
      this.page,
      "button[title='Change properties']",
      "//div[text()='Selected Properties']"
    );
  }

  // async navigateToSearchTab() {
  //   clickAndWaitFor(
  //     this.page,
  //     "a[data-rb-event-key='list']",
  //     "input[id='input-pid']"
  //   );
  // }

  // async selectPropertyByPID(pid) {
  //   await this.page.locator("#input-searchBy").selectOption({ label: "PID" });
  //   await this.page.locator("#input-pid").fill("");
  //   await this.page.locator("#input-pid").fill(pid);
  //   await this.page.getByTestId("search").click();
  // }

  // async selectPropertyByPIN(pin) {
  //   await this.page.locator("#input-searchBy").selectOption({ label: "PIN" });
  //   await this.page.locator("#input-pin").fill("");
  //   await this.page.locator("#input-pin").fill(pin);
  //   await this.page.getByTestId("search").click();
  // }

  // async selectPropertyByAddress(address) {
  //   await this.page
  //     .locator("#input-searchBy")
  //     .selectOption({ label: "Address" });
  //   await this.page.locator("#input-address").fill("");
  //   await this.page.locator("#input-address").fill(address);
  //   await this.page.getByTestId("search").click();
  // }

  // async selectPropertyByPlan(plan) {
  //   await this.page
  //     .locator("#input-searchBy")
  //     .selectOption({ label: "Plan #" });
  //   await this.page.locator("#input-planNumber").fill("");
  //   await this.page.locator("#input-planNumber").fill(plan);
  //   await this.page.getByTestId("search").click();
  // }

  // async selectPropertyByLegalDescription(legalDescription) {
  //   await this.page
  //     .locator("#input-searchBy")
  //     .selectOption({ label: "Legal Description" });
  //   await this.page.locator("#input-legalDescription").fill("");
  //   await this.page.locator("#input-legalDescription").fill(legalDescription);
  //   await this.page.getByTestId("search").click();
  // }

  // async selectPropertyByLongLant(coordinates) {
  //   await this.page
  //     .locator("#input-searchBy")
  //     .selectOption({ label: "Lat/Long" });

  //   await this.page
  //     .locator("#number-input-coordinates\\.latitude\\.degrees")
  //     .fill(`${coordinates.LatitudeDegree}`);
  //   await this.page
  //     .locator("#number-input-coordinates\\.latitude\\.minutes")
  //     .fill(`${coordinates.LatitudeMinutes}`);
  //   await this.page
  //     .locator("#number-input-coordinates\\.latitude\\.seconds")
  //     .fill(`${coordinates.LatitudeSeconds}`);
  //   await this.page
  //     .locator("#input-coordinates\\.latitude\\.direction")
  //     .selectOption({ label: coordinates.LatitudeDirection });

  //   await this.page
  //     .locator("#number-input-coordinates\\.longitude\\.degrees")
  //     .fill(`${coordinates.LongitudeDegree}`);
  //   await this.page
  //     .locator("#number-input-coordinates\\.longitude\\.minutes")
  //     .fill(`${coordinates.LongitudeMinutes}`);
  //   await this.page
  //     .locator("#number-input-coordinates\\.longitude\\.seconds")
  //     .fill(`${coordinates.LongitudeSeconds}`);
  //   await this.page
  //     .locator("#input-coordinates\\.longitude\\.direction")
  //     .selectOption({ label: coordinates.LongitudeDirection });

  //   await this.page.getByTestId("search").click();
  // }

  async addNameSelectedProperty(name, index) {
    await this.page
      .locator("#input-properties." + index + ".name']")
      .fill(name);
  }

  // async selectFirstOptionFromSearch() {
  //   const firstResultChoice = await this.page.locator(
  //     "div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:first-child div input"
  //   );
  //   await firstResultChoice.waitFor({ state: "visible" });
  //   await firstResultChoice.check();

  //   const addPropertyBttn = await this.page.getByTestId(
  //     "add-selected-properties-button"
  //   );
  //   await addPropertyBttn.waitFor({ state: "visible" });
  //   await addPropertyBttn.click();

  //   while (
  //     (await this.page.locator("div[class='modal-content']").count()) > 0
  //   ) {
  //     if (
  //       await this.page
  //         .locator("div[class='modal-content']")
  //         .textContent()
  //         .includes(
  //           "This property has already been added to one or more acquisition files."
  //         )
  //     ) {
  //       await expect(
  //         this.page.locator(
  //           "div[class='modal-header'] div[class='modal-title h4']"
  //         )
  //       ).toBe("User Override Required");
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent(
  //         "This property has already been added to one or more acquisition files."
  //       );
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent("Do you want to acknowledge and proceed?");
  //     }
  //     if (
  //       await this.page
  //         .locator("div[class='modal-content']")
  //         .textContent()
  //         .includes(
  //           "This property has already been added to one or more research files."
  //         )
  //     ) {
  //       expect(
  //         this.page.locator(
  //           "div[class='modal-header'] div[class='modal-title h4']"
  //         )
  //       ).toBe("User Override Required");
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent(
  //         "This property has already been added to one or more research files."
  //       );
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent("Do you want to acknowledge and proceed?");
  //     }
  //     if (
  //       await this.page
  //         .locator("div[class='modal-content']")
  //         .textContent()
  //         .includes(
  //           "This property has already been added to one or more disposition files."
  //         )
  //     ) {
  //       expect(
  //         this.page.locator(
  //           "div[class='modal-header'] div[class='modal-title h4']"
  //         )
  //       ).toBe("User Override Required");
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent(
  //         "This property has already been added to one or more disposition files."
  //       );
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent("Do you want to acknowledge and proceed?");
  //     }
  //     if (
  //       await this.page
  //         .locator("div[class='modal-content']")
  //         .textContent()
  //         .includes(
  //           "This property has already been added to one or more files."
  //         )
  //     ) {
  //       expect(
  //         this.page.locator(
  //           "div[class='modal-header'] div[class='modal-title h4']"
  //         )
  //       ).toBe("User Override Required");
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent(
  //         "This property has already been added to one or more files."
  //       );
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent("Do you want to acknowledge and proceed?");
  //     }
  //     if (
  //       await sharedModals
  //         .ModalContent()
  //         .Contains(
  //           "You have selected a property not previously in the inventory."
  //         )
  //     ) {
  //       expect(
  //         this.page.locator(
  //           "div[class='modal-header'] div[class='modal-title h4']"
  //         )
  //       ).toBe("Not inventory property");
  //       await expect(
  //         this.page.locator("div[class='modal-body']")
  //       ).toHaveTextContent(
  //         "You have selected a property not previously in the inventory. Do you want to add this property to the lease?"
  //       );
  //     }

  //     await this.page.getByTestId("ok-modal-button").click();
  //   }
  // }

  async selectFirstPropertyOptionFromFile() {
    await this.page
      .locator("div[data-testid='menu-item-row-1'] div:nth-child(3)")
      .click();
  }

  async noRowsResultsMessageFromSearch() {
    return await this.page
      .locator("div[data-testid='map-properties'] div[class='no-rows-message']")
      .textContent();
  }

  async verifyPropertiesToIncludeInFileInitForm() {
    //Title and instructions
    await this.page
      .locator('h2 div:has-text("Properties to include in this file")')
      .first()
      .isVisible();
    await this.page
      .locator(
        'p:has-text("Select one or more properties that you want to include in this disposition. You can choose a location from the map, or search by other criteria.")'
      )
      .isVisible();

    //Selected Properties section
    await this.page.getByText("Selected properties").isVisible();
    await this.page
      .getByTestId("tooltip-icon-property-selector-tooltip")
      .isVisible();
    await this.page.getByText("No Properties selected").isVisible();
  }

  // async verifySearchTabPropertiesFeature() {
  //   const searchPropsTab = await this.page.locator(
  //     "//a[contains(text(),'Search')]"
  //   );
  //   expect(searchPropsTab).toBeVisible();

  //   const propertySubtitle = await this.page
  //     .locator("div[data-testid='property-search-selector-section'] h2 div div")
  //     .textContent();
  //   expect(propertySubtitle).toEqual("Search for a property");

  //   const searchBySelect = await this.page.locator("#input-searchBy");
  //   expect(searchBySelect).toBeVisible();

  //   const pidInput = await this.page.locator("#input-pid");
  //   expect(pidInput).toBeVisible();

  //   const searchButton = await this.page.locator("#search-button");
  //   expect(searchButton).toBeVisible();

  //   const resetButton = await this.page.locator("#reset-button");
  //   expect(resetButton).toBeVisible();

  //   const tableHeader = await this.page.locator(
  //     "div[class='thead thead-light']"
  //   );
  //   expect(tableHeader).toBeVisible();

  //   await expect(
  //     this.page.locator("input[data-testid='selectrow-parent']")
  //   ).toBeVisible();
  //   await expect(
  //     this.page.locator("//div[@class='th']/div[contains(text(), 'PID')]")
  //   ).toBeVisible();
  //   await expect(
  //     this.page.locator("//div[@class='th']/div[contains(text(), 'PIN')]")
  //   ).toBeVisible();
  //   await expect(
  //     this.page.locator("//div[@class='th']/div[contains(text(), 'Plan #')]")
  //   ).toBeVisible();
  //   await expect(
  //     this.page.locator("//div[@class='th']/div[contains(text(), 'Address')]")
  //   ).toBeVisible();
  // }

  async selectNthPropertyOptionFromFile(index) {
    const elementOrder = index++;
    await this.page
      .locator(
        "div[data-testid='menu-item-row-" + elementOrder + "'] div:nth-child(3)"
      )
      .click();
  }

  async deleteLastPropertyFromFile() {
    const propertyIndex =
      (await this.page
        .locator("div[class='align-items-center mb-3 no-gutters row']")
        .count()) - 1;
    await this.page.getByTestId("delete-property-" + propertyIndex).click();

    if ((await this.page.locator("div[class='modal-content']").count()) > 0) {
      await expect(
        this.page.locator("div[class='modal-title h4']")
      ).toHaveTextContent("Removing Property from Lease/Licence");
      await expect(
        this.page.locator("div[class='modal-body']")
      ).toHaveTextContent(
        "Are you sure you want to remove this property from this lease/licence?"
      );
      await this.page.getByTestId("ok-modal-button").click();
    }

    const propertiesAfterRemove = await this.page
      .locator("div[class='align-items-center mb-3 no-gutters row']")
      .count();
    expect(propertiesAfterRemove).toBe(propertyIndex - 1);
  }

  async saveFileProperties() {
    clickSaveButton(this.page, "button[title='Edit management file']");

    const headerContent = await this.page
      .locator("div[class='modal-header'] div[class='modal-title h4']")
      .textContent();
    expect(headerContent).toEqual("Confirm changes");

    const modalContent1 = await this.page
      .locator("div[class='modal-body'] div")
      .textContent();
    expect(modalContent1).toEqual(
      "You have made changes to the properties in this file."
    );

    const modalContent2 = await this.page
      .locator("div[class='modal-body'] strong")
      .textContent();
    expect(modalContent2).toEqual("Do you want to save these changes?");

    await this.page.getByTestId("ok-modal-button").click();

    while (
      await this.page
        .locator("div.modal-content")
        .isVisible()
        .catch(() => false)
    ) {
      const modalHeader = this.page
        .locator("div.modal-header .modal-title.h4")
        .last();
      const modalBody = this.page.locator("div.modal-body").last();

      const modalText = (await modalBody.textContent()) ?? "";

      if (
        modalText.includes(
          "You have added one or more properties to the disposition file"
        )
      ) {
        await expect(modalHeader).toContainText("User Override Required");
        await expect(modalBody).toContainText(
          "You have added one or more properties to the disposition file that are not in the MOTI Inventory. Do you want to proceed?"
        );
        await this.sharedModal.mainModalClickOKBttn();
      } else if (
        modalText.includes(
          "You have added one or more properties to the management file"
        )
      ) {
        await expect(modalHeader).toContainText("User Override Required");
        await expect(modalBody).toContainText(
          "You have added one or more properties to the management file that are not in the MOTI Inventory. To acquire these properties, add them to an acquisition file. Do you want to proceed?"
        );
        await this.sharedModal.mainModalClickOKBttn();
      } else {
        await expect(modalHeader).toContainText("User Override Required");
        await expect(modalBody).toContainText(
          "The selected property already exists in the system's inventory. However, the record is missing spatial details."
        );
        await expect(modalBody).toContainText(
          "To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection."
        );
        await this.sharedModal.mainModalClickOKBttn();
      }
    }

    await this.page.waitForTimeout(1000);
  }
}

module.exports = SharedFileProperties;
