import {
  Api_AcquisitionFile,
  Api_AcquisitionFileProperty,
  Api_AcquisitionFileTeam,
} from '@/models/api/AcquisitionFile';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_Project } from '@/models/api/Project';
import Api_TypeCode from '@/models/api/TypeCode';
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
  acquisitionFileStatusTypeCode?: Api_TypeCode<string>;
  fileProperties?: Api_AcquisitionFileProperty[];
  project?: Api_Project;
  alternateProject?: Api_Project;
  aquisitionTeam?: Api_AcquisitionFileTeam[];
  compensationRequisitions?: Api_CompensationRequisition[];

  static fromApi(base: Api_AcquisitionFile): AcquisitionSearchResultModel {
    var newModel = new AcquisitionSearchResultModel();
    newModel.id = base.id;
    newModel.fileName = base.fileName;
    newModel.fileNumber = base.fileNumber;
    newModel.legacyFileNumber = base.legacyFileNumber;
    newModel.regionCode = base.regionCode?.description;
    newModel.appLastUpdateUserid = base.appLastUpdateUserid;
    newModel.appCreateTimestamp = base.appCreateTimestamp;
    newModel.appCreateUserid = base.appCreateUserid;
    newModel.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    newModel.acquisitionFileStatusTypeCode = base.fileStatusTypeCode;
    newModel.fileProperties = base.fileProperties;
    newModel.project = base.project;
    newModel.compensationRequisitions = base.compensationRequisitions;
    newModel.aquisitionTeam = base.acquisitionTeam;
    return newModel;
  }
}
