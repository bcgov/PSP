import React from 'react';
import { dequal } from 'dequal';

/** util function used by other useDeep* hooks */
export function useDeepCompareMemoize(value: React.DependencyList) {
  const ref = React.useRef<React.DependencyList>([]);

  if (!dequal(value, ref.current)) {
    ref.current = value;
  }

  return ref.current;
}
