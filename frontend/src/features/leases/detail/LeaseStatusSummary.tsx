import { ILease } from 'interfaces';
import moment from 'moment';
import * as React from 'react';
import styled from 'styled-components';

export interface ILeaseStatusSummaryProps {
  lease?: ILease;
}

/**
 * Lease status component, displays file number and lease active status.
 * @param {ILeaseStatusSummaryProps} param0
 */
export const LeaseStatusSummary: React.FunctionComponent<ILeaseStatusSummaryProps> = ({
  lease,
}) => {
  const isActive = moment().isSameOrBefore(moment(lease?.expiryDate), 'day');
  return (
    <StyledLeaseStatusSummary className={isActive ? 'active' : 'inactive'}>
      <b>{isActive ? 'Active' : 'Inactive'}</b>
      <b>{lease?.lFileNo ?? ''}</b>
    </StyledLeaseStatusSummary>
  );
};

const StyledLeaseStatusSummary = styled.div`
  max-width: 11rem;
  max-height: 100%;
  white-space: nowrap;
  border-radius: 1rem;
  background-color: white;
  display: flex;
  flex-direction: column;
  border: 1px solid ${props => props.theme.css.accentColor};
  padding: 0.2rem 1rem;
  b {
    padding: 0.25rem 0;
    color: black;
    &:first-child {
      color: ${props => props.theme.css.accentColor};
    }
  }
  &.active {
    border: 1px solid ${props => props.theme.css.completedColor};
    b:first-child {
      color: ${props => props.theme.css.completedColor};
    }
  }
`;

export default LeaseStatusSummary;
