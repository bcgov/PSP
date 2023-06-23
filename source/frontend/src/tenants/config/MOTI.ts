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
      title: 'TRAN Property Information Management System (PIMS)',
      heading:
        'PIMS enables you to view highways and properties owned by the Ministry of Transportation and Infrastructure',
      body: 'WARNING: Not all data included within has been vetted for accuracy and completeness. Please use caution when proceeding and confirm data before relying on it.',
      backgroundImage: '/tenants/MOTI/background-image.jpg',
    },
    layers: [],
  },
};
