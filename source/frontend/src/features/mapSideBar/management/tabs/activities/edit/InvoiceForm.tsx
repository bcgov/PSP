import { FormikProps } from 'formik';
import React, { ChangeEvent, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastCurrencyInput, FastDatePicker, Input, TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { ManagementActivityFormModel } from './models';

export interface IInvoiceForm {
  namespace: string;
  index: number;
  formikProps: FormikProps<ManagementActivityFormModel>;
  onDelete: () => void;
  gstConstant: number;
  pstConstant: number;
}

export const InvoiceForm: React.FunctionComponent<IInvoiceForm> = ({
  namespace,
  index,
  formikProps,
  onDelete,
  gstConstant,
  pstConstant,
}) => {
  const { values, setFieldValue } = formikProps;

  const [customGst, setCustomGst] = useState<number | null>(null);
  const [customPst, setCustomPst] = useState<number | null>(null);

  useEffect(() => {
    const pretaxAmount = values.invoices[index].pretaxAmount;
    const isPstRequired = values.invoices[index].isPstRequired;
    const gstAmount = customGst === null ? pretaxAmount * gstConstant : customGst;

    const pstAmount =
      (customPst === null ? pretaxAmount * pstConstant : customPst) * (isPstRequired ? 1 : 0);
    const totalValue = pretaxAmount + gstAmount + pstAmount;

    if (totalValue !== values.invoices[index].totalAmount) {
      setFieldValue(`${namespace}.gstAmount`, gstAmount);
      setFieldValue(`${namespace}.pstAmount`, pstAmount);
      setFieldValue(`${namespace}.totalAmount`, totalValue);
    }
  }, [
    customGst,
    customPst,
    gstConstant,
    index,
    namespace,
    pstConstant,
    setFieldValue,
    values.invoices,
  ]);

  const onAmountChange = async () => {
    setCustomGst(null);
    setCustomPst(null);
  };

  const onGstChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const regex = /[^0-9.-]/g;
    const cleanValue = changeEvent.target.value.replace(regex, '');
    const gstValue = parseFloat(cleanValue);
    if (isNaN(gstValue)) {
      setCustomGst(null);
    } else {
      setCustomGst(gstValue ?? null);
    }
  };

  const onPstChange = async (changeEvent: ChangeEvent<HTMLInputElement>) => {
    const regex = /[^0-9.-]/g;
    const cleanValue = changeEvent.target.value.replace(regex, '');
    const pstValue = parseFloat(cleanValue);
    if (isNaN(pstValue)) {
      setCustomPst(null);
    } else {
      setCustomPst(pstValue ?? null);
    }
  };

  return (
    <Section header={`Invoice ${index + 1}`} isCollapsable initiallyExpanded>
      <SectionField label="Invoice number" contentWidth={{ xs: 7 }}>
        <Row className="no-gutters">
          <Col className="col-10">
            <Input field={`${namespace}.invoiceNum`} />
          </Col>
          <Col className="col-1 pl-4">
            <StyledRemoveLinkButton
              title="invoice delete"
              data-testid="invoice-delete-button"
              icon={<FaTrash size={24} id={`invoice-delete-${index}`} title="invoice delete" />}
              onClick={onDelete}
            />
          </Col>
        </Row>
      </SectionField>

      <SectionField label="Invoice date" contentWidth={{ xs: 7 }} required>
        <FastDatePicker field={`${namespace}.invoiceDateTime`} formikProps={formikProps} />
      </SectionField>

      <SectionField label="Description" contentWidth={{ xs: 12 }} required>
        <TextArea field={`${namespace}.description`} />
      </SectionField>

      <SectionField label="Amount (before tax)" contentWidth={{ xs: 7 }} required>
        <FastCurrencyInput
          field={`${namespace}.pretaxAmount`}
          formikProps={formikProps}
          onChange={onAmountChange}
        />
      </SectionField>

      <SectionField label="GST amount" contentWidth={{ xs: 7 }}>
        <FastCurrencyInput
          field={`${namespace}.gstAmount`}
          formikProps={formikProps}
          onChange={onGstChange}
        />
      </SectionField>

      <SectionField label="PST applicable?" contentWidth={{ xs: 7 }}>
        <YesNoSelect field={`${namespace}.isPstRequired`} notNullable />
      </SectionField>

      <SectionField
        label="PST amount"
        contentWidth={{ xs: 7 }}
        required={formikProps.values.invoices[index].isPstRequired}
      >
        <FastCurrencyInput
          field={`${namespace}.pstAmount`}
          formikProps={formikProps}
          onChange={onPstChange}
          disabled={!formikProps.values.invoices[index].isPstRequired}
        />
      </SectionField>

      <SectionField label="Total amount" contentWidth={{ xs: 7 }}>
        <FastCurrencyInput field={`${namespace}.totalAmount`} formikProps={formikProps} disabled />
      </SectionField>
    </Section>
  );
};
