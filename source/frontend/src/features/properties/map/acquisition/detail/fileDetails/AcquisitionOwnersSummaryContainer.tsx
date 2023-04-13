import { useCallback, useEffect, useState } from 'react';

import { useAcquisitionProvider } from '../../hooks/useAcquisitionProvider';
import { DetailAcquisitionFileOwner } from '../models';

export interface IAcquisitionOwnersContainerProps {
  acquisitionFileId: number;
  View: React.FC<IAcquisitionOwnersSummaryViewProps>;
}

export interface IAcquisitionOwnersSummaryViewProps {
  ownersList?: DetailAcquisitionFileOwner[];
  isLoading: boolean;
}

const AcquisitionOwnersSummaryContainer: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionOwnersContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [ownersDetails, setOwnersDetails] = useState<DetailAcquisitionFileOwner[] | undefined>(
    undefined,
  );

  const {
    getAcquisitionOwners: {
      execute: retrieveAcquisitionFileOwners,
      loading: loadingAcquisitionFileOwners,
    },
  } = useAcquisitionProvider();

  const fetchOwnersApi = useCallback(async () => {
    if (acquisitionFileId) {
      const acquisitionOwners = await retrieveAcquisitionFileOwners(acquisitionFileId);
      if (acquisitionOwners !== undefined) {
        const ownerDetailList = acquisitionOwners.map(o => DetailAcquisitionFileOwner.fromApi(o));
        setOwnersDetails([...ownerDetailList]);
      }
    }
  }, [acquisitionFileId, retrieveAcquisitionFileOwners]);

  useEffect(() => {
    fetchOwnersApi();
  }, [fetchOwnersApi]);

  return <View ownersList={ownersDetails} isLoading={loadingAcquisitionFileOwners} />;
};

export default AcquisitionOwnersSummaryContainer;
