import {
  Api_DispositionFile,
  Api_DispositionFileProperty,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { emptyStringtoNullable, toTypeCode, toTypeCodeNullable } from '@/utils/formUtils';

import { PropertyForm } from '../../shared/models';
import { DispositionOfferFormModel } from './DispositionOfferFormModel';
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
  sales: DispositionSaleFormModel[] = [];
  // Appraisal and Value
  appraisedValueAmount: number | null = null;
  appraisalDate: string | null = null;
  bcaValueAmount: number | null = null;
  bcaRollYear: string | null = null;
  listPriceAmount: number | null = null;

  constructor(
    readonly id: number | null = null,
    readonly fileNumber: string | null = null,
    dispositionFileStatus: string = 'ACTIVE',
    dispositionStatus: string = 'UNKNOWN',
  ) {
    this.id = id;
    this.fileNumber = fileNumber;
    this.fileStatusTypeCode = dispositionFileStatus;
    this.dispositionStatusTypeCode = dispositionStatus;
  }

  toApi(): Api_DispositionFile {
    return {
      id: this.id ?? undefined,
      fileName: this.fileName ?? undefined,
      fileNumber: this.fileNumber ?? undefined,
      fileStatusTypeCode: toTypeCode(this.fileStatusTypeCode),
      fileReference: emptyStringtoNullable(this.referenceNumber),
      assignedDate: this.assignedDate,
      completionDate: this.completionDate,
      dispositionTypeCode: toTypeCodeNullable(this.dispositionTypeCode),
      dispositionTypeOther: this.dispositionTypeOther ? this.dispositionTypeOther : null,
      dispositionStatusTypeCode: toTypeCodeNullable(this.dispositionStatusTypeCode),
      initiatingBranchTypeCode: toTypeCode(this.initiatingBranchTypeCode),
      physicalFileStatusTypeCode: toTypeCode(this.physicalFileStatusTypeCode),
      fundingTypeCode: toTypeCodeNullable(this.fundingTypeCode),
      initiatingDocumentTypeCode: toTypeCode(this.initiatingDocumentTypeCode),
      initiatingDocumentTypeOther: this.initiatingDocumentTypeOther
        ? this.initiatingDocumentTypeOther
        : null,
      initiatingDocumentDate: this.initiatingDocumentDate,
      regionCode: toTypeCodeNullable(Number(this.regionCode)),
      dispositionTeam: this.team
        .filter(x => !!x.contact && !!x.teamProfileTypeCode)
        .map(x => x.toApi(this.id || 0))
        .filter((x): x is Api_DispositionFileTeam => x !== null),
      fileProperties: this.fileProperties.map<Api_DispositionFileProperty>(ap => {
        return {
          id: ap.id,
          propertyName: ap.name,
          displayOrder: ap.displayOrder,
          rowVersion: ap.rowVersion,
          property: ap.toApi(),
          propertyId: ap.apiId,
          acquisitionFile: { id: this.id },
        };
      }),

      offers: this.offers.map(x => x.toApi()),
      sales: this.sales.map(x => x.toApi()),
      project: null,
      projectId: null,
      product: null,
      productId: null,
      // Appraisal and Value
      appraisedValueAmount: this.appraisedValueAmount,
      appraisalDate: this.appraisalDate,
      bcaValueAmount: this.bcaValueAmount,
      bcaRollYear: this.bcaRollYear,
      listPriceAmount: this.listPriceAmount,
    };
  }
}
