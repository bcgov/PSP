import { FastDatePicker, Input, Select } from 'components/common/form';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { UpdateAcquisitionTeamSubForm } from '../../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { UpdateAcquisitionSummaryFormModel } from './models';
import StatusToolTip from './StatusToolTip';

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
>((props, formikRef) => {
  const { initialValues, validationSchema, onSubmit } = props;

  const { getOptionsByType } = useLookupCodeHelpers();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);
  const fileStatusTypeCodes = getOptionsByType(API.ACQUISITION_FILE_STATUS_TYPES);

  return (
    <Formik<UpdateAcquisitionSummaryFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
          <Container>
            <Section>
              <SectionField label="Status" helpText={<StatusToolTip />} helpTextPlacement="auto">
                <Select
                  field="fileStatusTypeCode"
                  options={fileStatusTypeCodes}
                  placeholder="Select..."
                  required
                />
              </SectionField>
            </Section>

            <Section header="Schedule">
              <SectionField label="Assigned date">
                <FastDatePicker field="assignedDate" formikProps={formikProps} />
              </SectionField>
              <SectionField
                label="Delivery date"
                helpText="Date for delivery of the property to the project"
              >
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
              <UpdateAcquisitionTeamSubForm />
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
  background-color: ${props => props.theme.css.filterBackgroundColor};

  .react-datepicker-wrapper {
    max-width: 14rem;
  }

  [name='region'] {
    max-width: 25rem;
  }
`;
