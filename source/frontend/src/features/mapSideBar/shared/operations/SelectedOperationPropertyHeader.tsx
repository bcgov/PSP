import { Col } from 'react-bootstrap';

import { HeaderRow } from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';

export const SelectedOperationPropertyHeader: React.FunctionComponent = () => {
  return (
    <HeaderRow className="no-gutters">
      <Col md={3}>Identifier</Col>
      <Col md={2}>Plan</Col>
      <Col md={3}>
        Area m<sup>2</sup>
      </Col>
      <Col md={3}>Address</Col>
      <Col md={1}></Col>
    </HeaderRow>
  );
};

export default SelectedOperationPropertyHeader;
