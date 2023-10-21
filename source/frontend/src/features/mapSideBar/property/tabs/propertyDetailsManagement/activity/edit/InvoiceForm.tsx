import { FormikProps, useFormikContext } from 'formik';
import React, { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastCurrencyInput, FastDatePicker, Input, TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';

import { PropertyActivityFormModel } from './models';

export interface IInvoiceForm {
  namespace: string;
  index: number;
  formikProps: FormikProps<PropertyActivityFormModel>;
  onDelete: () => void;
  gstConstant: number;
  pstConstant: number;
}

export const InvoiceForm: React.FunctionComponent<React.PropsWithChildren<IInvoiceForm>> = ({
  namespace,
  index,
  formikProps,
  onDelete,
  gstConstant,
  pstConstant,
}) => {
  const { values, setFieldValue } = useFormikContext<PropertyActivityFormModel>();

  useEffect(() => {
    const pretaxAmount = values.invoices[index].pretaxAmount;
    const isPstRequired = values.invoices[index].isPstRequired;
    const gstAmount = pretaxAmount * gstConstant;
    const pstAmount = pretaxAmount * pstConstant * (isPstRequired ? 1 : 0);
    const totalValue = pretaxAmount + gstAmount + pstAmount;

    if (totalValue !== values.invoices[index].totalAmount) {
      setFieldValue(`${namespace}.gstAmount`, gstAmount);
      setFieldValue(`${namespace}.pstAmount`, pstAmount);
      setFieldValue(`${namespace}.totalAmount`, totalValue);
    }
  }, [gstConstant, index, namespace, pstConstant, setFieldValue, values.invoices]);

  return (
    <Section header={`Invoice ${index + 1}`} isCollapsable initiallyExpanded>
      <Row>
        <Col className="col-10">
          <SectionField label="Invoice number" labelWidth="4" contentWidth="8">
            <Input field={`${namespace}.invoiceNum`} className="pl-5" />
          </SectionField>
        </Col>
        <Col className="col-auto">
          <StyledRemoveLinkButton
            title="invoice delete"
            data-testid="invoice-delete-button"
            icon={<FaTrash size={24} id={`invoice-delete-${index}`} title="invoice delete" />}
            onClick={onDelete}
          />
        </Col>
      </Row>

      <SectionField label="Invoice date" contentWidth="7" required>
        <FastDatePicker field={`${namespace}.invoiceDateTime`} formikProps={formikProps} />
      </SectionField>

      <SectionField label="Description" contentWidth="12" required>
        <TextArea field={`${namespace}.description`} />
      </SectionField>

      <SectionField label="Amount (before tax)" contentWidth="7" required>
        <FastCurrencyInput field={`${namespace}.pretaxAmount`} formikProps={formikProps} />
      </SectionField>

      <SectionField label="GST amount" contentWidth="7">
        <FastCurrencyInput field={`${namespace}.gstAmount`} formikProps={formikProps} disabled />
      </SectionField>

      <SectionField label="PST applicable?" contentWidth="7">
        <YesNoSelect field={`${namespace}.isPstRequired`} notNullable />
      </SectionField>

      <SectionField label="PST amount" contentWidth="7">
        <FastCurrencyInput field={`${namespace}.pstAmount`} formikProps={formikProps} disabled />
      </SectionField>

      <SectionField label="Total amount" contentWidth="7">
        <FastCurrencyInput field={`${namespace}.totalAmount`} formikProps={formikProps} disabled />
      </SectionField>
    </Section>
  );
};
