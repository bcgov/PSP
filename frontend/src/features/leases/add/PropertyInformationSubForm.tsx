import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseH2 } from '../detail/styles';
import { PropertyRows } from './PropertyRows';
import * as Styled from './styles';

export interface IPropertyInformationSubFormProps {}

const PropertyInformationSubForm: React.FunctionComponent<IPropertyInformationSubFormProps> = () => {
  return (
    <>
      <Row>
        <Col>
          <LeaseH2>Property Information</LeaseH2>
        </Col>
      </Row>
      <PropertyRows />
      <Row>
        <Col>
          <Styled.LargeTextArea label="Description" field="description" />
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.LargeTextArea label="Notes" field="note" />
        </Col>
      </Row>
    </>
  );
};

export default PropertyInformationSubForm;
