import { MOTIConfig, CITZConfig } from '.';
import { ITenantConfig } from './ITenantConfig';

/**
 * Default tenant configuration.
 */
export const defaultTenant = {
  title: 'Default Tenant Name',
  shortName: 'PIMS',
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

// This ensures the tenant information is set once.
export let tenant: ITenantConfig = defaultTenant;

/**
 * Provides the configuration settings for the specified tenant value in the environment variables.
 * Environment variable "REACT_APP_TENANT".
 * Call this function at the beginning of the application to set the global variable.
 * @returns {ITenantConfig} Tenant configuration settings.
 */
export const setTenant = (): ITenantConfig => {
  switch (process.env.REACT_APP_TENANT) {
    case 'MOTI':
      tenant = MOTIConfig;
      break;
    case 'CITZ':
      tenant = CITZConfig;
      break;
  }
  return tenant;
};

export default tenant;
