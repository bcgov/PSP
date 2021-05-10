import { ITenantConfig } from 'tenants/ITenantConfig';

/**
 * Default tenant configuration.
 */
export const defaultTenant: ITenantConfig = {
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

export default defaultTenant;
