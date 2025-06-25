import { useMemo } from 'react';
import { matchPath, useLocation } from 'react-router-dom';

import { exists } from '@/utils';

interface UsePropertyIndexOptions {
  /**
   * Route pattern with :menuIndex param. Default: '/property/:menuIndex/'
   * Example: '/leases/55/property/:menuIndex/'
   */
  routePattern?: string;
}

/**
 * Custom hook to extract the zero-based property index from the current URL path.
 * Assumes the URL contains a segment like `/property/:menuIndex/`, where `menuIndex` is 1-based.
 *
 * @param options Optional config to customize the route pattern.
 * @returns {number | null} The zero-based index of the property, or null if not matched.
 */
export function useFilePropertyIdFromUrl(options: UsePropertyIndexOptions = {}): number | null {
  const location = useLocation();

  const { routePattern = '*/property/:filePropertyId/' } = options;

  return useMemo(() => {
    const match = matchPath<Record<string, string>>(location.pathname, {
      path: routePattern,
      exact: false,
      strict: false,
    });

    if (exists(match?.params?.filePropertyId)) {
      const filePropertyId = Number(match.params.filePropertyId);
      return isNaN(filePropertyId) ? null : filePropertyId;
    }

    return null;
  }, [location.pathname, routePattern]);
}
