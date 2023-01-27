import { FastDatePicker, Input, TextArea } from 'components/common/form';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import { ProjectForm } from './models';

export interface IProductSubFormProps {
  nameSpace: string;
  formikProps: FormikProps<ProjectForm>;
}

export const ProductSubForm: React.FunctionComponent<IProductSubFormProps> = ({
  nameSpace,
  formikProps,
}) => {
  return (
    <>
      <Row>
        <Col>
          <SectionField label="Product code">
            <Input field={withNameSpace(nameSpace, 'code')} />
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Name" labelWidth="2">
            <Input field={withNameSpace(nameSpace, 'description')} />
          </SectionField>
        </Col>
      </Row>
      <SectionField label="Start date" labelWidth="2" contentWidth="2">
        <FastDatePicker field={withNameSpace(nameSpace, 'startDate')} formikProps={formikProps} />
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Cost estimate">
            <Input field={withNameSpace(nameSpace, 'costEstimate')} />
          </SectionField>
        </Col>
        <Col>
          <SectionField label="Estimate date">
            <FastDatePicker
              field={withNameSpace(nameSpace, 'costEstimateDate')}
              formikProps={formikProps}
            />
          </SectionField>
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
