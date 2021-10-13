import { PAYMENT_RECEIVABLE_CODE_SET_NAME } from 'constants/API';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { ILease } from 'interfaces';
import moment from 'moment';
import * as React from 'react';

import LeaseStatusSummary from './LeaseStatusSummary';
import StackedPidTenantFields from './StackedPidTenantFields';
import * as Styled from './styles';

interface ILeaseAndLicenseHeaderProps {
  lease?: ILease;
}

/**
 * Common header for lease and licenses. Requires an active lease to display.
 * @param {ILeaseAndLicenseHeaderProps} param0
 */
export const LeaseAndLicenseHeader: React.FunctionComponent<ILeaseAndLicenseHeaderProps> = ({
  lease,
}) => {
  const { getCodeById } = useLookupCodeHelpers();
  const paymentReceivableType = lease?.paymentReceivableTypeId
    ? getCodeById(PAYMENT_RECEIVABLE_CODE_SET_NAME, lease?.paymentReceivableTypeId)
    : '';
  return (
    <Styled.LeaseHeader className="lease-header">
      <Styled.LeaseH3>Lease / License</Styled.LeaseH3>
      <LeaseStatusSummary lease={lease} />
      <Styled.LeaseHeaderText>
        <label>Expiry:</label>
        <b>{lease?.expiryDate ? moment(lease?.expiryDate).format('MMM DD, YYYY') : ''}</b>
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

export default LeaseAndLicenseHeader;
