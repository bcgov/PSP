const { expect } = require("@playwright/test");

function getUserCredential(envKey) {
  return process.env[envKey];
}

function clickSaveButton(page) {
  page.getByTestId("save-button").click();
}

async function cancelAction(page) {
  page.getByTestId("cancel-button").click();

  if ((await page.locator("div[class='modal-content']").count()) > 0) {
    const titleLocator = page.locator("div.modal-title.h4").last();
    await expect(titleLocator).toHaveText("Confirm Changes");

    const content = await page
      .locator("div[class='modal-body']")
      .last()
      .textContent();

    expect(content).toContain(
      "If you choose to cancel now, your changes will not be saved."
    );
    expect(content).toContain("Do you want to proceed?");

    await page.getByTestId("ok-modal-button").click();
  }
}

function getViewFieldListContent(page, element) {
  let result = [];
  let parentElement = page.locator(element);
  let childrenElements = parentElement.page.getByRole("span");
  childrenElements.forEach((childElement) => {
    result.push(childElement.textContent());
  });
  result.sort();
  return result;
}

function transformDateFormat(date) {
  if (date == "") {
    return "";
  } else {
    const dateObject = new Date(dateString);
    return dateObject.toLocaleDateString("en-US", {
      month: "short", // "MMM"
      day: "numeric", // "d"
      year: "numeric", // "yyyy"
    });
  }
}

function transformCurrencyFormat(amount) {
  if (amount == "") {
    return "$0.00";
  } else {
    const value = parseFloat(amount);
    return (
      "$" +
      value.toLocaleString("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      })
    );
  }
}

function splitStringToArray(listToString) {
  const result = listToString
    .split(";")
    .map((item) => item.trim())
    .filter((item) => item.length > 0);

  return result;
}

async function clickAndWaitFor(
  page,
  clickSelector,
  waitSelector,
  options = {}
) {
  const {
    maxRetries = 3,
    clickDelay = 2000, // ms delay before retry click
    timeout = 5000, // time to wait for expected element before retry
  } = options;

  let lastError;

  for (let attempt = 1; attempt <= maxRetries; attempt++) {
    try {
      await page.click(clickSelector, { force: true });

      // Wait for the target element to appear
      await page.waitForSelector(waitSelector, { timeout });
      return; // Success — exit function
    } catch (err) {
      lastError = err;

      if (attempt < maxRetries) {
        console.warn(
          `Attempt ${attempt} failed. Retrying click on "${clickSelector}"...`
        );
        await page.waitForTimeout(clickDelay);
      }
    }
  }

  throw new Error(
    `Failed to click "${clickSelector}" and wait for "${waitSelector}" after ${maxRetries} attempts. Last error: ${lastError}`
  );
}

async function fillTypeahead(page, selector, text, optionSelector) {
  const input = page.locator(selector);

  // Clear and type slowly to mimic human typing
  await input.fill("");
  for (const char of text) {
    await input.type(char, { delay: 50 });
  }

  // Wait for dropdown to appear
  const menuLocator = page.locator(optionSelector);
  await menuLocator.waitFor({ state: "visible" });

  // Find the matching option
  const option = menuLocator
    .locator(`a[role='option']`, { hasText: text })
    .first();
  await option.waitFor({ state: "visible" });

  // Click the option
  await option.click({ force: true });
}

module.exports = {
  getUserCredential,
  clickSaveButton,
  clickCancelButton: cancelAction,
  getViewFieldListContent,
  transformDateFormat,
  transformCurrencyFormat,
  splitStringToArray,
  clickAndWaitFor,
  fillTypeahead,
};
