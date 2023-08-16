import { InterestHolderType } from '@/constants/interestHolderTypes';
import { IAutocompletePrediction } from '@/interfaces';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFileOwner,
  Api_AcquisitionFilePerson,
} from '@/models/api/AcquisitionFile';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { fromTypeCode, stringToUndefined, toTypeCode } from '@/utils/formUtils';

import {
  AcquisitionOwnerFormModel,
  AcquisitionTeamFormModel,
  WithAcquisitionOwners,
  WithAcquisitionTeam,
} from '../../../common/models';
import { InterestHolderForm } from '../../stakeholders/update/models';

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

  ownerSolicitor: InterestHolderForm = new InterestHolderForm(InterestHolderType.OWNER_SOLICITOR);
  ownerRepresentative: InterestHolderForm = new InterestHolderForm(
    InterestHolderType.OWNER_REPRESENTATIVE,
  );
  otherInterestHolders: Api_InterestHolder[] = [];
  legacyStakeholders: string[] = [];

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileNo: this.fileNo,
      fileNumber: this.fileNumber,
      legacyFileNumber: this.legacyFileNumber,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: stringToUndefined(this.assignedDate),
      deliveryDate: stringToUndefined(this.deliveryDate),
      completionDate: stringToUndefined(this.completionDate),
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
      acquisitionFileInterestHolders: [
        ...this.otherInterestHolders,
        InterestHolderForm.toApi(this.ownerSolicitor, []),
        InterestHolderForm.toApi(this.ownerRepresentative, []),
      ].filter((x): x is Api_InterestHolder => x !== null),
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

    const interestHolders = model.acquisitionFileInterestHolders?.map(x =>
      InterestHolderForm.fromApi(x, x.interestHolderType?.id as InterestHolderType),
    );
    newForm.ownerSolicitor =
      interestHolders?.find(x => x.interestTypeCode === InterestHolderType.OWNER_SOLICITOR) ??
      new InterestHolderForm(InterestHolderType.OWNER_SOLICITOR, model.id);
    newForm.ownerRepresentative =
      interestHolders?.find(x => x.interestTypeCode === InterestHolderType.OWNER_REPRESENTATIVE) ??
      new InterestHolderForm(InterestHolderType.OWNER_REPRESENTATIVE, model.id);

    newForm.otherInterestHolders =
      model.acquisitionFileInterestHolders?.filter(
        x =>
          x.interestHolderType?.id !== InterestHolderType.OWNER_SOLICITOR &&
          x.interestHolderType?.id !== InterestHolderType.OWNER_REPRESENTATIVE,
      ) || [];

    return newForm;
  }
}
