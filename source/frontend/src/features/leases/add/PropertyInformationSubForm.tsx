import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseH3 } from '../detail/styles';
import { PropertyRows } from './PropertyRows';

export interface IPropertyInformationSubFormProps {}

const PropertyInformationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IPropertyInformationSubFormProps>
> = () => {
  return (
    <>
      <Row>
        <Col>
          <LeaseH3>Property Information</LeaseH3>
        </Col>
      </Row>
      <PropertyRows />
    </>
  );
};

export default PropertyInformationSubForm;
