const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class MapLayers {
    constructor(page) {
        this.page = page;
    }

    async navigateMapLayers() {
        await this.page.locator("#layersControlButton").click();
    }


    async verifyMapLayersForm() {
        await expect(this.page.locator("//div[contains(text(),'PIMS')]")).toBeVisible();
        await expect(this.page.locator("label[for='research']")).toBeVisible();
        await expect(this.page.locator("#research")).toBeVisible();
        await expect(this.page.locator("label[for='acquisition']")).toBeVisible();
        await expect(this.page.locator("#acquisition")).toBeVisible();
        await expect(this.page.locator("label[for='management']")).toBeVisible();
        await expect(this.page.locator("#management")).toBeVisible();
        await expect(this.page.locator("label[for='disposition']")).toBeVisible();
        await expect(this.page.locator("#disposition")).toBeVisible();
        await expect(this.page.locator("label[for='LeaseLayers']")).toBeVisible();
        await expect(this.page.locator("#LeaseLayers")).toBeVisible();
        await expect(this.page.locator("label[for='InterestLayers']")).toBeVisible();
        await expect(this.page.locator("#InterestLayers")).toBeVisible();

        await expect(this.page.locator("//div[contains(text(),'External')]")).toBeVisible();
        await expect(this.page.locator("label[for='administrative_group']")).toBeVisible();
        await expect(this.page.locator("#administrative_group")).toBeVisible();
        await expect(this.page.locator("label[for='legalHighwayResearch']")).toBeVisible();
        await expect(this.page.locator("#legalHighwayResearch")).toBeVisible();
        await expect(this.page.locator("label[for='firstNations']")).toBeVisible();
        await expect(this.page.locator("#firstNations")).toBeVisible();
        await expect(this.page.locator("label[for='landOwnership']")).toBeVisible();
        await expect(this.page.locator("#landOwnership")).toBeVisible();
        await expect(this.page.locator("label[for='zoning']")).toBeVisible();
        await expect(this.page.locator("#zoning")).toBeVisible();
        await expect(this.page.locator("label[for='electoral']")).toBeVisible();
        await expect(this.page.locator("#electoral")).toBeVisible();
        await expect(this.page.locator("label[for='federal_bc_parks']")).toBeVisible();
        await expect(this.page.locator("#federal_bc_parks")).toBeVisible();
    }
}

module.exports = MapLayers;