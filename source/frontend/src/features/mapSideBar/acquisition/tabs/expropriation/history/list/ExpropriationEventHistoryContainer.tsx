import React, { useEffect } from 'react';

import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';

import { IExpropriationEventHistoryViewProps } from './ExpropriationEventHistoryView';

interface IUpdateBctfaOwnershipContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<IExpropriationEventHistoryViewProps>;
}

export const ExpropriationEventHistoryContainer: React.FunctionComponent<
  IUpdateBctfaOwnershipContainerProps
> = ({ acquisitionFileId, View }) => {
  const {
    getAcquisitionExpropriationEvents: {
      execute: getAcquisitionExpropriationEvents,
      loading,
      response,
    },
  } = useExpropriationEventRepository();

  useEffect(() => {
    getAcquisitionExpropriationEvents(acquisitionFileId);
  }, [acquisitionFileId, getAcquisitionExpropriationEvents]);

  return <View isLoading={loading} expropriationEvents={response} />;
};

export default ExpropriationEventHistoryContainer;
