import styled from 'styled-components';

interface IMapSideBarProps {
  showWindow: boolean;
}

const MapActionWindow: React.FunctionComponent<IMapSideBarProps> = ({ showWindow, ...props }) => {
  return (
    <StyledMapWindow show={showWindow} data-testid="map-action-window">
      <>{props.children}</>
    </StyledMapWindow>
  );
};

export default MapActionWindow;

const StyledMapWindow = styled.div<{ show: boolean }>`
  flex-grow: 1;
  border-radius: 1rem;
  border: solid 0.3rem lightgray;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  overflow: hidden;
  transition: 1s;
`;
