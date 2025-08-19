import * as dotenv from "dotenv";
import { setWorldConstructor } from "@cucumber/cucumber";
import { launchBrowser } from "../utils/browserSetup.js";

// Import your page objects
import LoginPage from "./pages/LoginPage.js";
import ManagementFileDetails from "./pages/ManagementFileDetails.js";
import Notes from "./pages/Notes.js";
import SearchManagementFiles from "./pages/SearchManagementFiles.js";
import SharedActivities from "./pages/SharedActivities.js";
import SharedFileProperties from "./pages/SharedFileProperties.js";
import SharedPagination from "./pages/SharedPagination.js";

// Load environment variables from .env
dotenv.config();

class CustomWorld {
  constructor() {
    global.baseURL = process.env.BASE_URL;
    this.browser = null;
    this.context = null;
    this.page = null;

    // Page objects will be set after browser starts
    this.loginPage = null;
    this.managementFileDetails = null;
    this.notes = null;
    this.searchManagementFiles = null;
    this.sharedActivities = null;
    this.sharedFileProperties = null;
    this.sharedPagination = null;
  }

  async openBrowser() {
    const { browser, context, page } = await launchBrowser();
    this.browser = browser;
    this.context = context;
    this.page = page;

    // Initialize page objects here
    this.loginPage = new LoginPage(this.page);
    this.managementFileDetails = new ManagementFileDetails(this.page);
    this.notes = new Notes(this.page);
    this.searchManagementFiles = new SearchManagementFiles(this.page);
    this.sharedActivities = new SharedActivities(this.page);
    this.sharedFileProperties = new SharedFileProperties(this.page);
    this.sharedPagination = new SharedPagination(this.page);

    this.page.setDefaultTimeout(20_000);
    this.page.setDefaultNavigationTimeout(45_000);
  }

  async closeBrowser() {
    //await this.browser?.close();
  }
}

setWorldConstructor(CustomWorld);
