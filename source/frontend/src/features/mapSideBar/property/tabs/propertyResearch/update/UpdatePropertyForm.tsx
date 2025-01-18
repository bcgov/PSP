import { Formik, FormikHelpers, FormikProps } from 'formik';

import { Input, Multiselect, Select, SelectOption, TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { PropertyResearchFilePurposeFormModel, UpdatePropertyFormModel } from './models';

export interface IUpdatePropertyResearchFormProps {
  formikRef: React.RefObject<FormikProps<UpdatePropertyFormModel>>;
  /** Initial values of the form */
  initialValues: UpdatePropertyFormModel;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: UpdatePropertyFormModel,
    formikHelpers: FormikHelpers<UpdatePropertyFormModel>,
  ) => void | Promise<any>;
}

const UpdatePropertyForm: React.FunctionComponent<IUpdatePropertyResearchFormProps> = props => {
  const { formikRef, initialValues, validationSchema, onSubmit } = props;
  const { getByType } = useLookupCodeHelpers();

  const opinionOptions: SelectOption[] = [
    { label: 'Unknown', value: 'unknown' },
    { label: 'Yes', value: 'yes' },
    { label: 'No', value: 'no' },
  ];

  const propertyResearchPurposeOptions = getByType(API.PROPERTY_RESEARCH_PURPOSE_TYPES).map(x =>
    PropertyResearchFilePurposeFormModel.fromLookup(x),
  );

  return (
    <Formik<UpdatePropertyFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      <StyledSummarySection>
        <Section header="Property of Interest">
          <SectionField label="Descriptive name">
            <Input field="propertyName" />
          </SectionField>
          <SectionField label="Purpose">
            <Multiselect
              field="propertyResearchPurposeTypes"
              displayValue="propertyPurposeTypeDescription"
              placeholder="Select Property Purpose"
              options={propertyResearchPurposeOptions}
              hidePlaceholder
            />
          </SectionField>
          <SectionField label="Legal opinion req'd?">
            <Select field="isLegalOpinionRequired" options={opinionOptions} />
          </SectionField>
          <SectionField label="Legal opinion obtained?">
            <Select field="isLegalOpinionObtained" options={opinionOptions} />
          </SectionField>
          <SectionField label="Document reference">
            <Input field="documentReference" />
          </SectionField>
        </Section>

        <Section header="Research Summary">
          <SectionField label="Summary comments" />
          <TextArea field="researchSummary" />
        </Section>
      </StyledSummarySection>
    </Formik>
  );
};

export default UpdatePropertyForm;
