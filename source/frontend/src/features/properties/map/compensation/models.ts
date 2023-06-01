import {
  Api_CompensationFinancial,
  Api_CompensationRequisition,
} from 'models/api/CompensationRequisition';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { Api_Payee } from 'models/api/Payee';
import { Api_PayeeCheque } from 'models/api/PayeeCheque';
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
  payees: AcquisitionPayeeFormModel[] = [];
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
      payees: this.payees.map<Api_Payee>(x => x.toApi()),
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
    compensation.payees = apiModel.payees?.map(x => AcquisitionPayeeFormModel.fromApi(x)) || [];
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
    compensation.rowVersion = apiModel.rowVersion ?? null;

    return compensation;
  }
}

export class FinacialActivityFormModel {
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

  static fromApi(model: Api_CompensationFinancial): FinacialActivityFormModel {
    const newForm = new FinacialActivityFormModel(model.id, model.compensationId);
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
  acquisitionOwnerId: string = '';
  interestHolderId: string = '';
  ownerRepresentativeId: string = '';
  ownerSolicitorId: string = '';
  acquisitionFilePersonId: string = '';
  cheques: AcquisitionPayeeChequeFormModel[] = [];
  rowVersion: number | null = null;

  constructor(id: number | null = null, compensationRequisitionId: number | null = null) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  static fromApi(apiModel: Api_Payee): AcquisitionPayeeFormModel {
    const payeeModel = new AcquisitionPayeeFormModel(
      apiModel.id,
      apiModel.compensationRequisitionId,
    );

    payeeModel.acquisitionOwnerId = apiModel.acquisitionOwnerId?.toString() ?? '';
    payeeModel.interestHolderId = apiModel.interestHolderId?.toString() ?? '';
    payeeModel.ownerRepresentativeId = apiModel.ownerRepresentativeId?.toString() ?? '';
    payeeModel.ownerSolicitorId = apiModel.ownerSolicitorId?.toString() ?? '';
    payeeModel.acquisitionFilePersonId = apiModel.acquisitionFilePersonId?.toString() ?? '';
    payeeModel.cheques =
      apiModel.cheques?.map(x => AcquisitionPayeeChequeFormModel.fromApi(x)) || [];
    payeeModel.rowVersion = apiModel.rowVersion ?? null;

    return payeeModel;
  }

  toApi(): Api_Payee {
    return {
      id: this._id,
      compensationRequisitionId: this._compensationRequisitionId ?? null,
      acquisitionOwnerId: +this.acquisitionOwnerId,
      interestHolderId: null,
      ownerRepresentativeId: null,
      ownerSolicitorId: null,
      acquisitionFilePersonId: null,
      cheques: this.cheques.map<Api_PayeeCheque>(x => x.toApi()),
      rowVersion: this.rowVersion ?? null,
    };
  }
}

export class AcquisitionPayeeChequeFormModel {
  readonly _id: number | null;
  readonly _acquisitionPayeeId: number | null;

  constructor(id: number | null = null, acquisitionPayeeId: number | null = null) {
    this._id = id;
    this._acquisitionPayeeId = acquisitionPayeeId;
  }

  pretaxAmout: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;
  gstNumber: string = '';
  isPaymentInTrust: boolean = false;
  rowVersion: number | null = null;

  static fromApi(apiModel: Api_PayeeCheque): AcquisitionPayeeChequeFormModel {
    const payeeChequeModel = new AcquisitionPayeeChequeFormModel(
      apiModel.id,
      apiModel.acquisitionPayeeId,
    );

    payeeChequeModel.pretaxAmout = apiModel.pretaxAmout ?? 0;
    payeeChequeModel.taxAmount = apiModel.taxAmount ?? 0;
    payeeChequeModel.totalAmount = apiModel.totalAmount ?? 0;
    payeeChequeModel.gstNumber = apiModel.gstNumber ?? '';
    payeeChequeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeChequeModel.rowVersion = apiModel.rowVersion ?? null;

    return payeeChequeModel;
  }

  toApi(): Api_PayeeCheque {
    return {
      id: this._id,
      acquisitionPayeeId: this._acquisitionPayeeId,
      isPaymentInTrust: this.isPaymentInTrust,
      pretaxAmout: this.pretaxAmout,
      taxAmount: this.taxAmount,
      totalAmount: this.totalAmount,
      gstNumber: this.gstNumber,
      rowVersion: this.rowVersion ?? null,
    };
  }
}
