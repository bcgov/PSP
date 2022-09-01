import styled from 'styled-components';

interface IMapSideBarProps {
  showSideBar: boolean;
}

const MapSideBar: React.FunctionComponent<IMapSideBarProps> = ({ showSideBar, ...props }) => {
  return (
    <StyledMapSideBar show={showSideBar}>
      <>{props.children}</>
    </StyledMapSideBar>
  );
};

export default MapSideBar;

const StyledMapSideBar = styled.div<{ show: boolean }>`
  display: flex;
  flex-flow: column;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  min-width: 93rem;
  margin-left: ${props => (props.show ? `0rem` : `-93rem`)};
  padding: 1.4rem 1.6rem;
  padding-bottom: 2rem;
  overflow: hidden;
  transition: 1s;
`;
