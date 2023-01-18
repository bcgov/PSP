import { Select } from 'components/common/form';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import React from 'react';
import { Container } from 'react-bootstrap';

import { ProjectForm } from './models';

export interface IAddProjectFormProps {
  /** Initial values of the form */
  initialValues: ProjectForm;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => void | Promise<any>;
}

export const AddProjectForm = React.forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>(
  (props, ref) => {
    const { getOptionsByType } = useLookupCodeHelpers();
    const projectStatusTypeCodes = getOptionsByType(API.PROJECT_STATUS_TYPES);

    const { initialValues, validationSchema, onSubmit } = props;

    const handleSubmit = (values: ProjectForm, formikHelpers: FormikHelpers<ProjectForm>) => {
      onSubmit(values, formikHelpers);
    };

    return (
      <Formik<ProjectForm>
        enableReinitialize
        innerRef={ref}
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <>
            <Container>
              <Section header="Project">
                <SectionField label="Status">
                  <Select
                    field="projectStatusType"
                    options={projectStatusTypeCodes}
                    placeholder="Select..."
                  />
                </SectionField>
              </Section>
            </Container>
          </>
        )}
      </Formik>
    );
  },
);

export default AddProjectForm;
