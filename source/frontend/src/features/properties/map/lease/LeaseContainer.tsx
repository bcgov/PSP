import { ReactComponent as Fence } from 'assets/images/fence.svg';
import { useLeaseDetail } from 'features/leases';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import React, { useCallback } from 'react';
import styled from 'styled-components';

import LeaseHeader from './common/LeaseHeader';
export interface ILeaseContainerProps {
  leaseId: number;
  onClose?: () => void;
}

export const LeaseContainer: React.FC<ILeaseContainerProps> = ({ leaseId, onClose }) => {
  const close = useCallback(() => onClose && onClose(), [onClose]);
  const { lease } = useLeaseDetail(leaseId);

  return (
    <MapSideBarLayout
      showCloseButton
      onClose={close}
      title="Lease / License"
      icon={
        <Fence
          title="Lease file icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
        />
      }
      header={<LeaseHeader lease={lease} />}
    >
      <StyledFormWrapper></StyledFormWrapper>
    </MapSideBarLayout>
  );
};

export default LeaseContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
