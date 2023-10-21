import { ArrayHelpers, FieldArray, FormikProps, getIn } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';

import { StyledSectionAddButton } from '@/components/common/styles';

import { InvoiceForm } from './InvoiceForm';
import { ActivityInvoiceFormModel, PropertyActivityFormModel } from './models';

export interface IInvoiceListForm {
  field: string;
  formikProps: FormikProps<PropertyActivityFormModel>;
  gstConstant: number;
  pstConstant: number;
}

export const InvoiceListForm: React.FunctionComponent<
  React.PropsWithChildren<IInvoiceListForm>
> = ({ field, formikProps, gstConstant, pstConstant }) => {
  // clear out existing values instead of removing last item from array
  const onRemove = (index: number, arrayHelpers: ArrayHelpers) => {
    arrayHelpers.remove(index);
    return;
  };

  const invoices = getIn(formikProps.values, field) as ActivityInvoiceFormModel[];
  return (
    <FieldArray name={field}>
      {arrayHelpers => (
        <>
          <Row className="justify-content-end no-gutters">
            <Col className="col-auto pr-4">
              <StyledSectionAddButton
                onClick={() => arrayHelpers.push(new ActivityInvoiceFormModel())}
              >
                <FaPlus size="2rem" />
                &nbsp;{'Add an Invoice'}
              </StyledSectionAddButton>
            </Col>
          </Row>
          {invoices.map((invoice, index) => (
            <InvoiceForm
              key={`activity-${invoice.propertyActivityId}-invoice-${index}`}
              namespace={`${field}.${index}`}
              index={index}
              formikProps={formikProps}
              onDelete={() => onRemove(index, arrayHelpers)}
              gstConstant={gstConstant}
              pstConstant={pstConstant}
            />
          ))}
        </>
      )}
    </FieldArray>
  );
};
