import React, { useEffect, useState } from 'react';

import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';
import { exists, isValidId } from '@/utils';

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
    ExpropriationEventFormModel.createEmpty(acquisitionFileId),
  );

  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    getExpropriationEvents: {
      execute: getExpropriationEvents,
      loading,
      response: expropriationEventsResponse,
    },
    addExpropriationEvent: { execute: addExpropriationEvent },
    updateExpropriationEvent: { execute: updateExpropriationEvent },
    deleteExpropriationEvent: { execute: deleteExpropriationEvent },
  } = useExpropriationEventRepository();

  const expropriationEvents: ApiGen_Concepts_ExpropriationEvent[] =
    expropriationEventsResponse ?? [];

  useEffect(() => {
    getExpropriationEvents(acquisitionFileId);
  }, [acquisitionFileId, getExpropriationEvents]);

  // Launch the expropriation modal in response to the user clicking the "Add" button
  const onAddExpropriation = () => {
    setEditExpropiationEventValue(ExpropriationEventFormModel.createEmpty(acquisitionFileId));
    setShowExpropriationEditModal(true);
  };

  // Launch the expropriation modal in response to the user clicking the "Update" icon-button on a given row in the history table
  const onUpdateExpropriation = (id: number) => {
    const event = expropriationEvents.find((x: ApiGen_Concepts_ExpropriationEvent) => x.id === id);
    setEditExpropiationEventValue(ExpropriationEventFormModel.fromApi(event));
    setShowExpropriationEditModal(true);
  };

  // Called when the SAVE button is clicked on the expropriation event modal.
  // Sends the save request (either an update or an add). Uses the response to update the parent lease.
  const onSaveExpropriationEvent = async (values: ExpropriationEventFormModel) => {
    if (exists(values)) {
      const updatedEvent = isValidId(values.id)
        ? await updateExpropriationEvent(acquisitionFileId, values.toApi())
        : await addExpropriationEvent(acquisitionFileId, values.toApi());

      if (isValidId(updatedEvent?.id)) {
        setEditExpropiationEventValue(ExpropriationEventFormModel.createEmpty(acquisitionFileId));
        setShowExpropriationEditModal(false);
        await getExpropriationEvents(acquisitionFileId);
      }
    } else {
      console.error();
    }
  };

  const onDeleteExpropriation = async (id: number) => {
    const event = expropriationEvents.find((x: ApiGen_Concepts_ExpropriationEvent) => x.id === id);

    // show confirmation popup before actually removing the Expropriation Event
    setModalContent({
      ...getDeleteModalProps(),
      variant: 'error',
      title: 'Delete Expropriation Event',
      message: `You have selected to delete this Exproprpriation Event from the history.
                                            Do you want to proceed?`,
      okButtonText: 'Yes',
      cancelButtonText: 'No',
      handleOk: async () => {
        if (isValidId(event.id)) {
          const result = await deleteExpropriationEvent(acquisitionFileId, event.id);
          if (!result) {
            console.error('Unable to delete expropriation event');
          }
        } else {
          console.error('Invalid expropriation event');
        }
        setDisplayModal(false);
      },
      handleCancel: () => {
        setDisplayModal(false);
      },
    });

    setDisplayModal(true);
  };

  return (
    <>
      <View
        isLoading={loading}
        expropriationEvents={expropriationEvents}
        onAdd={onAddExpropriation}
        onUpdate={onUpdateExpropriation}
        onDelete={onDeleteExpropriation}
      />
      <ModalView
        acquisitionFileId={acquisitionFileId}
        display={showExpropriationEditModal}
        initialValues={editExpropiationEventValue}
        onCancel={() => {
          setEditExpropiationEventValue(ExpropriationEventFormModel.createEmpty(acquisitionFileId));
          setShowExpropriationEditModal(false);
        }}
        onSave={onSaveExpropriationEvent}
      />
    </>
  );
};

export default ExpropriationEventHistoryContainer;
