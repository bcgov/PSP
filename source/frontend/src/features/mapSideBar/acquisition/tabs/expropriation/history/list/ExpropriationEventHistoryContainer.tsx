import React, { useEffect } from 'react';

import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';

import { IExpropriationEventHistoryViewProps } from './ExpropriationEventHistoryView';

interface IExpropriationEventHistoryContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<IExpropriationEventHistoryViewProps>;
}

export const ExpropriationEventHistoryContainer: React.FunctionComponent<
  IExpropriationEventHistoryContainerProps
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

  const handleAdd = () => {
    // TODO:
  };

  const handleUpdate = (expropriationEventId: number) => {
    // TODO:
  };

  const handleDelete = (expropriationEventId: number) => {
    // TODO:
  };

  return (
    <View
      isLoading={loading}
      expropriationEvents={response ?? []}
      onAdd={handleAdd}
      onUpdate={handleUpdate}
      onDelete={handleDelete}
    />
  );
};

export default ExpropriationEventHistoryContainer;
