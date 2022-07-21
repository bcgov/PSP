import { useCallback, useState } from 'react';

/**
 * Hook that abstracts the state management for opening and closing modals
 */
export function useModalManagement(initialOpen = false) {
  const [isModalOpened, setModalVisibility] = useState(initialOpen);

  const openModal = useCallback(() => setModalVisibility(true), []);
  const closeModal = useCallback(() => setModalVisibility(false), []);

  return [isModalOpened, openModal, closeModal] as const;
}
