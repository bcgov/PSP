import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';

export class AcquisitionSearchResultModel {
  id?: number;
  fileNumber?: string;
  legacyFileNumber?: string;
  fileName?: string;
  regionCode?: string;
  appLastUpdateUserid?: string;
  appCreateTimestamp?: UtcIsoDateTime;
  appCreateUserid?: string;
  appLastUpdateTimestamp?: UtcIsoDateTime;
  acquisitionFileStatusTypeCode?: ApiGen_Base_CodeType<string>;
  fileProperties?: ApiGen_Concepts_AcquisitionFileProperty[];
  project?: ApiGen_Concepts_Project;
  alternateProject?: ApiGen_Concepts_Project;
  acquisitionTeam?: ApiGen_Concepts_AcquisitionFileTeam[];
  compensationRequisitions?: ApiGen_Concepts_CompensationRequisition[];

  static fromApi(base: ApiGen_Concepts_AcquisitionFile): AcquisitionSearchResultModel {
    const newModel = new AcquisitionSearchResultModel();
    newModel.id = base.id;
    newModel.fileName = base.fileName ?? undefined;
    newModel.fileNumber = base.fileNumber ?? undefined;
    newModel.legacyFileNumber = base.legacyFileNumber ?? undefined;
    newModel.regionCode = base.regionCode?.description ?? undefined;
    newModel.appLastUpdateUserid = base.appLastUpdateUserid ?? undefined;
    newModel.appCreateTimestamp = base.appCreateTimestamp;
    newModel.appCreateUserid = base.appCreateUserid ?? undefined;
    newModel.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    newModel.acquisitionFileStatusTypeCode = base.fileStatusTypeCode ?? undefined;
    newModel.fileProperties = base.fileProperties ?? undefined;
    newModel.project = base.project ?? undefined;
    newModel.compensationRequisitions = base.compensationRequisitions ?? undefined;
    newModel.acquisitionTeam = base.acquisitionTeam ?? undefined;
    return newModel;
  }
}
