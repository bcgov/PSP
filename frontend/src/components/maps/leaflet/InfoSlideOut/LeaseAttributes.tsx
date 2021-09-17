import { Label } from 'components/common/Label';
import { ILease } from 'interfaces';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';

import { OuterRow } from './InfoContent';
import { ThreeColumnItem } from './ThreeColumnItem';

interface ILeaseAttributes {
  /** the selected lease information */
  leases?: ILease[];
}

/**
 * Displays lease specific information needed on the information slide out
 * @param leases the selected lease data for this property
 */
export const LeaseAttributes: React.FC<ILeaseAttributes> = ({ leases }) => {
  return leases?.length ? (
    <>
      <ListGroup>
        <Label className="header">Active Leases</Label>
        {leases?.map(lease => (
          <OuterRow key={`lease-${lease.id}`}>
            <ThreeColumnItem
              leftSideLabel={'Lease Expiry Date:'}
              rightSideItem={lease?.expiryDate ?? 'N/A'}
            />
            <ThreeColumnItem
              leftSideLabel={'Tenant Name:'}
              rightSideItem={lease?.tenantName ?? ''}
            />
          </OuterRow>
        ))}
      </ListGroup>
    </>
  ) : null;
};

export default LeaseAttributes;
