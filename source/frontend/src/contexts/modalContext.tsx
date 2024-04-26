import { createContext, ReactChild, ReactNode, useCallback, useState } from 'react';

import { ModalContent, ModalProps } from '@/components/common/GenericModal';

export interface IModalContext {
  modalProps: ModalProps;
  setModalContent: (modalProps?: ModalContent) => void;
  setDisplayModal: (display: boolean) => void;
}

export const ModalContext = createContext<IModalContext>({
  modalProps: { variant: 'info' },
  setDisplayModal: () => {
    throw Error('setDisplayModal function not defined');
  },
  setModalContent: () => {
    throw Error('setModalProps function not defined');
  },
});

export const ModalContextProvider = (props: {
  children: ReactChild | ReactChild[] | ReactNode;
}) => {
  const [modalProps, setModalProps] = useState<ModalProps>({ variant: 'info' });
  const [showModal, setShowModal] = useState<boolean>(false);

  const updateFunction = useCallback(
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
