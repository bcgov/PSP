import { Input } from 'components/common/form/';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ResearchFileNameGuide } from '../common/ResearchFileNameGuide';
import ResearchProperties from './ResearchProperties';

const AddResearchForm: React.FunctionComponent = () => {
  return (
    <>
      <Row className="py-4 no-gutters">
        <Col xs="auto" className="pr-5">
          <strong>Name this research file:</strong>
        </Col>
        <Col xs="auto">
          <LargeInput field="name" placeholder="Road name - Descriptive text" />A unique file number
          will be generated for this research file on save.
        </Col>
      </Row>
      <ResearchFileNameGuide />
      <ResearchProperties />
    </>
  );
};

export default AddResearchForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
