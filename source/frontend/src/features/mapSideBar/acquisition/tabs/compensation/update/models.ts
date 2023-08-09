import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { booleanToString, stringToBoolean, stringToUndefined, toTypeCode } from '@/utils/formUtils';

import { PayeeOption } from '../../../models/PayeeOption';

export class CompensationRequisitionFormModel {
  id: number | null = null;
  acquisitionFileId: number;
  status: string = '';
  fiscalYear: string = '';
  stob: string = '';
  yearlyFinancial: Api_FinancialCode | null = null;
  serviceLine: string = '';
  chartOfAccounts: Api_FinancialCode | null = null;
  responsibilityCentre: string = '';
  Responsibility: Api_FinancialCode | null = null;
  readonly finalizedDate: string = '';
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  financials: FinancialActivityFormModel[] = [];
  payees: AcquisitionPayeeFormModel[] = [];
  isDisabled: string = '';
  rowVersion: number | null = null;
  payeeKey: string = '';

  constructor(id: number | null, acquisitionFileId: number = 0, finalizedDate: string) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
    this.finalizedDate = finalizedDate;
  }

  toApi(payeeOptions: PayeeOption[]): Api_CompensationRequisition {
    let apiPayee: Api_CompensationPayee | null = PayeeOption.toApi(
      this.payees[0]._id,
      this.id,
      this.payeeKey,
      payeeOptions,
    );

    if (apiPayee) {
      apiPayee.id = this.payees[0]._id;
      apiPayee.gstNumber = this.payees[0].gstNumber;
      apiPayee.isPaymentInTrust = this.payees[0].isPaymentInTrust;
      apiPayee.rowVersion = this.payees[0].rowVersion;
    }

    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
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
      generationDate: stringToUndefined(this.generationDatetTime),
      specialInstruction: stringToUndefined(this.specialInstruction),
      detailedRemarks: stringToUndefined(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: this.financials
        .filter(x => !x.isEmpty())
        .map<Api_CompensationFinancial>(x => x.toApi()),
      payees: apiPayee === null ? [] : [apiPayee],
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
    compensation.Responsibility = apiModel.responsibility;
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.financials =
      apiModel.financials?.map(x => FinancialActivityFormModel.fromApi(x)) || [];
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.payees = apiModel.payees?.map(x => AcquisitionPayeeFormModel.fromApi(x)) || [];
    compensation.payeeKey = PayeeOption.fromApi(apiModel.payees);
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
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

    if (compensation.payees.length === 0) {
      let defaultPayee = AcquisitionPayeeFormModel.defatultPayee(compensation.id!);
      defaultPayee.pretaxAmount = payeePretaxAmount;
      defaultPayee.taxAmount = payeeTaxAmount;
      defaultPayee.taxAmount = payeeTotalAmount;

      compensation.payees.push(defaultPayee);
    } else {
      compensation.payees[0].pretaxAmount = payeePretaxAmount;
      compensation.payees[0].taxAmount = payeeTaxAmount;
      compensation.payees[0].totalAmount = payeeTotalAmount;
    }

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
  readonly _id: number | null;
  readonly _compensationRequisitionId: number | null;

  payeeSelectedOption: string = '';
  gstNumber: string = '';
  isPaymentInTrust: boolean = false;
  pretaxAmount: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;
  acquisitionOwnerId: string = '';
  interestHolderId: string = '';
  acquisitionFilePersonId: string = '';
  rowVersion: number | null = null;
  isDisabled: string = '';

  constructor(id: number | null = null, compensationRequisitionId: number | null = null) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  static fromApi(apiModel: Api_CompensationPayee): AcquisitionPayeeFormModel {
    const payeeModel = new AcquisitionPayeeFormModel(
      apiModel.id,
      apiModel.compensationRequisitionId,
    );

    payeeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeModel.gstNumber = apiModel.gstNumber ?? '';
    payeeModel.acquisitionOwnerId = apiModel.acquisitionOwnerId?.toString() ?? '';
    payeeModel.interestHolderId = apiModel.interestHolderId?.toString() ?? '';
    payeeModel.acquisitionFilePersonId = apiModel.acquisitionFilePersonId?.toString() ?? '';
    payeeModel.rowVersion = apiModel.rowVersion ?? null;

    return payeeModel;
  }

  static defatultPayee(compensationRequisitionId: number): AcquisitionPayeeFormModel {
    let payeeModel = new AcquisitionPayeeFormModel(null, compensationRequisitionId);

    return payeeModel;
  }

  toApi(): Api_CompensationPayee {
    return {
      id: this._id,
      compensationRequisitionId: this._compensationRequisitionId ?? null,
      compensationRequisition: null,
      isPaymentInTrust: stringToBoolean(this.isPaymentInTrust),
      gstNumber: this.gstNumber,
      acquisitionOwnerId: this.acquisitionOwnerId === '' ? null : +this.acquisitionOwnerId,
      interestHolderId: null,
      interestHolder: null,
      motiSolicitor: null,
      motiSolicitorId: null,
      acquisitionFilePersonId: null,
      acquisitionOwner: null,
      isDisabled: stringToBoolean(this.isDisabled),
      rowVersion: this.rowVersion ?? null,
    };
  }
}
