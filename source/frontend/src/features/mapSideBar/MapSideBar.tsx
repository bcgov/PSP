import { useContext } from 'react';
import styled from 'styled-components';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Backdrop } from '@/components/common/styles';

import { SideBarContext } from './context/sidebarContext';
import MapRouter from './router/MapRouter';

interface IMapSideBarProps {}

const MapSideBar: React.FunctionComponent<React.PropsWithChildren<IMapSideBarProps>> = () => {
  const { fullWidth } = useContext(SideBarContext);

  const mapMachine = useMapStateMachine();

  return (
    <StyledMapSideBar show={mapMachine.isSidebarOpen} fullWidth={fullWidth}>
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

const StyledMapSideBar = styled.div<{ show: boolean; fullWidth: boolean }>`
  display: flex;
  position: relative;
  flex-flow: column;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  min-width: ${props => (props.fullWidth ? `100%` : `93rem`)};
  margin-left: ${props => (props.show ? `0rem` : `-93rem`)};
  padding: 1.4rem 1.6rem;
  padding-bottom: 2rem;
  overflow: hidden;
  transition: 1s;
  width: ${props => (props.show ? `100%` : `0`)};
`;
