import * as React from 'react';
import { useState } from 'react';

import { ModalContent, ModalProps } from '@/components/common/GenericModal';

export interface IModalContext {
  modalProps: ModalProps;
  setModalContent: (modalProps?: ModalContent) => void;
  setDisplayModal: (display: boolean) => void;
}

export const ModalContext = React.createContext<IModalContext>({
  modalProps: { variant: 'info' },
  setDisplayModal: () => {
    throw Error('setDisplayModal function not defined');
  },
  setModalContent: () => {
    throw Error('setModalProps function not defined');
  },
});

export const ModalContextProvider = (props: {
  children: React.ReactChild | React.ReactChild[] | React.ReactNode;
}) => {
  const [modalProps, setModalProps] = useState<ModalProps>({ variant: 'info' });
  const [showModal, setShowModal] = useState<boolean>(false);

  const updateFunction = React.useCallback(
    (updatedModalContent?: ModalContent) => {
      setModalProps({
        ...updatedModalContent,
        variant: updatedModalContent?.variant ?? 'info',
        display: showModal,
        setDisplay: setShowModal,
      });
    },
    [showModal, setShowModal],
  );

  return (
    <ModalContext.Provider
      value={{
        modalProps: {
          ...modalProps,
          display: showModal,
          setDisplay: setShowModal,
        },
        setModalContent: updateFunction,
        setDisplayModal: setShowModal,
      }}
    >
      {props.children}
    </ModalContext.Provider>
  );
};
