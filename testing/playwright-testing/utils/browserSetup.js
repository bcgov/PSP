import { chromium } from "playwright";

const BROWSER_ARGS = [
  "--no-first-run",
  "--no-default-browser-check",
  "--disable-default-apps",
  "--disable-popup-blocking",
  "--disable-component-extensions-with-background-pages",
  "--disable-extensions",
  "--disable-background-networking",
  "--disable-sync",
  "--disable-translate",
  "--disable-features=DefaultBrowserPrompt,TranslateUI",
];

/**
 * Launches a new Browser instance.
 * @returns {Promise<import('playwright').Browser>}
 */
async function launchBrowser() {
  const browser = await chromium.launch({
    headless: false,
    args: BROWSER_ARGS,
  });
  return browser;
}

/**
 * Creates a new, isolated BrowserContext.
 * @param {import('playwright').Browser} browser - The launched browser instance.
 * @returns {Promise<import('playwright').BrowserContext>}
 */
async function createNewContext(browser) {
  const context = await browser.newContext({
    viewport: { width: 1550, height: 900 },
    userAgent:
      "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/91.0 Safari/537.36",
  });
  return context;
}

export { launchBrowser, createNewContext };
