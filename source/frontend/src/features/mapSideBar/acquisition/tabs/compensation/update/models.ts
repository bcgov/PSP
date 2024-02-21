import isNumber from 'lodash/isNumber';

import { SelectOption } from '@/components/common/form';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { isValidId, isValidIsoDateTime } from '@/utils';
import { booleanToString, stringToBoolean, stringToNull } from '@/utils/formUtils';

export class CompensationRequisitionFormModel {
  id: number | null;
  acquisitionFileId: number;
  status = '';
  fiscalYear = '';
  stob: SelectOption | null = null;
  yearlyFinancial: ApiGen_Concepts_FinancialCode | null = null;
  serviceLine: SelectOption | null = null;
  chartOfAccounts: ApiGen_Concepts_FinancialCode | null = null;
  responsibilityCentre: SelectOption | null = null;
  responsibility: ApiGen_Concepts_FinancialCode | null = null;
  readonly finalizedDate: string;
  agreementDateTime = '';
  expropriationNoticeServedDateTime = '';
  expropriationVestingDateTime = '';
  advancedPaymentServedDate = '';
  generationDatetTime = '';
  specialInstruction = '';
  detailedRemarks = '';
  financials: FinancialActivityFormModel[] = [];
  payee: AcquisitionPayeeFormModel;
  alternateProject: IAutocompletePrediction | null = null;
  rowVersion: number | null = null;

