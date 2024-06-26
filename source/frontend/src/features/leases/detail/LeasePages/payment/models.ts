import { defaultTypeCode } from '@/interfaces/ITypeCode';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { ApiGen_Concepts_Payment } from '@/models/api/generated/ApiGen_Concepts_Payment';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { isValidIsoDateTime } from '@/utils';
import { stringToNumber, stringToNumberOrNull } from '@/utils/formUtils';

export class FormLeasePeriod {
  id: number | null = null;
  leaseId: number | null = null;
  statusTypeCode: ApiGen_Base_CodeType<string> | null = null;
  leasePmtFreqTypeCode: ApiGen_Base_CodeType<string> | null = null;
  startDate = '';
  effectiveDateHist: string | null = null;
  expiryDate = '';
  renewalDate = '';
  paymentAmount: NumberFieldValue = '';
  gstAmount: NumberFieldValue = '';
  paymentDueDateStr = '';
  paymentNote = '';
  isGstEligible?: boolean;
  isTermExercised?: boolean;
  isFlexible = false;
  payments: FormLeasePayment[] = [];
  rowVersion?: number;

  public static toApi(
    formLeasePeriod: FormLeasePeriod,
    gstConstant?: number,
  ): ApiGen_Concepts_LeasePeriod {
    return {
      ...formLeaseTerm,
      isFlexible: formLeaseTerm.isFlexible ?? false,
      leaseId: formLeaseTerm.leaseId ?? 0,
      startDate: isValidIsoDateTime(formLeaseTerm.startDate) ? formLeaseTerm.startDate : null,
      renewalDate: null,
      expiryDate: isValidIsoDateTime(formLeasePeriod.expiryDate)
        ? formLeasePeriod.expiryDate
        : null,
      paymentAmount: stringToNumberOrNull(formLeasePeriod.paymentAmount),
      gstAmount: stringToNumberOrNull(
        formLeasePeriod.isGstEligible && gstConstant !== undefined
          ? (formLeasePeriod.paymentAmount as number) * (gstConstant / 100)
          : null,
      ),
      leasePmtFreqTypeCode: formLeasePeriod.leasePmtFreqTypeCode?.id
        ? formLeasePeriod.leasePmtFreqTypeCode
        : null,
      statusTypeCode: formLeasePeriod.statusTypeCode?.id ? formLeasePeriod.statusTypeCode : null,
      payments: formLeasePeriod.payments.map(payment => FormLeasePayment.toApi(payment)),
      isGstEligible: formLeasePeriod.isGstEligible ?? false,
      isTermExercised: formLeasePeriod.isTermExercised ?? false,
      ...getEmptyBaseAudit(formLeasePeriod.rowVersion),
    };
  }

  public static fromApi(apiLeasePeriod: ApiGen_Concepts_LeasePeriod): FormLeasePeriod {
    return {
      ...apiLeaseTerm,
      isFlexible: apiLeaseTerm.isFlexible ?? false,
      startDate: isValidIsoDateTime(apiLeaseTerm.startDate) ? apiLeaseTerm.startDate : '',
      expiryDate: isValidIsoDateTime(apiLeaseTerm.expiryDate) ? apiLeaseTerm.expiryDate : '',
      renewalDate: isValidIsoDateTime(apiLeaseTerm.renewalDate) ? apiLeaseTerm.renewalDate : '',
      paymentAmount: apiLeaseTerm.paymentAmount ?? '',
      gstAmount: apiLeaseTerm.gstAmount ?? '',
      paymentDueDateStr: apiLeaseTerm.paymentDueDateStr ?? '',
      paymentNote: apiLeaseTerm.paymentNote ?? '',
      payments:
        apiLeasePeriod.payments?.map((payment: ApiGen_Concepts_Payment) =>
          FormLeasePayment.fromApi(payment),
        ) ?? [],
      isGstEligible: apiLeasePeriod.isGstEligible ?? undefined,
      isTermExercised: apiLeasePeriod.isTermExercised ?? undefined,
      effectiveDateHist: null,
      rowVersion: apiLeasePeriod.rowVersion ?? undefined,
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
  gstAmount: '',
  paymentDueDateStr: '',
  paymentNote: '',
  isGstEligible: false,
  isTermExercised: false,
  isFlexible: false,
  effectiveDateHist: '',
  statusTypeCode: defaultTypeCode(),
  leasePmtFreqTypeCode: defaultTypeCode(),
  payments: [],
};

export class FormLeasePayment {
  id?: number;
  leasePeriodId = 0;
  leasePaymentMethodType: ApiGen_Base_CodeType<string> | null = null;
  receivedDate = '';
  note?: string;
  leasePaymentStatusTypeCode?: ApiGen_Base_CodeType<string>;
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
      leasePaymentMethodType: formLeasePayment.leasePaymentMethodType,
      note: formLeasePayment.note ?? null,
      ...getEmptyBaseAudit(formLeasePayment.rowVersion),
    };
  }

  public static fromApi(apiLeasePayment: ApiGen_Concepts_Payment): FormLeasePayment {
    const leasePayment = new FormLeasePayment();
    leasePayment.id = apiLeasePayment.id ?? undefined;
    leasePayment.leasePeriodId = apiLeasePayment.leasePeriodId;
    leasePayment.leasePaymentMethodType = apiLeasePayment.leasePaymentMethodType ?? null;
    leasePayment.receivedDate = isValidIsoDateTime(apiLeasePayment.receivedDate)
      ? apiLeasePayment.receivedDate
      : '';
    leasePayment.amountPreTax = apiLeasePayment.amountPreTax;
    leasePayment.amountPst = apiLeasePayment.amountPst ?? '';
    leasePayment.amountGst = apiLeasePayment.amountGst ?? '';
    leasePayment.amountTotal = apiLeasePayment.amountTotal ?? '';
    leasePayment.note = apiLeasePayment.note ?? undefined;
    leasePayment.leasePaymentStatusTypeCode =
      apiLeasePayment.leasePaymentStatusTypeCode ?? undefined;
    leasePayment.rowVersion = apiLeasePayment.rowVersion ?? undefined;
    return leasePayment;
  }
}

export const defaultFormLeasePayment: FormLeasePayment = {
  id: 0,
  leasePeriodId: 0,
  leasePaymentMethodType: defaultTypeCode(),
  receivedDate: '',
  amountPreTax: '',
  amountPst: '',
  amountGst: '',
  amountTotal: '',
  note: '',
  leasePaymentStatusTypeCode: defaultTypeCode(),
};
