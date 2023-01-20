import { AsyncTypeahead, FastDatePicker, Input, Select } from 'components/common/form';
import TooltipIcon from 'components/common/TooltipIcon';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { UpdateAcquisitionTeamSubForm } from '../../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { useProjectTypeahead } from '../../hooks/useProjectTypeahead';
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
  const acquisitionFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);

  const { handleTypeaheadSearch, isTypeaheadLoading, matchedProjects } = useProjectTypeahead();

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
              <SectionField
                label="Status"
                tooltip={
                  <TooltipIcon
                    className="tooltip-light"
                    toolTipId="status-field-tooltip"
                    toolTip={<StatusToolTip />}
                    placement="auto"
                  />
                }
              >
                <Select
                  field="fileStatusTypeCode"
                  options={fileStatusTypeCodes}
                  placeholder="Select..."
                  required
                />
              </SectionField>
            </Section>

            <Section header="Project">
              <SectionField label="Ministry project">
                <AsyncTypeahead
                  field="project"
                  labelKey="text"
                  isLoading={isTypeaheadLoading}
                  options={matchedProjects}
                  onSearch={handleTypeaheadSearch}
                />
              </SectionField>
              <SectionField label="Product">
                <Select field="product" options={[]} placeholder="Select..." />
              </SectionField>
              <SectionField label="Funding">
                <Select
                  field="fundingTypeCode"
                  options={acquisitionFundingTypes}
                  placeholder="Select..."
                  onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                    const selectedValue = [].slice
                      .call(e.target.selectedOptions)
                      .map((option: HTMLOptionElement & number) => option.value)[0];
                    if (!!selectedValue && selectedValue !== 'OTHER') {
                      formikProps.setFieldValue('fundingTypeOtherDescription', '');
                    }
                  }}
                />
              </SectionField>
              {formikProps.values?.fundingTypeCode === 'OTHER' && (
                <SectionField label="Other funding">
                  <Input field="fundingTypeOtherDescription" />
                </SectionField>
              )}
            </Section>

            <Section header="Schedule">
              <SectionField label="Assigned date">
                <FastDatePicker field="assignedDate" formikProps={formikProps} />
              </SectionField>
              <SectionField
                label="Delivery date"
                tooltip="Date for delivery of the property to the project"
              >
                <FastDatePicker field="deliveryDate" formikProps={formikProps} />
              </SectionField>
            </Section>

            <Section header="Acquisition Details">
              <SectionField label="Acquisition file name">
                <Input field="fileName" />
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

const Container = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};

  .react-datepicker-wrapper {
    max-width: 14rem;
  }

  [name='region'] {
    max-width: 25rem;
  }
`;
