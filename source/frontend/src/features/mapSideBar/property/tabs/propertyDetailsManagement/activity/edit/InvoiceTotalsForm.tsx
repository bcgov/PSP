import { FormikProps, useFormikContext } from 'formik';
import React, { useEffect } from 'react';

import { FastCurrencyInput } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { PropertyActivityFormModel } from './models';

export interface IInvoiceTotalsForm {
  formikProps: FormikProps<PropertyActivityFormModel>;
}

export const InvoiceTotalsForm: React.FunctionComponent<
  React.PropsWithChildren<IInvoiceTotalsForm>
> = ({ formikProps }) => {
  const { values, setFieldValue } = useFormikContext<PropertyActivityFormModel>();

  useEffect(() => {
    const invoices = values.invoices;
    let totalBeforeTax = 0;
    let totalGst = 0;
    let totalPst = 0;
    let totalAmount = 0;

    invoices.forEach(x => {
      totalBeforeTax += x.pretaxAmount;
      totalGst += x.gstAmount;
      totalPst += x.pstAmount;
      totalAmount += x.totalAmount;
    });

    if (totalAmount !== values.totalAmount) {
      setFieldValue('pretaxAmount', totalBeforeTax);
      setFieldValue('gstAmount', totalGst);
      setFieldValue('pstAmount', totalPst);
      setFieldValue('totalAmount', totalAmount);
    }
  }, [setFieldValue, values.invoices, values.totalAmount]);

  return (
    <Section header="Invoices Total">
      <SectionField label="Total (before tax)" contentWidth="7">
        <FastCurrencyInput field="pretaxAmount" formikProps={formikProps} disabled />
      </SectionField>
      <SectionField label="GST amount" contentWidth="7">
        <FastCurrencyInput field="gstAmount" formikProps={formikProps} disabled />
      </SectionField>
      <SectionField label="PST amount" contentWidth="7">
        <FastCurrencyInput field="pstAmount" formikProps={formikProps} disabled />
      </SectionField>
      <SectionField label="Total amount" contentWidth="7">
        <FastCurrencyInput field="totalAmount" formikProps={formikProps} disabled />
      </SectionField>
    </Section>
  );
};
