import { InterestHolderType } from '@/constants/interestHolderTypes';
import { ChecklistItemFormModel } from '@/features/mapSideBar/shared/tabs/checklist/update/models';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { fromTypeCode, fromTypeCodeNullable, toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidId, isValidIsoDateTime } from '@/utils/utils';

import {
  AcquisitionOwnerFormModel,
  AcquisitionTeamFormModel,
  WithAcquisitionOwners,
  WithAcquisitionTeam,
} from '../../../common/models';
import { ProgressStatusModel } from '../../../models/ProgressStatusModel';
import { TakingTypeStatusModel } from '../../../models/TakingTypeStatusModel';
import { InterestHolderForm } from '../../stakeholders/update/models';

export class UpdateAcquisitionSummaryFormModel
  implements WithAcquisitionTeam, WithAcquisitionOwners
{
  id?: number;
  parentAcquisitionFileId: number | null = null;
  fileNo?: number;
  fileNumberSuffix?: number;
  fileNumber?: string;
  fileName?: string = '';
  legacyFileNumber?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  estimatedCompletionDate?: string;
  possessionDate?: string;

  //Progress Statuses
  progressStatuses: ProgressStatusModel[] = [];
  appraisalStatusType: string | null = null;
  legalSurveyStatusType: string | null = null;
  takingStatuses: TakingTypeStatusModel[] = [];
  expropiationRiskStatusType: string | null = null;

  rowVersion?: number;
  // Code Tables
  fileStatusTypeCode?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  team: AcquisitionTeamFormModel[] = [];
  owners: AcquisitionOwnerFormModel[] = [];
  fileChecklist: ChecklistItemFormModel[] = [];

  subfileInterestTypeCode: string | null = null;
  otherSubfileInterestType: string | null = null;

  project?: IAutocompletePrediction;
  product = '';
  // read-only project and product descriptions (for sub-files)
  formattedProject = '';
  formatterProduct = '';

  fundingTypeCode?: string;
  fundingTypeOtherDescription = '';

  ownerSolicitor: InterestHolderForm = new InterestHolderForm(InterestHolderType.OWNER_SOLICITOR);
  ownerRepresentative: InterestHolderForm = new InterestHolderForm(
    InterestHolderType.OWNER_REPRESENTATIVE,
  );
  otherInterestHolders: ApiGen_Concepts_InterestHolder[] = [];
  legacyStakeholders: string[] = [];

  toApi(): ApiGen_Concepts_AcquisitionFile {
    return {
      id: this.id || 0,
      parentAcquisitionFileId: isValidId(this.parentAcquisitionFileId)
        ? this.parentAcquisitionFileId
        : null,
      fileNo: this.fileNo ?? 0,
      fileNumber: this.fileNumber ?? null,
      fileNumberSuffix: this.fileNumberSuffix ?? 0,
      legacyFileNumber: this.legacyFileNumber ?? null,
      fileName: this.fileName ?? null,
      assignedDate: isValidIsoDateTime(this.assignedDate) ? this.assignedDate : null,
      deliveryDate: isValidIsoDateTime(this.deliveryDate) ? this.deliveryDate : null,
      estimatedCompletionDate: isValidIsoDateTime(this.estimatedCompletionDate)
        ? this.estimatedCompletionDate
        : null,
      possessionDate: isValidIsoDateTime(this.possessionDate) ? this.possessionDate : null,
      acquisitionFileProgressStatuses: this.progressStatuses.map(x => x.toApi(this.id ?? 0)),
      acquisitionFileAppraisalStatusTypeCode: toTypeCodeNullable(this.appraisalStatusType),
      acquisitionFileLegalSurveyStatusTypeCode: toTypeCodeNullable(this.legalSurveyStatusType),
      acquisitionFileTakingStatuses: this.takingStatuses.map(x => x.toApi(this.id ?? 0)),
      acquisitionFileExpropiationRiskStatusTypeCode: toTypeCodeNullable(
        this.expropiationRiskStatusType,
      ),
      fileStatusTypeCode: toTypeCodeNullable(this.fileStatusTypeCode),
      acquisitionPhysFileStatusTypeCode: toTypeCodeNullable(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCodeNullable(this.acquisitionType),
      regionCode: toTypeCodeNullable(Number(this.region)),
      projectId: isValidId(this.project?.id) ? this.project!.id : null,
      productId: this.product !== '' ? Number(this.product) : null,
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      fundingOther: this.fundingTypeOtherDescription,
      subfileInterestTypeCode: toTypeCodeNullable(this.subfileInterestTypeCode),
      otherSubfileInterestType: this.otherSubfileInterestType,
      acquisitionFileOwners: this.owners
        .filter(x => !x.isEmpty())
        .map<ApiGen_Concepts_AcquisitionFileOwner>(x => x.toApi()),
      acquisitionTeam: this.team
        .filter(x => !!x.contact && !!x.contactTypeCode)
        .map(x => x.toApi(this.id || 0))
        .filter(exists),
      acquisitionFileInterestHolders: [
        ...this.otherInterestHolders,
        InterestHolderForm.toApi(this.ownerSolicitor, []),
        InterestHolderForm.toApi(this.ownerRepresentative, []),
      ].filter(exists),
      fileChecklistItems: this.fileChecklist.map(x => x.toApi()),
      compensationRequisitions: null,
      fileProperties: [],
      legacyStakeholders: null,
      product: null,
      project: null,
      totalAllowableCompensation: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_AcquisitionFile): UpdateAcquisitionSummaryFormModel {
    const newForm = new UpdateAcquisitionSummaryFormModel();
    newForm.id = model.id;
    newForm.parentAcquisitionFileId = model.parentAcquisitionFileId;
    newForm.fileNo = model.fileNo;
    newForm.fileNumberSuffix = model.fileNumberSuffix;
    newForm.fileNumber = model.fileNumber ?? undefined;
    newForm.legacyFileNumber = model.legacyFileNumber ?? undefined;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion ?? undefined;
    newForm.assignedDate = model.assignedDate ?? undefined;
    newForm.deliveryDate = model.deliveryDate ?? undefined;
    newForm.estimatedCompletionDate = model.estimatedCompletionDate ?? undefined;
    newForm.possessionDate = model.possessionDate ?? undefined;
    newForm.progressStatuses =
      model.acquisitionFileProgressStatuses.map(x => ProgressStatusModel.fromApi(x)) ?? [];
    newForm.appraisalStatusType = fromTypeCodeNullable(
      model.acquisitionFileAppraisalStatusTypeCode,
    );
    newForm.legalSurveyStatusType = fromTypeCodeNullable(
      model.acquisitionFileLegalSurveyStatusTypeCode,
    );
    newForm.takingStatuses =
      model.acquisitionFileTakingStatuses.map(x => TakingTypeStatusModel.fromApi(x)) ?? [];
    newForm.expropiationRiskStatusType = fromTypeCodeNullable(
      model.acquisitionFileExpropiationRiskStatusTypeCode,
    );
    newForm.fileStatusTypeCode = fromTypeCode(model.fileStatusTypeCode) ?? undefined;
    newForm.acquisitionPhysFileStatusType =
      fromTypeCode(model.acquisitionPhysFileStatusTypeCode) ?? undefined;
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode) ?? undefined;
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    newForm.team = model.acquisitionTeam?.map(x => AcquisitionTeamFormModel.fromApi(x)) || [];
    newForm.owners =
      model.acquisitionFileOwners?.map(x => AcquisitionOwnerFormModel.fromApi(x)) || [];
    newForm.fundingTypeCode = model.fundingTypeCode?.id ?? undefined;
    newForm.fundingTypeOtherDescription = model.fundingOther || '';
    newForm.project = exists(model.project)
      ? { id: model.project?.id || 0, text: model.project?.description || '' }
      : undefined;
    newForm.product = model.product?.id?.toString() ?? '';
    // project and product read-only values for display on sub-files
    newForm.formattedProject = exists(model.project)
      ? model.project.code + ' - ' + model.project.description
      : '';
    newForm.formatterProduct = exists(model.product)
      ? model.product.code + ' ' + model.product.description
      : '';
    newForm.subfileInterestTypeCode = fromTypeCode(model.subfileInterestTypeCode);
    newForm.otherSubfileInterestType = model.otherSubfileInterestType;
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
