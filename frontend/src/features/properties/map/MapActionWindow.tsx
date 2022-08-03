import styled from 'styled-components';

interface IMapSideBarProps {
  showWindow: boolean;
}

const MapActionWindow: React.FunctionComponent<IMapSideBarProps> = ({ showWindow, ...props }) => {
  return (
    <StyledMapWindow show={showWindow} data-test="map-window">
      <>{props.children}</>
    </StyledMapWindow>
  );
};

export default MapActionWindow;

const StyledMapWindow = styled.div<{ show: boolean }>`
  display: block;
  flex-flow: column;
  height: 90vh;
  right: 0;
  top: 0;
  z-index: 10000;
  border-radius: 1rem;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  width: 93rem;
  min-width: 64rem;
  max-width: 64rem;
  margin-left: ${props => (props.show ? `0rem` : `-93rem`)};
  padding-bottom: 2rem;

  overflow: hidden;
  transition: 1s;
  position: absolute;
`;
