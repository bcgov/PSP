import { ILease } from 'interfaces';
import moment from 'moment';
import queryString from 'query-string';
import * as React from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { useLocation } from 'react-router-dom';
import { prettyFormatDate } from 'utils';

import LeaseHeaderAddresses from './LeaseHeaderAddresses';
import LeaseStatusSummary from './LeaseStatusSummary';
import StackedPidTenantFields from './StackedTenantFields';
import * as Styled from './styles';

interface ILeaseHeaderProps {
  lease?: ILease;
}

/**
 * Common header for lease and licenses. Requires an active lease to display.
 * @param {ILeaseHeaderProps} param0
 */
export const LeaseHeader: React.FunctionComponent<React.PropsWithChildren<ILeaseHeaderProps>> = ({
  lease,
}) => {
  const location = useLocation();
  const { edit } = queryString.parse(location.search);
  const isExpired = moment().isAfter(moment(lease?.expiryDate, 'YYYY-MM-DD'), 'day');
  return (
    <Styled.LeaseHeader className="lease-header">
      <Styled.LeaseH1>{edit ? 'Update' : ''} Lease / License</Styled.LeaseH1>
      <LeaseStatusSummary lease={lease} />
      <Styled.LeaseHeaderText>
        {isExpired && (
          <Styled.ExpiredWarning>
            <AiOutlineExclamationCircle size={16} />
            &nbsp; EXPIRED
          </Styled.ExpiredWarning>
        )}
        <label>Expiry:</label>
        <b>{prettyFormatDate(lease?.expiryDate)}</b>
      </Styled.LeaseHeaderText>
      <Styled.LeaseHeaderText>
        <b>{lease?.paymentReceivableType?.description}</b>
      </Styled.LeaseHeaderText>

      <Styled.LeaseHeaderRight>
        <LeaseHeaderAddresses lease={lease} />
        <StackedPidTenantFields lease={lease} />
      </Styled.LeaseHeaderRight>
    </Styled.LeaseHeader>
  );
};

export default LeaseHeader;
