import { defaultTenant, ITenantConfig } from '..';

/**
 * Tenant configuration settings for CITZ.
 */
export const config: ITenantConfig = {
  ...defaultTenant,
  ...{
    id: 'CITZ',
    title: 'Property Inventory Management System',
    logo: {
      favicon: '/tenants/CITZ/favicon.ico',
      image: '/tenants/CITZ/logo_only.png',
      imageWithText: '/tenants/CITZ/logo_with_text.png',
    },
    login: {
      title: 'Search and visualize government property information',
      heading: 'PIMS enables you to search properties owned by the Government of British Columbia',
      body:
        'The data provided can assist your organization in making informed, timely, and strategic decisions on the optimal use of real property assets on behalf of the people and priorities of British Columbia.',
      backgroundImage: '/tenants/CITZ/background-image.png',
    },
    layers: [],
  },
};
