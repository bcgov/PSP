import styled from 'styled-components';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';

export interface ILeaseStatusSummaryProps {
  lease?: ApiGen_Concepts_Lease;
}

/**
 * Lease status component, displays file number and lease active status.
 * @param {ILeaseStatusSummaryProps} param0
 */
export const LeaseStatusSummary: React.FunctionComponent<
  React.PropsWithChildren<ILeaseStatusSummaryProps>
> = ({ lease }) => {
  return lease ? (
    <StyledLeaseStatusSummary className={lease?.fileStatusTypeCode?.id?.toLowerCase()}>
      <b>{lease?.fileStatusTypeCode?.description}</b>
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
  border: 1px solid ${props => props.theme.bcTokens.themeGold100};
  padding: 0.2rem 1rem;
  height: 90%;
  b {
    padding: 0.25rem 0;
    color: black;
    &:first-child {
      color: ${props => props.theme.bcTokens.themeGold100};
    }
  }
  &.active {
    border: 1px solid ${props => props.theme.bcTokens.iconsColorSuccess};
    b:first-child {
      color: ${props => props.theme.bcTokens.iconsColorSuccess};
    }
  }
  &.terminated {
    border: 1px solid ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
    b:first-child {
      color: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
    }
  }
  &.discard {
    border: 1px solid ${props => props.theme.css.activeActionColor};
    b:first-child {
      color: ${props => props.theme.css.activeActionColor};
    }
  }
  &.draft {
    border: 1px solid ${props => props.theme.css.activeActionColor};
    b:first-child {
      color: ${props => props.theme.css.activeActionColor};
    }
  }
`;

export default LeaseStatusSummary;
