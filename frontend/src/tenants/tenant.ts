import { MOTIConfig, CITZConfig } from '.';
import { ITenantConfig } from './ITenantConfig';

/**
 * Default tenant configuration.
 */
export const defaultTenant = {
  title: 'Default Tenant Name',
  logo: {
    favicon: '',
    image: '',
    imageWithText: '',
  },
  login: {
    title: '',
    heading: '',
    body: '',
  },
};

/**
 * Provides the configuration settings for the specified tenant value in the environment variables.
 * Environment variable "REACT_APP_TENANT".
 * @returns Tenant configuration settings.
 */
export const tenant = (): ITenantConfig => {
  switch (process.env.REACT_APP_TENANT) {
    case 'MOTI':
      return MOTIConfig;
    case 'CITZ':
      return CITZConfig;
    default:
      return defaultTenant;
  }
};

export default tenant;
