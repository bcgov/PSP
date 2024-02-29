import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { stringToNull, stringToNumber, toTypeCodeNullable } from '@/utils/formUtils';

export class AcquisitionAgreementFormModel {
  public agreementTypeCode: string | null = null;
  public agreementTypeDescription: string | null = null;
  public agreementDate: string | null = null;
  public completionDate: string | null = null;
  public terminationDate: string | null = null;
  public commencementDate: string | null = null;
  public possessionDate: string | null = null;
  public depositAmount: string | null = null;
  public noLaterThanDays: string | null = null;
  public purchasePrice: string | null = null;
  public legalSurveyPlanNum: string | null = null;
  public offerDate: string | null = null;
  public expiryDateTime: string | null = null;
  public signedDate: string | null = null;
  public inspectionDate: string | null = null;
  public agreementStatusTypeCode = 'DRAFT';
  public agreementStatusTypeDescription: string | null = null;
  public cancellationNote: string | null = null;
  public isDraft: boolean | null = null;

  constructor(
    readonly acquisitionFileId: number,
    readonly agreementId: number = 0,
    readonly rowVersion: number | null = null,
  ) {
    this.agreementId = agreementId;
    this.acquisitionFileId = acquisitionFileId;
    this.rowVersion = rowVersion;
  }

  static fromApi(apiModel: ApiGen_Concepts_Agreement): AcquisitionAgreementFormModel {
    const agreement = new AcquisitionAgreementFormModel(
      apiModel.acquisitionFileId,
      apiModel.agreementId,
      apiModel.rowVersion,
    );

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
    agreement.cancellationNote = apiModel.cancellationNote || '';
    agreement.isDraft = apiModel.isDraft;

    return agreement;
  }

  public toApi(): ApiGen_Concepts_Agreement {
    return {
      agreementId: this.agreementId,
      acquisitionFileId: this.acquisitionFileId,
      agreementType: toTypeCodeNullable(this.agreementTypeCode),
      agreementDate: stringToNull(this.agreementDate),
      agreementStatusType: toTypeCodeNullable(this.agreementStatusTypeCode),
      completionDate: stringToNull(this.completionDate),
      terminationDate: stringToNull(this.terminationDate),
      commencementDate: stringToNull(this.commencementDate),
      possessionDate: stringToNull(this.possessionDate),
      depositAmount: this.depositAmount !== '' ? Number(this.depositAmount) : null,
      noLaterThanDays: stringToNumber(this.noLaterThanDays),
      purchasePrice: stringToNumber(this.purchasePrice),
      legalSurveyPlanNum: stringToNull(this.legalSurveyPlanNum),
      offerDate: stringToNull(this.offerDate),
      expiryDateTime: stringToNull(this.expiryDateTime),
      signedDate: stringToNull(this.signedDate),
      inspectionDate: stringToNull(this.inspectionDate),
      cancellationNote: stringToNull(this.cancellationNote),
      isDraft: this.isDraft,
      rowVersion: this.rowVersion,
    };
  }
}
