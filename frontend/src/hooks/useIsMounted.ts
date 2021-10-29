import { useCallback, useEffect, useRef } from 'react';

/**
 * Hook to check if your component is still mounted.
 * Useful for avoiding errors that result from changing the state in an unmounted component.
 * @returns a "memoized" function that returns true if the component is mounted.
 */
function useIsMounted() {
  const isMounted = useRef(false);

  useEffect(() => {
    isMounted.current = true;
    return function cleanup() {
      isMounted.current = false;
    };
  }, []);

  return useCallback(() => isMounted.current, []);
}

export default useIsMounted;
