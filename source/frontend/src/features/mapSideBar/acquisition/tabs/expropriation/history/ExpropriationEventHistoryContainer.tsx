import React, { useEffect, useState } from 'react';

import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';

import { IExpropriationEventHistoryViewProps } from './ExpropriationEventHistoryView';
import { IExpropriationEventModalProps } from './modal/ExpropriationEventModal';
import { ExpropriationEventFormModel } from './models';

interface IExpropriationEventHistoryContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<IExpropriationEventHistoryViewProps>;
  ModalView: React.FunctionComponent<IExpropriationEventModalProps>;
}

export const ExpropriationEventHistoryContainer: React.FunctionComponent<
  IExpropriationEventHistoryContainerProps
> = ({ acquisitionFileId, View, ModalView }) => {
  const [showExpropriationEditModal, setShowExpropriationEditModal] = useState<boolean>(false);
  const [editExpropiationEventValue, setEditExpropiationEventValue] = useState(
    new ExpropriationEventFormModel(acquisitionFileId),
  );

  const {
    getExpropriationEvents: { execute: getExpropriationEvents, loading, response },
  } = useExpropriationEventRepository();

  useEffect(() => {
    getExpropriationEvents(acquisitionFileId);
  }, [acquisitionFileId, getExpropriationEvents]);

  const handleAdd = () => {
    // TODO:
  };

  const handleUpdate = (expropriationEventId: number) => {
    // TODO:
  };

  const handleDelete = (expropriationEventId: number) => {
    // TODO:
  };

  const onSaveExpropriationEvent = (values: ExpropriationEventFormModel) => {
    // TODO:
  };

  return (
    <>
      <View
        isLoading={loading}
        expropriationEvents={response ?? []}
        onAdd={handleAdd}
        onUpdate={handleUpdate}
        onDelete={handleDelete}
      />
      <ModalView
        acquisitionFileId={acquisitionFileId}
        display={showExpropriationEditModal}
        initialValues={editExpropiationEventValue}
        onCancel={() => {
          setEditExpropiationEventValue(new ExpropriationEventFormModel(acquisitionFileId));
        }}
        onSave={onSaveExpropriationEvent}
      />
    </>
  );
};

export default ExpropriationEventHistoryContainer;
