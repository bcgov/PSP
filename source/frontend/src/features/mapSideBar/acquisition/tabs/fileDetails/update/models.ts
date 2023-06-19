import { IAutocompletePrediction } from '@/interfaces';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFilePerson,
} from '@/models/api/AcquisitionFile';
import { fromTypeCode, stringToNull, toTypeCode } from '@/utils/formUtils';

import {
  AcquisitionOwnerFormModel,
  AcquisitionRepresentativeFormModel,
  AcquisitionSolicitorFormModel,
  AcquisitionTeamFormModel,
  WithAcquisitionOwners,
  WithAcquisitionTeam,
} from '../../../common/models';

export class UpdateAcquisitionSummaryFormModel
  implements WithAcquisitionTeam, WithAcquisitionOwners
{
  id?: number;
  fileNo?: number;
  fileNumber?: string;
  fileName?: string = '';
  legacyFileNumber?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  completionDate?: string;
  rowVersion?: number;
  // Code Tables
  fileStatusTypeCode?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  team: AcquisitionTeamFormModel[] = [];
  owners: AcquisitionOwnerFormModel[] = [];

  project?: IAutocompletePrediction;
  product: string = '';
  fundingTypeCode?: string;
  fundingTypeOtherDescription: string = '';
  ownerSolicitor: AcquisitionSolicitorFormModel = new AcquisitionSolicitorFormModel(null);
  ownerRepresentative: AcquisitionRepresentativeFormModel = new AcquisitionRepresentativeFormModel(
    null,
  );

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileNo: this.fileNo,
      fileNumber: this.fileNumber,
      legacyFileNumber: this.legacyFileNumber,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: stringToNull(this.assignedDate),
      deliveryDate: stringToNull(this.deliveryDate),
      completionDate: stringToNull(this.completionDate),
      fileStatusTypeCode: toTypeCode(this.fileStatusTypeCode),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCode(this.acquisitionType),
      regionCode: toTypeCode(Number(this.region)),
      projectId: this.project?.id !== undefined && this.project?.id !== 0 ? this.project?.id : null,
      productId: this.product !== '' ? Number(this.product) : null,
      fundingTypeCode: toTypeCode(this.fundingTypeCode),
      fundingOther: this.fundingTypeOtherDescription,
      acquisitionFileOwners: this.owners
        .filter(x => !x.isEmpty())
        .map<Api_AcquisitionFileOwner>(x => x.toApi()),
      acquisitionTeam: this.team
        .filter(x => !!x.contact && !!x.contactTypeCode)
        .map<Api_AcquisitionFilePerson>(x => x.toApi(this.id || 0)),
      acquisitionFileOwnerSolicitors: this.ownerSolicitor.contact
        ? [this.ownerSolicitor.toApi()]
        : [],
      acquisitionFileOwnerRepresentatives: this.ownerRepresentative.contact
        ? [this.ownerRepresentative.toApi()]
        : [],
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
    newForm.completionDate = model.completionDate ?? '';
    newForm.fileStatusTypeCode = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    newForm.team = model.acquisitionTeam?.map(x => AcquisitionTeamFormModel.fromApi(x)) || [];
    newForm.owners =
      model.acquisitionFileOwners?.map(x => AcquisitionOwnerFormModel.fromApi(x)) || [];
    newForm.fundingTypeCode = model.fundingTypeCode?.id;
    newForm.fundingTypeOtherDescription = model.fundingOther || '';
    newForm.project =
      model.project !== undefined
        ? { id: model.project?.id || 0, text: model.project?.description || '' }
        : undefined;
    newForm.product = model.product?.id?.toString() ?? '';
    newForm.ownerSolicitor = model.acquisitionFileOwnerSolicitors?.length
      ? AcquisitionSolicitorFormModel.fromApi(model.acquisitionFileOwnerSolicitors[0])
      : new AcquisitionSolicitorFormModel(null);
    newForm.ownerRepresentative = model.acquisitionFileOwnerRepresentatives?.length
      ? AcquisitionRepresentativeFormModel.fromApi(model.acquisitionFileOwnerRepresentatives[0])
      : new AcquisitionRepresentativeFormModel(null);

    return newForm;
  }
}
