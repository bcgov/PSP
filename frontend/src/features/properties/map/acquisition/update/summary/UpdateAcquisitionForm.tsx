import { FastDatePicker, Input, Select } from 'components/common/form';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { UpdateAcquisitionSummaryFormModel } from './models';

export interface IUpdateAcquisitionFormProps {
  /** Initial values of the form */
  initialValues: UpdateAcquisitionSummaryFormModel;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: UpdateAcquisitionSummaryFormModel,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
  ) => void | Promise<any>;
}

export const UpdateAcquisitionForm = React.forwardRef<
  FormikProps<UpdateAcquisitionSummaryFormModel>,
  IUpdateAcquisitionFormProps
>((props, ref) => {
  const { initialValues, validationSchema, onSubmit } = props;

  const { getOptionsByType } = useLookupCodeHelpers();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);

  return (
    <Formik<UpdateAcquisitionSummaryFormModel>
      enableReinitialize
      innerRef={ref}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
          <Container>
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
                <LargeInput field="fileName" />
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

            <Section header="Acquisition Team">
              {/* <AddAcquisitionTeamForm formikProps={formikProps} /> */}
            </Section>
          </Container>

          <Prompt
            when={formikProps.dirty && formikProps.submitCount === 0}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
        </>
      )}
    </Formik>
  );
});

export default UpdateAcquisitionForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 100%;
  }
`;

const Container = styled.div`
  .form-section {
    margin: 0;
    padding-left: 0;
  }

  .tab-pane {
    .form-section {
      margin: 1.5rem;
      padding-left: 1.5rem;
    }
  }

  .react-datepicker-wrapper {
    max-width: 14rem;
  }

  [name='region'] {
    max-width: 25rem;
  }
`;
