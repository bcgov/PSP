import { Form } from 'formik';
import { defaultTypeCode } from '@/interfaces/ITypeCode';
import { FormLeasePeriod, defaultFormLeasePeriod } from './models';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';

describe('payments model tests', () => {
  it('fromApi handles a minimal object', () => {
    let model = FormLeasePeriod.fromApi({
      id: 1,
    } as ApiGen_Concepts_LeasePeriod);
    expect(model).toBeDefined();
  });

  it('fromApi converts booleans', () => {
    let model = FormLeasePeriod.fromApi({
      id: 1,
      isVariable: true,
      isFlexible: false,
    } as ApiGen_Concepts_LeasePeriod);
    expect(model.isVariable).toBe('true');
    expect(model.isFlexible).toBe('false');
  });

  it('toApi converts booleans', () => {
    let model = FormLeasePeriod.toApi({
      ...defaultFormLeasePeriod,
      isVariable: 'true',
      isFlexible: 'false',
    });
    expect(model.isVariable).toBe(true);
    expect(model.isFlexible).toBe(false);
  });

  it('toApi does not convert gst if gst not specified', () => {
    let model = FormLeasePeriod.toApi({
      id: 1,
      isAdditionalRentGstEligible: false,
      isVariableRentGstEligible: false,
      isGstEligible: false,
      variableRentPaymentAmount: 1,
      additionalRentPaymentAmount: 1,
      paymentAmount: 1,
      gstAmount: 7,
      variableRentGstAmount: 8,
      additionalRentGstAmount: 9,
    } as FormLeasePeriod);
    expect(model.gstAmount).toBe(null);
    expect(model.variableRentGstAmount).toBe(null);
    expect(model.additionalRentGstAmount).toBe(null);
  });

  it('toApi does convert gst if gst specified', () => {
    let model = FormLeasePeriod.toApi({
      id: 1,
      isAdditionalRentGstEligible: true,
      isVariableRentGstEligible: true,
      isGstEligible: true,
      variableRentPaymentAmount: 1,
      additionalRentPaymentAmount: 1,
      paymentAmount: 1,
      gstAmount: 7,
      variableRentGstAmount: 8,
      additionalRentGstAmount: 9,
    } as FormLeasePeriod);
    expect(model.gstAmount).toBe(7);
    expect(model.variableRentGstAmount).toBe(8);
    expect(model.additionalRentGstAmount).toBe(9);
  });
});
