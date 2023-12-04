import { Formik, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { Select } from '@/components/common/form';
import { FastDatePicker } from '@/components/common/form/FastDatePicker';
import { Input } from '@/components/common/form/Input';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';

import { AddDispositionFormYupSchema } from '../models/AddDispositionFormYupSchema';
import { DispositionFormModel } from '../models/DispositionFormModel';
import DispositionPropertiesSubForm from './DispositionPropertiesSubForm';
import DispositionTeamSubForm from './DispositionTeamSubForm';

export interface IDispositionFormProps {
  initialValues: DispositionFormModel;
  onSubmit: (
    values: DispositionFormModel,
    setSubmitting: (isSubmitting: boolean) => void,
    userOverrides: UserOverrideCode[],
  ) => void | Promise<any>;
}

const DispositionForm = React.forwardRef<FormikProps<DispositionFormModel>, IDispositionFormProps>(
  (props, ref) => {
    const { initialValues, onSubmit } = props;
    const { getOptionsByType } = useLookupCodeHelpers();

    const dispositionFundingTypes = getOptionsByType(API.DISPOSITION_FUNDING_TYPES);
    const dispositionStatusTypesOptions = getOptionsByType(API.DISPOSITION_STATUS_TYPES);
    const dispositionTypesOptions = getOptionsByType(API.DISPOSITION_TYPES);
    const dispositionInitiatingBranchTypesOptions = getOptionsByType(
      API.DISPOSITION_INITIATING_BRANCH_TYPES,
    );
    const dispositionInitiatingDocTypesOptions = getOptionsByType(
      API.DISPOSITION_INITIATING_DOC_TYPES,
    );
    const dispositionPhysicalFileStatusOptions = getOptionsByType(
      API.DISPOSITION_PHYSICAL_STATUS_TYPES,
    );

    return (
      <Formik<DispositionFormModel>
        enableReinitialize
        innerRef={ref}
        initialValues={initialValues}
        validationSchema={AddDispositionFormYupSchema}
        onSubmit={async (values, formikHelpers) => {
          onSubmit(values, formikHelpers.setSubmitting, []);
        }}
      >
        {formikProps => {
          return (
            <>
              <Container>
                <Section header="Project">
                  <SectionField label="Ministry project"></SectionField>
                  <SectionField label="Product"></SectionField>
                  <SectionField label="Funding">
                    <Select
                      field="fundingTypeCode"
                      options={dispositionFundingTypes}
                      placeholder="Select funding..."
                    />
                  </SectionField>
                </Section>

                <Section header="Schedule">
                  <SectionField label="Assigned date">
                    <FastDatePicker field="assignedDate" formikProps={formikProps} />
                  </SectionField>
                  <SectionField label="Disposition completed date">
                    <FastDatePicker field="completionDate" formikProps={formikProps} />
                  </SectionField>
                </Section>

                <Section header="Properties to include in this file:">
                  <DispositionPropertiesSubForm formikProps={formikProps} />
                </Section>

                <Section header="Disposition Details">
                  <SectionField label="Disposition file name">
                    <Input field="fileName" />
                  </SectionField>
                  <SectionField
                    label="Reference number"
                    tooltip="Provide available reference number for historic program or file number (e.g.  RAEG, Acquisition File, etc.)."
                  >
                    <Input field="referenceNumber" />
                  </SectionField>
                  <SectionField label="Disposition status" required>
                    <Select
                      field="dispositionStatusTypeCode"
                      options={dispositionStatusTypesOptions}
                      required
                    />
                  </SectionField>

                  <SectionField label="Disposition type" required>
                    <Select
                      field="dispositionTypeCode"
                      options={dispositionTypesOptions}
                      placeholder="Select..."
                      required
                    />
                  </SectionField>
                  {formikProps.values?.dispositionTypeCode === 'OTHER' && (
                    <SectionField label="Other (Disposition Type)" required>
                      <Input field="dispositionTypeOther" required />
                    </SectionField>
                  )}

                  <SectionField
                    label="Initiating document"
                    tooltip="Provide the type of document that has initiated the disposition process."
                  >
                    <Select
                      field="initiatingDocumentTypeCode"
                      options={dispositionInitiatingDocTypesOptions}
                      placeholder="Select..."
                      required
                    />
                  </SectionField>
                  {formikProps.values?.initiatingDocumentTypeCode === 'OTHER' && (
                    <SectionField label="Other (Initiating Document)" required>
                      <Input field="initiatingDocumentTypeOther" required />
                    </SectionField>
                  )}
                  <SectionField
                    label="Initiating document date"
                    tooltip="Provide the date initiating document was signed off."
                  >
                    <FastDatePicker field="initiatingDocumentDate" formikProps={formikProps} />
                  </SectionField>

                  <SectionField label="Physical file status">
                    <Select
                      field="physicalFileStatusTypeCode"
                      options={dispositionPhysicalFileStatusOptions}
                      placeholder="Select file status..."
                    />
                  </SectionField>
                  <SectionField label="Initiating branch">
                    <Select
                      field="initiatingBranchTypeCode"
                      options={dispositionInitiatingBranchTypesOptions}
                      placeholder="Select..."
                    />
                  </SectionField>
                  <SectionField label="Ministry region" required>
                    <UserRegionSelectContainer
                      field="regionCode"
                      placeholder="Select region..."
                      required
                    />
                  </SectionField>
                </Section>

                <Section header="Disposition Team">
                  <DispositionTeamSubForm />
                </Section>
              </Container>
            </>
          );
        }}
      </Formik>
    );
  },
);

export default DispositionForm;

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

  [name='region'] {
    max-width: 25rem;
  }
`;
