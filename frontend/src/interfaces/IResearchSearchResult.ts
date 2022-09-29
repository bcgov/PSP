import Api_TypeCode from 'interfaces/ITypeCode';
import { Api_ResearchFileProperty } from 'models/api/ResearchFile';

export interface IResearchSearchResult {
  id: number;
  fileNumber: string;
  fileName?: string;
  region?: string;
  appLastUpdateUserid: string;
  appCreateTimestamp: string;
  appCreateUserid: string;
  appLastUpdateTimestamp: string;
  fileStatusTypeCode: Api_TypeCode<string>;
  fileProperties: Api_ResearchFileProperty[];
}
