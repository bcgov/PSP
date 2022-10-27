import queryString from 'query-string';
import { useMemo } from 'react';
import { useLocation } from 'react-router-dom';

/**
 * A custom hook that builds on useLocation to parse the query string for you.
 * @returns an object with key = value pairs
 */
export const useQuery = () => {
  const { search } = useLocation();
  return useMemo(() => queryString.parse(search), [search]);
};
