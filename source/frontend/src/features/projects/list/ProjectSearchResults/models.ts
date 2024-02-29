import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { isValidIsoDateTime } from '@/utils';

export class ProjectSearchResultModel {
  id: number | null = null;
  code: string | '' = '';
  description: string | '' = '';
  region: string | '' = '';
  status: string | '' = '';
  lastUpdatedBy: string | '' = '';
  lastUpdatedDate: string | '' = '';

  static fromApi(base: ApiGen_Concepts_Project): ProjectSearchResultModel {
    const newModel = new ProjectSearchResultModel();

    newModel.id = base.id ?? null;
    newModel.code = base.code?.toString() ?? '';
    newModel.description = base.description ?? '';
    newModel.region = base.regionCode?.description ?? '';
    newModel.status = base.projectStatusTypeCode?.description ?? '';
    newModel.lastUpdatedBy = base.appLastUpdateUserid ?? '';
    newModel.lastUpdatedDate = isValidIsoDateTime(base.appLastUpdateTimestamp)
      ? base.appLastUpdateTimestamp
      : '';

    return newModel;
  }
}
