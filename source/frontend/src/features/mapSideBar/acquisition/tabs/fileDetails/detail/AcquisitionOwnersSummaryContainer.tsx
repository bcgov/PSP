import { useCallback, useEffect, useState } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';

export interface IAcquisitionOwnersContainerProps {
  acquisitionFileId: number;
  View: React.FC<IAcquisitionOwnersSummaryViewProps>;
}

export interface IAcquisitionOwnersSummaryViewProps {
  ownersList?: ApiGen_Concepts_AcquisitionFileOwner[];
  isLoading: boolean;
}

const AcquisitionOwnersSummaryContainer: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionOwnersContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [ownersDetails, setOwnersDetails] = useState<
    ApiGen_Concepts_AcquisitionFileOwner[] | undefined
  >(undefined);

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
