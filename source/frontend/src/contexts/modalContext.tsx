import * as React from 'react';
import { useState } from 'react';

import { ModalContent, ModalProps } from '@/components/common/GenericModal';

export interface IModalContext {
  modalProps?: ModalProps;
  setModalContent: (modalProps?: ModalContent) => void;
  setDisplayModal: (display: boolean) => void;
}

export const ModalContext = React.createContext<IModalContext>({
  modalProps: undefined,
  setDisplayModal: (display: boolean) => {
    throw Error('setDisplayModal function not defined');
  },
  setModalContent: (modalProps?: ModalContent) => {
    throw Error('setModalProps function not defined');
  },
});

export const ModalContextProvider = (props: {
  children: React.ReactChild | React.ReactChild[] | React.ReactNode;
}) => {
  const [modalProps, setModalProps] = useState<ModalProps | undefined>(undefined);
  const [showModal, setShowModal] = useState<boolean>(false);

  const updateFunction = React.useCallback(
    (updatedModalContent?: ModalContent) => {
      setModalProps({
        ...updatedModalContent,
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
