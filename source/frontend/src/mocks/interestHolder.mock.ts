import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';

export const emptyApiInterestHolder: Api_InterestHolder = {
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
  interestHolderType: {},
};

export const emptyInterestHolderProperty: Api_InterestHolderProperty = {
  interestHolderId: null,
  propertyInterestTypes: [],
  interestHolderPropertyId: null,
  acquisitionFileProperty: null,
  acquisitionFilePropertyId: null,
  isDisabled: false,
  rowVersion: 0,
};
