import Container from 'react-bootstrap/Container';
import styled from 'styled-components';

export const ListView = styled(Container)`
  background-color: #fff;
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  padding: 0;
`;

export const ScrollContainer = styled.div`
  padding: 1.6rem 3.2rem;
  flex-grow: 1; // because all parents are flex and have flex-grow set to 1 this takes all available space - calc no longer needed!
  overflow-y: auto;
`;

export const ScrollXYContainer = styled.div`
  padding: 1.6rem 3.2rem;
  flex-grow: 1; // because all parents are flex and have flex-grow set to 1 this takes all available space - calc no longer needed!
  overflow-y: auto;
  overflow-x: auto;
`;

export const WithShadow = styled(Container)`
  padding: 0;
  box-shadow: 0px 4px 5px rgba(0, 0, 0, 0.2);
  z-index: 500;
`;

export const TableContainer = styled(Container)`
  margin-top: 1rem;
  margin-bottom: 4rem;
`;

export const Ribbon = styled.div`
  text-align: right;
  margin-right: 5rem;
`;
