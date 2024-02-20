import { InterestHolderType } from '@/constants/interestHolderTypes';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { fromTypeCode, stringToNumberOrNull, toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidId, isValidIsoDateTime } from '@/utils/utils';

import { PropertyForm } from '../../shared/models';
import { ChecklistItemFormModel } from '../../shared/tabs/checklist/update/models';
import {
  AcquisitionOwnerFormModel,
  AcquisitionTeamFormModel,
  WithAcquisitionOwners,
  WithAcquisitionTeam,
} from '../common/models';
import { InterestHolderForm } from '../tabs/stakeholders/update/models';

export class AcquisitionForm implements WithAcquisitionTeam, WithAcquisitionOwners {
  id?: number;
  fileName?: string = '';
  legacyFileNumber?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  acquisitionFileStatusType?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  properties: PropertyForm[] = [];
  team: AcquisitionTeamFormModel[] = [];
  owners: AcquisitionOwnerFormModel[] = [];
  fileCheckList: ChecklistItemFormModel[] = [];

  project?: IAutocompletePrediction;
  product = '';
  fundingTypeCode?: string;
  fundingTypeOtherDescription = '';
  ownerSolicitor: InterestHolderForm = new InterestHolderForm(InterestHolderType.OWNER_SOLICITOR);
  ownerRepresentative: InterestHolderForm = new InterestHolderForm(
    InterestHolderType.OWNER_REPRESENTATIVE,
  );
  totalAllowableCompensation: number | '' = '';

  toApi(): ApiGen_Concepts_AcquisitionFile {
    return {
      id: this.id ?? 0,
      fileName: this.fileName ?? null,
      assignedDate: isValidIsoDateTime(this.assignedDate) ? this.assignedDate : null,
      deliveryDate: isValidIsoDateTime(this.deliveryDate) ? this.deliveryDate : null,
      totalAllowableCompensation: stringToNumberOrNull(this.totalAllowableCompensation),
      legacyFileNumber: this.legacyFileNumber ?? null,
      fileStatusTypeCode: toTypeCodeNullable(this.acquisitionFileStatusType),
      acquisitionPhysFileStatusTypeCode: toTypeCodeNullable(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCodeNullable(this.acquisitionType),
      regionCode: toTypeCodeNullable(Number(this.region)),
      projectId: isValidId(this.project?.id) ? this.project!.id : null,
      productId: this.product !== '' ? Number(this.product) : null,
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      fundingOther: this.fundingTypeOtherDescription,
      // ACQ file properties
      fileProperties: this.properties.map<ApiGen_Concepts_AcquisitionFileProperty>(ap => ({
        id: ap.id ?? 0,
        propertyName: ap.name ?? null,
        displayOrder: ap.displayOrder ?? null,
        rowVersion: ap.rowVersion ?? null,
        property: ap.toApi(),
        propertyId: ap.apiId ?? 0,
        fileId: this.id ?? 0,
        acquisitionFile: null,
        file: null,
      })),
      acquisitionFileOwners: this.owners
        .filter(x => !x.isEmpty())
        .map<ApiGen_Concepts_AcquisitionFileOwner>(x => x.toApi()),
      acquisitionTeam: this.team
        .filter(x => !!x.contact && !!x.contactTypeCode)
        .map(x => x.toApi(this.id || 0))
        .filter(exists),
      acquisitionFileInterestHolders: [
        InterestHolderForm.toApi(this.ownerSolicitor, []),
        InterestHolderForm.toApi(this.ownerRepresentative, []),
      ].filter(exists),
      fileChecklistItems: this.fileCheckList.map(x => x.toApi()),
      completionDate: null,
      compensationRequisitions: null,
      fileNo: 0,
      fileNumber: null,
      legacyStakeholders: null,
      product: null,
      project: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = model.id;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion ?? undefined;
    newForm.assignedDate = model.assignedDate ?? undefined;
    newForm.deliveryDate = model.deliveryDate ?? undefined;
    newForm.totalAllowableCompensation = model.totalAllowableCompensation || '';
    newForm.legacyFileNumber = model.legacyFileNumber ?? undefined;
    newForm.acquisitionFileStatusType = fromTypeCode(model.fileStatusTypeCode) ?? undefined;
    newForm.acquisitionPhysFileStatusType =
      fromTypeCode(model.acquisitionPhysFileStatusTypeCode) ?? undefined;
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode) ?? undefined;
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    // ACQ file properties
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.product = model.product?.id?.toString() ?? '';
    newForm.fundingTypeCode = model.fundingTypeCode?.id ?? undefined;
    newForm.fundingTypeOtherDescription = model.fundingOther || '';
    newForm.project = exists(model.project)
      ? { id: model.project.id || 0, text: model.project.description || '' }
      : undefined;

    const interestHolders = model.acquisitionFileInterestHolders?.map(x =>
      InterestHolderForm.fromApi(x, x.interestHolderType?.id as InterestHolderType),
    );
    newForm.ownerSolicitor =
      interestHolders?.find(x => x.interestTypeCode === InterestHolderType.OWNER_SOLICITOR) ??
      new InterestHolderForm(InterestHolderType.OWNER_SOLICITOR, model.id);
    newForm.ownerRepresentative =
      interestHolders?.find(x => x.interestTypeCode === InterestHolderType.OWNER_REPRESENTATIVE) ??
      new InterestHolderForm(InterestHolderType.OWNER_REPRESENTATIVE, model.id);

    return newForm;
  }
}
