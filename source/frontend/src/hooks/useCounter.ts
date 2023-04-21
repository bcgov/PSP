import { useCallback, useEffect, useState } from 'react';

interface ICounterProps {
  initial?: number; // default = 0
  min?: number; // default = undefined
  max?: number; // default = undefined
}

/**
 * React hook to manage state for a counter.
 * It supports clamping the counter to a given range; e.g. [0..10]
 *
 * @param props the hook properties
 * @returns hook state and helper functions to increment, decrement and reset the counter
 */
export default function useCounter({ initial = 0, min, max }: ICounterProps) {
  // Increments/decrements the counter. Two scenarios are supported:
  //   1. if no "range" passed to this hook, it will increment/decrement the counter indefinitely
  //   2. if "range [min, max]" was provided, it will increment/decrement the counter until it reaches the maximum.
  const [count, setCount] = useState(initial);

  useEffect(() => {
    setCount(initial);
  }, [initial]);

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
