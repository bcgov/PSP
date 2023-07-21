import Api_TypeCode from '@/interfaces/ITypeCode';
import { Api_ResearchFileProperty } from '@/models/api/ResearchFile';

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

export class ResearchSearchResultModel {
  id?: number;
  rfileNumber?: string;
  name?: string;
  region?: string;
  appLastUpdateUserid?: string;
  appCreateTimestamp?: string;
  appCreateUserid?: string;
  appLastUpdateTimestamp?: string;
  researchFileStatusTypeCode?: Api_TypeCode<string>;
  fileProperties?: Api_ResearchFileProperty[];

  static fromApi(base: IResearchSearchResult): ResearchSearchResultModel {
    var newModel = new ResearchSearchResultModel();
    newModel.id = base.id;
    newModel.name = base.fileName;
    newModel.rfileNumber = base.fileNumber;
    newModel.region = base.region;
    newModel.appLastUpdateUserid = base.appLastUpdateUserid;
    newModel.appCreateTimestamp = base.appCreateTimestamp;
    newModel.appCreateUserid = base.appCreateUserid;
    newModel.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    newModel.researchFileStatusTypeCode = base.fileStatusTypeCode;
    newModel.fileProperties = base.fileProperties;
    return newModel;
  }
}
