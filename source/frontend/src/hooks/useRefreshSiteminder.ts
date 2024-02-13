import axios from 'axios';
import { useCallback } from 'react';

import { useTenant } from '@/tenants';

export const useRefreshSiteminder = () => {
  const { parcelsLayerUrl } = useTenant();

  const refresh = useCallback(() => {
    axios.get(
      parcelsLayerUrl +
        '?outputFormat=application%2Fjson&request=GetFeature&maxFeatures=0&typeName=geo.allgov%3AWHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW&service=WFS&version=1.0.0',
    );
  }, [parcelsLayerUrl]);

  return refresh;
};
