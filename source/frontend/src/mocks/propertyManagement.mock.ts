import { Api_PropertyManagement, Api_PropertyManagementPurpose } from '@/models/api/Property';

export const getMockApiPropertyManagement = (id = 123459): Api_PropertyManagement => ({
  id,
  rowVersion: 1,
  managementPurposes: [],
  additionalDetails: 'test',
  isTaxesPayable: null,
  isUtilitiesPayable: null,
  relatedLeases: 0,
  leaseExpiryDate: null,
});

export const getMockApiPropertyManagementPurpose = (id = 1): Api_PropertyManagementPurpose => ({
  id,
  rowVersion: 1,
  propertyId: 123459,
  propertyPurposeTypeCode: {
    id: 'BCFERRIES',
    description: 'BC Ferries',
  },
});
