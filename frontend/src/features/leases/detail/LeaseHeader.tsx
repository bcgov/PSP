import { PAYMENT_RECEIVABLE_CODE_SET_NAME } from 'constants/API';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
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
  const { getCodeById } = useLookupCodeHelpers();
  const paymentReceivableType = lease?.paymentReceivableTypeId
    ? getCodeById(PAYMENT_RECEIVABLE_CODE_SET_NAME, lease?.paymentReceivableTypeId)
    : '';
  return (
    <Styled.LeaseHeader className="lease-header">
      <Styled.LeaseH1>Lease / License</Styled.LeaseH1>
      <LeaseStatusSummary lease={lease} />
      <Styled.LeaseHeaderText>
        <label>Expiry:</label>
        <b>{prettyFormatDate(lease?.expiryDate)}</b>
      </Styled.LeaseHeaderText>
      <Styled.LeaseHeaderText>
        <b>{paymentReceivableType}</b>
      </Styled.LeaseHeaderText>
      <Styled.LeaseHeaderRight>
        <StackedPidTenantFields lease={lease} />
      </Styled.LeaseHeaderRight>
    </Styled.LeaseHeader>
  );
};

export default LeaseHeader;
