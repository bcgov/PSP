import { ITenantConfig } from '../ITenantConfig';
import { defaultTenant } from '..';

export const Config: ITenantConfig = {
  ...defaultTenant,
  ...{
    title: 'Property Inventory Management System',
    shortName: 'PIMS',
    logo: {
      favicon: '/tenants/CITZ/favicon-16x16.ico',
      image: '/tenants/CITZ/logo_only.png',
      imageWithText: '/tenants/CITZ/logo_with_text.png',
    },
    login: {
      title: 'Search and visualize government property information',
      heading: 'PIMS enables you to search properties owned by the Government of British Columbia',
      body:
        'The data provided can assist your agency in making informed, timely, and strategic decisions on the optimal use of real property assets on behalf of the people and priorities of British Columbia.',
    },
  },
};
