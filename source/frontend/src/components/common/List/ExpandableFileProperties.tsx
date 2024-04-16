import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { formatApiAddress } from '@/utils';

export interface IExpandableFilePropertiesProps {
  fileProperties?: ApiGen_Concepts_FileProperty[];
  maxDisplayCount: number;
}

const ExpandableFileProperties: React.FunctionComponent<IExpandableFilePropertiesProps> = props => {
  const [isExpanded, setExpanded] = useState(false);

  const fileProperties = props.fileProperties || [];
  const maxDisplayCount = props.maxDisplayCount;

  let displayProperties: ApiGen_Concepts_FileProperty[] = [];
  if (!isExpanded) {
    displayProperties = fileProperties.slice(0, maxDisplayCount);
  } else {
    displayProperties = fileProperties;
  }

  const rowItems = displayProperties.map((property, index) => {
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

  if (fileProperties.length > rowItems.length || isExpanded) {
    rowItems.push(
      <Row key="showMoreKey">
        <Col />
        <Col md="auto">
          <LinkButton onClick={() => setExpanded(!isExpanded)}>
            {isExpanded ? 'hide' : `[+${fileProperties.length - rowItems.length} more...]`}
          </LinkButton>
        </Col>
      </Row>,
    );
  }

  return <div className="w-100">{rowItems}</div>;
};

export default ExpandableFileProperties;

const PropertyRow = styled(Row)`
  border-radius: 0.4rem;
`;
