import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { Api_AcquisitionFileProperty } from '@/models/api/AcquisitionFile';
import { formatApiAddress } from '@/utils';

export interface IAcquisitionPropertiesProps {
  acquisitionProperties?: Api_AcquisitionFileProperty[];
  maxDisplayCount: number;
}

const AcquisitionProperties: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionPropertiesProps>
> = props => {
  const [isExpanded, setExpanded] = useState(false);

  const acquisitionProperties = props.acquisitionProperties || [];
  const maxDisplayCount = props.maxDisplayCount;

  let displayProperties: Api_AcquisitionFileProperty[] = [];
  if (!isExpanded) {
    displayProperties = acquisitionProperties.slice(0, maxDisplayCount);
  } else {
    displayProperties = acquisitionProperties;
  }

  let rowItems = displayProperties.map((property, index) => {
    return (
      <PropertyRow key={index} className="mx-0 my-2 border border-secondary">
        <Col md="12">
          <strong className="pr-2">Address:</strong>
          {formatApiAddress(property.property?.address)}
        </Col>
        {property.property?.pid && (
          <Col md="auto">
            <div>
              <strong className="pr-2">PID:</strong> {property.property.pid}
            </div>
          </Col>
        )}
        {property.property?.pin && (
          <Col md="auto">
            <div>
              <strong className="pr-2">PIN:</strong> {property.property.pin}
            </div>
          </Col>
        )}
      </PropertyRow>
    );
  });

  if (acquisitionProperties.length > rowItems.length || isExpanded) {
    rowItems.push(
      <Row key="showMoreKey">
        <Col />
        <Col md="auto">
          <LinkButton onClick={() => setExpanded(!isExpanded)}>
            {isExpanded ? 'hide' : `[+${acquisitionProperties.length - rowItems.length} more...]`}
          </LinkButton>
        </Col>
      </Row>,
    );
  }

  return <div className="w-100">{rowItems}</div>;
};

export default AcquisitionProperties;

const PropertyRow = styled(Row)`
  border-radius: 0.4rem;
`;
