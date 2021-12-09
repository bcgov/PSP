import { ILease } from 'interfaces';
import * as React from 'react';
import { prettyFormatDate } from 'utils';

import LeaseStatusSummary from './LeaseStatusSummary';
import StackedPidTenantFields from './StackedPidTenantFields';
import * as Styled from './styles';

interface ILeaseHeaderProps {
  lease?: ILease;
}

/**
 * Common header for lease and licenses. Requires an active lease to display.
 * @param {ILeaseHeaderProps} param0
 */
export const LeaseHeader: React.FunctionComponent<ILeaseHeaderProps> = ({ lease }) => {
  return (
    <Styled.LeaseHeader className="lease-header">
      <Styled.LeaseH1>Lease / License</Styled.LeaseH1>
      <LeaseStatusSummary lease={lease} />
      <Styled.LeaseHeaderText>
        <label>Expiry:</label>
        <b>{prettyFormatDate(lease?.expiryDate)}</b>
      </Styled.LeaseHeaderText>
      <Styled.LeaseHeaderText>
        <b>{lease?.paymentReceivableType?.description}</b>
      </Styled.LeaseHeaderText>
      <Styled.LeaseHeaderRight>
        <StackedPidTenantFields lease={lease} />
      </Styled.LeaseHeaderRight>
    </Styled.LeaseHeader>
  );
};

export default LeaseHeader;
