import { InlineInput } from 'components/common/form/styles';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import ResearchProperties from './ResearchProperties';

const AddResearchForm: React.FunctionComponent = () => {
  return (
    <>
      <Row className="py-4 no-gutters">
        <Col xs="auto" className="pr-5">
          <strong>Name this research file:</strong>
        </Col>
        <Col xs="auto">
          <LargeInlineInput field="name" />A unique file number will be generated for this research
          file on save.
        </Col>
      </Row>
      <ResearchProperties />
    </>
  );
};

export default AddResearchForm;

const LargeInlineInput = styled(InlineInput)`
  input.form-control {
    min-width: 50rem;
    max-width: 50rem;
  }
`;
