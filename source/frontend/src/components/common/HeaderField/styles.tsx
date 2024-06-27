import { Row } from 'react-bootstrap';
import styled from 'styled-components';

export const StyledRow = styled(Row)`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

export const StyledFiller = styled.div`
  height: 100%;
  display: flex;
  flex-direction: column;
`;
