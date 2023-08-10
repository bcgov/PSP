import { Api_ExpropriationPayment } from '@/models/api/Form8';

export const mockGetForm8Api = (
  id: number = 1,
  acquisitionFileId: number = 1,
): Api_ExpropriationPayment => ({
  id,
  acquisitionFileId,
  acquisitionOwnerId: 10,
  interestHolderId: null,
  expropriatingAuthorityId: 1,
  description: 'MY DESCRIPTION',
  isDisabled: false,
  rowVersion: 1,
  paymentItems: [],
});
