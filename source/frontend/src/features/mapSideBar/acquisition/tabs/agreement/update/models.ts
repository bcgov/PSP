import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { isValidIsoDateTime } from '@/utils';
import { stringToNull, stringToNumberOrNull, toTypeCodeNullable } from '@/utils/formUtils';

export class SingleAgreementFormModel {
  public agreementId = 0;

  public agreementTypeCode = '';
  public agreementTypeDescription = '';
  public agreementDate = '';
  public completionDate = '';
  public terminationDate = '';
  public commencementDate = '';
  public possessionDate = '';
  public depositAmount = '';
  public noLaterThanDays = '';
  public purchasePrice = '';
  public legalSurveyPlanNum = '';
  public offerDate = '';
  public expiryDateTime = '';
  public signedDate = '';
  public inspectionDate = '';
  public agreementStatusTypeCode = 'DRAFT';
  public agreementStatusTypeDescription = '';
  public cancellationNote = '';

  public rowVersion: number | null = null;

  static fromApi(apiModel: ApiGen_Concepts_Agreement): SingleAgreementFormModel {
    const agreement = new SingleAgreementFormModel();

    agreement.agreementId = apiModel.agreementId;
    agreement.agreementTypeCode = apiModel.agreementType?.id || '';
    agreement.agreementTypeDescription = apiModel.agreementType?.description || '';
    agreement.agreementDate = apiModel.agreementDate || '';
    agreement.agreementStatusTypeCode = apiModel.agreementStatusType?.id || '';
    agreement.agreementStatusTypeDescription = apiModel.agreementStatusType?.description || '';
    agreement.completionDate = apiModel.completionDate || '';
    agreement.terminationDate = apiModel.terminationDate || '';
    agreement.commencementDate = apiModel.commencementDate || '';
    agreement.possessionDate = apiModel.possessionDate || '';
    agreement.depositAmount = apiModel.depositAmount?.toString() || '';
    agreement.noLaterThanDays = apiModel.noLaterThanDays?.toString() || '';
    agreement.purchasePrice = apiModel.purchasePrice?.toString() || '';
    agreement.legalSurveyPlanNum = apiModel.legalSurveyPlanNum || '';
    agreement.offerDate = apiModel.offerDate || '';
    agreement.expiryDateTime = apiModel.expiryDateTime || '';
    agreement.signedDate = apiModel.signedDate || '';
    agreement.inspectionDate = apiModel.inspectionDate || '';
    agreement.rowVersion = apiModel.rowVersion || null;
    agreement.cancellationNote = apiModel.cancellationNote || '';

    return agreement;
  }

  public toApi(acquisitionFileId: number): ApiGen_Concepts_Agreement {
    return {
      agreementId: this.agreementId,
      acquisitionFileId: acquisitionFileId,
      agreementType: toTypeCodeNullable(this.agreementTypeCode) || {
        id: null,
        description: null,
        displayOrder: null,
        isDisabled: false,
      },
      agreementDate: isValidIsoDateTime(this.agreementDate) ? this.agreementDate : null,
      agreementStatusType: toTypeCodeNullable(this.agreementStatusTypeCode) || {
        id: null,
        description: null,
        displayOrder: null,
        isDisabled: false,
      },
      completionDate: isValidIsoDateTime(this.completionDate) ? this.completionDate : null,
      terminationDate: isValidIsoDateTime(this.terminationDate) ? this.terminationDate : null,
      commencementDate: isValidIsoDateTime(this.commencementDate) ? this.commencementDate : null,
      possessionDate: isValidIsoDateTime(this.possessionDate) ? this.possessionDate : null,
      depositAmount: this.depositAmount !== '' ? Number(this.depositAmount) : null,
      noLaterThanDays: stringToNumberOrNull(this.noLaterThanDays),
      purchasePrice: stringToNumberOrNull(this.purchasePrice),
      legalSurveyPlanNum: stringToNull(this.legalSurveyPlanNum),
      offerDate: isValidIsoDateTime(this.offerDate) ? this.offerDate : null,
      expiryDateTime: isValidIsoDateTime(this.expiryDateTime) ? this.expiryDateTime : null,
      signedDate: isValidIsoDateTime(this.signedDate) ? this.signedDate : null,
      inspectionDate: isValidIsoDateTime(this.inspectionDate) ? this.inspectionDate : null,
      cancellationNote: stringToNull(this.cancellationNote),
      isDraft: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}

export class AgreementsFormModel {
  acquisitionFileId: number;
  agreements: SingleAgreementFormModel[] = [];

  public constructor(acquisitionFileId: number) {
    this.acquisitionFileId = acquisitionFileId;
  }

  static fromApi(
    acquisitionFileId: number,
    agreements: ApiGen_Concepts_Agreement[],
  ): AgreementsFormModel {
    const newFormModel = new AgreementsFormModel(acquisitionFileId);
    agreements.forEach(x => newFormModel.agreements.push(SingleAgreementFormModel.fromApi(x)));
    return newFormModel;
  }

  public toApi(): ApiGen_Concepts_Agreement[] {
    return this.agreements.map(x => x.toApi(this.acquisitionFileId));
  }
}

export const isAcquisitionFile = (file: unknown): file is ApiGen_Concepts_AcquisitionFile =>
  !!file && Object.prototype.hasOwnProperty.call(file, 'acquisitionTypeCode');
