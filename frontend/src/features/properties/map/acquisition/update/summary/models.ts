import { Api_AcquisitionFile, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

import { AcquisitionTeamFormModel, WithAcquisitionTeam } from '../../common/models';

export class UpdateAcquisitionSummaryFormModel implements WithAcquisitionTeam {
  id?: number;
  fileNo?: number;
  fileNumber?: string;
  fileName?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  fileStatusTypeCode?: string;
  acquisitionFileStatusType?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  team: AcquisitionTeamFormModel[] = [];

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileNo: this.fileNo,
      fileNumber: this.fileNumber,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: this.assignedDate,
      deliveryDate: this.deliveryDate,
      fileStatusTypeCode: toTypeCode(this.acquisitionFileStatusType),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCode(this.acquisitionType),
      regionCode: toTypeCode(Number(this.region)),
      acquisitionTeam: this.team
        .filter(x => !!x.contact && !!x.contactTypeCode)
        .map<Api_AcquisitionFilePerson>(x => x.toApi()),
    };
  }

  static fromApi(model: Api_AcquisitionFile): UpdateAcquisitionSummaryFormModel {
    const newForm = new UpdateAcquisitionSummaryFormModel();
    newForm.id = model.id;
    newForm.fileNo = model.fileNo;
    newForm.fileNumber = model.fileNumber;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.fileStatusTypeCode = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionFileStatusType = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    newForm.team = model.acquisitionTeam?.map(x => AcquisitionTeamFormModel.fromApi(x)) || [];

    return newForm;
  }
}
