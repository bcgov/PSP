import { ILeaseProperty } from 'interfaces';
import { useState } from 'react';
import { Button, Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

const PropertyRow = styled(Row)`
  border-radius: 0.4rem;
`;

export interface ILeasePropertiesProps {
  properties: ILeaseProperty[];
  maxDisplayCount: number;
}

const LeaseProperties: React.FunctionComponent<ILeasePropertiesProps> = props => {
  const [isExpanded, setExpanded] = useState(false);

  const properties = props.properties;
  const maxDisplayCount = props.maxDisplayCount;

  let displayProperties: ILeaseProperty[] = [];
  if (!isExpanded) {
    displayProperties = properties.slice(0, maxDisplayCount);
  } else {
    displayProperties = properties;
  }

  let rowItems = displayProperties.map((property, index) => {
    return (
      <PropertyRow key={index + property.id} className="mx-0 my-2 border border-secondary">
        <Col md="12">
          <strong className="pr-2">Address:</strong>
          {property.address}
        </Col>
        <Col md="auto">
          <div>
            <strong className="pr-2">PID:</strong> {property.pid || 'N.A'}
          </div>
        </Col>
        <Col md="auto">
          <div>
            <strong className="pr-2">PIN:</strong> {property.pin || 'N.A'}
          </div>
        </Col>
      </PropertyRow>
    );
  });

  if (properties.length > rowItems.length || isExpanded) {
    rowItems.push(
      <Row key="showMoreKey">
        <Col />
        <Col md="auto">
          <Button variant="link" onClick={() => setExpanded(!isExpanded)}>
            {isExpanded ? 'hide' : `[+${properties.length - rowItems.length} more...]`}
          </Button>
        </Col>
      </Row>,
    );
  }

  return <div className="w-100">{rowItems}</div>;
};

export default LeaseProperties;
