import axios from 'axios';
import { useCallback } from 'react';

import { useTenant } from '@/tenants';

import { useModalContext } from './useModalContext';

export const useRefreshSiteminder = () => {
  const { parcelMapFullyAttributed } = useTenant();
  const { setModalContent, setDisplayModal } = useModalContext();

  const refresh = useCallback(async () => {
    const response = await axios.get(
      parcelMapFullyAttributed.url +
        `?outputFormat=application%2Fjson&request=GetFeature&maxFeatures=0&typeName=${parcelMapFullyAttributed.layers}&service=WFS&version=1.0.0`,
      { withCredentials: true },
    );
    if (response.status !== 200) {
      setModalContent({
        title: 'Session Expired',
        message:
          'Your SITEMINDER session has expired. Please save any in-progress work and log out of the PIMS application.',
        okButtonText: 'OK',
        variant: 'warning',
      });
      setDisplayModal(true);
      console.error('Unable to refresh Siteminder cookie');
    }
  }, [parcelMapFullyAttributed, setModalContent, setDisplayModal]);

  return refresh;
};
