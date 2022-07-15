import { useCallback, useState } from 'react';

/**
 * Hook that abstracts the state management for opening and closing modals
 */
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
