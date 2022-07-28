import { FastDatePicker, Input, Select } from 'components/common/form/';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import React from 'react';
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
}

export const AddAcquisitionForm = React.forwardRef<
  FormikProps<AcquisitionForm>,
  IAddAcquisitionFormProps
>((props, ref) => {
  const { initialValues, validationSchema, onSubmit } = props;

  const { getOptionsByType } = useLookupCodeHelpers();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);

  return (
    <Formik<AcquisitionForm>
      enableReinitialize
      innerRef={ref}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
          <Section header="Schedule">
            <SectionField label="Assigned date">
              <FastDatePicker field="assignedDate" formikProps={formikProps} />
            </SectionField>
            <SectionField label="Delivery date">
              <FastDatePicker field="deliveryDate" formikProps={formikProps} />
            </SectionField>
          </Section>

          <Section header="Acquisition Details">
            <SectionField label="Acquisition file name">
              <LargeInput field="name" />
            </SectionField>
            <SectionField label="Physical file status">
              <Select
                field="acquisitionPhysFileStatusType"
                options={acquisitionPhysFileTypes}
                placeholder="Select..."
              />
            </SectionField>
            <SectionField label="Acquisition type">
              <Select
                field="acquisitionType"
                options={acquisitionTypes}
                placeholder="Select..."
                required
              />
            </SectionField>
            <SectionField label="Ministry region">
              <Select
                field="region"
                options={regionTypes}
                placeholder="Select region..."
                required
              />
            </SectionField>
          </Section>
        </>
      )}
    </Formik>
  );
});

export default AddAcquisitionForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
