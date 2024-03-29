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

export const HalfWidthLayout = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
  height: 100%;
  width: 50%;
  min-width: 93rem;
  overflow: hidden;
  padding: 1.4rem 1.6rem;
  padding-bottom: 0;
`;

export const ScrollingFormLayout = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  overflow: auto;
  padding-right: 1rem;
`;
