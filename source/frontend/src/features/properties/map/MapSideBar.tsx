import { Api_Property } from 'models/api/Property';
import { useContext } from 'react';
import styled from 'styled-components';

import { SideBarContext } from './context/sidebarContext';
import MapRouter from './MapRouter';

interface IMapSideBarProps {
  showSideBar: boolean;
  setShowSideBar: (showSideBar: boolean) => void;
  onZoom?: (apiProperty?: Api_Property) => void;
}

const MapSideBar: React.FunctionComponent<React.PropsWithChildren<IMapSideBarProps>> = ({
  showSideBar,
  setShowSideBar,
  onZoom,
}) => {
  const { fullWidth } = useContext(SideBarContext);
  return (
    <StyledMapSideBar show={showSideBar} fullWidth={fullWidth}>
      <MapRouter showSideBar={showSideBar} setShowSideBar={setShowSideBar} onZoom={onZoom} />
    </StyledMapSideBar>
  );
};

export default MapSideBar;

const StyledMapSideBar = styled.div<{ show: boolean; fullWidth: boolean }>`
  display: flex;
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
