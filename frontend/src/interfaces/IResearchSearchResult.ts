import Api_TypeCode from 'interfaces/ITypeCode';
export interface IResearchSearchResult {
  id: number;
  rFileNumber: string;
  name?: string;
  region?: string;
  createdByIdir: string;
  appCreateTimestamp: string;
  updatedByIdir: string;
  appUpdateTimestamp: string;
  researchFileStatusTypeCode: Api_TypeCode<string>;
}
