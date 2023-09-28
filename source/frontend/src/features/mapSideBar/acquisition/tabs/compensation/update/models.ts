import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { booleanToString, stringToBoolean, stringToUndefined, toTypeCode } from '@/utils/formUtils';

export class CompensationRequisitionFormModel {
  id: number | null;
  acquisitionFileId: number;
  status: string = '';
  fiscalYear: string = '';
  stob: string = '';
  yearlyFinancial: Api_FinancialCode | null = null;
  serviceLine: string = '';
  chartOfAccounts: Api_FinancialCode | null = null;
  responsibilityCentre: string = '';
  Responsibility: Api_FinancialCode | null = null;
  readonly finalizedDate: string;
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  advancedPaymentServedDate: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  financials: FinancialActivityFormModel[] = [];
  payee: AcquisitionPayeeFormModel;
  alternateProject: IAutocompletePrediction | null = null;
  isDisabled: string = '';
  rowVersion: number | null = null;

  constructor(id: number | null, acquisitionFileId: number = 0, finalizedDate: string) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
    this.payee = new AcquisitionPayeeFormModel();
    this.finalizedDate = finalizedDate;
  }

  toApi(payeeOptions: PayeeOption[]): Api_CompensationRequisition {
    let modelWithPayeeInformation = this.payee.toApi(payeeOptions);

    return {
      ...modelWithPayeeInformation,
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      alternateProjectId:
        this.alternateProject?.id !== undefined && this.alternateProject?.id !== 0
          ? this.alternateProject?.id
          : null,
      isDraft: this.status === 'draft' ? true : false,
      fiscalYear: stringToUndefined(this.fiscalYear),
      yearlyFinancialId: this.stob === '' ? null : Number(this.stob),
      yearlyFinancial: null,
      chartOfAccountsId: this.serviceLine === '' ? null : Number(this.serviceLine),
      chartOfAccounts: null,
      responsibilityId: this.responsibilityCentre === '' ? null : Number(this.responsibilityCentre),
      responsibility: null,
      agreementDate: stringToUndefined(this.agreementDateTime),
      finalizedDate: stringToUndefined(this.finalizedDate),
      expropriationNoticeServedDate: stringToUndefined(this.expropriationNoticeServedDateTime),
      expropriationVestingDate: stringToUndefined(this.expropriationVestingDateTime),
      advancedPaymentServedDate: stringToUndefined(this.advancedPaymentServedDate),
      generationDate: stringToUndefined(this.generationDatetTime),
      specialInstruction: stringToUndefined(this.specialInstruction),
      detailedRemarks: stringToUndefined(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: this.financials
        .filter(x => !x.isEmpty())
        .map<Api_CompensationFinancial>(x => x.toApi()),
      rowVersion: this.rowVersion ?? undefined,
    };
  }

  static fromApi(apiModel: Api_CompensationRequisition): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
      apiModel.finalizedDate ?? '',
    );

    compensation.status =
      apiModel.isDraft === true ? 'draft' : apiModel.isDraft === null ? '' : 'final';
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.stob = apiModel.yearlyFinancialId?.toString() || '';
    compensation.yearlyFinancial = apiModel.yearlyFinancial;
    compensation.serviceLine = apiModel.chartOfAccountsId?.toString() || '';
    compensation.chartOfAccounts = apiModel.chartOfAccounts;
    compensation.responsibilityCentre = apiModel.responsibilityId?.toString() || '';
    compensation.alternateProject =
      apiModel.alternateProject !== null
        ? {
            id: apiModel.alternateProject?.id || 0,
            text: apiModel.alternateProject?.description || '',
          }
        : null;
    compensation.Responsibility = apiModel.responsibility;
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.advancedPaymentServedDate = apiModel.advancedPaymentServedDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
    compensation.financials =
      apiModel.financials?.map(x => FinancialActivityFormModel.fromApi(x)) || [];

    compensation.rowVersion = apiModel.rowVersion ?? null;

    const payeePretaxAmount = apiModel?.financials
      .map(f => f.pretaxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const payeeTaxAmount = apiModel?.financials
      .map(f => f.taxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const payeeTotalAmount = apiModel?.financials
      .map(f => f.totalAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    compensation.payee = AcquisitionPayeeFormModel.fromApi(apiModel);
    compensation.payee.pretaxAmount = payeePretaxAmount;
    compensation.payee.taxAmount = payeeTaxAmount;
    compensation.payee.totalAmount = payeeTotalAmount;

    return compensation;
  }
}

export class FinancialActivityFormModel {
  readonly _id: number | null = null;
  readonly _compensationRequisitionId: number;

  financialActivityCodeId: string = '';
  financialActivityCode: Api_FinancialCode | null = null;
  pretaxAmount: number = 0;
  isGstRequired: string = '';
  taxAmount: number = 0;
  totalAmount: number = 0;
  rowVersion: number | null = null;
  isDisabled: string = '';

  constructor(id: number | null = null, compensationRequisitionId: number) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  isEmpty(): boolean {
    return this.financialActivityCode === null && this.pretaxAmount === 0;
  }

  toApi(): Api_CompensationFinancial {
    return {
      id: this._id,
      compensationId: this._compensationRequisitionId,
      financialActivityCodeId: +this.financialActivityCodeId,
      financialActivityCode: this.financialActivityCode
        ? toTypeCode<number>(+this.financialActivityCode) ?? null
        : null,
      pretaxAmount: this.pretaxAmount,
      isGstRequired: stringToBoolean(this.isGstRequired),
      taxAmount: this.taxAmount,
      totalAmount: this.totalAmount,
      rowVersion: this.rowVersion ?? null,
      isDisabled: stringToBoolean(this.isDisabled),
    };
  }

  static fromApi(model: Api_CompensationFinancial): FinancialActivityFormModel {
    const newForm = new FinancialActivityFormModel(model.id, model.compensationId);
    newForm.pretaxAmount = model.pretaxAmount ?? 0;
    newForm.isGstRequired = booleanToString(model.isGstRequired);
    newForm.taxAmount = model.taxAmount ?? 0;
    newForm.totalAmount = model.totalAmount ?? 0;
    newForm.financialActivityCodeId = model.financialActivityCodeId.toString() ?? '';
    newForm.financialActivityCode = model.financialActivityCode;
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.isDisabled = booleanToString(model.isDisabled);

    return newForm;
  }
}

export class AcquisitionPayeeFormModel {
  payeeKey: string = '';
  gstNumber: string = '';
  legacyPayee: string = '';
  isPaymentInTrust: boolean = false;

  pretaxAmount: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;

  static fromApi(apiModel: Api_CompensationRequisition): AcquisitionPayeeFormModel {
    const payeeModel = new AcquisitionPayeeFormModel();

    payeeModel.payeeKey = PayeeOption.fromApi(apiModel);
    payeeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeModel.gstNumber = apiModel.gstNumber ?? '';
    payeeModel.legacyPayee = apiModel.legacyPayee ?? '';

    return payeeModel;
  }

  toApi(payeeOptions: PayeeOption[]): Api_CompensationRequisition {
    let modelWithPayeeInformation: Api_CompensationRequisition = PayeeOption.toApi(
      this.payeeKey,
      payeeOptions,
    );

    return {
      ...modelWithPayeeInformation,
      legacyPayee: stringToUndefined(this.legacyPayee),
      isPaymentInTrust: stringToBoolean(this.isPaymentInTrust),
      gstNumber: this.gstNumber,
    };
  }
}
