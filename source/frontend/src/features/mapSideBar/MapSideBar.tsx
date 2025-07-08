import { FaExclamationCircle } from 'react-icons/fa';
import styled from 'styled-components';

import { SideBarType } from '@/components/common/mapFSM/machineDefinition/types';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Backdrop } from '@/components/common/styles';
import { exists } from '@/utils';

import MapRouter from './router/MapRouter';

export interface IMapSideBarViewState {
  isFullWidth?: boolean;
  isCollapsed?: boolean;
  isOpen?: boolean;
  type: SideBarType;
}

const MapSideBar: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const mapMachine = useMapStateMachine();

  return (
    <StyledMapSideBar sideBarState={mapMachine.mapSideBarViewState}>
      {mapMachine.isSelecting && (
        <StyledBackdrop parentScreen onClick={() => mapMachine.finishSelection()}>
          <StyledSelectingText
            style={{ color: 'white', fontFamily: 'BCSans-Bold', fontSize: '2.5rem' }}
          >
            Selecting Properties...
            <br /> Click here to exit property selection.
          </StyledSelectingText>
        </StyledBackdrop>
      )}
      {mapMachine.isRepositioning && (
        <StyledBackdrop parentScreen onClick={() => mapMachine.finishReposition()}>
          <StyledSelectingText
            style={{ color: 'white', fontFamily: 'BCSans-Bold', fontSize: '2.5rem' }}
          >
            <p>Relocating property marker...</p>
            <br />
            <p>
              <FaExclamationCircle size={56} />
            </p>
            <p>
              Click on the new location within the property
              <br />
              boundary to move the marker.
            </p>
            <br />
            Click here to exit property selection.
          </StyledSelectingText>
        </StyledBackdrop>
      )}
      <MapRouter />
    </StyledMapSideBar>
  );
};

export default MapSideBar;

export const StyledBackdrop = styled(Backdrop)`
  background-color: rgba(0, 0, 0, 0.7);
`;

const StyledSelectingText = styled.p`
  color: white;
  font-family: BCSans-Bold;
  font-size: 2.5rem;
`;

const StyledMapSideBar = styled.div<{ sideBarState: IMapSideBarViewState }>`
  display: flex;
  position: relative;
  flex-flow: column;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  overflow: hidden;
  transition: 1s;
  ${({ sideBarState }) => {
    if (exists(sideBarState) && sideBarState.isOpen) {
      if (sideBarState.isCollapsed) {
        return `
          min-width: 7.4rem;
          max-width: 7.4rem;
          margin-left: 0rem;
          width: 100%;
        `;
      } else if (sideBarState.isFullWidth) {
        return `
          min-width: 100%;
          max-width: 100%;
          margin-left: 0rem;
        `;
      } else {
        return `
          min-width: 93rem;
          max-width: 93rem;
          @media only screen and (max-width: 1199px) {
            min-width: 70rem;
          }
          margin-left: 0rem;
        `;
      }
    } else {
      return `
          min-width: 0rem;
          max-width: 0rem;
          width: 0rem;
          padding: 0;
        `;
    }
  }}
`;
