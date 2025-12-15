const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../common.js");

class MapInteractions {
  constructor(page) {
        this.page = page;
    }

    async scrollZoomIn() {
        // Scroll UP to zoom IN (assuming standard map behavior)
        await this.page.mouse.wheel(0, -500); // Scrolls 500 units up
    }

    async scrollZoomOut() {
        // Scroll DOWN to zoom OUT
        await this.page.mouse.wheel(0, 500); // Scrolls 500 units down
    }

    async countPropMarkers() {
        const propertyMarkers = await this.page.locator("div[class='leaflet-marker-icon leaflet-div-icon leaflet-zoom-animated leaflet-interactive']");
        expect(propertyMarkers).toBeVisible();
        return propertyMarkers.count();
    }
}
module.exports = MapInteractions;