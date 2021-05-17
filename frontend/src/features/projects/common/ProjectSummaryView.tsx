import { Formik } from 'formik';
import { noop } from 'lodash';
import React from 'react';
import { Container, Form } from 'react-bootstrap';
import styled from 'styled-components';

import { ReviewProjectForm } from '../dispose';
import { StepActions } from '../dispose/components/StepActions';
import { ProjectNotes, StepStatusIcon, useProject, useStepForm } from '.';
import { PublicNotes } from './components/ProjectNotes';
import StepErrorSummary from './components/StepErrorSummary';
import { IStepProps } from './interfaces';

const ProjectSummaryViewContainer = styled(Container)`
  button.btn-warning:disabled {
    display: none;
  }
`;

/**
 * Read only version of all step components. Allows notes field to be edited
 */
const ProjectSummaryView = ({ formikRef }: IStepProps) => {
  const { project } = useProject();
  const { onSubmitReview, noFetchingProjectRequests } = useStepForm();
  const initialValues = { ...project, confirmation: true };
  return (
    <ProjectSummaryViewContainer fluid className="ProjectSummaryView">
      <StepStatusIcon approvedOn={project.approvedOn} status={project.status} />
      <Formik
        initialValues={initialValues}
        enableReinitialize={true}
        innerRef={formikRef}
        onSubmit={(values, actions) => onSubmitReview(values, formikRef)}
      >
        {formikProps => (
          <Form>
            <ReviewProjectForm canEdit={false} />
            <ProjectNotes disabled={!project.status?.isActive} />
            <PublicNotes disabled={!project.status?.isActive} />
            <StepErrorSummary />
            <StepActions
              onSave={() => formikProps.submitForm()}
              onNext={noop}
              nextDisabled={true}
              saveDisabled={!project.status?.isActive}
              isFetching={!noFetchingProjectRequests}
            />
          </Form>
        )}
      </Formik>
    </ProjectSummaryViewContainer>
  );
};

export default ProjectSummaryView;
