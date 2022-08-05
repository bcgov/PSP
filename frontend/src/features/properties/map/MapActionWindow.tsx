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
  flex-grow: 1;
  border-radius: 1rem;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  overflow: hidden;
  transition: 1s;
  padding: 0.5rem;
`;
