import { ITenantConfig2 } from '@/hooks/pims-api/interfaces/ITenantConfig';

import defaultTenant from './defaultTenant';

/**
 * Tenant configuration settings for MOTI.
 */
export const config: ITenantConfig2 = {
  ...defaultTenant,
  ...{
    id: 'MOTI',
    title: 'Property Information Management System',
    logo: {
      favicon: '/tenants/MOTI/favicon.ico',
      image: '/tenants/MOTI/PIMS-logo.png',
      imageWithText: '/tenants/MOTI/PIMS-logo-with-text.png',
    },
    login: {
      title: 'MOTI Property Information Management System (PIMS)',
      heading:
        'PIMS enables users to track and manage information relating to the property interests of the MOTI and BCTFA.',
      body: 'By signing in you acknowledge that not all data included within has been vetted for completeness and accuracy. Please exercise caution by verifying information prior to relying on it.',
      backgroundImage: '/tenants/MOTI/background-image.jpg',
    },
    layers: [],
  },
};
