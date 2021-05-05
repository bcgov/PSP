/**
 * Interface for tenant configuration settings.
 */
export interface ITenantConfig {
  // The name of the application to display in the header.
  title: string;
  logo: ITenantLogoConfig;
  login: ITenantLoginConfig;
}

export interface ITenantLoginConfig {
  title: string;
  heading: string;
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
