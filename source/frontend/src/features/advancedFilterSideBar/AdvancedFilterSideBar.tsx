import { FaFilter } from 'react-icons/fa';
import styled from 'styled-components';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

const AdvancedFilterSideBar: React.FC = () => {
  const mapMachine = useMapStateMachine();

  return (
    <StyledMapSideBar show={mapMachine.isAdvancedFilterSidebarOpen}>
      <StyledHeader>
        <FaFilter size="1.6em" />
        <StyledTitle>Filter By:</StyledTitle>
      </StyledHeader>
    </StyledMapSideBar>
  );
};

export default AdvancedFilterSideBar;

const StyledMapSideBar = styled.div<{ show: boolean }>`
  display: flex;
  position: relative;
  flex-flow: column;
  background-color: #fff;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
  overflow: hidden;
  transition: 1s;
  max-width: ${props => (props.show ? `34.1rem` : `0`)};
  width: ${props => (props.show ? `100%` : `0`)};
`;

const StyledHeader = styled.div`
  width: 100%;
  height: 8rem;
  background-color: ${({ theme }) => theme.css.primaryColor};
  color: #fff;
  display: flex;
  flex-direction: row;
  align-items: center;
  padding: 1rem 1.8rem;
  gap: 1.5rem;
`;

const StyledTitle = styled.p`
  font-size: 1.8rem;
  color: #ffffff;
  text-decoration: none solid rgb(255, 255, 255);
  line-height: 1.8rem;
  font-weight: bold;
  margin: 0;
`;
