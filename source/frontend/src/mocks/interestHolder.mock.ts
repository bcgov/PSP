import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_InterestHolderProperty } from '@/models/api/generated/ApiGen_Concepts_InterestHolderProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const emptyApiInterestHolder: ApiGen_Concepts_InterestHolder = {
  interestHolderId: null,
  acquisitionFileId: null,
  person: null,
  personId: null,
  organization: null,
  organizationId: null,
  isDisabled: false,
  interestHolderProperties: [],
  primaryContactId: null,
  primaryContact: null,
  comment: null,
  interestHolderType: null,
  ...getEmptyBaseAudit(),
};

export const emptyInterestHolderProperty: ApiGen_Concepts_InterestHolderProperty = {
  interestHolderId: null,
  propertyInterestTypes: [],
  interestHolderPropertyId: null,
  acquisitionFileProperty: null,
  acquisitionFilePropertyId: null,
  ...getEmptyBaseAudit(),
};
