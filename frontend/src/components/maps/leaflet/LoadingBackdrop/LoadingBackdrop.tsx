import * as React from 'react';
import Spinner from 'react-bootstrap/Spinner';
import styled from 'styled-components';

export interface LoadingBackdropProps {
  show?: boolean;
  parentScreen?: boolean;
}

const Backdrop = styled.div<LoadingBackdropProps>`
  width: 100%;
  height: 100%;
  position: ${(props: any) => (props.parentScreen ? 'absolute' : 'fixed')};
  z-index: 999;
  top: 0;
  left: 0;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  align-items: center;
  align-content: center;
  justify-items: center;
  justify-content: center;
`;

const LoadingBackdrop: React.FC<LoadingBackdropProps> = ({ show, parentScreen }) => {
  return show ? (
    <Backdrop parentScreen={parentScreen}>
      <Spinner animation="border" variant="warning" data-testid="filter-backdrop-loading" />
    </Backdrop>
  ) : null;
};

export default LoadingBackdrop;
