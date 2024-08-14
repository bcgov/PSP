import { isNumber } from 'lodash';

import { SelectOption } from '@/components/common/form';
import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcquisitionProperty } from '@/models/api/generated/ApiGen_Concepts_CompReqAcquisitionProperty';
import { ApiGen_Concepts_CompReqLeaseProperty } from '@/models/api/generated/ApiGen_Concepts_CompReqLeaseProperty';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { isValidId, isValidIsoDateTime } from '@/utils';
import { stringToNull } from '@/utils/formUtils';

import { PayeeOption } from '../../acquisition/models/PayeeOptionModel';
import { CompensationPayeeFormModel } from './AcquisitionPayeeFormModel';
import { FinancialActivityFormModel } from './FinancialActivityFormModel';

export class CompensationRequisitionFormModel {
  id: number | null;
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
  payee: CompensationPayeeFormModel;
  alternateProject: IAutocompletePrediction | null = null;
  selectedProperties: ApiGen_Concepts_FileProperty[] = [];
  rowVersion: number | null = null;

  constructor(
    id: number | null,
    readonly acquisitionFileId: number | null,
    readonly leaseId: number | null,
    finalizedDate: string,
  ) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
    this.leaseId = leaseId;
    this.payee = new CompensationPayeeFormModel(id);
    this.finalizedDate = isValidIsoDateTime(finalizedDate) ? finalizedDate : '';
  }

  toApi(payeeOptions: PayeeOption[]): ApiGen_Concepts_CompensationRequisition {
    const modelWithPayeeInformation = this.payee.toApi(payeeOptions);

    return {
      ...modelWithPayeeInformation,
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      leaseId: this.leaseId,
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
      compReqAcquisitionProperties:
        this.acquisitionFileId != null
          ? this.selectedProperties.map(x => {
              return {
                compensationRequisitionPropertyId: null,
                compensationRequisitionId: this.id,
                propertyAcquisitionFileId: x.id,
                acquisitionFileProperty: null,
              } as ApiGen_Concepts_CompReqAcquisitionProperty;
            })
          : null,
      compReqLeaseProperties:
        this.leaseId != null
          ? this.selectedProperties.map(x => {
              return {
                compensationRequisitionPropertyId: null,
                compensationRequisitionId: this.id,
                propertyLeaseId: x.id,
                leaseProperty: null,
              } as ApiGen_Concepts_CompReqLeaseProperty;
            })
          : null,
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
      apiModel.leaseId,
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

    compensation.payee = CompensationPayeeFormModel.fromApi(apiModel);
    compensation.payee.pretaxAmount = payeePretaxAmount;
    compensation.payee.taxAmount = payeeTaxAmount;
    compensation.payee.totalAmount = payeeTotalAmount;

    if (apiModel.acquisitionFileId != null) {
      compensation.selectedProperties = apiModel.compReqAcquisitionProperties.map(
        x => x.acquisitionFileProperty,
      );
    } else if (apiModel.leaseId != null) {
      compensation.selectedProperties = apiModel.compReqLeaseProperties.map(x => x.leaseProperty);
    }

    return compensation;
  }
}
