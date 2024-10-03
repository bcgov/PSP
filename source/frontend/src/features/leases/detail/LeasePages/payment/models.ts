import { defaultTypeCode } from '@/interfaces/ITypeCode';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { ApiGen_Concepts_Payment } from '@/models/api/generated/ApiGen_Concepts_Payment';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { isValidIsoDateTime } from '@/utils';
import { stringToNumber, stringToNumberOrNull } from '@/utils/formUtils';

import { ApiGen_CodeTypes_LeasePaymentCategoryTypes } from './../../../../../models/api/generated/ApiGen_CodeTypes_LeasePaymentCategoryTypes';

export class FormLeasePeriod {
  id: number | null = null;
  leaseId: number | null = null;
  statusTypeCode: ApiGen_Base_CodeType<string> | null = null;
  leasePmtFreqTypeCode: ApiGen_Base_CodeType<string> | null = null;
  additionalRentFreqTypeCode: ApiGen_Base_CodeType<string> | null = null;
  variableRentFreqTypeCode: ApiGen_Base_CodeType<string> | null = null;
  startDate = '';
  effectiveDateHist: string | null = null;
  expiryDate = '';
  renewalDate = '';
  paymentAmount: NumberFieldValue = '';
  additionalRentPaymentAmount: NumberFieldValue = '';
  variableRentPaymentAmount: NumberFieldValue = '';
  gstAmount: NumberFieldValue = '';
  additionalRentGstAmount: NumberFieldValue = '';
  variableRentGstAmount: NumberFieldValue = '';
  paymentDueDateStr = '';
  paymentNote = '';
  isGstEligible?: boolean;
  isVariableRentGstEligible?: boolean;
  isAdditionalRentGstEligible?: boolean;
  isTermExercised?: boolean;
  isFlexible: 'true' | 'false' = 'false';
  isVariable: 'true' | 'false' = 'false';
  payments: FormLeasePayment[] = [];
  rowVersion?: number;

  public static toApi(formLeasePeriod: FormLeasePeriod): ApiGen_Concepts_LeasePeriod {
    return {
      ...formLeasePeriod,
      isFlexible: formLeasePeriod.isFlexible === 'true',
      isVariable: formLeasePeriod.isVariable === 'true',
      leaseId: formLeasePeriod.leaseId ?? 0,
      startDate: isValidIsoDateTime(formLeasePeriod.startDate) ? formLeasePeriod.startDate : null,
      renewalDate: null,
      expiryDate: isValidIsoDateTime(formLeasePeriod.expiryDate)
        ? formLeasePeriod.expiryDate
        : null,
      paymentAmount: stringToNumberOrNull(formLeasePeriod.paymentAmount),
      additionalRentPaymentAmount: stringToNumberOrNull(
        formLeasePeriod.additionalRentPaymentAmount,
      ),
      variableRentPaymentAmount: stringToNumberOrNull(formLeasePeriod.variableRentPaymentAmount),
      gstAmount: stringToNumberOrNull(
        formLeasePeriod.isGstEligible ? formLeasePeriod.gstAmount : null,
      ),
      additionalRentGstAmount: stringToNumberOrNull(
        formLeasePeriod.isAdditionalRentGstEligible
          ? formLeasePeriod.additionalRentGstAmount
          : null,
      ),
      variableRentGstAmount: stringToNumberOrNull(
        formLeasePeriod.isVariableRentGstEligible ? formLeasePeriod.variableRentGstAmount : null,
      ),
      leasePmtFreqTypeCode: formLeasePeriod.leasePmtFreqTypeCode?.id
        ? formLeasePeriod.leasePmtFreqTypeCode
        : null,
      additionalRentFreqTypeCode: formLeasePeriod.additionalRentFreqTypeCode?.id
        ? formLeasePeriod.additionalRentFreqTypeCode
        : null,
      variableRentFreqTypeCode: formLeasePeriod.variableRentFreqTypeCode?.id
        ? formLeasePeriod.variableRentFreqTypeCode
        : null,
      statusTypeCode: formLeasePeriod.statusTypeCode?.id ? formLeasePeriod.statusTypeCode : null,
      payments: formLeasePeriod.payments?.map(payment => FormLeasePayment.toApi(payment)),
      isGstEligible: formLeasePeriod.isGstEligible ?? false,
      isAdditionalRentGstEligible: formLeasePeriod.isAdditionalRentGstEligible,
      isVariableRentGstEligible: formLeasePeriod.isVariableRentGstEligible,
      isTermExercised: formLeasePeriod.isTermExercised ?? false,
      ...getEmptyBaseAudit(formLeasePeriod.rowVersion),
    };
  }

