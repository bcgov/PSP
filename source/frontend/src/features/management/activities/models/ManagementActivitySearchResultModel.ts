import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivityProperty } from '@/models/api/generated/ApiGen_Concepts_PropertyActivityProperty';

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
  activivityProperty: ApiGen_Concepts_PropertyActivityProperty | null;

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
    newModel.activivityProperty = base.activityProperties ? base.activityProperties[0] : null;

    return newModel;
  }
}
