import { InterestHolderType } from '@/constants/interestHolderTypes';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import {
  fromTypeCode,
  fromTypeCodeNullable,
  stringToNumberOrNull,
  toTypeCodeNullable,
} from '@/utils/formUtils';
import { exists, isValidId, isValidIsoDateTime } from '@/utils/utils';

import { PropertyForm } from '../../shared/models';
import { ChecklistItemFormModel } from '../../shared/tabs/checklist/update/models';
import {
  AcquisitionOwnerFormModel,
  AcquisitionTeamFormModel,
  WithAcquisitionOwners,
  WithAcquisitionTeam,
} from '../common/models';
import { ProgressStatusModel } from '../models/ProgressStatusModel';
import { TakingTypeStatusModel } from '../models/TakingTypeStatusModel';
import { InterestHolderForm } from '../tabs/stakeholders/update/models';

export class AcquisitionForm implements WithAcquisitionTeam, WithAcquisitionOwners {
  id?: number;
  parentAcquisitionFileId: number | null = null;
  fileName?: string = '';
  legacyFileNumber?: string = '';
  assignedDate?: string = '';
  deliveryDate?: string = '';
  estimatedCompletionDate?: string = '';
  possessionDate?: string = '';
  rowVersion?: number;

  //Progress Statuses
  progressStatuses: ProgressStatusModel[] = [];
  appraisalStatusType: string | null = null;
  legalSurveyStatusType: string | null = null;
  takingStatuses: TakingTypeStatusModel[] = [];
  expropiationRiskStatusType: string | null = null;

  // Code Tables
  acquisitionFileStatusType?: string = '';
  acquisitionPhysFileStatusType?: string = '';
  acquisitionType?: string = '';
  // MOTI region
  region?: string = '';
  properties: PropertyForm[] = [];
  team: AcquisitionTeamFormModel[] = [];
  owners: AcquisitionOwnerFormModel[] = [];
  fileCheckList: ChecklistItemFormModel[] = [];

  subfileInterestTypeCode: string | null = null;
  otherSubfileInterestType: string | null = null;

  project?: IAutocompletePrediction;
  product = '';
  // read-only project and product descriptions (for sub-files)
  formattedProject = '';
  formattedProduct = '';

  fundingTypeCode?: string = '';
  fundingTypeOtherDescription = '';
  ownerSolicitor: InterestHolderForm = new InterestHolderForm(InterestHolderType.OWNER_SOLICITOR);
  ownerRepresentative: InterestHolderForm = new InterestHolderForm(
    InterestHolderType.OWNER_REPRESENTATIVE,
  );
  totalAllowableCompensation: number | '' = '';

  toApi(): ApiGen_Concepts_AcquisitionFile {
    return {
      id: this.id ?? 0,
      parentAcquisitionFileId: isValidId(this.parentAcquisitionFileId)
        ? this.parentAcquisitionFileId
        : null,
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
      totalAllowableCompensation: stringToNumberOrNull(this.totalAllowableCompensation),
      legacyFileNumber: this.legacyFileNumber ?? null,
      fileStatusTypeCode: toTypeCodeNullable(this.acquisitionFileStatusType),
      acquisitionPhysFileStatusTypeCode: toTypeCodeNullable(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCodeNullable(this.acquisitionType),
      regionCode: toTypeCodeNullable(Number(this.region)),
      projectId: isValidId(this.project?.id) ? this.project!.id : null,
      productId: isValidId(Number(this.product)) ? Number(this.product) : null,
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      fundingOther: this.fundingTypeOtherDescription,
      subfileInterestTypeCode: toTypeCodeNullable(this.subfileInterestTypeCode),
      otherSubfileInterestType: this.otherSubfileInterestType,
      // ACQ file properties
      fileProperties: this.properties.map(x => this.toPropertyApi(x)),
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
      compensationRequisitions: null,
      fileNo: 0,
      fileNumber: null,
      fileNumberSuffix: 0,
      legacyStakeholders: null,
      product: null,
      project: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  private toPropertyApi(x: PropertyForm): ApiGen_Concepts_AcquisitionFileProperty {
    const apiFileProperty = x.toFilePropertyApi(this.id);
    return {
      ...apiFileProperty,
      file: null,
    };
  }

  // Creates a form model that follows the rules for creating sub-files from a parent (main) file
  // Copy the following from Parent File:
  // - Project and Product
  // - Schedule
  // - Acquisition details (except file name)
  // - Acquisition team
  // - Owner information is NOT copied over
  static fromParentFileApi(parentFile: ApiGen_Concepts_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = undefined;
    newForm.parentAcquisitionFileId = parentFile.id;
    newForm.rowVersion = undefined;
    // project + product (read-only in sub-files)
    newForm.formattedProject = exists(parentFile.project)
      ? parentFile.project.code + ' - ' + parentFile.project.description
      : '';
    newForm.formattedProduct = exists(parentFile.product)
      ? parentFile.product.code + ' ' + parentFile.product.description
      : '';
    newForm.project = exists(parentFile.project)
      ? { id: parentFile.project?.id || 0, text: parentFile.project?.description || '' }
      : undefined;
    newForm.product = parentFile.product?.id?.toString() ?? '';

    newForm.fundingTypeCode = fromTypeCode(parentFile.fundingTypeCode) ?? undefined;
    newForm.fundingTypeOtherDescription = parentFile.fundingOther || '';
    // schedule
    newForm.assignedDate = parentFile.assignedDate ?? undefined;
    newForm.deliveryDate = parentFile.deliveryDate ?? undefined;
    newForm.estimatedCompletionDate = parentFile.estimatedCompletionDate ?? undefined;
    newForm.possessionDate = parentFile.possessionDate ?? undefined;
    // acquisition details
    newForm.fileName = '';
    newForm.legacyFileNumber = parentFile.legacyFileNumber ?? undefined;
    newForm.acquisitionPhysFileStatusType =
      fromTypeCode(parentFile.acquisitionPhysFileStatusTypeCode) ?? undefined;
    newForm.acquisitionType = fromTypeCode(parentFile.acquisitionTypeCode) ?? undefined;
    newForm.region = fromTypeCode(parentFile.regionCode)?.toString() ?? undefined;
    // acquisition team
    newForm.team =
      parentFile.acquisitionTeam?.map(x => {
        const teamForm = AcquisitionTeamFormModel.fromApi(x);
        teamForm.id = null;
        return teamForm;
      }) || [];

    return newForm;
  }

  static fromApi(model: ApiGen_Concepts_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = model.id;
    newForm.parentAcquisitionFileId = model.parentAcquisitionFileId;
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

    return newForm;
  }
}

export const isAcquisitionFile = (file: unknown): file is ApiGen_Concepts_AcquisitionFile =>
  !!file && Object.prototype.hasOwnProperty.call(file, 'acquisitionTypeCode');
