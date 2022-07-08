import { useCallback, useState } from 'react';

export function useModalManagement() {
  const [isModalOpened, setModalVisibility] = useState(false);

  const openModal = useCallback(() => setModalVisibility(true), []);
  const closeModal = useCallback(() => setModalVisibility(false), []);

  return {
    isModalOpened,
    openModal,
    closeModal,
  };
}
