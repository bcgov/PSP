import { ReactComponent as Fence } from 'assets/images/fence.svg';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { useLeaseDetail } from 'features/leases';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import { FormikProps } from 'formik';
import React, { useCallback, useReducer, useRef } from 'react';
import styled from 'styled-components';

import LeaseHeader from './common/LeaseHeader';
import ViewSelector from './ViewSelector';

export interface ILeaseContainerProps {
  leaseId: number;
  onClose?: () => void;
}

// Interface for our internal state
export interface LeaseContainerState {
  isEditing: boolean;
}

const initialState: LeaseContainerState = {
  isEditing: false,
};

export const LeaseContainer: React.FC<ILeaseContainerProps> = ({ leaseId, onClose }) => {
  // keep track of our internal container state
  const [containerState, setContainerState] = useReducer(
    (prevState: LeaseContainerState, newState: Partial<LeaseContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    initialState,
  );

  const formikRef = useRef<FormikProps<any>>(null);
  const close = useCallback(() => onClose && onClose(), [onClose]);
  const { lease } = useLeaseDetail(leaseId);

  if (lease === undefined) {
    return <LoadingBackdrop show={true} parentScreen={true} />;
  }

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
      <StyledFormWrapper>
        <ViewSelector
          ref={formikRef}
          lease={lease}
          isEditing={containerState.isEditing}
          setContainerState={setContainerState}
        />
      </StyledFormWrapper>
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
