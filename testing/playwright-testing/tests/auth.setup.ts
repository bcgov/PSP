// tests/auth.setup.ts
import { test as setup } from "@playwright/test";
import path from "path";
import { LoginPage } from "../pages/login.page"; // Import your POM

// Define where the session JSON will be stored
const authFile = path.join(__dirname, "../.auth/user.json");

setup("authenticate", async ({ page }) => {
  // Pulling variables directly from the environment
  const username = process.env.USER_TRANPSP1_ID;
  const password = process.env.USER_TRANPSP1_PASSWORD;

  // Defensive check: Crash early if someone forgot to set up their .env file
  if (!username || !password) {
    throw new Error(
      "Missing USER_TRANPSP1_ID or USER_TRANPSP1_PASSWORD in environment variables!"
    );
  }

  // 1. Inject the page fixture into your Login Page Object
  const loginPage = new LoginPage(page);
  await loginPage.goto();
  await loginPage.getStarted();

  // Wait for SiteMinder
  await page.waitForURL(/.*gov\.bc\.ca\/clp-cgi\/int\/logon\.cgi\?.*/);

  // 2. Fill in the login form
  await page.locator("input#user").fill(username);
  await page.locator("input#password").fill(password);

  await page.getByRole("button", { name: "Continue" }).click();

  // 3. CRITICAL: Wait for the app to finish redirecting & setting cookies
  await page.waitForURL(`${process.env.BASE_URL}/mapview`);

  // 4. Save the storage state (cookies, localStorage) to the file
  await page.context().storageState({ path: authFile });
});
