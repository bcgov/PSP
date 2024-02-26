import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';

export const mockFinancialCode: () => ApiGen_Concepts_FinancialCode = () => ({
  id: 3,
  type: ApiGen_Concepts_FinancialCodeTypes.BusinessFunction,
  code: 'CONSTRUC',
  description: 'CONSTRUCTION',
  effectiveDate: '1999-11-22T18:22:24',
  appCreateTimestamp: '2022-12-06T00:32:27.063',
  appLastUpdateTimestamp: '2022-12-06T00:32:27.063',
  appLastUpdateUserid: 'dbo',
  appCreateUserid: 'dbo',
  rowVersion: 1,
  appCreateUserGuid: null,
  appLastUpdateUserGuid: null,
  displayOrder: null,
  expiryDate: null,
});
