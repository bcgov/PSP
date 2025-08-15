import { FormikProps } from 'formik';
import React, { useEffect } from 'react';
import { FaPlus } from 'react-icons/fa';

import { FastCurrencyInput } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';

import { ManagementActivityFormModel } from './models';

export interface IInvoiceTotalsForm {
  formikProps: FormikProps<ManagementActivityFormModel>;
  onAdd: () => void;
}

export const InvoiceTotalsForm: React.FunctionComponent<IInvoiceTotalsForm> = ({
  formikProps,
  onAdd,
}) => {
  const { values, setFieldValue } = formikProps;

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
    <Section
      header={
        <SimpleSectionHeader title="Invoices Total">
          <StyledSectionAddButton onClick={onAdd} data-testid="add-invoice-button">
            <FaPlus size="2rem" />
            &nbsp;{'Add an Invoice'}
          </StyledSectionAddButton>
        </SimpleSectionHeader>
      }
    >
      <SectionField label="Total (before tax)" contentWidth={{ xs: 7 }}>
        <FastCurrencyInput field="pretaxAmount" formikProps={formikProps} disabled />
      </SectionField>
      <SectionField label="GST amount" contentWidth={{ xs: 7 }}>
        <FastCurrencyInput field="gstAmount" formikProps={formikProps} disabled />
      </SectionField>
      <SectionField label="PST amount" contentWidth={{ xs: 7 }}>
        <FastCurrencyInput field="pstAmount" formikProps={formikProps} disabled />
      </SectionField>
      <SectionField label="Total amount" contentWidth={{ xs: 7 }}>
        <FastCurrencyInput field="totalAmount" formikProps={formikProps} disabled />
      </SectionField>
    </Section>
  );
};
