import { useCallback, useEffect, useState } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { Api_AcquisitionFileOwner } from '@/models/api/AcquisitionFile';

export interface IAcquisitionOwnersContainerProps {
  acquisitionFileId: number;
  View: React.FC<IAcquisitionOwnersSummaryViewProps>;
}

export interface IAcquisitionOwnersSummaryViewProps {
  ownersList?: Api_AcquisitionFileOwner[];
  isLoading: boolean;
}

const AcquisitionOwnersSummaryContainer: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionOwnersContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [ownersDetails, setOwnersDetails] = useState<Api_AcquisitionFileOwner[] | undefined>(
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
        setOwnersDetails([...acquisitionOwners]);
      }
    }
  }, [acquisitionFileId, retrieveAcquisitionFileOwners]);

  useEffect(() => {
    fetchOwnersApi();
  }, [fetchOwnersApi]);

  return <View ownersList={ownersDetails} isLoading={loadingAcquisitionFileOwners} />;
};

export default AcquisitionOwnersSummaryContainer;
