import { Api_Agreement } from 'models/api/Agreement';
import { booleanToString, stringToBooleanOrNull, stringToNull, toTypeCode } from 'utils/formUtils';

export class SingleAgreementFormModel {
  public agreementId: number = 0;

  public agreementTypeCode: string = '';
  public agreementTypeDescription: string = '';
  public agreementDate: string = '';
  public agreementStatus: string = '';
  public completionDate: string = '';
  public terminationDate: string = '';
  public commencementDate: string = '';
  public depositAmount: string = '';
  public noLaterThanDays: string = '';
  public purchasePrice: string = '';
  public legalSurveyPlanNum: string = '';
  public offerDate: string = '';
  public expiryDateTime: string = '';
  public signedDate: string = '';
  public inspectionDate: string = '';

  public rowVersion: number | null = null;

  static fromApi(apiModel: Api_Agreement): SingleAgreementFormModel {
    const agreement = new SingleAgreementFormModel();

    agreement.agreementId = apiModel.agreementId;
    agreement.agreementTypeCode = apiModel.agreementType.id || '';
    agreement.agreementTypeDescription = apiModel.agreementType.description || '';
    agreement.agreementDate = apiModel.agreementDate || '';
    agreement.agreementStatus =
      apiModel.agreementStatus !== null ? booleanToString(apiModel.agreementStatus) : '';
    agreement.completionDate = apiModel.completionDate || '';
    agreement.terminationDate = apiModel.terminationDate || '';
    agreement.commencementDate = apiModel.commencementDate || '';
    agreement.depositAmount = apiModel.depositAmount?.toString() || '';
    agreement.noLaterThanDays = apiModel.noLaterThanDays?.toString() || '';
    agreement.purchasePrice = apiModel.purchasePrice?.toString() || '';
    agreement.legalSurveyPlanNum = apiModel.legalSurveyPlanNum || '';
    agreement.offerDate = apiModel.offerDate || '';
    agreement.expiryDateTime = apiModel.expiryDateTime || '';
    agreement.signedDate = apiModel.signedDate || '';
    agreement.inspectionDate = apiModel.inspectionDate || '';
    agreement.rowVersion = apiModel.rowVersion || null;

    return agreement;
  }

  public toApi(acquisitionFileId: number): Api_Agreement {
    return {
      agreementId: this.agreementId,
      acquisitionFileId: acquisitionFileId,
      agreementType: toTypeCode(this.agreementTypeCode) || {},
      agreementDate: stringToNull(this.agreementDate),
      agreementStatus: stringToBooleanOrNull(this.agreementStatus),
      completionDate: stringToNull(this.completionDate),
      terminationDate: stringToNull(this.terminationDate),
      commencementDate: stringToNull(this.commencementDate),
      depositAmount: this.depositAmount !== '' ? Number(this.depositAmount) : null,
      noLaterThanDays: stringToNull(this.noLaterThanDays),
      purchasePrice: stringToNull(this.purchasePrice),
      legalSurveyPlanNum: stringToNull(this.legalSurveyPlanNum),
      offerDate: stringToNull(this.offerDate),
      expiryDateTime: stringToNull(this.expiryDateTime),
      signedDate: stringToNull(this.signedDate),
      inspectionDate: stringToNull(this.inspectionDate),
      rowVersion: this.rowVersion ?? undefined,
    };
  }
}

export class AgreementsFormModel {
  acquisitionFileId: number;
  agreements: SingleAgreementFormModel[] = [];

  public constructor(acquisitionFileId: number) {
    this.acquisitionFileId = acquisitionFileId;
  }

  static fromApi(acquisitionFileId: number, agreements: Api_Agreement[]): AgreementsFormModel {
    const newFormModel = new AgreementsFormModel(acquisitionFileId);
    agreements.forEach(x => newFormModel.agreements.push(SingleAgreementFormModel.fromApi(x)));
    return newFormModel;
  }

  public toApi(): Api_Agreement[] {
    return this.agreements.map(x => x.toApi(this.acquisitionFileId));
  }
}
