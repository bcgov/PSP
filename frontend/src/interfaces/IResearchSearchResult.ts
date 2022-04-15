import Api_TypeCode from 'interfaces/ITypeCode';

import { Api_PropertyResearchFile } from './../models/api/PropertyResearchFile';
export interface IResearchSearchResult {
  id: number;
  rfileNumber: string;
  name?: string;
  region?: string;
  appLastUpdateUserid: string;
  appCreateTimestamp: string;
  appCreateUserid: string;
  appLastUpdateTimestamp: string;
  researchFileStatusTypeCode: Api_TypeCode<string>;
  researchProperties: Api_PropertyResearchFile[];
}
