import { defaultTypeCode } from '@/interfaces/ITypeCode';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_LeaseTerm } from '@/models/api/generated/ApiGen_Concepts_LeaseTerm';
import { ApiGen_Concepts_Payment } from '@/models/api/generated/ApiGen_Concepts_Payment';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { isValidIsoDateTime } from '@/utils';
import { stringToNumber, stringToNumberOrNull } from '@/utils/formUtils';

export class FormLeaseTerm {
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
  payments: FormLeasePayment[] = [];
  rowVersion?: number;

  public static toApi(
    formLeaseTerm: FormLeaseTerm,
    gstConstant?: number,
  ): ApiGen_Concepts_LeaseTerm {
    return {
      ...formLeaseTerm,
      leaseId: formLeaseTerm.leaseId ?? 0,
      startDate: isValidIsoDateTime(formLeaseTerm.startDate) ? formLeaseTerm.startDate : null,
      renewalDate: null,
      expiryDate: isValidIsoDateTime(formLeaseTerm.expiryDate) ? formLeaseTerm.expiryDate : null,
      paymentAmount: stringToNumberOrNull(formLeaseTerm.paymentAmount),
      gstAmount: stringToNumberOrNull(
        formLeaseTerm.isGstEligible && gstConstant !== undefined
          ? (formLeaseTerm.paymentAmount as number) * (gstConstant / 100)
          : null,
      ),
      leasePmtFreqTypeCode: formLeaseTerm.leasePmtFreqTypeCode?.id
        ? formLeaseTerm.leasePmtFreqTypeCode
        : null,
      statusTypeCode: formLeaseTerm.statusTypeCode?.id ? formLeaseTerm.statusTypeCode : null,
      payments: formLeaseTerm.payments.map(payment => FormLeasePayment.toApi(payment)),
      isGstEligible: formLeaseTerm.isGstEligible ?? false,
      isTermExercised: formLeaseTerm.isTermExercised ?? false,
      ...getEmptyBaseAudit(formLeaseTerm.rowVersion),
    };
  }

  public static fromApi(apiLeaseTerm: ApiGen_Concepts_LeaseTerm): FormLeaseTerm {
    return {
      ...apiLeaseTerm,
      startDate: isValidIsoDateTime(apiLeaseTerm.startDate) ? apiLeaseTerm.startDate : '',
      expiryDate: isValidIsoDateTime(apiLeaseTerm.expiryDate) ? apiLeaseTerm.expiryDate : '',
      renewalDate: isValidIsoDateTime(apiLeaseTerm.renewalDate) ? apiLeaseTerm.renewalDate : '',
      paymentAmount: apiLeaseTerm.paymentAmount ?? '',
      gstAmount: apiLeaseTerm.gstAmount ?? '',
      paymentDueDateStr: apiLeaseTerm.paymentDueDateStr ?? '',
      paymentNote: apiLeaseTerm.paymentNote ?? '',
      payments:
        apiLeaseTerm.payments?.map((payment: ApiGen_Concepts_Payment) =>
          FormLeasePayment.fromApi(payment),
        ) ?? [],
      isGstEligible: apiLeaseTerm.isGstEligible ?? undefined,
      isTermExercised: apiLeaseTerm.isTermExercised ?? undefined,
      effectiveDateHist: null,
      rowVersion: apiLeaseTerm.rowVersion ?? undefined,
    };
  }
}

export const defaultFormLeaseTerm: FormLeaseTerm = {
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
  effectiveDateHist: '',
  statusTypeCode: defaultTypeCode(),
  leasePmtFreqTypeCode: defaultTypeCode(),
  payments: [],
};
export class FormLeasePayment {
  id?: number;
  leaseTermId = 0;
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
    leasePayment.leaseTermId = apiLeasePayment.leaseTermId;
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
  leaseTermId: 0,
  leasePaymentMethodType: defaultTypeCode(),
  receivedDate: '',
  amountPreTax: '',
  amountPst: '',
  amountGst: '',
  amountTotal: '',
  note: '',
  leasePaymentStatusTypeCode: defaultTypeCode(),
};
