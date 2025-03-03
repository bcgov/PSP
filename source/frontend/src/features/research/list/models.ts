import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';

export class ResearchSearchResultModel {
  id?: number;
  rfileNumber?: string;
  name?: string;
  appLastUpdateUserid?: string;
  appCreateTimestamp?: string;
  appCreateUserid?: string;
  appLastUpdateTimestamp?: string;
  researchFileStatusTypeCode?: ApiGen_Base_CodeType<string>;
  fileProperties?: ApiGen_Concepts_ResearchFileProperty[];

  static fromApi(base: ApiGen_Concepts_ResearchFile): ResearchSearchResultModel {
    const newModel = new ResearchSearchResultModel();
    newModel.id = base.id;
    newModel.name = base.fileName;
    newModel.rfileNumber = base.fileNumber;
    newModel.appLastUpdateUserid = base.appLastUpdateUserid;
    newModel.appCreateTimestamp = base.appCreateTimestamp;
    newModel.appCreateUserid = base.appCreateUserid;
    newModel.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    newModel.researchFileStatusTypeCode = base.fileStatusTypeCode;
    newModel.fileProperties = base.fileProperties;
    return newModel;
  }
}
