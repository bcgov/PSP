import { defaultTypeCode } from '@/interfaces';
import { Api_LeasePayment } from '@/models/api/LeasePayment';
import { Api_LeaseTerm } from '@/models/api/LeaseTerm';
import Api_TypeCode from '@/models/api/TypeCode';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { stringToUndefined, toRequiredTypeCode } from '@/utils/formUtils';

export class FormLeaseTerm {
  id: number | null = null;
  leaseId: number | null = null;
  statusTypeCode: Api_TypeCode<string> | null = null;
  leasePmtFreqTypeCode: Api_TypeCode<string> | null = null;
  startDate: string = '';
  effectiveDateHist: string | null = null;
  expiryDate: string = '';
  renewalDate: string = '';
  endDateHist: string = '';
  paymentAmount: NumberFieldValue = '';
  gstAmount: NumberFieldValue = '';
  paymentDueDate: string = '';
  paymentNote: string = '';
  isGstEligible?: boolean;
  isTermExercised?: boolean;
  payments: FormLeasePayment[] = [];
  rowVersion?: number;

  public static toApi(formLeaseTerm: FormLeaseTerm, gstConstant?: number): Api_LeaseTerm {
    return {
      ...formLeaseTerm,
      leaseId: formLeaseTerm.leaseId ?? 0,
      startDate: stringToUndefined(formLeaseTerm.startDate),
      renewalDate: null,
      expiryDate: stringToUndefined(formLeaseTerm.expiryDate),
      paymentAmount: stringToUndefined(formLeaseTerm.paymentAmount),
      gstAmount: stringToUndefined(
        formLeaseTerm.isGstEligible && gstConstant !== undefined
          ? (formLeaseTerm.paymentAmount as number) * (gstConstant / 100)
          : null,
      ),
      leasePmtFreqTypeCode: formLeaseTerm.leasePmtFreqTypeCode?.id
        ? formLeaseTerm.leasePmtFreqTypeCode
        : null,
      statusTypeCode: formLeaseTerm.statusTypeCode?.id ? formLeaseTerm.statusTypeCode : null,
      payments: formLeaseTerm.payments.map(payment => FormLeasePayment.toApi(payment)),
      isGstEligible: formLeaseTerm.isGstEligible ?? null,
      isTermExercised: formLeaseTerm.isTermExercised ?? null,
    };
  }

  public static fromApi(apiLeaseTerm: Api_LeaseTerm): FormLeaseTerm {
    return {
      ...apiLeaseTerm,
      expiryDate: apiLeaseTerm.expiryDate ?? '',
      renewalDate: apiLeaseTerm.renewalDate ?? '',
      endDateHist: apiLeaseTerm.endDateHist ?? '',
      paymentAmount: apiLeaseTerm.paymentAmount ?? '',
      gstAmount: apiLeaseTerm.gstAmount ?? '',
      paymentDueDate: apiLeaseTerm.paymentDueDate ?? '',
      paymentNote: apiLeaseTerm.paymentNote ?? '',
      payments: apiLeaseTerm.payments.map((payment: Api_LeasePayment) =>
        FormLeasePayment.fromApi(payment),
      ),
      isGstEligible: apiLeaseTerm.isGstEligible ?? undefined,
      isTermExercised: apiLeaseTerm.isTermExercised ?? undefined,
    };
  }
}

export const defaultFormLeaseTerm: FormLeaseTerm = {
  leaseId: 0,
  id: null,
  startDate: '',
  expiryDate: '',
  renewalDate: '',
  endDateHist: '',
  paymentAmount: '',
  gstAmount: '',
  paymentDueDate: '',
  paymentNote: '',
  isGstEligible: false,
  isTermExercised: false,
  effectiveDateHist: '',
  statusTypeCode: defaultTypeCode,
  leasePmtFreqTypeCode: defaultTypeCode,
  payments: [],
};
export class FormLeasePayment {
  id?: number;
  leaseTermId: number = 0;
  leasePaymentMethodType?: Api_TypeCode<string>;
  receivedDate: string = '';
  note?: string;
  leasePaymentStatusTypeCode?: Api_TypeCode<string>;
  amountPreTax: NumberFieldValue = '';
  amountGst: NumberFieldValue = '';
  amountPst: NumberFieldValue = '';
  amountTotal: NumberFieldValue = '';
  rowVersion?: number;

  public static toApi(formLeasePayment: FormLeasePayment): Api_LeasePayment {
    return {
      ...formLeasePayment,
      id: formLeasePayment.id ?? null,
      amountPreTax: stringToUndefined(formLeasePayment.amountPreTax),
      amountGst: stringToUndefined(formLeasePayment.amountGst),
      amountPst: stringToUndefined(formLeasePayment.amountPst),
      amountTotal: stringToUndefined(formLeasePayment.amountTotal),
      leasePaymentStatusTypeCode: formLeasePayment.leasePaymentStatusTypeCode?.id
        ? formLeasePayment.leasePaymentStatusTypeCode
        : null,
      leasePaymentMethodType: toRequiredTypeCode(formLeasePayment.leasePaymentMethodType),
      note: formLeasePayment.note ?? null,
      rowVersion: formLeasePayment.rowVersion ?? undefined,
    };
  }

  public static fromApi(apiLeasePayment: Api_LeasePayment): FormLeasePayment {
    const leasePayment = new FormLeasePayment();
    leasePayment.id = apiLeasePayment.id ?? undefined;
    leasePayment.leaseTermId = apiLeasePayment.leaseTermId;
    leasePayment.leasePaymentMethodType = apiLeasePayment.leasePaymentMethodType;
    leasePayment.receivedDate = apiLeasePayment.receivedDate;
    leasePayment.amountPreTax = apiLeasePayment.amountPreTax;
    leasePayment.amountPst = apiLeasePayment.amountPst ?? '';
    leasePayment.amountGst = apiLeasePayment.amountGst ?? '';
    leasePayment.amountTotal = apiLeasePayment.amountTotal ?? '';
    leasePayment.note = apiLeasePayment.note ?? undefined;
    leasePayment.leasePaymentStatusTypeCode =
      apiLeasePayment.leasePaymentStatusTypeCode ?? undefined;
    leasePayment.rowVersion = apiLeasePayment.rowVersion;
    return leasePayment;
  }
}

export const defaultFormLeasePayment: FormLeasePayment = {
  id: 0,
  leaseTermId: 0,
  leasePaymentMethodType: defaultTypeCode,
  receivedDate: '',
  amountPreTax: '',
  amountPst: '',
  amountGst: '',
  amountTotal: '',
  note: '',
  leasePaymentStatusTypeCode: defaultTypeCode,
};
