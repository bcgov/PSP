import { useModalContext } from 'hooks/useModalContext';
import * as React from 'react';

import GenericModal from './GenericModal';

interface IModalContainerProps {}

export const ModalContainer: React.FunctionComponent<IModalContainerProps> = props => {
  const { modalProps } = useModalContext();
  return <GenericModal {...modalProps} />;
};

export default ModalContainer;
