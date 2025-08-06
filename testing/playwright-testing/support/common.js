const BASE_URL = process.env.BASE_URL;

function getUserCredential(envKey) {
  return process.env[envKey];
}

function clickSaveButton(page) {
  page.getByTestId("save-button").click();
}

function clickCancelButton(page) {
  page.getByTestId("cancel-button").click();
}

module.exports = {
  BASE_URL,
  getUserCredential,
  clickSaveButton,
  clickCancelButton,
};
