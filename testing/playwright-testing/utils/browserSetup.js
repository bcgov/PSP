const { chromium } = require("playwright");
const fs = require("fs");

const USER_DATA_DIR = "./tmp-profile"; // or path.resolve(__dirname, 'tmp-profile')

// Optional: clean it on each run if needed
if (fs.existsSync(USER_DATA_DIR)) {
  fs.rmSync(USER_DATA_DIR, { recursive: true, force: true });
}

async function launchBrowser() {
  const context = await chromium.launchPersistentContext(USER_DATA_DIR, {
    headless: false,
    args: [
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
      "--window-size=1920,1080",
      "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/91.0 Safari/537.36",
    ],
  });

  const page = await context.newPage();
  return { browser: context.browser(), context, page };
}

module.exports = { launchBrowser };
