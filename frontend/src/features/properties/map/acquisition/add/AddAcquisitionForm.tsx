import { Input } from 'components/common/form/';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { AcquisitionForm } from './models';

export interface IAddAcquisitionFormProps {
  /** Initial values of the form */
  initialValues: AcquisitionForm;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: AcquisitionForm,
    formikHelpers: FormikHelpers<AcquisitionForm>,
  ) => void | Promise<any>;
  /** Optional - callback to notify when save button is pressed. */
  onSaveClick?: (noteForm: AcquisitionForm, formikProps: FormikProps<AcquisitionForm>) => void;
  /** Optional - callback to notify when cancel button is pressed. */
  onCancelClick?: (formikProps: FormikProps<AcquisitionForm>) => void;
}

export const AddAcquisitionForm: React.FC<IAddAcquisitionFormProps> = props => {
  const { initialValues, validationSchema, onSubmit, onSaveClick, onCancelClick } = props;

  return (
    <Formik<AcquisitionForm>
      enableReinitialize
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
          <Section header="Schedule">
            <SectionField label="Assigned date:"></SectionField>
            <SectionField label="Delivery date:"></SectionField>
          </Section>

          <Section header="Acquisition Details">
            <SectionField label="Acquisition file name:">
              <LargeInput field="name" />
            </SectionField>
            <SectionField label="Physical file status:"></SectionField>
            <SectionField label="Acquisition type:"></SectionField>
            <SectionField label="Ministry region::"></SectionField>
          </Section>
        </>
      )}
    </Formik>
  );
};

export default AddAcquisitionForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