  constructor(id: number | null, acquisitionFileId = 0, finalizedDate: string) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
    this.payee = new AcquisitionPayeeFormModel();
    this.finalizedDate = isValidIsoDateTime(finalizedDate) ? finalizedDate : '';
  }

  toApi(payeeOptions: PayeeOption[]): ApiGen_Concepts_CompensationRequisition {
    const modelWithPayeeInformation = this.payee.toApi(payeeOptions);

    return {
      ...modelWithPayeeInformation,
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      alternateProjectId: isValidId(this.alternateProject?.id) ? this.alternateProject!.id : null,
      isDraft: this.status === 'draft' ? true : false,
      fiscalYear: stringToNull(this.fiscalYear),
      yearlyFinancialId:
        !!this.stob?.value && isNumber(this.stob?.value) ? Number(this.stob?.value) : null,
      yearlyFinancial: null,
      chartOfAccountsId:
        !!this.serviceLine?.value && isNumber(this.serviceLine?.value)
          ? Number(this.serviceLine?.value)
          : null,
      chartOfAccounts: null,
      responsibilityId:
        !!this.responsibilityCentre?.value && isNumber(this.responsibilityCentre?.value)
          ? Number(this.responsibilityCentre?.value)
          : null,
      responsibility: null,
      agreementDate: isValidIsoDateTime(this.agreementDateTime) ? this.agreementDateTime : null,
      finalizedDate: isValidIsoDateTime(this.finalizedDate) ? this.finalizedDate : null,
      expropriationNoticeServedDate: isValidIsoDateTime(this.expropriationNoticeServedDateTime)
        ? this.expropriationNoticeServedDateTime
        : null,
      expropriationVestingDate: isValidIsoDateTime(this.expropriationVestingDateTime)
        ? this.expropriationVestingDateTime
        : null,
      advancedPaymentServedDate: isValidIsoDateTime(this.advancedPaymentServedDate)
        ? this.advancedPaymentServedDate
        : null,
      generationDate: isValidIsoDateTime(this.generationDatetTime)
        ? this.generationDatetTime
        : null,
      specialInstruction: stringToNull(this.specialInstruction),
      detailedRemarks: stringToNull(this.detailedRemarks),
      financials: this.financials
        .filter(x => !x.isEmpty())
        .map<ApiGen_Concepts_CompensationFinancial>(x => x.toApi()),
      rowVersion: this.rowVersion ?? null,
    };
  }

  static fromApi(
    apiModel: ApiGen_Concepts_CompensationRequisition,
    yearlyFinancialOptions: SelectOption[] = [],
    chartOfAccountOptions: SelectOption[] = [],
    responsibilityCentreOptions: SelectOption[] = [],
    financialActivityOptions: SelectOption[] = [],
  ): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
      apiModel.finalizedDate ?? '',
    );

    compensation.status =
      apiModel.isDraft === true ? 'draft' : apiModel.isDraft === null ? '' : 'final';
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.stob =
      !!apiModel.yearlyFinancialId && isNumber(apiModel.yearlyFinancialId)
        ? yearlyFinancialOptions.find(c => +c.value === apiModel.yearlyFinancialId) ?? null
        : null;
    compensation.yearlyFinancial = apiModel.yearlyFinancial;
    compensation.serviceLine =
      !!apiModel.chartOfAccountsId && isNumber(apiModel.chartOfAccountsId)
        ? chartOfAccountOptions.find(c => +c.value === apiModel.chartOfAccountsId) ?? null
        : null;
    compensation.chartOfAccounts = apiModel.chartOfAccounts;
    compensation.responsibilityCentre =
      !!apiModel.responsibilityId && isNumber(apiModel.responsibilityId)
        ? responsibilityCentreOptions.find(c => +c.value === apiModel.responsibilityId) ?? null
        : null;
    compensation.responsibility = apiModel.responsibility;
    compensation.alternateProject =
      apiModel.alternateProject !== null
        ? {
            id: apiModel.alternateProject?.id || 0,
            text: apiModel.alternateProject?.description || '',
          }
        : null;
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.advancedPaymentServedDate = apiModel.advancedPaymentServedDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.financials =
      apiModel.financials?.map(x =>
        FinancialActivityFormModel.fromApi(x, financialActivityOptions),
      ) || [];

    compensation.rowVersion = apiModel.rowVersion ?? null;

    const payeePretaxAmount =
      apiModel?.financials?.map(f => f.pretaxAmount ?? 0).reduce((prev, next) => prev + next, 0) ??
      0;

    const payeeTaxAmount =
      apiModel?.financials?.map(f => f.taxAmount ?? 0).reduce((prev, next) => prev + next, 0) ?? 0;

    const payeeTotalAmount =
      apiModel?.financials?.map(f => f.totalAmount ?? 0).reduce((prev, next) => prev + next, 0) ??
      0;

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

  financialActivityCodeId: SelectOption | null = null;
  financialActivityCode: ApiGen_Concepts_FinancialCode | null = null;
  pretaxAmount = 0;
  isGstRequired = '';
  taxAmount = 0;
  totalAmount = 0;
  rowVersion: number | null = null;
  isDisabled = '';

  constructor(id: number | null = null, compensationRequisitionId: number) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  isEmpty(): boolean {
    return this.financialActivityCode === null && this.pretaxAmount === 0;
  }

  toApi(): ApiGen_Concepts_CompensationFinancial {
    return {
      id: this._id,
      compensationId: this._compensationRequisitionId,
      financialActivityCodeId:
        !!this.financialActivityCodeId?.value && isNumber(this.financialActivityCodeId?.value)
          ? Number(this.financialActivityCodeId?.value)
          : 0,
      financialActivityCode: null,
      pretaxAmount: this.pretaxAmount,
      isGstRequired: stringToBoolean(this.isGstRequired),
      taxAmount: this.taxAmount,
      totalAmount: this.totalAmount,
      isDisabled: stringToBoolean(this.isDisabled),
      h120CategoryId: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(
    model: ApiGen_Concepts_CompensationFinancial,
    financialActivityOptions: SelectOption[] = [],
  ): FinancialActivityFormModel {
    const newForm = new FinancialActivityFormModel(model.id, model.compensationId);
    newForm.pretaxAmount = model.pretaxAmount ?? 0;
    newForm.isGstRequired = booleanToString(model.isGstRequired);
    newForm.taxAmount = model.taxAmount ?? 0;
    newForm.totalAmount = model.totalAmount ?? 0;
    newForm.financialActivityCodeId =
      !!model.financialActivityCodeId && isNumber(model.financialActivityCodeId)
        ? financialActivityOptions.find(c => +c.value === model.financialActivityCodeId) ?? null
        : null;
    newForm.financialActivityCode = model.financialActivityCode;
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.isDisabled = booleanToString(model.isDisabled);

    return newForm;
  }
}

export class AcquisitionPayeeFormModel {
  payeeKey = '';
  gstNumber = '';
  legacyPayee = '';
  isPaymentInTrust = false;

  pretaxAmount = 0;
  taxAmount = 0;
  totalAmount = 0;

  static fromApi(apiModel: ApiGen_Concepts_CompensationRequisition): AcquisitionPayeeFormModel {
    const payeeModel = new AcquisitionPayeeFormModel();

    payeeModel.payeeKey = PayeeOption.fromApi(apiModel);
    payeeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeModel.gstNumber = apiModel.gstNumber ?? '';
    payeeModel.legacyPayee = apiModel.legacyPayee ?? '';

    return payeeModel;
  }

  toApi(payeeOptions: PayeeOption[]): ApiGen_Concepts_CompensationRequisition {
    const modelWithPayeeInformation: ApiGen_Concepts_CompensationRequisition = PayeeOption.toApi(
      this.payeeKey,
      payeeOptions,
    );

    return {
      ...modelWithPayeeInformation,
      legacyPayee: stringToNull(this.legacyPayee),
      isPaymentInTrust: stringToBoolean(this.isPaymentInTrust),
      gstNumber: this.gstNumber,
    };
  }
}
