import { useEffect } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';

/**
 * Notifies the map state machine to enter/exit edit properties mode.
 * Use this hook in any component that needs to toggle edit properties mode on mount/unmount.
 */
export function useEditPropertiesMode() {
  const { setEditPropertiesMode, mapSideBarViewState } = useMapStateMachine();
  useEffect(() => {
    // required, otherwise the state machine will not be ready to accept this update.
    if (mapSideBarViewState.isOpen) {
      setEditPropertiesMode(true);
    }
    return () => {
      setEditPropertiesMode(false);
    };
  }, [setEditPropertiesMode, mapSideBarViewState]);
}
