import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';

export class ManagementActivitySearchResultModel {
  id: number | null = null;
  managementFileId: number | null = null;
  fileName = '';
  project: ApiGen_Concepts_Project | null = null;
  legacyFileNum = '';
  activityStatus = '';
  activityType = '';
  activitySubType = '';
  description = '';
  properties: ApiGen_Concepts_Property[] = [];

  static fromApi(base: ApiGen_Concepts_PropertyActivity): ManagementActivitySearchResultModel {
    const newModel = new ManagementActivitySearchResultModel();
    newModel.id = base.id ?? null;
    newModel.managementFileId = base.managementFileId ?? null;
    newModel.fileName = base.managementFile?.fileName ?? '';
    newModel.legacyFileNum = base.managementFile?.legacyFileNum ?? '';
    newModel.project = base.managementFile?.project ?? null;
    newModel.properties = base.managementFileId
      ? base.managementFile.fileProperties?.map(x => x.property) ?? []
      : base.activityProperties?.map(x => x.property) ?? [];
    newModel.activityType = base.activityTypeCode?.description ?? '';
    newModel.activitySubType = base.activitySubtypeCode?.description ?? '';
    newModel.activityStatus = base.activityStatusTypeCode?.description ?? '';
    newModel.description = base.description ?? '';

    return newModel;
  }
}
