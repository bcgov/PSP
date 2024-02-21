import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';

export interface IResearchSearchResult {
  id: number;
  fileNumber: string;
  fileName?: string;
  region?: string;
  appLastUpdateUserid: string;
  appCreateTimestamp: string;
  appCreateUserid: string;
  appLastUpdateTimestamp: string;
  fileStatusTypeCode: ApiGen_Base_CodeType<string>;
  fileProperties: ApiGen_Concepts_ResearchFileProperty[];
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
  researchFileStatusTypeCode?: ApiGen_Base_CodeType<string>;
  fileProperties?: ApiGen_Concepts_ResearchFileProperty[];

  static fromApi(base: IResearchSearchResult): ResearchSearchResultModel {
    const newModel = new ResearchSearchResultModel();
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
