import { Api_Form8 } from '@/models/api/Form8';

export const mockGetForm8Api = (id: number = 1, acquisitionFileId: number = 1): Api_Form8 => ({
  id,
  acquisitionFileId,
  acquisitionOwnerId: 10,
  interestHolderId: null,
  expropriatingAuthorityId: 10,
  paymentItemTypeCode: null,
  description: 'MY DESCRIPTION',
  isGstRequired: null,
  pretaxAmount: null,
  taxAmount: null,
  totalAmount: null,
  isDisabled: false,
  rowVersion: 1,
});