  public static fromApi(apiLeasePeriod: ApiGen_Concepts_LeasePeriod): FormLeasePeriod {
    return {
      ...apiLeasePeriod,
      isFlexible: apiLeasePeriod.isFlexible ? 'true' : 'false',
      isVariable: apiLeasePeriod.isVariable ? 'true' : 'false',
      startDate: isValidIsoDateTime(apiLeasePeriod.startDate) ? apiLeasePeriod.startDate : '',
      expiryDate: isValidIsoDateTime(apiLeasePeriod.expiryDate) ? apiLeasePeriod.expiryDate : '',
      renewalDate: isValidIsoDateTime(apiLeasePeriod.renewalDate) ? apiLeasePeriod.renewalDate : '',
      paymentAmount: apiLeasePeriod.paymentAmount ?? '',
      gstAmount: apiLeasePeriod.gstAmount ?? '',
      additionalRentGstAmount: apiLeasePeriod.additionalRentGstAmount ?? '',
      variableRentGstAmount: apiLeasePeriod.variableRentGstAmount ?? '',
      paymentDueDateStr: apiLeasePeriod.paymentDueDateStr ?? '',
      paymentNote: apiLeasePeriod.paymentNote ?? '',
      payments:
        apiLeasePeriod.payments?.map((payment: ApiGen_Concepts_Payment) =>
          FormLeasePayment.fromApi(payment),
        ) ?? [],
      isGstEligible: apiLeasePeriod.isGstEligible ?? undefined,
      isAdditionalRentGstEligible: apiLeasePeriod.isAdditionalRentGstEligible ?? undefined,
      isVariableRentGstEligible: apiLeasePeriod.isVariableRentGstEligible ?? undefined,
      isTermExercised: apiLeasePeriod.isTermExercised ?? undefined,
      effectiveDateHist: null,
      rowVersion: apiLeasePeriod.rowVersion ?? undefined,
      leasePmtFreqTypeCode: apiLeasePeriod.leasePmtFreqTypeCode,
      additionalRentFreqTypeCode: apiLeasePeriod.additionalRentFreqTypeCode,
      variableRentFreqTypeCode: apiLeasePeriod.variableRentFreqTypeCode,
    };
  }
}

export const defaultFormLeasePeriod: FormLeasePeriod = {
  leaseId: 0,
  id: null,
  startDate: '',
  expiryDate: '',
  renewalDate: '',
  paymentAmount: '',
  variableRentPaymentAmount: '',
  additionalRentPaymentAmount: '',
  gstAmount: '',
  additionalRentGstAmount: '',
  variableRentGstAmount: '',
  paymentDueDateStr: '',
  paymentNote: '',
  isGstEligible: false,
  isVariableRentGstEligible: false,
  isAdditionalRentGstEligible: false,
  isTermExercised: false,
  isFlexible: 'false',
  isVariable: 'false',
  effectiveDateHist: '',
  statusTypeCode: defaultTypeCode(),
  leasePmtFreqTypeCode: defaultTypeCode(),
  additionalRentFreqTypeCode: defaultTypeCode(),
  variableRentFreqTypeCode: defaultTypeCode(),
  payments: [],
};

export class FormLeasePayment {
  id?: number;
  leasePeriodId = 0;
  leasePaymentMethodType: ApiGen_Base_CodeType<string>;
  receivedDate = '';
  note?: string;
  leasePaymentStatusTypeCode?: ApiGen_Base_CodeType<string>;
  leasePaymentCategoryTypeCode?: ApiGen_Base_CodeType<string>;
  amountPreTax: NumberFieldValue = '';
  amountGst: NumberFieldValue = '';
  amountPst: NumberFieldValue = '';
  amountTotal: NumberFieldValue = '';
  rowVersion?: number;

  public static toApi(formLeasePayment: FormLeasePayment): ApiGen_Concepts_Payment {
    return {
      ...formLeasePayment,
      id: formLeasePayment.id ?? null,
      amountPreTax: stringToNumber(formLeasePayment.amountPreTax),
      amountGst: stringToNumber(formLeasePayment.amountGst),
      amountPst: stringToNumber(formLeasePayment.amountPst),
      amountTotal: stringToNumber(formLeasePayment.amountTotal),
      leasePaymentStatusTypeCode: formLeasePayment.leasePaymentStatusTypeCode?.id
        ? formLeasePayment.leasePaymentStatusTypeCode
        : null,
      leasePaymentCategoryTypeCode: formLeasePayment.leasePaymentCategoryTypeCode?.id
        ? formLeasePayment.leasePaymentCategoryTypeCode
        : null,
      leasePaymentMethodType: formLeasePayment.leasePaymentMethodType,

      note: formLeasePayment.note ?? null,
      ...getEmptyBaseAudit(formLeasePayment.rowVersion),
    };
  }

