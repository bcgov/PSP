import { Api_AcquisitionFile, Api_AcquisitionFileProperty } from 'models/api/AcquisitionFile';
import { Api_Project } from 'models/api/Project';
import Api_TypeCode from 'models/api/TypeCode';

export class AcquisitionSearchResultModel {
  id?: number;
  fileNumber?: string;
  fileName?: string;
  regionCode?: string;
  appLastUpdateUserid?: string;
  appCreateTimestamp?: string;
  appCreateUserid?: string;
  appLastUpdateTimestamp?: string;
  acquisitionFileStatusTypeCode?: Api_TypeCode<string>;
  fileProperties?: Api_AcquisitionFileProperty[];
  project?: Api_Project;

  static fromApi(base: Api_AcquisitionFile): AcquisitionSearchResultModel {
    var newModel = new AcquisitionSearchResultModel();
    newModel.id = base.id;
    newModel.fileName = base.fileName;
    newModel.fileNumber = base.fileNumber;
    newModel.regionCode = base.regionCode?.description;
    newModel.appLastUpdateUserid = base.appLastUpdateUserid;
    newModel.appCreateTimestamp = base.appCreateTimestamp;
    newModel.appCreateUserid = base.appCreateUserid;
    newModel.appLastUpdateTimestamp = base.appLastUpdateTimestamp;
    newModel.acquisitionFileStatusTypeCode = base.fileStatusTypeCode;
    newModel.fileProperties = base.fileProperties;
    newModel.project = base.project;
    return newModel;
  }
}
