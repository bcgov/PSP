class LoginPage {
  constructor(page) {
    this.page = page;
    this.usernameInput = 'input[name="user"]';
    this.passwordInput = 'input[name="password"]';
    this.loginButton = 'input[name="btnSubmit"]';
  }

  async navigate(baseUrl) {
    await this.page.goto(`${baseUrl}`);
  }

  async login(username, password) {
    await this.page.getByRole('button', { name: 'Sign In' }).click();
    await this.page.fill(this.usernameInput, username);
    await this.page.fill(this.passwordInput, password);
    await this.page.locator(this.loginButton).click();
  }
}

module.exports = { LoginPage };
