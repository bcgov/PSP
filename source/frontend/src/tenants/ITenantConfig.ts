import { ILayerItem } from 'components/maps/leaflet/LayersControl/types';

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
  // optional additional layers to add using config.
  layers: ILayerItem[];
  // the url that should be used to query the PSP properties layer.
  propertiesUrl?: string;
  // configuration pertaining the Fully Attributed Parcel Map layer
  parcelMapFullyAttributed: IFullyAttributedParcelLayerConfig;
  bcAssessment: IBcAssessmentLayerConfig;
  // the amount of time it takes to time out the idle prompt (in seconds)
  idlePromptTimeout: number;
  // the amount of time it takes to display the idle prompt (in seconds)
  idleTimeout: number;
}

export interface ITenantLoginConfig {
  // Title of the login page.
  title: string;
  // Heading to display after the title.
  heading: string;
  // Body of the login page.
  body: string;
  // Path to the background image for the login page.
  backgroundImage?: string;
}

export interface ITenantLogoConfig {
  // Path to favicon.
  favicon: string;
  // Path to image with no text.
  image: string;
  // Path to image with text.
  imageWithText: string;
}

export interface IFullyAttributedParcelLayerConfig {
  url: string;
  name: string;
}

export interface IBcAssessmentLayerConfig {
  url: string;
  names: { [key: string]: string };
}
