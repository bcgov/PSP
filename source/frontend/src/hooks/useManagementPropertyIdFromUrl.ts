import { useEffect, useState } from 'react';
import { useLocation } from 'react-router';
import { matchPath } from 'react-router-dom';

import { useManagementFileRepository } from '@/hooks/repositories/useManagementFileRepository';
import { isValidId } from '@/utils';

/**
 * Hook to extract the propertyId from the URL pattern:
 * /mapview/sidebar/management/:managementFileId/property/:propertyFileId
 * It queries the management file and finds the propertyId for the given propertyFileId.
 */
export function useManagementPropertyIdFromUrl() {
  const location = useLocation();
  const {
    getManagementProperties: { execute: getManagementFileProperties, loading },
  } = useManagementFileRepository();
  const [propertyId, setPropertyId] = useState<number | undefined>(undefined);

  // Match the URL pattern and extract params
  const match = matchPath<{ managementFileId: string; propertyFileId: string }>(location.pathname, {
    path: '*/management/:managementFileId/property/:propertyFileId/*',
    exact: true,
    strict: false,
  });
  const managementFileId = match?.params?.managementFileId;
  const propertyFileId = match?.params?.propertyFileId;

  useEffect(() => {
    const fetchPropertyId = async () => {
      if (isValidId(Number(managementFileId)) && isValidId(Number(propertyFileId))) {
        const response = await getManagementFileProperties(Number(managementFileId));
        const properties = response ?? [];
        const matchingFileProperty = properties.find(p => String(p.id) === propertyFileId);
        setPropertyId(matchingFileProperty?.property?.id);
      } else {
        setPropertyId(undefined);
      }
    };
    fetchPropertyId();
    // Only re-run if the URL changes
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [managementFileId, propertyFileId, getManagementFileProperties]);

  return { propertyId, loading };
}
