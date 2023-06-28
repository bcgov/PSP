import useEventListener from '@/hooks/useEventListener';

/**
 * Will execute the passed callback whenever the refreshMap event is fired. Provides a function to dispatch the refreshMap event. Automatically cleans up event listener.
 * @param callback
 */

const mapRefreshEventName = 'refreshMap';
//const mapRefreshEvent = new CustomEvent(mapRefreshEventName);
//const fireMapRefreshEvent = () => window.dispatchEvent(mapRefreshEvent);

export const useMapRefreshEvent = (callback: (event: Event) => void) => {
  const listener = useEventListener(mapRefreshEventName, callback);

  return { listener };
};
