import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseH3 } from '../detail/styles';
import * as Styled from './styles';

interface IReferenceSubFormProps {}

const ReferenceSubForm: React.FunctionComponent<IReferenceSubFormProps> = props => {
  return (
    <>
      <Row>
        <Col>
          <LeaseH3>Reference Information</LeaseH3>
        </Col>
      </Row>

      <Row>
        <Col>
          <Styled.MediumTextArea
            label="Location of documents:"
            field="documentationReference"
            tooltip="Use this space to paste in links or system paths to relevant documents"
          />
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.LargeInlineInput label="LIS #" field="tfaFileNo" type="number" />
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.LargeInlineInput label="PS #:" field="psFileNo" />
        </Col>
      </Row>
    </>
  );
};

export default ReferenceSubForm;
