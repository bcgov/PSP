import { ITenantConfig } from 'tenants/ITenantConfig';

/**
 * Default tenant configuration.
 */
export const defaultTenant: ITenantConfig = {
  id: 'DFLT',
  title: 'Default Tenant Name',
  shortName: 'PIMS',
  colour: '#003366',
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
