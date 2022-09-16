import Api_TypeCode from 'interfaces/ITypeCode';

import { Api_PropertyResearchFile } from './../models/api/PropertyResearchFile';
export interface IResearchSearchResult {
  id: number;
  fileNumber: string;
  fileName?: string;
  region?: string;
  appLastUpdateUserid: string;
  appCreateTimestamp: string;
  appCreateUserid: string;
  appLastUpdateTimestamp: string;
  researchFileStatusTypeCode: Api_TypeCode<string>;
  researchProperties: Api_PropertyResearchFile[];
}
