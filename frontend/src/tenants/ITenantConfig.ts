/**
 * Interface for tenant configuration settings.
 */
export interface ITenantConfig {
  // The unique identifier for the tenant.
  id: string;
  // The name of the application to display in the header.
  title: string;
  // The shortname of the application.
  shortName: string;
  // The colour to identify the environment.
  colour: string;
  // The logos to display.
  logo: ITenantLogoConfig;
  // Login page settings.
  login: ITenantLoginConfig;
}

export interface ITenantLoginConfig {
  // Title of the login page.
  title: string;
  // Heading to display after the title.
  heading: string;
  // Body of the login page.
  body: string;
}

export interface ITenantLogoConfig {
  // Path to favicon.
  favicon: string;
  // Path to image with no text.
  image: string;
  // Path to image with text.
  imageWithText: string;
}
