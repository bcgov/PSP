import { FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { FastCurrencyInput, FastDatePicker, Input, TextArea } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { withNameSpace } from '@/utils/formUtils';

import { ProjectForm } from '../models';

export interface IProductSubFormProps {
  index: number;
  nameSpace: string;
  formikProps: FormikProps<ProjectForm>;
}

export const ProductSubForm: React.FunctionComponent<IProductSubFormProps> = ({
  index,
  nameSpace,
  formikProps,
}) => {
  const costEstimate = formikProps.values.products[index].costEstimate;
  return (
    <>
      <Row>
        <Col>
          <SectionField label="Product code" required>
            <Input field={withNameSpace(nameSpace, 'code')} />
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Name" labelWidth="2" required>
            <Input field={withNameSpace(nameSpace, 'description')} />
          </SectionField>
        </Col>
      </Row>
      <SectionField label="Start date" labelWidth="2" contentWidth="4">
        <FastDatePicker field={withNameSpace(nameSpace, 'startDate')} formikProps={formikProps} />
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Cost estimate">
            <FastCurrencyInput
              field={withNameSpace(nameSpace, 'costEstimate')}
              formikProps={formikProps}
            />
          </SectionField>
        </Col>
        <Col>
          {costEstimate?.toString() !== '' && (
            <SectionField label="Estimate date">
              <FastDatePicker
                field={withNameSpace(nameSpace, 'costEstimateDate')}
                formikProps={formikProps}
              />
            </SectionField>
          )}
        </Col>
      </Row>
      <SectionField label="Objectives" labelWidth="12">
        <MediumTextArea field={withNameSpace(nameSpace, 'objective')} />
      </SectionField>
      <SectionField label="Scope" labelWidth="12">
        <MediumTextArea field={withNameSpace(nameSpace, 'scope')} />
      </SectionField>
    </>
  );
};

export default ProductSubForm;

export const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 80rem;
    height: 7rem;
    resize: none;
  }
`;