  public static fromApi(apiLeasePayment: ApiGen_Concepts_Payment): FormLeasePayment {
    const leasePayment = new FormLeasePayment();
    leasePayment.id = apiLeasePayment?.id ?? undefined;
    leasePayment.leasePeriodId = apiLeasePayment?.leasePeriodId;
    leasePayment.leasePaymentMethodType = apiLeasePayment?.leasePaymentMethodType ?? null;
    leasePayment.receivedDate = isValidIsoDateTime(apiLeasePayment.receivedDate)
      ? apiLeasePayment?.receivedDate
      : '';
    leasePayment.amountPreTax = apiLeasePayment?.amountPreTax;
    leasePayment.amountPst = apiLeasePayment?.amountPst ?? '';
    leasePayment.amountGst = apiLeasePayment?.amountGst ?? '';
    leasePayment.amountTotal = apiLeasePayment?.amountTotal ?? '';
    leasePayment.note = apiLeasePayment?.note ?? undefined;
    leasePayment.leasePaymentStatusTypeCode =
      apiLeasePayment?.leasePaymentStatusTypeCode ?? undefined;
    leasePayment.leasePaymentCategoryTypeCode = apiLeasePayment?.leasePaymentCategoryTypeCode ?? {
      ...defaultTypeCode(),
      id: ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE,
    };
    leasePayment.rowVersion = apiLeasePayment?.rowVersion ?? undefined;
    return leasePayment;
  }
}

export class LeasePeriodByCategoryProjection {
  readonly leasePmtFreqTypeCode: ApiGen_Base_CodeType<string> | null;
  readonly paymentAmount: NumberFieldValue;
  readonly isGstEligible: boolean | undefined;
  readonly gstAmount: NumberFieldValue;
  readonly payments: FormLeasePayment[];
  readonly isTermExercised: boolean | undefined;
  readonly category: ApiGen_CodeTypes_LeasePaymentCategoryTypes;

  constructor(
    leasePeriod: FormLeasePeriod | undefined,
    category: ApiGen_CodeTypes_LeasePaymentCategoryTypes,
  ) {
    this.leasePmtFreqTypeCode = leasePeriod?.leasePmtFreqTypeCode ?? null;
    this.paymentAmount = leasePeriod?.paymentAmount ?? 0;
    this.isGstEligible = leasePeriod?.isGstEligible;
    this.gstAmount = leasePeriod?.gstAmount ?? '';
    this.payments = leasePeriod?.payments ?? [];
    this.isTermExercised = leasePeriod?.isTermExercised;
    this.category = category ?? ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE;

    if (category === ApiGen_CodeTypes_LeasePaymentCategoryTypes.ADDL) {
      this.isGstEligible = leasePeriod?.isAdditionalRentGstEligible;
      this.paymentAmount = leasePeriod?.additionalRentPaymentAmount ?? 0;
      this.leasePmtFreqTypeCode = leasePeriod?.additionalRentFreqTypeCode ?? null;
      this.gstAmount = leasePeriod?.additionalRentGstAmount ?? '';
    } else if (category === ApiGen_CodeTypes_LeasePaymentCategoryTypes.VBL) {
      this.isGstEligible = leasePeriod?.isVariableRentGstEligible;
      this.paymentAmount = leasePeriod?.variableRentPaymentAmount ?? 0;
      this.leasePmtFreqTypeCode = leasePeriod?.variableRentFreqTypeCode ?? null;
      this.gstAmount = leasePeriod?.variableRentGstAmount ?? '';
    }
  }
}

export const defaultFormLeasePayment: FormLeasePayment = {
  id: 0,
  leasePeriodId: 0,
  leasePaymentMethodType: { ...defaultTypeCode(), id: 'CHEQ' },
  receivedDate: '',
  amountPreTax: '',
  amountPst: '',
  amountGst: '',
  amountTotal: '',
  note: '',
  leasePaymentStatusTypeCode: defaultTypeCode(),
  leasePaymentCategoryTypeCode: {
    ...defaultTypeCode(),
    id: ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE,
  },
};
