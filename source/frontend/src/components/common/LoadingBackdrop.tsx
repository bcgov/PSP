import * as React from 'react';
import Spinner from 'react-bootstrap/Spinner';

import { Backdrop } from '@/components/common/styles';

export interface LoadingBackdropProps {
  show?: boolean;
  parentScreen?: boolean;
}

const LoadingBackdrop: React.FC<React.PropsWithChildren<LoadingBackdropProps>> = ({
  show,
  parentScreen,
}) => {
  return show ? (
    <Backdrop parentScreen={parentScreen}>
      <Spinner animation="border" variant="warning" data-testid="filter-backdrop-loading" />
    </Backdrop>
  ) : null;
};

export default LoadingBackdrop;
