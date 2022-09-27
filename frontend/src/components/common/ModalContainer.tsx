import { ModalContext } from 'contexts/modalContext';
import * as React from 'react';
import { useContext } from 'react';

import GenericModal from './GenericModal';

interface IModalContainerProps {}

export const ModalContainer: React.FunctionComponent<IModalContainerProps> = props => {
  const { modalProps } = useContext(ModalContext);
  return <GenericModal {...modalProps} />;
};

export default ModalContainer;
