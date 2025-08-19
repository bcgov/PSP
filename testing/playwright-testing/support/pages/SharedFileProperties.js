const { expect } = require("@playwright/test");

class SharedFileProperties {
  constructor(page) {
    this.page = page;
  }

  async navigateToAddPropertiesToFile() {
    await this.page.locator("button[title='Change properties']").click();
  }

  async navigateToSearchTab() {
    await this.page.locator("a[data-rb-event-key='list']").click();
  }

  async selectPropertyByPID(pid) {
    await this.page.locator("#input-searchBy").selectOption({ label: "PID" });
    await this.page.locator("#input-pid").fill("");
    await this.page.locator("#input-pid").fill(pid.toString());
    await this.page.getByTestId("search").click();
  }

  async selectPropertyByPIN(pin) {
    await this.page.locator("#input-searchBy").selectOption({ label: "PIN" });
    await this.page.locator("#input-pin").fill("");
    await this.page.locator("#input-pin").fill(pin.toString());
    await this.page.getByTestId("search").click();
  }

  async selectPropertyByAddress(address) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Address" });
    await this.page.locator("#input-address").fill("");
    await this.page.locator("#input-address").fill(address);
    await this.page.getByTestId("search").click();
  }

  async selectPropertyByPlan(plan) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Plan #" });
    await this.page.locator("#input-planNumber").fill("");
    await this.page.locator("#input-planNumber").fill(plan);
    await this.page.getByTestId("search").click();
  }

  async selectPropertyByLegalDescription(legalDescription) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Legal Description" });
    await this.page.locator("#input-legalDescription").fill("");
    await this.page.locator("#input-legalDescription").fill(legalDescription);
    await this.page.getByTestId("search").click();
  }

  async selectPropertyByLongLant(coordinates) {
    await this.page
      .locator("#input-searchBy")
      .selectOption({ label: "Lat/Long" });

    await this.page
      .locator("#number-input-coordinates.latitude.degrees")
      .fill("");
    await this.page
      .locator("#number-input-coordinates.latitude.minutes")
      .fill("");
    await this.page.locator("#coordinates.latitude.seconds").fill("");
    await this.page
      .locator("#number-input-coordinates.longitude.degrees")
      .fill("");
    await this.page
      .locator("#number-input-coordinates.longitude.minutes")
      .fill("");
    await this.page
      .locator("#number-input-coordinates.longitude.seconds")
      .fill("");

    await this.page
      .locator("#number-input-coordinates.latitude.degrees")
      .fill(coordinates.LatitudeDegree);
    await this.page
      .locator("#number-input-coordinates.latitude.minutes")
      .fill(coordinates.LatitudeMinutes);
    await this.page
      .locator("#number-input-coordinates.latitude.seconds")
      .fill(coordinates.LatitudeSeconds);
    await this.page
      .locator("#select-coordinates.latitude.direction")
      .selectOption({ label: coordinates.LatitudeDirection });

    await this.page
      .locator("#number-input-coordinates.longitude.degrees")
      .fill(coordinates.LongitudeDegree);
    await this.page
      .locator("#number-input-coordinates.longitude.minutes")
      .fill(coordinates.LongitudeMinutes);
    await this.page
      .locator("#number-input-coordinates.longitude.seconds")
      .fill(coordinates.LongitudeSeconds);
    await this.page
      .locator("#select-coordinates.longitude.direction")
      .selectOption({ label: coordinates.LongitudeDirection });

    await this.page.getByTestId("search").click();
  }

  async addNameSelectedProperty(name, index) {
    await this.page
      .locator("#input-properties." + index + ".name']")
      .fill(name);
  }

  async selectFirstOptionFromSearch() {
    await this.page
      .locator(
        "div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .first()
      .check();

    await this.page.getByTestId("add-selected-properties-button").click();

    while (
      (await this.page.locator("div[class='modal-content']").count()) > 0
    ) {
      if (
        await this.page
          .locator("div[class='modal-content']")
          .textContent()
          .includes(
            "This property has already been added to one or more acquisition files."
          )
      ) {
        await expect(
          this.page.locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          )
        ).toBe("User Override Required");
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent(
          "This property has already been added to one or more acquisition files."
        );
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent("Do you want to acknowledge and proceed?");
      }
      if (
        await this.page
          .locator("div[class='modal-content']")
          .textContent()
          .includes(
            "This property has already been added to one or more research files."
          )
      ) {
        await expect(
          this.page.locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          )
        ).toBe("User Override Required");
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent(
          "This property has already been added to one or more research files."
        );
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent("Do you want to acknowledge and proceed?");
      }
      if (
        await this.page
          .locator("div[class='modal-content']")
          .textContent()
          .includes(
            "This property has already been added to one or more disposition files."
          )
      ) {
        await expect(
          this.page.locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          )
        ).toBe("User Override Required");
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent(
          "This property has already been added to one or more disposition files."
        );
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent("Do you want to acknowledge and proceed?");
      }
      if (
        await this.page
          .locator("div[class='modal-content']")
          .textContent()
          .includes(
            "This property has already been added to one or more files."
          )
      ) {
        await expect(
          this.page.locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          )
        ).toBe("User Override Required");
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent(
          "This property has already been added to one or more files."
        );
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent("Do you want to acknowledge and proceed?");
      }
      if (
        await sharedModals
          .ModalContent()
          .Contains(
            "You have selected a property not previously in the inventory."
          )
      ) {
        await expect(
          this.page.locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          )
        ).toBe("Not inventory property");
        await expect(
          this.page.locator("div[class='modal-body']")
        ).toHaveTextContent(
          "You have selected a property not previously in the inventory. Do you want to add this property to the lease?"
        );
      }

      await this.page.getByTestId("ok-modal-button").click();
    }
  }

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

    //Tabs search options
    await this.page.getByRole("tab", { name: "Locate on Map" }).isVisible();
    await this.page.getByRole("tab", { name: "Search" }).isVisible();

    //Select a Property Details
    await this.page.getByRole("label", { hasText: "PID:" }).isVisible();
    await this.page.getByRole("label", { hasText: "PIN:" }).isVisible();
    await this.page.getByRole("label", { hasText: "Plan #:" }).isVisible();
    await this.page.getByRole("label", { hasText: "Address:" }).isVisible();
    await this.page.getByRole("label", { hasText: "Region:" }).isVisible();
    await this.page.getByRole("label", { hasText: "District:" }).isVisible();
    await this.page
      .getByRole("label", { hasText: "Legal Description:" })
      .isVisible();

    //Selected Properties section
    await this.page.getByText("Selected properties").isVisible();
    await this.page
      .getByTestId("tooltip-icon-property-selector-tooltip")
      .isVisible();
    await this.page.getByText("No Properties selected").isVisible();
  }

  async verifySearchTabPropertiesFeature() {
    expect(this.page.locator("a[data-rb-event-key='list']")).toBeVisible();

    // const propertySubtitle = await this.page.locator(
    //     "div[data-testid='property-search-selector-section'] h2 div div"
    //   ).textContent();
    // expect(propertySubtitle).toEqual("Search for a property");

    await expect(this.page.locator("#input-searchBy")).toBeVisible();
    await expect(this.page.locator("#input-pid")).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();
    await expect(
      this.page.locator("div[class='thead thead-light']")
    ).toBeVisible();
    await expect(
      this.page.locator("input[data-testid='selectrow-parent']")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[@class='th']/div[contains(text(), 'PID')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[@class='th']/div[contains(text(), 'PIN')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[@class='th']/div[contains(text(), 'Plan #')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[@class='th']/div[contains(text(), 'Address')]")
    ).toBeVisible();
  }

  async selectNthPropertyOptionFromFile(index) {
    const elementOrder = index++;
    await this.page
      .locator(
        "div[data-testid='menu-item-row-" + elementOrder + "'] div:nth-child(3)"
      )
      .click();
  }

  async resetSearch() {
    await this.page.getByTestId("reset-button").click();
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
    await expect(propertiesAfterRemove).toBe(propertyIndex - 1);
  }

  async saveFileProperties() {
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
      (await this.page.locator("div[class='modal-content']").count()) > 0
    ) {
      if (
        await this.page
          .locator("div[class='modal-body']")
          .last()
          .textContent()
          .includes(
            "You have added one or more properties to the disposition file that are not in the MOTI Inventory"
          )
      ) {
        await expect(
          locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          ).last()
        ).toHabeTextContent("User Override Required");
        await expect(
          locator("div[class='modal-body']").last().textContent()
        ).toHabeTextContent(
          "You have added one or more properties to the disposition file that are not in the MOTI Inventory. Do you want to proceed?"
        );
      } else if (
        await this.page
          .locator("div[class='modal-body']")
          .last()
          .textContent()
          .includes(
            "You have added one or more properties to the management file that are not in the MOTI Inventory"
          )
      ) {
        await expect(
          locator(
            "div[class='modal-header'] div[class='modal-title h4']"
          ).last()
        ).toHabeTextContent("User Override Required");
        await expect(
          locator("div[class='modal-body']").last().textContent()
        ).toHabeTextContent(
          "You have added one or more properties to the management file that are not in the MOTI Inventory. To acquire these properties, add them to an acquisition file. Do you want to proceed?"
        );
      } else {
        await expect(
          this.page
            .locator("div[class='modal-header'] div[class='modal-title h4']")
            .last()
        ).toBe("User Override Required");
        await expect(
          this.page.locator("div[class='modal-body']").last()
        ).toHaveTextContent(
          "The selected property already exists in the system's inventory. However, the record is missing spatial details."
        );
        await expect(
          this.page.locator("div[class='modal-body']").last()
        ).toHaveTextContent(
          "To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection."
        );
      }

      await this.page.getByTestId("ok-modal-button").click();
    }
  }
}

module.exports = SharedFileProperties;
