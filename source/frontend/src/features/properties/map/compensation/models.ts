import {
  Api_CompensationFinancial,
  Api_CompensationRequisition,
} from 'models/api/CompensationRequisition';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { booleanToString, stringToBoolean, stringToNull, toTypeCode } from 'utils/formUtils';

export class CompensationRequisitionFormModel {
  id: number | null = null;
  acquisitionFileId: number;
  status: string = '' || 'draft' || 'final';
  fiscalYear: string = '';
  stob: string = '';
  yearlyFinancial: Api_FinancialCode | null = null;
  serviceLine: string = '';
  chartOfAccounts: Api_FinancialCode | null = null;
  responsibilityCentre: string = '';
  Responsibility: Api_FinancialCode | null = null;
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  financials: FinacialActivityFormModel[] = [];
  isDisabled: string = '';
  rowVersion: number | null = null;

  constructor(id: number | null, acquisitionFileId: number = 0) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(): Api_CompensationRequisition {
    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      isDraft: this.status === 'draft' ? true : false,
      fiscalYear: stringToNull(this.fiscalYear),
      yearlyFinancialId: +this.stob ?? null,
      yearlyFinancial: null,
      chartOfAccountsId: +this.serviceLine ?? null,
      chartOfAccounts: null,
      responsibilityId: +this.responsibilityCentre ?? null,
      responsibility: null,
      agreementDate: stringToNull(this.agreementDateTime),
      expropriationNoticeServedDate: stringToNull(this.expropriationNoticeServedDateTime),
      expropriationVestingDate: stringToNull(this.expropriationVestingDateTime),
      generationDate: stringToNull(this.generationDatetTime),
      specialInstruction: stringToNull(this.specialInstruction),
      detailedRemarks: stringToNull(this.detailedRemarks),
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
    );

    compensation.status =
      apiModel.isDraft === true ? 'draft' : apiModel.isDraft === null ? '' : 'final';
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.stob = apiModel.yearlyFinancialId?.toString() || '';
    compensation.yearlyFinancial = apiModel.yearlyFinancial;
    compensation.serviceLine = apiModel.chartOfAccountsId?.toString() || '';
    compensation.chartOfAccounts = apiModel.chartOfAccounts;
    compensation.responsibilityCentre = apiModel.responsibilityId?.toString() || '';
    compensation.Responsibility = apiModel.responsibility;
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.financials =
      apiModel.financials?.map(x => FinacialActivityFormModel.fromApi(x)) || [];
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
    compensation.rowVersion = apiModel.rowVersion ?? null;

    return compensation;
  }
}

export class FinacialActivityFormModel {
  id: number | null = null;
  compensationRequisitionId: number = 0;
  financialActivityCodeId: string = '';
  financialActivityCode: Api_FinancialCode | null = null;
  pretaxAmount: number = 0;
  isGstRequired: string = '';
  taxAmount: number = 0;
  totalAmount: number = 0;
  rowVersion: number | null = null;
  isDisabled: string = '';

  isEmpty(): boolean {
    return this.financialActivityCode === null && this.pretaxAmount === 0;
  }

  toApi(): Api_CompensationFinancial {
    return {
      id: this.id,
      compensationId: this.compensationRequisitionId,
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

  static fromApi(model: Api_CompensationFinancial): FinacialActivityFormModel {
    const newForm = new FinacialActivityFormModel();
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
