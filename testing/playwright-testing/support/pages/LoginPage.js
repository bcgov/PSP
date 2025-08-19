class LoginPage {
  constructor(page) {
    this.page = page;
  }

  async navigate(baseUrl) {
    await this.page.goto(`${baseUrl}`);
  }

  async login(username, password) {
    await this.page.getByRole("button", { name: "Sign In" }).click();
    await this.page.locator('input[name="user"]').fill(username);
    await this.page.locator('input[name="password"]').fill(password);
    await this.page.locator('input[name="btnSubmit"]').click();
  }
}

module.exports = LoginPage;
