import { useCallback, useState } from 'react';

interface ICounterProps {
  initial?: number; // default = 0
  min?: number; // default = undefined
  max?: number; // default = undefined
}

/**
 * React hook to manage a counter state.
 * @param props the hook properties
 * @returns
 */
export default function useCounter({ initial = 0, min, max }: ICounterProps) {
  // Increments the counter. Two scenarios are supported:
  //   1. if no "range" passed to this hook, it will increment the counter indefinitely
  //   2. if "range [min, max]" was provided, it will increment the counter until it reaches the maximum.
  const [count, setCount] = useState(initial);

  const increment = useCallback(() => {
    if (max === undefined) {
      setCount(x => x + 1);
    } else {
      setCount(x => (x < max ? x + 1 : x));
    }
  }, [max]);

  const decrement = useCallback(() => {
    if (min === undefined) {
      setCount(x => x - 1);
    } else {
      setCount(x => (x > min ? x - 1 : x));
    }
  }, [min]);

  const reset = useCallback(() => setCount(initial), [initial]);

  return { count, increment, decrement, reset };
}
