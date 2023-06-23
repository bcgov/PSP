import { ILayerItem } from '@/components/maps/leaflet/Control/LayersControl/types';

/**
 * API Tenant configuration.
 */
export interface ITenantConfig {
  // Unique code to identify the tenant.
  code: string;
  // The name of the tenant.
  name: string;
  // The tenant description.
  description: string;
  // Tenant configuration settings.
  settings: {
    // The email to use for the help desk.
    helpDeskEmail: string;
  };
}

/**
 * Interface for tenant configuration settings.
 */
export interface ITenantConfig2 {
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
  propertiesUrl: string;
  // configuration pertaining the Fully Attributed Parcel Map layer
  parcelMapFullyAttributed: ILayerConfig;
  electoralLayerUrl: string;
  municipalLayerUrl: string;
  parcelsLayerUrl: string;
  regionalLayerUrl: string;
  motiRegionLayerUrl: string;
  hwyDistrictLayerUrl: string;
  alrLayerUrl: string;
  reservesLayerUrl: string;
  boundaryLayerUrl: string;
  bcAssessment: IBcAssessmentLayerConfig;
  // the amount of time it takes to time out the idle prompt (in minutes)
  idlePromptTimeout: number;
  // the amount of time it takes to display the idle prompt (in minutes)
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

export interface ILayerConfig {
  url: string;
  name: string;
}

export interface IBcAssessmentLayerConfig {
  url: string;
  names: { [key: string]: string };
}

export default ITenantConfig;
