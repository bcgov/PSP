import { useEffect, useRef } from 'react';

import { RootState, useAppSelector } from '../store/store';

/**
 * Hook that wraps useSelector, fires only when the selector is updated. Does not fire on initial load.
 * Intended to be used to monitor the value of a given part of the store.
 * @param selector the section of the store to monitor for updates.
 */
export function useCallbackAppSelector<TState = RootState, TSelected = unknown>(
  selector: (state: TState) => TSelected,
  callbackFn?: (updatedValue: TSelected) => void,
) {
  const selected: TSelected = useAppSelector(selector as any);
  const isInitialMount = useRef(true);

  useEffect(() => {
    if (isInitialMount.current) {
      isInitialMount.current = false;
    } else {
      callbackFn && callbackFn(selected);
    }
    // TODO: currently useApi is causing this to refresh when the jwt token expires, that hook needs to be re-written
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selected]);
}
