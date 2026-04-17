import { first } from 'lodash';

import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_NoticeOfClaim } from '@/models/api/generated/ApiGen_Concepts_NoticeOfClaim';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { applyDisplayOrder } from '@/utils';
import { fromTypeCode, toNullableId, toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidId, isValidString } from '@/utils/utils';

import { PropertyForm } from '../../shared/models';
import { ManagementTeamSubFormModel, WithManagementTeam } from './ManagementTeamSubFormModel';

export class ManagementFormModel implements WithManagementTeam {
  fileName: string | null = '';
  additionalDetails: string | null = '';
  filePurpose: string | null = '';
  legacyFileNum: string | null = '';
  fileStatusTypeCode: string | null = null;
  regionCode: string | null = null;
  project: IAutocompletePrediction | null = null;
  productId: string | null = null;
  fundingTypeCode: string | null = null;
  purposeTypeCode: string | null = null;
  responsiblePayer: IContactSearchResult | null = null;
  responsiblePayerPrimaryContactId: number | null = null;
  fileProperties: PropertyForm[] = [];
  team: ManagementTeamSubFormModel[] = [];
  noticeOfClaim: ApiGen_Concepts_NoticeOfClaim;

  constructor(
    readonly id: number | null = null,
    readonly fileNumber: string | null = null,
    readonly rowVersion: number | null = null,
    managementFileStatus = 'ACTIVE',
  ) {
    this.id = id;
    this.fileNumber = fileNumber;
    this.fileStatusTypeCode = managementFileStatus;
  }

  toApi(): ApiGen_Concepts_ManagementFile {
    const fileProperties = this.fileProperties.map(x => this.toPropertyApi(x));
    const sortedProperties = applyDisplayOrder(fileProperties);
    const personId = this.responsiblePayer?.personId ?? null;
    const organizationId = !personId ? this.responsiblePayer?.organizationId ?? null : null;
    const noticeOfClaim: ApiGen_Concepts_NoticeOfClaim | null = this.noticeOfClaim
      ? {
          ...this.noticeOfClaim,
          receivedDate: isValidString(this.noticeOfClaim?.receivedDate)
            ? this.noticeOfClaim.receivedDate
            : null,
          comment: isValidString(this.noticeOfClaim?.comment?.trim())
            ? this.noticeOfClaim.comment.trim()
            : null,
        }
      : null;
    const hasNoticeOfClaim =
      isValidString(noticeOfClaim?.receivedDate) || isValidString(noticeOfClaim?.comment);

    return {
      id: this.id ?? 0,
      fileName: this.fileName ?? null,
      additionalDetails: this.additionalDetails ?? null,
      filePurpose: this.filePurpose ?? null,
      fileNumber: this.fileNumber ?? null,
      legacyFileNum: this.legacyFileNum ?? null,
      regionCode: exists(this.regionCode) ? toTypeCodeNullable(Number(this.regionCode)) : null,
      fileStatusTypeCode: toTypeCodeNullable(this.fileStatusTypeCode),
      totalAllowableCompensation: null,
      project: null,
      projectId: this.project?.id !== undefined && this.project?.id !== 0 ? this.project?.id : null,
      product: null,
      productId: this.productId ? Number(this.productId) : null,
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      purposeTypeCode: toTypeCodeNullable(this.purposeTypeCode),
      responsiblePayerPersonId: toNullableId(personId),
      responsiblePayerPerson: null,
      responsiblePayerOrganizationId: toNullableId(organizationId),
      responsiblePayerOrganization: null,
      responsiblePayerPrimaryContactId: isValidId(personId)
        ? null
        : toNullableId(this.responsiblePayerPrimaryContactId),
      responsiblePayerPrimaryContact: null,
      managementTeam: this.team
        .filter(x => !!x.contact && !!x.teamProfileTypeCode)
        .map(x => x.toApi(this.id || 0))
        .filter(exists),
      fileProperties: sortedProperties ?? [],
      ...getEmptyBaseAudit(this.rowVersion),
      noticeOfClaim: hasNoticeOfClaim && noticeOfClaim ? [noticeOfClaim] : [],
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  private toPropertyApi(x: PropertyForm): ApiGen_Concepts_ManagementFileProperty {
    const apiFileProperty = x.toFilePropertyApi(this.id);
    return {
      ...apiFileProperty,
      file: null,
    };
  }

  static fromApi(model: ApiGen_Concepts_ManagementFile): ManagementFormModel {
    const managementForm = new ManagementFormModel(
      model.id,
      model.fileNumber,
      model.rowVersion,
      model.fileStatusTypeCode?.id ?? undefined,
    );

    managementForm.additionalDetails = model.additionalDetails ?? '';
    managementForm.filePurpose = model.filePurpose ?? '';
    managementForm.project = model.project
      ? { id: model.project?.id || 0, text: model.project?.description || '' }
      : null;
    managementForm.productId = model.product?.id?.toString() ?? '';
    managementForm.fundingTypeCode = fromTypeCode(model.fundingTypeCode) ?? '';
    managementForm.purposeTypeCode = fromTypeCode(model.purposeTypeCode) ?? '';
    managementForm.fileName = model.fileName ?? '';
    managementForm.regionCode = fromTypeCode(model.regionCode)?.toString() ?? '';
    managementForm.team =
      model.managementTeam?.map(x => ManagementTeamSubFormModel.fromApi(x)) || [];
    managementForm.fileProperties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    managementForm.noticeOfClaim = exists(model.noticeOfClaim) ? first(model.noticeOfClaim) : null;

    const contact: IContactSearchResult | undefined = exists(model.responsiblePayerPerson)
      ? fromApiPerson(model.responsiblePayerPerson)
      : exists(model.responsiblePayerOrganization)
      ? fromApiOrganization(model.responsiblePayerOrganization)
      : undefined;

    managementForm.responsiblePayerPrimaryContactId = model.responsiblePayerPrimaryContactId;

    managementForm.responsiblePayer = contact;

    return managementForm;
  }
}
