import { ModalProps } from 'components/common/GenericModal';
import noop from 'lodash/noop';
import * as React from 'react';
import { useState } from 'react';

export interface IModalContext {
  modalProps?: ModalProps;
  setModalProps: (modalProps?: ModalProps) => void;
  setDisplayModal: (display: boolean) => void;
}

export const ModalContext = React.createContext<IModalContext>({
  modalProps: undefined,
  setModalProps: noop,
  setDisplayModal: noop,
});

export const ModalContextProvider = (props: IModalContext & { children: any }) => {
  const [modalProps, setModalProps] = useState<ModalProps | undefined>(undefined);
  const displayFunction = React.useCallback(
    (displayModal: boolean) => {
      setModalProps({ ...modalProps, display: displayModal });
    },
    [modalProps],
  );
  const updateFunction = React.useCallback(
    (updatedModalProps?: ModalProps) =>
      setModalProps({
        ...modalProps,
        ...updatedModalProps,
        display: updatedModalProps?.display ?? modalProps?.display ?? false,
        setDisplay: displayFunction,
      }),
    [displayFunction, modalProps],
  );

  return (
    <ModalContext.Provider
      value={{
        // if user info is not available when authenticated, then the auth state is not ready
        modalProps: {
          ...modalProps,
          display: modalProps?.display ?? false,
          setDisplay: displayFunction,
        },
        setModalProps: updateFunction,
        setDisplayModal: displayFunction,
      }}
    >
      {props.children}
    </ModalContext.Provider>
  );
};
