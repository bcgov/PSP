import * as React from 'react';
import styled from 'styled-components';

import { Api_Lease } from '@/models/api/Lease';

export interface ILeaseStatusSummaryProps {
  lease?: Api_Lease;
}

/**
 * Lease status component, displays file number and lease active status.
 * @param {ILeaseStatusSummaryProps} param0
 */
export const LeaseStatusSummary: React.FunctionComponent<
  React.PropsWithChildren<ILeaseStatusSummaryProps>
> = ({ lease }) => {
  return !!lease ? (
    <StyledLeaseStatusSummary className={lease?.statusType?.id?.toLowerCase()}>
      <b>{lease?.statusType?.description}</b>
      <b>{lease?.lFileNo ?? ''}</b>
    </StyledLeaseStatusSummary>
  ) : (
    <StyledLeaseStatusSummary className="draft">
      <b>DRAFT</b>
      <b></b>
    </StyledLeaseStatusSummary>
  );
};

const StyledLeaseStatusSummary = styled.div`
  max-width: fit-content;
  max-height: 8rem;
  min-width: 11rem;
  white-space: nowrap;
  border-radius: 1rem;
  background-color: white;
  display: flex;
  flex-direction: column;
  border: 1px solid ${props => props.theme.css.accentColor};
  padding: 0.2rem 1rem;
  height: 90%;
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
  &.terminated {
    border: 1px solid ${props => props.theme.css.dangerColor};
    b:first-child {
      color: ${props => props.theme.css.dangerColor};
    }
  }
  &.discard {
    border: 1px solid ${props => props.theme.css.discardedColor};
    b:first-child {
      color: ${props => props.theme.css.discardedColor};
    }
  }
  &.draft {
    border: 1px solid ${props => props.theme.css.draftColor};
    b:first-child {
      color: ${props => props.theme.css.draftColor};
    }
  }
`;

export default LeaseStatusSummary;
