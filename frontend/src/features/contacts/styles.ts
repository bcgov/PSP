import { Row } from 'react-bootstrap';
import styled from 'styled-components';

export const H1 = styled.h1`
  text-align: left;
`;

export const H2 = styled.h2`
  text-align: left;
`;

export const H2Primary = styled.h2`
  text-align: left;
  color: ${props => props.theme.css.primaryColor};
`;

export const RowAligned = styled(Row)`
  text-align: left;
`;

export const StatusIndicators = styled.div`
  border-radius: 1rem;
  background-color: white;
  border: 1px solid ${props => props.theme.css.lightVariantColor};
  padding: 0.2rem 1rem;
  color: ${props => props.theme.css.lightVariantColor};
  &.active {
    color: ${props => props.theme.css.completedColor};
    border: 1px solid ${props => props.theme.css.completedColor};
  }
`;
