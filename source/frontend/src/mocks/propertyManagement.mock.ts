import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { ApiGen_Concepts_PropertyManagementPurpose } from '@/models/api/generated/ApiGen_Concepts_PropertyManagementPurpose';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const getMockApiPropertyManagement = (id = 123459): ApiGen_Concepts_PropertyManagement => ({
  id,
  managementPurposes: [],
  additionalDetails: 'test',
  isTaxesPayable: null,
  isUtilitiesPayable: null,
  relatedLeases: 0,
  leaseExpiryDate: null,
  ...getEmptyBaseAudit(1),
});

export const getMockApiPropertyManagementPurpose = (
  id = 1,
): ApiGen_Concepts_PropertyManagementPurpose => ({
  id,
  propertyId: 123459,
  propertyPurposeTypeCode: {
    id: 'BCFERRIES',
    description: 'BC Ferries',
    displayOrder: null,
    isDisabled: false,
  },
  ...getEmptyBaseAudit(1),
});
