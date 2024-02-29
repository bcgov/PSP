import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton } from '@/components/common/buttons';

export interface ILeaseTenantsProps {
  tenantNames: string[];
  maxDisplayCount: number;
}

const LeaseTenants: React.FunctionComponent<
  React.PropsWithChildren<ILeaseTenantsProps>
> = props => {
  const [isExpanded, setExpanded] = useState(false);

  const names = props.tenantNames;
  const maxDisplayCount = props.maxDisplayCount;

  let displayNames: string[] = [];
  if (!isExpanded) {
    displayNames = names.slice(0, maxDisplayCount);
  } else {
    displayNames = names;
  }

  const rowItems = displayNames.map((tenantName, index) => {
    const isLastItem = index + 1 === names.length;
    return (
      <Row key={index} className={`mx-0 ${isLastItem ? '' : 'border-bottom'}`}>
        <Col md="auto">
          <div> {tenantName}</div>
        </Col>
      </Row>
    );
  });

  if (names.length > rowItems.length || isExpanded) {
    rowItems.push(
      <Row key="showMoreKey" className="mx-0 align-items-end">
        <Col />
        <Col md="auto">
          <LinkButton onClick={() => setExpanded(!isExpanded)}>
            {isExpanded ? 'hide' : `[+${names.length - rowItems.length} more...]`}
          </LinkButton>
        </Col>
      </Row>,
    );
  }

  return <div className="w-100">{rowItems}</div>;
};

export default LeaseTenants;
