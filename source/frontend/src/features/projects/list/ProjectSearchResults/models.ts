import { Api_Project } from 'models/api/Project';

export class ProjectSearchResultModel {
  id: number | undefined;
  code: string | undefined;
  description: string | undefined;
  region: string | undefined;
  status: string | undefined;
  lastUpdatedBy: string | undefined;
  lastUpdatedDate: string | undefined;

  static fromApi(base: Api_Project): ProjectSearchResultModel {
    var newModel = new ProjectSearchResultModel();

    newModel.id = base.id ?? undefined;
    newModel.code = base.code?.toString() ?? '';
    newModel.description = base.description ?? '';
    newModel.region = base.regionCode?.description ?? '';
    newModel.status = base.projectStatusTypeCode?.description;
    newModel.lastUpdatedBy = base.appLastUpdateUserid ?? '';
    newModel.lastUpdatedDate = base.appLastUpdateTimestamp;

    return newModel;
  }
}
