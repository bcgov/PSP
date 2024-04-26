import { useContext } from 'react';

import { ModalContext } from '@/contexts/modalContext';

import GenericModal from './GenericModal';

export const ModalContainer: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { modalProps } = useContext(ModalContext);
  return <GenericModal {...modalProps} />;
};

export default ModalContainer;
