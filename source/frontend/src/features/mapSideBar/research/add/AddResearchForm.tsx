import { useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Input } from '@/components/common/form/';
import { Section } from '@/components/common/Section/Section';

import { PropertyForm } from '../../shared/models';
import PropertiesListContainer from '../../shared/update/properties/PropertiesListContainer';
import { ResearchFileNameGuide } from '../common/ResearchFileNameGuide';
import { UpdateProjectsSubForm } from '../common/updateProjects/UpdateProjectsSubForm';
import { ResearchForm } from './models';

export interface IAddResearchFormProps {
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const AddResearchForm: React.FC<IAddResearchFormProps> = props => {
  const { values } = useFormikContext<ResearchForm>();

  return (
    <StyledFormWrapper>
      <Section>
        <Row className="py-4 no-gutters">
          <Col xs="auto" className="pr-5">
            <strong>Name this research file:</strong>
          </Col>
          <Col xs="auto">
            <LargeInput
              field="name"
              placeholder="Road name - MOTT District - Location or description"
            />
            A unique file number will be generated for this research file on save.
          </Col>
        </Row>
        <ResearchFileNameGuide />
      </Section>
      <Section header="Project">
        <UpdateProjectsSubForm field="researchFileProjects" />
      </Section>

      <Section header="Properties to include in this file:">
        <PropertiesListContainer
          properties={values.properties}
          verifyCanRemove={(_, removeCallback) => removeCallback()}
          needsConfirmationBeforeAdd={props.confirmBeforeAdd}
          canUploadShapefiles={false}
        />
      </Section>
    </StyledFormWrapper>
  );
};

export default AddResearchForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
