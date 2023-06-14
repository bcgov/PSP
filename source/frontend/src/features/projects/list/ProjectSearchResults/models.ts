import { Api_Project } from '@/models/api/Project';

export class ProjectSearchResultModel {
  id: number | null = null;
  code: string | '' = '';
  description: string | '' = '';
  region: string | '' = '';
  status: string | '' = '';
  lastUpdatedBy: string | '' = '';
  lastUpdatedDate: string | '' = '';

  static fromApi(base: Api_Project): ProjectSearchResultModel {
    var newModel = new ProjectSearchResultModel();

    newModel.id = base.id ?? null;
    newModel.code = base.code?.toString() ?? '';
    newModel.description = base.description ?? '';
    newModel.region = base.regionCode?.description ?? '';
    newModel.status = base.projectStatusTypeCode?.description ?? '';
    newModel.lastUpdatedBy = base.appLastUpdateUserid ?? '';
    newModel.lastUpdatedDate = base.appLastUpdateTimestamp ?? '';

    return newModel;
  }
}
