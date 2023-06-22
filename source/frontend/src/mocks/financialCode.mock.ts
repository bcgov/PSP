import { Api_FinancialCode } from '@/models/api/FinancialCode';

export const mockFinancialCode: () => Api_FinancialCode = () => ({
  id: 3,
  type: 'BusinessFunction',
  code: 'CONSTRUC',
  description: 'CONSTRUCTION',
  effectiveDate: '1999-11-22T18:22:24',
  appCreateTimestamp: '2022-12-06T00:32:27.063',
  appLastUpdateTimestamp: '2022-12-06T00:32:27.063',
  appLastUpdateUserid: 'dbo',
  appCreateUserid: 'dbo',
  rowVersion: 1,
});
