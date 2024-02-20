import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileProperty } from '@/models/api/generated/ApiGen_Concepts_DispositionFileProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { emptyStringtoNullable, fromTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import { exists, isValidIsoDateTime } from '@/utils/utils';

import { PropertyForm } from '../../shared/models';
import { ChecklistItemFormModel } from '../../shared/tabs/checklist/update/models';
import { DispositionOfferFormModel } from '../tabs/offersAndSale/dispositionOffer/models/DispositionOfferFormModel';
import { DispositionAppraisalFormModel } from './DispositionAppraisalFormModel';
import { DispositionSaleFormModel } from './DispositionSaleFormModel';
import { DispositionTeamSubFormModel, WithDispositionTeam } from './DispositionTeamSubFormModel';

export class DispositionFormModel implements WithDispositionTeam {
  fileName: string | null = '';
  fileStatusTypeCode: string | null = null;
  referenceNumber: string | null = '';
  assignedDate: string | null = null;
  completionDate: string | null = null;
  dispositionTypeCode: string | null = null;
  dispositionTypeOther: string | null = '';
  dispositionStatusTypeCode: string | null = null;
  initiatingBranchTypeCode: string | null = null;
  physicalFileStatusTypeCode: string | null = null;
  fundingTypeCode: string | null = null;
  initiatingDocumentTypeCode: string | null = null;
  initiatingDocumentTypeOther: string | null = '';
  initiatingDocumentDate: string | null = null;
  regionCode: string | null = '';
  fileProperties: PropertyForm[] = [];
  team: DispositionTeamSubFormModel[] = [];
  offers: DispositionOfferFormModel[] = [];
  fileChecklist: ChecklistItemFormModel[] = [];
  sale: DispositionSaleFormModel | null = null;
  appraisal: DispositionAppraisalFormModel | null = null;

  constructor(
    readonly id: number | null = null,
    readonly fileNumber: string | null = null,
    readonly rowVersion: number | null = null,
    dispositionFileStatus = 'ACTIVE',
    dispositionStatus = 'UNKNOWN',
  ) {
    this.id = id;
    this.fileNumber = fileNumber;
    this.fileStatusTypeCode = dispositionFileStatus;
    this.dispositionStatusTypeCode = dispositionStatus;
  }

  toApi(): ApiGen_Concepts_DispositionFile {
    return {
      id: this.id ?? 0,
      fileName: this.fileName ?? null,
      fileNumber: this.fileNumber ?? null,
      fileStatusTypeCode: toTypeCodeNullable(this.fileStatusTypeCode),
      fileReference: emptyStringtoNullable(this.referenceNumber),
      assignedDate: isValidIsoDateTime(this.assignedDate) ? this.assignedDate : this.assignedDate,
      completionDate: isValidIsoDateTime(this.completionDate)
        ? this.completionDate
        : this.completionDate,
      dispositionTypeCode: toTypeCodeNullable(this.dispositionTypeCode),
      dispositionTypeOther: this.dispositionTypeOther ? this.dispositionTypeOther : null,
      dispositionStatusTypeCode: toTypeCodeNullable(this.dispositionStatusTypeCode),
      initiatingBranchTypeCode: toTypeCodeNullable(this.initiatingBranchTypeCode),
      physicalFileStatusTypeCode: toTypeCodeNullable(this.physicalFileStatusTypeCode),
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      initiatingDocumentTypeCode: toTypeCodeNullable(this.initiatingDocumentTypeCode),
      initiatingDocumentTypeOther: this.initiatingDocumentTypeOther
        ? this.initiatingDocumentTypeOther
        : null,
      initiatingDocumentDate: isValidIsoDateTime(this.initiatingDocumentDate)
        ? this.initiatingDocumentDate
        : null,
      regionCode: toTypeCodeNullable(Number(this.regionCode)),
      dispositionTeam: this.team
        .filter(x => !!x.contact && !!x.teamProfileTypeCode)
        .map(x => x.toApi(this.id || 0))
        .filter(exists),
      fileProperties: this.fileProperties.map<ApiGen_Concepts_DispositionFileProperty>(ap => ({
        id: ap.id ?? 0,
        propertyName: ap.name ?? null,
        displayOrder: ap.displayOrder ?? null,
        rowVersion: ap.rowVersion ?? null,
        property: ap.toApi(),
        propertyId: ap.apiId ?? 0,
        file: null,
        fileId: 0,
      })),

      dispositionOffers: this.offers.map(x => x.toApi()),
      dispositionSale: this.sale ? this.sale.toApi() : null,
      dispositionAppraisal: this.appraisal ? this.appraisal.toApi() : null,
      fileChecklistItems: this.fileChecklist.map(x => x.toApi()),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_DispositionFile): DispositionFormModel {
    const dispositionForm = new DispositionFormModel(
      model.id,
      model.fileNumber,
      model.rowVersion,
      model.fileStatusTypeCode?.id ?? undefined,
      model.dispositionStatusTypeCode?.id ?? undefined,
    );

    dispositionForm.fundingTypeCode = fromTypeCode(model.fundingTypeCode) ?? '';
    dispositionForm.fileName = model.fileName ?? '';
    dispositionForm.referenceNumber = model.fileReference;
    dispositionForm.assignedDate = model.assignedDate;
    dispositionForm.completionDate = model.completionDate;
    dispositionForm.dispositionTypeCode = fromTypeCode(model.dispositionTypeCode) ?? '';
    dispositionForm.dispositionTypeOther = model.dispositionTypeOther;
    dispositionForm.initiatingBranchTypeCode = fromTypeCode(model.initiatingBranchTypeCode) ?? '';
    dispositionForm.physicalFileStatusTypeCode =
      fromTypeCode(model.physicalFileStatusTypeCode) ?? '';
    dispositionForm.initiatingDocumentTypeCode =
      fromTypeCode(model.initiatingDocumentTypeCode) ?? '';
    dispositionForm.initiatingDocumentTypeOther = model.initiatingDocumentTypeOther;
    dispositionForm.initiatingDocumentDate = model.initiatingDocumentDate;
    dispositionForm.regionCode = fromTypeCode(model.regionCode)?.toString() ?? '';

    dispositionForm.team =
      model.dispositionTeam?.map(x => DispositionTeamSubFormModel.fromApi(x)) || [];
    dispositionForm.fileProperties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];

    return dispositionForm;
  }
}
