import { useMemo } from 'react';
import { matchPath, useHistory } from 'react-router-dom';

import { exists } from '@/utils';

/**
 * Custom hook to extract the zero-based property index from the current URL path.
 * Assumes the URL contains a segment like `/property/:menuIndex/`, where `menuIndex` is 1-based.
 *
 * @returns {number | null} The zero-based index of the property, or null if not matched.
 */
export function useFilePropertyIdFromUrl(): {
  fileId: number | null;
  filePropertyId: number | null;
} {
  const { location } = useHistory();

  return useMemo(() => {
    const match = matchPath<Record<string, string>>(location.pathname, {
      path: '/mapview/sidebar/:fileType/:fileId/property/:filePropertyId/',
      exact: false,
      strict: false,
    });

    if (exists(match?.params?.filePropertyId)) {
      const filePropertyId = Number(match.params.filePropertyId);
      const fileId = Number(match.params.fileId);
      return {
        fileId: isNaN(fileId) ? null : fileId,
        filePropertyId: isNaN(filePropertyId) ? null : filePropertyId,
      };
    }

    return { fileId: null, filePropertyId: null };
  }, [location.pathname]);
}
