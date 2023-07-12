import { Api_Agreement } from '@/models/api/Agreement';
import {
  booleanToString,
  stringToBooleanOrNull,
  stringToUndefined,
  toTypeCode,
} from '@/utils/formUtils';

export class SingleAgreementFormModel {
  public agreementId: number = 0;

  public agreementTypeCode: string = '';
  public agreementTypeDescription: string = '';
  public agreementDate: string = '';
  public isDraft: string = '';
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
    agreement.isDraft = apiModel.isDraft !== null ? booleanToString(apiModel.isDraft) : '';
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
      agreementDate: stringToUndefined(this.agreementDate),
      isDraft: stringToBooleanOrNull(this.isDraft),
      completionDate: stringToUndefined(this.completionDate),
      terminationDate: stringToUndefined(this.terminationDate),
      commencementDate: stringToUndefined(this.commencementDate),
      depositAmount: this.depositAmount !== '' ? Number(this.depositAmount) : null,
      noLaterThanDays: stringToUndefined(this.noLaterThanDays),
      purchasePrice: stringToUndefined(this.purchasePrice),
      legalSurveyPlanNum: stringToUndefined(this.legalSurveyPlanNum),
      offerDate: stringToUndefined(this.offerDate),
      expiryDateTime: stringToUndefined(this.expiryDateTime),
      signedDate: stringToUndefined(this.signedDate),
      inspectionDate: stringToUndefined(this.inspectionDate),
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
