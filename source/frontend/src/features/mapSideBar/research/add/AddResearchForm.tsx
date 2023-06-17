import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Input } from '@/components/common/form/';
import { Section } from '@/components/common/Section/Section';

import { ResearchFileNameGuide } from '../common/ResearchFileNameGuide';
import { UpdateProjectsSubForm } from '../common/updateProjects/UpdateProjectsSubForm';
import ResearchProperties from './ResearchProperties';

const AddResearchForm: React.FC = () => {
  return (
    <StyledFormWrapper>
      <Section>
        <Row className="py-4 no-gutters">
          <Col xs="auto" className="pr-5">
            <strong>Name this research file:</strong>
          </Col>
          <Col xs="auto">
            <LargeInput field="name" placeholder="Road name - Descriptive text" />A unique file
            number will be generated for this research file on save.
          </Col>
        </Row>
        <ResearchFileNameGuide />
      </Section>
      <Section header="Project">
        <UpdateProjectsSubForm field="researchFileProjects" />
      </Section>
      <ResearchProperties />
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
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
