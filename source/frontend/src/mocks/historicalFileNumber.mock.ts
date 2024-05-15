import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';

export const mockHistoricalFileNumber = (
  id = 1,
  propertyId = 10,
  historicalFileNumber: string,
  historicalFileNumberTypeCode: string,
  historicalFileNumberTypeCodeDescription: string,
  otherHistFileNumberTypeCode: string | null = null,
): ApiGen_Concepts_HistoricalFileNumber => ({
  id: id,
  propertyId: propertyId,
  historicalFileNumber: historicalFileNumber,
  historicalFileNumberTypeCode: {
    id: historicalFileNumberTypeCode,
    description: historicalFileNumberTypeCodeDescription,
    isDisabled: false,
    displayOrder: 1,
  },
  property: null,
  otherHistFileNumberTypeCode: otherHistFileNumberTypeCode,
  isDisabled: false,
  rowVersion: 1,
  appCreateTimestamp: '2022-05-28T00:57:37.42',
  appLastUpdateTimestamp: '2022-07-28T00:57:37.42',
  appLastUpdateUserid: 'admin',
  appCreateUserid: 'admin',
  appLastUpdateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
  appCreateUserGuid: '14c9a273-6f4a-4859-8d59-9264d3cee53f',
});
