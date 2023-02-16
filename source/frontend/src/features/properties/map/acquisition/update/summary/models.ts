import { IAutocompletePrediction } from 'interfaces';
import { Api_AcquisitionFile, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

import { AcquisitionTeamFormModel, WithAcquisitionTeam } from '../../common/models';

export class UpdateAcquisitionSummaryFormModel implements WithAcquisitionTeam {
  id?: number;
  fileNo?: number;
  fileNumber?: string;
  fileName?: string = '';
  legacyFileNumber?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  fileStatusTypeCode?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  team: AcquisitionTeamFormModel[] = [];

  project?: IAutocompletePrediction;
  product?: number;
  fundingTypeCode?: string;
  fundingTypeOtherDescription: string = '';

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileNo: this.fileNo,
      fileNumber: this.fileNumber,
      legacyFileNumber: this.legacyFileNumber,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: this.assignedDate,
      deliveryDate: this.deliveryDate,
      fileStatusTypeCode: toTypeCode(this.fileStatusTypeCode),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCode(this.acquisitionType),
      regionCode: toTypeCode(Number(this.region)),
      project:
        this.project?.id !== undefined && this.project?.id !== 0
          ? { id: this.project?.id }
          : undefined,
      product:
        this.product !== undefined && this.product !== 0 ? { id: Number(this.product) } : undefined,
      fundingTypeCode: toTypeCode(this.fundingTypeCode),
      fundingOther: this.fundingTypeOtherDescription,
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
    newForm.legacyFileNumber = model.legacyFileNumber;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.fileStatusTypeCode = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    newForm.team = model.acquisitionTeam?.map(x => AcquisitionTeamFormModel.fromApi(x)) || [];
    newForm.fundingTypeCode = model.fundingTypeCode?.id;
    newForm.fundingTypeOtherDescription = model.fundingOther || '';
    newForm.project =
      model.project !== undefined
        ? { id: model.project?.id || 0, text: model.project?.description || '' }
        : undefined;
    newForm.product = model.product?.id;

    return newForm;
  }
}
